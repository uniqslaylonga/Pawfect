using System.Globalization;
using MyApp.Controls;

namespace MyApp;

/// <summary>
/// Appointments screen. Open this file's designer in Visual Studio to move or restyle anything.
/// It shows only what you give it through <see cref="SetAppointments"/>; with nothing loaded it shows empty states.
/// </summary>
internal partial class AppointmentsPage : UserControl
{
    private const int PageSize = 8;

    private IReadOnlyList<AppointmentItem> _all = Array.Empty<AppointmentItem>();
    private List<AppointmentItem> _filtered = new();
    private readonly HashSet<int> _checkedIds = new();
    private DateTime? _rangeStart;          // Sunday of the week being shown, or null for "all dates"
    private int _pageIndex;
    private bool _suspendFilter;
    private enum StatusGroup { None, Waiting, Confirmed, WalkIn }
    private StatusGroup _group;   // set by a stat tile or ShowPending: Waiting = Triage Pending + Rescheduled, In queue = Admitted + In-Service + Sanitation Buffer + Ready for Pick-up; picking a status in the drop-down clears it
    private IReadOnlyList<AppointmentItem> _walkInSales = Array.Empty<AppointmentItem>();   // today's walk-in sales, listed only under the Walk-ins view
    private DateTime? _dayOnly;   // set by ShowToday: list just this one day instead of the whole week
    private bool _building;

    public event EventHandler<AppointmentItem>? ViewRequested;
    public event EventHandler<AppointmentItem>? EditRequested;
    public event EventHandler<AppointmentItem>? MoreRequested;
    public event EventHandler? ManageCustomersRequested;
    public event EventHandler? ServicesCatalogRequested;
    public event EventHandler? ViewFullCalendarRequested;
    public event EventHandler? NewAppointmentRequested;

    public AppointmentsPage()
    {
        InitializeComponent();
        DoubleBuffered = true;

        // Quick Actions: a "New Appointment" tile next to Services Catalog (staff book walk-ins and phone calls).
        quickGrid.SetColumnSpan(servicesCatalogTile, 1);
        var newAppointmentTile = new MyApp.Controls.ActionTile
        {
            BackColor = Color.White,
            Dock = DockStyle.Fill,
            Font = servicesCatalogTile.Font,
            Glyph = "\uE710",
            Name = "newAppointmentTile",
            Text = "New Appointment",
        };
        newAppointmentTile.Click += (_, _) => NewAppointmentRequested?.Invoke(this, EventArgs.Empty);
        quickGrid.Controls.Add(newAppointmentTile, 1, 1);

        FitLegend();

        // The four stat tiles at the top filter the list below to the appointments they count.
        Ui.MakeClickable(totalCard, (_, _) => ApplyTile(null, StatusGroup.None));
        Ui.MakeClickable(pendingCard, (_, _) => ApplyTile(AppointmentStatus.Pending, StatusGroup.Waiting));
        Ui.MakeClickable(confirmedCard, (_, _) => ApplyTile(AppointmentStatus.Confirmed, StatusGroup.Confirmed));
        Ui.MakeClickable(cancelledCard, (_, _) => ApplyTile(AppointmentStatus.Cancelled, StatusGroup.None));

        components ??= new System.ComponentModel.Container();
        new ToolTip(components).SetToolTip(dateRange, "Click the dates to switch between this week and all dates.");

        statusFilter.SetItems(new[] { "All Statuses" }
            .Concat(Enum.GetValues<AppointmentStatus>().Select(AppointmentStatusStyle.Label)));
        serviceFilter.SetItems(new[] { "All Services" });

        searchField.Box.TextChanged += (_, _) =>
        {
            if (!_suspendFilter) ApplyFilters(resetPage: true);
        };

        toolbar.Resize += (_, _) => LayoutToolbar();

        _rangeStart = WeekStart(DateTime.Today);
        UpdateRangeText();
        ApplyFilters(resetPage: true);
    }

    // ------------------------------------------------------------------ public API

    /// <summary>Replaces everything shown on the page with these appointments.</summary>
    public void SetAppointments(IEnumerable<AppointmentItem> appointments)
    {
        _all = appointments.ToList();
        _checkedIds.IntersectWith(_all.Select(a => a.Id));

        serviceFilter.SetItems(new[] { "All Services" }.Concat(
            _all.Select(a => a.Service)
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .OrderBy(s => s, StringComparer.CurrentCultureIgnoreCase)));

        monthCalendar.SetMarkers(_all);
        ApplyFilters(resetPage: false);
    }

    /// <summary>Today's walk-in sales (negative ids). They only appear while the Walk-ins view is open and never count in the tiles or the calendar.</summary>
    public void SetWalkInSales(IEnumerable<AppointmentItem> sales)
    {
        _walkInSales = sales.ToList();
        if (_group == StatusGroup.WalkIn) ApplyFilters(resetPage: false);
    }

    /// <summary>Every appointment currently loaded (used by the full calendar window).</summary>
    public IReadOnlyList<AppointmentItem> Appointments => _all;

    /// <summary>Clears the search and filters, then lists just this day (picked in the calendar) with the day selected.</summary>
    public void ShowDate(DateTime date)
    {
        _suspendFilter = true;
        searchField.Box.Text = "";
        statusFilter.SelectedIndex = 0;
        serviceFilter.SelectedIndex = 0;
        _suspendFilter = false;

        SetRange(WeekStart(date), date, dayOnly: true);
    }

    /// <summary>
    /// Sizes each legend label to its text (the designer widths cut "Confirmed", "Pending" and the rest short) and keeps every
    /// dot together with its label. If all four don't fit on one line they sit in a neat 2 x 2 grid instead.
    /// </summary>
    private void FitLegend()
    {
        var pairs = new (Control Dot, Label Label)[]
        {
            (legendConfirmedDot, legendConfirmedLabel), (legendPendingDot, legendPendingLabel),
            (legendRescheduledDot, legendRescheduledLabel), (legendCancelledDot, legendCancelledLabel),
        };

        foreach (var (_, label) in pairs)
        {
            label.AutoSize = false;
            label.Height = 20;
            label.Width = TextRenderer.MeasureText(label.Text, label.Font).Width + 10;
        }

        const int gap = 10;
        int need = pairs.Sum(p => p.Dot.Width + p.Label.Width + gap);
        bool grid = need > legendFlow.ClientSize.Width;
        int cell = pairs.Max(p => p.Dot.Width + p.Label.Width) + gap;

        legendFlow.SuspendLayout();
        legendFlow.Controls.Clear();
        legendFlow.WrapContents = true;
        foreach (var (dot, label) in pairs)
        {
            var holder = new Panel
            {
                BackColor = Color.White,
                Height = 20,
                Width = grid ? cell : dot.Width + label.Width + gap,
                Margin = new Padding(0),
            };
            dot.Margin = Padding.Empty;
            label.Margin = Padding.Empty;
            dot.Location = new Point(0, 0);
            label.Location = new Point(dot.Width, 0);
            holder.Controls.Add(dot);
            holder.Controls.Add(label);
            legendFlow.Controls.Add(holder);
        }

        if (grid)
        {
            legendFlow.Height = 52;
            rightColumn.RowStyles[0].Height += 24;
        }
        legendFlow.ResumeLayout(true);
    }

    /// <summary>Clears the search and service filter, then lists every Pending appointment on all dates.</summary>
    public void ShowPending()
    {
        _suspendFilter = true;
        searchField.Box.Text = "";
        serviceFilter.SelectedIndex = 0;
        statusFilter.SelectedIndex = Array.IndexOf(Enum.GetValues<AppointmentStatus>(), AppointmentStatus.Pending) + 1;   // index 0 is "All Statuses"
        _group = StatusGroup.Waiting;   // after the index change, whose handler clears the group
        _suspendFilter = false;

        SetRange(null);
    }

    /// <summary>Lists just today's appointments, optionally only those with one status (dashboard tiles).</summary>
    public void ShowToday(AppointmentStatus? status = null)
    {
        _suspendFilter = true;
        searchField.Box.Text = "";
        serviceFilter.SelectedIndex = 0;
        statusFilter.SelectedIndex = status is null ? 0 : Array.IndexOf(Enum.GetValues<AppointmentStatus>(), status.Value) + 1;
        _group = StatusGroup.None;
        _suspendFilter = false;

        SetRange(WeekStart(DateTime.Today), DateTime.Today);
        _dayOnly = DateTime.Today;   // after SetRange, which clears it
        UpdateRangeText();
        ApplyFilters(resetPage: true);
    }

    /// <summary>Lists today's walk-ins: customers who came without a booking (kiosk or staff-entered) and today's walk-in sales.</summary>
    public void ShowWalkInsToday()
    {
        _suspendFilter = true;
        searchField.Box.Text = "";
        serviceFilter.SelectedIndex = 0;
        statusFilter.SelectedIndex = 0;
        _group = StatusGroup.WalkIn;   // after the index change, whose handler clears the group
        _suspendFilter = false;

        SetRange(WeekStart(DateTime.Today), DateTime.Today);
        _dayOnly = DateTime.Today;   // after SetRange, which clears it
        UpdateRangeText();
        ApplyFilters(resetPage: true);
    }

    /// <summary>
    /// A stat tile was clicked: show the appointments it counts (same date range, no search or service filter, so the list
    /// has exactly as many rows as the number on the tile).
    /// </summary>
    private void ApplyTile(AppointmentStatus? status, StatusGroup group)
    {
        _suspendFilter = true;
        searchField.Box.Text = "";
        serviceFilter.SelectedIndex = 0;
        statusFilter.SelectedIndex = status is null ? 0 : Array.IndexOf(Enum.GetValues<AppointmentStatus>(), status.Value) + 1;
        _group = group;   // after the index change, whose handler clears the group
        _suspendFilter = false;

        ApplyFilters(resetPage: true);
    }

    /// <summary>Outlines the stat tile whose list is showing.</summary>
    private void UpdateTileHighlight()
    {
        CardPanel? active = null;
        if (_group == StatusGroup.WalkIn) active = null;
        else if (_group == StatusGroup.Waiting) active = pendingCard;
        else if (_group == StatusGroup.Confirmed) active = confirmedCard;
        else if (statusFilter.SelectedIndex == 0) active = totalCard;
        else if (statusFilter.SelectedItem == AppointmentStatusStyle.Label(AppointmentStatus.Cancelled)) active = cancelledCard;

        foreach (var card in new[] { totalCard, pendingCard, confirmedCard, cancelledCard })
            card.BorderColor = ReferenceEquals(card, active) ? Theme.Primary : Theme.Border;
    }

    /// <summary>Clears the search, filters and week so every appointment is listed.</summary>
    public void ShowAllDates() => viewAllTile_Click(this, EventArgs.Empty);

    /// <summary>The appointments whose row checkbox is ticked.</summary>
    public IReadOnlyList<AppointmentItem> GetCheckedAppointments()
        => _all.Where(a => _checkedIds.Contains(a.Id)).ToList();

    // ------------------------------------------------------------------ filtering

    private static DateTime WeekStart(DateTime day) => day.Date.AddDays(-(int)day.DayOfWeek);

    private bool InRange(DateTime when)
        => _dayOnly is { } day ? when.Date == day :
           _rangeStart is null || (when.Date >= _rangeStart.Value && when.Date <= _rangeStart.Value.AddDays(6));

    private static bool Matches(AppointmentItem a, string query)
    {
        static bool Has(string value, string q) => value.Contains(q, StringComparison.CurrentCultureIgnoreCase);
        return Has(a.CustomerName, query) || Has(a.PetName, query) || Has(a.Service, query) || Has(a.QueueNumber, query);
    }

    private int PageCount => Math.Max(1, (int)Math.Ceiling(_filtered.Count / (double)PageSize));

    private void ApplyFilters(bool resetPage)
    {
        if (resetPage) _pageIndex = 0;

        string query = searchField.Value.Trim();
        AppointmentStatus? status = statusFilter.SelectedIndex > 0
            ? Enum.GetValues<AppointmentStatus>()[statusFilter.SelectedIndex - 1]
            : null;
        string? service = serviceFilter.SelectedIndex > 0 ? serviceFilter.SelectedItem : null;

        // Walk-ins view: the walk-in appointments plus today's walk-in sales, so the list matches the dashboard's Walk-ins number.
        IEnumerable<AppointmentItem> source = _group == StatusGroup.WalkIn
            ? _all.Where(a => a.IsWalkIn && a.Status != AppointmentStatus.Cancelled).Concat(_walkInSales)
            : _all;

        _filtered = source
            .Where(a => InRange(a.Start))
            .Where(a => _group switch
            {
                StatusGroup.Waiting => a.Status is AppointmentStatus.Pending or AppointmentStatus.Rescheduled,
                StatusGroup.Confirmed => IsInQueue(a.Status),
                StatusGroup.WalkIn => true,
                _ => status is null || a.Status == status,
            })
            .Where(a => service is null || string.Equals(a.Service, service, StringComparison.OrdinalIgnoreCase))
            .Where(a => query.Length == 0 || Matches(a, query))
            .OrderBy(a => a.Start)
            .ToList();

        if (_pageIndex >= PageCount) _pageIndex = PageCount - 1;

        BuildRows();
        UpdateFooter();
        UpdateStats();
        UpdateUpcoming();
        UpdateTileHighlight();
    }

    private void UpdateRangeText()
    {
        if (_dayOnly is { } day)
        {
            string text = day.ToString("MMM d, yyyy", CultureInfo.CurrentCulture);
            dateRange.Text = day == DateTime.Today ? "Today \u00B7 " + text : text;
            LayoutToolbar();
            return;
        }

        if (_rangeStart is null)
        {
            dateRange.Text = "All dates";
            return;
        }

        var start = _rangeStart.Value;
        var end = start.AddDays(6);
        var culture = CultureInfo.CurrentCulture;
        dateRange.Text = $"{start.ToString("MMM d, yyyy", culture)} – {end.ToString("MMM d, yyyy", culture)}";
        LayoutToolbar();
    }

    /// <summary>
    /// Lays the filter bar out from the right edge: the date chip is sized to its text, then the two drop-downs,
    /// and the search box takes whatever width is left.
    /// </summary>
    private void LayoutToolbar()
    {
        const int gap = 8;
        int right = toolbar.ClientSize.Width;
        if (right < 200) return;

        int textWidth = TextRenderer.MeasureText(dateRange.Text, dateRange.Font).Width;
        dateRange.Width = Math.Max(170, textWidth + 32 + 54 + 8);   // icon + text + the two arrows
        dateRange.Left = right - dateRange.Width;
        serviceFilter.Left = dateRange.Left - gap - serviceFilter.Width;
        statusFilter.Left = serviceFilter.Left - gap - statusFilter.Width;
        searchField.Width = Math.Max(120, statusFilter.Left - gap - searchField.Left);
    }

    private void SetRange(DateTime? weekStart, DateTime? selectedDay = null, bool dayOnly = false)
    {
        _dayOnly = dayOnly ? selectedDay?.Date : null;
        _rangeStart = weekStart;
        monthCalendar.SelectedDate = selectedDay;
        // Show the month of the day that was picked. (Using the week's first day flipped the calendar to the
        // previous month whenever the week started there.) For plain week navigation use the middle of the week.
        var focus = selectedDay ?? weekStart?.AddDays(3);
        if (focus is not null) monthCalendar.Month = focus.Value;
        UpdateRangeText();
        ApplyFilters(resetPage: true);
    }

    // ------------------------------------------------------------------ table

    private void BuildRows()
    {
        _building = true;
        rowsHost.SuspendLayout();

        foreach (var old in rowsHost.Controls.OfType<AppointmentRowView>().ToList())
        {
            rowsHost.Controls.Remove(old);
            old.Dispose();
        }

        var page = _filtered.Skip(_pageIndex * PageSize).Take(PageSize).ToList();

        // Rows are docked to the top, so add them last-to-first to keep the earliest on top.
        for (int i = page.Count - 1; i >= 0; i--)
        {
            var item = page[i];
            var row = new AppointmentRowView(item) { Dock = DockStyle.Top, Checked = _checkedIds.Contains(item.Id) };
            row.CheckedChanged += (_, _) =>
            {
                if (row.Checked) _checkedIds.Add(item.Id); else _checkedIds.Remove(item.Id);
                SyncHeaderCheck();
            };
            row.ViewClicked += (_, _) => ViewRequested?.Invoke(this, item);
            row.EditClicked += (_, _) => EditRequested?.Invoke(this, item);
            row.MoreClicked += (_, _) => MoreRequested?.Invoke(this, item);
            rowsHost.Controls.Add(row);
        }

        bool hasData = _all.Count > 0;
        appointmentsEmpty.Title = hasData ? "No matching appointments" : "No appointments found";
        appointmentsEmpty.Hint = hasData
            ? "Try a different search, filter or week."
            : "Appointments will show up here once they are booked.";
        appointmentsEmpty.Visible = page.Count == 0;

        rowsHost.AutoScrollPosition = Point.Empty;
        rowsHost.ResumeLayout(true);
        _building = false;
        SyncHeaderCheck();
    }

    private void SyncHeaderCheck()
    {
        if (_building) return;
        var rows = rowsHost.Controls.OfType<AppointmentRowView>().ToList();
        bool all = rows.Count > 0 && rows.All(r => r.Checked);
        if (headerCheck.Checked == all) return;

        _building = true;
        headerCheck.Checked = all;
        _building = false;
    }

    private void UpdateFooter()
    {
        if (_filtered.Count == 0)
        {
            showingLabel.Text = "Showing 0 appointments";
        }
        else
        {
            int from = _pageIndex * PageSize + 1;
            int to = Math.Min(_filtered.Count, from + PageSize - 1);
            showingLabel.Text = $"Showing {from}–{to} of {_filtered.Count} appointments";
        }

        pageChip.Text = (_pageIndex + 1).ToString(CultureInfo.CurrentCulture);
        prevPageButton.Enabled = _pageIndex > 0;
        nextPageButton.Enabled = _pageIndex < PageCount - 1;
    }

    // ------------------------------------------------------------------ side panels

    /// <summary>Admitted, In-Service, Sanitation Buffer and Ready for Pick-up: the pet has passed triage and is not finished yet.</summary>
    private static bool IsInQueue(AppointmentStatus s)
        => s is AppointmentStatus.Confirmed or AppointmentStatus.CheckedIn or AppointmentStatus.SanitationBuffer or AppointmentStatus.ReadyForPickup;

    private void UpdateStats()
    {
        var inRange = _all.Where(a => InRange(a.Start)).ToList();
        var today = inRange.Where(a => a.Start.Date == DateTime.Today).ToList();

        static bool IsPending(AppointmentItem a) => a.Status is AppointmentStatus.Pending or AppointmentStatus.Rescheduled;
        static bool IsConfirmed(AppointmentItem a) => IsInQueue(a.Status);
        static bool IsCancelled(AppointmentItem a) => a.Status == AppointmentStatus.Cancelled;

        void SetStat(Label value, Label delta, Func<AppointmentItem, bool> match)
        {
            value.Text = inRange.Count(match).ToString(CultureInfo.CurrentCulture);
            delta.Text = $"{today.Count(match)} today";
        }

        SetStat(totalValue, totalDelta, _ => true);
        SetStat(pendingValue, pendingDelta, IsPending);
        SetStat(confirmedValue, confirmedDelta, IsConfirmed);
        SetStat(cancelledValue, cancelledDelta, IsCancelled);
    }

    private void UpdateUpcoming()
    {
        // "Upcoming" = still to be served today, so finished (Completed) and Cancelled appointments are left out.
        var today = _all
            .Where(a => a.Start.Date == DateTime.Today
                && a.Status is not (AppointmentStatus.Cancelled or AppointmentStatus.Completed))
            .OrderBy(a => a.Start)
            .ToList();

        upcomingList.SuspendLayout();
        foreach (var old in upcomingList.Controls.OfType<UpcomingItemView>().ToList())
        {
            upcomingList.Controls.Remove(old);
            old.Dispose();
        }
        for (int i = today.Count - 1; i >= 0; i--)
            upcomingList.Controls.Add(new UpcomingItemView(today[i]) { Dock = DockStyle.Top });
        upcomingList.AutoScrollPosition = Point.Empty;
        upcomingList.ResumeLayout(true);

        upcomingList.Visible = today.Count > 0;
        upcomingEmpty.Visible = today.Count == 0;
    }

    // ------------------------------------------------------------------ events from the designer

    private void statusFilter_SelectedIndexChanged(object? sender, EventArgs e)
    {
        _group = StatusGroup.None;
        if (!_suspendFilter) ApplyFilters(resetPage: true);
    }

    private void serviceFilter_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (!_suspendFilter) ApplyFilters(resetPage: true);
    }

    private void dateRange_PreviousClicked(object? sender, EventArgs e)
        => SetRange((_rangeStart ?? WeekStart(DateTime.Today)).AddDays(-7));

    private void dateRange_NextClicked(object? sender, EventArgs e)
        => SetRange((_rangeStart ?? WeekStart(DateTime.Today)).AddDays(7));

    private void dateRange_TextClicked(object? sender, EventArgs e)
        => SetRange(_rangeStart is null ? WeekStart(DateTime.Today) : null);

    private void monthCalendar_DateClicked(object? sender, DateTime date)
        => SetRange(WeekStart(date), date, dayOnly: true);   // list just the clicked day

    private void headerCheck_CheckedChanged(object? sender, EventArgs e)
    {
        if (_building) return;
        _building = true;
        foreach (var row in rowsHost.Controls.OfType<AppointmentRowView>())
        {
            row.Checked = headerCheck.Checked;
            if (row.Checked) _checkedIds.Add(row.Item.Id); else _checkedIds.Remove(row.Item.Id);
        }
        _building = false;
    }

    private void prevPageButton_Click(object? sender, EventArgs e)
    {
        if (_pageIndex <= 0) return;
        _pageIndex--;
        BuildRows();
        UpdateFooter();
    }

    private void nextPageButton_Click(object? sender, EventArgs e)
    {
        if (_pageIndex >= PageCount - 1) return;
        _pageIndex++;
        BuildRows();
        UpdateFooter();
    }

    /// <summary>"View All Appointments": clears the search, filters and week so every appointment is listed.</summary>
    private void viewAllTile_Click(object? sender, EventArgs e)
    {
        _suspendFilter = true;
        searchField.Box.Text = "";
        statusFilter.SelectedIndex = 0;
        serviceFilter.SelectedIndex = 0;
        _suspendFilter = false;

        SetRange(null);
    }

    private void manageCustomersTile_Click(object? sender, EventArgs e)
        => ManageCustomersRequested?.Invoke(this, EventArgs.Empty);

    private void servicesCatalogTile_Click(object? sender, EventArgs e)
        => ServicesCatalogRequested?.Invoke(this, EventArgs.Empty);

    private void calendarLink_Click(object? sender, EventArgs e)
        => ViewFullCalendarRequested?.Invoke(this, EventArgs.Empty);

    /// <summary>"View All" on Upcoming Today: jump to this week.</summary>
    private void upcomingLink_Click(object? sender, EventArgs e)
        => SetRange(WeekStart(DateTime.Today));

    private void calendarCard_Paint(object sender, PaintEventArgs e)
    {

    }
}
