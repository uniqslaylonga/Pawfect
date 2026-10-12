using Microsoft.EntityFrameworkCore;
using Pawfect.Data;
using Pawfect.Models;

namespace Pawfect.Services;

public record BookingInfo(Dictionary<string, int> PerService, int Total, int ThisMonth, int LastMonth);

public class CatalogService(PawfectDbContext db)
{
    // Philippines is UTC+8, no daylight saving (same convention as DashboardService)
    private static DateTime LocalMonthStartUtc(int monthsBack)
    {
        var local = DateTime.UtcNow.AddHours(8);
        var start = new DateTime(local.Year, local.Month, 1).AddMonths(-monthsBack);
        return DateTime.SpecifyKind(start.AddHours(-8), DateTimeKind.Utc);
    }

    public async Task<List<ServiceItem>> LoadAsync() =>
        await db.ServiceItems.AsNoTracking().OrderBy(s => s.ServiceItemId).ToListAsync();

    // Bookings are real appointments (not cancelled) whose ServiceName matches a catalog service name.
    public async Task<BookingInfo> LoadBookingsAsync(List<string> serviceNames)
    {
        var perService = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        if (serviceNames.Count == 0) return new BookingInfo(perService, 0, 0, 0);

        var lowered = serviceNames.Select(n => n.ToLower()).ToList();
        var rows = await db.Appointments.AsNoTracking()
            .Where(a => a.Status != "Cancelled" && lowered.Contains(a.ServiceName.ToLower()))
            .Select(a => new { a.ServiceName, a.ScheduledAt })
            .ToListAsync();

        foreach (var g in rows.GroupBy(r => r.ServiceName, StringComparer.OrdinalIgnoreCase))
            perService[g.Key] = g.Count();

        var thisStart = LocalMonthStartUtc(0);
        var lastStart = LocalMonthStartUtc(1);
        var thisMonth = rows.Count(r => r.ScheduledAt >= thisStart);
        var lastMonth = rows.Count(r => r.ScheduledAt >= lastStart && r.ScheduledAt < thisStart);
        return new BookingInfo(perService, rows.Count, thisMonth, lastMonth);
    }

    public async Task<string?> AddAsync(ServiceItem s, string actor)
    {
        if (await NameTaken(s.Name, 0)) return "A service with that name already exists.";
        s.DateCreated = DateTime.UtcNow;
        db.ServiceItems.Add(s);
        await db.SaveChangesAsync();
        await Log("Service added", $"{s.Name} ({s.Category}) added", actor);
        return null;
    }

    public async Task<string?> UpdateAsync(ServiceItem form, string actor)
    {
        var s = await db.ServiceItems.FindAsync(form.ServiceItemId);
        if (s is null) return "Service not found.";
        if (await NameTaken(form.Name, form.ServiceItemId)) return "A service with that name already exists.";
        s.Name = form.Name;
        s.Description = form.Description;
        s.Category = form.Category;
        s.SizeApplicability = form.SizeApplicability;
        s.DurationMinutes = form.DurationMinutes;
        s.MinPrice = form.MinPrice;
        s.MaxPrice = form.MaxPrice;
        s.IsActive = form.IsActive;
        await db.SaveChangesAsync();
        await Log("Service updated", $"{s.Name} updated", actor);
        return null;
    }

    public async Task SetActiveAsync(int id, bool active, string actor)
    {
        var s = await db.ServiceItems.FindAsync(id);
        if (s is null) return;
        s.IsActive = active;
        await db.SaveChangesAsync();
        await Log(active ? "Service activated" : "Service deactivated", s.Name, actor);
    }

    public async Task DeleteAsync(int id, string actor)
    {
        var s = await db.ServiceItems.FindAsync(id);
        if (s is null) return;
        db.ServiceItems.Remove(s);
        await db.SaveChangesAsync();
        await Log("Service removed", s.Name, actor);
    }

    public async Task SavePricingAsync(IEnumerable<(int Id, decimal Min, decimal Max)> rows, string actor)
    {
        var changed = 0;
        foreach (var r in rows)
        {
            var s = await db.ServiceItems.FindAsync(r.Id);
            if (s is null || (s.MinPrice == r.Min && s.MaxPrice == r.Max)) continue;
            s.MinPrice = r.Min;
            s.MaxPrice = r.Max;
            changed++;
        }
        if (changed == 0) return;
        await db.SaveChangesAsync();
        await Log("Pricing updated", $"{changed} service price(s) updated", actor);
    }

    public async Task SaveDurationsAsync(IEnumerable<(int Id, int Minutes)> rows, string actor)
    {
        var changed = 0;
        foreach (var r in rows)
        {
            var s = await db.ServiceItems.FindAsync(r.Id);
            if (s is null || s.DurationMinutes == r.Minutes) continue;
            s.DurationMinutes = r.Minutes;
            changed++;
        }
        if (changed == 0) return;
        await db.SaveChangesAsync();
        await Log("Durations updated", $"{changed} service duration(s) updated", actor);
    }

    private Task<bool> NameTaken(string name, int exceptId) =>
        db.ServiceItems.AnyAsync(x => x.Name.ToLower() == name.ToLower() && x.ServiceItemId != exceptId);

    private async Task Log(string action, string details, string actor)
    {
        db.AuditLogs.Add(new AuditLog { Action = action, Details = details, UserName = actor });
        await db.SaveChangesAsync();
    }
}
