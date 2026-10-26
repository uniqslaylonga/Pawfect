using System.Globalization;
using MyApp.Controls;
using static MyApp.EmployeeManagementPage;

namespace MyApp;

/// <summary>
/// Inventory &amp; FIFO Management screen. Layout lives in InventoryFifoPage.Designer.cs, so open this file's designer
/// in Visual Studio to move or restyle anything. Everything shown comes from dbo.Products / dbo.StockBatches through
/// Load(); with no products the counts show 0 and the table and alert lists stay empty.
/// </summary>
internal partial class InventoryFifoPage : UserControl
{
    private const int PageSize = 10;

    private List<InventoryRow> _all = new();
    private int _productsAdded;
    private int _unitsReceived;
    private int _page = 1;

    private readonly Panel alertsList = NewList();
    private readonly Panel lowStockList = NewList();

    public event EventHandler? AddProductRequested;
    public event EventHandler? UpdateStockRequested;
    public event EventHandler? ViewBatchesRequested;
    public event EventHandler? GenerateReportRequested;
    public event EventHandler? ViewAllAlertsRequested;
    public event EventHandler? ViewAllLowStockRequested;

    /// <summary>"Restock" on a table row or card: the product id to add a batch to.</summary>
    public event Action<int>? RestockRequested;

    /// <summary>"Edit" on a table row: the product id to edit.</summary>
    public event Action<int>? EditProductRequested;

    public InventoryFifoPage()
    {
        InitializeComponent();
        DoubleBuffered = true;

        categoryFilter.SetItems(new[] { "All Categories" });     // filled from the real products
        stockStatusFilter.SetItems(new[] { "All Stock Status", "In Stock", "Low Stock", "Out of Stock", "Near Expiry", "Expired" });
        supplierFilter.SetItems(new[] { "All Suppliers" });      // filled from the real batches

        alertsCard.Controls.Add(alertsList);
        alertsList.BringToFront();
        lowStockCard.Controls.Add(lowStockList);
        lowStockList.BringToFront();

        addProductButton.Click += (_, _) => AddProductRequested?.Invoke(this, EventArgs.Empty);
        OnClick(addProductTile, (_, _) => AddProductRequested?.Invoke(this, EventArgs.Empty));
        OnClick(updateStockTile, (_, _) => UpdateStockRequested?.Invoke(this, EventArgs.Empty));
        OnClick(viewBatchesTile, (_, _) => ViewBatchesRequested?.Invoke(this, EventArgs.Empty));
        OnClick(generateReportTile, (_, _) => GenerateReportRequested?.Invoke(this, EventArgs.Empty));
        alertsLink.Click += (_, _) => ViewAllAlertsRequested?.Invoke(this, EventArgs.Empty);
        lowStockLink.Click += (_, _) => ViewAllLowStockRequested?.Invoke(this, EventArgs.Empty);
        Ui.MakeClickable(invLowStockTile, (_, _) => ViewAllLowStockRequested?.Invoke(this, EventArgs.Empty));   // the "Low Stock Items" tile

        listViewToggle.Click += (_, _) => SetView(list: true);
        gridViewToggle.Click += (_, _) => SetView(list: false);

        inventorySearch.Box.TextChanged += (_, _) => { _page = 1; Apply(); };
        categoryFilter.SelectedIndexChanged += (_, _) => { _page = 1; Apply(); };
        stockStatusFilter.SelectedIndexChanged += (_, _) => { _page = 1; Apply(); };
        supplierFilter.SelectedIndexChanged += (_, _) => { _page = 1; Apply(); };
        inventoryPrevPage.Click += (_, _) => { _page--; Apply(); };
        inventoryNextPage.Click += (_, _) => { _page++; Apply(); };
    }

    /// <summary>True while the table is in list view, false in grid view.</summary>
    public bool IsListView { get; private set; } = true;

    private void SetView(bool list)
    {
        IsListView = list;
        Style(listViewToggle, active: list);
        Style(gridViewToggle, active: !list);
        Apply();

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

    private static Panel NewList() => new() { Dock = DockStyle.Fill, AutoScroll = true, BackColor = Color.White, Visible = false };

    // ------------------------------------------------------------------ data

    /// <summary>Stores the products and batches from the database, then shows them through the current filters.</summary>
    public void Load(List<InventoryRow> rows, int productsAddedThisMonth, int unitsReceivedThisMonth)
    {
        _all = rows;
        _productsAdded = productsAddedThisMonth;
        _unitsReceived = unitsReceivedThisMonth;

        // Filters list what really exists; SetItems keeps the current choice when it is still there.
        categoryFilter.SetItems(new[] { "All Categories" }.Concat(Distinct(rows.Select(r => r.Category))));
        supplierFilter.SetItems(new[] { "All Suppliers" }.Concat(Distinct(rows.Select(r => r.Supplier))));

        Apply();
    }

    private static IEnumerable<string> Distinct(IEnumerable<string> values) =>
        values.Where(v => !string.IsNullOrWhiteSpace(v))
              .Distinct(StringComparer.OrdinalIgnoreCase)
              .OrderBy(v => v, StringComparer.CurrentCultureIgnoreCase);

    /// <summary>"View Batches": clears every filter so the whole batch list is in the table.</summary>
    public void ShowAllBatches()
    {
        inventorySearch.Box.Text = "";
        categoryFilter.SelectedIndex = 0;
        stockStatusFilter.SelectedIndex = 0;
        supplierFilter.SelectedIndex = 0;
        _page = 1;
        Apply();
    }

    /// <summary>Shows only the lines with this status (e.g. "Low Stock"); the "View All" links use it.</summary>
    public void FilterByStatus(string label)
    {
        inventorySearch.Box.Text = "";
        categoryFilter.SelectedIndex = 0;
        supplierFilter.SelectedIndex = 0;
        int index = stockStatusFilter.Items.ToList().FindIndex(i => i == label);
        stockStatusFilter.SelectedIndex = Math.Max(0, index);
        _page = 1;
        Apply();
    }

    /// <summary>Shows just the lines for one product (used when an alert on the dashboard is clicked).</summary>
    public void ShowItem(string name)
    {
        categoryFilter.SelectedIndex = 0;
        supplierFilter.SelectedIndex = 0;
        stockStatusFilter.SelectedIndex = 0;
        _page = 1;
        inventorySearch.Box.Text = name;
        Apply();
    }

    /// <summary>True when there is at least one expired batch (the Alerts "View All" link then shows those first).</summary>
    public bool HasExpired => _all.Any(r => r.Expiry is DateTime e && e.Date < DateTime.Today);

    /// <summary>True when some product still has units but is at or below its reorder level.</summary>
    public bool HasLowStock => _all.Any(r => r.QtyOnHand > 0 && r.ReorderLevel > 0 && r.QtyOnHand <= r.ReorderLevel);

    private void Apply()
    {
        var today = DateTime.Today;
        string q = inventorySearch.Value.Trim();
        string? category = categoryFilter.SelectedIndex > 0 ? categoryFilter.SelectedItem : null;
        string? supplier = supplierFilter.SelectedIndex > 0 ? supplierFilter.SelectedItem : null;
        string? status = stockStatusFilter.SelectedIndex > 0 ? stockStatusFilter.SelectedItem : null;

        var shown = _all.Where(r =>
            (category is null || string.Equals(r.Category, category, StringComparison.OrdinalIgnoreCase)) &&
            (supplier is null || string.Equals(r.Supplier, supplier, StringComparison.OrdinalIgnoreCase)) &&
            (status is null || InventoryData.Label(InventoryData.StateOf(r, today)) == status) &&
            (q.Length == 0 ||
             r.Name.Contains(q, StringComparison.OrdinalIgnoreCase) ||
             r.Code.Contains(q, StringComparison.OrdinalIgnoreCase) ||
             r.Category.Contains(q, StringComparison.OrdinalIgnoreCase) ||
             r.BatchNo.Contains(q, StringComparison.OrdinalIgnoreCase) ||
             r.Supplier.Contains(q, StringComparison.OrdinalIgnoreCase))).ToList();

        int pages = Math.Max(1, (int)Math.Ceiling(shown.Count / (double)PageSize));
        _page = Math.Clamp(_page, 1, pages);
        var slice = shown.Skip((_page - 1) * PageSize).Take(PageSize).ToList();

        inventoryRows.SuspendLayout();
        ClearRows(inventoryRows, inventoryEmpty);
        inventoryEmpty.Visible = slice.Count == 0;
        inventoryColumnsHead.Visible = IsListView;
        if (IsListView)
        {
            for (int i = slice.Count - 1; i >= 0; i--)
                inventoryRows.Controls.Add(BuildRow(slice[i], today));
        }
        else if (slice.Count > 0)
        {
            inventoryRows.Controls.Add(BuildCards(slice, today));
        }
        inventoryRows.ResumeLayout(true);

        inventoryShowing.Text = shown.Count == 0
            ? "Showing 0 products"
            : $"Showing {(_page - 1) * PageSize + 1} to {(_page - 1) * PageSize + slice.Count} of {shown.Count} entries";
        inventoryPageChip.Text = _page.ToString(CultureInfo.CurrentCulture);

        ApplyTiles(today);
        ApplyLists(today);
    }

    // ------------------------------------------------------------------ table / cards

    private static Color StateColor(StockState s) => s switch
    {
        StockState.OutOfStock or StockState.Expired => Theme.Danger,
        StockState.LowStock or StockState.NearExpiry => Theme.Orange,
        _ => Good,
    };

    private static string Day(DateTime? d) => d is DateTime v ? v.ToString("MMM d, yyyy", CultureInfo.CurrentCulture) : "\u2014";

    private Control BuildRow(InventoryRow r, DateTime today)
    {
        var grid = NewRowGrid(inventoryColumnsGrid);

        void Put(Control header, string text, Color? color = null)
            => grid.Controls.Add(NewCell(text, color), inventoryColumnsGrid.GetColumn(header), 0);

        var state = InventoryData.StateOf(r, today);
        Color? expiryColor = r.Expiry is not null && state is StockState.Expired or StockState.NearExpiry ? StateColor(state) : null;

        Put(inventoryHeaderCheck, "");
        Put(inventoryColProduct, r.Name);
        Put(inventoryColCategory, r.Category);
        Put(inventoryColBatch, r.BatchNo.Length == 0 ? "\u2014" : r.BatchNo);
        Put(inventoryColSupplier, r.Supplier.Length == 0 ? "\u2014" : r.Supplier);
        Put(inventoryColReceived, Day(r.Received));
        Put(inventoryColExpiry, Day(r.Expiry), expiryColor);
        Put(inventoryColQuantity, r.BatchQty.ToString("N0", CultureInfo.CurrentCulture));
        Put(inventoryColStatus, InventoryData.Label(state), StateColor(state));

        int id = r.ProductId;
        grid.Controls.Add(NewActions(
            ("Restock", () => RestockRequested?.Invoke(id)),
            ("Edit", () => EditProductRequested?.Invoke(id))),
            inventoryColumnsGrid.GetColumn(inventoryColActions), 0);

        return WrapRow(grid);
    }

    private Control BuildCards(List<InventoryRow> slice, DateTime today)
    {
        var flow = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoScroll = true, BackColor = Color.White, Padding = new Padding(0, 12, 0, 0) };

        static Label Line(string text, int y, int h, Font font, Color color) => new()
        {
            Text = text, Location = new Point(12, y), Size = new Size(206, h), Font = font, ForeColor = color,
            BackColor = Color.Transparent, AutoEllipsis = true, UseMnemonic = false,
        };

        foreach (var r in slice)
        {
            var state = InventoryData.StateOf(r, today);
            var card = new Panel { Size = new Size(230, 118), Margin = new Padding(0, 0, 12, 12), BackColor = Theme.Subtle, Cursor = Cursors.Hand };
            card.Controls.Add(Line(r.Name, 10, 20, new Font("Segoe UI", 9.5F, FontStyle.Bold), Ink));
            card.Controls.Add(Line(r.Category, 32, 18, new Font("Segoe UI", 8.5F), Quiet));
            card.Controls.Add(Line(r.BatchNo.Length == 0 ? "No batch" : $"Batch {r.BatchNo}", 52, 18, new Font("Segoe UI", 8.5F), Quiet));
            card.Controls.Add(Line(r.Expiry is null ? "No expiry date" : $"Expires {Day(r.Expiry)}", 70, 18, new Font("Segoe UI", 8.5F), Quiet));
            card.Controls.Add(Line($"{r.BatchQty.ToString("N0", CultureInfo.CurrentCulture)} units  \u00B7  {InventoryData.Label(state)}", 92, 20,
                new Font("Segoe UI", 9F, FontStyle.Bold), StateColor(state)));

            int id = r.ProductId;
            OnClick(card, (_, _) => RestockRequested?.Invoke(id));
            flow.Controls.Add(card);
        }
        return flow;
    }

    // ------------------------------------------------------------------ tiles and side lists

    private void ApplyTiles(DateTime today)
    {
        var culture = CultureInfo.CurrentCulture;
        var products = _all.GroupBy(r => r.ProductId).Select(g => g.First()).ToList();

        invProductsValue.Text = products.Count.ToString(culture);
        invProductsDelta.Text = AddedText(_productsAdded);

        invQuantityValue.Text = products.Sum(p => p.QtyOnHand).ToString("N0", culture);
        invQuantityDelta.Text = _unitsReceived > 0 ? $"+{_unitsReceived.ToString("N0", culture)} received this month" : "None received this month";

        int low = products.Count(p => p.ReorderLevel > 0 && p.QtyOnHand <= p.ReorderLevel);
        int none = products.Count(p => p.QtyOnHand <= 0);
        invLowStockValue.Text = low.ToString(culture);
        invLowStockDelta.Text = none == 0 ? "None out of stock" : $"{none.ToString(culture)} out of stock";

        int expired = _all.Count(r => r.Expiry is DateTime e && e.Date < today);
        int near = _all.Count(r => r.Expiry is DateTime e && e.Date >= today && e.Date <= today.AddDays(InventoryData.NearExpiryDays));
        invExpiryValue.Text = (expired + near).ToString(culture);
        invExpiryDelta.Text = expired == 0 ? "None expired" : $"{expired.ToString(culture)} expired";
    }

    private void ApplyLists(DateTime today)
    {
        // Alerts: batches that are expired or expire soon (soonest first).
        var alerts = new List<DashboardRowView>();
        foreach (var r in _all.Where(r => r.Expiry is DateTime).OrderBy(r => r.Expiry))
        {
            var expiry = r.Expiry!.Value.Date;
            int days = (expiry - today).Days;
            if (expiry < today)
                alerts.Add(Clickable(new DashboardRowView(r.Name, $"Batch {r.BatchNo} expired {Day(expiry)}", $"{r.BatchQty} left", Theme.Danger), r.Name));
            else if (days <= InventoryData.NearExpiryDays)
                alerts.Add(Clickable(new DashboardRowView(r.Name,
                    days == 0 ? $"Batch {r.BatchNo} expires today" : $"Batch {r.BatchNo} expires in {days} day{(days == 1 ? "" : "s")}",
                    $"{r.BatchQty} left", Theme.Orange), r.Name));
        }
        FillList(alertsList, alertsEmpty, alerts);

        // Low stock: products at or below their reorder level, empty shelves first.
        var low = _all.GroupBy(r => r.ProductId).Select(g => g.First())
            .Where(p => p.ReorderLevel > 0 && p.QtyOnHand <= p.ReorderLevel)
            .OrderBy(p => p.QtyOnHand).ThenBy(p => p.Name)
            .Select(p => Clickable(new DashboardRowView(p.Name,
                p.QtyOnHand <= 0 ? $"Out of stock (reorder level {p.ReorderLevel})" : $"{p.QtyOnHand} left (reorder at {p.ReorderLevel})",
                p.Code, p.QtyOnHand <= 0 ? Theme.Danger : Theme.Orange), p.Name))
            .ToList();
        FillList(lowStockList, lowStockEmpty, low);
    }

    /// <summary>Makes a side-list row clickable: it narrows the table to that product.</summary>
    private DashboardRowView Clickable(DashboardRowView row, string productName)
    {
        row.Clickable = true;
        row.Click += (_, _) => ShowItem(productName);
        return row;
    }

    /// <summary>Rebuilds a side list; with no rows it hides the list and shows the card's empty-state message instead.</summary>
    private static void FillList(Panel list, EmptyState empty, IReadOnlyList<DashboardRowView> rows)
    {
        list.SuspendLayout();
        foreach (var old in list.Controls.OfType<DashboardRowView>().ToList())
        {
            list.Controls.Remove(old);
            old.Dispose();
        }

        // Rows are docked to the top, so add them last-to-first to keep the first row on top.
        for (int i = rows.Count - 1; i >= 0; i--)
        {
            rows[i].Dock = DockStyle.Top;
            list.Controls.Add(rows[i]);
        }
        list.AutoScrollPosition = Point.Empty;
        list.ResumeLayout(true);

        list.Visible = rows.Count > 0;
        empty.Visible = rows.Count == 0;
    }
}
