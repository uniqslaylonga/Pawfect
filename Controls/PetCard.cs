using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace MyApp.Controls;

/// <summary>Selectable pet tile: round avatar on the left, name and "Dog • Golden Retriever" beside it, a tick when chosen.</summary>
internal sealed class PetCard : Control
{
    private static readonly Dictionary<float, Font> EmojiFonts = new();

    private static Font EmojiAt(float points)
    {
        points = FullScreen.Pt(points);
        if (!EmojiFonts.TryGetValue(points, out var font))
            EmojiFonts[points] = font = new Font("Segoe UI Emoji", points, FontStyle.Regular, GraphicsUnit.Point);
        return font;
    }
    private string _detail = "";
    private string _species = "";
    private bool _selected;
    private bool _hover;

    public PetCard()
    {
        SetStyle(ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
        BackColor = Theme.Bg;
        Cursor = Cursors.Hand;
        // 240 x 112 with a 16 px gap: three tiles per row and two full rows (six tiles) fit the 820 x 262 My Pets list without scrolling.
        Size = new Size(240, 112);
        Margin = new Padding(0, 0, 16, 16);
    }

    [Category("Pawfect"), Description("Second line under the name, e.g. Dog • Golden Retriever.")]
    [DefaultValue("")]
    public string Detail { get => _detail; set { _detail = value; Invalidate(); } }

    [Category("Pawfect"), Description("Dog, Cat ... decides which picture is shown.")]
    [DefaultValue("")]
    public string Species { get => _species; set { _species = value; Invalidate(); } }

    [Category("Pawfect"), Description("Show the blue outline and tick.")]
    [DefaultValue(false)]
    public bool Selected { get => _selected; set { _selected = value; Invalidate(); } }

    protected override void OnTextChanged(EventArgs e) { Invalidate(); base.OnTextChanged(e); }
    protected override void OnMouseEnter(EventArgs e) { _hover = true; Invalidate(); base.OnMouseEnter(e); }
    protected override void OnMouseLeave(EventArgs e) { _hover = false; Invalidate(); base.OnMouseLeave(e); }

    private string Emoji()
    {
        string s = _species.ToLowerInvariant();
        return s.Contains("dog") ? "\U0001F436" : s.Contains("cat") ? "\U0001F431" : "\U0001F43E";
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;

        // Laid out for a 240 x 112 tile; everything grows with the tile when the screen is bigger.
        float k = Math.Max(0.5f, Math.Min(Width / 240f, Height / 112f));
        int S(float v) => (int)Math.Round(v * k);

        var rect = new RectangleF(1, 1, Width - 3, Height - 3);
        using (var path = Ui.RoundRect(rect, S(12)))
        {
            using (var fill = new SolidBrush(Color.White)) g.FillPath(fill, path);
            using var pen = new Pen(_selected ? Color.FromArgb(37, 99, 235) : _hover ? Color.FromArgb(160, 196, 245) : Theme.Border, _selected ? 2f : 1f);
            g.DrawPath(pen, path);
        }

        // avatar on the left, vertically centred; name and detail stacked to its right
        int d = S(64);
        var avatar = new Rectangle(S(16), (Height - d) / 2, d, d);
        using (var b = new SolidBrush(Color.FromArgb(240, 246, 255))) g.FillEllipse(b, avatar);
        using (var pen = new Pen(Theme.Border)) g.DrawEllipse(pen, avatar);
        TextRenderer.DrawText(g, Emoji(), EmojiAt(20f * k), avatar, Theme.Text,
            TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix);

        int tx = avatar.Right + S(14);
        int tw = Width - tx - S(14);
        int top = (Height - S(46)) / 2;
        TextRenderer.DrawText(g, Text, Theme.Face(FullScreen.Pt(10.5f * k), FontStyle.Bold), new Rectangle(tx, top, tw, S(24)), Theme.Text,
            TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine);
        TextRenderer.DrawText(g, _detail, Theme.Face(FullScreen.Pt(8f * k)), new Rectangle(tx, top + S(26), tw, S(20)), Theme.Muted,
            TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine);

        if (_selected)
        {
            var tick = new Rectangle(Width - S(32), S(8), S(20), S(20));
            using (var b = new SolidBrush(Color.FromArgb(37, 99, 235))) g.FillEllipse(b, tick);
            TextRenderer.DrawText(g, "", Theme.GlyphFont(FullScreen.Pt(8f * k)), tick, Color.White,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix);
        }
    }
}

/// <summary>Wide two-state tab button ("My Pets" / "Add New Pet"): blue when active, white with a blue outline when not.</summary>
internal sealed class SegmentButton : Control
{
    private string _glyph = "";
    private bool _active;
    private bool _hover;

    public SegmentButton()
    {
        SetStyle(ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
        Font = Theme.Face(9.5f, FontStyle.Bold);
        BackColor = Theme.Bg;
        Cursor = Cursors.Hand;
        Size = new Size(400, 44);
    }

    [Category("Pawfect"), Description("Icon character from the Segoe MDL2 Assets font.")]
    [DefaultValue("")]
    public string Glyph { get => _glyph; set { _glyph = value; Invalidate(); } }

    [Category("Pawfect"), Description("Show this tab as the selected one.")]
    [DefaultValue(false)]
    public bool Active { get => _active; set { _active = value; Invalidate(); } }

    protected override void OnTextChanged(EventArgs e) { Invalidate(); base.OnTextChanged(e); }
    protected override void OnMouseEnter(EventArgs e) { _hover = true; Invalidate(); base.OnMouseEnter(e); }
    protected override void OnMouseLeave(EventArgs e) { _hover = false; Invalidate(); base.OnMouseLeave(e); }

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        var blue = Color.FromArgb(37, 99, 235);

        using (var path = Ui.RoundRect(new RectangleF(0.5f, 0.5f, Width - 2, Height - 2), 12))
        {
            using (var fill = new SolidBrush(_active ? blue : _hover ? Theme.PrimarySoft : Color.White)) g.FillPath(fill, path);
            if (!_active)
            {
                using var pen = new Pen(Color.FromArgb(190, 214, 248));
                g.DrawPath(pen, path);
            }
        }

        var fore = _active ? Color.White : blue;
        const TextFormatFlags flags = TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine | TextFormatFlags.VerticalCenter;
        float k = Math.Max(0.5f, Height / 44f);
        int gw = _glyph.Length > 0 ? (int)Math.Round(24 * k) : 0;
        int tw = TextRenderer.MeasureText(g, Text, Font, new Size(Width, Height), flags).Width;
        int x = (Width - (gw + tw)) / 2;
        if (gw > 0)
            TextRenderer.DrawText(g, _glyph, Theme.GlyphFont(FullScreen.Pt(10f * k)), new Rectangle(x, 0, gw, Height), fore, flags | TextFormatFlags.HorizontalCenter);
        TextRenderer.DrawText(g, Text, Font, new Rectangle(x + gw, 0, tw + 4, Height), fore, flags | TextFormatFlags.Left);
    }
}
