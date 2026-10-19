using Microsoft.Data.SqlClient;
using MyApp.Controls;

namespace MyApp;

internal sealed class ProductDialog : ThemedDialog
{
    private readonly InputField nameBox;
    private readonly ComboBox categoryBox, unitBox;
    private readonly NumericUpDown reorderBox;

    public string? SavedName { get; private set; }

    public ProductDialog()
        : base("Add Product", "Add an item you keep in stock.")
    {
        nameBox = AddInput("Product name", "\uE719", "e.g. Oatmeal Shampoo");
        categoryBox = AddCombo("Category", InventoryRepository.Categories);
        unitBox = AddCombo("Unit", InventoryRepository.Units);
        reorderBox = AddNumber("Low-stock level (reorder at or below)", 10, 0, 100000);

        AddButtons("Add Product", Save_Click);
    }

    private void Save_Click(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(nameBox.Value))
        {
            MessageBox.Show(this, "Product name is required.");
            return;
        }

        try
        {
            InventoryRepository.AddProduct(nameBox.Value, (string)categoryBox.SelectedItem!,
                                           (string)unitBox.SelectedItem!, (int)reorderBox.Value);
            SavedName = nameBox.Value.Trim();
            DialogResult = DialogResult.OK;
        }
        catch (SqlException ex) when (ex.Number is 2627 or 2601)
        {
            MessageBox.Show(this, "A product with that name already exists.");
        }
        catch (SqlException ex)
        {
            MessageBox.Show(this, "Could not save: " + ex.Message);
        }
    }
}