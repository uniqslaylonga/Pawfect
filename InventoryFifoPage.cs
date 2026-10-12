using MyApp.Controls;

namespace MyApp;

/// <summary>
/// Inventory &amp; FIFO Management screen. Layout lives in InventoryFifoPage.Designer.cs, so open this file's designer
/// in Visual Studio to move or restyle anything. Nothing is pre-filled: counts show 0 and the table, alerts and
/// low-stock list stay empty until products and batches are loaded.
/// </summary>
internal partial class InventoryFifoPage : UserControl
{
    public event EventHandler? AddProductRequested;
    public event EventHandler? UpdateStockRequested;
    public event EventHandler? ViewBatchesRequested;
    public event EventHandler? GenerateReportRequested;
    public event EventHandler? ViewAllAlertsRequested;
    public event EventHandler? ViewAllLowStockRequested;

    public InventoryFifoPage()
    {
        InitializeComponent();
        DoubleBuffered = true;

        categoryFilter.SetItems(new[] { "All Categories", "Pet Food", "Grooming", "Accessories", "Supplies", "Health", "Toys", "Treats" });
        stockStatusFilter.SetItems(new[] { "All Stock Status", "In Stock", "Low Stock", "Out of Stock" });
        supplierFilter.SetItems(new[] { "All Suppliers" }); // filled from real suppliers once they exist

        addProductButton.Click += (_, _) => AddProductRequested?.Invoke(this, EventArgs.Empty);
        OnClick(addProductTile, (_, _) => AddProductRequested?.Invoke(this, EventArgs.Empty));
        OnClick(updateStockTile, (_, _) => UpdateStockRequested?.Invoke(this, EventArgs.Empty));
        OnClick(viewBatchesTile, (_, _) => ViewBatchesRequested?.Invoke(this, EventArgs.Empty));
        OnClick(generateReportTile, (_, _) => GenerateReportRequested?.Invoke(this, EventArgs.Empty));
        alertsLink.Click += (_, _) => ViewAllAlertsRequested?.Invoke(this, EventArgs.Empty);
        lowStockLink.Click += (_, _) => ViewAllLowStockRequested?.Invoke(this, EventArgs.Empty);

        listViewToggle.Click += (_, _) => SetView(list: true);
        gridViewToggle.Click += (_, _) => SetView(list: false);
    }

    /// <summary>True while the table is in list view, false in grid view.</summary>
    public bool IsListView { get; private set; } = true;

    private void SetView(bool list)
    {
        IsListView = list;
        Style(listViewToggle, active: list);
        Style(gridViewToggle, active: !list);

        static void Style(IconBadge toggle, bool active)
        {
            toggle.FillColor = active ? Theme.Primary : Theme.Subtle;
            toggle.ForeColor = active ? Color.White : Theme.Muted;
        }
    }

    // Child controls swallow clicks, so hook the tile and everything inside it.
    private static void OnClick(Control root, EventHandler handler)
    {
        root.Click += handler;
        foreach (Control child in root.Controls)
            OnClick(child, handler);
    }
}
