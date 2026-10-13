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
    private bool _waitingOnly;   // set by ShowPending: list Pending and Rescheduled together; any status change clears it
    private bool _building;

    public event EventHandler<AppointmentItem>? ViewRequested;
    public event EventHandler<AppointmentItem>? EditRequested;
    public event EventHandler<AppointmentItem>? MoreRequested;
    public event EventHandler? ManageCustomersRequested;
    public event EventHandler? ServicesCatalogRequested;
    public event EventHandler? ViewFullCalendarRequested;

    public AppointmentsPage()
    {
        InitializeComponent();
        DoubleBuffered = true;

        FitLegend();

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

    /// <summary>Every appointment currently loaded (used by the full calendar window).</summary>
    public IReadOnlyList<AppointmentItem> Appointments => _all;

    /// <summary>Clears the search and filters, then shows the week containing this day with the day selected.</summary>
    public void ShowDate(DateTime date)
    {
        _suspendFilter = true;
        searchField.Box.Text = "";
        statusFilter.SelectedIndex = 0;
        serviceFilter.SelectedIndex = 0;
        _suspendFilter = false;

        SetRange(WeekStart(date), date);
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
        _waitingOnly = true;   // after the index change, whose handler clears the flag
        _suspendFilter = false;

        SetRange(null);
    }

    /// <summary>Clears the search, filters and week so every appointment is listed.</summary>
    public void ShowAllDates() => viewAllTile_Click(this, EventArgs.Empty);

    /// <summary>The appointments whose row checkbox is ticked.</summary>
    public IReadOnlyList<AppointmentItem> GetCheckedAppointments()
        => _all.Where(a => _checkedIds.Contains(a.Id)).ToList();

    // ------------------------------------------------------------------ filtering

    private static DateTime WeekStart(DateTime day) => day.Date.AddDays(-(int)day.DayOfWeek);

    private bool InRange(DateTime when)
        => _rangeStart is null || (when.Date >= _rangeStart.Value && when.Date <= _rangeStart.Value.AddDays(6));

    private static bool Matches(AppointmentItem a, string query)
    {
        static bool Has(string value, string q) => value.Contains(q, StringComparison.CurrentCultureIgnoreCase);
        return Has(a.CustomerName, query) || Has(a.PetName, query) || Has(a.Service, query);
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

        _filtered = _all
            .Where(a => InRange(a.Start))
            .Where(a => _waitingOnly ? a.Status is AppointmentStatus.Pending or AppointmentStatus.Rescheduled
                                     : status is null || a.Status == status)
            .Where(a => service is null || string.Equals(a.Service, service, StringComparison.OrdinalIgnoreCase))
            .Where(a => query.Length == 0 || Matches(a, query))
            .OrderBy(a => a.Start)
            .ToList();

        if (_pageIndex >= PageCount) _pageIndex = PageCount - 1;

        BuildRows();
        UpdateFooter();
        UpdateStats();
        UpdateUpcoming();
    }

    private void UpdateRangeText()
    {
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

    private void SetRange(DateTime? weekStart, DateTime? selectedDay = null)
    {
        _rangeStart = weekStart;
        monthCalendar.SelectedDate = selectedDay;
        if (weekStart is not null) monthCalendar.Month = weekStart.Value;
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

    private void UpdateStats()
    {
        var inRange = _all.Where(a => InRange(a.Start)).ToList();
        var today = inRange.Where(a => a.Start.Date == DateTime.Today).ToList();

        static bool IsPending(AppointmentItem a) => a.Status is AppointmentStatus.Pending or AppointmentStatus.Rescheduled;
        static bool IsConfirmed(AppointmentItem a) => a.Status is AppointmentStatus.Confirmed or AppointmentStatus.CheckedIn;
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
        _waitingOnly = false;
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
        => SetRange(WeekStart(date), date);

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
