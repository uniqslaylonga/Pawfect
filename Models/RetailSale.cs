namespace Pawfect.Models;

// A walk-in retail sale
public class RetailSale
{
    public int RetailSaleId { get; set; }
    public DateTime SoldAt { get; set; } = DateTime.UtcNow;
    public decimal Total { get; set; }
}
