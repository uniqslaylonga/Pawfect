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

    public static List<Row> GetAll()
    {
        using var conn = Open();
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
