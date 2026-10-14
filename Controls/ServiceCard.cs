using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace MyApp.Controls;

/// <summary>Selectable service tile for step 4: icon, name, price and "~ 30 mins", with a blue outline and tick when chosen.</summary>
internal sealed class ServiceCard : Control
{
    private string _price = "";
    private string _duration = "";
    private string _glyph = "\uEB51";
    private bool _selected;
    private bool _hover;

    public ServiceCard()
    {
        SetStyle(ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
        BackColor = Theme.Bg;
        Cursor = Cursors.Hand;
        Size = new Size(396, 130);
        TabStop = false;
    }

    [Category("Pawfect"), Description("Price line, e.g. P150.")]
    [DefaultValue("")]
    public string Price { get => _price; set { _price = value; Invalidate(); } }

    [Category("Pawfect"), Description("Duration line, e.g. ~ 30 mins.")]
    [DefaultValue("")]
    public string Duration { get => _duration; set { _duration = value; Invalidate(); } }

    [Category("Pawfect"), Description("Segoe MDL2 Assets icon shown above the name.")]
    public string Glyph { get => _glyph; set { _glyph = value; Invalidate(); } }

    [Category("Pawfect"), Description("Show the blue outline and tick.")]
    [DefaultValue(false)]
    public bool Selected { get => _selected; set { _selected = value; Invalidate(); } }

    protected override void OnTextChanged(EventArgs e) { Invalidate(); base.OnTextChanged(e); }
    protected override void OnMouseEnter(EventArgs e) { _hover = true; Invalidate(); base.OnMouseEnter(e); }
    protected override void OnMouseLeave(EventArgs e) { _hover = false; Invalidate(); base.OnMouseLeave(e); }

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        var blue = Color.FromArgb(37, 99, 235);

        var rect = new RectangleF(1, 1, Width - 3, Height - 3);
        using (var path = Ui.RoundRect(rect, 14))
        {
            using (var fill = new SolidBrush(_selected ? Color.FromArgb(239, 245, 255) : Color.White)) g.FillPath(fill, path);
            using var pen = new Pen(_selected ? blue : _hover ? Color.FromArgb(160, 196, 245) : Theme.Border, _selected ? 2f : 1f);
            g.DrawPath(pen, path);
        }

        const TextFormatFlags centre = TextFormatFlags.HorizontalCenter | TextFormatFlags.Top | TextFormatFlags.NoPadding |
                                       TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine | TextFormatFlags.EndEllipsis;

        TextRenderer.DrawText(g, _glyph, Theme.GlyphFont(18f), new Rectangle(0, 14, Width, 30), blue, centre);
        TextRenderer.DrawText(g, Text, Theme.Face(10f, FontStyle.Bold), new Rectangle(8, 50, Width - 16, 20), Theme.Text, centre);
        TextRenderer.DrawText(g, _price, Theme.Face(11f, FontStyle.Bold), new Rectangle(8, 72, Width - 16, 22), Theme.Text, centre);
        TextRenderer.DrawText(g, _duration, Theme.Face(8f), new Rectangle(8, 98, Width - 16, 16), Theme.Muted, centre);

        if (_selected)
        {
            var dot = new Rectangle(Width - 34, 10, 22, 22);
            using (var brush = new SolidBrush(blue)) g.FillEllipse(brush, dot);
            TextRenderer.DrawText(g, "\uE73E", Theme.GlyphFont(8f), dot, Color.White,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine);
        }
    }
}
