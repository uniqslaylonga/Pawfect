using System.Globalization;
using MyApp.Controls;

namespace MyApp;

/// <summary>
/// Reports &amp; Financial Auditing screen. Layout lives in ReportsAuditingPage.Designer.cs, so open this file's designer
/// in Visual Studio to move or restyle anything. This file only fills it: AdminForm asks <see cref="BuildQuery"/> what to load,
/// reads it from the database (ReportsData) and hands the result to <see cref="ShowData"/>.
/// </summary>
internal partial class ReportsAuditingPage : UserControl
{
    public event EventHandler? DownloadReportRequested;
    public event EventHandler? ExportPdfRequested;
    public event EventHandler? ExportExcelRequested;
    public event EventHandler? ViewAllReportsRequested;
    public event EventHandler? ViewAllActivityRequested;

    /// <summary>Raised whenever the tab, a filter or the date range changed and the table must be loaded again.</summary>
    public event EventHandler? RefreshRequested;

    /// <summary>Raised with the new tab index: 0 = Sales Audit Logs, 1 = Inventory Movement Report, 2 = Audit Summary.</summary>
    public event EventHandler<int>? ReportTabChanged;

    private const int PageSize = 10;
    private static readonly Font RowFont = new("Segoe UI", 9F);
    private static readonly Font ViewFont = new("Segoe UI", 8.5F, FontStyle.Bold);

    private enum RangeKind { All, Day, Week, Month }

    private readonly TabPill[] _tabs;
    private readonly ToolTip _tips = new();
    private readonly HashSet<int> _selected = new();
    private ContextMenuStrip? _rangeMenu;
    private Panel? _activityList;

    private List<AuditRow> _rows = new();
    private AuditTotals _totals = new(0, 0, 0);
    private RangeKind _kind = RangeKind.All;
    private DateTime _anchor = DateTime.Today;
    private int _page = 1;
    private bool _syncing;
    private bool _suppress;

    public ReportsAuditingPage()
    {
        InitializeComponent();
        DoubleBuffered = true;

        categoryFilter.SetItems(new[] { "All Categories" });   // the real categories arrive through SetFilterLists
        staffFilter.SetItems(new[] { "All Staff" });           // the real staff arrive through SetFilterLists
        categoryFilter.SelectedIndexChanged += (_, _) => Reload();
        staffFilter.SelectedIndexChanged += (_, _) => Reload();

        auditDateRange.PreviousClicked += (_, _) => Step(-1);
        auditDateRange.NextClicked += (_, _) => Step(1);
        auditDateRange.TextClicked += (_, _) => ShowRangeMenu();
        UpdateRangeText();

        _tabs = new[] { salesLogsTab, movementTab, summaryTab };
        for (int i = 0; i < _tabs.Length; i++)
        {
            int index = i;
            _tabs[i].Click += (_, _) => SelectTab(index);
        }
        ApplyTabLabels();

        auditPrevPage.Click += (_, _) => GoToPage(_page - 1);
        auditNextPage.Click += (_, _) => GoToPage(_page + 1);
        auditHeaderCheck.CheckedChanged += HeaderCheckChanged;

        // Quick reports jump to the matching tab; the printable report downloads.
        OnClick(salesReportRow, (_, _) => SelectTab(0));
        OnClick(movementReportRow, (_, _) => SelectTab(1));
        OnClick(financialReportRow, (_, _) => SelectTab(2));
        OnClick(printableReportRow, (_, _) => DownloadReportRequested?.Invoke(this, EventArgs.Empty));

        downloadReportButton.Click += (_, _) => DownloadReportRequested?.Invoke(this, EventArgs.Empty);
        OnClick(exportPdfTile, (_, _) => ExportPdfRequested?.Invoke(this, EventArgs.Empty));
        OnClick(exportExcelTile, (_, _) => ExportExcelRequested?.Invoke(this, EventArgs.Empty));
        quickReportsLink.Click += (_, _) => ViewAllReportsRequested?.Invoke(this, EventArgs.Empty);
        recentActivityLink.Click += (_, _) => ViewAllActivityRequested?.Invoke(this, EventArgs.Empty);
    }

    // ------------------------------------------------------------------ tabs

    /// <summary>Index of the tab being shown (see <see cref="ReportTabChanged"/>).</summary>
    public int SelectedTab { get; private set; }

    public AuditTab CurrentTab => (AuditTab)SelectedTab;

    public void SelectTab(int index)
    {
        if (index < 0 || index >= _tabs.Length) return;

        for (int i = 0; i < _tabs.Length; i++)
            _tabs[i].Active = i == index;

        if (index == SelectedTab) return;
        SelectedTab = index;
        ApplyTabLabels();
        ReportTabChanged?.Invoke(this, index);
        Reload();
    }

    /// <summary>Column titles and the empty-state hint follow the tab.</summary>
    private void ApplyTabLabels()
    {
        var h = ReportsData.Headers(CurrentTab);
        auditColDateTime.Text = h[0];
        auditColType.Text = h[1];
        auditColProduct.Text = h[2];
        auditColQuantity.Text = h[3];
        auditColAmount.Text = h[4];
        auditColReference.Text = h[5];
        auditColStatus.Text = h[6];
        auditEmpty.Hint = ReportsData.EmptyHint(CurrentTab);
    }

    // ------------------------------------------------------------------ filters

    /// <summary>Fills the Category and Staff drop-downs from the database. The current choice is kept if it still exists.</summary>
    public void SetFilterLists(IEnumerable<string> categories, IEnumerable<string> staff)
    {
        categoryFilter.SetItems(new[] { "All Categories" }.Concat(categories));
        staffFilter.SetItems(new[] { "All Staff" }.Concat(staff));
    }

    /// <summary>Back to everything: all categories, all staff, all dates.</summary>
    public void ResetFilters()
    {
        _suppress = true;
        categoryFilter.SelectedIndex = 0;
        staffFilter.SelectedIndex = 0;
        _suppress = false;

        _kind = RangeKind.All;
        _anchor = DateTime.Today;
        UpdateRangeText();
        Reload();
    }

    private void Reload()
    {
        if (_suppress) return;
        _page = 1;
        RefreshRequested?.Invoke(this, EventArgs.Empty);
    }

    // ---- date range

    private static (DateTime Start, DateTime End) Bounds(RangeKind kind, DateTime anchor)
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
                var (start, end) = Bounds(RangeKind.Week, _anchor);
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
        auditDateRange.Text = text;
        auditPeriodValue.Text = text;
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
        if (direction > 0 && Bounds(_kind, next).Start > DateTime.Today)
        {
            // Nothing to show in the future: stay on the current period.
            if (wasAll)
            {
                UpdateRangeText();
                Reload();
            }
            return;
        }

        _anchor = next;
        UpdateRangeText();
        Reload();
    }

    private void ShowRangeMenu()
    {
        _rangeMenu?.Dispose();
        _rangeMenu = PawMenu.Create(auditDateRange.Width);

        void Add(string text, RangeKind kind, DateTime anchor, bool isChecked)
        {
            var item = new PawMenuItem(MenuKind.Action, text) { Checked = isChecked, MenuWidth = auditDateRange.Width - 12 };
            item.Click += (_, _) =>
            {
                _kind = kind;
                _anchor = anchor;
                UpdateRangeText();
                Reload();
            };
            _rangeMenu.Items.Add(item);
        }

        var today = DateTime.Today;
        var lastMonth = today.AddMonths(-1);
        Add("All dates", RangeKind.All, today, _kind == RangeKind.All);
        Add("Today", RangeKind.Day, today, _kind == RangeKind.Day && _anchor.Date == today);
        Add("This week", RangeKind.Week, today, _kind == RangeKind.Week && Bounds(RangeKind.Week, _anchor).Start == Bounds(RangeKind.Week, today).Start);
        Add("This month", RangeKind.Month, today, _kind == RangeKind.Month && _anchor.Year == today.Year && _anchor.Month == today.Month);
        Add("Last month", RangeKind.Month, lastMonth, _kind == RangeKind.Month && _anchor.Year == lastMonth.Year && _anchor.Month == lastMonth.Month);
        _rangeMenu.Show(auditDateRange, new Point(0, auditDateRange.Height + 2));
    }

    /// <summary>What AdminForm should load: the current filter, and the two periods the "vs." lines compare.</summary>
    public AuditQuery BuildQuery()
    {
        string? category = categoryFilter.SelectedIndex > 0 ? categoryFilter.SelectedItem : null;
        string? staff = staffFilter.SelectedIndex > 0 ? staffFilter.SelectedItem : null;

        AuditFilter Make(DateTime? from, DateTime? to) => new(from, to, category, staff);

        if (_kind == RangeKind.All)
        {
            // Everything is shown, so the "vs." line compares this month with last month.
            var thisMonth = Bounds(RangeKind.Month, DateTime.Today);
            var lastMonth = Bounds(RangeKind.Month, DateTime.Today.AddMonths(-1));
            return new AuditQuery(Make(null, null), Make(thisMonth.Start, thisMonth.End), Make(lastMonth.Start, lastMonth.End),
                CurrentTab, "this month vs. last month");
        }

        var now = Bounds(_kind, _anchor);
        var before = Bounds(_kind, Shift(_kind, _anchor, -1));
        string label = _kind switch
        {
            RangeKind.Day => "vs. previous day",
            RangeKind.Week => "vs. previous week",
            _ => "vs. previous month",
        };
        var current = Make(now.Start, now.End);
        return new AuditQuery(current, current, Make(before.Start, before.End), CurrentTab, label);
    }

    // ------------------------------------------------------------------ showing data

    /// <summary>Fills the cards and the table. Rows are cut into pages of ten.</summary>
    public void ShowData(AuditReport report, string compareLabel)
    {
        var culture = CultureInfo.CurrentCulture;
        _totals = report.Current;
        _rows = report.Rows;
        _selected.Clear();
        _page = 1;

        auditSalesValue.Text = Money.Symbol + report.Current.Sales.ToString("N2", culture);
        auditMovementValue.Text = report.Current.Units == 1 ? "1 item" : $"{report.Current.Units.ToString("N0", culture)} items";
        auditDiscrepanciesValue.Text = report.Current.Discrepancies.ToString("N0", culture);

        auditSalesDelta.Text = PercentDelta(report.CompareNow.Sales, report.ComparePrev.Sales, compareLabel);
        auditMovementDelta.Text = CountDelta(report.CompareNow.Units, report.ComparePrev.Units, compareLabel);
        auditDiscrepanciesDelta.Text = CountDelta(report.CompareNow.Discrepancies, report.ComparePrev.Discrepancies, compareLabel);

        UpdateRangeText();
        RenderPage();
    }

    private static string PercentDelta(decimal now, decimal before, string label)
    {
        if (now == 0 && before == 0) return "— " + label;
        if (now == before) return "No change " + label;
        if (before == 0) return $"+{Money.Format(now)} {label}";

        decimal pct = Math.Round((now - before) / before * 100m, 0);
        return $"{(pct >= 0 ? "+" : "−")}{Math.Abs(pct).ToString("N0", CultureInfo.CurrentCulture)}% {label}";
    }

    private static string CountDelta(int now, int before, string label)
    {
        int diff = now - before;
        return diff == 0
            ? "No change " + label
            : $"{(diff > 0 ? "+" : "−")}{Math.Abs(diff).ToString("N0", CultureInfo.CurrentCulture)} {label}";
    }

    private void RenderPage()
    {
        int pages = Math.Max(1, (int)Math.Ceiling(_rows.Count / (double)PageSize));
        _page = Math.Clamp(_page, 1, pages);
        int first = (_page - 1) * PageSize;
        var shown = _rows.Skip(first).Take(PageSize).ToList();

        auditRows.SuspendLayout();
        _tips.RemoveAll();

        foreach (var old in auditRows.Controls.Cast<Control>().Where(c => c != auditEmpty).ToList())
        {
            auditRows.Controls.Remove(old);
            old.Dispose();
        }

        auditEmpty.Visible = shown.Count == 0;

        // Dock = Top stacks the last-added control on top, so add in reverse.
        for (int i = shown.Count - 1; i >= 0; i--)
            auditRows.Controls.Add(BuildRow(shown[i], first + i));

        auditRows.AutoScrollPosition = Point.Empty;
        auditRows.ResumeLayout(true);

        string noun = _rows.Count == 1 ? "audit log" : "audit logs";
        auditShowing.Text = _rows.Count == 0
            ? "Showing 0 audit logs"
            : $"Showing {first + 1} to {first + shown.Count} of {_rows.Count} {noun}";

        auditPageChip.Text = _page.ToString(CultureInfo.CurrentCulture);
        auditPrevPage.Enabled = _page > 1;
        auditNextPage.Enabled = _page < pages;
        auditPrevPage.Invalidate();
        auditNextPage.Invalidate();

        SyncHeaderCheck();
    }

    private void GoToPage(int page)
    {
        int pages = Math.Max(1, (int)Math.Ceiling(_rows.Count / (double)PageSize));
        if (page < 1 || page > pages || page == _page) return;
        _page = page;
        RenderPage();
    }

    // ---- rows

    private sealed class AuditRowPanel : Panel
    {
        public CheckMark? Check;

        public AuditRowPanel()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
            Dock = DockStyle.Top;
            Height = 44;
            Padding = new Padding(0, 0, 0, 1);   // the 1px strip left at the bottom is the divider line
            BackColor = Color.White;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.Clear(BackColor);
            using var pen = new Pen(Theme.Border);
            e.Graphics.DrawLine(pen, 0, Height - 1, Width, Height - 1);
        }
    }

    /// <summary>The coloured status badge of a row.</summary>
    private sealed class PillCell : Control
    {
        private readonly string _text;
        private readonly AuditBadge _badge;

        public PillCell(string text, AuditBadge badge)
        {
            _text = text;
            _badge = badge;
            SetStyle(ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
            BackColor = Color.White;
            Dock = DockStyle.Fill;
            Margin = Padding.Empty;
            TabStop = false;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.Clear(BackColor);
            Pills.DrawAt(e.Graphics, _text, ReportsData.BadgeFore(_badge), ReportsData.BadgeFill(_badge),
                ReportsData.BadgeBorder(_badge), 8, Height / 2, Math.Max(20, Width - 12));
        }
    }

    private Control BuildRow(AuditRow r, int index)
    {
        var grid = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.White,
            Margin = Padding.Empty,
            ColumnCount = auditColumnsGrid.ColumnCount,
            RowCount = 1,
        };
        foreach (ColumnStyle s in auditColumnsGrid.ColumnStyles)
            grid.ColumnStyles.Add(new ColumnStyle(s.SizeType, s.Width));
        grid.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

        var check = new CheckMark { Anchor = AnchorStyles.None, BackColor = Color.White, Checked = _selected.Contains(index) };
        check.CheckedChanged += (_, _) =>
        {
            if (check.Checked) _selected.Add(index); else _selected.Remove(index);
            if (!_syncing) SyncHeaderCheck();
        };
        grid.Controls.Add(check, 0, 0);

        void Put(int column, string text, Color color)
        {
            grid.Controls.Add(new Label
            {
                Text = text,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(8, 0, 0, 0),
                Margin = Padding.Empty,
                AutoEllipsis = true,
                UseMnemonic = false,
                BackColor = Color.White,
                ForeColor = color,
                Font = RowFont,
            }, column, 0);
        }

        var dark = Color.FromArgb(31, 42, 60);
        string when = r.When.TimeOfDay == TimeSpan.Zero
            ? r.When.ToString("MMM d, yyyy", CultureInfo.CurrentCulture)
            : r.When.ToString("MMM d, yyyy h:mm tt", CultureInfo.CurrentCulture);

        Put(1, when, dark);
        Put(2, r.Type, dark);
        Put(3, r.Item, dark);
        Put(4, r.Quantity, dark);
        Put(5, r.Amount, r.AmountIsNegative ? Theme.Danger : dark);
        Put(6, r.Reference, Theme.TextSoft);

        var pill = new PillCell(r.Status, r.Badge);
        grid.Controls.Add(pill, 7, 0);
        if (r.Note.Length > 0) _tips.SetToolTip(pill, r.Note);

        if (r.TransactionId is not null)
        {
            var view = new Label
            {
                Text = "View",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(8, 0, 0, 0),
                Margin = Padding.Empty,
                UseMnemonic = false,
                BackColor = Color.White,
                ForeColor = Theme.Primary,
                Font = ViewFont,
                Cursor = Cursors.Hand,
            };
            view.Click += (_, _) => ShowSale(r);
            grid.Controls.Add(view, 8, 0);
        }

        var row = new AuditRowPanel { Check = check };
        row.Controls.Add(grid);
        return row;
    }

    private void ShowSale(AuditRow r)
    {
        if (r.TransactionId is not int id) return;
        var owner = FindForm();

        try
        {
            if (r.Note.Length > 0)
                Msg.Show(owner, "Needs a second look\n\n" + r.Note, "Audit finding", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            var receipt = EmployeeData.GetReceipt(id);
            if (receipt is null)
            {
                Msg.Show(owner, "That receipt no longer exists.", "Pawfect", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using var form = new ReceiptForm(receipt);
            form.ShowDialog(owner);
        }
        catch (Microsoft.Data.SqlClient.SqlException ex)
        {
            Msg.Show(owner, "Couldn't open the receipt.\n\n" + ex.Message, "Pawfect", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    // ---- selection

    private void HeaderCheckChanged(object? sender, EventArgs e)
    {
        if (_syncing) return;
        _syncing = true;
        foreach (var row in auditRows.Controls.OfType<AuditRowPanel>())
            if (row.Check is { } box) box.Checked = auditHeaderCheck.Checked;
        _syncing = false;
    }

    /// <summary>Ticks the header box only when every row on this page is ticked.</summary>
    private void SyncHeaderCheck()
    {
        var rows = auditRows.Controls.OfType<AuditRowPanel>().ToList();
        bool all = rows.Count > 0 && rows.All(r => r.Check?.Checked == true);
        if (auditHeaderCheck.Checked == all) return;

        _syncing = true;
        auditHeaderCheck.Checked = all;
        _syncing = false;
    }

    // ------------------------------------------------------------------ export

    /// <summary>Totals of the whole period shown on the cards (the exports print these above the table).</summary>
    public AuditTotals Totals => _totals;

    /// <summary>The period as shown in the date box ("All dates", "October 2026", ...).</summary>
    public string PeriodText => RangeText();

    public int TotalRows => _rows.Count;

    /// <summary>True when the person ticked some rows: the export then holds only those.</summary>
    public bool HasSelection => _selected.Any(i => i >= 0 && i < _rows.Count);

    /// <summary>The ticked rows, or every row of the current filter when nothing is ticked.</summary>
    public IReadOnlyList<AuditRow> ExportRows => HasSelection
        ? _selected.Where(i => i >= 0 && i < _rows.Count).OrderBy(i => i).Select(i => _rows[i]).ToList()
        : _rows;

    // ------------------------------------------------------------------ recent activity card

    /// <summary>Fills the "Recent Audit Activity" card (newest first).</summary>
    public void SetRecentActivity(List<AuditLog.Entry> entries)
    {
        if (_activityList is null)
        {
            _activityList = new Panel { Dock = DockStyle.Fill, AutoScroll = true, BackColor = Color.White, Visible = false };
            recentActivityCard.Controls.Add(_activityList);
            _activityList.BringToFront();   // below the card's header, filling what is left
        }

        var culture = CultureInfo.CurrentCulture;
        var rows = entries.Select(e => new DashboardRowView(
            e.Action, e.Details.Length > 0 ? $"{e.Actor} · {e.Details}" : e.Actor,
            e.When.Date == DateTime.Today ? e.When.ToString("h:mm tt", culture) : e.When.ToString("MMM d", culture),
            (e.Action.Contains("Deactivated", StringComparison.OrdinalIgnoreCase) || e.Action.Contains("Removed", StringComparison.OrdinalIgnoreCase)) ? Theme.Danger
            : e.Action.Contains("Created", StringComparison.OrdinalIgnoreCase) ? Theme.Green
            : Theme.Primary)).ToList();

        _activityList.SuspendLayout();
        foreach (var old in _activityList.Controls.OfType<DashboardRowView>().ToList())
        {
            _activityList.Controls.Remove(old);
            old.Dispose();
        }

        // Rows are docked to the top, so add them last-to-first to keep the newest on top.
        for (int i = rows.Count - 1; i >= 0; i--)
        {
            rows[i].Dock = DockStyle.Top;
            _activityList.Controls.Add(rows[i]);
        }
        _activityList.AutoScrollPosition = Point.Empty;
        _activityList.ResumeLayout(true);

        _activityList.Visible = rows.Count > 0;
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
