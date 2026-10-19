using MyApp.Controls;

namespace MyApp;

/// <summary>
/// Reports &amp; Financial Auditing screen. Layout lives in ReportsAuditingPage.Designer.cs, so open this file's designer
/// in Visual Studio to move or restyle anything. Nothing is pre-filled: totals show 0 and the audit table and
/// activity list stay empty until audit records are loaded.
/// </summary>
internal partial class ReportsAuditingPage : UserControl
{
    private string[] _headers = Array.Empty<string>();
    private List<string[]> _rows = new();

    public (string[] Headers, List<string[]> Rows) CurrentTable => (_headers, _rows);
    private List<ReportRepository.MovementRow> _movements = new();
    private List<AuditLog.Entry> _audit = new();

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
        ReportTabChanged += (_, _) => Render();
    }

    public void LoadReports(List<ReportRepository.MovementRow> movements, List<AuditLog.Entry> audit)
    {
        _movements = movements;
        _audit = audit;
        Render();
    }

    private void Render()
    {
        var rows = new List<string[]>();   // Date/Time, Type, Product, Quantity, Amount, Reference, Status

        switch (SelectedTab)
        {
            case 1:
                SetHeaders("Date / Time", "Type", "Product", "Quantity", "Amount", "Batch", "Status");
                auditEmpty.Title = "No stock movements yet";
                auditEmpty.Hint = "Stock in and stock out entries will appear here.";
                foreach (var m in _movements)
                    rows.Add(new[] { m.When.ToString("MMM d, yyyy h:mm tt"), "Stock " + m.Type, m.Product,
                                 $"{m.Quantity} {m.Unit}", "—", m.BatchNo ?? "—", m.Actor });
                break;

            case 2:
                SetHeaders("Date / Time", "Action", "Details", "Quantity", "Amount", "Done by", "Status");
                auditEmpty.Title = "No audit entries yet";
                auditEmpty.Hint = "Recorded actions will appear here.";
                foreach (var a in _audit)
                    rows.Add(new[] { a.When.ToString("MMM d, yyyy h:mm tt"), a.Action, a.Details,
                                 "—", "—", a.Actor, "Logged" });
                break;

            default:
                SetHeaders("Date / Time", "Type", "Product", "Quantity", "Amount", "Reference", "Status");
                auditEmpty.Title = "No sales recorded yet";
                auditEmpty.Hint = "Sales will appear here once billing is set up.";

                break;

        }

        auditRows.SuspendLayout();

        foreach (var old in auditRows.Controls.Cast<Control>().Where(c => c != auditEmpty).ToList())
        {
            auditRows.Controls.Remove(old);
            old.Dispose();
        }

        auditEmpty.Visible = rows.Count == 0;
        _rows = rows;

        for (int i = rows.Count - 1; i >= 0; i--)
            auditRows.Controls.Add(BuildReportRow(rows[i]));

        auditMovementValue.Text = _movements.Count.ToString();
        auditPeriodValue.Text = "All time";
        auditShowing.Text = rows.Count == 0 ? "No records" : $"Showing 1 to {rows.Count} records";

        auditRows.ResumeLayout(true);
    }

    private void SetHeaders(string c1, string c2, string c3, string c4, string c5, string c6, string c7)
    {
        auditColDateTime.Text = c1;
        auditColType.Text = c2;
        auditColProduct.Text = c3;
        auditColQuantity.Text = c4;
        auditColAmount.Text = c5;
        auditColReference.Text = c6;
        auditColStatus.Text = c7;
        _headers = new[] { c1, c2, c3, c4, c5, c6, c7 };
    }

    private Control BuildReportRow(string[] cells)
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

        var headers = new Control[]
        {
        auditColDateTime, auditColType, auditColProduct, auditColQuantity,
        auditColAmount, auditColReference, auditColStatus,
        };
        for (int i = 0; i < headers.Length; i++)
        {
            grid.Controls.Add(new Label
            {
                Text = cells[i],
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                AutoEllipsis = true,
                UseMnemonic = false,
                BackColor = Color.White,
                ForeColor = Color.FromArgb(31, 42, 60),
                Font = new Font("Segoe UI", 9F),
            }, auditColumnsGrid.GetColumn(headers[i]), 0);
        }

        var row = new Panel { Dock = DockStyle.Top, Height = 44, BackColor = Color.White };
        row.Controls.Add(grid);
        return row;
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
