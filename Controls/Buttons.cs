using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace MyApp.Controls;

/// <summary>Sidebar menu entry.</summary>
internal sealed class NavItem : Control
{
    private string _glyph = "";
    private bool _hover;
    private bool _active;

    public NavItem()
    {
        SetStyle(ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
        Font = Theme.Face(9f);
        BackColor = Color.White;
        Cursor = Cursors.Hand;
        Size = new Size(219, 42);
    }

    public NavItem(string glyph, string text, int width, int height = 42, bool active = false) : this()
    {
        _glyph = glyph;
        _active = active;
        Text = text;
        Size = new Size(width, height);
    }

    [Category("Pawfect"), Description("Icon character from the Segoe MDL2 Assets font.")]
    [DefaultValue("")]
    public string Glyph { get => _glyph; set { _glyph = value; Invalidate(); } }

    [Category("Pawfect"), Description("Highlight this entry as the current page.")]
    [DefaultValue(false)]
    public bool Active { get => _active; set { _active = value; Invalidate(); } }

    protected override void OnTextChanged(EventArgs e) { Invalidate(); base.OnTextChanged(e); }

    protected override void OnMouseEnter(EventArgs e) { _hover = true; Invalidate(); base.OnMouseEnter(e); }
    protected override void OnMouseLeave(EventArgs e) { _hover = false; Invalidate(); base.OnMouseLeave(e); }

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;

        if (_active || _hover)
        {
            using var path = Ui.RoundRect(new RectangleF(0, 0, Width - 1, Height - 1), 8);
            using var b = new SolidBrush(_active ? Theme.Primary : Theme.SubtleHover);
            g.FillPath(b, path);
        }

        var fore = _active ? Color.White : Theme.TextSoft;

        TextRenderer.DrawText(g, _glyph, Theme.GlyphFont(11f), new Rectangle(10, 0, 28, Height), fore,
            TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter |
            TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine);

        TextRenderer.DrawText(g, Text, Font, new Rectangle(46, 0, Width - 54, Height), fore,
            TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.WordBreak |
            TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix);
    }
}

/// <summary>Rounded filter tab ("All (0)", "Walk-in (0)" ...).</summary>
internal sealed class TabPill : Control
{
    private bool _hover;
    private bool _active;

    public TabPill()
    {
        SetStyle(ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
        Font = Theme.Face(8.5f);
        BackColor = Color.White;
        Cursor = Cursors.Hand;
    }

    public TabPill(string text, bool active = false) : this()
    {
        _active = active;
        Text = text;
    }

    [Category("Pawfect"), Description("Show this tab as the selected one.")]
    [DefaultValue(false)]
    public bool Active { get => _active; set { _active = value; Invalidate(); } }

    protected override void OnFontChanged(EventArgs e)
    {
        FitToText();
        Invalidate();
        base.OnFontChanged(e);
    }

    public override string Text
    {
        get => base.Text;
        set
        {
            base.Text = value;
            FitToText();
            Invalidate();
        }
    }

    private void FitToText()
    {
        int w = TextRenderer.MeasureText(base.Text, Font).Width;
        Size = new Size(w + 24, 26);
    }

    protected override void OnMouseEnter(EventArgs e) { _hover = true; Invalidate(); base.OnMouseEnter(e); }
    protected override void OnMouseLeave(EventArgs e) { _hover = false; Invalidate(); base.OnMouseLeave(e); }

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;

        using var path = Ui.RoundRect(new RectangleF(0.5f, 0.5f, Width - 2, Height - 2), Height / 2f);
        using (var fill = new SolidBrush(_active ? Theme.Primary : _hover ? Theme.PrimarySoft : Color.White))
            g.FillPath(fill, path);
        if (!_active)
        {
            using var pen = new Pen(Theme.Border);
            g.DrawPath(pen, path);
        }

        TextRenderer.DrawText(g, Text, Font, ClientRectangle, _active ? Color.White : Theme.TextSoft,
            TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter |
            TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine);
    }
}

/// <summary>Quick Actions tile: icon on top, label underneath.</summary>
internal sealed class ActionTile : Control
{
    private string _glyph = "";
    private bool _hover;

    public ActionTile()
    {
        SetStyle(ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
        Font = Theme.Face(8.5f);
        BackColor = Color.White;
        Cursor = Cursors.Hand;
        Size = new Size(150, 62);
    }

    public ActionTile(string glyph, string text) : this()
    {
        _glyph = glyph;
        Text = text;
    }

    [Category("Pawfect"), Description("Icon character from the Segoe MDL2 Assets font.")]
    [DefaultValue("")]
    public string Glyph { get => _glyph; set { _glyph = value; Invalidate(); } }

    protected override void OnTextChanged(EventArgs e) { Invalidate(); base.OnTextChanged(e); }

    protected override void OnMouseEnter(EventArgs e) { _hover = true; Invalidate(); base.OnMouseEnter(e); }
    protected override void OnMouseLeave(EventArgs e) { _hover = false; Invalidate(); base.OnMouseLeave(e); }

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;

        using (var path = Ui.RoundRect(new RectangleF(0, 0, Width - 1, Height - 1), 8))
        using (var b = new SolidBrush(_hover ? Theme.SubtleHover : Theme.Subtle))
            g.FillPath(b, path);

        int y = Math.Max(0, (Height - 42) / 2);

        TextRenderer.DrawText(g, _glyph, Theme.GlyphFont(12f), new Rectangle(0, y, Width, 20), Theme.Primary,
            TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter |
            TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine);

        TextRenderer.DrawText(g, Text, Font, new Rectangle(4, y + 26, Width - 8, 16), Theme.TextSoft,
            TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter |
            TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine);
    }
}

/// <summary>Round icon-only button (notification bell).</summary>
internal sealed class IconButton : Control
{
    private string _glyph = "";
    private bool _hover;
    private bool _showDot;

    public IconButton()
    {
        SetStyle(ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
        BackColor = Color.White;
        Cursor = Cursors.Hand;
        Size = new Size(38, 38);
    }

    public IconButton(string glyph, Color back) : this()
    {
        _glyph = glyph;
        BackColor = back;
    }

    [Category("Pawfect"), Description("Icon character from the Segoe MDL2 Assets font.")]
    [DefaultValue("")]
    public string Glyph { get => _glyph; set { _glyph = value; Invalidate(); } }

    /// <summary>Red unread dot. Off until there is something to be notified about.</summary>
    [Category("Pawfect"), Description("Show the red unread dot.")]
    [DefaultValue(false)]
    public bool ShowDot { get => _showDot; set { _showDot = value; Invalidate(); } }

    protected override void OnMouseEnter(EventArgs e) { _hover = true; Invalidate(); base.OnMouseEnter(e); }
    protected override void OnMouseLeave(EventArgs e) { _hover = false; Invalidate(); base.OnMouseLeave(e); }

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;

        if (_hover)
        {
            using var b = new SolidBrush(Theme.Subtle);
            g.FillEllipse(b, 0, 0, Width - 1, Height - 1);
        }

        TextRenderer.DrawText(g, _glyph, Theme.GlyphFont(12f), ClientRectangle, Theme.TextSoft,
            TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter |
            TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine);

        if (_showDot)
        {
            using var dot = new SolidBrush(Theme.Danger);
            g.FillEllipse(dot, Width / 2f + 4, Height / 2f - 12, 8, 8);
        }
    }
}

/// <summary>Solid blue rounded call-to-action button.</summary>
internal sealed class PrimaryButton : Control
{
    private bool _hover;
    private bool _down;

    public PrimaryButton()
    {
        SetStyle(ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
        Font = Theme.Face(10f, FontStyle.Bold);
        BackColor = Color.White;
        Cursor = Cursors.Hand;
        Size = new Size(316, 44);
    }

    public PrimaryButton(string text) : this()
    {
        Text = text;
    }

    protected override void OnTextChanged(EventArgs e) { Invalidate(); base.OnTextChanged(e); }
    protected override void OnMouseEnter(EventArgs e) { _hover = true; Invalidate(); base.OnMouseEnter(e); }
    protected override void OnMouseLeave(EventArgs e) { _hover = false; _down = false; Invalidate(); base.OnMouseLeave(e); }
    protected override void OnMouseDown(MouseEventArgs e) { _down = true; Invalidate(); base.OnMouseDown(e); }
    protected override void OnMouseUp(MouseEventArgs e) { _down = false; Invalidate(); base.OnMouseUp(e); }
    protected override void OnEnabledChanged(EventArgs e) { Invalidate(); base.OnEnabledChanged(e); }

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;

        var fillColor = !Enabled ? Color.FromArgb(157, 188, 235)
            : _down ? Color.FromArgb(40, 98, 178)
            : _hover ? Color.FromArgb(47, 107, 192)
            : Theme.Primary;

        using (var path = Ui.RoundRect(new RectangleF(0, 0, Width - 1, Height - 1), 8))
        using (var b = new SolidBrush(fillColor))
            g.FillPath(b, path);

        TextRenderer.DrawText(g, Text, Font, ClientRectangle, Color.White,
            TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter |
            TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine);
    }
}
