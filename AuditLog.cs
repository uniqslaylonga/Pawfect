using System.Data;
using Microsoft.Data.SqlClient;

namespace MyApp;

/// <summary>Writes and reads the audit trail (dbo.AuditLog).</summary>
internal static class AuditLog
{
    public sealed record Entry(int Id, string Actor, string Action, string Details, DateTime When);

    private static bool _tableChecked;

    /// <summary>Creates dbo.AuditLog the first time it is needed, so no manual SQL step is required.</summary>
    private static void EnsureTable(SqlConnection conn)
    {
        if (_tableChecked) return;
        using var cmd = new SqlCommand(
            @"IF OBJECT_ID(N'dbo.AuditLog', N'U') IS NULL
              CREATE TABLE dbo.AuditLog
              (
                  AuditId    INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_AuditLog PRIMARY KEY,
                  ActorName  NVARCHAR(100) NOT NULL,
                  ActorEmail NVARCHAR(256) NULL,
                  Action     NVARCHAR(50)  NOT NULL,
                  Details    NVARCHAR(500) NOT NULL CONSTRAINT DF_AuditLog_Details DEFAULT N'',
                  CreatedAt  DATETIME2     NOT NULL CONSTRAINT DF_AuditLog_CreatedAt DEFAULT SYSUTCDATETIME()
              );", conn);
        cmd.ExecuteNonQuery();
        _tableChecked = true;
    }

    /// <summary>Records one action. Never throws: a logging problem must not break the screen the admin is using.</summary>
    public static void Record(UserAccount? actor, string action, string details)
        => Record(actor?.Name ?? "System", actor?.Email, action, details);

    /// <summary>A sign-in that was refused (wrong password or unknown email). Only the typed email is stored, never the password.</summary>
    public static void RecordFailedLogin(string email)
        => Record(email.Length > 0 ? email : "Unknown", email, "Login Failed", "Incorrect email or password");

    private static void Record(string actorName, string? actorEmail, string action, string details)
    {
        try
        {
            using var conn = new SqlConnection(Database.ConnectionString);
            conn.Open();
            EnsureTable(conn);

            using var cmd = new SqlCommand(
                "INSERT INTO dbo.AuditLog (ActorName, ActorEmail, Action, Details) VALUES (@n, @e, @a, @d)", conn);
            cmd.Parameters.Add("@n", SqlDbType.NVarChar, 100).Value = actorName.Length > 100 ? actorName[..100] : actorName;
            cmd.Parameters.Add("@e", SqlDbType.NVarChar, 256).Value = (object?)actorEmail ?? DBNull.Value;
            cmd.Parameters.Add("@a", SqlDbType.NVarChar, 50).Value = action;
            cmd.Parameters.Add("@d", SqlDbType.NVarChar, 500).Value = details.Length > 500 ? details[..500] : details;
            cmd.ExecuteNonQuery();
        }
        catch (SqlException ex)
        {
            System.Diagnostics.Debug.WriteLine("Audit write failed: " + ex.Message);
        }
    }

    /// <summary>Appointment requests still waiting for staff, and active login accounts (the Audit Trail's "Pending" and "System Users" tiles).</summary>
    public static (int Pending, int ActiveUsers, int InactiveUsers) GetCounts()
    {
        using var conn = new SqlConnection(Database.ConnectionString);
        conn.Open();
        using var cmd = new SqlCommand(@"
SELECT (SELECT COUNT(*) FROM dbo.Appointments WHERE Status IN (N'Pending', N'Rescheduled')),
       (SELECT COUNT(*) FROM dbo.Users WHERE IsActive = 1),
       (SELECT COUNT(*) FROM dbo.Users WHERE IsActive = 0);", conn);
        using var r = cmd.ExecuteReader();
        return r.Read() ? (r.GetInt32(0), r.GetInt32(1), r.GetInt32(2)) : (0, 0, 0);
    }

    /// <summary>Newest entries first.</summary>
    public static List<Entry> GetRecent(int top = 200)
    {
        var list = new List<Entry>();

        using var conn = new SqlConnection(Database.ConnectionString);
        conn.Open();
        EnsureTable(conn);

        using var cmd = new SqlCommand(
            "SELECT TOP (@top) AuditId, ActorName, Action, Details, CreatedAt FROM dbo.AuditLog ORDER BY AuditId DESC", conn);
        cmd.Parameters.Add("@top", SqlDbType.Int).Value = top;
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
            list.Add(new Entry(reader.GetInt32(0), reader.GetString(1), reader.GetString(2),
                               reader.GetString(3), reader.GetDateTime(4).ToLocalTime()));
        return list;
    }
}

/// <summary>What an audit entry means: its module, kind of action, status and reference. Worked out from the action text, so old entries classify too.</summary>
internal static class AuditInfo
{
    public static string Module(string action)
    {
        bool Has(string t) => action.Contains(t, StringComparison.OrdinalIgnoreCase);
        if (Has("Login")) return "System";
        if (Has("Appointment")) return "Appointments";
        if (Has("Product") || Has("Stock")) return "Inventory";
        if (Has("Service")) return "Services";
        if (Has("Sale") || Has("Report")) return "Financial";
        if (Has("Room")) return "Rooms";
        if (Has("User") || Has("Employee")) return "Users";
        return "System";
    }

    /// <summary>Created, Updated, Approved, Deleted (removed or deactivated), Login or Exported.</summary>
    public static string Type(string action)
    {
        bool Has(string t) => action.Contains(t, StringComparison.OrdinalIgnoreCase);
        if (Has("Login")) return "Login";
        if (Has("Deactivated") || Has("Removed") || Has("Deleted") || Has("Cancelled")) return "Deleted";
        if (Has("Confirmed") || Has("Checked In") || Has("Approved")) return "Approved";
        if (Has("Created") || Has("Added") || Has("Completed")) return "Created";
        if (Has("Exported")) return "Exported";
        return "Updated";
    }

    public static bool IsFailed(string action) => action.Contains("Failed", StringComparison.OrdinalIgnoreCase);

    public static string Status(string action) => IsFailed(action) ? "Failed" : "Success";

    /// <summary>Deactivated / removed / cancelled records and refused sign-ins.</summary>
    public static bool IsCritical(string action) => IsFailed(action) || Type(action) == "Deleted";

    /// <summary>"#12" or "Receipt R-0001" taken from the details, otherwise an em dash.</summary>
    public static string Reference(string details)
    {
        var m = System.Text.RegularExpressions.Regex.Match(details, @"Receipt\s+\S+|#\d+");
        return m.Success ? m.Value.TrimEnd(',', '.', ')') : "\u2014";
    }
}
