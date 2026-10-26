using Microsoft.Data.SqlClient;
using MyApp.Controls;

namespace MyApp;

/// <summary>Service Catalog Management > Manage Pricing: edit every service's price in one place, or raise / lower them all by a percentage.</summary>
internal sealed class ManagePricingDialog : DialogBase
{
    private sealed class PriceRow
    {
        public required ServiceRepository.ServiceRow Service;
        public required Panel Row;
        public required NumericUpDown Box;
    }

    private readonly List<PriceRow> _rows = new();
    private readonly TextBox searchBox = new() { PlaceholderText = "Search services" };
    private readonly NumericUpDown percentBox = new() { DecimalPlaces = 1, Minimum = -90, Maximum = 500, Increment = 5 };
    private readonly PawButton applyPercent = new() { Text = "Apply to shown services", Kind = PawButtonKind.Outline, Size = new Size(190, 34) };
    private readonly FlowLayoutPanel list = new()
    {
        Dock = DockStyle.Fill,
        FlowDirection = FlowDirection.TopDown,
        WrapContents = false,
        AutoScroll = true,
        BackColor = Color.White,
        Padding = new Padding(24, 4, 8, 8),
    };

    /// <summary>How many prices were changed, so AdminForm can write it to the audit log.</summary>
    public int ChangedCount { get; private set; }

    public ManagePricingDialog()
        : base("Manage Pricing", "Save Prices", "\uE8C7", "Edit prices, then save them all at once")
    {
        ClientSize = new Size(620, 600);

        List<ServiceRepository.ServiceRow> services;
        try
        {
            services = ServiceRepository.GetAll();
        }
        catch (SqlException ex)
        {
            services = new();
            ShowError("Could not load services: " + ex.Message);
        }

        AddRow("Search", searchBox);
        AddRow("Change all by (%)", percentBox);
        AddRow("", applyPercent);

        foreach (var s in services) _rows.Add(BuildRow(s));
        foreach (var r in _rows) list.Controls.Add(r.Row);

        Controls.Add(list);
        list.BringToFront();   // the list takes whatever space the header, fields and button bar leave

        list.SizeChanged += (_, _) => FitRows();
        searchBox.TextChanged += (_, _) => ApplyFilter();
        applyPercent.Click += (_, _) => ApplyPercent();
        Shown += (_, _) => FitRows();

        if (services.Count == 0) SaveButton.Enabled = false;
    }

    private PriceRow BuildRow(ServiceRepository.ServiceRow s)
    {
        var row = new Panel { Height = 46, BackColor = Color.White, Margin = new Padding(0) };
        row.Paint += (_, e) =>
        {
            using var pen = new Pen(Theme.Border);
            e.Graphics.DrawLine(pen, 0, row.Height - 1, row.Width, row.Height - 1);
        };

        string band = ServicePricing.BandText(s.Species, s.MinKg, s.MaxKg);
        string sub = string.Join("  \u00b7  ", new[] { s.Category, s.Size == "All" ? "" : s.Size, band, s.Active ? "" : "Inactive" }
            .Where(x => x.Length > 0));

        var name = Ui.Text(s.Name, 9.5f, Theme.Text, Color.White, FontStyle.Bold);
        name.SetBounds(0, 5, 380, 20);
        var detail = Ui.Text(sub, 8f, Theme.Muted, Color.White);
        detail.SetBounds(0, 25, 380, 16);

        var box = new NumericUpDown { DecimalPlaces = 2, Minimum = 0, Maximum = 100000, Increment = 50, Value = Math.Min(100000, s.Price) };
        var frame = FieldFrame.ForEditor(box);
        frame.Dock = DockStyle.None;
        frame.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        frame.SetBounds(row.Width - 150, 5, 140, 36);

        row.Controls.Add(name);
        row.Controls.Add(detail);
        row.Controls.Add(frame);
        row.Tag = frame;

        return new PriceRow { Service = s, Row = row, Box = box };
    }

    private void FitRows()
    {
        int width = Math.Max(300, list.ClientSize.Width - list.Padding.Horizontal);
        foreach (var r in _rows)
        {
            r.Row.Width = width;
            if (r.Row.Tag is Control frame) frame.Left = width - frame.Width - 4;
        }
    }

    private void ApplyFilter()
    {
        string q = searchBox.Text.Trim();
        list.SuspendLayout();
        foreach (var r in _rows)
            r.Row.Visible = q.Length == 0
                || r.Service.Name.Contains(q, StringComparison.OrdinalIgnoreCase)
                || r.Service.Category.Contains(q, StringComparison.OrdinalIgnoreCase);
        list.ResumeLayout(true);
    }

    private void ApplyPercent()
    {
        ShowError("");
        decimal factor = 1 + percentBox.Value / 100m;
        foreach (var r in _rows.Where(x => x.Row.Visible))
        {
            decimal next = Math.Round(r.Box.Value * factor, 0, MidpointRounding.AwayFromZero);   // whole pesos
            r.Box.Value = Math.Clamp(next, r.Box.Minimum, r.Box.Maximum);
        }
    }

    protected override void OnSaveClicked()
    {
        var changes = _rows
            .Where(r => r.Box.Value != r.Service.Price)
            .Select(r => (r.Service.Id, r.Box.Value))
            .ToList();

        if (changes.Count == 0)
        {
            ShowError("No prices were changed.");
            return;
        }

        try
        {
            ServiceRepository.UpdatePrices(changes);
            ChangedCount = changes.Count;
            DialogResult = DialogResult.OK;
        }
        catch (SqlException ex)
        {
            ShowError("Could not save: " + ex.Message);
        }
    }
}
