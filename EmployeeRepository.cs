using System.Data;
using Microsoft.Data.SqlClient;

namespace MyApp;

/// <summary>Employees, plus the optional login account (dbo.Users) linked to each one.</summary>
internal static class EmployeeRepository
{
    public static readonly string[] Positions = { "Admin", "Front Desk", "Groomer" };

    public sealed record EmployeeRow(int Id, string Name, string Position, DateTime StartDate,
                                     string Email, bool Active, int? UserId, string? Username);

    private static string RoleFor(string position) => position == "Admin" ? "Admin" : "Employee";

    public static List<EmployeeRow> GetAll()
    {
        var list = new List<EmployeeRow>();

        using var conn = new SqlConnection(Database.ConnectionString);
        conn.Open();

        using var cmd = new SqlCommand(
            @"SELECT e.EmployeeId, e.FullName, e.Position, e.StartDate, e.Email, e.IsActive, e.UserId, u.Username
              FROM dbo.Employees e
              LEFT JOIN dbo.Users u ON u.UserId = e.UserId
              ORDER BY e.FullName", conn);
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
            list.Add(new EmployeeRow(reader.GetInt32(0), reader.GetString(1), reader.GetString(2),
                reader.GetDateTime(3), reader.GetString(4), reader.GetBoolean(5),
                reader.IsDBNull(6) ? null : reader.GetInt32(6),
                reader.IsDBNull(7) ? null : reader.GetString(7)));
        return list;
    }

    /// <summary>
    /// Adds an employee. If a username is given, a login account is created too (password required).
    /// Throws SqlException 2627/2601 if the email or username is already used.
    /// </summary>
    public static void Add(string name, string position, DateTime startDate, string email,
                           string? username, string? password)
    {
        using var conn = new SqlConnection(Database.ConnectionString);
        conn.Open();
        using var tx = conn.BeginTransaction();
        try
        {
            int? userId = null;
            if (!string.IsNullOrWhiteSpace(username))
                userId = CreateUser(conn, tx, name, email, username, password!, position);

            using var cmd = new SqlCommand(
                @"INSERT INTO dbo.Employees (FullName, Position, StartDate, Email, UserId)
                  VALUES (@n, @p, @d, @e, @u)", conn, tx);
            cmd.Parameters.Add("@n", SqlDbType.NVarChar, 100).Value = name.Trim();
            cmd.Parameters.Add("@p", SqlDbType.NVarChar, 20).Value = position;
            cmd.Parameters.Add("@d", SqlDbType.Date).Value = startDate.Date;
            cmd.Parameters.Add("@e", SqlDbType.NVarChar, 256).Value = email.Trim();
            cmd.Parameters.Add("@u", SqlDbType.Int).Value = (object?)userId ?? DBNull.Value;
            cmd.ExecuteNonQuery();

            tx.Commit();
        }
        catch
        {
            tx.Rollback();
            throw;
        }
    }

    /// <summary>
    /// Updates an employee and their login. A blank password keeps the current one; an employee
    /// without an account gets one when a username is given. Throws InvalidOperationException
    /// if it would remove the last active admin, and SqlException 2627/2601 for duplicates.
    /// </summary>
    public static void Update(int id, string name, string position, DateTime startDate, string email,
                              string? username, string? password)
    {
        using var conn = new SqlConnection(Database.ConnectionString);
        conn.Open();
        using var tx = conn.BeginTransaction();
        try
        {
            int? userId;
            using (var cmd = new SqlCommand("SELECT UserId FROM dbo.Employees WHERE EmployeeId = @id", conn, tx))
            {
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
                var o = cmd.ExecuteScalar();
                userId = o == null || o is DBNull ? null : (int)o;
            }

            if (userId == null)
            {
                if (!string.IsNullOrWhiteSpace(username))
                    userId = CreateUser(conn, tx, name, email, username, password!, position);
            }
            else
            {
                if (RoleFor(position) != "Admin" && IsLastActiveAdmin(conn, tx, userId.Value))
                    throw new InvalidOperationException("You can't change the role of the last active admin.");

                bool newPassword = !string.IsNullOrWhiteSpace(password);
                using var cmd = new SqlCommand(
                    @"UPDATE dbo.Users
                      SET FullName = @n, Email = @e, Role = @r, Username = @un"
                    + (newPassword ? ", PasswordHash = @h" : "")
                    + " WHERE UserId = @u", conn, tx);
                cmd.Parameters.Add("@n", SqlDbType.NVarChar, 100).Value = name.Trim();
                cmd.Parameters.Add("@e", SqlDbType.NVarChar, 256).Value = email.Trim();
                cmd.Parameters.Add("@r", SqlDbType.NVarChar, 10).Value = RoleFor(position);
                cmd.Parameters.Add("@un", SqlDbType.NVarChar, 50).Value =
                    string.IsNullOrWhiteSpace(username) ? DBNull.Value : username.Trim();
                if (newPassword)
                    cmd.Parameters.Add("@h", SqlDbType.NVarChar, 200).Value = PasswordHasher.Hash(password!);
                cmd.Parameters.Add("@u", SqlDbType.Int).Value = userId.Value;
                cmd.ExecuteNonQuery();
            }

            using (var cmd = new SqlCommand(
                @"UPDATE dbo.Employees
                  SET FullName = @n, Position = @p, StartDate = @d, Email = @e, UserId = @u
                  WHERE EmployeeId = @id", conn, tx))
            {
                cmd.Parameters.Add("@n", SqlDbType.NVarChar, 100).Value = name.Trim();
                cmd.Parameters.Add("@p", SqlDbType.NVarChar, 20).Value = position;
                cmd.Parameters.Add("@d", SqlDbType.Date).Value = startDate.Date;
                cmd.Parameters.Add("@e", SqlDbType.NVarChar, 256).Value = email.Trim();
                cmd.Parameters.Add("@u", SqlDbType.Int).Value = (object?)userId ?? DBNull.Value;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
                cmd.ExecuteNonQuery();
            }

            tx.Commit();
        }
        catch
        {
            tx.Rollback();
            throw;
        }
    }

    /// <summary>Soft delete. Also blocks or restores the linked login. Returns false for the last active admin.</summary>
    public static bool SetActive(int id, bool active)
    {
        using var conn = new SqlConnection(Database.ConnectionString);
        conn.Open();
        using var tx = conn.BeginTransaction();
        try
        {
            int? userId;
            using (var cmd = new SqlCommand("SELECT UserId FROM dbo.Employees WHERE EmployeeId = @id", conn, tx))
            {
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
                var o = cmd.ExecuteScalar();
                userId = o == null || o is DBNull ? null : (int)o;
            }

            if (!active && userId != null && IsLastActiveAdmin(conn, tx, userId.Value))
            {
                tx.Rollback();
                return false;
            }

            using (var cmd = new SqlCommand("UPDATE dbo.Employees SET IsActive = @a WHERE EmployeeId = @id", conn, tx))
            {
                cmd.Parameters.Add("@a", SqlDbType.Bit).Value = active;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
                cmd.ExecuteNonQuery();
            }

            if (userId != null)
            {
                using var cmd = new SqlCommand("UPDATE dbo.Users SET IsActive = @a WHERE UserId = @u", conn, tx);
                cmd.Parameters.Add("@a", SqlDbType.Bit).Value = active;
                cmd.Parameters.Add("@u", SqlDbType.Int).Value = userId.Value;
                cmd.ExecuteNonQuery();
            }

            tx.Commit();
            return true;
        }
        catch
        {
            tx.Rollback();
            throw;
        }
    }

    private static int CreateUser(SqlConnection conn, SqlTransaction tx, string name, string email,
                                  string username, string password, string position)
    {
        using var cmd = new SqlCommand(
            @"INSERT INTO dbo.Users (FullName, Email, PasswordHash, Role, Username)
              OUTPUT INSERTED.UserId
              VALUES (@n, @e, @h, @r, @un)", conn, tx);
        cmd.Parameters.Add("@n", SqlDbType.NVarChar, 100).Value = name.Trim();
        cmd.Parameters.Add("@e", SqlDbType.NVarChar, 256).Value = email.Trim();
        cmd.Parameters.Add("@h", SqlDbType.NVarChar, 200).Value = PasswordHasher.Hash(password);
        cmd.Parameters.Add("@r", SqlDbType.NVarChar, 10).Value = RoleFor(position);
        cmd.Parameters.Add("@un", SqlDbType.NVarChar, 50).Value = username.Trim();
        return (int)cmd.ExecuteScalar()!;
    }

    /// <summary>True when this user is an active admin and no other active admin exists.</summary>
    private static bool IsLastActiveAdmin(SqlConnection conn, SqlTransaction tx, int userId)
    {
        using var cmd = new SqlCommand(
            @"SELECT CASE WHEN EXISTS (SELECT 1 FROM dbo.Users WHERE UserId = @u AND Role = 'Admin' AND IsActive = 1)
                          AND NOT EXISTS (SELECT 1 FROM dbo.Users WHERE UserId <> @u AND Role = 'Admin' AND IsActive = 1)
                     THEN 1 ELSE 0 END", conn, tx);
        cmd.Parameters.Add("@u", SqlDbType.Int).Value = userId;
        return (int)cmd.ExecuteScalar()! == 1;
    }
}