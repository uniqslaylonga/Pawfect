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

    // Child controls swallow clicks, so hook the row/tile and everything inside it.
    private static void OnClick(Control root, EventHandler handler)
    {
        root.Click += handler;
        foreach (Control child in root.Controls)
            OnClick(child, handler);
    }
}
