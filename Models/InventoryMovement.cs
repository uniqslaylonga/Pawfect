namespace Pawfect.Models;

// One stock movement (in / out / adjustment) written by InventoryService. Powers the
// Inventory Movement report.
public class InventoryMovement
{
    public static readonly string[] Types = { "Stock In", "Stock Out", "Adjustment" };

    public int InventoryMovementId { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;   // store as UTC
    public string ProductName { get; set; } = string.Empty;
    public string? Category { get; set; }
    public string? BatchNumber { get; set; }
    public string MovementType { get; set; } = "Stock In";       // Stock In, Stock Out, Adjustment
    public int Quantity { get; set; }                            // signed: negative = stock removed
    public string? Reason { get; set; }
    public string? UserName { get; set; }
}
