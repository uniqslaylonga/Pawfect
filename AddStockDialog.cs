using Microsoft.Data.SqlClient;
using MyApp.Controls;

namespace MyApp;

/// <summary>Restock: pick a product and record the delivery as a new batch (units, supplier, dates). Opened from Inventory > Update Stock / Restock (admin).</summary>
internal sealed class AddStockDialog : DialogBase
{
    private readonly List<ProductItem> _products;
    private readonly PawSelect productBox = new();
    private readonly NumericUpDown quantityBox = new() { Minimum = 1, Maximum = 100000, Value = 1 };
    private readonly TextBox batchBox = new() { PlaceholderText = "Optional - one is made up if left empty" };
    private readonly TextBox supplierBox = new() { PlaceholderText = "Optional" };
    private readonly PawDatePicker receivedBox = new();
    private readonly PawDatePicker expiryBox = new() { AllowEmpty = true, Placeholder = "No expiry date" };

    /// <summary>What was added, e.g. "Added 12 x Dog Shampoo (P0001) in batch B20261021-001. Now 20 in stock." Written to the audit log.</summary>
    public string? Summary { get; private set; }
    /// <summary>The product that received stock, for the audit log reference.</summary>
    public int ProductId { get; private set; }

    public AddStockDialog(int? productId = null)
        : base("Update Stock", "Add Stock", "\uE719", "Record a delivery as a new batch")
    {
        _products = EmployeeData.GetProducts();   // may throw SqlException: the caller shows it

        ClientSize = new Size(600, ClientSize.Height);   // wider, so long product names are not cut off

        // Typing narrows the list (name, code or stock), and only a product from the list can be picked.
        productBox.Searchable = true;
        productBox.SearchPlaceholder = "Type a name or code, e.g. aozi or FO-CAT";
        productBox.EmptyText = "No products yet";
        productBox.Items.AddRange(_products
            .Select(p => (object)$"{p.Name}  ({p.Code})  -  {p.QtyOnHand} in stock").ToArray());
        if (_products.Count > 0)
            productBox.SelectedIndex = productId is int id ? Math.Max(0, _products.FindIndex(p => p.Id == id)) : 0;

        receivedBox.MaxDate = DateTime.Today;
        receivedBox.Value = DateTime.Today;
        expiryBox.Checked = false;

        AddRow("Product *", productBox);
        AddRow("Units to add *", quantityBox);
        AddRow("Batch number", batchBox);
        AddRow("Supplier", supplierBox);
        AddRow("Received date *", receivedBox);
        AddRow("Expiry date", expiryBox);
        FitHeight();
    }

    protected override void OnSaveClicked()
    {
        if (_products.Count == 0)
        {
            ShowError("There are no products yet. Add the product first.");
            return;
        }
        if (productBox.SelectedIndex < 0)
        {
            ShowError("Choose a product.");
            return;
        }

        DateTime received = receivedBox.Value.Date;
        DateTime? expiry = expiryBox.Checked ? expiryBox.Value.Date : null;
        if (expiry is DateTime e && e < received)
        {
            ShowError("The expiry date can't be before the received date.");
            return;
        }

        var product = _products[productBox.SelectedIndex];
        int quantity = (int)quantityBox.Value;

        try
        {
            var (now, batchNo) = InventoryData.ReceiveBatch(product.Id, quantity, batchBox.Text, supplierBox.Text, received, expiry);
            ProductId = product.Id;
            Summary = $"Added {quantity} x {product.Name} ({product.Code}) in batch {batchNo}. Now {now} in stock.";
            DialogResult = DialogResult.OK;
        }
        catch (InvalidOperationException ex)
        {
            ShowError(ex.Message);
        }
        catch (SqlException ex)
        {
            ShowError("Could not save: " + ex.Message);
        }
    }
}
