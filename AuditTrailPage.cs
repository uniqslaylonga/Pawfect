using System.Globalization;
using MyApp.Controls;

namespace MyApp;

/// <summary>
/// Audit Trail / Activity Logs screen. Layout lives in AuditTrailPage.Designer.cs, so open this file's designer
/// in Visual Studio to move or restyle anything. AdminForm hands in the log entries (<see cref="LoadEntries"/>) and the
/// counts for the Pending / Users tiles (<see cref="SetCounts"/>); this page does the rest: date range, user / module /
/// action filters, search, quick filters, paging, the Recent Activity list, the Audit Summary and what the export buttons save.
/// </summary>
internal partial class AuditTrailPage : UserControl
{
    private const int PageSize = 12;

    private enum RangeKind { All, Day, Week, Month }

    public event EventHandler? ExportPdfRequested;
    public event EventHandler? ExportExcelRequested;
    public event EventHandler? ViewAllQuickFiltersRequested;
    public event EventHandler? ViewAllActivityRequested;

    /// <summary>Raised with "Today", "This Week", "Critical Actions" or "Login Activity".</summary>
    public event EventHandler<string>? QuickFilterSelected;

    private List<AuditLog.Entry> _allEntries = new();
    private readonly HashSet<int> _selected = new();      // ticked entry ids
    private List<AuditLog.Entry> _shown = new();           // what the current filters let through (all pages)
    private List<(Control Row, CheckMark? Check, int Id)> _rowViews = new();
    private Panel? _recentList;
    private ContextMenuStrip? _rangeMenu;

    private RangeKind _kind = RangeKind.All;
    private DateTime _anchor = DateTime.Today;
    private bool _criticalOnly;
    private int _page;
    private bool _syncing;
    private int _pending, _activeUsers, _inactiveUsers;

    public AuditTrailPage()
    {
        InitializeComponent();
        DoubleBuffered = true;

        userFilter.SetItems(new[] { "All Users" });   // the real people arrive with the entries
        moduleFilter.SetItems(new[] { "All Modules", "Appointments", "Inventory", "Services", "Users", "Rooms", "Financial", "System" });
        actionTypeFilter.SetItems(new[] { "All Action Types", "Created", "Updated", "Approved", "Deleted", "Login", "Exported" });
        exportPdfSub.Text = "(Filtered logs)";
        exportExcelSub.Text = "(Filtered logs)";
        activityColActions.Text = "";

        activitySearch.Box.TextChanged += (_, _) => { _page = 0; ApplyActivityFilter(); };
        userFilter.SelectedIndexChanged += (_, _) => { if (!_syncing) { _page = 0; ApplyActivityFilter(); } };
        moduleFilter.SelectedIndexChanged += (_, _) => { _page = 0; ApplyActivityFilter(); };
        actionTypeFilter.SelectedIndexChanged += (_, _) => { _page = 0; ApplyActivityFilter(); };

        foreach (var range in new[] { activityDateRange, overviewDateRange })
        {
            var field = range;
            field.PreviousClicked += (_, _) => Step(-1);
            field.NextClicked += (_, _) => Step(1);
            field.TextClicked += (_, _) => ShowRangeMenu(field);
        }
        UpdateRangeText();

        activityPrevPage.Click += (_, _) => { _page--; ApplyActivityFilter(); };
        activityNextPage.Click += (_, _) => { _page++; ApplyActivityFilter(); };
        activityHeaderCheck.CheckedChanged += (_, _) => { if (!_syncing) SetPageChecked(activityHeaderCheck.Checked); };

        OnClick(todayRow, (_, _) => { QuickFilterSelected?.Invoke(this, "Today"); ApplyQuickFilter("Today"); });
        OnClick(weekRow, (_, _) => { QuickFilterSelected?.Invoke(this, "This Week"); ApplyQuickFilter("This Week"); });
        OnClick(criticalRow, (_, _) => { QuickFilterSelected?.Invoke(this, "Critical Actions"); ApplyQuickFilter("Critical Actions"); });
        OnClick(loginRow, (_, _) => { QuickFilterSelected?.Invoke(this, "Login Activity"); ApplyQuickFilter("Login Activity"); });

        OnClick(exportPdfTile, (_, _) => ExportPdfRequested?.Invoke(this, EventArgs.Empty));
        OnClick(exportExcelTile, (_, _) => ExportExcelRequested?.Invoke(this, EventArgs.Empty));
        quickFiltersLink.Click += (_, _) => { ViewAllQuickFiltersRequested?.Invoke(this, EventArgs.Empty); ResetFilters(); };
        recentActivityLink.Click += (_, _) => { ViewAllActivityRequested?.Invoke(this, EventArgs.Empty); ResetFilters(); };
    }

    // ------------------------------------------------------------------ data in

    /// <summary>Stores the entries from the database, then shows them through the current filters.</summary>
    public void LoadEntries(List<AuditLog.Entry> entries)
    {
        _allEntries = entries;
        _selected.IntersectWith(entries.Select(e => e.Id));

        // The user drop-down lists everyone who appears in the log; the current choice is kept.
        string? current = userFilter.SelectedItem;
        _syncing = true;
        userFilter.SetItems(new[] { "All Users" }.Concat(entries.Select(e => e.Actor).Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(a => a, StringComparer.CurrentCultureIgnoreCase)));
        if (current is not null)
        {
            int at = userFilter.Items.ToList().FindIndex(i => string.Equals(i, current, StringComparison.OrdinalIgnoreCase));
            if (at >= 0) userFilter.SelectedIndex = at;
        }
        _syncing = false;

        SetRecentActivity(entries.Take(30).ToList());
        ApplyActivityFilter();
    }

    /// <summary>The numbers that don't come from the log: appointment requests waiting, and active / inactive login accounts.</summary>
    public void SetCounts(int pending, int activeUsers, int inactiveUsers)
    {
        _pending = pending;
        _activeUsers = activeUsers;
        _inactiveUsers = inactiveUsers;
        UpdateTiles();
    }

    // ------------------------------------------------------------------ what export saves

    /// <summary>The ticked entries, or - when none are ticked - every entry the filters let through.</summary>
    public List<AuditLog.Entry> ExportRows
        => _selected.Count > 0 ? _shown.Where(e => _selected.Contains(e.Id)).ToList() : _shown.ToList();

    /// <summary>The period shown in the date picker, for export titles.</summary>
    public string PeriodText => RangeText();

    public bool HasSelection => _selected.Count > 0 && _shown.Any(e => _selected.Contains(e.Id));

    // ------------------------------------------------------------------ filters

    private bool InRange(AuditLog.Entry e)
    {
        if (_kind == RangeKind.All) return true;
        var (start, end) = RangeBounds(_kind, _anchor);
        return e.When >= start && e.When < end;
    }

    private IEnumerable<AuditLog.Entry> Filtered()
    {
        string q = activitySearch.Value.Trim();
        string? user = userFilter.SelectedIndex > 0 ? userFilter.SelectedItem : null;
        string? module = moduleFilter.SelectedIndex > 0 ? moduleFilter.SelectedItem : null;
        string? type = actionTypeFilter.SelectedIndex > 0 ? actionTypeFilter.SelectedItem : null;

        return _allEntries.Where(e =>
            InRange(e) &&
            (user is null || string.Equals(e.Actor, user, StringComparison.OrdinalIgnoreCase)) &&
            (module is null || AuditInfo.Module(e.Action) == module) &&
            (type is null || AuditInfo.Type(e.Action) == type) &&
            (!_criticalOnly || AuditInfo.IsCritical(e.Action)) &&
            (q.Length == 0 ||
             e.Actor.Contains(q, StringComparison.OrdinalIgnoreCase) ||
             e.Action.Contains(q, StringComparison.OrdinalIgnoreCase) ||
             e.Details.Contains(q, StringComparison.OrdinalIgnoreCase) ||
             AuditInfo.Module(e.Action).Contains(q, StringComparison.OrdinalIgnoreCase)));
    }

    private void ApplyQuickFilter(string name)
    {
        switch (name)
        {
            case "Today":
                _kind = RangeKind.Day; _anchor = DateTime.Today; _criticalOnly = false;
                break;
            case "This Week":
                _kind = RangeKind.Week; _anchor = DateTime.Today; _criticalOnly = false;
                break;
            case "Critical Actions":
                _criticalOnly = !_criticalOnly;   // click again to switch it off
                break;
            case "Login Activity":
                int login = actionTypeFilter.Items.ToList().IndexOf("Login");
                actionTypeFilter.SelectedIndex = actionTypeFilter.SelectedIndex == login ? 0 : login;
                break;
        }
        _page = 0;
        UpdateRangeText();
        ApplyActivityFilter();
    }

    /// <summary>Clears every filter, then searches for this text (used when a dashboard activity row is clicked).</summary>
    public void ShowOnly(string text)
    {
        ResetFilters();
        activitySearch.Box.Text = text;   // typing into the box applies the filter
    }

    /// <summary>Clears the date range, drop-downs, search and quick filters.</summary>
    public void ResetFilters()
    {
        _kind = RangeKind.All;
        _anchor = DateTime.Today;
        _criticalOnly = false;
        _page = 0;
        _syncing = true;
        userFilter.SelectedIndex = 0;
        moduleFilter.SelectedIndex = 0;
        actionTypeFilter.SelectedIndex = 0;
        _syncing = false;
        activitySearch.Box.Text = "";
        UpdateRangeText();
        ApplyActivityFilter();
    }

    // ------------------------------------------------------------------ date range

    private static (DateTime Start, DateTime End) RangeBounds(RangeKind kind, DateTime anchor)
    {
        switch (kind)
        {
            case RangeKind.Day:
                return (anchor.Date, anchor.Date.AddDays(1));
            case RangeKind.Week:
                int back = ((int)anchor.DayOfWeek + 6) % 7;          // weeks run Monday to Sunday
                var monday = anchor.Date.AddDays(-back);
                return (monday, monday.AddDays(7));
            default:
                var first = new DateTime(anchor.Year, anchor.Month, 1);
                return (first, first.AddMonths(1));
        }
    }

    private static DateTime Shift(RangeKind kind, DateTime anchor, int direction) => kind switch
    {
        RangeKind.Day => anchor.AddDays(direction),
        RangeKind.Week => anchor.AddDays(7 * direction),
        RangeKind.Month => anchor.AddMonths(direction),
        _ => anchor,
    };

    private string RangeText()
    {
        var culture = CultureInfo.CurrentCulture;
        switch (_kind)
        {
            case RangeKind.Day:
                return _anchor.ToString("MMM d, yyyy", culture);
            case RangeKind.Week:
                var (start, end) = RangeBounds(RangeKind.Week, _anchor);
                return start.ToString("MMM d", culture) + " – " + end.AddDays(-1).ToString("MMM d, yyyy", culture);
            case RangeKind.Month:
                return _anchor.ToString("MMMM yyyy", culture);
            default:
                return "All dates";
        }
    }

    private void UpdateRangeText()
    {
        string text = RangeText();
        activityDateRange.Text = text;
        overviewDateRange.Text = text;
    }

    private void Step(int direction)
    {
        bool wasAll = _kind == RangeKind.All;
        if (wasAll)
        {
            _kind = RangeKind.Month;
            _anchor = DateTime.Today;
        }

        var next = Shift(_kind, _anchor, direction);
        if (direction > 0 && RangeBounds(_kind, next).Start > DateTime.Today)
        {
            // Nothing to show in the future: stay on the current period.
            if (wasAll) { UpdateRangeText(); _page = 0; ApplyActivityFilter(); }
            return;
        }

        _anchor = next;
        _page = 0;
        UpdateRangeText();
        ApplyActivityFilter();
    }

    private void ShowRangeMenu(DateRangeField owner)
    {
        _rangeMenu?.Dispose();
        _rangeMenu = PawMenu.Create(owner.Width);

        void Add(string text, RangeKind kind, DateTime anchor, bool isChecked)
        {
            var item = new PawMenuItem(MenuKind.Action, text) { Checked = isChecked, MenuWidth = owner.Width - 12 };
            item.Click += (_, _) =>
            {
                _kind = kind;
                _anchor = anchor;
                _page = 0;
                UpdateRangeText();
                ApplyActivityFilter();
            };
            _rangeMenu.Items.Add(item);
        }

        var today = DateTime.Today;
        var lastMonth = today.AddMonths(-1);
        Add("All dates", RangeKind.All, today, _kind == RangeKind.All);
        Add("Today", RangeKind.Day, today, _kind == RangeKind.Day && _anchor.Date == today);
        Add("This week", RangeKind.Week, today, _kind == RangeKind.Week && RangeBounds(RangeKind.Week, _anchor).Start == RangeBounds(RangeKind.Week, today).Start);
        Add("This month", RangeKind.Month, today, _kind == RangeKind.Month && _anchor.Year == today.Year && _anchor.Month == today.Month);
        Add("Last month", RangeKind.Month, lastMonth, _kind == RangeKind.Month && _anchor.Year == lastMonth.Year && _anchor.Month == lastMonth.Month);
        _rangeMenu.Show(owner, new Point(0, owner.Height + 2));
    }

    // ------------------------------------------------------------------ table

    private void ApplyActivityFilter()
    {
        _shown = Filtered().ToList();

        int pages = Math.Max(1, (_shown.Count + PageSize - 1) / PageSize);
        _page = Math.Clamp(_page, 0, pages - 1);
        int first = _page * PageSize;
        var pageRows = _shown.Skip(first).Take(PageSize).ToList();

        activityRows.SuspendLayout();
        foreach (var old in activityRows.Controls.Cast<Control>().Where(c => c != activityEmpty).ToList())
        {
            activityRows.Controls.Remove(old);
            old.Dispose();
        }
        _rowViews = new();

        activityEmpty.Visible = _shown.Count == 0;
        activityEmpty.Title = _allEntries.Count == 0 ? "No activity yet" : "No matching activity";
        activityEmpty.Hint = _allEntries.Count == 0
            ? "System actions will appear here as they happen."
            : "Try another date range or clear the filters.";

        // Dock = Top stacks the last-added control on top, so add in reverse.
        for (int i = pageRows.Count - 1; i >= 0; i--)
            activityRows.Controls.Add(BuildActivityRow(pageRows[i]));
        activityRows.AutoScrollPosition = Point.Empty;

        activityShowing.Text = _shown.Count == 0
            ? "No activity"
            : $"Showing {first + 1} to {first + pageRows.Count} of {_shown.Count} entries";
        activityPageChip.Text = (_page + 1).ToString(CultureInfo.CurrentCulture);
        activityPrevPage.Enabled = _page > 0;
        activityNextPage.Enabled = _page < pages - 1;
        activityPrevPage.Invalidate();
        activityNextPage.Invalidate();

        activityRows.ResumeLayout(true);
        SyncHeaderCheck();
        UpdateTiles();
    }

    private void UpdateTiles()
    {
        var culture = CultureInfo.CurrentCulture;
        var inRange = _allEntries.Where(InRange).ToList();
        var today = DateTime.Today;

        // Tiles and summary follow the date range only, so the filters below don't make the totals jump around.
        actTotalValue.Text = inRange.Count.ToString(culture);
        var monthStart = new DateTime(today.Year, today.Month, 1);
        int thisMonth = _allEntries.Count(e => e.When >= monthStart);
        int lastMonth = _allEntries.Count(e => e.When >= monthStart.AddMonths(-1) && e.When < monthStart);
        actTotalDelta.Text = Delta(thisMonth, lastMonth, "vs. last month");

        int todayEntries = _allEntries.Count(e => e.When.Date == today);
        int sameDayLastWeek = _allEntries.Count(e => e.When.Date == today.AddDays(-7));
        actTodayValue.Text = todayEntries.ToString(culture);
        actTodayDelta.Text = Delta(todayEntries, sameDayLastWeek, "vs. same day last week");

        actPendingValue.Text = _pending.ToString(culture);
        actPendingDelta.Text = _pending == 0 ? "Nothing waiting" : "Appointment requests waiting";

        actUsersValue.Text = _activeUsers.ToString(culture);
        actUsersDelta.Text = _inactiveUsers == 0 ? "All accounts active" : $"{_inactiveUsers} inactive";

        createdValue.Text = inRange.Count(e => AuditInfo.Type(e.Action) == "Created").ToString(culture);
        updatedValue.Text = inRange.Count(e => AuditInfo.Type(e.Action) == "Updated").ToString(culture);
        approvedValue.Text = inRange.Count(e => AuditInfo.Type(e.Action) == "Approved").ToString(culture);
        deletedValue.Text = inRange.Count(e => AuditInfo.Type(e.Action) == "Deleted").ToString(culture);

        int weekStartBack = ((int)today.DayOfWeek + 6) % 7;
        var weekStart = today.AddDays(-weekStartBack);
        todayCount.Text = todayEntries.ToString(culture);
        weekCount.Text = _allEntries.Count(e => e.When >= weekStart).ToString(culture);
        criticalCount.Text = inRange.Count(e => AuditInfo.IsCritical(e.Action)).ToString(culture);
        loginCount.Text = inRange.Count(e => AuditInfo.Type(e.Action) == "Login").ToString(culture);
    }

    private static string Delta(int now, int before, string suffix)
    {
        int diff = now - before;
        return diff == 0 ? $"No change {suffix}" : diff > 0 ? $"+{diff} {suffix}" : $"−{-diff} {suffix}";
    }

    private Control BuildActivityRow(AuditLog.Entry e)
    {
        var grid = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.White,
            Margin = Padding.Empty,
            ColumnCount = activityColumnsGrid.ColumnCount,
            RowCount = 1,
        };
        foreach (ColumnStyle s in activityColumnsGrid.ColumnStyles)
            grid.ColumnStyles.Add(new ColumnStyle(s.SizeType, s.Width));
        grid.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

        void Put(Control header, string text, Color? color = null, bool bold = false)
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
                Font = new Font("Segoe UI", 9F, bold ? FontStyle.Bold : FontStyle.Regular),
                Padding = new Padding(8, 0, 0, 0),
            }, activityColumnsGrid.GetColumn(header), 0);
        }

        var check = new CheckMark { Anchor = AnchorStyles.None, BackColor = Color.White, Checked = _selected.Contains(e.Id) };
        check.CheckedChanged += (_, _) =>
        {
            if (check.Checked) _selected.Add(e.Id); else _selected.Remove(e.Id);
            if (!_syncing) SyncHeaderCheck();
        };
        grid.Controls.Add(check, 0, 0);

        bool failed = AuditInfo.IsFailed(e.Action);
        Put(activityColDateTime, e.When.ToString("MMM d, yyyy h:mm tt", CultureInfo.CurrentCulture));
        Put(activityColUser, e.Actor, bold: true);
        Put(activityColAction, e.Action);
        Put(activityColModule, AuditInfo.Module(e.Action));
        Put(activityColDescription, e.Details);
        string reference = AuditInfo.Reference(e.Details);
        Put(activityColReference, reference, reference == "—" ? Color.FromArgb(122, 136, 156) : null);
        Put(activityColStatus, AuditInfo.Status(e.Action), failed ? Theme.Danger : Color.FromArgb(46, 158, 91), bold: true);

        var view = new Label
        {
            Text = "View",
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleLeft,
            BackColor = Color.White,
            ForeColor = Color.FromArgb(58, 123, 213),
            Font = new Font("Segoe UI", 9F, FontStyle.Bold),
            Cursor = Cursors.Hand,
            UseMnemonic = false,
        };
        view.Click += (_, _) => Msg.Show(FindForm(),
            $"{e.Action}\n\nWho: {e.Actor}\nWhen: {e.When.ToString("dddd, MMM d, yyyy h:mm:ss tt", CultureInfo.CurrentCulture)}\nModule: {AuditInfo.Module(e.Action)}\nStatus: {AuditInfo.Status(e.Action)}\n\n{(e.Details.Length > 0 ? e.Details : "No further details.")}",
            "Pawfect", MessageBoxButtons.OK, MessageBoxIcon.Information);
        grid.Controls.Add(view, activityColumnsGrid.GetColumn(activityColActions), 0);

        var row = new Panel { Dock = DockStyle.Top, Height = 44, BackColor = Color.White };
        row.Controls.Add(grid);
        _rowViews.Add((row, check, e.Id));
        return row;
    }

    private void SetPageChecked(bool on)
    {
        _syncing = true;
        foreach (var (_, check, id) in _rowViews)
        {
            if (check is null) continue;
            check.Checked = on;
            if (on) _selected.Add(id); else _selected.Remove(id);
        }
        _syncing = false;
    }

    private void SyncHeaderCheck()
    {
        bool all = _rowViews.Count > 0 && _rowViews.All(r => r.Check is { Checked: true });
        _syncing = true;
        activityHeaderCheck.Checked = all;
        _syncing = false;
    }

    // ------------------------------------------------------------------ recent activity (right column)

    private void SetRecentActivity(List<AuditLog.Entry> entries)
    {
        if (_recentList is null)
        {
            _recentList = new Panel { Dock = DockStyle.Fill, AutoScroll = true, BackColor = Color.White, Visible = false };
            recentActivityCard.Controls.Add(_recentList);
            _recentList.BringToFront();   // below the card's header, filling what is left
            _recentList.MouseEnter += (_, _) => _recentList.Focus();
        }

        var culture = CultureInfo.CurrentCulture;
        var rows = entries.Select(e =>
        {
            var row = new DashboardRowView(
                e.Action, e.Details.Length > 0 ? $"{e.Actor} · {e.Details}" : e.Actor,
                e.When.Date == DateTime.Today ? e.When.ToString("h:mm tt", culture) : e.When.ToString("MMM d", culture),
                AuditInfo.IsCritical(e.Action) ? Theme.Danger
                : AuditInfo.Type(e.Action) == "Created" ? Theme.Green
                : Theme.Primary);
            row.Clickable = true;
            // Narrows the activity table on the left to just this entry (the row only has room for part of its text).
            row.Click += (_, _) => ShowOnly(e.Details.Length > 0 ? e.Details : e.Action);
            row.MouseEnter += (_, _) => _recentList?.Focus();   // so the mouse wheel scrolls the list while the pointer is over a row
            return row;
        }).ToList();

        _recentList.SuspendLayout();
        foreach (var old in _recentList.Controls.OfType<DashboardRowView>().ToList())
        {
            _recentList.Controls.Remove(old);
            old.Dispose();
        }
        for (int i = rows.Count - 1; i >= 0; i--)
        {
            rows[i].Dock = DockStyle.Top;
            _recentList.Controls.Add(rows[i]);
        }
        _recentList.AutoScrollPosition = Point.Empty;
        _recentList.ResumeLayout(true);

        _recentList.Visible = rows.Count > 0;
        recentActivityEmpty.Visible = rows.Count == 0;
    }

    // Child controls swallow clicks, so hook the row/tile and everything inside it.
    private static void OnClick(Control root, EventHandler handler)
    {
        root.Click += handler;
        foreach (Control child in root.Controls)
            OnClick(child, handler);
    }
}
