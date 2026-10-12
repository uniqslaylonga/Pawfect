using System.Data;
using Microsoft.Data.SqlClient;

namespace MyApp;

/// <summary>Everything the Service Catalog page needs from dbo.Services.</summary>
internal static class ServiceRepository
{
     
    public sealed record ServiceRow(
        int Id, string Name, string Category, string Size, string? Description,
        decimal Price, int DurationMinutes, bool Active);

    public static List<ServiceRow> GetAll()
    {
        var list = new List<ServiceRow>();

        using var conn = new SqlConnection(Database.ConnectionString);
        conn.Open();

        using var cmd = new SqlCommand(
            "SELECT ServiceId, Name, Category, Size, Description, Price, DurationMinutes, IsActive " +
            "FROM dbo.Services ORDER BY Name, Size", conn);
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
            list.Add(new ServiceRow(
                reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3),
                reader.IsDBNull(4) ? null : reader.GetString(4),
                reader.GetDecimal(5), reader.GetInt32(6), reader.GetBoolean(7)));
        return list;
    }

    /// <summary>Throws SqlException 2627/2601 if this name and size already exist.</summary>
    public static void Add(string name, string category, string size, string? description, decimal price, int minutes)
    {
        using var conn = new SqlConnection(Database.ConnectionString);
        conn.Open();

        using var cmd = new SqlCommand(
            "INSERT INTO dbo.Services (Name, Category, Size, Description, Price, DurationMinutes) " +
            "VALUES (@n, @c, @s, @d, @p, @m)", conn);
        Fill(cmd, name, category, size, description, price, minutes);
        cmd.ExecuteNonQuery();
    }

    /// <summary>Throws SqlException 2627/2601 if this name and size are taken.</summary>
    public static void Update(int id, string name, string category, string size, string? description, decimal price, int minutes)
    {
        using var conn = new SqlConnection(Database.ConnectionString);
        conn.Open();

        using var cmd = new SqlCommand(
            "UPDATE dbo.Services SET Name = @n, Category = @c, Size = @s, Description = @d, " +
            "Price = @p, DurationMinutes = @m WHERE ServiceId = @id", conn);
        Fill(cmd, name, category, size, description, price, minutes);
        cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
        cmd.ExecuteNonQuery();
    }

    /// <summary>Soft delete: hides the service from new bookings but keeps old records valid.</summary>
    public static void SetActive(int id, bool active)
    {
        using var conn = new SqlConnection(Database.ConnectionString);
        conn.Open();

        using var cmd = new SqlCommand("UPDATE dbo.Services SET IsActive = @a WHERE ServiceId = @id", conn);
        cmd.Parameters.Add("@a", SqlDbType.Bit).Value = active;
        cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
        cmd.ExecuteNonQuery();
    }

    private static void Fill(SqlCommand cmd, string name, string category, string size, string? description, decimal price, int minutes)
    {
        cmd.Parameters.Add("@n", SqlDbType.NVarChar, 100).Value = name.Trim();
        cmd.Parameters.Add("@c", SqlDbType.NVarChar, 50).Value = category;
        cmd.Parameters.Add("@s", SqlDbType.NVarChar, 10).Value = size;
        cmd.Parameters.Add("@d", SqlDbType.NVarChar, 300).Value =
            string.IsNullOrWhiteSpace(description) ? DBNull.Value : description.Trim();
        var p = cmd.Parameters.Add("@p", SqlDbType.Decimal);
        p.Precision = 10;
        p.Scale = 2;
        p.Value = price;
        cmd.Parameters.Add("@m", SqlDbType.Int).Value = minutes;
    }
}