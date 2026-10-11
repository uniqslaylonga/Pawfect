using MyApp.Controls;

namespace MyApp;

/// <summary>
/// Reports &amp; Financial Auditing screen. Layout lives in ReportsAuditingPage.Designer.cs, so open this file's designer
/// in Visual Studio to move or restyle anything. Nothing is pre-filled: totals show 0 and the audit table and
/// activity list stay empty until audit records are loaded.
/// </summary>
internal partial class ReportsAuditingPage : UserControl
{
    public event EventHandler? DownloadReportRequested;
    public event EventHandler? ExportPdfRequested;
    public event EventHandler? ExportExcelRequested;
    public event EventHandler? ViewAllReportsRequested;
    public event EventHandler? ViewAllActivityRequested;

    /// <summary>Raised with the new tab index: 0 = Sales Audit Logs, 1 = Inventory Movement Report, 2 = Audit Summary.</summary>
    public event EventHandler<int>? ReportTabChanged;

    private readonly TabPill[] _tabs;

    public ReportsAuditingPage()
    {
        InitializeComponent();
        DoubleBuffered = true;

        categoryFilter.SetItems(new[] { "All Categories" }); // filled from real categories later
        staffFilter.SetItems(new[] { "All Staff" });         // filled from real staff later

        _tabs = new[] { salesLogsTab, movementTab, summaryTab };
        for (int i = 0; i < _tabs.Length; i++)
        {
            int index = i;
            _tabs[i].Click += (_, _) => SelectTab(index);
        }

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

    /// <summary>Index of the tab being shown (see <see cref="ReportTabChanged"/>).</summary>
    public int SelectedTab { get; private set; }

    public void SelectTab(int index)
    {
        if (index < 0 || index >= _tabs.Length) return;

        for (int i = 0; i < _tabs.Length; i++)
            _tabs[i].Active = i == index;

        if (index == SelectedTab) return;
        SelectedTab = index;
        ReportTabChanged?.Invoke(this, index);
    }

    // Child controls swallow clicks, so hook the row/tile and everything inside it.
    private static void OnClick(Control root, EventHandler handler)
    {
        root.Click += handler;
        foreach (Control child in root.Controls)
            OnClick(child, handler);
    }
}
