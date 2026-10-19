using System.Data;
using Microsoft.Data.SqlClient;

namespace MyApp;

/// <summary>Grooming rooms (dbo.Rooms).</summary>
internal static class RoomRepository
{
    public static readonly string[] Types = { "Grooming", "Bathing", "Spa", "Others" };
    public static readonly string[] Statuses = { "Available", "Occupied", "Cleaning" };

    public sealed record RoomRow(int Id, string Name, string Type, string Status, bool Active);

    public static List<RoomRow> GetAll()
    {
        var list = new List<RoomRow>();
        using var conn = new SqlConnection(Database.ConnectionString);
        conn.Open();

        using var cmd = new SqlCommand(
            "SELECT RoomId, Name, RoomType, Status, IsActive FROM dbo.Rooms ORDER BY Name", conn);
        using var r = cmd.ExecuteReader();
        while (r.Read())
            list.Add(new RoomRow(r.GetInt32(0), r.GetString(1), r.GetString(2), r.GetString(3), r.GetBoolean(4)));
        return list;
    }

    /// <summary>Throws SqlException 2627/2601 if the room name already exists.</summary>
    public static void Add(string name, string type)
    {
        using var conn = new SqlConnection(Database.ConnectionString);
        conn.Open();
        using var cmd = new SqlCommand("INSERT INTO dbo.Rooms (Name, RoomType) VALUES (@n, @t)", conn);
        cmd.Parameters.Add("@n", SqlDbType.NVarChar, 50).Value = name.Trim();
        cmd.Parameters.Add("@t", SqlDbType.NVarChar, 20).Value = type;
        cmd.ExecuteNonQuery();
    }

    public static void Update(int id, string name, string type, bool active)
    {
        using var conn = new SqlConnection(Database.ConnectionString);
        conn.Open();
        using var cmd = new SqlCommand(
            "UPDATE dbo.Rooms SET Name = @n, RoomType = @t, IsActive = @a WHERE RoomId = @id", conn);
        cmd.Parameters.Add("@n", SqlDbType.NVarChar, 50).Value = name.Trim();
        cmd.Parameters.Add("@t", SqlDbType.NVarChar, 20).Value = type;
        cmd.Parameters.Add("@a", SqlDbType.Bit).Value = active;
        cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
        cmd.ExecuteNonQuery();
    }

    public static void SetStatus(int id, string status)
    {
        using var conn = new SqlConnection(Database.ConnectionString);
        conn.Open();
        using var cmd = new SqlCommand("UPDATE dbo.Rooms SET Status = @s WHERE RoomId = @id", conn);
        cmd.Parameters.Add("@s", SqlDbType.NVarChar, 10).Value = status;
        cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
        cmd.ExecuteNonQuery();
    }
}