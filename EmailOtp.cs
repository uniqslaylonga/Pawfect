using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace MyApp;

/// <summary>Something the customer can read: SMTP isn't set up, the mail server refused, the code expired...</summary>
internal sealed class OtpException : Exception
{
    public OtpException(string message, Exception? inner = null) : base(message, inner) { }
}

/// <summary>One code that was emailed. Only a salted hash of the code is kept in memory, never the code itself.</summary>
internal sealed class OtpChallenge
{
    internal OtpChallenge(string email, byte[] salt, byte[] hash, DateTime expiresAtUtc)
    {
        Email = email;
        Salt = salt;
        Hash = hash;
        ExpiresAtUtc = expiresAtUtc;
    }

    public string Email { get; }
    internal byte[] Salt { get; }
    internal byte[] Hash { get; }
    public DateTime ExpiresAtUtc { get; }
    public int FailedAttempts { get; internal set; }
}

/// <summary>
/// Sends a 6-digit one-time code by email and checks what the customer types back.
/// Mail settings come from environment variables (PAWFECT_SMTP_HOST, _PORT, _USER, _PASSWORD, _FROM) or from a
/// smtp.config file next to Pawfect.exe (or in any folder above it) with one "key=value" per line:
///     host=smtp.gmail.com
///     port=587
///     user=yourshop@gmail.com
///     password=your-app-password
///     from=Pawfect &lt;yourshop@gmail.com&gt;     (optional, defaults to user)
/// </summary>
internal static class EmailOtp
{
    public const int CodeLength = 6;
    public const int ResendSeconds = 45;
    public const int MaxAttempts = 5;
    private static readonly TimeSpan Lifetime = TimeSpan.FromMinutes(10);

    public static bool IsConfigured => LoadSettings() is not null;

    /// <summary>Where to put smtp.config, shown when nothing is set up yet.</summary>
    public static string ConfigHint => Path.Combine(AppContext.BaseDirectory, "smtp.config");

    public static async Task<OtpChallenge> SendAsync(string toEmail, string recipientName)
    {
        var settings = LoadSettings()
            ?? throw new OtpException("Email sending isn't set up yet. Ask the shop to add an smtp.config file next to Pawfect.exe.");

        string code = RandomNumberGenerator.GetInt32(0, 1_000_000).ToString("D6");
        var salt = RandomNumberGenerator.GetBytes(16);
        var challenge = new OtpChallenge(toEmail.Trim(), salt, Hash(code, salt), DateTime.UtcNow + Lifetime);

        MimeMessage message;
        try { message = BuildMessage(settings, challenge.Email, recipientName, code); }
        catch (ParseException ex) { throw new OtpException("An email address isn't valid - check the user and from lines in smtp.config.", ex); }
        catch (Exception ex) when (ex is not OtpException) { throw new OtpException($"The email couldn't be prepared.\r\n({ex.Message})", ex); }

        // Gmail accepts 587 (STARTTLS) and 465 (SSL). Some networks block one of them, so try the other as a fallback.
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
                return challenge;
            }
            catch (MailKit.Security.AuthenticationException ex)
            {
                throw new OtpException($"The mail server refused the login - check the user and app password in smtp.config.\r\n({ex.Message})", ex);
            }
            catch (Exception ex) when (ex is SocketException or IOException or MailKit.ProtocolException or MailKit.CommandException or MailKit.ServiceNotConnectedException
                                          or MailKit.Security.SslHandshakeException
                                          or System.Security.Authentication.AuthenticationException
                                          or TimeoutException or OperationCanceledException or InvalidOperationException)
            {
                if (ex.Message.Contains("5.7.", StringComparison.Ordinal) || ex.Message.Contains("not accepted", StringComparison.OrdinalIgnoreCase))
                    throw new OtpException($"Gmail refused the login - the email or app password in smtp.config is wrong.\r\n({ex.Message})", ex);
                last = ex;   // try the next port
            }
            catch (Exception ex)
            {
                // Anything else: show the real reason instead of letting the app fail silently.
                throw new OtpException($"The email couldn't be sent.\r\n({ex.GetType().Name}: {ex.Message})", ex);
            }
        }
        throw new OtpException($"We couldn't reach the mail server. Check your internet connection, or try another network.\r\n({last?.Message})", last);
    }

    public static void Verify(OtpChallenge challenge, string typed)
    {
        if (DateTime.UtcNow > challenge.ExpiresAtUtc)
            throw new OtpException("That code has expired. Tap Resend OTP to get a new one.");
        if (challenge.FailedAttempts >= MaxAttempts)
            throw new OtpException("Too many wrong tries. Tap Resend OTP to get a new code.");

        bool ok = typed.Length == CodeLength
                  && CryptographicOperations.FixedTimeEquals(Hash(typed, challenge.Salt), challenge.Hash);
        if (ok) return;

        challenge.FailedAttempts++;
        int left = MaxAttempts - challenge.FailedAttempts;
        throw new OtpException(left > 0
            ? $"That code isn't right. {left} {(left == 1 ? "try" : "tries")} left."
            : "Too many wrong tries. Tap Resend OTP to get a new code.");
    }

    private static byte[] Hash(string code, byte[] salt)
        => SHA256.HashData(salt.Concat(Encoding.UTF8.GetBytes(code)).ToArray());

    // ------------------------------------------------------------------ the email itself

    private static MimeMessage BuildMessage(SmtpSettings s, string to, string name, string code)
    {
        string first = WebUtility.HtmlEncode(name.Split(' ', StringSplitOptions.RemoveEmptyEntries).FirstOrDefault() ?? "there");
        int minutes = (int)Lifetime.TotalMinutes;

        string text =
            $"Hi {name},\r\n\r\nYour Pawfect verification code is: {code}\r\n\r\n" +
            $"It expires in {minutes} minutes. If you didn't ask for this, you can ignore this email.\r\n\r\n- Pawfect";

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
     <h1 style=""margin:12px 0 6px 0;font-size:22px;color:#13294B;"">Verify it's you</h1>
     <p style=""margin:0;font-size:14px;line-height:21px;color:#44536A;"">Hi {first}, use this code to confirm your email and add your pet. Enter it in the Pawfect app.</p>
   </td></tr>
   <tr><td align=""center"" style=""padding:24px 32px;"">
     <div style=""background:#EAF2FD;border:1px solid #D6E6FB;border-radius:12px;padding:18px 0;font-size:34px;font-weight:700;letter-spacing:10px;color:#3A7BD5;text-indent:10px;"">{code}</div>
   </td></tr>
   <tr><td style=""padding:0 32px 28px 32px;"">
     <p style=""margin:0;font-size:13px;line-height:20px;color:#7A889C;"">This code expires in {minutes} minutes. If you didn't ask for it, you can safely ignore this email &mdash; nobody can add a pet to your account without it.</p>
   </td></tr>
  </table>
  <p style=""margin:16px 0 0 0;font-size:12px;color:#7A889C;"">Pawfect &middot; Grooming &bull; Health &bull; Happiness</p>
 </td></tr>
</table></body></html>";

        var message = new MimeMessage();
        message.From.Add(MailboxAddress.Parse(s.From));
        message.To.Add(MailboxAddress.Parse(to));
        message.Subject = $"{code} is your Pawfect verification code";
        // The logo travels inside the message (cid:) so it shows even when the mail client blocks remote images.
        var body = new BodyBuilder { TextBody = text };
        var logo = body.LinkedResources.Add("pawfect-logo.png", MyApp.Controls.PawBadge.RenderPng(120, Theme.Primary, Color.White, 27f),
            new ContentType("image", "png"));
        logo.ContentId = "pawfect-logo";
        body.HtmlBody = html;
        message.Body = body.ToMessageBody();
        return message;
    }

    // ------------------------------------------------------------------ settings

    private sealed record SmtpSettings(string Host, int Port, string User, string Password, string From);

    private static SmtpSettings? _settings;
    private static bool _loaded;

    private static SmtpSettings? LoadSettings()
    {
        if (_loaded && _settings is not null) return _settings;
        _settings = ReadSettings();
        _loaded = true;
        return _settings;
    }

    private static SmtpSettings? ReadSettings()
    {
        var values = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        foreach (var key in new[] { "host", "port", "user", "password", "from" })
        {
            foreach (var scope in new[] { EnvironmentVariableTarget.Process, EnvironmentVariableTarget.User, EnvironmentVariableTarget.Machine })
            {
                var v = Environment.GetEnvironmentVariable("PAWFECT_SMTP_" + key.ToUpperInvariant(), scope);
                if (!string.IsNullOrWhiteSpace(v)) { values[key] = v.Trim(); break; }
            }
        }

        try
        {
            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            for (int depth = 0; dir is not null && depth < 6; depth++, dir = dir.Parent)
            {
                foreach (var name in new[] { "smtp.config", "smtp.config.txt" })
                {
                    var file = Path.Combine(dir.FullName, name);
                    if (!File.Exists(file)) continue;
                    foreach (var line in File.ReadAllLines(file))
                    {
                        int eq = line.IndexOf('=');
                        if (eq <= 0 || line.TrimStart().StartsWith('#')) continue;
                        values.TryAdd(line[..eq].Trim(), line[(eq + 1)..].Trim());
                    }
                    goto done;
                }
            }
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            // Unreadable folder; fall through to "not configured".
        }
    done:

        if (!values.TryGetValue("host", out var host) || !values.TryGetValue("user", out var user) ||
            !values.TryGetValue("password", out var password))
            return null;

        int port = values.TryGetValue("port", out var p) && int.TryParse(p, out var n) ? n : 587;
        values.TryGetValue("from", out var sender);
        return new SmtpSettings(host, port, user, password, string.IsNullOrWhiteSpace(sender) ? user : sender);
    }
}