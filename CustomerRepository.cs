using System.Data;
using Microsoft.Data.SqlClient;

namespace MyApp;

/// <summary>Everything the customer section needs from dbo.Customers.</summary>
internal static class CustomerRepository
{
    public sealed record CustomerRow(int Id, string Name, string Phone, string? Email, bool Active, DateTime CreatedAt);

    public static List<CustomerRow> GetAll()
    {
        var list = new List<CustomerRow>();

        using var conn = new SqlConnection(Database.ConnectionString);
        conn.Open();

        using var cmd = new SqlCommand(
            "SELECT CustomerId, FullName, Phone, Email, IsActive, CreatedAt FROM dbo.Customers ORDER BY FullName", conn);
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
            list.Add(new CustomerRow(
                reader.GetInt32(0), reader.GetString(1), reader.GetString(2),
                reader.IsDBNull(3) ? null : reader.GetString(3),
                reader.GetBoolean(4), reader.GetDateTime(5).ToLocalTime()));
        return list;
    }

    public static int CountActive()
    {
        using var conn = new SqlConnection(Database.ConnectionString);
        conn.Open();
        using var cmd = new SqlCommand("SELECT COUNT(1) FROM dbo.Customers WHERE IsActive = 1", conn);
        return (int)cmd.ExecuteScalar()!;
    }

    /// <summary>Throws SqlException 2627/2601 if the phone number is already registered.</summary>
    public static void Add(string name, string phone, string? email)
    {
        using var conn = new SqlConnection(Database.ConnectionString);
        conn.Open();

        using var cmd = new SqlCommand(
            "INSERT INTO dbo.Customers (FullName, Phone, Email) VALUES (@n, @p, @e)", conn);
        Fill(cmd, name, phone, email);
        cmd.ExecuteNonQuery();
    }

    /// <summary>Throws SqlException 2627/2601 if the phone number is taken.</summary>
    public static void Update(int id, string name, string phone, string? email)
    {
        using var conn = new SqlConnection(Database.ConnectionString);
        conn.Open();

        using var cmd = new SqlCommand(
            "UPDATE dbo.Customers SET FullName = @n, Phone = @p, Email = @e WHERE CustomerId = @id", conn);
        Fill(cmd, name, phone, email);
        cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
        cmd.ExecuteNonQuery();
    }

    /// <summary>Soft delete: pets and appointments point at customers.</summary>
    public static void SetActive(int id, bool active)
    {
        using var conn = new SqlConnection(Database.ConnectionString);
        conn.Open();

        using var cmd = new SqlCommand("UPDATE dbo.Customers SET IsActive = @a WHERE CustomerId = @id", conn);
        cmd.Parameters.Add("@a", SqlDbType.Bit).Value = active;
        cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
        cmd.ExecuteNonQuery();
    }

    private static void Fill(SqlCommand cmd, string name, string phone, string? email)
    {
        cmd.Parameters.Add("@n", SqlDbType.NVarChar, 100).Value = name.Trim();
        cmd.Parameters.Add("@p", SqlDbType.NVarChar, 30).Value = phone.Trim();
        cmd.Parameters.Add("@e", SqlDbType.NVarChar, 256).Value =
            string.IsNullOrWhiteSpace(email) ? DBNull.Value : email.Trim();
    }
}