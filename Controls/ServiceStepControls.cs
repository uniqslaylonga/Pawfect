using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Text.RegularExpressions;

namespace MyApp.Controls;

/// <summary>
/// Shared look of the redesigned step 4 (Service): colours, pixel-sized fonts and the line icons.
/// Fonts are made in PIXELS (not points) from the control's own size, so the text grows with the page by the same factor
/// on a 100 % screen (1920 x 1080) and on a 125 % screen (1536 x 864).
/// </summary>
internal static class SvcLook
{
    public static readonly Color Blue = Color.FromArgb(37, 99, 235);
    public static readonly Color BlueSoft = Color.FromArgb(239, 245, 255);
    public static readonly Color BlueHover = Color.FromArgb(160, 196, 245);
    public static readonly Color InfoFill = Color.FromArgb(234, 241, 254);
    public static readonly Color InfoText = Color.FromArgb(30, 64, 175);
    public static readonly Color Navy = Color.FromArgb(27, 38, 70);
    public static readonly Color Line = Color.FromArgb(226, 232, 242);
    public static readonly Color Muted = Color.FromArgb(120, 133, 156);

    private static readonly Dictionary<(int, FontStyle), Font> Fonts = new();

    /// <summary>Segoe UI sized in pixels (cached, never disposed - same idea as <see cref="Theme.Face"/>).</summary>
    public static Font Px(float px, FontStyle style = FontStyle.Regular)
    {
        int key = Math.Max(4, (int)Math.Round(px * 4f));   // quarter-pixel steps keep the cache small
        if (!Fonts.TryGetValue((key, style), out var font))
        {
            font = new Font("Segoe UI", key / 4f, style, GraphicsUnit.Pixel);
            Fonts[(key, style)] = font;
        }
        return font;
    }

    public const TextFormatFlags Centre = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding |
                                          TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine;

    // ------------------------------------------------------------------ the small paw used for the logo and the watermark

    public static void Paw(Graphics g, float x, float y, float size, Color colour)
    {
        using var brush = new SolidBrush(colour);
        void Blob(float cx, float cy, float w, float h)
            => g.FillEllipse(brush, x + (cx - w / 2f) * size, y + (cy - h / 2f) * size, w * size, h * size);
        Blob(0.50f, 0.70f, 0.52f, 0.42f);
        Blob(0.12f, 0.43f, 0.20f, 0.26f);
        Blob(0.36f, 0.20f, 0.20f, 0.28f);
        Blob(0.64f, 0.20f, 0.20f, 0.28f);
        Blob(0.88f, 0.43f, 0.20f, 0.26f);
    }

    // ------------------------------------------------------------------ line icons (24 x 24 units, drawn with a round pen)

    /// <summary>Picks an icon from the service name (names come from dbo.Services; anything unknown gets a paw).</summary>
    public static string IconKeyFor(string name)
    {
        string n = (name ?? "").ToLowerInvariant();
        if (n.Contains("flea") || n.Contains("tick")) return "bug";
        if (n.Contains("fragrance") || n.Contains("perfume") || n.Contains("cologne")) return "bottle";
        if (Regex.IsMatch(n, @"\bspa\b")) return "spa";
        if (n.Contains("paw pad") || n.Contains("paw care")) return "pawcare";
        if (n.Contains("nail")) return "paw";
        if (Regex.IsMatch(n, @"\bears?\b")) return "ear";
        if (n.Contains("teeth") || n.Contains("tooth") || n.Contains("dental")) return "tooth";
        if (n.Contains("shed")) return "brush";
        if (Regex.IsMatch(n, @"\bcats?\b")) return "cat";
        if (n.Contains("haircut") || n.Contains("hair cut") || n.Contains("trim")) return "comb";
        if (n.Contains("bath") || n.Contains("wash") || n.Contains("shampoo")) return "tub";
        if (n.Contains("groom")) return "scissors";
        return "paw";
    }

    /// <summary>Draws the icon for <paramref name="key"/> inside <paramref name="box"/> with a stroke of <paramref name="stroke"/> pixels.</summary>
    public static void DrawIcon(Graphics g, string key, RectangleF box, Color colour, float stroke)
    {
        var state = g.Save();
        var oldSmoothing = g.SmoothingMode;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        float s = box.Width / 24f;
        g.TranslateTransform(box.X, box.Y);
        g.ScaleTransform(s, s);

        using var pen = new Pen(colour, Math.Max(0.9f, stroke / s)) { StartCap = LineCap.Round, EndCap = LineCap.Round, LineJoin = LineJoin.Round };
        using var fill = new SolidBrush(colour);

        void Line(float x1, float y1, float x2, float y2) => g.DrawLine(pen, x1, y1, x2, y2);
        void Oval(float cx, float cy, float rx, float ry) => g.DrawEllipse(pen, cx - rx, cy - ry, rx * 2, ry * 2);
        void Curve(params float[] p)   // p = x0 y0, then (c1x c1y c2x c2y x y) groups
        {
            using var path = new GraphicsPath();
            float cx = p[0], cy = p[1];
            for (int i = 2; i + 5 < p.Length; i += 6)
            {
                path.AddBezier(cx, cy, p[i], p[i + 1], p[i + 2], p[i + 3], p[i + 4], p[i + 5]);
                cx = p[i + 4]; cy = p[i + 5];
            }
            g.DrawPath(pen, path);
        }
        void PawOutline()
        {
            Oval(12f, 16.2f, 4.6f, 3.6f);
            Oval(5.4f, 11.3f, 1.7f, 2.3f);
            Oval(9.3f, 7.2f, 1.8f, 2.4f);
            Oval(14.7f, 7.2f, 1.8f, 2.4f);
            Oval(18.6f, 11.3f, 1.7f, 2.3f);
        }

        switch (key)
        {
            case "tub":
                Line(3f, 11.5f, 21f, 11.5f);
                Curve(5f, 11.5f, 5f, 14f, 5f, 18.2f, 8.4f, 18.2f);
                Line(8.4f, 18.2f, 15.6f, 18.2f);
                Curve(15.6f, 18.2f, 19f, 18.2f, 19f, 14f, 19f, 11.5f);
                Line(8f, 18.4f, 7f, 20.5f);
                Line(16f, 18.4f, 17f, 20.5f);
                Oval(9f, 7.6f, 1.7f, 1.7f);
                Oval(13.6f, 5.4f, 1.2f, 1.2f);
                Oval(15.4f, 8.4f, 0.8f, 0.8f);
                break;

            case "scissors":
                Oval(6f, 6.5f, 2.6f, 2.6f);
                Oval(6f, 17.5f, 2.6f, 2.6f);
                Line(8f, 8.2f, 20.5f, 17.8f);
                Line(8f, 15.8f, 20.5f, 6.2f);
                break;

            case "paw":
                PawOutline();
                break;

            case "pawcare":
                PawOutline();
                Line(19.8f, 2.6f, 19.8f, 5.6f);
                Line(18.3f, 4.1f, 21.3f, 4.1f);
                break;

            case "ear":
                Curve(7.5f, 9.5f, 7.5f, 5.5f, 10f, 3.5f, 13f, 3.5f,
                      16.5f, 3.5f, 18.5f, 6f, 18.5f, 9f,
                      18.5f, 12.5f, 15.5f, 13.5f, 14.5f, 16f,
                      13.8f, 18f, 13f, 20.5f, 10.5f, 20.5f,
                      8.5f, 20.5f, 7.5f, 19f, 7.5f, 17.5f);
                Curve(10.5f, 9.5f, 10.5f, 7.8f, 11.5f, 6.8f, 13f, 6.8f,
                      14.5f, 6.8f, 15.5f, 7.8f, 15.5f, 9f,
                      15.5f, 11f, 12.8f, 11.5f, 12.8f, 14f);
                break;

            case "comb":
                using (var bar = Ui.RoundRect(new RectangleF(3f, 6.5f, 18f, 5.5f), 2.2f)) g.DrawPath(pen, bar);
                foreach (float x in new[] { 5.6f, 8.8f, 12f, 15.2f, 18.4f }) Line(x, 12f, x, 17.5f);
                break;

            case "brush":
                using (var head = Ui.RoundRect(new RectangleF(7f, 3f, 10f, 10f), 3.2f)) g.DrawPath(pen, head);
                Line(10.2f, 6.2f, 10.2f, 9.8f);
                Line(13.8f, 6.2f, 13.8f, 9.8f);
                Line(12f, 13f, 12f, 20.8f);
                break;

            case "tooth":
                Curve(7f, 4.5f, 9f, 3.5f, 10.5f, 5f, 12f, 5f,
                      13.5f, 5f, 15f, 3.5f, 17f, 4.5f,
                      19.5f, 5.8f, 19f, 9.5f, 18f, 12f,
                      17f, 14.5f, 17f, 20.5f, 14.8f, 20.5f,
                      13f, 20.5f, 13.3f, 16f, 12f, 16f,
                      10.7f, 16f, 11f, 20.5f, 9.2f, 20.5f,
                      7f, 20.5f, 7f, 14.5f, 6f, 12f,
                      5f, 9.5f, 4.5f, 5.8f, 7f, 4.5f);
                break;

            case "bug":
                Oval(12f, 14f, 4.5f, 5.5f);
                Oval(12f, 6.6f, 2.4f, 2.4f);
                Line(11f, 4.4f, 9.4f, 2.4f);
                Line(13f, 4.4f, 14.6f, 2.4f);
                Line(7.6f, 12f, 3.6f, 10f);
                Line(7.5f, 15f, 3.2f, 15f);
                Line(8.2f, 18f, 4.8f, 20.6f);
                Line(16.4f, 12f, 20.4f, 10f);
                Line(16.5f, 15f, 20.8f, 15f);
                Line(15.8f, 18f, 19.2f, 20.6f);
                break;

            case "bottle":
                using (var body = Ui.RoundRect(new RectangleF(6.5f, 10.5f, 11f, 10f), 2.6f)) g.DrawPath(pen, body);
                Line(10f, 10.5f, 10f, 7.8f);
                Line(14f, 10.5f, 14f, 7.8f);
                Oval(12f, 5.6f, 2.2f, 2.2f);
                Oval(12f, 15.5f, 1.7f, 1.7f);
                break;

            case "spa":
                Curve(12f, 4.5f, 15.8f, 8.5f, 15.8f, 13f, 12f, 17f);
                Curve(12f, 4.5f, 8.2f, 8.5f, 8.2f, 13f, 12f, 17f);
                Curve(12f, 17f, 7f, 16.5f, 4.2f, 13.5f, 3.6f, 9.5f);
                Curve(3.6f, 9.5f, 8.5f, 9.5f, 11f, 12f, 12f, 17f);
                Curve(12f, 17f, 17f, 16.5f, 19.8f, 13.5f, 20.4f, 9.5f);
                Curve(20.4f, 9.5f, 15.5f, 9.5f, 13f, 12f, 12f, 17f);
                Line(5.5f, 20.2f, 18.5f, 20.2f);
                break;

            case "cat":
                Curve(5f, 8f, 5.2f, 6f, 5.4f, 5f, 5.6f, 3.6f);
                Curve(5.6f, 3.6f, 7f, 4.4f, 8.4f, 5.4f, 9.6f, 6.2f);
                Curve(9.6f, 6.2f, 11.2f, 5.7f, 12.8f, 5.7f, 14.4f, 6.2f);
                Curve(14.4f, 6.2f, 15.6f, 5.4f, 17f, 4.4f, 18.4f, 3.6f);
                Curve(18.4f, 3.6f, 18.6f, 5f, 18.8f, 6f, 19f, 8f);
                Curve(19f, 8f, 20.4f, 10.4f, 20.2f, 13.2f, 19.4f, 15f);
                Curve(19.4f, 15f, 18f, 18.8f, 15f, 19.8f, 12f, 19.8f);
                Curve(12f, 19.8f, 9f, 19.8f, 6f, 18.8f, 4.6f, 15f);
                Curve(4.6f, 15f, 3.8f, 13.2f, 3.6f, 10.4f, 5f, 8f);
                g.FillEllipse(fill, 8.1f, 11f, 2f, 2.4f);
                g.FillEllipse(fill, 13.9f, 11f, 2f, 2.4f);
                g.FillPolygon(fill, new[] { new PointF(10.9f, 14.6f), new PointF(13.1f, 14.6f), new PointF(12f, 15.9f) });
                break;

            default:
                PawOutline();
                break;
        }

        g.Restore(state);
        g.SmoothingMode = oldSmoothing;
    }
}

/// <summary>
/// The whole page behind step 4, painted in one go: the white rounded card with its soft shadow, the Pawfect header with the
/// five-step bar, the two column titles, the divider and the schedule box. Real controls (service cards, day cards, buttons,
/// info bars) sit on top of it. Laid out for a 900 x 540 page; everything painted here grows with the control.
/// </summary>
internal sealed class ServiceStepCanvas : Panel
{
    public const int DesignWidth = 900, DesignHeight = 540;

    private string[] _stepNames = { "Contact", "Select Pet", "Vaccine Check", "Service", "Ticket" };
    private int _activeStep = 3;   // 0-based

    public ServiceStepCanvas()
    {
        SetStyle(ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw, true);
        BackColor = Theme.Bg;
        TabStop = false;
        Size = new Size(DesignWidth, DesignHeight);
    }

    /// <summary>Labels under the five step circles (taken from the form's own step bar, so they never drift apart).</summary>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string[] StepNames { get => _stepNames; set { _stepNames = value; Invalidate(); } }

    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public int ActiveStep { get => _activeStep; set { _activeStep = value; Invalidate(); } }

    /// <summary>Title of the left column, e.g. "Choose a Grooming Service".</summary>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string LeftTitle { get; set; } = "Choose a Grooming Service";

    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string LeftSubtitle { get; set; } = "Select a service package for your pet.";

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

        float k = Math.Min(Width / (float)DesignWidth, Height / (float)DesignHeight);
        float S(float v) => v * k;
        Font F(float px, FontStyle st = FontStyle.Regular) => SvcLook.Px(px * k, st);
        const TextFormatFlags left = TextFormatFlags.Left | TextFormatFlags.Top | TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine;
        const TextFormatFlags leftWrap = TextFormatFlags.Left | TextFormatFlags.Top | TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix | TextFormatFlags.WordBreak;

        // ---- the white card, with a soft shadow
        var card = new RectangleF(S(18), S(16), S(864), S(508));
        for (int i = 6; i >= 1; i--)
        {
            var r = new RectangleF(card.X - S(i), card.Y - S(i) + S(3), card.Width + S(i * 2), card.Height + S(i * 2));
            using var shade = new SolidBrush(Color.FromArgb(5, 30, 60, 120));
            using var sp = Ui.RoundRect(r, S(18 + i));
            g.FillPath(shade, sp);
        }
        using (var cp = Ui.RoundRect(card, S(18)))
        using (var white = new SolidBrush(Color.White))
            g.FillPath(white, cp);

        // ---- header: paw + name + tagline
        SvcLook.Paw(g, S(46), S(32), S(21), SvcLook.Blue);
        TextRenderer.DrawText(g, "Pawfect", F(17, FontStyle.Bold), new Rectangle((int)S(76), (int)S(29), (int)S(160), (int)S(22)), SvcLook.Navy, left);
        TextRenderer.DrawText(g, "Grooming \u2022 Health \u2022 Happiness", F(9), new Rectangle((int)S(76), (int)S(51), (int)S(220), (int)S(14)), SvcLook.Muted, left);

        // ---- header: the five steps (circles joined by thin lines, the current one filled blue)
        float[] cx = { 600, 662, 724, 786, 848 };
        float cy = 42, d = 20;
        using (var linePen = new Pen(SvcLook.Line, Math.Max(1f, S(1f))))
        {
            for (int i = 0; i < 4; i++)
                g.DrawLine(linePen, S(cx[i] + d / 2 + 5), S(cy), S(cx[i + 1] - d / 2 - 5), S(cy));
        }
        for (int i = 0; i < 5; i++)
        {
            bool active = i == _activeStep;
            var circle = new RectangleF(S(cx[i] - d / 2), S(cy - d / 2), S(d), S(d));
            if (active)
            {
                using var b = new SolidBrush(SvcLook.Blue);
                g.FillEllipse(b, circle);
            }
            else
            {
                using var b = new SolidBrush(Color.White);
                using var p = new Pen(SvcLook.Line, Math.Max(1f, S(1f)));
                g.FillEllipse(b, circle);
                g.DrawEllipse(p, circle);
            }
            TextRenderer.DrawText(g, (i + 1).ToString(), F(10, FontStyle.Bold), Rectangle.Round(circle),
                active ? Color.White : SvcLook.Muted, SvcLook.Centre);

            string label = i < _stepNames.Length ? _stepNames[i] : "";
            TextRenderer.DrawText(g, label, F(9.5f, active ? FontStyle.Bold : FontStyle.Regular),
                new Rectangle((int)S(cx[i] - 36), (int)S(cy + 14), (int)S(72), (int)S(14)),
                active ? SvcLook.Blue : SvcLook.Muted,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.Top | TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine);
        }

        // ---- thin rule under the header
        using (var rule = new Pen(Color.FromArgb(236, 240, 247), Math.Max(1f, S(1f))))
            g.DrawLine(rule, S(46), S(82), S(854), S(82));

        // ---- left column titles
        TextRenderer.DrawText(g, LeftTitle, F(20, FontStyle.Bold), new Rectangle((int)S(46), (int)S(94), (int)S(470), (int)S(28)), SvcLook.Navy, left);
        TextRenderer.DrawText(g, LeftSubtitle, F(10), new Rectangle((int)S(46), (int)S(124), (int)S(470), (int)S(16)), SvcLook.Muted, left);

        // ---- vertical divider between the two columns
        using (var rule = new Pen(Color.FromArgb(236, 240, 247), Math.Max(1f, S(1f))))
            g.DrawLine(rule, S(536), S(98), S(536), S(504));

        // ---- right column: faint paw watermark, calendar tile, title, subtitle
        SvcLook.Paw(g, S(822), S(96), S(26), Color.FromArgb(226, 233, 245));

        var tile = new RectangleF(S(560), S(96), S(34), S(34));
        using (var tp = Ui.RoundRect(tile, S(9)))
        using (var tb = new SolidBrush(Color.FromArgb(232, 240, 254)))
            g.FillPath(tb, tp);
        DrawCalendar(g, new RectangleF(tile.X + S(8), tile.Y + S(8), S(18), S(18)), S(1.5f));

        TextRenderer.DrawText(g, "Select a Schedule", F(17, FontStyle.Bold), new Rectangle((int)S(604), (int)S(94), (int)S(220), (int)S(24)), SvcLook.Navy, left);
        TextRenderer.DrawText(g, "Choose a day when you are available. Pick at least one day that you are sure is free.", F(9.5f),
            new Rectangle((int)S(604), (int)S(118), (int)S(240), (int)S(30)), SvcLook.Muted, leftWrap);

        // ---- the white box that holds the seven day cards
        var box = new RectangleF(S(560), S(156), S(294), S(172));
        using (var bp = Ui.RoundRect(box, S(14)))
        using (var bb = new SolidBrush(Color.White))
        using (var bpen = new Pen(Color.FromArgb(228, 234, 244), Math.Max(1f, S(1f))))
        {
            g.FillPath(bb, bp);
            g.DrawPath(bpen, bp);
        }
    }

    private static void DrawCalendar(Graphics g, RectangleF box, float stroke)
    {
        using var pen = new Pen(SvcLook.Blue, Math.Max(1f, stroke)) { StartCap = LineCap.Round, EndCap = LineCap.Round, LineJoin = LineJoin.Round };
        float u = box.Width / 18f;
        using (var body = Ui.RoundRect(new RectangleF(box.X + u, box.Y + 2.5f * u, 16 * u, 14.5f * u), 3f * u)) g.DrawPath(pen, body);
        g.DrawLine(pen, box.X + u, box.Y + 7f * u, box.X + 17f * u, box.Y + 7f * u);
        g.DrawLine(pen, box.X + 5.5f * u, box.Y + 0.5f * u, box.X + 5.5f * u, box.Y + 4.5f * u);
        g.DrawLine(pen, box.X + 12.5f * u, box.Y + 0.5f * u, box.X + 12.5f * u, box.Y + 4.5f * u);
        using var dot = new SolidBrush(SvcLook.Blue);
        g.FillEllipse(dot, box.X + 5f * u, box.Y + 10f * u, 1.8f * u, 1.8f * u);
        g.FillEllipse(dot, box.X + 8.1f * u, box.Y + 10f * u, 1.8f * u, 1.8f * u);
        g.FillEllipse(dot, box.X + 11.2f * u, box.Y + 10f * u, 1.8f * u, 1.8f * u);
    }
}

/// <summary>One day of the week on the schedule side: "Mon" over "Monday", a blue outline and tick when picked.</summary>
internal sealed class DayCard : Control
{
    private string _longName = "";
    private bool _checked;
    private bool _hover;

    public DayCard()
    {
        SetStyle(ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint, true);
        BackColor = Color.White;
        Cursor = Cursors.Hand;
        Size = new Size(60, 42);
        TabStop = false;
    }

    public event EventHandler? CheckedChanged;

    /// <summary>Short name shown big ("Mon") is the control's Text; this is the small line under it ("Monday").</summary>
    [Category("Pawfect"), DefaultValue("")]
    public string LongName { get => _longName; set { _longName = value; Invalidate(); } }

    [Category("Pawfect"), DefaultValue(false)]
    public bool Checked
    {
        get => _checked;
        set
        {
            if (_checked == value) return;
            _checked = value;
            Invalidate();
            CheckedChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    protected override void OnClick(EventArgs e) { Checked = !Checked; base.OnClick(e); }
    protected override void OnTextChanged(EventArgs e) { Invalidate(); base.OnTextChanged(e); }
    protected override void OnMouseEnter(EventArgs e) { _hover = true; Invalidate(); base.OnMouseEnter(e); }
    protected override void OnMouseLeave(EventArgs e) { _hover = false; Invalidate(); base.OnMouseLeave(e); }

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.Clear(BackColor);

        float k = Math.Max(0.5f, Math.Min(Width / 60f, Height / 42f));   // laid out for a 60 x 42 tile
        float S(float v) => v * k;

        var rect = new RectangleF(1, 1, Width - 3, Height - 3);
        using (var path = Ui.RoundRect(rect, S(9)))
        {
            using (var fill = new SolidBrush(_checked ? Color.FromArgb(247, 250, 255) : Color.White)) g.FillPath(fill, path);
            using var pen = new Pen(_checked ? SvcLook.Blue : _hover ? SvcLook.BlueHover : Color.FromArgb(223, 229, 240), _checked ? Math.Max(1.5f, S(1.6f)) : Math.Max(1f, S(1f)));
            g.DrawPath(pen, path);
        }

        TextRenderer.DrawText(g, Text, SvcLook.Px(S(11.5f), FontStyle.Bold), new Rectangle(0, (int)S(9), Width, (int)S(15)), SvcLook.Navy,
            TextFormatFlags.HorizontalCenter | TextFormatFlags.Top | TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine);
        TextRenderer.DrawText(g, _longName, SvcLook.Px(S(8.5f)), new Rectangle(0, (int)S(25), Width, (int)S(12)), SvcLook.Muted,
            TextFormatFlags.HorizontalCenter | TextFormatFlags.Top | TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine);

        // tick circle in the top-right corner (empty ring while not picked)
        var dot = new RectangleF(Width - S(15), S(5), S(10), S(10));
        if (_checked)
        {
            using var b = new SolidBrush(SvcLook.Blue);
            g.FillEllipse(b, dot);
            using var tick = new Pen(Color.White, Math.Max(1f, S(1.3f))) { StartCap = LineCap.Round, EndCap = LineCap.Round, LineJoin = LineJoin.Round };
            g.DrawLines(tick, new[]
            {
                new PointF(dot.X + dot.Width * 0.27f, dot.Y + dot.Height * 0.52f),
                new PointF(dot.X + dot.Width * 0.44f, dot.Y + dot.Height * 0.69f),
                new PointF(dot.X + dot.Width * 0.74f, dot.Y + dot.Height * 0.32f),
            });
        }
        else
        {
            using var ring = new Pen(Color.FromArgb(205, 213, 226), Math.Max(1f, S(1f)));
            g.DrawEllipse(ring, dot);
        }
    }
}

/// <summary>Pale-blue note with an (i) icon: one or two lines of text.</summary>
internal sealed class InfoBar : Control
{
    private string _detail = "";
    private bool _emphasis;

    public InfoBar()
    {
        SetStyle(ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint, true);
        BackColor = Color.White;
        Size = new Size(264, 26);
        TabStop = false;
    }

    /// <summary>The design size this bar was laid out for; the text and icon scale with the real size relative to it.</summary>
    [Category("Pawfect"), DefaultValue(26)]
    public int DesignHeight { get; set; } = 26;

    [Category("Pawfect"), DefaultValue(264)]
    public int DesignWidth { get; set; } = 264;

    /// <summary>Second, lighter line (optional).</summary>
    [Category("Pawfect"), DefaultValue("")]
    public string Detail { get => _detail; set { _detail = value ?? ""; Invalidate(); } }

    /// <summary>First line in medium weight (used for "Selected at least one day.").</summary>
    [Category("Pawfect"), DefaultValue(false)]
    public bool Emphasis { get => _emphasis; set { _emphasis = value; Invalidate(); } }

    protected override void OnTextChanged(EventArgs e) { Invalidate(); base.OnTextChanged(e); }

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.Clear(BackColor);

        float k = Math.Max(0.5f, Math.Min(Width / (float)DesignWidth, Height / (float)DesignHeight));
        float S(float v) => v * k;

        using (var path = Ui.RoundRect(new RectangleF(0, 0, Width - 1, Height - 1), S(8)))
        using (var fill = new SolidBrush(SvcLook.InfoFill))
            g.FillPath(fill, path);

        // (i) icon
        float d = S(10);
        var ring = new RectangleF(S(11), (Height - d) / 2f, d, d);
        using (var pen = new Pen(SvcLook.Blue, Math.Max(1f, S(1f))))
            g.DrawEllipse(pen, ring);
        using (var b = new SolidBrush(SvcLook.Blue))
        {
            g.FillEllipse(b, ring.X + d / 2f - S(0.6f), ring.Y + S(2f), S(1.2f), S(1.2f));
            g.FillRectangle(b, ring.X + d / 2f - S(0.55f), ring.Y + S(4f), S(1.1f), S(3.6f));
        }

        float x = S(29);
        var textArea = new Rectangle((int)x, 0, Math.Max(10, Width - (int)x - (int)S(8)), Height);
        const TextFormatFlags flags = TextFormatFlags.Left | TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix | TextFormatFlags.EndEllipsis | TextFormatFlags.SingleLine;

        if (_detail.Length == 0)
        {
            TextRenderer.DrawText(g, Text, SvcLook.Px(S(9.5f)), textArea, SvcLook.InfoText, flags | TextFormatFlags.VerticalCenter);
        }
        else
        {
            var top = new Rectangle(textArea.X, (int)S(6), textArea.Width, (int)S(13));
            var bottom = new Rectangle(textArea.X, (int)S(20), textArea.Width, (int)S(12));
            TextRenderer.DrawText(g, Text, SvcLook.Px(S(9.5f), _emphasis ? FontStyle.Bold : FontStyle.Regular), top, SvcLook.InfoText, flags | TextFormatFlags.Top);
            TextRenderer.DrawText(g, _detail, SvcLook.Px(S(8.5f)), bottom, Color.FromArgb(90, 116, 190), flags | TextFormatFlags.Top);
        }
    }
}

/// <summary>Rounded button for step 4: a white "Back" and a blue "Continue", arrow drawn by hand.</summary>
internal sealed class StepButton : Control
{
    private bool _primary;
    private bool _arrowLeft = true;
    private bool _hover;
    private bool _down;

    public StepButton()
    {
        SetStyle(ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint, true);
        BackColor = Color.White;
        Cursor = Cursors.Hand;
        Size = new Size(100, 32);
        TabStop = false;
    }

    /// <summary>Blue filled button with a right arrow (Continue) instead of a white one with a left arrow (Back).</summary>
    [Category("Pawfect"), DefaultValue(false)]
    public bool Primary { get => _primary; set { _primary = value; _arrowLeft = !value; Invalidate(); } }

    protected override void OnTextChanged(EventArgs e) { Invalidate(); base.OnTextChanged(e); }
    protected override void OnEnabledChanged(EventArgs e) { Invalidate(); base.OnEnabledChanged(e); }
    protected override void OnMouseEnter(EventArgs e) { _hover = true; Invalidate(); base.OnMouseEnter(e); }
    protected override void OnMouseLeave(EventArgs e) { _hover = false; _down = false; Invalidate(); base.OnMouseLeave(e); }
    protected override void OnMouseDown(MouseEventArgs e) { _down = true; Invalidate(); base.OnMouseDown(e); }
    protected override void OnMouseUp(MouseEventArgs e) { _down = false; Invalidate(); base.OnMouseUp(e); }

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.Clear(BackColor);

        float k = Math.Max(0.5f, Math.Min(Width / (_primary ? 106f : 76f), Height / 32f));
        float S(float v) => v * k;

        var rect = new RectangleF(1, 1, Width - 3, Height - 3);
        Color fillColour, textColour;
        if (!Enabled) { fillColour = _primary ? Color.FromArgb(160, 190, 240) : Color.White; textColour = _primary ? Color.White : Color.FromArgb(160, 170, 185); }
        else if (_primary) { fillColour = _down ? Color.FromArgb(29, 78, 216) : _hover ? Color.FromArgb(46, 108, 240) : SvcLook.Blue; textColour = Color.White; }
        else { fillColour = _hover ? Color.FromArgb(246, 249, 254) : Color.White; textColour = SvcLook.Navy; }

        using (var path = Ui.RoundRect(rect, S(9)))
        {
            using (var fill = new SolidBrush(fillColour)) g.FillPath(fill, path);
            if (!_primary)
            {
                using var pen = new Pen(_hover ? SvcLook.BlueHover : Color.FromArgb(223, 229, 240), Math.Max(1f, S(1f)));
                g.DrawPath(pen, path);
            }
        }

        // text + arrow, centred together
        var font = SvcLook.Px(S(11), FontStyle.Bold);
        var size = TextRenderer.MeasureText(g, Text, font, new Size(int.MaxValue, int.MaxValue),
            TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine);
        float arrowW = S(11), gap = S(7);
        float total = size.Width + gap + arrowW;
        float x0 = (Width - total) / 2f;
        float cy = Height / 2f;

        float textX = _arrowLeft ? x0 + arrowW + gap : x0;
        float arrowX = _arrowLeft ? x0 : x0 + size.Width + gap;
        TextRenderer.DrawText(g, Text, font, new Rectangle((int)Math.Round(textX), 0, size.Width + 2, Height), textColour,
            TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine);

        using var arrow = new Pen(textColour, Math.Max(1.2f, S(1.5f))) { StartCap = LineCap.Round, EndCap = LineCap.Round, LineJoin = LineJoin.Round };
        float head = S(3.6f);
        if (_arrowLeft)
        {
            g.DrawLine(arrow, arrowX, cy, arrowX + arrowW, cy);
            g.DrawLines(arrow, new[] { new PointF(arrowX + head, cy - head), new PointF(arrowX, cy), new PointF(arrowX + head, cy + head) });
        }
        else
        {
            g.DrawLine(arrow, arrowX, cy, arrowX + arrowW, cy);
            g.DrawLines(arrow, new[] { new PointF(arrowX + arrowW - head, cy - head), new PointF(arrowX + arrowW, cy), new PointF(arrowX + arrowW - head, cy + head) });
        }
    }
}
