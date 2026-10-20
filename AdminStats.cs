using System.Data;
using System.Globalization;
using Microsoft.Data.SqlClient;

namespace MyApp;

/// <summary>One point of a revenue chart: the x label and the gross revenue for that day / week / month.</summary>
internal sealed record RevenuePoint(string Label, double Value);

/// <summary>Everything the admin dashboard cards and charts show, read in one go.</summary>
internal sealed record AdminSnapshot(
    int Customers, int CustomersAtMonthStart,
    int Staff, int StaffAtMonthStart,
    int Pets, int PetsAtMonthStart,
    IReadOnlyList<RevenuePoint> Daily,        // last 14 days
    IReadOnlyList<RevenuePoint> Weekly,       // last 8 weeks (Monday to Sunday)
    IReadOnlyList<RevenuePoint> Monthly,      // last 12 months
    IReadOnlyList<string> MonthLabels,        // last 6 months, for the bar chart
    IReadOnlyList<double> ServicesByMonth,
    IReadOnlyList<double> RetailByMonth,
    int GroomingSalesThisMonth,               // completed sales that came from an appointment
    int RetailSalesThisMonth);                // completed sales with no appointment (walk-in)

internal enum AlertKind { OutOfStock, LowStock, Waiting, Reversed }

/// <summary>One line of the dashboard Alerts card.</summary>
internal sealed record DashAlert(AlertKind Kind, string Title, string Detail, string Meta);

/// <summary>Read-only numbers for the admin dashboard.</summary>
internal static class AdminStats
{
    public static int CountActiveStaff()
    {
        using var conn = new SqlConnection(Database.ConnectionString);
        conn.Open();
        return Scalar(conn, "SELECT COUNT(1) FROM dbo.Users WHERE IsActive = 1 AND Role <> 'Admin'");
    }

    private static int Scalar(SqlConnection conn, string sql, string? name = null, DateTime? value = null)
    {
        using var cmd = new SqlCommand(sql, conn);
        if (name is not null) cmd.Parameters.Add(name, SqlDbType.DateTime2).Value = value!.Value;
        return Convert.ToInt32(cmd.ExecuteScalar());
    }

    /// <summary>Throws SqlException if the database can't be reached.</summary>
    public static AdminSnapshot GetSnapshot()
    {
        var now = DateTime.Now;
        var today = now.Date;
        var monthStart = new DateTime(now.Year, now.Month, 1);
        var firstMonth = monthStart.AddMonths(-11);
        var culture = CultureInfo.CurrentCulture;

        using var conn = new SqlConnection(Database.ConnectionString);
        conn.Open();

        // "vs. last month" = how many were added since the 1st of this month (counts as they stand today).
        int customers = Scalar(conn, "SELECT COUNT(*) FROM dbo.Customers WHERE IsActive = 1");
        int customersBefore = Scalar(conn, "SELECT COUNT(*) FROM dbo.Customers WHERE IsActive = 1 AND CreatedAt < @m", "@m", monthStart);

        // Staff = active people on the employee roster (Employee & User Management > Employee Management).
        int staff = EmployeeRepository.CountActive();
        int staffBefore = EmployeeRepository.CountActive(monthStart);

        const string petsFrom = "FROM dbo.Pets p JOIN dbo.Customers c ON c.CustomerId = p.CustomerId WHERE p.IsActive = 1 AND c.IsActive = 1";
        int pets = Scalar(conn, "SELECT COUNT(*) " + petsFrom);
        int petsBefore = Scalar(conn, "SELECT COUNT(*) " + petsFrom + " AND p.CreatedAt < @m", "@m", monthStart);

        // Completed sales only: voided and refunded ones are not revenue.
        var totals = new List<(DateTime When, double Amount)>();
        using (var cmd = new SqlCommand(
            "SELECT CreatedAt, Total FROM dbo.Transactions WHERE Status = N'Completed' AND CreatedAt >= @from", conn))
        {
            cmd.Parameters.Add("@from", SqlDbType.DateTime2).Value = firstMonth;
            using var r = cmd.ExecuteReader();
            while (r.Read()) totals.Add((r.GetDateTime(0), (double)r.GetDecimal(1)));
        }

        // Services vs. retail: each line's share of its sale, so a discount is spread over the lines and the parts add up to the sale total.
        var parts = new List<(DateTime When, bool IsService, double Amount)>();
        using (var cmd = new SqlCommand(@"
SELECT t.CreatedAt, l.LineKind,
       CAST(l.UnitPrice * l.Quantity * CASE WHEN t.Subtotal > 0 THEN t.Total / t.Subtotal ELSE 1 END AS FLOAT)
FROM dbo.TransactionLines l
JOIN dbo.Transactions t ON t.TransactionId = l.TransactionId
WHERE t.Status = N'Completed' AND t.CreatedAt >= @from", conn))
        {
            cmd.Parameters.Add("@from", SqlDbType.DateTime2).Value = firstMonth;
            using var r = cmd.ExecuteReader();
            while (r.Read()) parts.Add((r.GetDateTime(0), r.GetString(1) == "Service", r.GetDouble(2)));
        }

        double Sum(DateTime from, DateTime to) => totals.Where(t => t.When >= from && t.When < to).Sum(t => t.Amount);

        var daily = Enumerable.Range(0, 14).Select(i => today.AddDays(i - 13))
            .Select(d => new RevenuePoint(d.ToString("MMM d", culture), Sum(d, d.AddDays(1)))).ToList();

        var thisWeek = today.AddDays(-(((int)today.DayOfWeek + 6) % 7));   // Monday
        var weekly = Enumerable.Range(0, 8).Select(i => thisWeek.AddDays(7 * (i - 7)))
            .Select(w => new RevenuePoint(w.ToString("MMM d", culture), Sum(w, w.AddDays(7)))).ToList();

        var months = Enumerable.Range(0, 12).Select(i => monthStart.AddMonths(i - 11)).ToList();
        var monthly = months.Select(m => new RevenuePoint(m.ToString("MMM yy", culture), Sum(m, m.AddMonths(1)))).ToList();

        var lastSix = months.Skip(6).ToList();
        double Part(DateTime m, bool service) =>
            parts.Where(p => p.IsService == service && p.When >= m && p.When < m.AddMonths(1)).Sum(p => p.Amount);

        int grooming, retail;
        using (var cmd = new SqlCommand(@"
SELECT ISNULL(SUM(CASE WHEN AppointmentId IS NOT NULL THEN 1 ELSE 0 END), 0),
       ISNULL(SUM(CASE WHEN AppointmentId IS NULL THEN 1 ELSE 0 END), 0)
FROM dbo.Transactions WHERE Status = N'Completed' AND CreatedAt >= @m", conn))
        {
            cmd.Parameters.Add("@m", SqlDbType.DateTime2).Value = monthStart;
            using var r = cmd.ExecuteReader();
            r.Read();
            grooming = Convert.ToInt32(r.GetValue(0));
            retail = Convert.ToInt32(r.GetValue(1));
        }

        return new AdminSnapshot(
            customers, customersBefore, staff, staffBefore, pets, petsBefore,
            daily, weekly, monthly,
            lastSix.Select(m => m.ToString("MMM", culture)).ToList(),
            lastSix.Select(m => Part(m, true)).ToList(),
            lastSix.Select(m => Part(m, false)).ToList(),
            grooming, retail);
    }

    /// <summary>
    /// Things the admin should look at: products and supplies at or below their reorder level, requests still waiting
    /// for confirmation, and sales voided or refunded this past week. The stock tables are optional; if they were never
    /// set up their alerts are simply left out.
    /// </summary>
    public static List<DashAlert> GetAlerts()
    {
        var list = new List<DashAlert>();
        using var conn = new SqlConnection(Database.ConnectionString);
        conn.Open();

        TryRead(conn, @"SELECT Name, QtyOnHand, ReorderLevel FROM dbo.Products
                        WHERE IsActive = 1 AND ReorderLevel > 0 AND QtyOnHand <= ReorderLevel ORDER BY QtyOnHand, Name", r =>
        {
            string name = r.GetString(0);
            int qty = r.GetInt32(1), level = r.GetInt32(2);
            list.Add(qty <= 0
                ? new DashAlert(AlertKind.OutOfStock, name, $"Out of stock (reorder level {level})", "Product")
                : new DashAlert(AlertKind.LowStock, name, $"Low stock: {qty} left (reorder at {level})", "Product"));
        });

        TryRead(conn, @"SELECT Name, QtyOnHand, MinStock, Unit FROM dbo.Consumables
                        WHERE IsActive = 1 AND MinStock > 0 AND QtyOnHand <= MinStock ORDER BY QtyOnHand, Name", r =>
        {
            string name = r.GetString(0), unit = r.GetString(3);
            int qty = r.GetInt32(1), min = r.GetInt32(2);
            list.Add(qty <= 0
                ? new DashAlert(AlertKind.OutOfStock, name, $"Out of stock (minimum {min} {unit})", "Supply")
                : new DashAlert(AlertKind.LowStock, name, $"Low stock: {qty} {unit} left (minimum {min})", "Supply"));
        });

        int waiting = Scalar(conn, "SELECT COUNT(*) FROM dbo.Appointments WHERE Status IN (N'Pending', N'Rescheduled')");
        if (waiting > 0)
            list.Add(new DashAlert(AlertKind.Waiting,
                waiting == 1 ? "1 request waiting" : $"{waiting} requests waiting",
                "Appointment requests that still need a groomer, a room and a confirmation", "Staff"));

        int reversed = Scalar(conn,
            "SELECT COUNT(*) FROM dbo.Transactions WHERE Status IN (N'Voided', N'Refunded') AND CreatedAt >= DATEADD(DAY, -7, SYSDATETIME())");
        if (reversed > 0)
            list.Add(new DashAlert(AlertKind.Reversed,
                reversed == 1 ? "1 voided or refunded sale" : $"{reversed} voided or refunded sales",
                "Sales from the last 7 days that were voided or refunded", "Sales"));

        return list.OrderBy(a => a.Kind).ToList();   // out of stock first, then low stock, waiting, reversed
    }

    /// <summary>Runs a query, but treats "table or column doesn't exist" as "nothing to report".</summary>
    private static void TryRead(SqlConnection conn, string sql, Action<SqlDataReader> each)
    {
        try
        {
            using var cmd = new SqlCommand(sql, conn);
            using var r = cmd.ExecuteReader();
            while (r.Read()) each(r);
        }
        catch (SqlException ex) when (ex.Number is 207 or 208)
        {
            // that part of the database isn't set up here
        }
    }
}
