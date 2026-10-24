using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace MyApp.Controls;

/// <summary>
/// Selectable service tile on customer step 4: line icon, name (wraps to two lines), price and a clock with "~ 30 mins",
/// a ring in the corner that turns into a blue tick when the tile is chosen. Laid out for a 109 x 100 tile; everything
/// grows with the tile, and the text is sized in pixels so it matches the tile on 100 % and 125 % screens alike.
/// </summary>
internal sealed class ServiceTile : Control
{
    public const int DesignWidth = 109, DesignHeight = 100;

    private string _price = "";
    private string _duration = "";
    private string _iconKey = "paw";
    private bool _selected;
    private bool _hover;

    public ServiceTile()
    {
        SetStyle(ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint, true);
        BackColor = Color.White;
        Cursor = Cursors.Hand;
        Size = new Size(DesignWidth, DesignHeight);
        TabStop = false;
    }

    [Category("Pawfect"), DefaultValue("")]
    public string Price { get => _price; set { _price = value; Invalidate(); } }

    [Category("Pawfect"), DefaultValue("")]
    public string Duration { get => _duration; set { _duration = value; Invalidate(); } }

    /// <summary>Which line icon to draw (see <see cref="SvcLook.IconKeyFor"/>).</summary>
    [Category("Pawfect"), DefaultValue("paw")]
    public string IconKey { get => _iconKey; set { _iconKey = value; Invalidate(); } }

    [Category("Pawfect"), DefaultValue(false)]
    public bool Selected { get => _selected; set { _selected = value; Invalidate(); } }

    protected override void OnTextChanged(EventArgs e) { Invalidate(); base.OnTextChanged(e); }
    protected override void OnMouseEnter(EventArgs e) { _hover = true; Invalidate(); base.OnMouseEnter(e); }
    protected override void OnMouseLeave(EventArgs e) { _hover = false; Invalidate(); base.OnMouseLeave(e); }

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.Clear(BackColor);   // the page colour behind the rounded corners

        float k = Math.Max(0.5f, Math.Min(Width / (float)DesignWidth, Height / (float)DesignHeight));
        float S(float v) => v * k;

        // ---- tile
        var rect = new RectangleF(1, 1, Width - 3, Height - 3);
        using (var path = Ui.RoundRect(rect, S(12)))
        {
            using (var fill = new SolidBrush(_selected ? SvcLook.BlueSoft : Color.White)) g.FillPath(fill, path);
            using var pen = new Pen(_selected ? SvcLook.Blue : _hover ? SvcLook.BlueHover : Color.FromArgb(226, 232, 242),
                _selected ? Math.Max(1.5f, S(1.7f)) : Math.Max(1f, S(1f)));
            g.DrawPath(pen, path);
        }

        // ---- corner ring / tick
        var dot = new RectangleF(Width - S(19), S(7), S(12), S(12));
        if (_selected)
        {
            using var b = new SolidBrush(SvcLook.Blue);
            g.FillEllipse(b, dot);
            using var tick = new Pen(Color.White, Math.Max(1f, S(1.4f))) { StartCap = LineCap.Round, EndCap = LineCap.Round, LineJoin = LineJoin.Round };
            g.DrawLines(tick, new[]
            {
                new PointF(dot.X + dot.Width * 0.26f, dot.Y + dot.Height * 0.52f),
                new PointF(dot.X + dot.Width * 0.43f, dot.Y + dot.Height * 0.69f),
                new PointF(dot.X + dot.Width * 0.75f, dot.Y + dot.Height * 0.32f),
            });
        }
        else
        {
            using var ring = new Pen(Color.FromArgb(205, 213, 226), Math.Max(1f, S(1f)));
            g.DrawEllipse(ring, dot);
        }

        // ---- icon
        float iconSize = S(22);
        SvcLook.DrawIcon(g, _iconKey, new RectangleF((Width - iconSize) / 2f, S(11), iconSize, iconSize), SvcLook.Blue, Math.Max(1.2f, S(1.5f)));

        // ---- name (wraps to two lines), price, duration: each starts where the one above ends
        const TextFormatFlags wrap = TextFormatFlags.HorizontalCenter | TextFormatFlags.Top | TextFormatFlags.NoPadding |
                                     TextFormatFlags.NoPrefix | TextFormatFlags.WordBreak | TextFormatFlags.EndEllipsis;
        var nameFont = SvcLook.Px(S(9.4f), FontStyle.Bold);
        int textW = Math.Max(10, Width - (int)S(12));
        int oneLine = TextRenderer.MeasureText(g, "Ag", nameFont, new Size(textW, int.MaxValue), wrap).Height;
        int nameH = TextRenderer.MeasureText(g, Text, nameFont, new Size(textW, int.MaxValue), wrap).Height;
        nameH = Math.Min(nameH, oneLine * 2);   // never more than two lines
        int y = (int)S(38);
        TextRenderer.DrawText(g, Text, nameFont, new Rectangle((Width - textW) / 2, y, textW, nameH), SvcLook.Navy, wrap);
        y += nameH + (int)S(2);

        var priceFont = SvcLook.Px(S(11.5f), FontStyle.Bold);
        int priceH = TextRenderer.MeasureText(g, "Ag", priceFont, new Size(textW, int.MaxValue), TextFormatFlags.NoPadding | TextFormatFlags.SingleLine).Height;
        TextRenderer.DrawText(g, _price, priceFont, new Rectangle((Width - textW) / 2, y, textW, priceH), SvcLook.Navy,
            TextFormatFlags.HorizontalCenter | TextFormatFlags.Top | TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine);
        y += priceH + (int)S(2);

        // clock + "~ 30 mins", centred as one piece
        var durFont = SvcLook.Px(S(8.6f));
        var durSize = TextRenderer.MeasureText(g, _duration, durFont, new Size(int.MaxValue, int.MaxValue),
            TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine);
        float clock = S(8), gap = S(3.5f);
        float total = clock + gap + durSize.Width;
        float x0 = (Width - total) / 2f;
        var clockBox = new RectangleF(x0, y + (durSize.Height - clock) / 2f, clock, clock);
        using (var cp = new Pen(SvcLook.Muted, Math.Max(1f, S(0.9f))) { StartCap = LineCap.Round, EndCap = LineCap.Round })
        {
            g.DrawEllipse(cp, clockBox);
            float cx = clockBox.X + clock / 2f, cy = clockBox.Y + clock / 2f;
            g.DrawLine(cp, cx, cy, cx, cy - clock * 0.28f);
            g.DrawLine(cp, cx, cy, cx + clock * 0.22f, cy + clock * 0.12f);
        }
        TextRenderer.DrawText(g, _duration, durFont, new Rectangle((int)Math.Round(x0 + clock + gap), y, durSize.Width + 2, durSize.Height), SvcLook.Muted,
            TextFormatFlags.Left | TextFormatFlags.Top | TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine);
    }
}
