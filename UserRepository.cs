using System.Data;
using Microsoft.Data.SqlClient;

namespace MyApp;

internal static class UserRepository
{
    public sealed record UserRow(int Id, string Name, string Email, string Role, bool Active, DateTime CreatedAt);

    public static List<UserRow> GetAll()
    {
        var list = new List<UserRow>();

        using var conn = new SqlConnection(Database.ConnectionString);
        conn.Open();

        using var cmd = new SqlCommand(
            "SELECT UserId, FullName, Email, Role, IsActive, CreatedAt FROM dbo.Users ORDER BY FullName", conn);
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
            list.Add(new UserRow(reader.GetInt32(0), reader.GetString(1), reader.GetString(2),
                                 reader.GetString(3), reader.GetBoolean(4), reader.GetDateTime(5)));
        return list;
    }

    public static bool Update(int userId, string name, string email, UserRole role)
    {
        using var conn = new SqlConnection(Database.ConnectionString);
        conn.Open();

        if (role != UserRole.Admin && IsAdmin(conn, userId))
        {
            using var check = new SqlCommand(
                "SELECT COUNT(1) FROM dbo.Users WHERE Role = 'Admin' AND IsActive = 1 AND UserId <> @id", conn);
            check.Parameters.Add("@id", SqlDbType.Int).Value = userId;
            if ((int)check.ExecuteScalar()! == 0) return false;
        }

        using var cmd = new SqlCommand(
            "UPDATE dbo.Users SET FullName = @name, Email = @email, Role = @role WHERE UserId = @id", conn);
        cmd.Parameters.Add("@name", SqlDbType.NVarChar, 100).Value = name.Trim();
        cmd.Parameters.Add("@email", SqlDbType.NVarChar, 256).Value = email.Trim();
        cmd.Parameters.Add("@role", SqlDbType.NVarChar, 10).Value = role.ToString();
        cmd.Parameters.Add("@id", SqlDbType.Int).Value = userId;
        cmd.ExecuteNonQuery();
        return true;
    }

    /// <summary>Deactivates or reactivates an account. Refuses to deactivate the last active admin.</summary>
    public static bool SetActive(int userId, bool active)
    {
        using var conn = new SqlConnection(Database.ConnectionString);
        conn.Open();

        if (!active)
        {
            using var check = new SqlCommand(
                @"SELECT COUNT(1) FROM dbo.Users
                  WHERE Role = 'Admin' AND IsActive = 1 AND UserId <> @id", conn);
            check.Parameters.Add("@id", SqlDbType.Int).Value = userId;
            bool targetIsAdmin = IsAdmin(conn, userId);
            if (targetIsAdmin && (int)check.ExecuteScalar()! == 0) return false;
        }

        using var cmd = new SqlCommand("UPDATE dbo.Users SET IsActive = @a WHERE UserId = @id", conn);
        cmd.Parameters.Add("@a", SqlDbType.Bit).Value = active;
        cmd.Parameters.Add("@id", SqlDbType.Int).Value = userId;
        cmd.ExecuteNonQuery();
        return true;
    }

    private static bool IsAdmin(SqlConnection conn, int userId)
    {
        using var cmd = new SqlCommand("SELECT COUNT(1) FROM dbo.Users WHERE UserId = @id AND Role = 'Admin'", conn);
        cmd.Parameters.Add("@id", SqlDbType.Int).Value = userId;
        return (int)cmd.ExecuteScalar()! > 0;
    }
}