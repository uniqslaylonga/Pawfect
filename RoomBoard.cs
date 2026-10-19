using System.Data;
using Microsoft.Data.SqlClient;

namespace MyApp;

/// <summary>Today's appointments per room, for the dashboard's Room Monitoring card.</summary>
internal static class RoomBoard
{
    public sealed record Booking(
        int RoomId, DateTime Start, string Status, string Customer, string Pet, string Service, string Groomer);

    /// <summary>Today's appointments that are assigned to a room, earliest first (cancelled and completed are left out).</summary>
    public static List<Booking> GetToday()
    {
        var list = new List<Booking>();
        DateTime day = DateTime.Today;

        using var conn = new SqlConnection(Database.ConnectionString);
        conn.Open();
        using var cmd = new SqlCommand(
            @"SELECT a.RoomId, a.StartTime, a.Status, c.FullName, p.Name, s.Name, g.Name
              FROM dbo.Appointments a
              JOIN dbo.Customers c ON c.CustomerId = a.CustomerId
              JOIN dbo.Pets p      ON p.PetId      = a.PetId
              JOIN dbo.Services s  ON s.ServiceId  = a.ServiceId
              LEFT JOIN dbo.Groomers g ON g.GroomerId = a.GroomerId
              WHERE a.RoomId IS NOT NULL
                AND a.StartTime >= @day AND a.StartTime < @next
                AND a.Status NOT IN (N'Cancelled', N'Completed')
              ORDER BY a.StartTime", conn);
        cmd.Parameters.Add("@day", SqlDbType.DateTime2).Value = day;
        cmd.Parameters.Add("@next", SqlDbType.DateTime2).Value = day.AddDays(1);

        using var r = cmd.ExecuteReader();
        while (r.Read())
            list.Add(new Booking(
                r.GetInt32(0), r.GetDateTime(1), r.GetString(2), r.GetString(3),
                r.GetString(4), r.GetString(5), r.IsDBNull(6) ? "" : r.GetString(6)));
        return list;
    }
}