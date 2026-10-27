using System.Data;
using System.Globalization;
using Microsoft.Data.SqlClient;
using MyApp.Controls;
using static MyApp.EmployeeManagementPage;

namespace MyApp;

/// <summary>
/// Service Catalog Management screen. Layout lives in ServiceCatalogPage.Designer.cs, so open this file's designer
/// in Visual Studio to move or restyle anything. Everything shown comes from dbo.Services / dbo.Appointments through
/// LoadData(); with no services the tiles show 0 and the table, popular list and category counts stay empty.
/// </summary>
internal partial class ServiceCatalogPage : UserControl
{
    private const int PageSize = 10;

    private List<ServiceRepository.ServiceRow> _all = new();
    private Dictionary<int, ServiceRepository.Bookings> _bookings = new();
    private int _page = 1;
    private readonly FlowLayoutPanel popularList = new()
    {
        Dock = DockStyle.Fill,
        FlowDirection = FlowDirection.TopDown,
        WrapContents = false,
        BackColor = Color.White,
        Visible = false,
    };

    /// <summary>"+ Add Service" or the "Add New Service" quick action was clicked.</summary>
    public event EventHandler? AddServiceRequested;

    /// <summary>The "Manage Pricing" quick action was clicked.</summary>
    public event EventHandler? ManagePricingRequested;

    /// <summary>The "Set Time Slots" quick action was clicked.</summary>
    public event EventHandler? SetTimeSlotsRequested;

    /// <summary>The Edit link on a service row (or a service card) was clicked.</summary>
    public event Action<ServiceRepository.ServiceRow>? EditServiceRequested;

    /// <summary>The Activate / Deactivate link on a service row was clicked.</summary>
    public event Action<int, bool>? ToggleServiceActiveRequested;

    public ServiceCatalogPage()
    {
        InitializeComponent();
        DoubleBuffered = true;

        categoryFilter.SetItems(new[] { "All Categories", "Grooming", "Health & Wellness", "Add-ons", "Others" });
        sizeFilter.SetItems(new[] { "All Sizes", "XS", "Small", "Medium", "Large", "XL", "Giant", "All" });
        statusFilter.SetItems(new[] { "All Status", "Active", "Inactive" });

        popularCard.Controls.Add(popularList);
        popularList.BringToFront();
        popularList.SizeChanged += (_, _) =>
        {
            foreach (Control row in popularList.Controls) row.Width = popularList.ClientSize.Width;
        };

        addServiceButton.Click += (_, _) => AddServiceRequested?.Invoke(this, EventArgs.Empty);
        OnClick(addServiceAction, (_, _) => AddServiceRequested?.Invoke(this, EventArgs.Empty));
        OnClick(pricingAction, (_, _) => ManagePricingRequested?.Invoke(this, EventArgs.Empty));
        OnClick(timeSlotsAction, (_, _) => SetTimeSlotsRequested?.Invoke(this, EventArgs.Empty));

        listViewToggle.Click += (_, _) => SetView(list: true);
        gridViewToggle.Click += (_, _) => SetView(list: false);

        serviceSearch.Box.TextChanged += (_, _) => { _page = 1; Apply(); };
        categoryFilter.SelectedIndexChanged += (_, _) =>
        {
            // Choosing "All Categories" really shows everything: leftover search text / size / status are cleared too.
            if (categoryFilter.SelectedIndex == 0) ClearOtherFilters();
            _page = 1;
            Apply();
        };
        sizeFilter.SelectedIndexChanged += (_, _) => { _page = 1; Apply(); };
        statusFilter.SelectedIndexChanged += (_, _) => { _page = 1; Apply(); };
        servicePrevPage.Click += (_, _) => { _page--; Apply(); };
        serviceNextPage.Click += (_, _) => { _page++; Apply(); };
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

    // Child controls swallow clicks, so hook the row and everything inside it.
    private static void OnClick(Control root, EventHandler handler)
    {
        root.Click += handler;
        foreach (Control child in root.Controls)
            OnClick(child, handler);
    }

    // ------------------------------------------------------------------ data

    /// <summary>Stores the full service list and booking counts from the database, then shows them through the current filters.</summary>
    public void LoadData(List<ServiceRepository.ServiceRow> services, Dictionary<int, ServiceRepository.Bookings> bookings)
    {
        _all = services;
        _bookings = bookings;

        // The four standard categories, plus any other category that exists in the data, so nothing is left out of the drop-down.
        var known = new[] { "Grooming", "Health & Wellness", "Add-ons", "Others" };
        var extra = services.Select(s => s.Category).Where(c => !string.IsNullOrWhiteSpace(c))
            .Distinct(StringComparer.OrdinalIgnoreCase).Except(known, StringComparer.OrdinalIgnoreCase)
            .OrderBy(c => c, StringComparer.CurrentCultureIgnoreCase);
        categoryFilter.SetItems(new[] { "All Categories" }.Concat(known).Concat(extra));

        Apply();
    }

    private bool _clearing;

    private void ClearOtherFilters()
    {
        if (_clearing) return;
        _clearing = true;
        try
        {
            serviceSearch.Box.Text = "";
            sizeFilter.SelectedIndex = 0;
            statusFilter.SelectedIndex = 0;
        }
        finally { _clearing = false; }
    }

    private int BookingsOf(ServiceRepository.ServiceRow s) => _bookings.TryGetValue(s.Id, out var b) ? b.Total : 0;

    private void Apply()
    {
        string q = serviceSearch.Value.Trim();
        string? category = categoryFilter.SelectedIndex > 0 ? categoryFilter.SelectedItem : null;
        string? size = sizeFilter.SelectedIndex > 0 ? sizeFilter.SelectedItem : null;

        var shown = _all.Where(s =>
            (category is null || s.Category == category) &&
            (size is null || s.Size == size) &&
            StatusMatches(statusFilter.SelectedIndex, s.Active) &&
            (q.Length == 0 ||
             s.Name.Contains(q, StringComparison.OrdinalIgnoreCase) ||
             s.Category.Contains(q, StringComparison.OrdinalIgnoreCase) ||
             (s.Description ?? "").Contains(q, StringComparison.OrdinalIgnoreCase))).ToList();

        // "All Categories" lists every category one after another (grouped), 10 services per page, so the pager works.
        if (category is null)
        {
            var order = categoryFilter.Items.ToList();   // drop-down order: Grooming, Health & Wellness, ...
            shown = shown.OrderBy(s => { int i = order.IndexOf(s.Category); return i < 0 ? int.MaxValue : i; }).ToList();
        }

        int pageSize = PageSize;
        int pages = Math.Max(1, (int)Math.Ceiling(shown.Count / (double)pageSize));
        _page = Math.Clamp(_page, 1, pages);
        var slice = shown.Skip((_page - 1) * pageSize).Take(pageSize).ToList();

        serviceRows.SuspendLayout();
        ClearRows(serviceRows, serviceEmpty);
        serviceEmpty.Visible = slice.Count == 0;
        serviceColumnsHead.Visible = IsListView;
        if (IsListView)
        {
            for (int i = slice.Count - 1; i >= 0; i--)
                serviceRows.Controls.Add(BuildRow(slice[i]));
        }
        else if (slice.Count > 0)
        {
            serviceRows.Controls.Add(BuildCards(slice));
        }
        serviceRows.ResumeLayout(true);

        serviceShowing.Text = shown.Count == 0
            ? "Showing 0 services"
            : $"Showing {(_page - 1) * pageSize + 1} to {(_page - 1) * pageSize + slice.Count} of {shown.Count} services";
        servicePageChip.Text = _page.ToString(CultureInfo.CurrentCulture);
        servicePrevPage.Enabled = _page > 1;
        serviceNextPage.Enabled = _page < pages;

        ApplyTiles();
        ApplyCategories();
        ApplyPopular();
    }

    // ------------------------------------------------------------------ table / cards

    private Control BuildRow(ServiceRepository.ServiceRow s)
    {
        var grid = NewRowGrid(serviceColumnsGrid);

        void Put(Control header, string text, Color? color = null)
            => grid.Controls.Add(NewCell(text, color), serviceColumnsGrid.GetColumn(header), 0);

        string extra = ServicePricing.BandText(s.Species, s.MinKg, s.MaxKg);
        if (extra.Length == 0 && !string.Equals(s.Species, "Both", StringComparison.OrdinalIgnoreCase)) extra = s.Species;

        Put(serviceHeaderCheck, "");
        Put(serviceColService, s.Name);
        Put(serviceColCategory, s.Category);
        Put(serviceColSize, extra.Length == 0 ? s.Size : $"{s.Size} \u00B7 {extra}");
        Put(serviceColDuration, $"{s.DurationMinutes} mins");
        Put(serviceColPrice, Money(s.Price));
        Put(serviceColStatus, s.Active ? "Active" : "Inactive", s.Active ? Good : Quiet);

        grid.Controls.Add(NewActions(
            ("Edit", () => EditServiceRequested?.Invoke(s)),
            (s.Active ? "Deactivate" : "Activate", () => ToggleServiceActiveRequested?.Invoke(s.Id, !s.Active))),
            serviceColumnsGrid.GetColumn(serviceColActions), 0);

        return WrapRow(grid);
    }

    private Control BuildCards(List<ServiceRepository.ServiceRow> slice)
    {
        var flow = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoScroll = true, BackColor = Color.White, Padding = new Padding(0, 12, 0, 0) };
        foreach (var s in slice)
        {
            var card = new ServiceCard
            {
                Text = s.Active ? s.Name : s.Name + " (inactive)",
                Price = Money(s.Price),
                Duration = $"~ {s.DurationMinutes} mins",
                BackColor = Color.White,
                Size = new Size(210, 120),
                Margin = new Padding(0, 0, 12, 12),
            };
            var row = s;
            card.Click += (_, _) => EditServiceRequested?.Invoke(row);
            flow.Controls.Add(card);
        }
        return flow;
    }

    private static string Money(decimal value) => "\u20B1" + value.ToString("N2", CultureInfo.CurrentCulture);

    // ------------------------------------------------------------------ tiles, categories, popular

    private void ApplyTiles()
    {
        var culture = CultureInfo.CurrentCulture;
        var active = _all.Where(s => s.Active).ToList();

        svcTotalValue.Text = _all.Count.ToString(culture);
        svcTotalDelta.Text = AddedText(_all.Count(s => s.CreatedAt >= MonthStart.ToUniversalTime()));

        svcPriceValue.Text = Money(active.Count == 0 ? 0 : active.Average(s => s.Price));
        svcPriceDelta.Text = active.Count == 0 ? "No active services" : $"Across {active.Count} active";

        svcDurationValue.Text = $"{(active.Count == 0 ? 0 : (int)Math.Round(active.Average(s => s.DurationMinutes)))} mins";
        svcDurationDelta.Text = svcPriceDelta.Text;

        int total = _all.Sum(BookingsOf);
        int thisMonth = _bookings.Values.Sum(b => b.ThisMonth);
        svcBookingsValue.Text = total.ToString(culture);
        svcBookingsDelta.Text = thisMonth <= 0 ? "No bookings this month" : $"+{thisMonth.ToString(culture)} this month";
    }

    private void ApplyCategories()
    {
        int Count(string category) => _all.Count(s => s.Category == category);
        catGroomingCount.Text = Count("Grooming").ToString(CultureInfo.CurrentCulture);
        catHealthCount.Text = Count("Health & Wellness").ToString(CultureInfo.CurrentCulture);
        catAddOnsCount.Text = Count("Add-ons").ToString(CultureInfo.CurrentCulture);
        catOthersCount.Text = Count("Others").ToString(CultureInfo.CurrentCulture);
    }

    /// <summary>Top 5 most booked services (the weight bands of one service are added together).</summary>
    private void ApplyPopular()
    {
        var top = _all.GroupBy(s => s.Name)
            .Select(g => (Name: g.Key, Count: g.Sum(BookingsOf)))
            .Where(x => x.Count > 0)
            .OrderByDescending(x => x.Count).ThenBy(x => x.Name)
            .Take(5).ToList();

        popularList.SuspendLayout();
        foreach (var old in popularList.Controls.Cast<Control>().ToList())
        {
            popularList.Controls.Remove(old);
            old.Dispose();
        }
        foreach (var (name, count) in top)
        {
            var row = new Panel { Height = 32, Width = popularList.ClientSize.Width, Margin = Padding.Empty, BackColor = Color.White };
            row.Controls.Add(new Label
            {
                Text = name, Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft, AutoEllipsis = true,
                UseMnemonic = false, ForeColor = Ink, BackColor = Color.White, Font = new Font("Segoe UI", 9F),
            });
            row.Controls.Add(new Label
            {
                Text = count == 1 ? "1 booking" : $"{count.ToString(CultureInfo.CurrentCulture)} bookings", Dock = DockStyle.Right, Width = 90,
                TextAlign = ContentAlignment.MiddleRight, ForeColor = Quiet, BackColor = Color.White, Font = new Font("Segoe UI", 9F, FontStyle.Bold),
            });
            popularList.Controls.Add(row);
        }
        popularList.Visible = top.Count > 0;
        popularEmpty.Visible = top.Count == 0;
        popularList.ResumeLayout(true);
    }
}


/// <summary>Everything the Service Catalog page needs from dbo.Services.</summary>
internal static class ServiceRepository
{

    public sealed record ServiceRow(
        int Id, string Name, string Category, string Size, string? Description,
        decimal Price, int DurationMinutes, bool Active,
        string Species = "Both", decimal? MinKg = null, decimal? MaxKg = null,   // weight band: price applies when MinKg < weight <= MaxKg
        DateTime CreatedAt = default);   // UTC

    public static List<ServiceRow> GetAll()
    {
        var list = new List<ServiceRow>();

        using var conn = new SqlConnection(Database.ConnectionString);
        conn.Open();
        ServicePricing.EnsureSchema(conn);

        using var cmd = new SqlCommand(
            "SELECT ServiceId, Name, Category, Size, Description, Price, DurationMinutes, IsActive, Species, MinKg, MaxKg, CreatedAt " +
            "FROM dbo.Services ORDER BY Name, Species, ISNULL(MinKg, 0), Size", conn);
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
            list.Add(new ServiceRow(
                reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3),
                reader.IsDBNull(4) ? null : reader.GetString(4),
                reader.GetDecimal(5), reader.GetInt32(6), reader.GetBoolean(7),
                reader.GetString(8), reader.IsDBNull(9) ? null : reader.GetDecimal(9), reader.IsDBNull(10) ? null : reader.GetDecimal(10),
                reader.IsDBNull(11) ? default : reader.GetDateTime(11)));
        return list;
    }

    public sealed record Bookings(int Total, int ThisMonth);

    /// <summary>Bookings per dbo.Services row (cancelled ones don't count), plus how many started this month.</summary>
    public static Dictionary<int, Bookings> GetBookings()
    {
        var map = new Dictionary<int, Bookings>();
        var now = DateTime.Now;

        using var conn = new SqlConnection(Database.ConnectionString);
        conn.Open();

        using var cmd = new SqlCommand(
            "SELECT ServiceId, COUNT(*), SUM(CASE WHEN StartTime >= @m THEN 1 ELSE 0 END) " +
            "FROM dbo.Appointments WHERE Status <> N'Cancelled' GROUP BY ServiceId", conn);
        cmd.Parameters.Add("@m", SqlDbType.DateTime2).Value = new DateTime(now.Year, now.Month, 1);
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
            map[reader.GetInt32(0)] = new Bookings(reader.GetInt32(1), reader.IsDBNull(2) ? 0 : reader.GetInt32(2));
        return map;
    }

    /// <summary>Throws SqlException 2627/2601 if this name and size already exist.</summary>
    public static int Add(string name, string category, string size, string? description, decimal price, int minutes,
        string species = "Both", decimal? minKg = null, decimal? maxKg = null)
    {
        using var conn = new SqlConnection(Database.ConnectionString);
        conn.Open();
        ServicePricing.EnsureSchema(conn);

        using var cmd = new SqlCommand(
            "INSERT INTO dbo.Services (Name, Category, Size, Description, Price, DurationMinutes, Species, MinKg, MaxKg) " +
            "OUTPUT INSERTED.ServiceId VALUES (@n, @c, @s, @d, @p, @m, @sp, @min, @max)", conn);
        Fill(cmd, name, category, size, description, price, minutes, species, minKg, maxKg);
        return (int)cmd.ExecuteScalar()!;
    }

    /// <summary>Throws SqlException 2627/2601 if this name and size are taken.</summary>
    public static void Update(int id, string name, string category, string size, string? description, decimal price, int minutes,
        string species = "Both", decimal? minKg = null, decimal? maxKg = null)
    {
        using var conn = new SqlConnection(Database.ConnectionString);
        conn.Open();
        ServicePricing.EnsureSchema(conn);

        using var cmd = new SqlCommand(
            "UPDATE dbo.Services SET Name = @n, Category = @c, Size = @s, Description = @d, " +
            "Price = @p, DurationMinutes = @m, Species = @sp, MinKg = @min, MaxKg = @max WHERE ServiceId = @id", conn);
        Fill(cmd, name, category, size, description, price, minutes, species, minKg, maxKg);
        cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
        cmd.ExecuteNonQuery();
    }

    /// <summary>Soft delete: hides the service from new bookings but keeps old records valid.</summary>
    public static void SetActive(int id, bool active)
    {
        using var conn = new SqlConnection(Database.ConnectionString);
        conn.Open();

        using var cmd = new SqlCommand("UPDATE dbo.Services SET IsActive = @a WHERE ServiceId = @id", conn);
        cmd.Parameters.Add("@a", SqlDbType.Bit).Value = active;
        cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
        cmd.ExecuteNonQuery();
    }

    /// <summary>Saves several prices in one transaction (Manage Pricing).</summary>
    public static void UpdatePrices(IEnumerable<(int Id, decimal Price)> changes)
    {
        using var conn = new SqlConnection(Database.ConnectionString);
        conn.Open();

        using var tx = conn.BeginTransaction();
        foreach (var (id, price) in changes)
        {
            using var cmd = new SqlCommand("UPDATE dbo.Services SET Price = @p WHERE ServiceId = @id", conn, tx);
            var p = cmd.Parameters.Add("@p", SqlDbType.Decimal);
            p.Precision = 10;
            p.Scale = 2;
            p.Value = price;
            cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
            cmd.ExecuteNonQuery();
        }
        tx.Commit();
    }

    private static void Fill(SqlCommand cmd, string name, string category, string size, string? description, decimal price, int minutes,
        string species, decimal? minKg, decimal? maxKg)
    {
        cmd.Parameters.Add("@sp", SqlDbType.NVarChar, 10).Value = species;
        foreach (var (pname, kg) in new[] { ("@min", minKg), ("@max", maxKg) })
        {
            var w = cmd.Parameters.Add(pname, SqlDbType.Decimal);
            w.Precision = 6;
            w.Scale = 2;
            w.Value = kg is decimal v && v > 0 ? (object)v : DBNull.Value;   // 0 / empty = no limit
        }
        cmd.Parameters.Add("@n", SqlDbType.NVarChar, 100).Value = name.Trim();
        cmd.Parameters.Add("@c", SqlDbType.NVarChar, 50).Value = category;
        cmd.Parameters.Add("@s", SqlDbType.NVarChar, 10).Value = size;
        cmd.Parameters.Add("@d", SqlDbType.NVarChar, 300).Value =
            string.IsNullOrWhiteSpace(description) ? DBNull.Value : description.Trim();
        var p = cmd.Parameters.Add("@p", SqlDbType.Decimal);
        p.Precision = 10;
        p.Scale = 2;
        p.Value = price;
        cmd.Parameters.Add("@m", SqlDbType.Int).Value = minutes;
    }
}