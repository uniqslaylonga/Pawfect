using System.Runtime.InteropServices;

namespace MyApp;

/// <summary>
/// Makes a form cover the whole screen (no title bar or border, taskbar hidden) and gives it its own
/// minimize / restore-or-full-screen / close buttons in the top-right corner.
/// Forms whose controls were laid out at a fixed size (landing, login, customer steps) are scaled up to fill the screen
/// (everything grows together: controls, spacing and text) and kept centred; forms that already stretch with their window
/// (admin) just grow. The middle button switches between full screen and a normal-sized window in the middle of the screen,
/// and that window can be moved by dragging its background or the thin strip along the top.
/// </summary>
internal static class FullScreen
{
    /// <summary>
    /// Whether the top-right minimize / restore / close buttons are shown. Off: the windows are plain full-screen
    /// (Esc and Alt+F4 still close them, the taskbar still minimizes). Set to true to bring the buttons back.
    /// </summary>
    public static bool ShowCaptionButtons { get; set; } = false;

    [DllImport("user32.dll")] private static extern bool ReleaseCapture();
    [DllImport("user32.dll")] private static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);

    private static void DragWindow(Form form)
    {
        ReleaseCapture();
        SendMessage(form.Handle, 0xA1 /* WM_NCLBUTTONDOWN */, 2 /* HTCAPTION */, 0);
    }

    /// <param name="centerContent">True for fixed-size layouts: wrap the existing controls in a panel that is scaled to the screen and centred.</param>
    /// <summary>Font sizes for hand-painted controls are rounded to quarter points so scaled fonts are reused, not created every paint.</summary>
    internal static float Pt(float points) => (float)Math.Round(points * 4f) / 4f;

    public static Control Apply(Form form, bool centerContent) => Apply(form, centerContent, out _);

    public static Control Apply(Form form, bool centerContent, out ContentScaler? contentScaler)
    {
        var designSize = form.ClientSize;               // designer forms (AutoScaleMode.Dpi): already DPI-scaled after InitializeComponent
        // Forms built in code (queue ticket, scan card, pet details ...) lay out in raw pixels but use point-sized fonts, so on a
        // 125 % screen (1536 x 864) their text is 25 % bigger than the layout it sits in. They get the DPI factor applied to the
        // layout (not the fonts) so text and boxes grow together.
        float dpi = Math.Max(1f, form.DeviceDpi / 96f);
        bool rawPixels = form.AutoScaleMode != AutoScaleMode.Dpi;
        Panel? stage = null;
        ContentScaler? scaler = null;
        if (centerContent)
        {
            var kids = form.Controls.Cast<Control>().ToArray();
            stage = new Panel { Size = designSize, BackColor = form.BackColor, TabStop = false };
            form.SuspendLayout();
            stage.Controls.AddRange(kids);              // AddRange keeps the original front-to-back order
            form.Controls.Add(stage);
            form.ResumeLayout(false);
            scaler = new ContentScaler(stage, rawPixels, dpi);   // also scales controls the form adds later (pet cards, step panels)
            if (rawPixels && dpi > 1.001f)
                designSize = new Size((int)Math.Round(designSize.Width * dpi), (int)Math.Round(designSize.Height * dpi));
        }

        form.FormBorderStyle = FormBorderStyle.None;
        form.MaximizeBox = false;
        form.MinimizeBox = false;
        form.MinimumSize = Size.Empty;
        form.StartPosition = FormStartPosition.Manual;
        form.ShowInTaskbar = true;                      // so a minimized window can be brought back from the taskbar

        bool full = true;
        var caption = new CaptionButtons { Visible = ShowCaptionButtons };
        var grip = new Panel { Height = 10, BackColor = Color.Transparent, TabStop = false, Visible = false };   // drag strip, windowed mode only
        form.Controls.Add(grip);
        form.Controls.Add(caption);
        caption.BringToFront();

        void Fit()
        {
            if (form.WindowState == FormWindowState.Minimized) return;
            var screen = Screen.FromControl(form);
            Rectangle target;
            if (full)
            {
                target = screen.Bounds;                 // the whole monitor, taskbar included
            }
            else
            {
                var area = screen.WorkingArea;
                int w = Math.Min(designSize.Width, (int)(area.Width * 0.92));
                int h = Math.Min(designSize.Height, (int)(area.Height * 0.92));
                target = new Rectangle(area.X + (area.Width - w) / 2, area.Y + (area.Height - h) / 2, w, h);
            }
            if (form.WindowState != FormWindowState.Normal) form.WindowState = FormWindowState.Normal;
            if (form.Bounds != target) form.Bounds = target;
        }

        void Layout()
        {
            if (form.WindowState == FormWindowState.Minimized) return;
            if (stage is not null && scaler is not null)
            {
                // Grow everything by the same factor until the design touches the screen on one axis.
                float s = Math.Min((float)form.ClientSize.Width / designSize.Width, (float)form.ClientSize.Height / designSize.Height);
                s = Math.Clamp(s, 0.5f, 3f);
                scaler.SetScale(s, new Size((int)Math.Round(designSize.Width * s), (int)Math.Round(designSize.Height * s)));
                stage.Location = new Point(Math.Max(0, (form.ClientSize.Width - stage.Width) / 2),
                                           Math.Max(0, (form.ClientSize.Height - stage.Height) / 2));
            }
            caption.Location = new Point(form.ClientSize.Width - caption.Width, 0);
            grip.Bounds = new Rectangle(0, 0, Math.Max(0, form.ClientSize.Width - caption.Width), grip.Height);
            grip.Visible = !full;
            grip.BringToFront();
            caption.BringToFront();
        }

        caption.MinimizeClicked += (_, _) =>
        {
            // A dialog opened from another full-screen form would otherwise minimise alone and leave its (disabled) owner covering the
            // screen. Minimising the root owner takes every owned window with it, and one taskbar click brings them all back.
            Form root = form;
            while (root.Owner is { } owner) root = owner;
            root.WindowState = FormWindowState.Minimized;
        };
        caption.ToggleClicked += (_, _) =>
        {
            full = !full;
            caption.IsFull = full;
            Fit();
            Layout();
        };
        caption.CloseClicked += (_, _) => form.Close();

        // A normal-sized window can be dragged by its top strip or by any empty background.
        MouseEventHandler drag = (_, e) => { if (!full && e.Button == MouseButtons.Left) DragWindow(form); };
        grip.MouseDown += drag;
        form.MouseDown += drag;
        if (stage is not null) stage.MouseDown += drag;

        Fit(); Layout();                                // sized and laid out now, before it is first shown (no visible resize jump)
        form.Load += (_, _) => { Fit(); Layout(); };
        form.Shown += (_, _) => { Fit(); Layout(); };
        FormFade.Attach(form);                          // fades in when shown, out when closed (registered after the layout hooks)
        form.Resize += (_, _) => Layout();
        contentScaler = scaler;
        return (Control?)stage ?? form;
    }
}

/// <summary>
/// Scales a tree of controls that was laid out at a fixed size: position, size, spacing and font of every control grow by the
/// same factor. Controls that get added to the tree later (they are built at the original size) are scaled as they arrive.
/// </summary>
internal sealed class ContentScaler
{
    private readonly Control _root;
    private readonly float _dpi;        // screen scaling: 1.25 for 1920 x 1080 shown as 1536 x 864
    private float _scale = 1f;

    /// <summary>How much bigger than the original design the content is right now (1 = original size).</summary>
    public float Scale => _scale;

    /// <summary>
    /// The factor for positions and sizes written as 96-dpi design pixels (the number typed in code): the content scale times the
    /// screen's DPI scaling. Use this, not <see cref="Scale"/>, to place a control by a design coordinate.
    /// </summary>
    public float LayoutScale => _scale * _dpi;

    /// <param name="builtAtRawPixels">True when the controls under <paramref name="root"/> were laid out in plain pixels (a form built in code),
    /// so they still need the DPI factor on their layout. Designer forms (AutoScaleMode.Dpi) are already scaled by WinForms.</param>
    public ContentScaler(Control root, bool builtAtRawPixels = false, float dpi = 1f)
    {
        _root = root;
        _dpi = Math.Max(1f, dpi);
        Hook(root);

        if (builtAtRawPixels && _dpi > 1.001f)
        {
            // Layout only: fonts are in points, so Windows has already made them the right size for this screen.
            var items = Collect(_root, includeRoot: false);
            var size = new Size((int)Math.Round(_root.Width * _dpi), (int)Math.Round(_root.Height * _dpi));
            _root.SuspendLayout();
            _root.Size = size;
            Apply(items, _dpi, 1f);
            _root.ResumeLayout(true);
        }
    }

    /// <summary>
    /// Scales the content to <paramref name="target"/> and gives the root its new size. Everything is read before the root
    /// changes size, because controls anchored to its right / bottom edge would otherwise be pushed by the size change on top
    /// of being scaled; the new bounds are then set explicitly, which also resets those anchors for the new size.
    /// </summary>
    public void SetScale(float target, Size rootSize)
    {
        float ratio = target / _scale;
        if (Math.Abs(ratio - 1f) < 0.002f && _root.Size == rootSize) return;

        var items = Collect(_root, includeRoot: false);
        _root.SuspendLayout();
        _root.Size = rootSize;
        Apply(items, ratio, ratio);
        _root.ResumeLayout(true);
        _scale = target;
    }

    private void Hook(Control c)
    {
        c.ControlAdded += OnAdded;
        foreach (Control child in c.Controls) Hook(child);
    }

    private void OnAdded(object? sender, ControlEventArgs e)
    {
        Hook(e.Control!);
        // Controls added later are built in plain pixels at the original size: layout needs DPI * scale, fonts (points) only the scale.
        float layout = _scale * _dpi;
        if (Math.Abs(layout - 1f) < 0.002f && Math.Abs(_scale - 1f) < 0.002f) return;
        Apply(Collect(e.Control!, includeRoot: true), layout, _scale);
    }

    private static List<(Control C, Rectangle Bounds, Padding Padding, Padding Margin, Font Font)> Collect(Control root, bool includeRoot)
    {
        var list = new List<(Control, Rectangle, Padding, Padding, Font)>();
        void Walk(Control c)
        {
            list.Add((c, c.Bounds, c.Padding, c.Margin, c.Font));
            foreach (Control child in c.Controls) Walk(child);
        }
        if (includeRoot) Walk(root);
        else foreach (Control child in root.Controls) Walk(child);
        return list;
    }

    // Everything is read first and applied afterwards, so a parent changing size or font cannot disturb what its children report.
    private static void Apply(List<(Control C, Rectangle Bounds, Padding Padding, Padding Margin, Font Font)> items, float layoutRatio, float fontRatio)
    {
        int S(int v) => (int)Math.Round(v * layoutRatio);
        Padding P(Padding p) => new(S(p.Left), S(p.Top), S(p.Right), S(p.Bottom));
        bool scaleFont = Math.Abs(fontRatio - 1f) > 0.002f;
        foreach (var (c, b, pad, mar, font) in items)
        {
            if (scaleFont) c.Font = new Font(font.FontFamily, font.SizeInPoints * fontRatio, font.Style, GraphicsUnit.Point);
            c.Padding = P(pad);
            c.Margin = P(mar);
            c.SetBounds(S(b.X), S(b.Y), S(b.Width), S(b.Height));
        }
    }
}

/// <summary>Three flat window buttons (minimize, restore / full screen, close), drawn by hand so they match the app.</summary>
internal sealed class CaptionButtons : Control
{
    private const int CellWidth = 46, CellHeight = 32;
    private static readonly Color Glyph = Color.FromArgb(51, 65, 85);
    private static readonly Color Hover = Color.FromArgb(226, 232, 240);
    private static readonly Color CloseHover = Color.FromArgb(232, 17, 35);

    private int _hot = -1;          // 0 minimize, 1 restore / full screen, 2 close
    private bool _isFull = true;

    public event EventHandler? MinimizeClicked;
    public event EventHandler? ToggleClicked;
    public event EventHandler? CloseClicked;

    public CaptionButtons()
    {
        SetStyle(ControlStyles.SupportsTransparentBackColor | ControlStyles.OptimizedDoubleBuffer |
                 ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);
        BackColor = Color.Transparent;
        TabStop = false;
        Cursor = Cursors.Default;
        Size = new Size(CellWidth * 3, CellHeight);
    }

    /// <summary>True while the form covers the whole screen (the middle button then shows "restore down").</summary>
    public bool IsFull
    {
        get => _isFull;
        set { _isFull = value; Invalidate(); }
    }

    private int CellAt(Point p) => p.X < 0 || p.Y < 0 || p.Y >= Height ? -1 : Math.Min(2, p.X / CellWidth);

    protected override void OnMouseMove(MouseEventArgs e)
    {
        base.OnMouseMove(e);
        int cell = CellAt(e.Location);
        if (cell != _hot) { _hot = cell; Invalidate(); }
    }

    protected override void OnMouseLeave(EventArgs e)
    {
        base.OnMouseLeave(e);
        if (_hot != -1) { _hot = -1; Invalidate(); }
    }

    protected override void OnMouseUp(MouseEventArgs e)
    {
        base.OnMouseUp(e);
        if (e.Button != MouseButtons.Left) return;
        switch (CellAt(e.Location))
        {
            case 0: MinimizeClicked?.Invoke(this, EventArgs.Empty); break;
            case 1: ToggleClicked?.Invoke(this, EventArgs.Empty); break;
            case 2: CloseClicked?.Invoke(this, EventArgs.Empty); break;
        }
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;   // crisp 1px lines like Windows
        for (int i = 0; i < 3; i++)
        {
            var cell = new Rectangle(i * CellWidth, 0, CellWidth, CellHeight);
            bool hot = i == _hot;
            if (hot)
            {
                using var fill = new SolidBrush(i == 2 ? CloseHover : Hover);
                g.FillRectangle(fill, cell);
            }
            using var pen = new Pen(hot && i == 2 ? Color.White : Glyph, 1f);
            int cx = cell.X + CellWidth / 2, cy = CellHeight / 2;
            switch (i)
            {
                case 0:   // minimize: a short horizontal line
                    g.DrawLine(pen, cx - 5, cy, cx + 5, cy);
                    break;
                case 1:
                    if (_isFull)   // "restore down": two overlapping squares
                    {
                        g.DrawRectangle(pen, cx - 5, cy - 2, 7, 7);
                        g.DrawLines(pen, new[] { new Point(cx - 3, cy - 4), new Point(cx - 3, cy - 5), new Point(cx + 4, cy - 5), new Point(cx + 4, cy + 2), new Point(cx + 3, cy + 2) });
                    }
                    else           // "full screen": one square
                    {
                        g.DrawRectangle(pen, cx - 5, cy - 5, 10, 10);
                    }
                    break;
                default:  // close: a cross
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    g.DrawLine(pen, cx - 5, cy - 5, cx + 5, cy + 5);
                    g.DrawLine(pen, cx - 5, cy + 5, cx + 5, cy - 5);
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
                    break;
            }
        }
    }
}