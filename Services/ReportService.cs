using Microsoft.EntityFrameworkCore;
using Pawfect.Data;
using Pawfect.Models;

namespace Pawfect.Services;

public class ReportData
{
    public List<RetailSale> Sales { get; set; } = new();               // in the selected period, newest first
    public List<InventoryMovement> Movements { get; set; } = new();    // in the selected period, newest first

    // previous period of the same length (for the "vs. last period" trends)
    public decimal PrevAuditedSales { get; set; }
    public int PrevMovementUnits { get; set; }
    public int PrevDiscrepancies { get; set; }

    public decimal ServiceRevenue { get; set; }                        // completed appointments in the period
    public Dictionary<string, string> Products { get; set; } = new();  // inventory product -> category
    public List<AuditLog> RecentActivity { get; set; } = new();
}

public class ReportService(PawfectDbContext db)
{
    // Actions this page writes to the audit trail (also what the "Recent Audit Activity" panel shows)
    public static readonly string[] ReportActions =
        { "Sale logged", "Return logged", "Adjustment logged", "Transaction audited", "Transaction flagged", "Transaction reopened" };

    public static decimal NetSales(IEnumerable<RetailSale> s) =>
        s.Where(x => x.TransactionType != "Adjustment").Sum(x => x.Total);

    public async Task<ReportData> LoadAsync(DateOnly from, DateOnly to)
    {
        var (start, end) = PhTime.RangeUtc(from, to);
        var days = to.DayNumber - from.DayNumber + 1;
        var prevStart = PhTime.RangeUtc(from.AddDays(-days), from.AddDays(-1)).StartUtc;

        var data = new ReportData
        {
            Sales = await db.RetailSales.AsNoTracking()
                .Where(s => s.SoldAt >= start && s.SoldAt < end)
                .OrderByDescending(s => s.SoldAt).ThenByDescending(s => s.RetailSaleId).ToListAsync(),
            Movements = await db.InventoryMovements.AsNoTracking()
                .Where(m => m.Timestamp >= start && m.Timestamp < end)
                .OrderByDescending(m => m.Timestamp).ThenByDescending(m => m.InventoryMovementId).ToListAsync()
        };

        var prevSales = await db.RetailSales.AsNoTracking()
            .Where(s => s.SoldAt >= prevStart && s.SoldAt < start)
            .Select(s => new { s.TransactionType, s.Status, s.Total }).ToListAsync();
        data.PrevAuditedSales = prevSales.Where(s => s.TransactionType != "Adjustment" && s.Status == "Audited").Sum(s => s.Total);
        data.PrevDiscrepancies = prevSales.Count(s => s.Status == "Flagged");

        var prevMoves = await db.InventoryMovements.AsNoTracking()
            .Where(m => m.Timestamp >= prevStart && m.Timestamp < start)
            .Select(m => m.Quantity).ToListAsync();
        data.PrevMovementUnits = prevMoves.Sum(q => Math.Abs(q));

        data.ServiceRevenue = await db.Appointments
            .Where(a => a.Status == "Completed" && a.ScheduledAt >= start && a.ScheduledAt < end)
            .SumAsync(a => a.Price);

        var products = await db.InventoryItems.AsNoTracking().Select(i => new { i.Name, i.Category }).ToListAsync();
        data.Products = products.GroupBy(p => p.Name).ToDictionary(g => g.Key, g => g.First().Category);

        data.RecentActivity = await db.AuditLogs.AsNoTracking()
            .Where(a => ReportActions.Contains(a.Action))
            .OrderByDescending(a => a.Timestamp).Take(5).ToListAsync();

        return data;
    }

    // ---------- record / audit ----------
    public async Task<string?> RecordAsync(RetailSale form, string actor)
    {
        var type = RetailSale.Types.Contains(form.TransactionType) ? form.TransactionType : "Sale";
        var product = (form.ProductName ?? "").Trim();
        if (product == "") return "Product or service is required.";
        if (form.Quantity < 1) return "Quantity must be at least 1.";
        if (type != "Adjustment" && form.Total <= 0) return "Amount must be above zero.";
        if (type == "Adjustment" && form.Total == 0) return "Amount can't be zero.";

        var total = type switch
        {
            "Return" => -Math.Abs(form.Total),
            "Sale" => Math.Abs(form.Total),
            _ => form.Total
        };

        var local = PhTime.ToLocal(form.SoldAt);
        var sale = new RetailSale
        {
            SoldAt = form.SoldAt,
            Total = total,
            TransactionType = type,
            ProductName = product,
            Category = string.IsNullOrWhiteSpace(form.Category) ? null : form.Category,
            Quantity = form.Quantity,
            Status = "Pending",
            StaffName = actor,
            Notes = string.IsNullOrWhiteSpace(form.Notes) ? null : form.Notes.Trim(),
            ReferenceNo = await NextReference(type, DateOnly.FromDateTime(local))
        };
        db.RetailSales.Add(sale);
        await db.SaveChangesAsync();
        await Log($"{type} logged", $"{sale.ReferenceNo} - {product} x{sale.Quantity}", actor);
        return null;
    }

    public async Task SetStatusAsync(IEnumerable<int> ids, string status, string actor)
    {
        if (!RetailSale.Statuses.Contains(status)) return;
        var list = await db.RetailSales.Where(s => ids.Contains(s.RetailSaleId) && s.Status != status).ToListAsync();
        if (list.Count == 0) return;
        foreach (var s in list) s.Status = status;
        await db.SaveChangesAsync();

        var action = status switch
        {
            "Audited" => "Transaction audited",
            "Flagged" => "Transaction flagged",
            _ => "Transaction reopened"
        };
        var details = list.Count == 1
            ? $"{DisplayRef(list[0])} - {list[0].ProductName}"
            : $"{string.Join(", ", list.Take(3).Select(DisplayRef))}{(list.Count > 3 ? $" +{list.Count - 3} more" : "")}";
        await Log(action, details, actor);
    }

    // Rows created before references existed get a stable derived one.
    public static string DisplayRef(RetailSale s) =>
        !string.IsNullOrWhiteSpace(s.ReferenceNo)
            ? s.ReferenceNo
            : $"{Prefix(s.TransactionType)}-{PhTime.ToLocal(s.SoldAt):yyyyMMdd}-{s.RetailSaleId:000}";

    private static string Prefix(string type) => type switch { "Return" => "RET", "Adjustment" => "ADJ", _ => "INV" };

    private async Task<string> NextReference(string type, DateOnly day)
    {
        var prefix = $"{Prefix(type)}-{day:yyyyMMdd}-";
        var count = await db.RetailSales.CountAsync(s => s.ReferenceNo.StartsWith(prefix));
        string candidate;
        do { candidate = $"{prefix}{++count:000}"; }
        while (await db.RetailSales.AnyAsync(s => s.ReferenceNo == candidate));
        return candidate;
    }

    private async Task Log(string action, string details, string actor)
    {
        db.AuditLogs.Add(new AuditLog { Action = action, Details = details, UserName = actor });
        await db.SaveChangesAsync();
    }
}
