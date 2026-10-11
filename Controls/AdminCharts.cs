using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace MyApp.Controls;

/// <summary>One coloured set of bars in a <see cref="BarChart"/> (one value per x label).</summary>
internal sealed record BarSeries(string Name, Color Tint, IReadOnlyList<double> Values);

/// <summary>
/// Shared plumbing for the revenue charts: plot area, y-axis scale, grid, x labels and the "no data" state.
/// Nothing is drawn from made-up numbers: until <c>SetData</c> is called the chart shows an empty state.
/// </summary>
internal abstract class ChartBase : Control
{
    private string _emptyText = "No revenue data yet";
    private string _emptyHint = "Totals will appear here once sales are recorded.";

    protected ChartBase()
    {
        SetStyle(ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
        BackColor = Color.White;
        TabStop = false;
        Size = new Size(300, 200);
    }

    [Category("Pawfect"), Description("Bold line shown while the chart has no data.")]
    [DefaultValue("No revenue data yet")]
    public string EmptyText { get => _emptyText; set { _emptyText = value; Invalidate(); } }

    [Category("Pawfect"), Description("Smaller grey line shown under the empty-state title.")]
    [DefaultValue("Totals will appear here once sales are recorded.")]
    public string EmptyHint { get => _emptyHint; set { _emptyHint = value; Invalidate(); } }

    protected Rectangle PlotAreaFor(int leftGutter) =>
        new(leftGutter, 10, Math.Max(0, Width - leftGutter - 8), Math.Max(0, Height - 10 - 26));

    /// <summary>Rounds the biggest value up to a tidy axis maximum split into <paramref name="steps"/> equal steps.</summary>
    protected static double NiceMax(double max, out int steps)
    {
        steps = 4;
        if (max <= 0) return steps;
        double raw = max / steps;
        double mag = Math.Pow(10, Math.Floor(Math.Log10(raw)));
        double norm = raw / mag;
        double nice = norm <= 1 ? 1 : norm <= 2 ? 2 : norm <= 5 ? 5 : 10;
        return nice * mag * steps;
    }

    protected static string Peso(double value) => Money.Format((decimal)value);

    /// <summary>Width of the y-axis label column for a given axis maximum.</summary>
    protected static int GutterFor(double axisMax) =>
        Math.Max(44, TextRenderer.MeasureText(Peso(axisMax), Theme.Face(8f)).Width + 16);

    protected void DrawAxes(Graphics g, Rectangle plot, double axisMax, int steps, IReadOnlyList<string> labels)
    {
        const TextFormatFlags flags = TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding |
                                      TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine;

        using var grid = new Pen(Theme.Border);
        for (int i = 0; i <= steps; i++)
        {
            int y = plot.Bottom - (int)Math.Round(i * (double)plot.Height / steps);
            g.DrawLine(grid, plot.Left, y, plot.Right, y);
            TextRenderer.DrawText(g, Peso(axisMax * i / steps), Theme.Face(8f),
                new Rectangle(0, y - 8, plot.Left - 8, 16), Theme.Muted, flags | TextFormatFlags.Right);
        }

        int n = labels.Count;
        if (n == 0) return;

        float slot = (float)plot.Width / n;
        int stride = Math.Max(1, (int)Math.Ceiling(52f / slot)); // skip labels when they'd overlap
        for (int i = 0; i < n; i += stride)
        {
            var r = new Rectangle((int)(plot.Left + i * slot + slot / 2f - 32), plot.Bottom + 6, 64, 16);
            TextRenderer.DrawText(g, labels[i], Theme.Face(8f), r, Theme.Muted, flags | TextFormatFlags.HorizontalCenter);
        }
    }

    protected void DrawEmpty(Graphics g)
    {
        var plot = PlotAreaFor(12);
        if (plot.Width < 60 || plot.Height < 40) return;

        using (var baseline = new Pen(Theme.Border))
            g.DrawLine(baseline, plot.Left, plot.Bottom, plot.Right, plot.Bottom);

        var titleFont = Theme.Face(9.5f, FontStyle.Bold);
        var hintFont = Theme.Face(8.5f);
        const TextFormatFlags flags = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter |
                                      TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine;

        var titleSize = TextRenderer.MeasureText(_emptyText, titleFont);
        var hintSize = TextRenderer.MeasureText(_emptyHint, hintFont);
        int cy = plot.Top + plot.Height / 2;
        int cx = plot.Left + plot.Width / 2;

        TextRenderer.DrawText(g, _emptyText, titleFont,
            new Rectangle(cx - titleSize.Width / 2 - 4, cy - 20, titleSize.Width + 8, 20), Theme.Text, flags);
        if (_emptyHint.Length > 0)
            TextRenderer.DrawText(g, _emptyHint, hintFont,
                new Rectangle(cx - hintSize.Width / 2 - 4, cy + 2, hintSize.Width + 8, 18), Theme.Muted, flags);
    }
}

/// <summary>Single-line area chart (Gross Revenue Summary). Hover a point to see its value.</summary>
internal sealed class LineChart : ChartBase
{
    private List<string> _labels = new();
    private List<double> _values = new();
    private Color _line = Theme.Primary;
    private int _hot = -1;
    private Rectangle _plot;
    private double _axisMax = 1;

    [Category("Pawfect"), Description("Colour of the line and its soft fill.")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public Color LineColor { get => _line; set { _line = value; Invalidate(); } }

    /// <summary>Plots one point per label. Both lists must be the same length (extra items are ignored).</summary>
    public void SetData(IReadOnlyList<string> labels, IReadOnlyList<double> values)
    {
        int n = Math.Min(labels.Count, values.Count);
        _labels = labels.Take(n).ToList();
        _values = values.Take(n).ToList();
        _hot = -1;
        Invalidate();
    }

    public void Clear() => SetData(Array.Empty<string>(), Array.Empty<double>());

    private PointF PointAt(int i) => new(
        _plot.Left + (i + 0.5f) * _plot.Width / _values.Count,
        _plot.Bottom - (float)(_values[i] / _axisMax * _plot.Height));

    protected override void OnMouseMove(MouseEventArgs e)
    {
        base.OnMouseMove(e);
        int hot = -1;
        if (_values.Count > 0 && _plot.Width > 0 && e.X >= _plot.Left && e.X <= _plot.Right)
            hot = Math.Clamp((int)((e.X - _plot.Left) / ((float)_plot.Width / _values.Count)), 0, _values.Count - 1);

        if (hot != _hot)
        {
            _hot = hot;
            Invalidate();
        }
    }

    protected override void OnMouseLeave(EventArgs e)
    {
        base.OnMouseLeave(e);
        if (_hot != -1)
        {
            _hot = -1;
            Invalidate();
        }
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;

        if (_values.Count == 0)
        {
            _plot = Rectangle.Empty;
            DrawEmpty(g);
            return;
        }

        _axisMax = NiceMax(_values.Max(), out int steps);
        _plot = PlotAreaFor(GutterFor(_axisMax));
        if (_plot.Width < 40 || _plot.Height < 40) return;

        DrawAxes(g, _plot, _axisMax, steps, _labels);

        var pts = Enumerable.Range(0, _values.Count).Select(PointAt).ToArray();

        if (pts.Length > 1)
        {
            var area = pts.Concat(new[] { new PointF(pts[^1].X, _plot.Bottom), new PointF(pts[0].X, _plot.Bottom) }).ToArray();
            using (var fill = new LinearGradientBrush(
                       new Rectangle(_plot.Left, _plot.Top, _plot.Width, _plot.Height + 1),
                       Color.FromArgb(70, _line), Color.FromArgb(0, _line), LinearGradientMode.Vertical))
                g.FillPolygon(fill, area);

            using var pen = new Pen(_line, 2f) { LineJoin = LineJoin.Round };
            g.DrawLines(pen, pts);
        }

        for (int i = 0; i < pts.Length; i++)
        {
            float r = i == _hot ? 5f : 3.5f;
            using var fill = new SolidBrush(Color.White);
            using var pen = new Pen(_line, 2f);
            g.FillEllipse(fill, pts[i].X - r, pts[i].Y - r, r * 2, r * 2);
            g.DrawEllipse(pen, pts[i].X - r, pts[i].Y - r, r * 2, r * 2);
        }

        if (_hot >= 0) DrawTooltip(g, pts[_hot], _labels[_hot], Peso(_values[_hot]));
    }

    private void DrawTooltip(Graphics g, PointF anchor, string title, string value)
    {
        var titleFont = Theme.Face(7.5f);
        var valueFont = Theme.Face(9f, FontStyle.Bold);
        int w = Math.Max(TextRenderer.MeasureText(title, titleFont).Width, TextRenderer.MeasureText(value, valueFont).Width) + 22;
        const int h = 40;

        float x = Math.Clamp(anchor.X - w / 2f, 0, Math.Max(0, Width - w));
        float y = anchor.Y - h - 10;
        if (y < 0) y = anchor.Y + 10;

        using (var path = Ui.RoundRect(new RectangleF(x, y, w, h), 6))
        using (var back = new SolidBrush(Theme.Text))
            g.FillPath(back, path);

        const TextFormatFlags flags = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter |
                                      TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine;
        TextRenderer.DrawText(g, title, titleFont, new Rectangle((int)x, (int)y + 4, w, 14), Color.FromArgb(190, 200, 214), flags);
        TextRenderer.DrawText(g, value, valueFont, new Rectangle((int)x, (int)y + 18, w, 18), Color.White, flags);
    }
}

/// <summary>Grouped bar chart (Revenue Overview). One group per x label, one bar per series.</summary>
internal sealed class BarChart : ChartBase
{
    private List<string> _labels = new();
    private List<BarSeries> _series = new();

    /// <summary>Draws one group of bars per label, one bar in each series' colour.</summary>
    public void SetData(IReadOnlyList<string> labels, IEnumerable<BarSeries> series)
    {
        _labels = labels.ToList();
        _series = series.ToList();
        Invalidate();
    }

    public void Clear() => SetData(Array.Empty<string>(), Array.Empty<BarSeries>());

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;

        double top = _series.Count == 0 ? 0 : _series.Max(s => s.Values.Count == 0 ? 0 : s.Values.Max());
        if (_labels.Count == 0 || top <= 0)
        {
            DrawEmpty(g);
            return;
        }

        double axisMax = NiceMax(top, out int steps);
        var plot = PlotAreaFor(GutterFor(axisMax));
        if (plot.Width < 40 || plot.Height < 40) return;

        DrawAxes(g, plot, axisMax, steps, _labels);

        float slot = (float)plot.Width / _labels.Count;
        const float gap = 3f;
        float barW = Math.Min(16f, slot / (_series.Count + 1.5f));
        float groupW = barW * _series.Count + gap * Math.Max(0, _series.Count - 1);

        for (int i = 0; i < _labels.Count; i++)
        {
            float x = plot.Left + i * slot + (slot - groupW) / 2f;
            foreach (var s in _series)
            {
                double v = i < s.Values.Count ? s.Values[i] : 0;
                if (v > 0)
                {
                    float h = Math.Max(1f, (float)(v / axisMax * plot.Height));
                    using var path = Ui.RoundRect(new RectangleF(x, plot.Bottom - h, barW, h), 3);
                    using var brush = new SolidBrush(s.Tint);
                    g.FillPath(brush, path);
                }
                x += barW + gap;
            }
        }
    }
}
