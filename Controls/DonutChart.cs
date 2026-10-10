using System.Drawing.Drawing2D;

namespace MyApp.Controls;

/// <summary>Ring chart. With no segments it shows an empty grey ring.</summary>
internal sealed class DonutChart : Control
{
    private readonly List<(double Value, Color Tint)> _segments = new();
    private string _centerValue = "0";
    private string _caption = "Completed";

    public DonutChart()
    {
        SetStyle(ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
        BackColor = Color.White;
        TabStop = false;
    }

    public void SetData(IEnumerable<(double Value, Color Tint)> segments, string centerValue, string caption)
    {
        _segments.Clear();
        _segments.AddRange(segments);
        _centerValue = centerValue;
        _caption = caption;
        Invalidate();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;

        int size = Math.Min(Width, Height) - 16;
        if (size < 40) return;

        float thick = size * 0.17f;
        var rect = new RectangleF(
            (Width - size) / 2f + thick / 2f,
            (Height - size) / 2f + thick / 2f,
            size - thick,
            size - thick);

        using (var track = new Pen(Theme.Track, thick))
            g.DrawEllipse(track, rect);

        double total = _segments.Sum(s => s.Value);
        if (total > 0)
        {
            float start = -90f;
            foreach (var seg in _segments)
            {
                if (seg.Value <= 0) continue;
                float sweep = (float)(seg.Value / total * 360d);
                using var pen = new Pen(seg.Tint, thick);
                g.DrawArc(pen, rect, start, sweep);
                start += sweep;
            }
        }

        int cx = Width / 2, cy = Height / 2;
        TextRenderer.DrawText(g, _centerValue, Theme.Face(16f, FontStyle.Bold),
            new Rectangle(cx - 40, cy - 24, 80, 28), Theme.Text,
            TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);
        TextRenderer.DrawText(g, _caption, Theme.Face(8f), new Rectangle(cx - 40, cy + 4, 80, 16), Theme.Muted,
            TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);
    }
}
