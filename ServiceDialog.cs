using Microsoft.Data.SqlClient;
using MyApp.Controls;

namespace MyApp;

internal sealed class ServiceDialog : ThemedDialog
{
    private readonly ServiceRepository.ServiceRow? _existing;
    private readonly InputField nameBox, descBox;
    private readonly ComboBox categoryBox, sizeBox;
    private readonly NumericUpDown priceBox, minutesBox;

    /// <summary>Name of the saved service, so AdminForm can write it to the audit log.</summary>
    public string? SavedName { get; private set; }

    public ServiceDialog(ServiceRepository.ServiceRow? existing = null)
        : base(existing == null ? "Add Service" : "Edit Service",
               existing == null ? "Add a grooming service to the catalog."
                                : "Update this service's details and price.")
    {
        _existing = existing;

        nameBox = AddInput("Service name", "\uE734", "e.g. Full Groom", existing?.Name);
        categoryBox = AddCombo("Category", new[] { "Grooming", "Health & Wellness", "Add-ons", "Others" }, existing?.Category);
        sizeBox = AddCombo("Size", new[] { "Small", "Medium", "Large", "Giant", "All" }, existing?.Size);
        descBox = AddInput("Description (optional)", "\uE946", "Short description", existing?.Description);
        priceBox = AddNumber("Price (₱)", existing?.Price ?? 0, 0, 100000, decimals: 2, step: 50);
        minutesBox = AddNumber("Duration (minutes)", existing?.DurationMinutes ?? 60, 5, 600, step: 5);

        AddButtons(existing == null ? "Add Service" : "Save Changes", Save_Click);
    }

    private void Save_Click(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(nameBox.Value))
        {
            MessageBox.Show(this, "Service name is required.");
            return;
        }

        string category = (string)categoryBox.SelectedItem!;
        string size = (string)sizeBox.SelectedItem!;

        try
        {
            if (_existing == null)
                ServiceRepository.Add(nameBox.Value, category, size, descBox.Value, priceBox.Value, (int)minutesBox.Value);
            else
                ServiceRepository.Update(_existing.Id, nameBox.Value, category, size, descBox.Value, priceBox.Value, (int)minutesBox.Value);

            SavedName = $"{nameBox.Value.Trim()} ({size})";
            DialogResult = DialogResult.OK;
        }
        catch (SqlException ex) when (ex.Number is 2627 or 2601)
        {
            MessageBox.Show(this, "That service already exists for this size.");
        }
        catch (SqlException ex)
        {
            MessageBox.Show(this, "Could not save: " + ex.Message);
        }
    }
}