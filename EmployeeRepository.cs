using System.Data;
using Microsoft.Data.SqlClient;

namespace MyApp;

/// <summary>
/// The staff roster (dbo.Employees): who works at the shop, in what role. This is separate from logins (dbo.Users).
/// An employee whose role is Groomer is also kept in dbo.Groomers, so they show up in the appointment screens.
/// </summary>
internal static class EmployeeRepository
{
    public static readonly string[] Roles = { "Admin", "Cashier", "Front Desk", "Groomer" };

    /// <summary>The department a role usually belongs to (pre-filled in the dialog, can be changed).</summary>
    public static string DefaultDepartment(string role) => role switch
    {
        "Admin" => "Administration",
        "Cashier" => "Billing",
        "Front Desk" => "Reception",
        "Groomer" => "Grooming",
        _ => "",
    };

    public sealed record Row(int Id, string Name, string Role, string Department, string Email, string Phone,
                             bool Active, DateTime CreatedAt, int? GroomerId);

    private static bool _tableChecked;

    /// <summary>Creates dbo.Employees (and dbo.Groomers if it is missing) the first time it is needed.</summary>
    private static void EnsureTable(SqlConnection conn)
    {
        if (_tableChecked) return;
        using var cmd = new SqlCommand(@"
IF OBJECT_ID(N'dbo.Employees', N'U') IS NULL
    CREATE TABLE dbo.Employees
    (
        EmployeeId INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Employees PRIMARY KEY,
        FullName   NVARCHAR(100) NOT NULL,
        Role       NVARCHAR(20)  NOT NULL CONSTRAINT CK_Employees_Role CHECK (Role IN (N'Admin', N'Cashier', N'Front Desk', N'Groomer')),
        Department NVARCHAR(60)  NOT NULL CONSTRAINT DF_Employees_Department DEFAULT N'',
        Email      NVARCHAR(256) NOT NULL CONSTRAINT DF_Employees_Email DEFAULT N'',
        Phone      NVARCHAR(30)  NOT NULL CONSTRAINT DF_Employees_Phone DEFAULT N'',
        GroomerId  INT           NULL,
        IsActive   BIT           NOT NULL CONSTRAINT DF_Employees_IsActive DEFAULT 1,
        CreatedAt  DATETIME2     NOT NULL CONSTRAINT DF_Employees_CreatedAt DEFAULT SYSDATETIME()
    );
IF OBJECT_ID(N'dbo.Groomers', N'U') IS NULL
    CREATE TABLE dbo.Groomers
    (
        GroomerId INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Groomers PRIMARY KEY,
        Name      NVARCHAR(80) NOT NULL,
        IsActive  BIT          NOT NULL CONSTRAINT DF_Groomers_IsActive DEFAULT 1
    );", conn);
        cmd.ExecuteNonQuery();
        _tableChecked = true;
    }

    private static SqlConnection Open()
    {
        var conn = new SqlConnection(Database.ConnectionString);
        conn.Open();
        EnsureTable(conn);
        return conn;
    }

    private static string Clip(string text, int max)
    {
        text = text.Trim();
        return text.Length > max ? text[..max] : text;
    }

    /// <summary>
    /// Puts every Admin / Employee login that is not on the roster yet onto it (matched by email), so existing accounts
    /// show up in Employee Management. Admin logins become Admin employees; Employee logins become Cashier (change it with Edit).
    /// </summary>
    private static void SyncFromUsers(SqlConnection conn)
    {
        // Groomers already in the appointment lists (e.g. the sample Groomer 1, 2, 3) become Groomer employees linked to that row.
        using (var groomers = new SqlCommand(@"
INSERT INTO dbo.Employees (FullName, Role, Department, GroomerId, IsActive)
SELECT g.Name, N'Groomer', N'Grooming', g.GroomerId, g.IsActive
FROM dbo.Groomers g
WHERE NOT EXISTS (SELECT 1 FROM dbo.Employees e WHERE e.GroomerId = g.GroomerId);", conn))
            groomers.ExecuteNonQuery();

        using var cmd = new SqlCommand(@"
INSERT INTO dbo.Employees (FullName, Role, Department, Email, IsActive)
SELECT u.FullName,
       CASE WHEN u.Role = N'Admin' THEN N'Admin' ELSE N'Cashier' END,
       CASE WHEN u.Role = N'Admin' THEN N'Administration' ELSE N'Billing' END,
       u.Email, u.IsActive
FROM dbo.Users u
WHERE u.Role IN (N'Admin', N'Employee')
  AND NOT EXISTS (SELECT 1 FROM dbo.Employees e WHERE e.Email = u.Email);", conn);
        cmd.ExecuteNonQuery();
    }

    public static List<Row> GetAll()
    {
        using var conn = Open();
        try { SyncFromUsers(conn); }
        catch (SqlException) { }   // a problem copying logins/groomers must never hide the roster itself
        using var cmd = new SqlCommand(@"
SELECT EmployeeId, FullName, Role, Department, Email, Phone, IsActive, CreatedAt, GroomerId
FROM dbo.Employees ORDER BY FullName, EmployeeId", conn);
        using var r = cmd.ExecuteReader();
        var list = new List<Row>();
        while (r.Read())
            list.Add(new Row(r.GetInt32(0), r.GetString(1), r.GetString(2), r.GetString(3), r.GetString(4), r.GetString(5),
                             r.GetBoolean(6), r.GetDateTime(7), r.IsDBNull(8) ? null : r.GetInt32(8)));
        return list;
    }

    /// <summary>Active employees (the dashboard "Staff" number). With <paramref name="addedBefore"/>, only those added before that local date.</summary>
    public static int CountActive(DateTime? addedBefore = null)
    {
        using var conn = Open();
        using var cmd = new SqlCommand(
            "SELECT COUNT(1) FROM dbo.Employees WHERE IsActive = 1" + (addedBefore is null ? "" : " AND CreatedAt < @d"), conn);
        if (addedBefore is { } d) cmd.Parameters.Add("@d", SqlDbType.DateTime2).Value = d;
        return (int)cmd.ExecuteScalar()!;
    }

    public static int Add(string name, string role, string department, string email, string phone)
    {
        using var conn = Open();
        using var tx = conn.BeginTransaction();

        int? groomerId = null;
        if (role == "Groomer")
            groomerId = NewGroomer(conn, tx, name, active: true);

        using var cmd = new SqlCommand(@"
INSERT INTO dbo.Employees (FullName, Role, Department, Email, Phone, GroomerId)
OUTPUT INSERTED.EmployeeId
VALUES (@n, @r, @d, @e, @p, @g)", conn, tx);
        cmd.Parameters.Add("@n", SqlDbType.NVarChar, 100).Value = Clip(name, 100);
        cmd.Parameters.Add("@r", SqlDbType.NVarChar, 20).Value = role;
        cmd.Parameters.Add("@d", SqlDbType.NVarChar, 60).Value = Clip(department, 60);
        cmd.Parameters.Add("@e", SqlDbType.NVarChar, 256).Value = Clip(email, 256);
        cmd.Parameters.Add("@p", SqlDbType.NVarChar, 30).Value = Clip(phone, 30);
        cmd.Parameters.Add("@g", SqlDbType.Int).Value = (object?)groomerId ?? DBNull.Value;
        int id = (int)cmd.ExecuteScalar()!;
        tx.Commit();
        return id;
    }

    /// <summary>Returns false when the employee no longer exists.</summary>
    public static bool Update(int id, string name, string role, string department, string email, string phone)
    {
        using var conn = Open();
        using var tx = conn.BeginTransaction();

        int? groomerId;
        bool active;
        using (var find = new SqlCommand("SELECT GroomerId, IsActive FROM dbo.Employees WHERE EmployeeId = @id", conn, tx))
        {
            find.Parameters.Add("@id", SqlDbType.Int).Value = id;
            using var r = find.ExecuteReader();
            if (!r.Read()) return false;
            groomerId = r.IsDBNull(0) ? null : r.GetInt32(0);
            active = r.GetBoolean(1);
        }

        if (role == "Groomer")
        {
            if (groomerId is int g) SetGroomer(conn, tx, g, name, active);       // keep the groomer list in step
            else groomerId = NewGroomer(conn, tx, name, active);
        }
        else if (groomerId is int old)
        {
            SetGroomerActive(conn, tx, old, false);   // no longer a groomer: stop offering them for appointments (old bookings keep the name)
            groomerId = null;
        }

        using var cmd = new SqlCommand(@"
UPDATE dbo.Employees
SET FullName = @n, Role = @r, Department = @d, Email = @e, Phone = @p, GroomerId = @g
WHERE EmployeeId = @id", conn, tx);
        cmd.Parameters.Add("@n", SqlDbType.NVarChar, 100).Value = Clip(name, 100);
        cmd.Parameters.Add("@r", SqlDbType.NVarChar, 20).Value = role;
        cmd.Parameters.Add("@d", SqlDbType.NVarChar, 60).Value = Clip(department, 60);
        cmd.Parameters.Add("@e", SqlDbType.NVarChar, 256).Value = Clip(email, 256);
        cmd.Parameters.Add("@p", SqlDbType.NVarChar, 30).Value = Clip(phone, 30);
        cmd.Parameters.Add("@g", SqlDbType.Int).Value = (object?)groomerId ?? DBNull.Value;
        cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
        cmd.ExecuteNonQuery();

        tx.Commit();
        return true;
    }

    /// <summary>Deactivates or reactivates an employee (records are kept). A groomer is also taken off, or put back on, the appointment lists.</summary>
    public static void SetActive(int id, bool active)
    {
        using var conn = Open();
        using var tx = conn.BeginTransaction();

        int? groomerId = null;
        using (var cmd = new SqlCommand(
            "UPDATE dbo.Employees SET IsActive = @a OUTPUT INSERTED.GroomerId WHERE EmployeeId = @id", conn, tx))
        {
            cmd.Parameters.Add("@a", SqlDbType.Bit).Value = active;
            cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
            if (cmd.ExecuteScalar() is int g) groomerId = g;
        }

        if (groomerId is int gid) SetGroomerActive(conn, tx, gid, active);
        tx.Commit();
    }

    public enum RemoveResult { Removed, NotFound, HasAppointments, LastAdmin, Self }

    /// <summary>
    /// Permanently deletes an employee. A groomer's entry in the appointment lists goes too, and so does the employee's
    /// login account (matched by email) - otherwise the roster would just copy it back the next time it loads.
    /// A groomer who already has appointments can't be removed, because those bookings still point at them.
    /// </summary>
    public static RemoveResult Remove(int id, string? signedInEmail)
    {
        using var conn = Open();
        using var tx = conn.BeginTransaction();

        int? groomerId;
        string email;
        using (var find = new SqlCommand("SELECT GroomerId, Email FROM dbo.Employees WHERE EmployeeId = @id", conn, tx))
        {
            find.Parameters.Add("@id", SqlDbType.Int).Value = id;
            using var r = find.ExecuteReader();
            if (!r.Read()) return RemoveResult.NotFound;
            groomerId = r.IsDBNull(0) ? null : r.GetInt32(0);
            email = r.GetString(1);
        }

        if (groomerId is int g)
        {
            using var used = new SqlCommand("SELECT COUNT(1) FROM dbo.Appointments WHERE GroomerId = @g", conn, tx);
            used.Parameters.Add("@g", SqlDbType.Int).Value = g;
            if ((int)used.ExecuteScalar()! > 0) return RemoveResult.HasAppointments;
        }

        // The login goes first so its safety checks (last admin, yourself) can stop the whole removal.
        if (email.Length > 0)
        {
            int? userId = null;
            using (var login = new SqlCommand(
                "SELECT UserId FROM dbo.Users WHERE Email = @e AND Role IN (N'Admin', N'Employee')", conn, tx))
            {
                login.Parameters.Add("@e", SqlDbType.NVarChar, 256).Value = email;
                if (login.ExecuteScalar() is int u) userId = u;
            }
            if (userId is int uid)
            {
                var res = UserRepository.Remove(conn, tx, uid, signedInEmail);
                if (res == UserRepository.RemoveResult.LastAdmin) return RemoveResult.LastAdmin;
                if (res == UserRepository.RemoveResult.Self) return RemoveResult.Self;
            }
        }

        using (var del = new SqlCommand("DELETE FROM dbo.Employees WHERE EmployeeId = @id", conn, tx))
        {
            del.Parameters.Add("@id", SqlDbType.Int).Value = id;
            del.ExecuteNonQuery();
        }
        if (groomerId is int gid)
        {
            using var delG = new SqlCommand("DELETE FROM dbo.Groomers WHERE GroomerId = @g", conn, tx);
            delG.Parameters.Add("@g", SqlDbType.Int).Value = gid;
            delG.ExecuteNonQuery();
        }

        tx.Commit();
        return RemoveResult.Removed;
    }

    // ------------------------------------------------------------------ the matching dbo.Groomers row

    private static int NewGroomer(SqlConnection conn, SqlTransaction tx, string name, bool active)
    {
        using var cmd = new SqlCommand(
            "INSERT INTO dbo.Groomers (Name, IsActive) OUTPUT INSERTED.GroomerId VALUES (@n, @a)", conn, tx);
        cmd.Parameters.Add("@n", SqlDbType.NVarChar, 80).Value = Clip(name, 80);
        cmd.Parameters.Add("@a", SqlDbType.Bit).Value = active;
        return (int)cmd.ExecuteScalar()!;
    }

    private static void SetGroomer(SqlConnection conn, SqlTransaction tx, int groomerId, string name, bool active)
    {
        using var cmd = new SqlCommand("UPDATE dbo.Groomers SET Name = @n, IsActive = @a WHERE GroomerId = @id", conn, tx);
        cmd.Parameters.Add("@n", SqlDbType.NVarChar, 80).Value = Clip(name, 80);
        cmd.Parameters.Add("@a", SqlDbType.Bit).Value = active;
        cmd.Parameters.Add("@id", SqlDbType.Int).Value = groomerId;
        cmd.ExecuteNonQuery();
    }

    private static void SetGroomerActive(SqlConnection conn, SqlTransaction tx, int groomerId, bool active)
    {
        using var cmd = new SqlCommand("UPDATE dbo.Groomers SET IsActive = @a WHERE GroomerId = @id", conn, tx);
        cmd.Parameters.Add("@a", SqlDbType.Bit).Value = active;
        cmd.Parameters.Add("@id", SqlDbType.Int).Value = groomerId;
        cmd.ExecuteNonQuery();
    }
}
