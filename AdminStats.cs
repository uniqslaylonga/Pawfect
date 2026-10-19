using System.Data;
using System.Globalization;
using Microsoft.Data.SqlClient;

namespace MyApp;

internal static class AdminStats
{
    public sealed record Growth(int Now, int BeforeThisMonth);
    public enum AlertKind { Expired, OutOfStock, NearExpiry, LowStock }
    public sealed record StockAlert(AlertKind Kind, string Title, string Detail);
    public sealed record RevenueSeries(IReadOnlyList<string> Labels, IReadOnlyList<double> Values);
    public sealed record RevenueSnapshot(
        RevenueSeries Daily, RevenueSeries Weekly, RevenueSeries Monthly,
        IReadOnlyList<string> OverviewLabels,
        IReadOnlyList<double> OverviewDaily, IReadOnlyList<double> OverviewWeekly, IReadOnlyList<double> OverviewMonthly);
    public sealed record SalesSplit(decimal ServiceRevenue, int ServiceSales, decimal RetailRevenue, int RetailSales);

    public const int ExpiryWarningDays = 30;

    private static SqlConnection Open()
    {
        var conn = new SqlConnection(Database.ConnectionString);
        conn.Open();
        return conn;
    }

    private static DateTime MonthStart => new(DateTime.Today.Year, DateTime.Today.Month, 1);

    public static int CountActiveStaff()
    {
        using var conn = Open();
        using var cmd = new SqlCommand("SELECT COUNT(1) FROM dbo.Employees WHERE IsActive = 1", conn);
        return (int)cmd.ExecuteScalar()!;
    }

    public static Growth StaffGrowth() =>
        CountGrowth("dbo.Employees", "IsActive = 1", "(StartDate IS NULL OR StartDate < @m)");
    public static Growth CustomerGrowth() =>
        CountGrowth("dbo.Customers", "IsActive = 1", "CreatedAt < @m");
    public static Growth PetGrowth() =>
        CountGrowth("dbo.Pets", "IsActive = 1", "CreatedAt < @m");

    private static Growth CountGrowth(string table, string activeFilter, string beforeMonthFilter)
    {
        using var conn = Open();
        using var cmd = new SqlCommand(
            $"SELECT COUNT(*), COUNT(CASE WHEN {beforeMonthFilter} THEN 1 END) FROM {table} WHERE {activeFilter}", conn);
        cmd.Parameters.Add("@m", SqlDbType.DateTime2).Value = MonthStart;
        using var r = cmd.ExecuteReader();
        r.Read();
        return new Growth(r.GetInt32(0), r.GetInt32(1));
    }

    public static RevenueSnapshot GetRevenue()
    {
        DateTime today = DateTime.Today;
        DateTime from = MonthStart.AddMonths(-5);

        var sales = new List<(DateTime Day, decimal Total)>();
        using (var conn = Open())
        using (var cmd = new SqlCommand(
                   "SELECT CreatedAt, Total FROM dbo.Transactions WHERE Status = N'Completed' AND CreatedAt >= @from", conn))
        {
            cmd.Parameters.Add("@from", SqlDbType.DateTime2).Value = from;
            using var r = cmd.ExecuteReader();
            while (r.Read())
                sales.Add((r.GetDateTime(0).Date, r.GetDecimal(1)));
        }

        double Sum(DateTime start, DateTime endInclusive) =>
            (double)sales.Where(s => s.Day >= start && s.Day <= endInclusive).Sum(s => s.Total);

        var culture = CultureInfo.CurrentCulture;

        var dayLabels = new List<string>();
        var dayValues = new List<double>();
        for (int i = 6; i >= 0; i--)
        {
            var day = today.AddDays(-i);
            dayLabels.Add(day.ToString("ddd d", culture));
            dayValues.Add(Sum(day, day));
        }

        var weekLabels = new List<string>();
        var weekValues = new List<double>();
        for (int k = 7; k >= 0; k--)
        {
            var end = today.AddDays(-7 * k);
            var start = end.AddDays(-6);
            weekLabels.Add(start.ToString("MMM d", culture));
            weekValues.Add(Sum(start, end));
        }

        var monthLabels = new List<string>();
        var monthValues = new List<double>();
        for (int m = 5; m >= 0; m--)
        {
            var start = MonthStart.AddMonths(-m);
            var end = start.AddMonths(1).AddDays(-1);
            monthLabels.Add(start.ToString("MMM", culture));
            monthValues.Add(Sum(start, end));
        }

        var overviewDaily = new List<double>();
        var overviewWeekly = new List<double>();
        var overviewMonthly = new List<double>();
        for (int i = 6; i >= 0; i--)
        {
            var day = today.AddDays(-i);
            overviewDaily.Add(Sum(day, day));
            overviewWeekly.Add(Sum(day.AddDays(-6), day));
            overviewMonthly.Add(Sum(day.AddDays(-29), day));
        }

        // An all-zero series becomes empty so the charts show "no data yet" instead of a flat line.
        static RevenueSeries Series(List<string> labels, List<double> values) =>
            values.Any(v => v > 0) ? new RevenueSeries(labels, values) : new RevenueSeries(Array.Empty<string>(), Array.Empty<double>());

        bool anyOverview = overviewMonthly.Any(v => v > 0);
        return new RevenueSnapshot(
            Series(dayLabels, dayValues), Series(weekLabels, weekValues), Series(monthLabels, monthValues),
            anyOverview ? dayLabels : new List<string>(),
            overviewDaily, overviewWeekly, overviewMonthly);
    }

    public static SalesSplit GetSalesSplit()
    {
        decimal serviceRevenue = 0, retailRevenue = 0;
        int serviceSales = 0, retailSales = 0;

        using var conn = Open();
        using var cmd = new SqlCommand(
            @"SELECT l.LineKind, SUM(l.UnitPrice * l.Quantity), COUNT(DISTINCT l.TransactionId)
              FROM dbo.TransactionLines l
              JOIN dbo.Transactions t ON t.TransactionId = l.TransactionId
              WHERE t.Status = N'Completed' AND t.CreatedAt >= @m
              GROUP BY l.LineKind", conn);
        cmd.Parameters.Add("@m", SqlDbType.DateTime2).Value = MonthStart;
        using var r = cmd.ExecuteReader();
        while (r.Read())
        {
            decimal revenue = r.GetDecimal(1);
            int sales = r.GetInt32(2);
            if (r.GetString(0) == "Service") { serviceRevenue = revenue; serviceSales = sales; }
            else { retailRevenue = revenue; retailSales = sales; }
        }
        return new SalesSplit(serviceRevenue, serviceSales, retailRevenue, retailSales);
    }

    public static List<StockAlert> GetAlerts()
    {
        var alerts = new List<StockAlert>();
        DateTime today = DateTime.Today;

        using var conn = Open();

        using (var cmd = new SqlCommand(
                   @"SELECT p.Name, p.Unit, b.BatchNo, b.ExpiryDate, b.QuantityRemaining
                     FROM dbo.StockBatches b
                     JOIN dbo.Products p ON p.ProductId = b.ProductId
                     WHERE p.IsActive = 1 AND b.QuantityRemaining > 0
                       AND b.ExpiryDate IS NOT NULL AND b.ExpiryDate <= @limit
                     ORDER BY b.ExpiryDate, p.Name", conn))
        {
            cmd.Parameters.Add("@limit", SqlDbType.Date).Value = today.AddDays(ExpiryWarningDays);
            using var r = cmd.ExecuteReader();
            while (r.Read())
            {
                string name = r.GetString(0), unit = r.GetString(1), batch = r.GetString(2);
                int days = (r.GetDateTime(3).Date - today).Days;
                int qty = r.GetInt32(4);
                string left = $"{qty} {unit} left in batch {batch}";

                if (days < 0)
                    alerts.Add(new(AlertKind.Expired, $"Expired: {name}",
                        $"Expired {Plural(-days, "day")} ago · {left}"));
                else
                    alerts.Add(new(AlertKind.NearExpiry, $"Expiring soon: {name}",
                        (days == 0 ? "Expires today" : $"Expires in {Plural(days, "day")}") + $" · {left}"));
            }
        }

        using (var cmd = new SqlCommand(
                   @"SELECT p.Name, p.Unit, p.ReorderLevel, ISNULL(SUM(b.QuantityRemaining), 0)
                     FROM dbo.Products p
                     LEFT JOIN dbo.StockBatches b ON b.ProductId = p.ProductId
                     WHERE p.IsActive = 1
                     GROUP BY p.ProductId, p.Name, p.Unit, p.ReorderLevel
                     HAVING ISNULL(SUM(b.QuantityRemaining), 0) <= p.ReorderLevel
                     ORDER BY ISNULL(SUM(b.QuantityRemaining), 0), p.Name", conn))
        using (var r = cmd.ExecuteReader())
        {
            while (r.Read())
            {
                string name = r.GetString(0), unit = r.GetString(1);
                int reorder = r.GetInt32(2), qty = r.GetInt32(3);

                if (qty == 0)
                    alerts.Add(new(AlertKind.OutOfStock, $"Out of stock: {name}", $"Reorder level is {reorder} {unit}"));
                else
                    alerts.Add(new(AlertKind.LowStock, $"Low stock: {name}", $"{qty} {unit} left · reorder level {reorder}"));
            }
        }

        return alerts.OrderBy(a => a.Kind).ToList();
    }

    private static string Plural(int n, string word) => n == 1 ? $"1 {word}" : $"{n} {word}s";
}