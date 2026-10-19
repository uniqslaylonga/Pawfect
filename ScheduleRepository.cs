using System.Data;
using Microsoft.Data.SqlClient;

namespace MyApp;

/// <summary>Weekly shifts and absences for employees.</summary>
internal static class ScheduleRepository
{
    public static readonly string[] Days =
        { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };

    public sealed record ShiftRow(int Day, TimeSpan Start, TimeSpan End);   // Day: 1 = Monday ... 7 = Sunday
    public sealed record AbsenceRow(int Id, DateTime Date, string? Reason, string RecordedBy);

    public static List<ShiftRow> GetShifts(int employeeId)
    {
        var list = new List<ShiftRow>();
        using var conn = new SqlConnection(Database.ConnectionString);
        conn.Open();
        using var cmd = new SqlCommand(
            "SELECT DayNo, StartTime, EndTime FROM dbo.EmployeeSchedules WHERE EmployeeId = @e ORDER BY DayNo", conn);
        cmd.Parameters.Add("@e", SqlDbType.Int).Value = employeeId;
        using var r = cmd.ExecuteReader();
        while (r.Read())
            list.Add(new ShiftRow(r.GetByte(0), r.GetTimeSpan(1), r.GetTimeSpan(2)));
        return list;
    }

    /// <summary>Replaces the employee's whole weekly schedule in one transaction.</summary>
    public static void SaveShifts(int employeeId, IEnumerable<ShiftRow> shifts)
    {
        using var conn = new SqlConnection(Database.ConnectionString);
        conn.Open();
        using var tx = conn.BeginTransaction();
        try
        {
            using (var del = new SqlCommand("DELETE FROM dbo.EmployeeSchedules WHERE EmployeeId = @e", conn, tx))
            {
                del.Parameters.Add("@e", SqlDbType.Int).Value = employeeId;
                del.ExecuteNonQuery();
            }

            foreach (var s in shifts)
            {
                using var ins = new SqlCommand(
                    "INSERT INTO dbo.EmployeeSchedules (EmployeeId, DayNo, StartTime, EndTime) VALUES (@e, @d, @s, @t)",
                    conn, tx);
                ins.Parameters.Add("@e", SqlDbType.Int).Value = employeeId;
                ins.Parameters.Add("@d", SqlDbType.TinyInt).Value = (byte)s.Day;
                ins.Parameters.Add("@s", SqlDbType.Time).Value = s.Start;
                ins.Parameters.Add("@t", SqlDbType.Time).Value = s.End;
                ins.ExecuteNonQuery();
            }
            tx.Commit();
        }
        catch
        {
            tx.Rollback();
            throw;
        }
    }

    public static List<AbsenceRow> GetAbsences(int employeeId)
    {
        var list = new List<AbsenceRow>();
        using var conn = new SqlConnection(Database.ConnectionString);
        conn.Open();
        using var cmd = new SqlCommand(
            @"SELECT AbsenceId, AbsenceDate, Reason, RecordedBy
              FROM dbo.EmployeeAbsences WHERE EmployeeId = @e ORDER BY AbsenceDate DESC", conn);
        cmd.Parameters.Add("@e", SqlDbType.Int).Value = employeeId;
        using var r = cmd.ExecuteReader();
        while (r.Read())
            list.Add(new AbsenceRow(r.GetInt32(0), r.GetDateTime(1),
                r.IsDBNull(2) ? null : r.GetString(2), r.GetString(3)));
        return list;
    }

    /// <summary>Throws SqlException 2627/2601 if that day is already recorded.</summary>
    public static void AddAbsence(int employeeId, DateTime date, string? reason, string actor)
    {
        using var conn = new SqlConnection(Database.ConnectionString);
        conn.Open();
        using var cmd = new SqlCommand(
            "INSERT INTO dbo.EmployeeAbsences (EmployeeId, AbsenceDate, Reason, RecordedBy) VALUES (@e, @d, @r, @a)", conn);
        cmd.Parameters.Add("@e", SqlDbType.Int).Value = employeeId;
        cmd.Parameters.Add("@d", SqlDbType.Date).Value = date.Date;
        cmd.Parameters.Add("@r", SqlDbType.NVarChar, 200).Value =
            string.IsNullOrWhiteSpace(reason) ? DBNull.Value : reason.Trim();
        cmd.Parameters.Add("@a", SqlDbType.NVarChar, 100).Value = actor;
        cmd.ExecuteNonQuery();
    }

    public static void RemoveAbsence(int absenceId)
    {
        using var conn = new SqlConnection(Database.ConnectionString);
        conn.Open();
        using var cmd = new SqlCommand("DELETE FROM dbo.EmployeeAbsences WHERE AbsenceId = @id", conn);
        cmd.Parameters.Add("@id", SqlDbType.Int).Value = absenceId;
        cmd.ExecuteNonQuery();
    }

    /// <summary>Per employee: working days per week, and absences recorded this month.</summary>
    public static Dictionary<int, (int Days, int Absences)> GetSummaries()
    {
        var map = new Dictionary<int, (int Days, int Absences)>();
        using var conn = new SqlConnection(Database.ConnectionString);
        conn.Open();

        using (var cmd = new SqlCommand(
            "SELECT EmployeeId, COUNT(1) FROM dbo.EmployeeSchedules GROUP BY EmployeeId", conn))
        using (var r = cmd.ExecuteReader())
            while (r.Read()) map[r.GetInt32(0)] = (r.GetInt32(1), 0);

        using (var cmd = new SqlCommand(
            "SELECT EmployeeId, COUNT(1) FROM dbo.EmployeeAbsences WHERE AbsenceDate >= @f GROUP BY EmployeeId", conn))
        {
            cmd.Parameters.Add("@f", SqlDbType.Date).Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            using var r = cmd.ExecuteReader();
            while (r.Read())
            {
                int id = r.GetInt32(0);
                map[id] = (map.TryGetValue(id, out var v) ? v.Days : 0, r.GetInt32(1));
            }
        }
        return map;
    }
}