using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace MyApp.Controls;

/// <summary>
/// Full-form background for the login screen: soft blue gradient, faint paw prints in the corners and an optional
/// drop shadow under one child (set <see cref="ShadowTarget"/> to the card). Dock it to Fill and put the screen's
/// controls on it; give them a Transparent BackColor so the gradient shows through.
/// </summary>
internal class BackdropPanel : Panel
{
    private Color _top = Color.FromArgb(242, 248, 255);
    private Color _bottom = Color.FromArgb(225, 237, 254);
    private Color _paw = Color.FromArgb(205, 224, 250);
    private bool _showPaws = true;
    private Control? _shadow;

    // x/y as a fraction of the panel, size in pixels, tilt in degrees.
    private static readonly (float X, float Y, float Size, float Rot)[] Paws =
    {
        (0.07f, 0.09f, 62f, -15f),
        (0.93f, 0.09f, 66f, 15f),
        (0.09f, 0.92f, 72f, -20f),
        (0.92f, 0.93f, 58f, 10f),
    };

    public BackdropPanel()
    {
        DoubleBuffered = true;
        ResizeRedraw = true;
        Margin = Padding.Empty;
    }

    [Category("Pawfect"), Description("Gradient colour at the top-left.")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public Color GradientStart { get => _top; set { _top = value; Invalidate(); } }

    [Category("Pawfect"), Description("Gradient colour at the bottom-right.")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public Color GradientEnd { get => _bottom; set { _bottom = value; Invalidate(); } }

    [Category("Pawfect"), Description("Colour of the faint corner paw prints.")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public Color PawColor { get => _paw; set { _paw = value; Invalidate(); } }

    [Category("Pawfect"), Description("Draw the paw prints in the corners.")]
    [DefaultValue(true)]
    public bool ShowPaws { get => _showPaws; set { _showPaws = value; Invalidate(); } }

    [Category("Pawfect"), Description("Child control (usually the card) that gets a soft drop shadow.")]
    [DefaultValue(null)]
    public Control? ShadowTarget
    {
        get => _shadow;
        set
        {
            if (_shadow == value) return;
            if (_shadow != null)
            {
                _shadow.LocationChanged -= OnTargetMoved;
                _shadow.SizeChanged -= OnTargetMoved;
            }
            _shadow = value;
            if (_shadow != null)
            {
                _shadow.LocationChanged += OnTargetMoved;
                _shadow.SizeChanged += OnTargetMoved;
            }
            Invalidate();
        }
    }

    private void OnTargetMoved(object? sender, EventArgs e) => Invalidate();

    protected override void OnPaintBackground(PaintEventArgs e)
    {
        var g = e.Graphics;
        var r = ClientRectangle;
        if (r.Width <= 0 || r.Height <= 0) return;

        using (var br = new LinearGradientBrush(r, _top, _bottom, 35f))
            g.FillRectangle(br, r);

        g.SmoothingMode = SmoothingMode.AntiAlias;

        if (_showPaws)
        {
            using var brush = new SolidBrush(_paw);
            foreach (var p in Paws)
                DrawPaw(g, brush, Width * p.X, Height * p.Y, p.Size, p.Rot);
        }

        if (_shadow != null && _shadow.Parent == this)
        {
            var b = _shadow.Bounds;
            using var shade = new SolidBrush(Color.FromArgb(5, 40, 90, 170));
            for (int i = 14; i >= 1; i--)
            {
                using var path = Ui.RoundRect(new RectangleF(b.X - i, b.Y - i + 12, b.Width + 2 * i, b.Height + 2 * i), 22 + i);
                g.FillPath(shade, path);
            }
        }
    }

    private static void DrawPaw(Graphics g, Brush brush, float cx, float cy, float s, float rot)
    {
        var state = g.Save();
        g.TranslateTransform(cx, cy);
        g.RotateTransform(rot);
        float ox = -s / 2f, oy = -s / 2f;

        void Blob(float x, float y, float w, float h)
            => g.FillEllipse(brush, ox + (x - w / 2f) * s, oy + (y - h / 2f) * s, w * s, h * s);

        Blob(0.50f, 0.70f, 0.52f, 0.42f); // pad
        Blob(0.12f, 0.43f, 0.20f, 0.26f); // toes
        Blob(0.36f, 0.20f, 0.20f, 0.28f);
        Blob(0.64f, 0.20f, 0.20f, 0.28f);
        Blob(0.88f, 0.43f, 0.20f, 0.26f);
        g.Restore(state);
    }
}
