using System.Data;
using Microsoft.Data.SqlClient;

namespace MyApp;

/// <summary>Everything that talks to the Users table.</summary>
internal static class Auth
{
    /// <summary>Returns the account when the email and password are right, otherwise null. Throws SqlException if the database can't be reached.</summary>
    public static UserAccount? SignIn(string email, string password)
    {
        using var conn = new SqlConnection(Database.ConnectionString);
        conn.Open();

        using var cmd = new SqlCommand(
            "SELECT FullName, Email, PasswordHash, Role FROM dbo.Users WHERE Email = @email AND IsActive = 1", conn);
        cmd.Parameters.Add("@email", SqlDbType.NVarChar, 256).Value = email.Trim();

        using var reader = cmd.ExecuteReader();
        if (!reader.Read()) return null;

        if (!PasswordHasher.Verify(password, reader.GetString(2))) return null;
        if (!Enum.TryParse<UserRole>(reader.GetString(3), out var role)) return null;

        return new UserAccount(reader.GetString(0), reader.GetString(1), role);
    }

    /// <summary>True when at least one active admin exists.</summary>
    public static bool AnyAdminExists()
    {
        using var conn = new SqlConnection(Database.ConnectionString);
        conn.Open();

        using var cmd = new SqlCommand("SELECT COUNT(1) FROM dbo.Users WHERE Role = 'Admin' AND IsActive = 1", conn);
        return (int)cmd.ExecuteScalar()! > 0;
    }

    /// <summary>Adds an account. Throws SqlException (number 2627 or 2601) if the email is already used.</summary>
    public static void CreateUser(string name, string email, string password, UserRole role)
    {
        using var conn = new SqlConnection(Database.ConnectionString);
        conn.Open();

        using var cmd = new SqlCommand(
            "INSERT INTO dbo.Users (FullName, Email, PasswordHash, Role) VALUES (@name, @email, @hash, @role)", conn);
        cmd.Parameters.Add("@name", SqlDbType.NVarChar, 100).Value = name.Trim();
        cmd.Parameters.Add("@email", SqlDbType.NVarChar, 256).Value = email.Trim();
        cmd.Parameters.Add("@hash", SqlDbType.NVarChar, 200).Value = PasswordHasher.Hash(password);
        cmd.Parameters.Add("@role", SqlDbType.NVarChar, 10).Value = role.ToString();
        cmd.ExecuteNonQuery();
    }
}
