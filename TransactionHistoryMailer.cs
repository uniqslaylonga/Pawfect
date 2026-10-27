using System.Globalization;
using System.Net;
using System.Net.Sockets;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Data.SqlClient;
using MimeKit;

namespace MyApp;

/// <summary>
/// Customer side: emails a customer their own transaction history (receipt no., date, what they paid for, total, status).
/// It only ever reads this customer's records and only ever sends to the email on their own account, so nobody can have
/// somebody else's history sent anywhere. Uses the same smtp.config as the verification code email.
/// </summary>
internal static class TransactionHistoryMailer
{
    private const int MaxRows = 100;   // keeps the email a sensible size; the note at the bottom says when older ones were left off

    /// <summary>j.doe@gmail.com -> j******@gmail.com (first letter, stars, then the domain).</summary>
    public static string Mask(string email)
    {
        int at = email.IndexOf('@');
        if (at < 1) return email;
        return email[0] + new string('*', Math.Min(Math.Max(at - 1, 1), 6)) + email[at..];
    }

    /// <summary>Where the history goes: the email on the customer's record, else <paramref name="fallbackEmail"/> (login / verified kiosk email).</summary>
    public static string ResolveEmail(int customerId, string fallbackEmail)
    {
        string onFile = "";
        try { onFile = EmployeeData.GetCustomerContact(customerId).Email.Trim(); }
        catch (SqlException) { /* fall back to the login email */ }
        return onFile.Length > 0 ? onFile : fallbackEmail.Trim();
    }

    /// <summary>
    /// Loads the customer's transactions and emails them. Returns the masked address it went to and how many transactions
    /// were in it; when the customer has none, nothing is sent (Count = 0). Problems come back as an <see cref="OtpException"/>
    /// with a message the customer can read.
    /// </summary>
    public static async Task<(string Masked, int Count)> SendAsync(int customerId, string customerName, string fallbackEmail)
    {
        string email = ResolveEmail(customerId, fallbackEmail);
        if (email.Length == 0)
            throw new OtpException("There's no email address on your account yet. Please ask the front desk to add one.");

        var settings = EmailOtp.LoadSettings()
            ?? throw new OtpException("Email sending isn't set up yet. Ask the shop to add an smtp.config file next to Pawfect.exe.");

        List<(TransactionItem Sale, List<(string Name, string Detail, int Quantity, decimal UnitPrice)> Lines)> history;
        try
        {
            history = await Task.Run(() => EmployeeData.GetTransactionsForCustomer(customerId)
                .Select(t => (t, EmployeeData.GetTransactionLines(t.Id)))
                .ToList());
        }
        catch (SqlException ex) { throw new OtpException("We couldn't load your transactions.\r\n(" + ex.Message + ")", ex); }

        if (history.Count == 0) return (Mask(email), 0);

        MimeMessage message;
        try { message = Build(settings, email, customerName, history); }
        catch (ParseException ex) { throw new OtpException("The email address on your account isn't valid. Please ask the front desk to fix it.", ex); }

        var ports = settings.Port == 465 ? new[] { 465, 587 } : new[] { settings.Port, 465 };
        Exception? last = null;
        foreach (int port in ports.Distinct())
        {
            try
            {
                using var client = new SmtpClient { Timeout = 15_000 };
                await client.ConnectAsync(settings.Host, port,
                    port == 465 ? SecureSocketOptions.SslOnConnect : SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(settings.User, settings.Password);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
                message.Dispose();
                AuditLog.Record(null, "Email Sent", $"Transaction history -> {email} (customer #{customerId}, {history.Count} transactions)");
                return (Mask(email), history.Count);
            }
            catch (MailKit.Security.AuthenticationException ex)
            {
                throw new OtpException("The mail server refused the login - check the user and app password in smtp.config.\r\n(" + ex.Message + ")", ex);
            }
            catch (Exception ex) when (ex is SocketException or IOException or MailKit.ProtocolException or MailKit.CommandException
                                          or MailKit.ServiceNotConnectedException or MailKit.Security.SslHandshakeException
                                          or System.Security.Authentication.AuthenticationException
                                          or TimeoutException or OperationCanceledException or InvalidOperationException)
            {
                last = ex;   // try the other port
            }
        }
        throw new OtpException("We couldn't reach the mail server. Check your internet connection, or try again in a moment.\r\n(" + last?.Message + ")", last);
    }

    // ------------------------------------------------------------------ the email itself

    private static MimeMessage Build(EmailOtp.SmtpSettings s, string to, string name,
        List<(TransactionItem Sale, List<(string Name, string Detail, int Quantity, decimal UnitPrice)> Lines)> history)
    {
        var us = CultureInfo.GetCultureInfo("en-US");
        string first = name.Split(' ', StringSplitOptions.RemoveEmptyEntries).FirstOrDefault() ?? "there";
        string E(string v) => WebUtility.HtmlEncode(v);

        var shown = history.Take(MaxRows).ToList();
        decimal spent = history.Where(h => h.Sale.Status == TransactionStatus.Completed).Sum(h => h.Sale.Total);
        int completed = history.Count(h => h.Sale.Status == TransactionStatus.Completed);

        // plain-text version
        var text = new System.Text.StringBuilder();
        text.Append($"Hi {name},\r\n\r\nHere is your Pawfect transaction history.\r\n\r\n");
        foreach (var (sale, lines) in shown)
        {
            text.Append($"{sale.ReceiptNumber}  |  {sale.When.ToString("MMM d, yyyy h:mm tt", us)}  |  {sale.PaymentMethod}  |  {TransactionStatusStyle.Label(sale.Status)}\r\n");
            foreach (var l in lines)
                text.Append($"   {l.Quantity} x {l.Name}{(l.Detail.Length > 0 ? " (" + l.Detail + ")" : "")}  {Money.Format(l.UnitPrice * l.Quantity)}\r\n");
            text.Append($"   Total: {Money.Format(sale.Total)}\r\n\r\n");
        }
        if (history.Count > shown.Count) text.Append($"(Showing your latest {shown.Count} of {history.Count} transactions.)\r\n\r\n");
        text.Append($"Total paid ({completed} completed): {Money.Format(spent)}\r\n\r\n- Pawfect");

        // HTML version: one card per transaction
        var rows = new System.Text.StringBuilder();
        foreach (var (sale, lines) in shown)
        {
            var (fore, fill, _) = TransactionStatusStyle.Pill(sale.Status);
            string Hex(Color c) => $"#{c.R:X2}{c.G:X2}{c.B:X2}";
            var items = new System.Text.StringBuilder();
            foreach (var l in lines)
            {
                string detail = l.Detail.Length > 0 ? $"<br><span style=\"color:#7A889C;font-size:12px;\">{E(l.Detail)}</span>" : "";
                items.Append($"<tr><td style=\"padding:3px 0;color:#44536A;\">{l.Quantity} &times; {E(l.Name)}{detail}</td>" +
                             $"<td style=\"padding:3px 0;text-align:right;color:#1F2A3C;white-space:nowrap;vertical-align:top;\">{E(Money.Format(l.UnitPrice * l.Quantity))}</td></tr>");
            }
            rows.Append($@"
   <tr><td style=""padding:0 32px 14px 32px;"">
     <table role=""presentation"" width=""100%"" cellpadding=""0"" cellspacing=""0"" style=""background:#F6F8FB;border:1px solid #E3E9F2;border-radius:12px;font-size:14px;"">
      <tr><td style=""padding:12px 16px 4px 16px;"">
        <span style=""font-weight:700;color:#1F2A3C;"">{E(sale.ReceiptNumber)}</span>
        <span style=""display:inline-block;margin-left:8px;padding:1px 8px;border-radius:10px;font-size:11px;font-weight:600;color:{Hex(fore)};background:{Hex(fill)};"">{E(TransactionStatusStyle.Label(sale.Status))}</span>
        <div style=""font-size:12px;color:#7A889C;margin-top:2px;"">{E(sale.When.ToString("dddd, MMM d, yyyy 'at' h:mm tt", us))} &middot; {E(sale.PaymentMethod)}</div>
      </td></tr>
      <tr><td style=""padding:4px 16px 0 16px;""><table role=""presentation"" width=""100%"" cellpadding=""0"" cellspacing=""0"" style=""font-size:13px;"">{items}</table></td></tr>
      <tr><td style=""padding:8px 16px 12px 16px;text-align:right;font-weight:700;color:#1F2A3C;"">Total {E(Money.Format(sale.Total))}</td></tr>
     </table>
   </td></tr>");
        }

        string more = history.Count > shown.Count
            ? $"<p style=\"margin:0 0 8px 0;font-size:12px;color:#7A889C;\">Showing your latest {shown.Count} of {history.Count} transactions.</p>" : "";

        string html = $@"<!DOCTYPE html>
<html><body style=""margin:0;padding:0;background:#F3F7FC;font-family:'Segoe UI',Arial,sans-serif;"">
<table role=""presentation"" width=""100%"" cellpadding=""0"" cellspacing=""0"" style=""background:#F3F7FC;padding:32px 12px;"">
 <tr><td align=""center"">
  <table role=""presentation"" width=""520"" cellpadding=""0"" cellspacing=""0"" style=""max-width:520px;width:100%;background:#FFFFFF;border:1px solid #E3E9F2;border-radius:16px;"">
   <tr><td style=""padding:28px 32px 8px 32px;"">
     <table role=""presentation"" cellpadding=""0"" cellspacing=""0""><tr>
      <td style=""padding-right:12px;""><img src=""cid:pawfect-logo"" width=""40"" height=""40"" alt="""" style=""display:block;border:0;width:40px;height:40px;""></td>
      <td style=""font-size:20px;font-weight:700;color:#1F2A3C;vertical-align:middle;"">Pawfect</td>
     </tr></table>
   </td></tr>
   <tr><td style=""padding:8px 32px 16px 32px;"">
     <h1 style=""margin:12px 0 6px 0;font-size:22px;color:#3A7BD5;"">Your transaction history</h1>
     <p style=""margin:0;font-size:14px;line-height:21px;color:#44536A;"">Hi {E(first)}, here is a copy of your Pawfect transactions, newest first.</p>
   </td></tr>{rows}
   <tr><td style=""padding:4px 32px 28px 32px;"">
     {more}
     <p style=""margin:0 0 8px 0;font-size:14px;font-weight:700;color:#1F2A3C;"">Total paid ({completed} completed): {E(Money.Format(spent))}</p>
     <p style=""margin:0;font-size:13px;line-height:20px;color:#7A889C;"">Voided or refunded sales are listed for your records but aren't counted in the total. Questions? Just visit the shop. This is an automatic message from Pawfect.</p>
   </td></tr>
  </table>
  <p style=""margin:16px 0 0 0;font-size:12px;color:#7A889C;"">Pawfect &middot; Grooming &bull; Health &bull; Happiness</p>
 </td></tr>
</table></body></html>";

        var message = new MimeMessage();
        message.From.Add(MailboxAddress.Parse(s.From));
        message.To.Add(new MailboxAddress(name, to));
        message.Subject = "Your Pawfect transaction history";

        var body = new BodyBuilder { TextBody = text.ToString() };
        var logo = body.LinkedResources.Add("pawfect-logo.png", MyApp.Controls.PawBadge.RenderPng(120, Theme.Primary, Color.White, 27f),
            new ContentType("image", "png"));
        logo.ContentId = "pawfect-logo";
        body.HtmlBody = html;
        message.Body = body.ToMessageBody();
        return message;
    }
}
