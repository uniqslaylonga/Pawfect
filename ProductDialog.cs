using Microsoft.Data.SqlClient;
using MyApp.Controls;

namespace MyApp;

/// <summary>Add a new product or edit an existing one (Inventory page). New products start with 0 stock; use Restock to receive the first batch.</summary>
internal sealed class ProductDialog : DialogBase
{
    private readonly InventoryProduct? _existing;
    private readonly TextBox codeBox = new() { PlaceholderText = "e.g. FO-DOG-013" };
    private readonly TextBox nameBox = new();
    private readonly TextBox categoryBox = new() { PlaceholderText = "e.g. Dry Dog Food" };
    private readonly NumericUpDown priceBox = new() { DecimalPlaces = 2, Maximum = 1000000, Increment = 5 };
    private readonly NumericUpDown costBox = new() { DecimalPlaces = 2, Maximum = 1000000, Increment = 5 };
    private readonly NumericUpDown reorderBox = new() { Maximum = 100000, Value = 3 };

    /// <summary>Name of the saved product, so AdminForm can write it to the audit log.</summary>
    public string? SavedName { get; private set; }

    public ProductDialog(InventoryProduct? existing = null)
        : base(existing == null ? "Add Product" : "Edit Product", "Save", "\uE7B8",
               existing == null ? "Name it, price it, set when to reorder" : existing.Name)
    {
        _existing = existing;

        if (existing != null)
        {
            codeBox.Text = existing.Code;
            nameBox.Text = existing.Name;
            categoryBox.Text = existing.Category;
            priceBox.Value = Math.Min(priceBox.Maximum, existing.Price);
            costBox.Value = Math.Min(costBox.Maximum, existing.UnitCost ?? 0);
            reorderBox.Value = Math.Min(reorderBox.Maximum, existing.ReorderLevel);
        }

        AddRow("Product code *", codeBox);
        AddRow("Product name *", nameBox);
        AddRow("Category *", categoryBox);
        AddRow("Selling price (\u20B1) *", priceBox);
        AddRow("Unit cost (\u20B1)", costBox);
        AddRow("Reorder level", reorderBox);
        FitHeight();
    }

    protected override void OnSaveClicked()
    {
        if (string.IsNullOrWhiteSpace(codeBox.Text)) { ShowError("Product code is required."); return; }
        if (string.IsNullOrWhiteSpace(nameBox.Text)) { ShowError("Product name is required."); return; }
        if (string.IsNullOrWhiteSpace(categoryBox.Text)) { ShowError("Category is required."); return; }
        if (priceBox.Value <= 0) { ShowError("Enter the selling price."); return; }

        decimal? cost = costBox.Value > 0 ? costBox.Value : null;
        int reorder = (int)reorderBox.Value;

        try
        {
            if (_existing == null)
                InventoryData.AddProduct(codeBox.Text, nameBox.Text, categoryBox.Text, priceBox.Value, cost, reorder);
            else
                InventoryData.UpdateProduct(_existing.Id, codeBox.Text, nameBox.Text, categoryBox.Text, priceBox.Value, cost, reorder);

            SavedName = $"{nameBox.Text.Trim()} ({codeBox.Text.Trim()})";
            DialogResult = DialogResult.OK;
        }
        catch (SqlException ex) when (ex.Number is 2627 or 2601)
        {
            ShowError("That product code is already used by another product.");
        }
        catch (SqlException ex)
        {
            ShowError("Could not save: " + ex.Message);
        }
    }
}
