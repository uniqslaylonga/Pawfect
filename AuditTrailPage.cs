namespace MyApp;

/// <summary>
/// Audit Trail / Activity Logs screen. Layout lives in AuditTrailPage.Designer.cs, so open this file's designer
/// in Visual Studio to move or restyle anything. Nothing is pre-filled: counts show 0 and the log table and
/// recent-activity list stay empty until activity records are loaded.
/// </summary>
internal partial class AuditTrailPage : UserControl
{
    public event EventHandler? ExportPdfRequested;
    public event EventHandler? ExportExcelRequested;
    public event EventHandler? ViewAllQuickFiltersRequested;
    public event EventHandler? ViewAllActivityRequested;

    /// <summary>Raised with "Today", "This Week", "Critical Actions" or "Login Activity".</summary>
    public event EventHandler<string>? QuickFilterSelected;

    public AuditTrailPage()
    {
        InitializeComponent();
        DoubleBuffered = true;

        activitySearch.Box.TextChanged += (_, _) => ApplyActivityFilter();
        userFilter.SetItems(new[] { "All Users" });   // filled from real accounts later
        moduleFilter.SetItems(new[] { "All Modules", "Appointments", "Inventory", "Services", "Records", "System", "Financial" });
        actionTypeFilter.SetItems(new[] { "All Action Types", "Created", "Updated", "Approved", "Deleted", "Login" });

        OnClick(todayRow, (_, _) => QuickFilterSelected?.Invoke(this, "Today"));
        OnClick(weekRow, (_, _) => QuickFilterSelected?.Invoke(this, "This Week"));
        OnClick(criticalRow, (_, _) => QuickFilterSelected?.Invoke(this, "Critical Actions"));
        OnClick(loginRow, (_, _) => QuickFilterSelected?.Invoke(this, "Login Activity"));

        OnClick(exportPdfTile, (_, _) => ExportPdfRequested?.Invoke(this, EventArgs.Empty));
        OnClick(exportExcelTile, (_, _) => ExportExcelRequested?.Invoke(this, EventArgs.Empty));
        quickFiltersLink.Click += (_, _) => ViewAllQuickFiltersRequested?.Invoke(this, EventArgs.Empty);
        recentActivityLink.Click += (_, _) => ViewAllActivityRequested?.Invoke(this, EventArgs.Empty);
    }

    private List<AuditLog.Entry> _allEntries = new();

    /// <summary>Stores the entries from the database, then shows them through the current search.</summary>
    public void LoadEntries(List<AuditLog.Entry> entries)
    {
        _allEntries = entries;
        ApplyActivityFilter();
    }

    private void ApplyActivityFilter()
    {
        string q = activitySearch.Value.Trim();
        var shown = _allEntries.Where(e =>
            q.Length == 0 ||
            e.Actor.Contains(q, StringComparison.OrdinalIgnoreCase) ||
            e.Action.Contains(q, StringComparison.OrdinalIgnoreCase) ||
            e.Details.Contains(q, StringComparison.OrdinalIgnoreCase)).ToList();

        activityRows.SuspendLayout();

        foreach (var old in activityRows.Controls.Cast<Control>().Where(c => c != activityEmpty).ToList())
        {
            activityRows.Controls.Remove(old);
            old.Dispose();
        }

        activityEmpty.Visible = shown.Count == 0;

        // Dock = Top stacks the last-added control on top, so add in reverse.
        for (int i = shown.Count - 1; i >= 0; i--)
            activityRows.Controls.Add(BuildActivityRow(shown[i]));

        // Tiles and summary always show totals for everything, not just the search result.
        actTotalValue.Text = _allEntries.Count.ToString();
        actTodayValue.Text = _allEntries.Count(e => e.When.Date == DateTime.Today).ToString();
        actUsersValue.Text = _allEntries.Select(e => e.Actor).Distinct().Count().ToString();
        createdValue.Text = _allEntries.Count(e => e.Action.Contains("Created")).ToString();
        updatedValue.Text = _allEntries.Count(e => e.Action.Contains("Edited") || e.Action.Contains("Activated")).ToString();
        deletedValue.Text = _allEntries.Count(e => e.Action.Contains("Deactivated")).ToString();

        activityShowing.Text = shown.Count == 0
            ? "No activity"
            : $"Showing 1 to {shown.Count} of {_allEntries.Count} entries";

        activityRows.ResumeLayout(true);
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
            }, activityColumnsGrid.GetColumn(header), 0);
        }

        Put(activityColDateTime, e.When.ToString("MMM d, yyyy h:mm tt"));
        Put(activityColUser, e.Actor);
        Put(activityColAction, e.Action);
        Put(activityColModule, "Users");
        Put(activityColDescription, e.Details);
        Put(activityColReference, "—", Color.FromArgb(122, 136, 156));
        Put(activityColStatus, "Success", Color.FromArgb(46, 158, 91));

        var row = new Panel { Dock = DockStyle.Top, Height = 44, BackColor = Color.White };
        row.Controls.Add(grid);
        return row;
    }

    // Child controls swallow clicks, so hook the row/tile and everything inside it.
    private static void OnClick(Control root, EventHandler handler)
    {
        root.Click += handler;
        foreach (Control child in root.Controls)
            OnClick(child, handler);
    }
}
