using System.ComponentModel.DataAnnotations.Schema;

namespace Pawfect.Models;

public class ServiceItem
{
    public static readonly string[] Categories = { "Grooming", "Health & Wellness", "Add-ons", "Others" };
    public static readonly string[] Sizes = { "Small", "Medium", "Large", "Giant" };

    public int ServiceItemId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Category { get; set; } = "Grooming";

    // Comma-separated sizes (e.g. "Small,Medium"). Empty means the service applies to all sizes.
    public string SizeApplicability { get; set; } = string.Empty;

    public int DurationMinutes { get; set; } = 30;
    public decimal MinPrice { get; set; }
    public decimal MaxPrice { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime DateCreated { get; set; } = DateTime.UtcNow;

    [NotMapped]
    public string[] SizeList =>
        SizeApplicability.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

    [NotMapped]
    public string SizeText => SizeList.Length == 0 ? "All Sizes" : string.Join(" / ", SizeList);

    [NotMapped]
    public decimal Midpoint => (MinPrice + MaxPrice) / 2;
}
