namespace Pawfect.Models;

// A retail transaction (walk-in sale, return, or stock/price adjustment) that can be audited.
public class RetailSale
{
    public static readonly string[] Types = { "Sale", "Return", "Adjustment" };
    public static readonly string[] Statuses = { "Pending", "Audited", "Flagged" };

    public int RetailSaleId { get; set; }
    public DateTime SoldAt { get; set; } = DateTime.UtcNow;   // store as UTC

    // Signed: returns are stored as negative amounts.
    public decimal Total { get; set; }

    public string ReferenceNo { get; set; } = string.Empty;   // INV-/RET-/ADJ-yyyyMMdd-001
    public string TransactionType { get; set; } = "Sale";     // Sale, Return, Adjustment
    public string? ProductName { get; set; }
    public string? Category { get; set; }
    public int Quantity { get; set; } = 1;
    public string Status { get; set; } = "Pending";           // Pending, Audited, Flagged
    public string? StaffName { get; set; }
    public string? Notes { get; set; }
}
