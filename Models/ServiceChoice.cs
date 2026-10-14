namespace MyApp;

/// <summary>A grooming service shown as a card on step 4 of the customer flow (price and duration come from dbo.Services).</summary>
internal sealed record ServiceChoice(int Id, string Name, decimal Price, int DurationMinutes);
