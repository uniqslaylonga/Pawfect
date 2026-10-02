namespace Pawfect.Models;

public class InventoryItem
{
    public int InventoryItemId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public int ReorderLevel { get; set; }
    public DateOnly? ExpiryDate { get; set; }
    public decimal UnitPrice { get; set; }
}
