using Microsoft.Data.SqlClient;
using MyApp.Controls;

namespace MyApp;

internal sealed class StockDialog : ThemedDialog
{
    private readonly string _actor;
    private readonly List<InventoryRepository.ProductRow> _products;
    private readonly ComboBox productBox, typeBox;
    private readonly NumericUpDown qtyBox;
    private readonly InputField batchBox, supplierBox, reasonBox;
    private readonly DateTimePicker expiryPicker;
    private readonly CheckBox noExpiry;

    /// <summary>Filled after a successful save, so AdminForm can write the audit log.</summary>
    public string AuditAction { get; private set; } = "";
    public string AuditDetails { get; private set; } = "";

    public StockDialog(List<InventoryRepository.ProductRow> products, string actor, int? productId, bool? stockIn)
        : base("Update Stock", "Record stock coming in or going out. Stock out uses the oldest batch first.")
    {
        _actor = actor;
        _products = products;

        var selected = products.FirstOrDefault(p => p.Id == productId)?.Name;
        productBox = AddCombo("Product", products.Select(p => p.Name), selected);
        typeBox = AddCombo("Action", new[] { "Stock In", "Stock Out" }, stockIn == false ? "Stock Out" : "Stock In");
        qtyBox = AddNumber("Quantity", 1, 1, 100000);
        batchBox = AddInput("Batch number (Stock In)", "\uE8EC", "Leave empty to auto-generate");
        supplierBox = AddInput("Supplier (Stock In)", "\uE716", "e.g. PetCare Supplies");
        expiryPicker = AddDate("Expiry date (Stock In)", DateTime.Today.AddMonths(6));
        noExpiry = AddCheck("No expiry date");
        reasonBox = AddInput("Reason (Stock Out)", "\uE946", "e.g. Used in grooming");

        typeBox.SelectedIndexChanged += (_, _) => UpdateState();
        noExpiry.CheckedChanged += (_, _) => UpdateState();

        AddButtons("Save", Save_Click);
        UpdateState();
    }

    private void UpdateState()
    {
        bool isIn = typeBox.SelectedIndex == 0;
        batchBox.Enabled = supplierBox.Enabled = noExpiry.Enabled = isIn;
        expiryPicker.Enabled = isIn && !noExpiry.Checked;
        reasonBox.Enabled = !isIn;
    }

    private void Save_Click(object? sender, EventArgs e)
    {
        if (productBox.SelectedIndex < 0)
        {
            MessageBox.Show(this, "Choose a product.");
            return;
        }

        var product = _products[productBox.SelectedIndex];
        int qty = (int)qtyBox.Value;
        try
        {
            if (typeBox.SelectedIndex == 0)
            {
                InventoryRepository.StockIn(product.Id, batchBox.Value, supplierBox.Value,
                    noExpiry.Checked ? null : expiryPicker.Value.Date, qty, _actor);
                AuditAction = "Stock In";
                AuditDetails = $"Added {qty} {product.Unit} of {product.Name}";
            }
            else
            {
                var (ok, available) = InventoryRepository.StockOut(product.Id, qty, reasonBox.Value, _actor);
                if (!ok)
                {
                    MessageBox.Show(this, $"Only {available} {product.Unit} of {product.Name} in stock.");
                    return;
                }
                AuditAction = "Stock Out";
                AuditDetails = $"Removed {qty} {product.Unit} of {product.Name} (oldest batches first)";
            }
            DialogResult = DialogResult.OK;
        }
        catch (SqlException ex) when (ex.Number is 2627 or 2601)
        {
            MessageBox.Show(this, "That batch number already exists for this product.");
        }
        catch (SqlException ex)
        {
            MessageBox.Show(this, "Could not save: " + ex.Message);
        }
    }
}