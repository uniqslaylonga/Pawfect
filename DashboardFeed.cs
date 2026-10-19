using System.Data;
using Microsoft.Data.SqlClient;

namespace MyApp;

/// <summary>Numbers and lists for the admin dashboard.</summary>
internal static class DashboardFeed
{
    /// <summary>Completed sales per day between two dates. A missing table counts as no sales.</summary>
    public static List<(DateTime Day, decimal Total)> GetRevenue(DateTime from, DateTime to)
    {
        var list = new List<(DateTime Day, decimal Total)>();
        try
        {
            using var conn = new SqlConnection(Database.ConnectionString);
            conn.Open();

            using var cmd = new SqlCommand(
                @"SELECT CAST(CreatedAt AS DATE), SUM(Total)
              FROM dbo.Transactions
              WHERE Status = N'Completed' AND CAST(CreatedAt AS DATE) BETWEEN @f AND @t
              GROUP BY CAST(CreatedAt AS DATE)
              ORDER BY 1", conn);
            cmd.Parameters.Add("@f", SqlDbType.Date).Value = from.Date;
            cmd.Parameters.Add("@t", SqlDbType.Date).Value = to.Date;
            using var r = cmd.ExecuteReader();
            while (r.Read())
                list.Add((r.GetDateTime(0), r.GetDecimal(1)));
        }
        catch (SqlException ex) when (ex.Number is 207 or 208) { }
        return list;
    }

    public sealed record Alert(string Title, string Detail, bool Critical);

    public static List<Alert> GetAlerts()
    {
        var today = DateTime.Today;
        var products = InventoryRepository.GetProducts().Where(p => p.Active).ToList();
        var batches = InventoryRepository.GetBatches();
        var byId = products.ToDictionary(p => p.Id);
        var totals = batches.GroupBy(b => b.ProductId).ToDictionary(g => g.Key, g => g.Sum(b => b.Remaining));
        var list = new List<Alert>();

        foreach (var p in products)
        {
            int total = totals.GetValueOrDefault(p.Id);
            if (total == 0) list.Add(new Alert(p.Name, "Out of stock", true));
            else if (total <= p.ReorderLevel) list.Add(new Alert(p.Name, $"Low stock: {total} {p.Unit} left", false));
        }

        foreach (var b in batches.Where(b => b.Expiry != null).OrderBy(b => b.Expiry))
        {
            if (!byId.TryGetValue(b.ProductId, out var p)) continue;
            int days = (b.Expiry!.Value.Date - today).Days;
            if (days < 0) list.Add(new Alert(p.Name, $"Batch {b.BatchNo} has expired", true));
            else if (days <= 30) list.Add(new Alert(p.Name, $"Batch {b.BatchNo} expires in {days} day(s)", false));
        }

        return list.OrderByDescending(a => a.Critical).ToList();
    }

    public static int NewCustomers() =>
        Scalar("SELECT COUNT(1) FROM dbo.Customers WHERE CreatedAt >= @m", MonthStart().ToUniversalTime());

    public static int NewStaff() =>
        Scalar("SELECT COUNT(1) FROM dbo.Employees WHERE StartDate >= @m", MonthStart());

    public static int CountActivePets() =>
        Safe(() => Scalar("SELECT COUNT(1) FROM dbo.Pets WHERE IsActive = 1"), 0);

    public static int NewPets() =>
        Safe(() => Scalar("SELECT COUNT(1) FROM dbo.Pets WHERE CreatedAt >= @m", MonthStart()), 0);

    /// <summary>Services sold and retail items sold this month (completed transactions only).</summary>
    public static (int Services, int Items) GetSplit()
    {
        try
        {
            using var conn = new SqlConnection(Database.ConnectionString);
            conn.Open();

            using var cmd = new SqlCommand(
                @"SELECT ISNULL(SUM(CASE WHEN l.LineKind = N'Service' THEN l.Quantity ELSE 0 END), 0),
                         ISNULL(SUM(CASE WHEN l.LineKind = N'Product' THEN l.Quantity ELSE 0 END), 0)
                  FROM dbo.TransactionLines l
                  JOIN dbo.Transactions t ON t.TransactionId = l.TransactionId
                  WHERE t.Status = N'Completed' AND t.CreatedAt >= @m", conn);
            cmd.Parameters.Add("@m", SqlDbType.DateTime2).Value = MonthStart();
            using var r = cmd.ExecuteReader();
            r.Read();
            return (Convert.ToInt32(r.GetValue(0)), Convert.ToInt32(r.GetValue(1)));
        }
        catch (SqlException ex) when (ex.Number is 207 or 208)
        {
            return (0, 0);
        }
    }

    private static DateTime MonthStart() => new(DateTime.Now.Year, DateTime.Now.Month, 1);

    private static int Scalar(string sql, DateTime? month = null)
    {
        using var conn = new SqlConnection(Database.ConnectionString);
        conn.Open();
        using var cmd = new SqlCommand(sql, conn);
        if (month != null) cmd.Parameters.Add("@m", SqlDbType.DateTime2).Value = month.Value;
        return Convert.ToInt32(cmd.ExecuteScalar());
    }

    /// <summary>Treats a missing table or column (SQL errors 207 and 208) as "no data yet".</summary>
    private static T Safe<T>(Func<T> read, T fallback)
    {
        try { return read(); }
        catch (SqlException ex) when (ex.Number is 207 or 208) { return fallback; }
    }
}