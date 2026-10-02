using Microsoft.EntityFrameworkCore;
using Pawfect.Data;
using Pawfect.Models;

namespace Pawfect.Services;

public class InventoryService(PawfectDbContext db)
{
    public const int NearExpiryDays = 30;

    // Philippines is UTC+8, no daylight saving (same convention as the other services)
    public static DateOnly Today => DateOnly.FromDateTime(DateTime.UtcNow.AddHours(8));

    public static string StatusOf(InventoryItem i, DateOnly today)
    {
        if (i.ExpiryDate is { } e && e < today) return "Expired";
        if (i.Quantity <= 0) return "Out of Stock";
        if (i.Quantity <= i.ReorderLevel) return "Low Stock";
        if (i.ExpiryDate is { } n && n <= today.AddDays(NearExpiryDays)) return "Expiring Soon";
        return "In Stock";
    }

    public async Task<List<InventoryItem>> LoadAsync() =>
        await db.InventoryItems.AsNoTracking()
            .OrderBy(i => i.Name).ThenBy(i => i.ReceivedDate).ThenBy(i => i.InventoryItemId)
            .ToListAsync();

    public async Task<string?> AddAsync(InventoryItem item, string actor)
    {
        item.Name = item.Name.Trim();
        item.ReceivedDate ??= Today;
        if (string.IsNullOrWhiteSpace(item.BatchNumber))
            item.BatchNumber = await NextBatchNumber(item.ReceivedDate.Value);
        else if (await BatchTaken(item.BatchNumber, 0))
            return "That batch number is already used.";

        db.InventoryItems.Add(item);
        await db.SaveChangesAsync();
        await Log("Inventory added", $"{item.Name} batch {item.BatchNumber} - {item.Quantity} pcs received", actor);
        return null;
    }

    public async Task<string?> UpdateAsync(InventoryItem form, string actor)
    {
        var i = await db.InventoryItems.FindAsync(form.InventoryItemId);
        if (i is null) return "Batch not found.";
        if (!string.IsNullOrWhiteSpace(form.BatchNumber) && await BatchTaken(form.BatchNumber, form.InventoryItemId))
            return "That batch number is already used.";

        i.Name = form.Name.Trim();
        i.Unit = form.Unit;
        i.Category = form.Category;
        i.Supplier = form.Supplier;
        i.BatchNumber = string.IsNullOrWhiteSpace(form.BatchNumber) ? i.BatchNumber : form.BatchNumber.Trim();
        i.ReceivedDate = form.ReceivedDate;
        i.ExpiryDate = form.ExpiryDate;
        i.Quantity = form.Quantity;
        i.ReorderLevel = form.ReorderLevel;
        i.UnitPrice = form.UnitPrice;
        await db.SaveChangesAsync();
        await Log("Inventory updated", $"{i.Name} batch {i.BatchNumber} updated", actor);
        return null;
    }

    public async Task DeleteAsync(IEnumerable<int> ids, string actor)
    {
        var list = await db.InventoryItems.Where(i => ids.Contains(i.InventoryItemId)).ToListAsync();
        if (list.Count == 0) return;
        db.InventoryItems.RemoveRange(list);
        await db.SaveChangesAsync();
        await Log("Inventory removed",
            list.Count == 1 ? $"{list[0].Name} batch {list[0].BatchNumber} removed" : $"{list.Count} batches removed", actor);
    }

    // Add stock to, or set the quantity of, one batch.
    public async Task<string?> AdjustBatchAsync(int id, int amount, bool setExact, string actor)
    {
        var i = await db.InventoryItems.FindAsync(id);
        if (i is null) return "Batch not found.";
        var newQty = setExact ? amount : i.Quantity + amount;
        if (newQty < 0) return "Quantity can't go below zero.";
        var old = i.Quantity;
        i.Quantity = newQty;
        await db.SaveChangesAsync();
        await Log("Stock updated", $"{i.Name} batch {i.BatchNumber}: {old} -> {newQty} pcs", actor);
        return null;
    }

    // FIFO: take stock from the oldest received, non-expired batches of a product first.
    public async Task<string?> StockOutFifoAsync(string product, int qty, string actor)
    {
        if (qty <= 0) return "Quantity must be at least 1.";
        var today = Today;
        var batches = (await db.InventoryItems
                .Where(i => i.Name == product && i.Quantity > 0)
                .ToListAsync())
            .Where(i => i.ExpiryDate is null || i.ExpiryDate >= today)
            .OrderBy(i => i.ReceivedDate ?? DateOnly.MinValue)
            .ThenBy(i => i.ExpiryDate ?? DateOnly.MaxValue)
            .ThenBy(i => i.InventoryItemId)
            .ToList();

        var available = batches.Sum(b => b.Quantity);
        if (available < qty) return $"Only {available} pcs of {product} are available (expired batches are skipped).";

        var left = qty;
        var used = new List<string>();
        foreach (var b in batches)
        {
            if (left == 0) break;
            var take = Math.Min(left, b.Quantity);
            b.Quantity -= take;
            left -= take;
            used.Add($"{b.BatchNumber} (-{take})");
        }
        await db.SaveChangesAsync();
        await Log("Stock used (FIFO)", $"{product}: {qty} pcs from {string.Join(", ", used)}", actor);
        return null;
    }

    private async Task<string> NextBatchNumber(DateOnly received)
    {
        var prefix = $"B-{received:yyyyMMdd}";
        var count = await db.InventoryItems.CountAsync(i => i.BatchNumber.StartsWith(prefix));
        string candidate;
        do { candidate = $"{prefix}{++count:00}"; }
        while (await db.InventoryItems.AnyAsync(i => i.BatchNumber == candidate));
        return candidate;
    }

    private Task<bool> BatchTaken(string batch, int exceptId) =>
        db.InventoryItems.AnyAsync(i => i.BatchNumber == batch.Trim() && i.InventoryItemId != exceptId);

    private async Task Log(string action, string details, string actor)
    {
        db.AuditLogs.Add(new AuditLog { Action = action, Details = details, UserName = actor });
        await db.SaveChangesAsync();
    }
}
