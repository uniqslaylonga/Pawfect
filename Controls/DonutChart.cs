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

        // Everything is sized from the control and the display scale, so nothing is cut off at 125% / 150% DPI.
        float scale = DeviceDpi / 96f;
        int margin = (int)Math.Ceiling(10 * scale);
        int size = Math.Min(Width, Height) - margin * 2;
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
                if (sweep >= 359.9f)
                    g.DrawEllipse(pen, rect);   // a single full segment: draw a closed ring instead of a 360 degree arc
                else
                    g.DrawArc(pen, rect, start, sweep);
                start += sweep;
            }
        }

        // Centre text: measure it, then stack value + caption in the middle of the hole.
        const TextFormatFlags flags = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter |
                                      TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix | TextFormatFlags.NoClipping;
        var valueFont = Theme.Face(16f, FontStyle.Bold);
        var captionFont = Theme.Face(8f);
        var valueSize = TextRenderer.MeasureText(g, _centerValue, valueFont, new Size(int.MaxValue, int.MaxValue), flags);
        var captionSize = TextRenderer.MeasureText(g, _caption, captionFont, new Size(int.MaxValue, int.MaxValue), flags);

        int gap = (int)(2 * scale);
        int blockHeight = valueSize.Height + gap + captionSize.Height;
        int holeWidth = (int)(size - thick * 2) - 4;
        int top = (Height - blockHeight) / 2;
        int cx = Width / 2;

        TextRenderer.DrawText(g, _centerValue, valueFont,
            new Rectangle(cx - holeWidth / 2, top, holeWidth, valueSize.Height), Theme.Text, flags);
        TextRenderer.DrawText(g, _caption, captionFont,
            new Rectangle(cx - holeWidth / 2, top + valueSize.Height + gap, holeWidth, captionSize.Height), Theme.Muted, flags);
    }
}
