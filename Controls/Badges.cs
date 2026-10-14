using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace MyApp.Controls;

/// <summary>Small rounded square (or circle) holding one icon glyph.</summary>
internal sealed class IconBadge : Control
{
    private string _glyph = "";
    private Color _fill = Theme.PrimarySoft;
    private bool _circle;
    private float _pt = 10f;
    private int _radius = 8;

    public IconBadge()
    {
        SetStyle(ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
        ForeColor = Theme.Primary;
        BackColor = Color.White;
        Size = new Size(34, 34);
        TabStop = false;
    }

    public IconBadge(string glyph, Color fill, Color fore, Color back, int size,
        bool circle = false, float glyphPt = 10f, int radius = 8) : this()
    {
        _glyph = glyph;
        _fill = fill;
        _circle = circle;
        _pt = glyphPt;
        _radius = radius;
        ForeColor = fore;
        BackColor = back;
        Size = new Size(size, size);
    }

    [Category("Pawfect"), Description("Icon character from the Segoe MDL2 Assets font.")]
    [DefaultValue("")]
    public string Glyph { get => _glyph; set { _glyph = value; Invalidate(); } }

    [Category("Pawfect"), Description("Colour of the badge shape. The icon uses ForeColor.")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public Color FillColor { get => _fill; set { _fill = value; Invalidate(); } }

    [Category("Pawfect"), Description("Draw a circle instead of a rounded square.")]
    [DefaultValue(false)]
    public bool Circle { get => _circle; set { _circle = value; Invalidate(); } }

    [Category("Pawfect"), Description("Icon size in points.")]
    [DefaultValue(10f)]
    public float GlyphSize { get => _pt; set { _pt = value; Invalidate(); } }

    [Category("Pawfect"), Description("Corner roundness of the square.")]
    [DefaultValue(8)]
    public int Radius { get => _radius; set { _radius = value; Invalidate(); } }

    private Image? _photo;

    /// <summary>A picture drawn inside the badge shape instead of the glyph (used for profile photos). Set from code only.</summary>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Image? Photo
    {
        get => _photo;
        set
        {
            if (ReferenceEquals(_photo, value)) return;
            _photo?.Dispose();
            _photo = value;
            Invalidate();
        }
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing) _photo?.Dispose();
        base.Dispose(disposing);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        var r = new RectangleF(0, 0, Width - 1, Height - 1);
        using var path = Ui.RoundRect(r, _circle ? Math.Min(Width, Height) : _radius);
        using var b = new SolidBrush(_fill);
        g.FillPath(b, path);

        if (_photo is not null)
        {
            // Fill the shape with the picture (centre-cropped), clipped to the circle / rounded square.
            var state = g.Save();
            g.SetClip(path);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            float scale = Math.Max((float)Width / _photo.Width, (float)Height / _photo.Height);
            float w = _photo.Width * scale, h = _photo.Height * scale;
            g.DrawImage(_photo, (Width - w) / 2f, (Height - h) / 2f, w, h);
            g.Restore(state);
            return;
        }

        TextRenderer.DrawText(g, _glyph, Theme.GlyphFont(_pt), ClientRectangle, ForeColor,
            TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter |
            TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine);
    }
}

/// <summary>The Pawfect paw logo drawn with GDI+ (no image files needed).</summary>
internal sealed class PawBadge : Control
{
    private Color _fill = Theme.PrimarySoft;
    private Color _paw = Theme.Primary;
    private int _radius = 10;

    public PawBadge()
    {
        SetStyle(ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw
                 | ControlStyles.SupportsTransparentBackColor, true);
        BackColor = Color.White;
        Size = new Size(44, 44);
        TabStop = false;
    }

    public PawBadge(Color fill, Color paw, Color back, int size, int radius = 10) : this()
    {
        _fill = fill;
        _paw = paw;
        _radius = radius;
        BackColor = back;
        Size = new Size(size, size);
    }

    [Category("Pawfect"), Description("Colour of the rounded square behind the paw.")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public Color FillColor { get => _fill; set { _fill = value; Invalidate(); } }

    [Category("Pawfect"), Description("Colour of the paw.")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public Color PawColor { get => _paw; set { _paw = value; Invalidate(); } }

    [Category("Pawfect"), Description("Corner roundness of the square.")]
    [DefaultValue(10)]
    public int Radius { get => _radius; set { _radius = value; Invalidate(); } }

    protected override void OnPaint(PaintEventArgs e) => Draw(e.Graphics, Width, Height, _fill, _paw, _radius);

    /// <summary>Draws the logo (rounded square + paw) into a <paramref name="width"/> x <paramref name="height"/> area.</summary>
    internal static void Draw(Graphics g, int width, int height, Color fill, Color pawColor, float radius)
    {
        g.SmoothingMode = SmoothingMode.AntiAlias;

        using (var path = Ui.RoundRect(new RectangleF(0, 0, width - 1, height - 1), radius))
        using (var bg = new SolidBrush(fill))
            g.FillPath(bg, path);

        float s = Math.Min(width, height) * 0.58f;
        float ox = (width - s) / 2f;
        float oy = (height - s) / 2f + s * 0.03f;
        using var brush = new SolidBrush(pawColor);

        void Blob(float cx, float cy, float w, float h)
            => g.FillEllipse(brush, ox + (cx - w / 2f) * s, oy + (cy - h / 2f) * s, w * s, h * s);

        Blob(0.50f, 0.70f, 0.52f, 0.42f); // pad
        Blob(0.12f, 0.43f, 0.20f, 0.26f); // toes
        Blob(0.36f, 0.20f, 0.20f, 0.28f);
        Blob(0.64f, 0.20f, 0.20f, 0.28f);
        Blob(0.88f, 0.43f, 0.20f, 0.26f);
    }

    /// <summary>The logo as a transparent-cornered PNG (used inline in e-mails, where emoji and web fonts are unreliable).</summary>
    internal static byte[] RenderPng(int pixels, Color fill, Color pawColor, float radius)
    {
        using var bitmap = new Bitmap(pixels, pixels);
        using (var g = Graphics.FromImage(bitmap))
        {
            g.Clear(Color.Transparent);
            Draw(g, pixels, pixels, fill, pawColor, radius);
        }
        using var output = new MemoryStream();
        bitmap.Save(output, System.Drawing.Imaging.ImageFormat.Png);
        return output.ToArray();
    }
}

/// <summary>Coloured legend dot.</summary>
internal sealed class Dot : Control
{
    private Color _color = Theme.Primary;

    public Dot()
    {
        SetStyle(ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
        BackColor = Color.White;
        Width = 18;
        TabStop = false;
    }

    public Dot(Color color, Color back) : this()
    {
        _color = color;
        BackColor = back;
    }

    [Category("Pawfect"), Description("Colour of the dot.")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public Color DotColor { get => _color; set { _color = value; Invalidate(); } }

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        const float d = 9f;
        using var b = new SolidBrush(_color);
        g.FillEllipse(b, (Width - d) / 2f, (Height - d) / 2f, d, d);
    }
}