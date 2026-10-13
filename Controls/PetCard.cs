using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace MyApp.Controls;

/// <summary>Selectable pet tile: round avatar, name and "Dog • Golden Retriever" underneath, a tick when chosen.</summary>
internal sealed class PetCard : Control
{
    private static readonly Font EmojiFont = new("Segoe UI Emoji", 28f, FontStyle.Regular, GraphicsUnit.Point);
    private string _detail = "";
    private string _species = "";
    private bool _selected;
    private bool _hover;

    public PetCard()
    {
        SetStyle(ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
        BackColor = Theme.Bg;
        Cursor = Cursors.Hand;
        Size = new Size(260, 200);
        Margin = new Padding(0, 0, 20, 20);
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

        var rect = new RectangleF(1, 1, Width - 3, Height - 3);
        using (var path = Ui.RoundRect(rect, 14))
        {
            using (var fill = new SolidBrush(Color.White)) g.FillPath(fill, path);
            using var pen = new Pen(_selected ? Color.FromArgb(37, 99, 235) : _hover ? Color.FromArgb(160, 196, 245) : Theme.Border, _selected ? 2f : 1f);
            g.DrawPath(pen, path);
        }

        const int d = 92;
        var avatar = new Rectangle((Width - d) / 2, 24, d, d);
        using (var b = new SolidBrush(Color.FromArgb(240, 246, 255))) g.FillEllipse(b, avatar);
        using (var pen = new Pen(Theme.Border)) g.DrawEllipse(pen, avatar);
        TextRenderer.DrawText(g, Emoji(), EmojiFont, avatar, Theme.Text,
            TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix);

        TextRenderer.DrawText(g, Text, Theme.Face(10.5f, FontStyle.Bold), new Rectangle(18, 138, Width - 36, 22), Theme.Text,
            TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine);
        TextRenderer.DrawText(g, _detail, Theme.Face(8f), new Rectangle(18, 162, Width - 36, 18), Theme.Muted,
            TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine);

        if (_selected)
        {
            var tick = new Rectangle(Width - 36, 12, 22, 22);
            using (var b = new SolidBrush(Color.FromArgb(37, 99, 235))) g.FillEllipse(b, tick);
            TextRenderer.DrawText(g, "", Theme.GlyphFont(8f), tick, Color.White,
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
        int gw = _glyph.Length > 0 ? 24 : 0;
        int tw = TextRenderer.MeasureText(g, Text, Font, new Size(Width, Height), flags).Width;
        int x = (Width - (gw + tw)) / 2;
        if (gw > 0)
            TextRenderer.DrawText(g, _glyph, Theme.GlyphFont(10f), new Rectangle(x, 0, gw, Height), fore, flags | TextFormatFlags.HorizontalCenter);
        TextRenderer.DrawText(g, Text, Font, new Rectangle(x + gw, 0, tw + 4, Height), fore, flags | TextFormatFlags.Left);
    }
}
