using System.Data;
using Microsoft.Data.SqlClient;

namespace MyApp;

internal static class UserRepository
{
    public sealed record UserRow(int Id, string Name, string Email, string Role, bool Active, DateTime CreatedAt, DateTime? LastLogin = null);   // CreatedAt and LastLogin are UTC

    public static List<UserRow> GetAll()
    {
        var list = new List<UserRow>();

        using var conn = new SqlConnection(Database.ConnectionString);
        conn.Open();

        string lastLogin = EnsureLastLoginColumn(conn) ? "LastLoginAt" : "CAST(NULL AS DATETIME2)";
        using var cmd = new SqlCommand(
            $"SELECT UserId, FullName, Email, Role, IsActive, CreatedAt, {lastLogin} FROM dbo.Users ORDER BY FullName", conn);
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
            list.Add(new UserRow(reader.GetInt32(0), reader.GetString(1), reader.GetString(2),
                                 reader.GetString(3), reader.GetBoolean(4), reader.GetDateTime(5),
                                 reader.IsDBNull(6) ? null : reader.GetDateTime(6)));
        return list;
    }

    private static bool _lastLoginChecked;

    /// <summary>Adds Users.LastLoginAt the first time it is needed. Returns false if the column can't be created.</summary>
    private static bool EnsureLastLoginColumn(SqlConnection conn)
    {
        if (_lastLoginChecked) return true;
        try
        {
            using var cmd = new SqlCommand(
                @"IF COL_LENGTH('dbo.Users', 'LastLoginAt') IS NULL
                      ALTER TABLE dbo.Users ADD LastLoginAt DATETIME2 NULL;", conn);
            cmd.ExecuteNonQuery();
            _lastLoginChecked = true;
            return true;
        }
        catch (SqlException)
        {
            return false;
        }
    }

    /// <summary>Stamps the time of a successful sign-in. Never throws: a problem here must not stop anyone from signing in.</summary>
    public static void RecordLogin(SqlConnection conn, string email)
    {
        try
        {
            if (!EnsureLastLoginColumn(conn)) return;
            using var cmd = new SqlCommand("UPDATE dbo.Users SET LastLoginAt = SYSUTCDATETIME() WHERE Email = @email", conn);
            cmd.Parameters.Add("@email", SqlDbType.NVarChar, 256).Value = email.Trim();
            cmd.ExecuteNonQuery();
        }
        catch (SqlException) { }
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

    public enum ProfileResult { Saved, WrongPassword, NotFound }

    /// <summary>Loads one account by email (used to refresh the signed-in user's profile).</summary>
    public static UserRow? GetByEmail(string email)
    {
        using var conn = new SqlConnection(Database.ConnectionString);
        conn.Open();

        using var cmd = new SqlCommand(
            "SELECT UserId, FullName, Email, Role, IsActive, CreatedAt FROM dbo.Users WHERE Email = @email", conn);
        cmd.Parameters.Add("@email", SqlDbType.NVarChar, 256).Value = email.Trim();
        using var reader = cmd.ExecuteReader();
        if (!reader.Read()) return null;
        return new UserRow(reader.GetInt32(0), reader.GetString(1), reader.GetString(2),
                           reader.GetString(3), reader.GetBoolean(4), reader.GetDateTime(5));
    }

    /// <summary>
    /// Lets a signed-in person edit their OWN name and email, and optionally change their password.
    /// The role is never touched here. A password change needs the current password.
    /// Throws SqlException (number 2627 or 2601) if the new email belongs to someone else.
    /// </summary>
    public static ProfileResult UpdateOwnProfile(string currentEmail, string name, string email,
                                                 string? currentPassword, string? newPassword)
    {
        using var conn = new SqlConnection(Database.ConnectionString);
        conn.Open();

        int userId;
        string storedHash;
        using (var find = new SqlCommand(
            "SELECT UserId, PasswordHash FROM dbo.Users WHERE Email = @email AND IsActive = 1", conn))
        {
            find.Parameters.Add("@email", SqlDbType.NVarChar, 256).Value = currentEmail.Trim();
            using var r = find.ExecuteReader();
            if (!r.Read()) return ProfileResult.NotFound;
            userId = r.GetInt32(0);
            storedHash = r.GetString(1);
        }

        bool changePassword = !string.IsNullOrEmpty(newPassword);
        if (changePassword && !PasswordHasher.Verify(currentPassword ?? "", storedHash))
            return ProfileResult.WrongPassword;

        using var cmd = new SqlCommand(
            @"UPDATE dbo.Users
              SET FullName = @name, Email = @email,
                  PasswordHash = CASE WHEN @hash IS NULL THEN PasswordHash ELSE @hash END
              WHERE UserId = @id", conn);
        cmd.Parameters.Add("@name", SqlDbType.NVarChar, 100).Value = name.Trim();
        cmd.Parameters.Add("@email", SqlDbType.NVarChar, 256).Value = email.Trim();
        cmd.Parameters.Add("@hash", SqlDbType.NVarChar, 200).Value =
            changePassword ? PasswordHasher.Hash(newPassword!) : DBNull.Value;
        cmd.Parameters.Add("@id", SqlDbType.Int).Value = userId;
        cmd.ExecuteNonQuery();
        return ProfileResult.Saved;
    }

    private static bool _avatarColumnChecked;

    /// <summary>Adds the AvatarImage column the first time it is needed, so no manual SQL step is required.</summary>
    private static void EnsureAvatarColumn(SqlConnection conn)
    {
        if (_avatarColumnChecked) return;
        using var cmd = new SqlCommand(
            @"IF COL_LENGTH('dbo.Users', 'AvatarImage') IS NULL
                  ALTER TABLE dbo.Users ADD AvatarImage VARBINARY(MAX) NULL;", conn);
        cmd.ExecuteNonQuery();
        _avatarColumnChecked = true;
    }

    /// <summary>The account's profile picture (PNG bytes), or null when none is set.</summary>
    public static byte[]? GetAvatar(string email)
    {
        using var conn = new SqlConnection(Database.ConnectionString);
        conn.Open();
        EnsureAvatarColumn(conn);

        using var cmd = new SqlCommand("SELECT AvatarImage FROM dbo.Users WHERE Email = @email", conn);
        cmd.Parameters.Add("@email", SqlDbType.NVarChar, 256).Value = email.Trim();
        return cmd.ExecuteScalar() as byte[];
    }

    /// <summary>Saves a profile picture, or removes it when <paramref name="png"/> is null.</summary>
    public static void SetAvatar(string email, byte[]? png)
    {
        using var conn = new SqlConnection(Database.ConnectionString);
        conn.Open();
        EnsureAvatarColumn(conn);

        using var cmd = new SqlCommand("UPDATE dbo.Users SET AvatarImage = @img WHERE Email = @email", conn);
        cmd.Parameters.Add("@img", SqlDbType.VarBinary, -1).Value = (object?)png ?? DBNull.Value;
        cmd.Parameters.Add("@email", SqlDbType.NVarChar, 256).Value = email.Trim();
        cmd.ExecuteNonQuery();
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

        using var cmd = new SqlCommand(@"
UPDATE dbo.Users SET IsActive = @a WHERE UserId = @id;
IF OBJECT_ID(N'dbo.Employees', N'U') IS NOT NULL
    UPDATE dbo.Employees SET IsActive = @a WHERE Email = (SELECT Email FROM dbo.Users WHERE UserId = @id) AND GroomerId IS NULL;", conn);
        cmd.Parameters.Add("@a", SqlDbType.Bit).Value = active;
        cmd.Parameters.Add("@id", SqlDbType.Int).Value = userId;
        cmd.ExecuteNonQuery();
        return true;
    }

    public enum RemoveResult { Removed, NotFound, LastAdmin, Self }

    /// <summary>
    /// Permanently deletes an account. Refuses to delete the last active admin, or the account that is signed in.
    /// Appointments, sales and customer records that pointed at the account are kept, just no longer linked to it.
    /// </summary>
    public static RemoveResult Remove(int userId, string? signedInEmail)
    {
        using var conn = new SqlConnection(Database.ConnectionString);
        conn.Open();
        using var tx = conn.BeginTransaction();
        var result = Remove(conn, tx, userId, signedInEmail);
        if (result == RemoveResult.Removed) tx.Commit(); else tx.Rollback();
        return result;
    }

    /// <summary>Same as <see cref="Remove(int,string?)"/>, inside a transaction the caller owns (the caller commits).</summary>
    internal static RemoveResult Remove(SqlConnection conn, SqlTransaction tx, int userId, string? signedInEmail)
    {
        string email, role;
        bool active;
        using (var find = new SqlCommand("SELECT Email, Role, IsActive FROM dbo.Users WHERE UserId = @id", conn, tx))
        {
            find.Parameters.Add("@id", SqlDbType.Int).Value = userId;
            using var r = find.ExecuteReader();
            if (!r.Read()) return RemoveResult.NotFound;
            email = r.GetString(0);
            role = r.GetString(1);
            active = r.GetBoolean(2);
        }

        if (signedInEmail is not null && string.Equals(email, signedInEmail.Trim(), StringComparison.OrdinalIgnoreCase))
            return RemoveResult.Self;

        if (role == "Admin" && active)
        {
            using var check = new SqlCommand(
                "SELECT COUNT(1) FROM dbo.Users WHERE Role = 'Admin' AND IsActive = 1 AND UserId <> @id", conn, tx);
            check.Parameters.Add("@id", SqlDbType.Int).Value = userId;
            if ((int)check.ExecuteScalar()! == 0) return RemoveResult.LastAdmin;
        }

        // Keep the history, drop the link to the deleted login.
        using (var unlink = new SqlCommand(@"
UPDATE dbo.Appointments  SET CreatedByUserId = NULL WHERE CreatedByUserId = @id;
UPDATE dbo.Transactions  SET CashierUserId   = NULL WHERE CashierUserId   = @id;
UPDATE dbo.Customers     SET UserId          = NULL WHERE UserId          = @id;
IF OBJECT_ID(N'dbo.Employees', N'U') IS NOT NULL
    DELETE FROM dbo.Employees WHERE Email = @email AND GroomerId IS NULL;
DELETE FROM dbo.Users WHERE UserId = @id;", conn, tx))
        {
            unlink.Parameters.Add("@id", SqlDbType.Int).Value = userId;
            unlink.Parameters.Add("@email", SqlDbType.NVarChar, 256).Value = email;
            unlink.ExecuteNonQuery();
        }
        return RemoveResult.Removed;
    }

    private static bool IsAdmin(SqlConnection conn, int userId)
    {
        using var cmd = new SqlCommand("SELECT COUNT(1) FROM dbo.Users WHERE UserId = @id AND Role = 'Admin'", conn);
        cmd.Parameters.Add("@id", SqlDbType.Int).Value = userId;
        return (int)cmd.ExecuteScalar()! > 0;
    }
}