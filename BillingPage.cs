using System.ComponentModel;
using System.Globalization;
using MyApp.Controls;

namespace MyApp;

/// <summary>
/// Counter POS &amp; Billing screen. Open this file's designer in Visual Studio to move or restyle anything.
/// It shows only what you give it through <see cref="SetProducts"/>, <see cref="SetServices"/>,
/// <see cref="SetAppointments"/> and <see cref="SetTransactions"/>; with nothing loaded it shows empty states.
/// </summary>
internal partial class BillingPage : UserControl
{
    private const int RecentLimit = 20;

    private IReadOnlyList<ProductItem> _products = Array.Empty<ProductItem>();
    private IReadOnlyList<ServiceOffering> _services = Array.Empty<ServiceOffering>();
    private IReadOnlyList<AppointmentItem> _appointments = Array.Empty<AppointmentItem>();
    private IReadOnlyList<TransactionItem> _transactions = Array.Empty<TransactionItem>();

    private readonly List<CartLine> _cart = new();
    private readonly OutlineButton[] _quick;
    private AppointmentItem? _selected;
    private string? _category;            // null = "All"
    private bool _percentDiscount = true;
    private bool _updating;

    public event EventHandler? OngoingAppointmentsRequested;
    public event EventHandler? TransactionHistoryRequested;
    public event EventHandler<TransactionItem>? ViewTransactionRequested;
    public event EventHandler<AppointmentItem>? ViewCustomerRequested;

    /// <summary>Raised when "Print Receipt" is pressed. Save the sale, print the receipt, then call <see cref="ClearTransaction"/>.</summary>
    public event EventHandler<BillingCheckout>? PrintReceiptRequested;

    public BillingPage()
    {
        InitializeComponent();
        DoubleBuffered = true;

        _quick = new[] { quick1Button, quick2Button, quick3Button, quick4Button };

        new ToolTip(components).SetToolTip(resetFiltersButton, "Clear the search and category filter.");

        customerSearch.Box.TextChanged += (_, _) => UpdateCustomerPanel();
        catalogSearch.Box.TextChanged += (_, _) => RebuildCatalog();
        customerCard.ClearClicked += (_, _) => ClearAppointment();
        customerCard.ViewProfileClicked += (_, _) =>
        {
            if (_selected is not null) ViewCustomerRequested?.Invoke(this, _selected);
        };

        receiptPreview.Cashier = "";
        dateTimer.Enabled = LicenseManager.UsageMode != LicenseUsageMode.Designtime;

        UpdateClock();
        ApplyCustomerMode();
        RebuildChips();
        RebuildCatalog();
        RebuildCart();
        RebuildTransactions();
    }

    // ------------------------------------------------------------------ public API

    /// <summary>Replaces the retail products that can be added to the cart.</summary>
    public void SetProducts(IEnumerable<ProductItem> products)
    {
        _products = products.ToList();
        if (_category is not null &&
            !_products.Any(p => string.Equals(p.Category, _category, StringComparison.OrdinalIgnoreCase)))
            _category = null;
        RebuildChips();
        RebuildCatalog();
    }

    /// <summary>Replaces the services that can be added to the cart. A picked appointment's service is matched by name.</summary>
    public void SetServices(IEnumerable<ServiceOffering> services)
    {
        _services = services.ToList();
        RebuildCatalog();
    }

    /// <summary>Replaces the appointments that can be picked in "From Appointment" mode.</summary>
    public void SetAppointments(IEnumerable<AppointmentItem> appointments)
    {
        _appointments = appointments.ToList();
        UpdateCustomerPanel();
    }

    /// <summary>Replaces the rows of "Recent Transactions".</summary>
    public void SetTransactions(IEnumerable<TransactionItem> transactions)
    {
        _transactions = transactions.ToList();
        RebuildTransactions();
    }

    /// <summary>The receipt number shown in the receipt preview (the number the next sale will get).</summary>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string ReceiptNumber
    {
        get => receiptPreview.ReceiptNumber;
        set => receiptPreview.ReceiptNumber = value ?? "";
    }

    /// <summary>The signed-in staff member shown on the receipt.</summary>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string CashierName
    {
        get => receiptPreview.Cashier;
        set => receiptPreview.Cashier = value ?? "";
    }

    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string StoreAddress
    {
        get => receiptPreview.Address;
        set => receiptPreview.Address = value ?? "";
    }

    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string StorePhone
    {
        get => receiptPreview.Phone;
        set => receiptPreview.Phone = value ?? "";
    }

    /// <summary>Empties the cart, customer, notes, discount and payment so a new sale can start.</summary>
    public void ClearTransaction()
    {
        _cart.Clear();
        _selected = null;
        customerSearch.Box.Text = "";
        notesBox.Text = "";
        discountBox.Text = "0";
        _percentDiscount = true;
        discountMode.Text = "%";
        receivedBox.Text = "";
        paymentMethod.SelectedIndex = 0;
        UpdateCustomerPanel();
        RebuildCart();
    }

    // ------------------------------------------------------------------ 1. customer

    private bool IsWalkIn => customerMode.SelectedIndex == 0;

    private static bool Has(string? value, string query)
        => value is not null && value.Contains(query, StringComparison.CurrentCultureIgnoreCase);

    private void customerMode_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (IsWalkIn)
        {
            _selected = null;
            _cart.RemoveAll(l => l.Kind == CartLineKind.Service);
            customerSearch.Box.Text = "";
            if (catalogTabs.SelectedIndex != 0) catalogTabs.SelectedIndex = 0;
            RebuildCart();
        }
        ApplyCustomerMode();
    }

    private void ApplyCustomerMode()
    {
        customerSearch.Box.Enabled = !IsWalkIn;
        UpdateCustomerPanel();
    }

    private void UpdateCustomerPanel()
    {
        string query = customerSearch.Value.Trim();
        bool searching = !IsWalkIn && query.Length > 0;

        customerCard.Visible = false;
        customerResults.Visible = false;
        customerEmpty.Visible = false;

        if (IsWalkIn)
        {
            customerEmpty.Title = "Walk-in customer";
            customerEmpty.Hint = "Retail items only. No appointment is needed.";
            customerEmpty.Visible = true;
            return;
        }

        if (!searching && _selected is not null)
        {
            customerCard.Item = _selected;
            customerCard.Visible = true;
            return;
        }

        var matches = _appointments
            .Where(a => a.Status != AppointmentStatus.Cancelled)
            .Where(a => searching
                ? Has(a.CustomerName, query) || Has(a.CustomerPhone, query) || Has(a.PetName, query) || Has(a.Service, query)
                : a.Start.Date == DateTime.Today)
            .OrderBy(a => a.Start)
            .Take(30)
            .ToList();

        BuildCustomerResults(matches);

        if (matches.Count > 0)
        {
            customerResults.Visible = true;
            return;
        }

        customerEmpty.Title = searching ? "No matching appointments"
            : _appointments.Count == 0 ? "No customer selected" : "No appointments today";
        customerEmpty.Hint = searching
            ? "Try a different name, pet or service."
            : "Search above to attach an appointment to this sale.";
        customerEmpty.Visible = true;
    }

    private void BuildCustomerResults(List<AppointmentItem> matches)
    {
        customerResults.SuspendLayout();
        foreach (var old in customerResults.Controls.OfType<CatalogRowView>().ToList())
        {
            customerResults.Controls.Remove(old);
            old.Dispose();
        }

        for (int i = matches.Count - 1; i >= 0; i--)
        {
            var a = matches[i];
            string sub = string.Join("  \u00B7  ", new[]
            {
                a.PetName, a.Service, a.Start.ToString("h:mm tt", CultureInfo.CurrentCulture),
            }.Where(s => !string.IsNullOrWhiteSpace(s)));

            var row = new CatalogRowView(a, "\uE77B", a.CustomerName, sub, "", "Select") { Dock = DockStyle.Top };
            row.ActionClicked += (_, _) => SelectAppointment(a);
            customerResults.Controls.Add(row);
        }

        customerResults.AutoScrollPosition = Point.Empty;
        customerResults.ResumeLayout(true);
    }

    private void SelectAppointment(AppointmentItem appointment)
    {
        _cart.RemoveAll(l => l.FromAppointment);
        _selected = appointment;
        customerSearch.Box.Text = "";

        var service = _services.FirstOrDefault(s =>
            string.Equals(s.Name, appointment.Service, StringComparison.OrdinalIgnoreCase));
        if (service is not null)
            _cart.Add(new CartLine(CartLineKind.Service, service.Id, service.Name, appointment.PetName, service.Price, 1, true));

        UpdateCustomerPanel();
        RebuildCart();
    }

    private void ClearAppointment()
    {
        _selected = null;
        _cart.RemoveAll(l => l.FromAppointment);
        UpdateCustomerPanel();
        RebuildCart();
    }

    // ------------------------------------------------------------------ 2. items / services

    private bool ServicesTab => catalogTabs.SelectedIndex == 1;

    private void catalogTabs_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (IsWalkIn && ServicesTab)
        {
            catalogTabs.SelectedIndex = 0;   // walk-ins are retail only; this re-enters and rebuilds
            return;
        }
        catalogSearch.Placeholder = ServicesTab ? "Search services..." : "Search by product name or code...";
        RebuildCatalog();
    }

    private void categoryChip_Click(object? sender, EventArgs e)
    {
        _category = ReferenceEquals(sender, allChip) ? null : (sender as TabPill)?.Text;
        SyncChips();
        RebuildCatalog();
    }

    private void resetFiltersButton_Click(object? sender, EventArgs e)
    {
        _category = null;
        SyncChips();
        catalogSearch.Box.Text = "";
        RebuildCatalog();
    }

    private void RebuildChips()
    {
        chipsFlow.SuspendLayout();
        foreach (var old in chipsFlow.Controls.OfType<TabPill>().Where(c => !ReferenceEquals(c, allChip)).ToList())
        {
            chipsFlow.Controls.Remove(old);
            old.Dispose();
        }

        var categories = _products
            .Select(p => p.Category)
            .Where(c => !string.IsNullOrWhiteSpace(c))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(c => c, StringComparer.CurrentCultureIgnoreCase)
            .ToList();

        foreach (var category in categories)
        {
            var chip = new TabPill(category) { Margin = new Padding(0, 0, 6, 6) };
            chip.Click += categoryChip_Click;
            chipsFlow.Controls.Add(chip);
        }

        chipsFlow.Visible = !ServicesTab && categories.Count > 0;
        SyncChips();
        chipsFlow.ResumeLayout(true);
    }

    private void SyncChips()
    {
        foreach (var chip in chipsFlow.Controls.OfType<TabPill>())
        {
            chip.Active = ReferenceEquals(chip, allChip)
                ? _category is null
                : string.Equals(chip.Text, _category, StringComparison.OrdinalIgnoreCase);
        }
    }

    private void RebuildCatalog()
    {
        string query = catalogSearch.Value.Trim();
        var rows = new List<CatalogRowView>();
        bool anySource;

        if (ServicesTab)
        {
            anySource = _services.Count > 0;
            foreach (var s in _services.Where(s => query.Length == 0 || Has(s.Name, query)).OrderBy(s => s.Name))
            {
                var service = s;
                var row = new CatalogRowView(service, "\uE734", service.Name, "", Money.Format(service.Price), "Add");
                row.ActionClicked += (_, _) => AddService(service);
                rows.Add(row);
            }
        }
        else
        {
            anySource = _products.Count > 0;
            var list = _products
                .Where(p => _category is null || string.Equals(p.Category, _category, StringComparison.OrdinalIgnoreCase))
                .Where(p => query.Length == 0 || Has(p.Name, query) || Has(p.Code, query) || Has(p.Category, query))
                .OrderBy(p => p.Name, StringComparer.CurrentCultureIgnoreCase);

            foreach (var p in list)
            {
                var product = p;
                var row = new CatalogRowView(product, "\uE719", product.Name, product.Code, Money.Format(product.Price), "Add");
                row.ActionClicked += (_, _) => AddProduct(product);
                rows.Add(row);
            }
        }

        catalogList.SuspendLayout();
        foreach (var old in catalogList.Controls.OfType<CatalogRowView>().ToList())
        {
            catalogList.Controls.Remove(old);
            old.Dispose();
        }

        // Rows are docked to the top, so add them last-to-first to keep the first one on top.
        for (int i = rows.Count - 1; i >= 0; i--)
        {
            rows[i].Dock = DockStyle.Top;
            catalogList.Controls.Add(rows[i]);
        }

        string what = ServicesTab ? "services" : "products";
        catalogEmpty.Glyph = ServicesTab ? "\uE734" : "\uE719";
        catalogEmpty.Title = anySource ? $"No matching {what}" : $"No {what} yet";
        catalogEmpty.Hint = anySource
            ? "Try a different search or category."
            : $"{(ServicesTab ? "Services" : "Products")} will show up here once they are added.";
        catalogEmpty.Visible = rows.Count == 0;

        chipsFlow.Visible = !ServicesTab && _products.Any(p => !string.IsNullOrWhiteSpace(p.Category));

        catalogList.AutoScrollPosition = Point.Empty;
        catalogList.ResumeLayout(true);
    }

    private void AddProduct(ProductItem product)
    {
        var existing = _cart.FirstOrDefault(l => l.Kind == CartLineKind.Product && l.SourceId == product.Id);
        if (existing is not null)
        {
            existing.Quantity++;
            cartRowsHost.Controls.OfType<CartRowView>().FirstOrDefault(r => ReferenceEquals(r.Line, existing))?.Invalidate();
            UpdateTotals();
            return;
        }

        _cart.Add(new CartLine(CartLineKind.Product, product.Id, product.Name, "", product.Price));
        RebuildCart();
    }

    private void AddService(ServiceOffering service)
    {
        if (_cart.Any(l => l.Kind == CartLineKind.Service && l.SourceId == service.Id)) return;

        _cart.Add(new CartLine(CartLineKind.Service, service.Id, service.Name, _selected?.PetName ?? "", service.Price));
        RebuildCart();
    }

    // ------------------------------------------------------------------ 3. cart

    private void RebuildCart()
    {
        cartRowsHost.SuspendLayout();
        foreach (var old in cartRowsHost.Controls.OfType<CartRowView>().ToList())
        {
            cartRowsHost.Controls.Remove(old);
            old.Dispose();
        }

        for (int i = _cart.Count - 1; i >= 0; i--)
        {
            var line = _cart[i];
            var row = new CartRowView(line) { Dock = DockStyle.Top };
            row.QuantityStepped += (_, delta) =>
            {
                line.Quantity = Math.Max(1, line.Quantity + delta);
                row.Invalidate();
                UpdateTotals();
            };
            row.RemoveClicked += (_, _) =>
            {
                _cart.Remove(line);
                BeginInvoke(RebuildCart);   // let the click finish before the row is disposed
            };
            cartRowsHost.Controls.Add(row);
        }

        cartEmpty.Visible = _cart.Count == 0;
        cartRowsHost.AutoScrollPosition = Point.Empty;
        cartRowsHost.ResumeLayout(true);
        UpdateTotals();
    }

    private void clearCartButton_Click(object? sender, EventArgs e)
    {
        _cart.Clear();
        RebuildCart();
    }

    private void newTransactionButton_Click(object? sender, EventArgs e) => ClearTransaction();

    private void discountMode_Click(object? sender, EventArgs e)
    {
        _percentDiscount = !_percentDiscount;
        discountMode.Text = _percentDiscount ? "%" : Money.Symbol;
        UpdateTotals();
    }

    private void discountBox_TextChanged(object? sender, EventArgs e) => UpdateTotals();

    private decimal Subtotal => _cart.Sum(l => l.Subtotal);

    private decimal DiscountAmount()
    {
        decimal subtotal = Subtotal;
        if (subtotal <= 0) return 0;

        decimal.TryParse(discountBox.Text.Trim(), NumberStyles.Number, CultureInfo.CurrentCulture, out var value);
        if (value < 0) value = 0;

        decimal discount = _percentDiscount ? subtotal * Math.Min(value, 100m) / 100m : Math.Min(value, subtotal);
        return Math.Round(discount, 2, MidpointRounding.AwayFromZero);
    }

    private decimal Total => Math.Max(0, Subtotal - DiscountAmount());

    private void UpdateTotals()
    {
        decimal total = Total;
        subtotalValue.Text = Money.Format(Subtotal);
        discountValue.Text = Money.Format(DiscountAmount());
        totalValue.Text = Money.Format(total);
        totalDueValue.Text = Money.Format(total);
        receiptPreview.ReceiptDate = _cart.Count > 0 ? DateTime.Now : null;
        UpdatePayment();
    }

    // ------------------------------------------------------------------ 4. payment

    private bool IsEWallet => paymentMethod.SelectedIndex == 1;

    private static string PlainAmount(decimal value)
        => value.ToString(value == decimal.Truncate(value) ? "N0" : "N2", CultureInfo.CurrentCulture);

    private decimal ParseReceived()
    {
        string text = receivedBox.Text.Replace(Money.Symbol, "").Trim();
        return decimal.TryParse(text, NumberStyles.Number, CultureInfo.CurrentCulture, out var v) ? Math.Max(0, v) : 0;
    }

    /// <summary>The exact total, then the next 500, 1,000 and 5,000 bill amounts above it.</summary>
    private static List<decimal> QuickAmounts(decimal total)
    {
        var list = new List<decimal>();
        if (total <= 0) return list;

        list.Add(total);
        decimal last = total;
        foreach (decimal step in new[] { 500m, 1000m, 5000m })
        {
            last = (decimal.Floor(last / step) + 1) * step;
            list.Add(last);
        }
        return list;
    }

    private void paymentMethod_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (!IsEWallet)
        {
            _updating = true;
            receivedBox.Text = "";
            _updating = false;
        }
        UpdatePayment();
    }

    private void receivedBox_TextChanged(object? sender, EventArgs e)
    {
        if (!_updating) UpdatePayment();
    }

    private void quickAmount_Click(object? sender, EventArgs e)
    {
        if (sender is OutlineButton { Tag: decimal amount })
            receivedBox.Text = PlainAmount(amount);
    }

    private void UpdatePayment()
    {
        decimal total = Total;
        bool ewallet = IsEWallet;

        _updating = true;
        receivedBox.ReadOnly = ewallet;
        if (ewallet) receivedBox.Text = total > 0 ? PlainAmount(total) : "";
        _updating = false;

        decimal received = ewallet ? total : ParseReceived();

        var amounts = ewallet ? new List<decimal>() : QuickAmounts(total);
        for (int i = 0; i < _quick.Length; i++)
        {
            bool show = i < amounts.Count;
            _quick[i].Visible = show;
            if (!show) continue;
            _quick[i].Tag = amounts[i];
            _quick[i].Text = Money.Format(amounts[i]);
            _quick[i].Selected = received == amounts[i] && receivedBox.Text.Trim().Length > 0;
        }

        bool shortBy = !ewallet && total > 0 && received < total && receivedBox.Text.Trim().Length > 0;
        changeLabel.Text = shortBy ? "Balance Due" : "Change";
        changeValue.Text = Money.Format(shortBy ? total - received : Math.Max(0, received - total));
        changeValue.ForeColor = shortBy ? Theme.Danger : Theme.Green;

        printButton.Enabled = _cart.Count > 0 && total > 0 && (ewallet || received >= total);
    }

    private void printButton_Click(object? sender, EventArgs e)
    {
        if (!printButton.Enabled) return;

        decimal total = Total;
        bool ewallet = IsEWallet;
        decimal received = ewallet ? total : ParseReceived();

        PrintReceiptRequested?.Invoke(this, new BillingCheckout(
            _cart.ToList(),
            Subtotal,
            DiscountAmount(),
            total,
            ewallet ? PaymentKind.EWallet : PaymentKind.Cash,
            received,
            Math.Max(0, received - total),
            notesBox.Text.Trim(),
            _selected));
    }

    // ------------------------------------------------------------------ recent transactions

    private void RebuildTransactions()
    {
        var list = _transactions.OrderByDescending(t => t.When).Take(RecentLimit).ToList();

        recentRowsHost.SuspendLayout();
        foreach (var old in recentRowsHost.Controls.OfType<TransactionRowView>().ToList())
        {
            recentRowsHost.Controls.Remove(old);
            old.Dispose();
        }

        for (int i = list.Count - 1; i >= 0; i--)
        {
            var item = list[i];
            var row = new TransactionRowView(item) { Dock = DockStyle.Top };
            row.ViewClicked += (_, _) => ViewTransactionRequested?.Invoke(this, item);
            recentRowsHost.Controls.Add(row);
        }

        recentEmpty.Visible = list.Count == 0;
        recentRowsHost.AutoScrollPosition = Point.Empty;
        recentRowsHost.ResumeLayout(true);
    }

    private void recentViewAll_Click(object? sender, EventArgs e)
        => TransactionHistoryRequested?.Invoke(this, EventArgs.Empty);

    private void historyButton_Click(object? sender, EventArgs e)
        => TransactionHistoryRequested?.Invoke(this, EventArgs.Empty);

    private void ongoingButton_Click(object? sender, EventArgs e)
        => OngoingAppointmentsRequested?.Invoke(this, EventArgs.Empty);

    // ------------------------------------------------------------------ clock

    private void UpdateClock()
    {
        var now = DateTime.Now;
        dateLabel.Text = now.ToString("MMM d, yyyy", CultureInfo.CurrentCulture);
        timeLabel.Text = now.ToString("h:mm tt", CultureInfo.CurrentCulture);
    }

    private void dateTimer_Tick(object? sender, EventArgs e)
    {
        UpdateClock();
        if (_cart.Count > 0) receiptPreview.ReceiptDate = DateTime.Now;
    }

    private void cartColumnsGrid_Paint(object sender, PaintEventArgs e)
    {

    }

    private void totalDueLabel_Click(object sender, EventArgs e)
    {

    }
}
