using Microsoft.Data.SqlClient;
using MyApp.Controls;

namespace MyApp;

internal sealed class ServiceDialog : DialogBase
{
    private static readonly string[] Categories = { "Grooming", "Health & Wellness", "Add-ons", "Others" };
    private static readonly string[] Sizes = { "Small", "Medium", "Large", "Giant", "All" };

    private readonly ServiceRepository.ServiceRow? _existing;
    private readonly TextBox nameBox = new();
    private readonly PawSelect categoryBox = new();
    private readonly PawSelect sizeBox = new();
    private readonly TextBox descBox = new() { PlaceholderText = "Optional" };
    private readonly NumericUpDown priceBox = new() { DecimalPlaces = 2, Maximum = 100000, Increment = 50 };
    private readonly NumericUpDown minutesBox = new() { Minimum = 5, Maximum = 600, Increment = 5, Value = 60 };

    /// <summary>Name of the saved service, so AdminForm can write it to the audit log.</summary>
    public string? SavedName { get; private set; }

    public ServiceDialog(ServiceRepository.ServiceRow? existing = null)
        : base(existing == null ? "Add Service" : "Edit Service", "Save", "\uE70F",
               existing == null ? "Name it, price it, set how long it takes" : existing.Name)
    {
        _existing = existing;

        categoryBox.Items.AddRange(Categories.Select(c => (object)c).ToArray());
        sizeBox.Items.AddRange(Sizes.Select(s => (object)s).ToArray());
        categoryBox.SelectedIndex = 0;
        sizeBox.SelectedIndex = 0;

        if (existing != null)
        {
            nameBox.Text = existing.Name;
            categoryBox.SelectedIndex = Math.Max(0, Array.IndexOf(Categories, existing.Category));
            sizeBox.SelectedIndex = Math.Max(0, Array.IndexOf(Sizes, existing.Size));
            descBox.Text = existing.Description ?? "";
            priceBox.Value = existing.Price;
            minutesBox.Value = existing.DurationMinutes;
        }

        AddRow("Service name *", nameBox);
        AddRow("Category", categoryBox);
        AddRow("Size", sizeBox);
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

        try
        {
            if (_existing == null)
                ServiceRepository.Add(nameBox.Text, category, size, descBox.Text, priceBox.Value, (int)minutesBox.Value);
            else
                ServiceRepository.Update(_existing.Id, nameBox.Text, category, size, descBox.Text, priceBox.Value, (int)minutesBox.Value);

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
