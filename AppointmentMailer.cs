using System.Data;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Data.SqlClient;
using MimeKit;

namespace MyApp;

/// <summary>Why the customer is being emailed about an appointment.</summary>
internal enum AppointmentMail { Scheduled, Rescheduled, Cancelled }

/// <summary>
/// Emails the customer when their appointment is cancelled, when staff set its schedule (day/time), or when it is moved to
/// another time. Uses the same smtp.config as the verification code email. It never blocks or breaks the screen the staff
/// member is using: the mail is sent in the background and any problem is only written to the audit trail.
/// </summary>
internal static class AppointmentMailer
{
    public static void Notify(int appointmentId, AppointmentMail kind)
    {
        // Fire and forget: the appointment change is already saved, the email is a courtesy on top of it.
        _ = Task.Run(async () =>
        {
            try { await SendAsync(appointmentId, kind); }
            catch (Exception ex)
            {
                AuditLog.Record(null, "Email Failed", $"Appointment #{appointmentId} ({kind}): {ex.Message}");
            }
        });
    }

    private sealed record Details(string CustomerName, string Email, string PetName, string ServiceName, DateTime Start, string Queue);

    private static Details? Load(int appointmentId)
    {
        using var conn = new SqlConnection(Database.ConnectionString);
        conn.Open();
        using var cmd = new SqlCommand(@"
SELECT c.FullName, ISNULL(c.Email, N''), p.Name, s.Name, a.StartTime, ISNULL(a.QueueNumber, N'')
FROM dbo.Appointments a
JOIN dbo.Customers c ON c.CustomerId = a.CustomerId
JOIN dbo.Pets p      ON p.PetId = a.PetId
JOIN dbo.Services s  ON s.ServiceId = a.ServiceId
WHERE a.AppointmentId = @id", conn);
        cmd.Parameters.Add("@id", SqlDbType.Int).Value = appointmentId;
        using var r = cmd.ExecuteReader();
        if (!r.Read()) return null;
        return new Details(r.GetString(0), r.GetString(1).Trim(), r.GetString(2), r.GetString(3), r.GetDateTime(4), r.GetString(5));
    }

    private static async Task SendAsync(int appointmentId, AppointmentMail kind)
    {
        var settings = EmailOtp.LoadSettings();
        if (settings is null) return;   // email isn't set up on this PC: nothing to do

        var d = Load(appointmentId);
        if (d is null || d.Email.Length == 0) return;   // no customer email on file

        MimeMessage message;
        try { message = Build(settings, d, kind); }
        catch (ParseException) { return; }   // the customer's email address isn't valid

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
                AuditLog.Record(null, "Email Sent", $"{Subject(kind)} -> {d.Email} (appointment #{appointmentId})");
                return;
            }
            catch (MailKit.Security.AuthenticationException ex)
            {
                throw new InvalidOperationException("The mail server refused the login - check smtp.config. " + ex.Message, ex);
            }
            catch (Exception ex) when (ex is SocketException or IOException or MailKit.ProtocolException or MailKit.CommandException
                                          or MailKit.ServiceNotConnectedException or MailKit.Security.SslHandshakeException
                                          or System.Security.Authentication.AuthenticationException
                                          or TimeoutException or OperationCanceledException or InvalidOperationException)
            {
                last = ex;   // try the other port
            }
        }
        throw new InvalidOperationException("Couldn't reach the mail server. " + last?.Message, last);
    }

    private static string Subject(AppointmentMail kind) => kind switch
    {
        AppointmentMail.Scheduled => "Your Pawfect appointment is scheduled",
        AppointmentMail.Rescheduled => "Your Pawfect appointment has been rescheduled",
        _ => "Your Pawfect appointment was cancelled",
    };

    private static MimeMessage Build(EmailOtp.SmtpSettings s, Details d, AppointmentMail kind)
    {
        string when = d.Start.ToString("dddd, MMMM d, yyyy 'at' h:mm tt", CultureInfo.GetCultureInfo("en-US"));
        string first = d.CustomerName.Split(' ', StringSplitOptions.RemoveEmptyEntries).FirstOrDefault() ?? "there";

        string headline, intro, accent, accentSoft;
        string textBody;
        switch (kind)
        {
            case AppointmentMail.Scheduled:
                headline = "Your appointment is scheduled";
                intro = $"Hi {first}, we've set the schedule for {d.PetName}'s visit. Please arrive a few minutes early.";
                accent = "#3A7BD5"; accentSoft = "#EAF2FD";
                textBody = $"Hi {d.CustomerName},\r\n\r\nWe've scheduled {d.PetName}'s {d.ServiceName} for {when}.\r\nPlease arrive a few minutes early.\r\n\r\n- Pawfect";
                break;
            case AppointmentMail.Rescheduled:
                headline = "Your appointment was rescheduled";
                intro = $"Hi {first}, {d.PetName}'s visit has been moved to a new time. Here are the new details.";
                accent = "#7C4DDB"; accentSoft = "#F1EAFD";
                textBody = $"Hi {d.CustomerName},\r\n\r\n{d.PetName}'s {d.ServiceName} has been moved to {when}.\r\nIf this time doesn't work for you, please contact us.\r\n\r\n- Pawfect";
                break;
            default:
                headline = "Your appointment was cancelled";
                intro = $"Hi {first}, {d.PetName}'s appointment has been cancelled. You can book a new one any time.";
                accent = "#D93636"; accentSoft = "#FDE8E8";
                textBody = $"Hi {d.CustomerName},\r\n\r\n{d.PetName}'s {d.ServiceName} on {when} has been cancelled.\r\nYou can book a new appointment any time.\r\n\r\n- Pawfect";
                break;
        }

        string E(string v) => WebUtility.HtmlEncode(v);
        string queueRow = d.Queue.Length == 0 ? "" :
            $"<tr><td style=\"padding:4px 0;color:#7A889C;\">Queue number</td><td style=\"padding:4px 0;text-align:right;font-weight:600;color:#1F2A3C;\">{E(d.Queue)}</td></tr>";

        string html = $@"<!DOCTYPE html>
<html><body style=""margin:0;padding:0;background:#F3F7FC;font-family:'Segoe UI',Arial,sans-serif;"">
<table role=""presentation"" width=""100%"" cellpadding=""0"" cellspacing=""0"" style=""background:#F3F7FC;padding:32px 12px;"">
 <tr><td align=""center"">
  <table role=""presentation"" width=""480"" cellpadding=""0"" cellspacing=""0"" style=""max-width:480px;width:100%;background:#FFFFFF;border:1px solid #E3E9F2;border-radius:16px;"">
   <tr><td style=""padding:28px 32px 8px 32px;"">
     <table role=""presentation"" cellpadding=""0"" cellspacing=""0""><tr>
      <td style=""padding-right:12px;""><img src=""cid:pawfect-logo"" width=""40"" height=""40"" alt="""" style=""display:block;border:0;width:40px;height:40px;""></td>
      <td style=""font-size:20px;font-weight:700;color:#1F2A3C;vertical-align:middle;"">Pawfect</td>
     </tr></table>
   </td></tr>
   <tr><td style=""padding:8px 32px 0 32px;"">
     <h1 style=""margin:12px 0 6px 0;font-size:22px;color:{accent};"">{E(headline)}</h1>
     <p style=""margin:0;font-size:14px;line-height:21px;color:#44536A;"">{E(intro)}</p>
   </td></tr>
   <tr><td style=""padding:20px 32px;"">
     <table role=""presentation"" width=""100%"" cellpadding=""0"" cellspacing=""0"" style=""background:{accentSoft};border-radius:12px;padding:12px 16px;font-size:14px;"">
      <tr><td style=""padding:4px 0;color:#7A889C;"">Pet</td><td style=""padding:4px 0;text-align:right;font-weight:600;color:#1F2A3C;"">{E(d.PetName)}</td></tr>
      <tr><td style=""padding:4px 0;color:#7A889C;"">Service</td><td style=""padding:4px 0;text-align:right;font-weight:600;color:#1F2A3C;"">{E(d.ServiceName)}</td></tr>
      <tr><td style=""padding:4px 0;color:#7A889C;"">{(kind == AppointmentMail.Cancelled ? "Was scheduled for" : "Date &amp; time")}</td><td style=""padding:4px 0;text-align:right;font-weight:600;color:#1F2A3C;"">{E(when)}</td></tr>
      {queueRow}
     </table>
   </td></tr>
   <tr><td style=""padding:0 32px 28px 32px;"">
     <p style=""margin:0;font-size:13px;line-height:20px;color:#7A889C;"">Questions? Just reply to the shop or visit us. This is an automatic message from Pawfect.</p>
   </td></tr>
  </table>
  <p style=""margin:16px 0 0 0;font-size:12px;color:#7A889C;"">Pawfect &middot; Grooming &bull; Health &bull; Happiness</p>
 </td></tr>
</table></body></html>";

        var message = new MimeMessage();
        message.From.Add(MailboxAddress.Parse(s.From));
        message.To.Add(new MailboxAddress(d.CustomerName, d.Email));
        message.Subject = Subject(kind);

        var body = new BodyBuilder { TextBody = textBody };
        var logo = body.LinkedResources.Add("pawfect-logo.png", MyApp.Controls.PawBadge.RenderPng(120, Theme.Primary, Color.White, 27f),
            new ContentType("image", "png"));
        logo.ContentId = "pawfect-logo";
        body.HtmlBody = html;
        message.Body = body.ToMessageBody();
        return message;
    }
}
