using Microsoft.EntityFrameworkCore;
using Pawfect.Data;
using Pawfect.Models;

namespace Pawfect.Services;

public record AuditEntry(
    int Id, DateTime Local, string User, string Role, string ActionType,
    string Module, string Description, string Reference, string Status, string RawAction, string? Details);

public class AuditData
{
    public List<AuditEntry> Entries { get; set; } = new();         // selected period, newest first
    public List<AuditEntry> Recent { get; set; } = new();          // latest 5 overall
    public int PreviousPeriodCount { get; set; }
    public int TodayCount { get; set; }
    public int WeekCount { get; set; }
    public int PendingActions { get; set; }
    public int SystemUsers { get; set; }
}

public class AuditTrailService(PawfectDbContext db)
{
    public static readonly string[] ActionTypes = { "Created", "Updated", "Approved", "Deleted", "Login" };

    // The app's audit log stores free-text actions ("Inventory added", "Service removed", ...).
    // These rules turn them into the action type / module the page shows.
    public static string TypeOf(string action)
    {
        var a = action.ToLowerInvariant();
        if (a.Contains("login")) return "Login";
        if (a.Contains("delet") || a.Contains("remov")) return "Deleted";
        if (a.Contains("approv") || a.Contains("audited")) return "Approved";
        if (a.Contains("added") || a.Contains("created") || a.Contains("logged")) return "Created";
        return "Updated";
    }

    public static string ModuleOf(string action)
    {
        var a = action.ToLowerInvariant();
        if (a.Contains("login")) return "System";
        if (a.StartsWith("inventory") || a.StartsWith("stock")) return "Inventory";
        if (a.StartsWith("service") || a.StartsWith("pricing") || a.StartsWith("durations")) return "Services";
        if (a.StartsWith("employee")) return "Employees";
        if (a.StartsWith("user")) return "Users";
        if (a.StartsWith("sale") || a.StartsWith("return") || a.StartsWith("adjustment") || a.StartsWith("transaction")) return "Financial";
        return "System";
    }

    private static string PrefixOf(string module) => module switch
    {
        "Inventory" => "INV", "Services" => "SRV", "Employees" => "EMP", "Users" => "USR", "Financial" => "FIN", _ => "SYS"
    };

    public static string RoleLabel(IEnumerable<string> roles)
    {
        var list = roles.ToList();
        if (list.Contains("Admin")) return "Administrator";
        return list.FirstOrDefault() ?? "—";
    }

    public async Task<AuditData> LoadAsync(DateOnly from, DateOnly to)
    {
        var (start, end) = PhTime.RangeUtc(from, to);
        var days = to.DayNumber - from.DayNumber + 1;
        var prevStart = PhTime.RangeUtc(from.AddDays(-days), from.AddDays(-1)).StartUtc;

        var logs = await db.AuditLogs.AsNoTracking()
            .Where(a => a.Timestamp >= start && a.Timestamp < end)
            .OrderByDescending(a => a.Timestamp).ThenByDescending(a => a.AuditLogId).ToListAsync();
        var recent = await db.AuditLogs.AsNoTracking()
            .OrderByDescending(a => a.Timestamp).ThenByDescending(a => a.AuditLogId).Take(5).ToListAsync();

        var names = logs.Concat(recent).Select(l => l.UserName).Where(n => !string.IsNullOrEmpty(n))
            .Select(n => n!).Distinct().ToList();
        var roleRows = await (from u in db.Users
                              where u.UserName != null && names.Contains(u.UserName)
                              join ur in db.UserRoles on u.Id equals ur.UserId
                              join r in db.Roles on ur.RoleId equals r.Id
                              select new { Name = u.UserName!, Role = r.Name! }).ToListAsync();
        var roles = roleRows.GroupBy(x => x.Name).ToDictionary(g => g.Key, g => RoleLabel(g.Select(x => x.Role)));

        AuditEntry Map(AuditLog l)
        {
            var type = TypeOf(l.Action);
            var module = ModuleOf(l.Action);
            var local = PhTime.ToLocal(l.Timestamp);
            var user = string.IsNullOrWhiteSpace(l.UserName) ? "System" : l.UserName!;
            var role = roles.TryGetValue(user, out var r) ? r : "—";
            var desc = string.IsNullOrWhiteSpace(l.Details) ? l.Action : $"{l.Action} - {l.Details}";
            return new AuditEntry(l.AuditLogId, local, user, role, type, module, desc,
                $"{PrefixOf(module)}-{local:yyyyMMdd}-{l.AuditLogId:000}", type, l.Action, l.Details);
        }

        var today = PhTime.Today;
        var weekStart = today.AddDays(-(((int)today.DayOfWeek + 6) % 7));   // Monday
        var todayRange = PhTime.RangeUtc(today, today);
        var weekRange = PhTime.RangeUtc(weekStart, today);

        var adminStaffRoleIds = await db.Roles.Where(r => r.Name == "Admin" || r.Name == "Staff").Select(r => r.Id).ToListAsync();

        return new AuditData
        {
            Entries = logs.Select(Map).ToList(),
            Recent = recent.Select(Map).ToList(),
            PreviousPeriodCount = await db.AuditLogs.CountAsync(a => a.Timestamp >= prevStart && a.Timestamp < start),
            TodayCount = await db.AuditLogs.CountAsync(a => a.Timestamp >= todayRange.StartUtc && a.Timestamp < todayRange.EndUtc),
            WeekCount = await db.AuditLogs.CountAsync(a => a.Timestamp >= weekRange.StartUtc && a.Timestamp < weekRange.EndUtc),
            // things waiting on someone: appointments to approve + sales/returns/adjustments to audit
            PendingActions = await db.Appointments.CountAsync(a => a.Status == "Pending")
                           + await db.RetailSales.CountAsync(s => s.Status == "Pending"),
            SystemUsers = await db.UserRoles.Where(ur => adminStaffRoleIds.Contains(ur.RoleId))
                .Select(ur => ur.UserId).Distinct().CountAsync()
        };
    }
}
