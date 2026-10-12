using System.Globalization;
using MyApp.Controls;

namespace MyApp;

/// <summary>
/// Customer &amp; Pet Lookup screen. Layout lives in LookupPage.Designer.cs. It shows only what you give it through
/// <see cref="SetData"/>; with nothing loaded every list shows its empty state and every count reads 0.
/// </summary>
internal partial class LookupPage : UserControl
{
    private const int PageSize = 8;
    private const int NewCustomerDays = 30;

    private IReadOnlyList<CustomerRecord> _customers = Array.Empty<CustomerRecord>();
    private IReadOnlyList<PetRecord> _pets = Array.Empty<PetRecord>();
    private bool _showPets;
    private bool _onlyNew;
    private int _pageIndex;
    private int _filteredCount;
    private bool _suspendFilter;

    public event EventHandler? SearchRequested;
    public event EventHandler? ExportCustomersRequested;
    public event EventHandler? ExportPetsRequested;

    /// <summary>"Book" was pressed on a row. The id is the customer to book for.</summary>
    public event EventHandler<int>? BookRequested;

    /// <summary>The "..." menu (or a right-click) was used on a row. The argument is the CustomerRecord or PetRecord.</summary>
    public event EventHandler<object>? MoreRequested;

    /// <summary>"+ Add Customer" was pressed.</summary>
    public event EventHandler? AddCustomerRequested;

    public LookupPage()
    {
        InitializeComponent();
        DoubleBuffered = true;

        // Membership tiers (Bronze / Silver / Gold) aren't stored anywhere, so they are not shown.
        // The row columns are a bit wider on the right now to make room for the "..." button; keep the header in step.
        float[] widths = { 20f, 16f, 15f, 20f, 11f, 18f };
        for (int i = 0; i < widths.Length; i++)
            columnsGrid.ColumnStyles[i + 1].Width = widths[i];

        // "+ Add Customer" sits at the end of the search row.
        filterGrid.ColumnCount = 6;
        filterGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 136F));
        var addCustomerButton = new PrimaryButton("+ Add Customer")
        {
            BackColor = Color.White,
            Dock = DockStyle.Fill,
            Font = Theme.Face(9f, FontStyle.Bold),
            Margin = new Padding(8, 0, 0, 0),
        };
        addCustomerButton.Click += (_, _) => AddCustomerRequested?.Invoke(this, EventArgs.Empty);
        filterGrid.Controls.Add(addCustomerButton, 5, 0);

        filterBronzeRow.Visible = false;
        filterSilverRow.Visible = false;
        filterGoldRow.Visible = false;

        customerFilter.SetItems(new[] { "All Customers", "With pets", "Without pets" });
        petFilter.SetItems(new[] { "All Pets" });
        sortFilter.SetItems(new[] { "Sort by: Newest", "Sort by: Oldest", "Sort by: Name (A-Z)" });

        customerFilter.SelectedIndexChanged += (_, _) => ApplyFilters(resetPage: true);
        petFilter.SelectedIndexChanged += (_, _) => ApplyFilters(resetPage: true);
        sortFilter.SelectedIndexChanged += (_, _) => ApplyFilters(resetPage: true);
        searchField.Box.TextChanged += (_, _) => ApplyFilters(resetPage: true);

        HookClick(filterNewRow, (_, _) => { _onlyNew = true; ApplyFilters(resetPage: true); });
        HookClick(filterAllRow, (_, _) => { _onlyNew = false; ApplyFilters(resetPage: true); });

        prevPageButton.Click += (_, _) =>
        {
            if (_pageIndex <= 0) return;
            _pageIndex--;
            ApplyFilters(resetPage: false);
        };
        nextPageButton.Click += (_, _) =>
        {
            if (_pageIndex >= PageCount - 1) return;
            _pageIndex++;
            ApplyFilters(resetPage: false);
        };

        ApplyFilters(resetPage: true);
    }

    // ------------------------------------------------------------------ public API

    /// <summary>Replaces the customers and pets shown on the page.</summary>
    public void SetData(IEnumerable<CustomerRecord> customers, IEnumerable<PetRecord> pets)
    {
        _customers = customers.ToList();
        _pets = pets.ToList();

        _suspendFilter = true;
        petFilter.SetItems(new[] { "All Pets" }.Concat(
            _pets.Select(p => p.Species)
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .OrderBy(s => s, StringComparer.CurrentCultureIgnoreCase)));
        _suspendFilter = false;

        filterAllCount.Text = _customers.Count.ToString(CultureInfo.CurrentCulture);
        filterNewCount.Text = _customers.Count(IsNew).ToString(CultureInfo.CurrentCulture);
        ApplyFilters(resetPage: false);
    }

    public IReadOnlyList<CustomerRecord> Customers => _customers;
    public IReadOnlyList<PetRecord> Pets => _pets;

    // ------------------------------------------------------------------ filtering

    private static bool IsNew(CustomerRecord c) => c.CreatedAt >= DateTime.Now.AddDays(-NewCustomerDays);

    private static bool Has(string? value, string query)
        => value is not null && value.Contains(query, StringComparison.CurrentCultureIgnoreCase);

    private int PageCount => Math.Max(1, (int)Math.Ceiling(_filteredCount / (double)PageSize));

    private void ApplyFilters(bool resetPage)
    {
        if (_suspendFilter) return;
        if (resetPage) _pageIndex = 0;

        string query = searchField.Value.Trim();
        var rows = new List<LookupRowView>();
        int total;

        if (_showPets)
        {
            string? species = petFilter.SelectedIndex > 0 ? petFilter.SelectedItem : null;
            var list = _pets
                .Where(p => species is null || string.Equals(p.Species, species, StringComparison.OrdinalIgnoreCase))
                .Where(p => query.Length == 0 || Has(p.Name, query) || Has(p.Breed, query) || Has(p.Species, query)
                            || Has(p.OwnerName, query) || Has(p.OwnerPhone, query))
                .ToList();

            list = sortFilter.SelectedIndex switch
            {
                1 => list.OrderBy(p => p.Id).ToList(),
                2 => list.OrderBy(p => p.Name, StringComparer.CurrentCultureIgnoreCase).ToList(),
                _ => list.OrderByDescending(p => p.Id).ToList(),
            };

            total = list.Count;
            _filteredCount = total;
            ClampPage();
            foreach (var p in list.Skip(_pageIndex * PageSize).Take(PageSize))
            {
                var pet = p;
                var row = new LookupRowView(pet,
                    new[] { pet.Name, pet.OwnerName, pet.Breed, pet.OwnerPhone, pet.Species }, "Book");
                row.ActionClicked += (_, _) => BookRequested?.Invoke(this, pet.CustomerId);
                row.MoreClicked += (_, _) => MoreRequested?.Invoke(this, pet);
                rows.Add(row);
            }
        }
        else
        {
            var list = _customers
                .Where(c => !_onlyNew || IsNew(c))
                .Where(c => customerFilter.SelectedIndex switch { 1 => c.PetCount > 0, 2 => c.PetCount == 0, _ => true })
                .Where(c => query.Length == 0 || Has(c.Name, query) || Has(c.Phone, query) || Has(c.Email, query) || Has(c.PetNames, query))
                .ToList();

            list = sortFilter.SelectedIndex switch
            {
                1 => list.OrderBy(c => c.Id).ToList(),
                2 => list.OrderBy(c => c.Name, StringComparer.CurrentCultureIgnoreCase).ToList(),
                _ => list.OrderByDescending(c => c.Id).ToList(),
            };

            total = list.Count;
            _filteredCount = total;
            ClampPage();
            foreach (var c in list.Skip(_pageIndex * PageSize).Take(PageSize))
            {
                var customer = c;
                string lastVisit = customer.LastVisit?.ToString("MMM d, yyyy", CultureInfo.CurrentCulture) ?? "No visits yet";
                var row = new LookupRowView(customer,
                    new[] { customer.Name, customer.PetNames.Length == 0 ? "No pets" : customer.PetNames, customer.Phone, customer.Email, lastVisit },
                    "Book");
                row.ActionClicked += (_, _) => BookRequested?.Invoke(this, customer.Id);
                row.MoreClicked += (_, _) => MoreRequested?.Invoke(this, customer);
                rows.Add(row);
            }
        }

        ShowRows(rows, total);
    }

    private void ClampPage()
    {
        if (_pageIndex >= PageCount) _pageIndex = PageCount - 1;
        if (_pageIndex < 0) _pageIndex = 0;
    }

    private void ShowRows(List<LookupRowView> rows, int total)
    {
        rowsHost.SuspendLayout();
        foreach (var old in rowsHost.Controls.OfType<LookupRowView>().ToList())
        {
            rowsHost.Controls.Remove(old);
            old.Dispose();
        }

        // Rows are docked to the top, so add them last-to-first to keep the first one on top.
        for (int i = rows.Count - 1; i >= 0; i--)
        {
            rows[i].Dock = DockStyle.Top;
            rowsHost.Controls.Add(rows[i]);
        }

        string what = _showPets ? "pets" : "customers";
        bool anySource = (_showPets ? _pets.Count : _customers.Count) > 0;
        customersEmpty.Glyph = _showPets ? "\uE80F" : "\uE716";
        customersEmpty.Title = anySource ? $"No matching {what}" : $"No {what} yet";
        customersEmpty.Hint = anySource
            ? "Try a different search or filter."
            : _showPets ? "Add a pet from a customer's \"...\" menu." : "Use + Add Customer to create the first one.";
        customersEmpty.Visible = rows.Count == 0;

        customersTitle.Text = _showPets
            ? $"Pets ({total.ToString(CultureInfo.CurrentCulture)})"
            : $"Customers ({total.ToString(CultureInfo.CurrentCulture)})";

        if (total == 0)
        {
            showingLabel.Text = $"Showing 0 {what}";
        }
        else
        {
            int from = _pageIndex * PageSize + 1;
            int to = Math.Min(total, from + PageSize - 1);
            showingLabel.Text = $"Showing {from}\u2013{to} of {total} {what}";
        }

        pageChip.Text = (_pageIndex + 1).ToString(CultureInfo.CurrentCulture);
        prevPageButton.Enabled = _pageIndex > 0;
        nextPageButton.Enabled = _pageIndex < PageCount - 1;

        rowsHost.AutoScrollPosition = Point.Empty;
        rowsHost.ResumeLayout(true);
    }

    private void ApplyColumnHeaders()
    {
        if (_showPets)
        {
            colCustomer.Text = "PET";
            colPets.Text = "OWNER";
            colPhone.Text = "BREED";
            colEmail.Text = "OWNER PHONE";
            colLastVisit.Text = "SPECIES";
        }
        else
        {
            colCustomer.Text = "CUSTOMER";
            colPets.Text = "PETS";
            colPhone.Text = "PHONE";
            colEmail.Text = "EMAIL";
            colLastVisit.Text = "LAST VISIT";
        }
        customersIcon.Text = _showPets ? "\uE80F" : "\uE716";
    }

    // Child controls swallow clicks, so hook the row and everything inside it.
    private static void HookClick(Control root, EventHandler handler)
    {
        root.Click += handler;
        root.Cursor = Cursors.Hand;
        foreach (Control child in root.Controls)
            HookClick(child, handler);
    }

    // ------------------------------------------------------------------ events from the designer

    private void customerTab_Click(object? sender, EventArgs e) => SelectTab(customerTab);

    private void petTab_Click(object? sender, EventArgs e) => SelectTab(petTab);

    private void SelectTab(TabPill selected)
    {
        customerTab.Active = ReferenceEquals(selected, customerTab);
        petTab.Active = ReferenceEquals(selected, petTab);

        _showPets = ReferenceEquals(selected, petTab);
        ApplyColumnHeaders();
        ApplyFilters(resetPage: true);
    }

    private void searchButton_Click(object? sender, EventArgs e)
    {
        ApplyFilters(resetPage: true);
        SearchRequested?.Invoke(this, EventArgs.Empty);
    }

    private void exportCustomersTile_Click(object? sender, EventArgs e) => ExportCustomersRequested?.Invoke(this, EventArgs.Empty);

    private void exportPetsTile_Click(object? sender, EventArgs e) => ExportPetsRequested?.Invoke(this, EventArgs.Empty);
}
