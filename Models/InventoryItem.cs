namespace Pawfect.Models;

// One row = one stock batch of a product (that is what FIFO works on).
public class InventoryItem
{
    public static readonly string[] Categories =
        { "Pet Food", "Grooming", "Accessories", "Supplies", "Health", "Toys", "Treats" };

    public int InventoryItemId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public int ReorderLevel { get; set; }
    public DateOnly? ExpiryDate { get; set; }
    public decimal UnitPrice { get; set; }

    public string Category { get; set; } = "Pet Food";
    public string? Supplier { get; set; }
    public string BatchNumber { get; set; } = string.Empty;
    public string? Unit { get; set; }          // pack size, e.g. "2kg" or "250ml"
    public DateOnly? ReceivedDate { get; set; }
}
