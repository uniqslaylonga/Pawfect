using System.Data;
using Microsoft.Data.SqlClient;

namespace MyApp;

/// <summary>A customer as shown in Customer &amp; Pet Lookup.</summary>
internal sealed record CustomerRecord(
    int Id, string Name, string Phone, string Email, string Address,
    DateTime CreatedAt, int PetCount, string PetNames, DateTime? LastVisit);

/// <summary>A pet and its owner.</summary>
internal sealed record PetRecord(
    int Id, int CustomerId, string Name, string Species, string Breed, string OwnerName, string OwnerPhone);

/// <summary>The numbers on the staff dashboard (all for today unless noted).</summary>
internal sealed record DashboardStats(
    int AppointmentsToday, int AppointmentsYesterday,
    int WalkInsToday, int WalkInsYesterday,
    int ServicesCompletedToday, int ServicesCompletedYesterday,
    int PendingQueue,
    int TotalCustomers,
    decimal RevenueToday,
    IReadOnlyList<(string Category, int Count)> ServicesByCategory);

/// <summary>Every database call the employee screens make. All of them throw SqlException if the database can't be reached.</summary>
internal static class EmployeeData
{
    private static SqlConnection Open()
    {
        var conn = new SqlConnection(Database.ConnectionString);
        conn.Open();
        return conn;
    }

    private static SqlCommand Cmd(SqlConnection conn, string sql, SqlTransaction? tx = null)
        => new(sql, conn, tx);

    private static SqlParameter P(SqlCommand cmd, string name, SqlDbType type, object? value, int size = 0)
    {
        var p = size > 0 ? cmd.Parameters.Add(name, type, size) : cmd.Parameters.Add(name, type);
        if (type == SqlDbType.Decimal) { p.Precision = 10; p.Scale = 2; }   // money columns are DECIMAL(10,2)
        p.Value = value ?? DBNull.Value;
        return p;
    }

    // ------------------------------------------------------------------ customers & pets

    public static List<CustomerRecord> GetCustomers()
    {
        const string sql = @"
SELECT c.CustomerId, c.FullName, c.Phone, c.Email, c.Address, c.CreatedAt,
       (SELECT COUNT(*) FROM dbo.Pets p WHERE p.CustomerId = c.CustomerId AND p.IsActive = 1) AS PetCount,
       ISNULL(STUFF((SELECT N', ' + p.Name FROM dbo.Pets p
                     WHERE p.CustomerId = c.CustomerId AND p.IsActive = 1 ORDER BY p.Name
                     FOR XML PATH(N''), TYPE).value(N'.', N'nvarchar(max)'), 1, 2, N''), N'') AS PetNames,
       (SELECT MAX(t.CreatedAt) FROM dbo.Transactions t
         WHERE t.CustomerId = c.CustomerId AND t.Status = N'Completed') AS LastVisit
FROM dbo.Customers c
WHERE c.IsActive = 1
ORDER BY c.CreatedAt DESC, c.CustomerId DESC";

        using var conn = Open();
        using var cmd = Cmd(conn, sql);
        using var r = cmd.ExecuteReader();
        var list = new List<CustomerRecord>();
        while (r.Read())
        {
            list.Add(new CustomerRecord(
                r.GetInt32(0), r.GetString(1), r.GetString(2), r.GetString(3), r.GetString(4),
                r.GetDateTime(5), r.GetInt32(6), r.GetString(7),
                r.IsDBNull(8) ? null : r.GetDateTime(8)));
        }
        return list;
    }

    public static List<PetRecord> GetPets()
    {
        const string sql = @"
SELECT p.PetId, p.CustomerId, p.Name, p.Species, p.Breed, c.FullName, c.Phone
FROM dbo.Pets p JOIN dbo.Customers c ON c.CustomerId = p.CustomerId
WHERE p.IsActive = 1 AND c.IsActive = 1
ORDER BY p.CreatedAt DESC, p.PetId DESC";

        using var conn = Open();
        using var cmd = Cmd(conn, sql);
        using var r = cmd.ExecuteReader();
        var list = new List<PetRecord>();
        while (r.Read())
            list.Add(new PetRecord(r.GetInt32(0), r.GetInt32(1), r.GetString(2), r.GetString(3), r.GetString(4), r.GetString(5), r.GetString(6)));
        return list;
    }

    /// <summary>Adds a customer (and optionally their first pet) in one go. Returns the new customer id.</summary>
    public static int AddCustomer(string name, string phone, string email, string address,
        string? petName = null, string petSpecies = "", string petBreed = "")
    {
        using var conn = Open();
        using var tx = conn.BeginTransaction();

        int customerId;
        using (var cmd = Cmd(conn,
            "INSERT INTO dbo.Customers (FullName, Phone, Email, Address) OUTPUT INSERTED.CustomerId VALUES (@n, @p, @e, @a)", tx))
        {
            P(cmd, "@n", SqlDbType.NVarChar, name.Trim(), 100);
            P(cmd, "@p", SqlDbType.NVarChar, phone.Trim(), 30);
            P(cmd, "@e", SqlDbType.NVarChar, email.Trim(), 256);
            P(cmd, "@a", SqlDbType.NVarChar, address.Trim(), 200);
            customerId = (int)cmd.ExecuteScalar()!;
        }

        if (!string.IsNullOrWhiteSpace(petName))
            InsertPet(conn, tx, customerId, petName, petSpecies, petBreed);

        tx.Commit();
        return customerId;
    }

    public static int AddPet(int customerId, string name, string species, string breed)
    {
        using var conn = Open();
        using var tx = conn.BeginTransaction();
        int id = InsertPet(conn, tx, customerId, name, species, breed);
        tx.Commit();
        return id;
    }

    private static int InsertPet(SqlConnection conn, SqlTransaction tx, int customerId, string name, string species, string breed)
    {
        using var cmd = Cmd(conn,
            "INSERT INTO dbo.Pets (CustomerId, Name, Species, Breed) OUTPUT INSERTED.PetId VALUES (@c, @n, @s, @b)", tx);
        P(cmd, "@c", SqlDbType.Int, customerId);
        P(cmd, "@n", SqlDbType.NVarChar, name.Trim(), 60);
        P(cmd, "@s", SqlDbType.NVarChar, species.Trim(), 30);
        P(cmd, "@b", SqlDbType.NVarChar, breed.Trim(), 60);
        return (int)cmd.ExecuteScalar()!;
    }

    // ------------------------------------------------------------------ catalog (read only here)

    public static List<ServiceOffering> GetServices()
    {
        using var conn = Open();
        using var cmd = Cmd(conn, "SELECT ServiceId, Name, Price FROM dbo.Services WHERE IsActive = 1 ORDER BY Name");
        using var r = cmd.ExecuteReader();
        var list = new List<ServiceOffering>();
        while (r.Read()) list.Add(new ServiceOffering(r.GetInt32(0), r.GetString(1), r.GetDecimal(2)));
        return list;
    }

    public static List<ProductItem> GetProducts()
    {
        using var conn = Open();
        using var cmd = Cmd(conn, "SELECT ProductId, Code, Name, Category, Price FROM dbo.Products WHERE IsActive = 1 ORDER BY Name");
        using var r = cmd.ExecuteReader();
        var list = new List<ProductItem>();
        while (r.Read()) list.Add(new ProductItem(r.GetInt32(0), r.GetString(1), r.GetString(2), r.GetString(3), r.GetDecimal(4)));
        return list;
    }

    // ------------------------------------------------------------------ appointments

    public static List<AppointmentItem> GetAppointments()
    {
        const string sql = @"
SELECT a.AppointmentId, a.StartTime, c.FullName, c.Phone, p.Name, p.Breed, s.Name, a.Status
FROM dbo.Appointments a
JOIN dbo.Customers c ON c.CustomerId = a.CustomerId
JOIN dbo.Pets p      ON p.PetId      = a.PetId
JOIN dbo.Services s  ON s.ServiceId  = a.ServiceId
ORDER BY a.StartTime";

        using var conn = Open();
        using var cmd = Cmd(conn, sql);
        using var r = cmd.ExecuteReader();
        var list = new List<AppointmentItem>();
        while (r.Read())
        {
            Enum.TryParse<AppointmentStatus>(r.GetString(7), out var status);
            list.Add(new AppointmentItem(r.GetInt32(0), r.GetDateTime(1), r.GetString(2), r.GetString(3),
                r.GetString(4), r.GetString(5), r.GetString(6), status));
        }
        return list;
    }

    public static int AddAppointment(int customerId, int petId, int serviceId, DateTime start, string createdByEmail)
    {
        const string sql = @"
INSERT INTO dbo.Appointments (CustomerId, PetId, ServiceId, StartTime, Status, CreatedByUserId)
OUTPUT INSERTED.AppointmentId
VALUES (@c, @p, @s, @t, N'Pending', (SELECT UserId FROM dbo.Users WHERE Email = @email))";

        using var conn = Open();
        using var cmd = Cmd(conn, sql);
        P(cmd, "@c", SqlDbType.Int, customerId);
        P(cmd, "@p", SqlDbType.Int, petId);
        P(cmd, "@s", SqlDbType.Int, serviceId);
        P(cmd, "@t", SqlDbType.DateTime2, start);
        P(cmd, "@email", SqlDbType.NVarChar, createdByEmail, 256);
        return (int)cmd.ExecuteScalar()!;
    }

    public static void SetAppointmentStatus(int appointmentId, AppointmentStatus status)
    {
        using var conn = Open();
        using var cmd = Cmd(conn, "UPDATE dbo.Appointments SET Status = @s WHERE AppointmentId = @id");
        P(cmd, "@s", SqlDbType.NVarChar, status.ToString(), 20);
        P(cmd, "@id", SqlDbType.Int, appointmentId);
        cmd.ExecuteNonQuery();
    }

    public static void RescheduleAppointment(int appointmentId, DateTime start)
    {
        using var conn = Open();
        using var cmd = Cmd(conn, "UPDATE dbo.Appointments SET StartTime = @t, Status = N'Rescheduled' WHERE AppointmentId = @id");
        P(cmd, "@t", SqlDbType.DateTime2, start);
        P(cmd, "@id", SqlDbType.Int, appointmentId);
        cmd.ExecuteNonQuery();
    }

    // ------------------------------------------------------------------ customers & pets: edit / delete

    private const string OpenStatuses = "N'Pending', N'Confirmed', N'CheckedIn', N'Rescheduled'";

    public static void UpdateCustomer(int customerId, string name, string phone, string email, string address)
    {
        using var conn = Open();
        using var cmd = Cmd(conn,
            "UPDATE dbo.Customers SET FullName = @n, Phone = @p, Email = @e, Address = @a WHERE CustomerId = @id AND IsActive = 1");
        P(cmd, "@n", SqlDbType.NVarChar, name.Trim(), 100);
        P(cmd, "@p", SqlDbType.NVarChar, phone.Trim(), 30);
        P(cmd, "@e", SqlDbType.NVarChar, email.Trim(), 256);
        P(cmd, "@a", SqlDbType.NVarChar, address.Trim(), 200);
        P(cmd, "@id", SqlDbType.Int, customerId);
        if (cmd.ExecuteNonQuery() == 0)
            throw new InvalidOperationException("That customer no longer exists.");
    }

    /// <summary>
    /// Removes a customer (and their pets) from every list. The rows stay in the database so old appointments and
    /// receipts keep their names. Returns null on success, otherwise the reason it was refused.
    /// </summary>
    public static string? DeleteCustomer(int customerId)
    {
        using var conn = Open();
        using var tx = conn.BeginTransaction();

        using (var check = Cmd(conn,
            "SELECT COUNT(*) FROM dbo.Appointments WHERE CustomerId = @c AND Status IN (" + OpenStatuses + ")", tx))
        {
            P(check, "@c", SqlDbType.Int, customerId);
            if ((int)check.ExecuteScalar()! > 0)
                return "This customer still has open appointments. Cancel or complete them first.";
        }

        using (var cmd = Cmd(conn, "UPDATE dbo.Pets SET IsActive = 0 WHERE CustomerId = @c", tx))
        {
            P(cmd, "@c", SqlDbType.Int, customerId);
            cmd.ExecuteNonQuery();
        }
        using (var cmd = Cmd(conn, "UPDATE dbo.Customers SET IsActive = 0 WHERE CustomerId = @c", tx))
        {
            P(cmd, "@c", SqlDbType.Int, customerId);
            cmd.ExecuteNonQuery();
        }

        tx.Commit();
        return null;
    }

    public static void UpdatePet(int petId, string name, string species, string breed)
    {
        using var conn = Open();
        using var cmd = Cmd(conn,
            "UPDATE dbo.Pets SET Name = @n, Species = @s, Breed = @b WHERE PetId = @id AND IsActive = 1");
        P(cmd, "@n", SqlDbType.NVarChar, name.Trim(), 60);
        P(cmd, "@s", SqlDbType.NVarChar, species.Trim(), 30);
        P(cmd, "@b", SqlDbType.NVarChar, breed.Trim(), 60);
        P(cmd, "@id", SqlDbType.Int, petId);
        if (cmd.ExecuteNonQuery() == 0)
            throw new InvalidOperationException("That pet no longer exists.");
    }

    /// <summary>Removes a pet from every list (history is kept). Returns null on success, otherwise the reason it was refused.</summary>
    public static string? DeletePet(int petId)
    {
        using var conn = Open();
        using var tx = conn.BeginTransaction();

        using (var check = Cmd(conn,
            "SELECT COUNT(*) FROM dbo.Appointments WHERE PetId = @p AND Status IN (" + OpenStatuses + ")", tx))
        {
            P(check, "@p", SqlDbType.Int, petId);
            if ((int)check.ExecuteScalar()! > 0)
                return "This pet still has open appointments. Cancel or complete them first.";
        }

        using (var cmd = Cmd(conn, "UPDATE dbo.Pets SET IsActive = 0 WHERE PetId = @p", tx))
        {
            P(cmd, "@p", SqlDbType.Int, petId);
            cmd.ExecuteNonQuery();
        }

        tx.Commit();
        return null;
    }

    // ------------------------------------------------------------------ appointments: edit / delete

    /// <summary>The customer, pet and service ids behind an appointment (the list items only carry names).</summary>
    public static (int CustomerId, int PetId, int ServiceId)? GetAppointmentRefs(int appointmentId)
    {
        using var conn = Open();
        using var cmd = Cmd(conn, "SELECT CustomerId, PetId, ServiceId FROM dbo.Appointments WHERE AppointmentId = @id");
        P(cmd, "@id", SqlDbType.Int, appointmentId);
        using var r = cmd.ExecuteReader();
        return r.Read() ? (r.GetInt32(0), r.GetInt32(1), r.GetInt32(2)) : null;
    }

    /// <summary>Changes the pet, service and time of an open appointment. Moving it to another time marks it Rescheduled.</summary>
    public static void UpdateAppointment(int appointmentId, int petId, int serviceId, DateTime start)
    {
        const string sql = @"
UPDATE a SET
    PetId = @p,
    ServiceId = @s,
    Status = CASE WHEN a.StartTime <> @t AND a.Status IN (N'Pending', N'Confirmed', N'CheckedIn') THEN N'Rescheduled' ELSE a.Status END,
    StartTime = @t
FROM dbo.Appointments a
WHERE a.AppointmentId = @id
  AND a.Status NOT IN (N'Cancelled', N'Completed')
  AND EXISTS (SELECT 1 FROM dbo.Pets p WHERE p.PetId = @p AND p.CustomerId = a.CustomerId AND p.IsActive = 1)";

        using var conn = Open();
        using var cmd = Cmd(conn, sql);
        P(cmd, "@p", SqlDbType.Int, petId);
        P(cmd, "@s", SqlDbType.Int, serviceId);
        P(cmd, "@t", SqlDbType.DateTime2, start);
        P(cmd, "@id", SqlDbType.Int, appointmentId);
        if (cmd.ExecuteNonQuery() == 0)
            throw new InvalidOperationException("This appointment can't be changed any more (it may be cancelled, completed or deleted).");
    }

    /// <summary>Permanently deletes an appointment. Returns null on success, otherwise the reason it was refused.</summary>
    public static string? DeleteAppointment(int appointmentId)
    {
        const string sql = @"
DELETE FROM dbo.Appointments
WHERE AppointmentId = @id
  AND Status <> N'Completed'
  AND NOT EXISTS (SELECT 1 FROM dbo.Transactions t WHERE t.AppointmentId = @id)";

        using var conn = Open();
        using var cmd = Cmd(conn, sql);
        P(cmd, "@id", SqlDbType.Int, appointmentId);
        return cmd.ExecuteNonQuery() > 0
            ? null
            : "This appointment has a receipt attached (or is already gone), so it can't be deleted.";
    }

    // ------------------------------------------------------------------ sales: view lines, void / refund

    public static List<(string Name, string Detail, int Quantity, decimal UnitPrice)> GetTransactionLines(int transactionId)
    {
        using var conn = Open();
        using var cmd = Cmd(conn,
            "SELECT Name, Detail, Quantity, UnitPrice FROM dbo.TransactionLines WHERE TransactionId = @id ORDER BY LineId");
        P(cmd, "@id", SqlDbType.Int, transactionId);
        using var r = cmd.ExecuteReader();
        var list = new List<(string, string, int, decimal)>();
        while (r.Read()) list.Add((r.GetString(0), r.GetString(1), r.GetInt32(2), r.GetDecimal(3)));
        return list;
    }

    /// <summary>
    /// Voids or refunds a completed sale. Receipts are never deleted so the books stay complete; a voided sale
    /// puts its appointment back to Checked In so it can be billed again. Returns false if the sale wasn't Completed.
    /// </summary>
    public static bool ChangeSaleStatus(int transactionId, TransactionStatus newStatus)
    {
        if (newStatus is not (TransactionStatus.Voided or TransactionStatus.Refunded))
            throw new ArgumentOutOfRangeException(nameof(newStatus));

        using var conn = Open();
        using var tx = conn.BeginTransaction();

        using (var cmd = Cmd(conn,
            "UPDATE dbo.Transactions SET Status = @s WHERE TransactionId = @id AND Status = N'Completed'", tx))
        {
            P(cmd, "@s", SqlDbType.NVarChar, newStatus.ToString(), 20);
            P(cmd, "@id", SqlDbType.Int, transactionId);
            if (cmd.ExecuteNonQuery() == 0) return false;
        }

        if (newStatus == TransactionStatus.Voided)
        {
            using var cmd = Cmd(conn, @"
UPDATE a SET Status = N'CheckedIn'
FROM dbo.Appointments a JOIN dbo.Transactions t ON t.AppointmentId = a.AppointmentId
WHERE t.TransactionId = @id AND a.Status = N'Completed'", tx);
            P(cmd, "@id", SqlDbType.Int, transactionId);
            cmd.ExecuteNonQuery();
        }

        tx.Commit();
        return true;
    }

    // ------------------------------------------------------------------ sales

    /// <summary>The receipt number the next sale will probably get (shown in the preview only).</summary>
    public static string PeekNextReceiptNumber()
    {
        using var conn = Open();
        using var cmd = Cmd(conn, "SELECT ISNULL(MAX(TransactionId), 0) + 1 FROM dbo.Transactions");
        return FormatReceipt((int)cmd.ExecuteScalar()!);
    }

    private static string FormatReceipt(int id) => $"PF-{id:D6}";

    public static List<TransactionItem> GetRecentTransactions(int limit = 50)
    {
        const string sql = @"
SELECT TOP (@limit) t.TransactionId, t.ReceiptNumber, t.CreatedAt, ISNULL(c.FullName, N'Walk-in'),
       (SELECT ISNULL(SUM(l.Quantity), 0) FROM dbo.TransactionLines l WHERE l.TransactionId = t.TransactionId AND l.LineKind = N'Service'),
       (SELECT ISNULL(SUM(l.Quantity), 0) FROM dbo.TransactionLines l WHERE l.TransactionId = t.TransactionId AND l.LineKind = N'Product'),
       t.Total, t.PaymentMethod, t.Status
FROM dbo.Transactions t
LEFT JOIN dbo.Customers c ON c.CustomerId = t.CustomerId
ORDER BY t.CreatedAt DESC, t.TransactionId DESC";

        using var conn = Open();
        using var cmd = Cmd(conn, sql);
        P(cmd, "@limit", SqlDbType.Int, limit);
        using var r = cmd.ExecuteReader();
        var list = new List<TransactionItem>();
        while (r.Read())
        {
            Enum.TryParse<TransactionStatus>(r.GetString(8), out var status);
            list.Add(new TransactionItem(r.GetInt32(0), r.GetString(1), r.GetDateTime(2), r.GetString(3),
                r.GetInt32(4), r.GetInt32(5), r.GetDecimal(6), r.GetString(7), status));
        }
        return list;
    }

    /// <summary>
    /// Saves the sale, its lines and (when it came from an appointment) marks that appointment Completed,
    /// all in one database transaction. Returns the receipt number.
    /// </summary>
    public static string SaveSale(BillingCheckout sale, string cashierEmail)
    {
        using var conn = Open();
        using var tx = conn.BeginTransaction();

        int? appointmentId = sale.Appointment?.Id;

        int transactionId;
        using (var cmd = Cmd(conn, @"
INSERT INTO dbo.Transactions
    (ReceiptNumber, CustomerId, AppointmentId, CashierUserId, Subtotal, Discount, Total,
     PaymentMethod, AmountReceived, ChangeGiven, Notes, Status)
OUTPUT INSERTED.TransactionId
VALUES
    (@temp, (SELECT CustomerId FROM dbo.Appointments WHERE AppointmentId = @appt), @appt,
     (SELECT UserId FROM dbo.Users WHERE Email = @email), @sub, @disc, @total,
     @method, @received, @change, @notes, N'Completed')", tx))
        {
            P(cmd, "@temp", SqlDbType.NVarChar, Guid.NewGuid().ToString("N")[..30], 30);   // replaced right below
            P(cmd, "@appt", SqlDbType.Int, appointmentId);
            P(cmd, "@email", SqlDbType.NVarChar, cashierEmail, 256);
            P(cmd, "@sub", SqlDbType.Decimal, sale.Subtotal);
            P(cmd, "@disc", SqlDbType.Decimal, sale.Discount);
            P(cmd, "@total", SqlDbType.Decimal, sale.Total);
            P(cmd, "@method", SqlDbType.NVarChar, sale.Method == PaymentKind.EWallet ? "E-Wallet" : "Cash", 20);
            P(cmd, "@received", SqlDbType.Decimal, sale.AmountReceived);
            P(cmd, "@change", SqlDbType.Decimal, sale.Change);
            P(cmd, "@notes", SqlDbType.NVarChar, sale.Notes, 500);
            transactionId = (int)cmd.ExecuteScalar()!;
        }

        string receipt = FormatReceipt(transactionId);
        using (var cmd = Cmd(conn, "UPDATE dbo.Transactions SET ReceiptNumber = @r WHERE TransactionId = @id", tx))
        {
            P(cmd, "@r", SqlDbType.NVarChar, receipt, 30);
            P(cmd, "@id", SqlDbType.Int, transactionId);
            cmd.ExecuteNonQuery();
        }

        foreach (var line in sale.Lines)
        {
            using var cmd = Cmd(conn, @"
INSERT INTO dbo.TransactionLines (TransactionId, LineKind, ServiceId, ProductId, Name, Detail, UnitPrice, Quantity)
VALUES (@t, @kind, @service, @product, @name, @detail, @price, @qty)", tx);
            bool isService = line.Kind == CartLineKind.Service;
            P(cmd, "@t", SqlDbType.Int, transactionId);
            P(cmd, "@kind", SqlDbType.NVarChar, isService ? "Service" : "Product", 10);
            P(cmd, "@service", SqlDbType.Int, isService ? line.SourceId : null);
            P(cmd, "@product", SqlDbType.Int, isService ? null : line.SourceId);
            P(cmd, "@name", SqlDbType.NVarChar, line.Name, 100);
            P(cmd, "@detail", SqlDbType.NVarChar, line.Detail, 100);
            P(cmd, "@price", SqlDbType.Decimal, line.UnitPrice);
            P(cmd, "@qty", SqlDbType.Int, line.Quantity);
            cmd.ExecuteNonQuery();
        }

        if (appointmentId is int id)
        {
            using var cmd = Cmd(conn, "UPDATE dbo.Appointments SET Status = N'Completed' WHERE AppointmentId = @id", tx);
            P(cmd, "@id", SqlDbType.Int, id);
            cmd.ExecuteNonQuery();
        }

        tx.Commit();
        return receipt;
    }

    // ------------------------------------------------------------------ dashboard

    public static DashboardStats GetDashboardStats()
    {
        const string sql = @"
DECLARE @today DATE = CAST(SYSDATETIME() AS DATE);
DECLARE @yesterday DATE = DATEADD(DAY, -1, @today);

SELECT
  (SELECT COUNT(*) FROM dbo.Appointments WHERE CAST(StartTime AS DATE) = @today AND Status <> N'Cancelled'),
  (SELECT COUNT(*) FROM dbo.Appointments WHERE CAST(StartTime AS DATE) = @yesterday AND Status <> N'Cancelled'),
  (SELECT COUNT(*) FROM dbo.Transactions WHERE CAST(CreatedAt AS DATE) = @today AND AppointmentId IS NULL AND Status = N'Completed'),
  (SELECT COUNT(*) FROM dbo.Transactions WHERE CAST(CreatedAt AS DATE) = @yesterday AND AppointmentId IS NULL AND Status = N'Completed'),
  (SELECT ISNULL(SUM(l.Quantity), 0) FROM dbo.TransactionLines l JOIN dbo.Transactions t ON t.TransactionId = l.TransactionId
     WHERE l.LineKind = N'Service' AND t.Status = N'Completed' AND CAST(t.CreatedAt AS DATE) = @today),
  (SELECT ISNULL(SUM(l.Quantity), 0) FROM dbo.TransactionLines l JOIN dbo.Transactions t ON t.TransactionId = l.TransactionId
     WHERE l.LineKind = N'Service' AND t.Status = N'Completed' AND CAST(t.CreatedAt AS DATE) = @yesterday),
  (SELECT COUNT(*) FROM dbo.Appointments WHERE Status IN (N'Pending', N'Rescheduled') AND CAST(StartTime AS DATE) = @today),
  (SELECT COUNT(*) FROM dbo.Customers WHERE IsActive = 1),
  (SELECT ISNULL(SUM(Total), 0) FROM dbo.Transactions WHERE Status = N'Completed' AND CAST(CreatedAt AS DATE) = @today);

SELECT s.Category, SUM(l.Quantity)
FROM dbo.TransactionLines l
JOIN dbo.Transactions t ON t.TransactionId = l.TransactionId
JOIN dbo.Services s ON s.ServiceId = l.ServiceId
WHERE l.LineKind = N'Service' AND t.Status = N'Completed' AND CAST(t.CreatedAt AS DATE) = @today
GROUP BY s.Category;";

        using var conn = Open();
        using var cmd = Cmd(conn, sql);
        using var r = cmd.ExecuteReader();
        r.Read();
        int[] n = Enumerable.Range(0, 8).Select(i => Convert.ToInt32(r.GetValue(i))).ToArray();
        decimal revenue = r.GetDecimal(8);

        var byCategory = new List<(string, int)>();
        if (r.NextResult())
            while (r.Read()) byCategory.Add((r.GetString(0), Convert.ToInt32(r.GetValue(1))));

        return new DashboardStats(n[0], n[1], n[2], n[3], n[4], n[5], n[6], n[7], revenue, byCategory);
    }

    // ------------------------------------------------------------------ customer portal (a customer's own data only)

    /// <summary>The customer record linked to this login, or null when the login isn't linked to one.</summary>
    public static (int Id, string Name)? GetCustomerForLogin(string email)
    {
        using var conn = Open();
        using var cmd = Cmd(conn, @"
SELECT c.CustomerId, c.FullName FROM dbo.Customers c
JOIN dbo.Users u ON u.UserId = c.UserId
WHERE u.Email = @email AND u.IsActive = 1 AND c.IsActive = 1");
        P(cmd, "@email", SqlDbType.NVarChar, email, 256);
        using var r = cmd.ExecuteReader();
        return r.Read() ? (r.GetInt32(0), r.GetString(1)) : null;
    }

    public static List<AppointmentItem> GetAppointmentsForCustomer(int customerId)
    {
        const string sql = @"
SELECT a.AppointmentId, a.StartTime, c.FullName, c.Phone, p.Name, p.Breed, s.Name, a.Status
FROM dbo.Appointments a
JOIN dbo.Customers c ON c.CustomerId = a.CustomerId
JOIN dbo.Pets p      ON p.PetId      = a.PetId
JOIN dbo.Services s  ON s.ServiceId  = a.ServiceId
WHERE a.CustomerId = @c
ORDER BY a.StartTime DESC";

        using var conn = Open();
        using var cmd = Cmd(conn, sql);
        P(cmd, "@c", SqlDbType.Int, customerId);
        using var r = cmd.ExecuteReader();
        var list = new List<AppointmentItem>();
        while (r.Read())
        {
            Enum.TryParse<AppointmentStatus>(r.GetString(7), out var status);
            list.Add(new AppointmentItem(r.GetInt32(0), r.GetDateTime(1), r.GetString(2), r.GetString(3),
                r.GetString(4), r.GetString(5), r.GetString(6), status));
        }
        return list;
    }

    public static List<TransactionItem> GetTransactionsForCustomer(int customerId)
    {
        const string sql = @"
SELECT t.TransactionId, t.ReceiptNumber, t.CreatedAt, c.FullName,
       (SELECT ISNULL(SUM(l.Quantity), 0) FROM dbo.TransactionLines l WHERE l.TransactionId = t.TransactionId AND l.LineKind = N'Service'),
       (SELECT ISNULL(SUM(l.Quantity), 0) FROM dbo.TransactionLines l WHERE l.TransactionId = t.TransactionId AND l.LineKind = N'Product'),
       t.Total, t.PaymentMethod, t.Status
FROM dbo.Transactions t JOIN dbo.Customers c ON c.CustomerId = t.CustomerId
WHERE t.CustomerId = @c
ORDER BY t.CreatedAt DESC";

        using var conn = Open();
        using var cmd = Cmd(conn, sql);
        P(cmd, "@c", SqlDbType.Int, customerId);
        using var r = cmd.ExecuteReader();
        var list = new List<TransactionItem>();
        while (r.Read())
        {
            Enum.TryParse<TransactionStatus>(r.GetString(8), out var status);
            list.Add(new TransactionItem(r.GetInt32(0), r.GetString(1), r.GetDateTime(2), r.GetString(3),
                r.GetInt32(4), r.GetInt32(5), r.GetDecimal(6), r.GetString(7), status));
        }
        return list;
    }

    /// <summary>Cancels an appointment only if it belongs to this customer and hasn't been served. Returns false if nothing changed.</summary>
    public static bool CancelOwnAppointment(int customerId, int appointmentId)
    {
        using var conn = Open();
        using var cmd = Cmd(conn, @"
UPDATE dbo.Appointments SET Status = N'Cancelled'
WHERE AppointmentId = @id AND CustomerId = @c AND Status IN (N'Pending', N'Confirmed', N'Rescheduled')");
        P(cmd, "@id", SqlDbType.Int, appointmentId);
        P(cmd, "@c", SqlDbType.Int, customerId);
        return cmd.ExecuteNonQuery() > 0;
    }

    /// <summary>Books for this customer only; the pet must belong to them.</summary>
    public static void AddOwnAppointment(int customerId, int petId, int serviceId, DateTime start, string loginEmail)
    {
        using var conn = Open();
        using var cmd = Cmd(conn, @"
INSERT INTO dbo.Appointments (CustomerId, PetId, ServiceId, StartTime, Status, CreatedByUserId)
SELECT @c, p.PetId, @s, @t, N'Pending', (SELECT UserId FROM dbo.Users WHERE Email = @email)
FROM dbo.Pets p WHERE p.PetId = @p AND p.CustomerId = @c AND p.IsActive = 1");
        P(cmd, "@c", SqlDbType.Int, customerId);
        P(cmd, "@p", SqlDbType.Int, petId);
        P(cmd, "@s", SqlDbType.Int, serviceId);
        P(cmd, "@t", SqlDbType.DateTime2, start);
        P(cmd, "@email", SqlDbType.NVarChar, loginEmail, 256);
        if (cmd.ExecuteNonQuery() == 0)
            throw new InvalidOperationException("That pet doesn't belong to this account.");
    }
}
