using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace MyApp.Controls;

/// <summary>
/// Soft blue "blob" used behind the pets on the welcome screen: big rounded top corners, flat bottom and right
/// edge so it sits flush against the form. Drop controls (e.g. a PictureBox) on it like on any Panel.
/// </summary>
internal class BlobPanel : Panel
{
    private Color _top = Color.FromArgb(240, 246, 255);
    private Color _bottom = Color.FromArgb(229, 241, 254);
    private Color _corner = Theme.Bg;
    private Color _accent = Color.FromArgb(96, 165, 250);
    private int _tl = 120;
    private int _tr = 80;
    private bool _decor = true;

    public BlobPanel()
    {
        DoubleBuffered = true;
        ResizeRedraw = true;
        Margin = Padding.Empty;
    }

    [Category("Pawfect"), Description("Gradient colour at the top of the blob.")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public Color FillTop { get => _top; set { _top = value; Invalidate(); } }

    [Category("Pawfect"), Description("Gradient colour at the bottom of the blob.")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public Color FillBottom { get => _bottom; set { _bottom = value; Invalidate(); } }

    [Category("Pawfect"), Description("Colour of whatever the blob sits on (shows in the cut-out corners).")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public Color CornerColor { get => _corner; set { _corner = value; Invalidate(); } }

    [Category("Pawfect"), Description("Colour of the little heart and sparkle marks.")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public Color AccentColor { get => _accent; set { _accent = value; Invalidate(); } }

    [Category("Pawfect"), Description("Roundness of the top-left corner.")]
    [DefaultValue(120)]
    public int TopLeftRadius { get => _tl; set { _tl = Math.Max(0, value); Invalidate(); } }

    [Category("Pawfect"), Description("Roundness of the top-right corner.")]
    [DefaultValue(80)]
    public int TopRightRadius { get => _tr; set { _tr = Math.Max(0, value); Invalidate(); } }

    [Category("Pawfect"), Description("Draw the heart and sparkle marks.")]
    [DefaultValue(true)]
    public bool ShowDecor { get => _decor; set { _decor = value; Invalidate(); } }

    protected override void OnPaintBackground(PaintEventArgs e)
    {
        using var b = new SolidBrush(_corner);
        e.Graphics.FillRectangle(b, ClientRectangle);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        if (Width < 4 || Height < 4) return;

        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;

        float tl = Math.Min(_tl, Math.Min(Width, Height) / 2f);
        float tr = Math.Min(_tr, Math.Min(Width, Height) / 2f);

        using var path = new GraphicsPath();
        if (tl > 0) path.AddArc(0, 0, tl * 2, tl * 2, 180, 90); else path.AddLine(0, 0, 0, 0);
        if (tr > 0) path.AddArc(Width - tr * 2, 0, tr * 2, tr * 2, 270, 90); else path.AddLine(Width, 0, Width, 0);
        path.AddLine(Width, tr, Width, Height);
        path.AddLine(Width, Height, 0, Height);
        path.CloseFigure();

        using (var fill = new LinearGradientBrush(new Rectangle(0, 0, Width, Height), _top, _bottom, 90f))
            g.FillPath(fill, path);

        if (!_decor) return;

        using var pen = new Pen(_accent, 1.8f) { StartCap = LineCap.Round, EndCap = LineCap.Round, LineJoin = LineJoin.Round };
        using (var heart = Heart(Width * 0.59f, Height * 0.07f, 24))
            g.DrawPath(pen, heart);
        Spark(g, pen, Width * 0.25f, Height * 0.10f, 0);
        Spark(g, pen, Width * 0.74f, Height * 0.13f, 180);
    }

    private static GraphicsPath Heart(float x, float y, float w)
    {
        float h = w * 0.9f;
        var p = new GraphicsPath();
        p.AddBezier(x + w / 2, y + h, x - w * 0.15f, y + h * 0.45f, x + w * 0.15f, y - h * 0.15f, x + w / 2, y + h * 0.28f);
        p.AddBezier(x + w / 2, y + h * 0.28f, x + w * 0.85f, y - h * 0.15f, x + w * 1.15f, y + h * 0.45f, x + w / 2, y + h);
        p.CloseFigure();
        return p;
    }

    private static void Spark(Graphics g, Pen pen, float x, float y, float angle)
    {
        var state = g.Save();
        g.TranslateTransform(x, y);
        g.RotateTransform(angle);
        g.DrawLine(pen, 0, -6, 9, -3);
        g.DrawLine(pen, 0, 0, 12, 0);
        g.DrawLine(pen, 0, 6, 9, 3);
        g.Restore(state);
    }
}
