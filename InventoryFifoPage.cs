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

    public event Action<int, bool>? RowStockRequested;

    private List<InventoryRepository.ProductRow> _products = new();
    private List<InventoryRepository.BatchRow> _batches = new();

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
        inventorySearch.Box.TextChanged += (_, _) => ApplyInventoryFilter();
    }

    public void LoadInventory(List<InventoryRepository.ProductRow> products, List<InventoryRepository.BatchRow> batches)
    {
        _products = products;
        _batches = batches;
        ApplyInventoryFilter();
    }

    private sealed record InvLine(int ProductId, string Product, string Category, string Batch, string Supplier,
                                  string Received, string Expiry, string Qty, int QtyValue, string Status);

    private static Color StatusColor(string status) => status switch
    {
        "In Stock" => Color.FromArgb(46, 158, 91),
        "Low Stock" or "Expiring Soon" => Color.FromArgb(217, 119, 6),
        _ => Color.FromArgb(220, 53, 69),   // Out of Stock, Expired
    };

    private void ApplyInventoryFilter()
    {
        var today = DateTime.Today;
        var active = _products.Where(p => p.Active).OrderBy(p => p.Name).ToList();
        var totals = _batches.GroupBy(b => b.ProductId).ToDictionary(g => g.Key, g => g.Sum(b => b.Remaining));

        // One line per batch (oldest first); a product with no stock gets a single "Out of Stock" line.
        var lines = new List<InvLine>();
        foreach (var p in active)
        {
            int total = totals.GetValueOrDefault(p.Id);
            string stock = total == 0 ? "Out of Stock" : total <= p.ReorderLevel ? "Low Stock" : "In Stock";
            var mine = _batches.Where(b => b.ProductId == p.Id).ToList();

            if (mine.Count == 0)
                lines.Add(new InvLine(p.Id, p.Name, p.Category, "—", "—", "—", "—", $"0 {p.Unit}", 0, stock));

            foreach (var b in mine)
            {
                string status = stock;
                if (b.Expiry is DateTime ex)
                {
                    if (ex.Date < today) status = "Expired";
                    else if (ex.Date <= today.AddDays(30)) status = "Expiring Soon";
                }
                lines.Add(new InvLine(p.Id, p.Name, p.Category, b.BatchNo, b.Supplier ?? "—",
                    b.Received.ToString("MMM d, yyyy"), b.Expiry?.ToString("MMM d, yyyy") ?? "—",
                    $"{b.Remaining} {p.Unit}", b.Remaining, status));
            }
        }

        string q = inventorySearch.Value.Trim();
        var shown = lines.Where(l =>
            q.Length == 0 ||
            l.Product.Contains(q, StringComparison.OrdinalIgnoreCase) ||
            l.Category.Contains(q, StringComparison.OrdinalIgnoreCase) ||
            l.Batch.Contains(q, StringComparison.OrdinalIgnoreCase) ||
            l.Supplier.Contains(q, StringComparison.OrdinalIgnoreCase)).ToList();

        inventoryRows.SuspendLayout();

        foreach (var old in inventoryRows.Controls.Cast<Control>().Where(c => c != inventoryEmpty).ToList())
        {
            inventoryRows.Controls.Remove(old);
            old.Dispose();
        }

        inventoryEmpty.Visible = shown.Count == 0;

        for (int i = shown.Count - 1; i >= 0; i--)
            inventoryRows.Controls.Add(BuildInventoryRow(shown[i]));

        // Tiles always cover everything, not just the search result.
        invProductsValue.Text = active.Count.ToString();
        invQuantityValue.Text = _batches.Sum(b => b.Remaining).ToString();
        invLowStockValue.Text = active.Count(p => totals.GetValueOrDefault(p.Id) <= p.ReorderLevel).ToString();
        invExpiryValue.Text = _batches.Count(b => b.Expiry is DateTime e && e.Date <= today.AddDays(30)).ToString();

        inventoryShowing.Text = shown.Count == 0
            ? "No stock"
            : $"Showing 1 to {shown.Count} of {lines.Count} items";

        inventoryRows.ResumeLayout(true);
    }

    private Control BuildInventoryRow(InvLine l)
    {
        var grid = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.White,
            Margin = Padding.Empty,
            ColumnCount = inventoryColumnsGrid.ColumnCount,
            RowCount = 1,
        };
        foreach (ColumnStyle s in inventoryColumnsGrid.ColumnStyles)
            grid.ColumnStyles.Add(new ColumnStyle(s.SizeType, s.Width));
        grid.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

        void Put(Control header, string text, Color? color = null)
        {
            grid.Controls.Add(new Label
            {
                Text = text,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                AutoEllipsis = true,
                UseMnemonic = false,
                BackColor = Color.White,
                ForeColor = color ?? Color.FromArgb(31, 42, 60),
                Font = new Font("Segoe UI", 9F),
            }, inventoryColumnsGrid.GetColumn(header), 0);
        }

        Put(inventoryColProduct, l.Product);
        Put(inventoryColCategory, l.Category);
        Put(inventoryColBatch, l.Batch);
        Put(inventoryColSupplier, l.Supplier);
        Put(inventoryColReceived, l.Received);
        Put(inventoryColExpiry, l.Expiry);
        Put(inventoryColQuantity, l.Qty);
        Put(inventoryColStatus, l.Status, StatusColor(l.Status));

        var actions = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.White,
            WrapContents = false,
            Margin = Padding.Empty,
            Padding = new Padding(0, 14, 0, 0),
        };
        Label Link(string text, Action onClick)
        {
            var link = new Label
            {
                Text = text,
                AutoSize = true,
                Margin = new Padding(0, 0, 14, 0),
                BackColor = Color.White,
                ForeColor = Color.FromArgb(58, 123, 213),
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Cursor = Cursors.Hand,
                UseMnemonic = false,
            };
            link.Click += (_, _) => onClick();
            return link;
        }
        actions.Controls.Add(Link("Stock In", () => RowStockRequested?.Invoke(l.ProductId, true)));
        if (l.QtyValue > 0)
            actions.Controls.Add(Link("Stock Out", () => RowStockRequested?.Invoke(l.ProductId, false)));
        grid.Controls.Add(actions, inventoryColumnsGrid.GetColumn(inventoryColActions), 0);

        var row = new Panel { Dock = DockStyle.Top, Height = 44, BackColor = Color.White };
        row.Controls.Add(grid);
        return row;
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
