using Microsoft.Data.SqlClient;
using MyApp.Controls;

namespace MyApp;

internal sealed class ServiceDialog : DialogBase
{
    private static readonly string[] Categories = { "Grooming", "Health & Wellness", "Add-ons", "Others" };
    private static readonly string[] Sizes = { "XS", "Small", "Medium", "Large", "XL", "Giant", "All" };
    private static readonly string[] Species = { "Both", "Dog", "Cat" };

    private readonly ServiceRepository.ServiceRow? _existing;
    private readonly TextBox nameBox = new();
    private readonly PawSelect categoryBox = new();
    private readonly PawSelect sizeBox = new();
    private readonly PawSelect speciesBox = new();
    private readonly NumericUpDown minKgBox = new() { DecimalPlaces = 1, Maximum = 200, Increment = 1 };
    private readonly NumericUpDown maxKgBox = new() { DecimalPlaces = 1, Maximum = 200, Increment = 1 };
    private readonly TextBox descBox = new() { PlaceholderText = "Optional" };
    private readonly NumericUpDown priceBox = new() { DecimalPlaces = 2, Maximum = 100000, Increment = 50 };
    private readonly NumericUpDown minutesBox = new() { Minimum = 5, Maximum = 600, Increment = 5, Value = 60 };

    /// <summary>Name of the saved service, so AdminForm can write it to the audit log.</summary>
    public string? SavedName { get; private set; }
    /// <summary>The service's id, for the audit log reference.</summary>
    public int SavedId { get; private set; }

    public ServiceDialog(ServiceRepository.ServiceRow? existing = null)
        : base(existing == null ? "Add Service" : "Edit Service", "Save", "\uE70F",
               existing == null ? "Name it, price it, set how long it takes" : existing.Name)
    {
        _existing = existing;

        categoryBox.Items.AddRange(Categories.Select(c => (object)c).ToArray());
        sizeBox.Items.AddRange(Sizes.Select(s => (object)s).ToArray());
        speciesBox.Items.AddRange(Species.Select(s => (object)s).ToArray());
        categoryBox.SelectedIndex = 0;
        sizeBox.SelectedIndex = Array.IndexOf(Sizes, "Small");
        speciesBox.SelectedIndex = 0;

        if (existing != null)
        {
            nameBox.Text = existing.Name;
            categoryBox.SelectedIndex = Math.Max(0, Array.IndexOf(Categories, existing.Category));
            sizeBox.SelectedIndex = Math.Max(0, Array.IndexOf(Sizes, existing.Size));
            speciesBox.SelectedIndex = Math.Max(0, Array.IndexOf(Species, existing.Species));
            minKgBox.Value = existing.MinKg ?? 0;
            maxKgBox.Value = existing.MaxKg ?? 0;
            descBox.Text = existing.Description ?? "";
            priceBox.Value = existing.Price;
            minutesBox.Value = existing.DurationMinutes;
        }

        AddRow("Service name *", nameBox);
        AddRow("Category", categoryBox);
        AddRow("Size", sizeBox);
        AddRow("Pet type", speciesBox);
        AddRow("Weight over (kg)", minKgBox);
        AddRow("Up to (kg)", maxKgBox);
        AddRow("Description", descBox);
        AddRow("Price (\u20B1)", priceBox);
        AddRow("Minutes", minutesBox);
        FitHeight();
    }

    protected override void OnSaveClicked()
    {
        if (string.IsNullOrWhiteSpace(nameBox.Text))
        {
            ShowError("Service name is required.");
            return;
        }

        string category = Categories[categoryBox.SelectedIndex];
        string size = Sizes[sizeBox.SelectedIndex];
        string species = Species[speciesBox.SelectedIndex];
        decimal? minKg = minKgBox.Value > 0 ? minKgBox.Value : null;   // 0 = no limit
        decimal? maxKg = maxKgBox.Value > 0 ? maxKgBox.Value : null;
        if (minKg is decimal lo && maxKg is decimal hi && hi <= lo)
        {
            ShowError("'Up to' must be higher than 'Weight over'.");
            return;
        }

        try
        {
            if (_existing == null)
                SavedId = ServiceRepository.Add(nameBox.Text, category, size, descBox.Text, priceBox.Value, (int)minutesBox.Value, species, minKg, maxKg);
            else
            {
                ServiceRepository.Update(_existing.Id, nameBox.Text, category, size, descBox.Text, priceBox.Value, (int)minutesBox.Value, species, minKg, maxKg);
                SavedId = _existing.Id;
            }

            SavedName = $"{nameBox.Text.Trim()} ({size})";
            DialogResult = DialogResult.OK;
        }
        catch (SqlException ex) when (ex.Number is 2627 or 2601)
        {
            ShowError("That service already exists for this size.");
        }
        catch (SqlException ex)
        {
            ShowError("Could not save: " + ex.Message);
        }
    }
}
