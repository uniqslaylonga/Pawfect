using Microsoft.EntityFrameworkCore;
using Pawfect.Data;

namespace Pawfect.Services;

public record ChartPoint(string Label, decimal Value);
public record BarGroup(string Label, decimal Daily, decimal Weekly, decimal Monthly);
public record ActivityItem(string Title, string Detail, string When, string Kind);

public class DashboardData
{
    public int Customers { get; set; }
    public int StaffCount { get; set; }
    public int Pets { get; set; }
    public double? PetsTrendPct { get; set; }   // null = not enough data (shown as N/A)

    public List<ChartPoint> Daily { get; set; } = new();
    public List<ChartPoint> Weekly { get; set; } = new();
    public List<ChartPoint> Monthly { get; set; } = new();
    public List<BarGroup> Overview { get; set; } = new();

    public int Appointments { get; set; }        // last 30 days
    public int Sales { get; set; }               // last 30 days

    public List<string> LowStock { get; set; } = new();
    public List<string> NearExpiry { get; set; } = new();
    public List<ActivityItem> Activity { get; set; } = new();
}

// Every number comes from the database. If there is no data the value is 0 or empty.
public class DashboardService(PawfectDbContext db)
{
    // Philippines is UTC+8, no daylight saving
    private static DateTime ToLocal(DateTime utc) => utc.AddHours(8);
    private static DateTime ToUtc(DateTime local) =>
        DateTime.SpecifyKind(local.AddHours(-8), DateTimeKind.Utc);

    public async Task<DashboardData> LoadAsync()
    {
        var data = new DashboardData();
        var today = ToLocal(DateTime.UtcNow).Date;

        // ---- People ----
        var adminStaffRoleIds = await db.Roles
            .Where(r => r.Name == "Admin" || r.Name == "Staff")
            .Select(r => r.Id).ToListAsync();
        var staffRoleId = await db.Roles
            .Where(r => r.Name == "Staff")
            .Select(r => r.Id).FirstOrDefaultAsync();

        data.Customers = await db.Users.CountAsync(u =>
            !db.UserRoles.Any(ur => ur.UserId == u.Id && adminStaffRoleIds.Contains(ur.RoleId)));
        data.StaffCount = staffRoleId is null
            ? 0
            : await db.UserRoles.CountAsync(ur => ur.RoleId == staffRoleId);
        data.Pets = await db.Pets.CountAsync(p => p.IsActive);

        // New pets this month vs last month
        var thisMonthStart = ToUtc(new DateTime(today.Year, today.Month, 1));
        var lastMonthStart = ToUtc(new DateTime(today.Year, today.Month, 1).AddMonths(-1));
        var newThis = await db.Pets.CountAsync(p => p.DateCreated >= thisMonthStart);
        var newLast = await db.Pets.CountAsync(p => p.DateCreated >= lastMonthStart && p.DateCreated < thisMonthStart);
        data.PetsTrendPct = newLast > 0 ? (newThis - newLast) * 100.0 / newLast : null;

        // ---- Revenue (completed appointments + retail sales) ----
        var from = ToUtc(today.AddDays(-200));
        var services = await db.Appointments
            .Where(a => a.Status == "Completed" && a.ScheduledAt >= from)
            .Select(a => new { a.ScheduledAt, a.Price }).ToListAsync();
        var sales = await db.RetailSales
            .Where(s => s.SoldAt >= from && s.TransactionType != "Adjustment")
            .Select(s => new { s.SoldAt, s.Total }).ToListAsync();

        var revenue = services.Select(x => (When: ToLocal(x.ScheduledAt), Amount: x.Price))
            .Concat(sales.Select(x => (When: ToLocal(x.SoldAt), Amount: x.Total)))
            .ToList();

        decimal Sum(DateTime fromInclusive, DateTime toExclusive) =>
            revenue.Where(r => r.When >= fromInclusive && r.When < toExclusive).Sum(r => r.Amount);

        for (var i = 6; i >= 0; i--)
        {
            var day = today.AddDays(-i);
            data.Daily.Add(new ChartPoint(day.ToString("MMM d"), Sum(day, day.AddDays(1))));
            data.Overview.Add(new BarGroup(
                day.ToString("MMM d"),
                Sum(day, day.AddDays(1)),
                Sum(day.AddDays(-6), day.AddDays(1)),
                Sum(day.AddDays(-29), day.AddDays(1))));
        }
        for (var i = 7; i >= 0; i--)
        {
            var end = today.AddDays(1).AddDays(-7 * i);
            var start = end.AddDays(-7);
            data.Weekly.Add(new ChartPoint(start.ToString("MMM d"), Sum(start, end)));
        }
        for (var i = 5; i >= 0; i--)
        {
            var m = new DateTime(today.Year, today.Month, 1).AddMonths(-i);
            data.Monthly.Add(new ChartPoint(m.ToString("MMM"), Sum(m, m.AddMonths(1))));
        }

        // ---- Appointments vs retail sales (last 30 days) ----
        var last30 = ToUtc(today.AddDays(-29));
        data.Appointments = await db.Appointments.CountAsync(a => a.Status != "Cancelled" && a.ScheduledAt >= last30);
        data.Sales = await db.RetailSales.CountAsync(s => s.SoldAt >= last30 && s.TransactionType == "Sale");

        // ---- Alerts ----
        var low = await db.InventoryItems
            .Where(i => i.Quantity <= i.ReorderLevel)
            .OrderBy(i => i.Quantity).Take(3).ToListAsync();
        data.LowStock = low.Select(i => $"{i.Name} ({i.Quantity} pcs)").ToList();

        var todayDate = DateOnly.FromDateTime(today);
        var soon = todayDate.AddDays(30);
        var expiring = await db.InventoryItems
            .Where(i => i.ExpiryDate != null && i.ExpiryDate >= todayDate && i.ExpiryDate <= soon)
            .OrderBy(i => i.ExpiryDate).Take(3).ToListAsync();
        data.NearExpiry = expiring
            .Select(i => $"{i.Name} (Exp: {i.ExpiryDate:MMM d, yyyy}) - {i.Quantity} pcs").ToList();

        // ---- Recent activity ----
        var logs = await db.AuditLogs.OrderByDescending(a => a.Timestamp).Take(5).ToListAsync();
        data.Activity = logs.Select(a =>
        {
            var local = ToLocal(a.Timestamp);
            var when = local.Date == today ? local.ToString("hh:mm tt")
                     : local.Date == today.AddDays(-1) ? "Yesterday"
                     : local.ToString("MMM d");
            var lower = a.Action.ToLowerInvariant();
            var kind = lower.Contains("delet") || lower.Contains("remov") ? "delete"
                     : lower.Contains("approv") || lower.Contains("added") || lower.Contains("created") ? "success"
                     : "info";
            var detail = string.Join(" - ", new[] { a.Details, a.UserName }.Where(x => !string.IsNullOrWhiteSpace(x)));
            return new ActivityItem(a.Action, detail, when, kind);
        }).ToList();

        return data;
    }
}
