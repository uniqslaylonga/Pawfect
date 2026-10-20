using System.Drawing.Drawing2D;
using MyApp.Controls;

namespace MyApp;

/// <summary>
/// Read-only list of the services an admin has set up (employees can't change prices).
/// One card per service; services that are priced by weight list a Dog / Cat tag, the weight range and the price per row.
/// A search box at the top filters by service name, species or weight text.
/// </summary>
internal sealed class ServiceListForm : DialogBase
{
    private readonly List<ServiceOffering> _services;
    private readonly Panel _list = new()
    {
        Dock = DockStyle.Fill,
        AutoScroll = true,
        BackColor = Theme.Subtle,
        Padding = new Padding(20, 10, 20, 10),
    };
    private readonly TextBox _search = new() { PlaceholderText = "Search services, Dog, Cat..." };

    public ServiceListForm() : base("Services", "Close", "", "Prices by pet species and weight")
    {
        ClientSize = new Size(560, 600);
        Grid.Visible = false; // this dialog has no label | input rows

        _services = EmployeeData.GetServices();

        // ---- search bar (white strip under the header)
        var searchBar = new Panel { Dock = DockStyle.Top, Height = 56, BackColor = Color.White, Padding = new Padding(20, 10, 20, 10) };
        var frame = FieldFrame.ForEditor(_search);
        frame.Dock = DockStyle.Fill;
        searchBar.Controls.Add(frame);
        searchBar.Paint += (_, e) =>
        {
            using var pen = new Pen(Theme.Border);
            e.Graphics.DrawLine(pen, 0, searchBar.Height - 1, searchBar.Width, searchBar.Height - 1);
        };

        // The fill panel is added first so it is docked last, below the search bar.
        var body = new Panel { Dock = DockStyle.Fill, BackColor = Theme.Subtle };
        body.Controls.Add(_list);
        body.Controls.Add(searchBar);
        Controls.Add(body);
        body.BringToFront();

        SaveButton.Visible = false;
        CancelButtonControl.Text = "Close";

        _search.TextChanged += (_, _) => Rebuild();
        Rebuild();
    }

    private void Rebuild()
    {
        string q = _search.Text.Trim();
        var groups = _services
            .Where(s => q.Length == 0
                        || s.Name.Contains(q, StringComparison.OrdinalIgnoreCase)
                        || s.Detail.Contains(q, StringComparison.OrdinalIgnoreCase))
            .GroupBy(s => s.Name)
            .ToList();

        _list.SuspendLayout();
        foreach (Control old in _list.Controls.Cast<Control>().ToList())
        {
            _list.Controls.Remove(old);
            old.Dispose();
        }

        if (groups.Count == 0)
        {
            string text = _services.Count == 0
                ? "No services exist yet. An admin needs to add them."
                : "No services match your search.";
            _list.Controls.Add(Ui.Text(text, 9.5f, Theme.Muted, Theme.Subtle, FontStyle.Regular, ContentAlignment.MiddleCenter));
            _list.Controls[0].Dock = DockStyle.Top;
            _list.Controls[0].Height = 120;
        }
        else
        {
            // Dock.Top stacks the last control added first, so add in reverse to keep the alphabetical order.
            foreach (var g in Enumerable.Reverse(groups))
                _list.Controls.Add(new ServiceCardView(g.Key, g.ToList()) { Dock = DockStyle.Top });
        }

        _list.ResumeLayout(true);
        _list.AutoScrollPosition = Point.Empty;
    }

    protected override void OnSaveClicked() => DialogResult = DialogResult.OK;
}

/// <summary>One service: its name, then a row per weight band (or just the price when it doesn't depend on weight).</summary>
internal sealed class ServiceCardView : Control
{
    private const int Gap = 10;          // space under the card
    private const int HeaderHeight = 46;
    private const int RowHeight = 34;

    private const TextFormatFlags LeftFlags = TextFormatFlags.Left | TextFormatFlags.VerticalCenter |
        TextFormatFlags.EndEllipsis | TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine;
    private const TextFormatFlags RightFlags = TextFormatFlags.Right | TextFormatFlags.VerticalCenter |
        TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine;
    private const TextFormatFlags CenterFlags = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter |
        TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine;

    private readonly string _name;
    private readonly List<(string Species, string Range, decimal Price)> _rows = new();
    private readonly decimal? _flatPrice;

    public ServiceCardView(string name, List<ServiceOffering> offers)
    {
        _name = name;
        SetStyle(ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw |
                 ControlStyles.AllPaintingInWmPaint, true);
        BackColor = Theme.Subtle;

        if (offers.Count == 1 && offers[0].Detail.Length == 0)
        {
            _flatPrice = offers[0].Price; // one price for every pet
            Height = HeaderHeight + Gap;
        }
        else
        {
            foreach (var o in offers)
            {
                // Detail looks like "Dog · up to 5 kg", "Cat · over 6 kg", "5.1-10 kg" or empty.
                string species = "", range = o.Detail.Length > 0 ? o.Detail : "All sizes";
                int dot = o.Detail.IndexOf(" · ", StringComparison.Ordinal);
                if (dot > 0)
                {
                    species = o.Detail[..dot];
                    range = o.Detail[(dot + 3)..];
                }
                _rows.Add((species, range, o.Price));
            }
            Height = HeaderHeight + _rows.Count * RowHeight + 8 + Gap;
        }
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.Clear(Theme.Subtle);

        var card = new Rectangle(0, 0, Width - 1, Height - Gap - 1);
        using (var path = RoundedRect(card, 10))
        using (var fill = new SolidBrush(Color.White))
        using (var pen = new Pen(Theme.Border))
        {
            g.FillPath(fill, path);
            g.DrawPath(pen, path);
        }

        // ---- header: service name (+ the price on the right when it is one flat price)
        var headerRect = new Rectangle(16, 0, card.Width - 32, HeaderHeight);
        if (_flatPrice is { } flat)
        {
            var priceText = Money.Format(flat);
            TextRenderer.DrawText(g, priceText, Theme.Face(10.5f, FontStyle.Bold), headerRect, Theme.Primary, RightFlags);
            int priceWidth = TextRenderer.MeasureText(priceText, Theme.Face(10.5f, FontStyle.Bold)).Width + 12;
            headerRect.Width -= priceWidth;
        }
        else
        {
            string count = _rows.Count == 1 ? "1 price" : $"{_rows.Count} prices";
            var countFont = Theme.Face(8.25f);
            TextRenderer.DrawText(g, count, countFont, headerRect, Theme.Muted, RightFlags);
            headerRect.Width -= TextRenderer.MeasureText(count, countFont).Width + 12;
        }
        TextRenderer.DrawText(g, _name, Theme.Face(10.5f, FontStyle.Bold), headerRect, Theme.Text, LeftFlags);

        // ---- one row per weight band
        for (int i = 0; i < _rows.Count; i++)
        {
            var (species, range, price) = _rows[i];
            int y = HeaderHeight + i * RowHeight;

            using (var line = new Pen(Theme.Border))
                g.DrawLine(line, 16, y, card.Width - 16, y);

            int x = 16;
            if (species.Length > 0)
            {
                var (soft, strong) = species.Equals("Cat", StringComparison.OrdinalIgnoreCase)
                    ? (Color.FromArgb(243, 237, 255), Theme.Purple)
                    : (Theme.PrimarySoft, Theme.Primary);
                var pill = new Rectangle(x, y + (RowHeight - 20) / 2, 44, 20);
                using (var pillPath = RoundedRect(pill, 10))
                using (var pillFill = new SolidBrush(soft))
                    g.FillPath(pillFill, pillPath);
                TextRenderer.DrawText(g, species, Theme.Face(8f, FontStyle.Bold), pill, strong, CenterFlags);
                x += 56;
            }

            var priceFont = Theme.Face(9.5f, FontStyle.Bold);
            string priceLabel = Money.Format(price);
            int priceW = TextRenderer.MeasureText(priceLabel, priceFont).Width + 8;
            TextRenderer.DrawText(g, priceLabel, priceFont, new Rectangle(card.Width - 16 - priceW, y, priceW, RowHeight), Theme.Text, RightFlags);
            TextRenderer.DrawText(g, range, Theme.Face(9f), new Rectangle(x, y, card.Width - 16 - priceW - x - 8, RowHeight), Theme.TextSoft, LeftFlags);
        }
    }

    private static GraphicsPath RoundedRect(Rectangle r, int radius)
    {
        int d = radius * 2;
        var p = new GraphicsPath();
        p.AddArc(r.X, r.Y, d, d, 180, 90);
        p.AddArc(r.Right - d, r.Y, d, d, 270, 90);
        p.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
        p.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
        p.CloseFigure();
        return p;
    }
}
