using System.Data;
using Microsoft.Data.SqlClient;

namespace MyApp;

/// <summary>Writes and reads the audit trail (dbo.AuditLog).</summary>
internal static class AuditLog
{
    public sealed record Entry(int Id, string Actor, string Action, string Details, DateTime When);

    /// <summary>Records one action. Never throws: a logging problem must not break the screen the admin is using.</summary>
    public static void Record(UserAccount? actor, string action, string details)
    {
        try
        {
            using var conn = new SqlConnection(Database.ConnectionString);
            conn.Open();

            using var cmd = new SqlCommand(
                "INSERT INTO dbo.AuditLog (ActorName, ActorEmail, Action, Details) VALUES (@n, @e, @a, @d)", conn);
            cmd.Parameters.Add("@n", SqlDbType.NVarChar, 100).Value = actor?.Name ?? "System";
            cmd.Parameters.Add("@e", SqlDbType.NVarChar, 256).Value = (object?)actor?.Email ?? DBNull.Value;
            cmd.Parameters.Add("@a", SqlDbType.NVarChar, 50).Value = action;
            cmd.Parameters.Add("@d", SqlDbType.NVarChar, 500).Value = details.Length > 500 ? details[..500] : details;
            cmd.ExecuteNonQuery();
        }
        catch (SqlException ex)
        {
            System.Diagnostics.Debug.WriteLine("Audit write failed: " + ex.Message);
        }
    }

    /// <summary>Newest entries first.</summary>
    public static List<Entry> GetRecent(int top = 200)
    {
        var list = new List<Entry>();

        using var conn = new SqlConnection(Database.ConnectionString);
        conn.Open();

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