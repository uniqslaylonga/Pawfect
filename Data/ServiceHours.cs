using System.Data;
using Microsoft.Data.SqlClient;

namespace MyApp;

/// <summary>
/// Service hours and appointment slot length, set from Service Catalog Management > Set Time Slots.
/// Stored in dbo.AppSettings (created on first use) and read by every time picker, so changing them
/// here changes the times staff can book. Defaults: 8:00 AM - 8:00 PM, 15-minute slots.
/// </summary>
internal static class ServiceHours
{
    private const string OpensKey = "ServiceHours.Opens";
    private const string ClosesKey = "ServiceHours.Closes";
    private const string StepKey = "ServiceHours.StepMinutes";

    public static readonly int[] StepChoices = { 15, 30, 45, 60 };

    private static bool _loaded;

    public static TimeSpan Opens { get; private set; } = TimeSpan.FromHours(8);
    public static TimeSpan Closes { get; private set; } = TimeSpan.FromHours(20);
    public static int StepMinutes { get; private set; } = 15;

    /// <summary>Reads the saved hours once; if the database can't be reached the defaults stay in place.</summary>
    public static void EnsureLoaded()
    {
        if (_loaded) return;
        try
        {
            using var conn = new SqlConnection(Database.ConnectionString);
            conn.Open();
            EnsureTable(conn);

            using var cmd = new SqlCommand(
                "SELECT SettingKey, SettingValue FROM dbo.AppSettings WHERE SettingKey IN (@o, @c, @s)", conn);
            cmd.Parameters.Add("@o", SqlDbType.NVarChar, 100).Value = OpensKey;
            cmd.Parameters.Add("@c", SqlDbType.NVarChar, 100).Value = ClosesKey;
            cmd.Parameters.Add("@s", SqlDbType.NVarChar, 100).Value = StepKey;

            var opens = Opens;
            var closes = Closes;
            int step = StepMinutes;
            using (var r = cmd.ExecuteReader())
            {
                while (r.Read())
                {
                    string key = r.GetString(0);
                    string value = r.IsDBNull(1) ? "" : r.GetString(1);
                    if (key == OpensKey && int.TryParse(value, out int o)) opens = TimeSpan.FromMinutes(o);
                    else if (key == ClosesKey && int.TryParse(value, out int c)) closes = TimeSpan.FromMinutes(c);
                    else if (key == StepKey && int.TryParse(value, out int s)) step = s;
                }
            }

            if (closes > opens && Array.IndexOf(StepChoices, step) >= 0)
            {
                Opens = opens;
                Closes = closes;
                StepMinutes = step;
            }
            _loaded = true;
        }
        catch (SqlException)
        {
            // keep the defaults; the next call tries again
        }
    }

    /// <summary>Saves the new hours. Throws SqlException when the database can't be updated.</summary>
    public static void Save(TimeSpan opens, TimeSpan closes, int stepMinutes)
    {
        using var conn = new SqlConnection(Database.ConnectionString);
        conn.Open();
        EnsureTable(conn);

        using var tx = conn.BeginTransaction();
        Upsert(conn, tx, OpensKey, ((int)opens.TotalMinutes).ToString());
        Upsert(conn, tx, ClosesKey, ((int)closes.TotalMinutes).ToString());
        Upsert(conn, tx, StepKey, stepMinutes.ToString());
        tx.Commit();

        Opens = opens;
        Closes = closes;
        StepMinutes = stepMinutes;
        _loaded = true;
    }

    private static void EnsureTable(SqlConnection conn)
    {
        using var cmd = new SqlCommand(@"
IF OBJECT_ID(N'dbo.AppSettings', N'U') IS NULL
    CREATE TABLE dbo.AppSettings (
        SettingKey   NVARCHAR(100) NOT NULL CONSTRAINT PK_AppSettings PRIMARY KEY,
        SettingValue NVARCHAR(400) NULL
    );", conn);
        cmd.ExecuteNonQuery();
    }

    private static void Upsert(SqlConnection conn, SqlTransaction tx, string key, string value)
    {
        using var cmd = new SqlCommand(@"
UPDATE dbo.AppSettings SET SettingValue = @v WHERE SettingKey = @k;
IF @@ROWCOUNT = 0 INSERT INTO dbo.AppSettings (SettingKey, SettingValue) VALUES (@k, @v);", conn, tx);
        cmd.Parameters.Add("@k", SqlDbType.NVarChar, 100).Value = key;
        cmd.Parameters.Add("@v", SqlDbType.NVarChar, 400).Value = value;
        cmd.ExecuteNonQuery();
    }
}
