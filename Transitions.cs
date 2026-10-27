using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace MyApp;

/// <summary>Master switch for every animation in this file. false = pages and screens change instantly.</summary>
internal static class Transitions
{
    public static readonly bool Enabled = true;
}

/// <summary>
/// Fades a whole form in when it opens and out when it closes, so moving between screens
/// (landing, login, dashboards, customer steps, queue ticket) no longer pops or resizes in view.
/// Attached automatically by FullScreen.Apply.
/// </summary>
internal static class FormFade
{
    private const int InMs = 200, OutMs = 140;

    public static void Attach(Form form)
    {
        if (!Transitions.Enabled) return;   // instant: no fade, the form just appears fully visible
        form.Opacity = 0;   // invisible until it has been sized and laid out (see FullScreen.Apply), then it fades in
        form.Shown += (_, _) => Animate(form, 0, 1, InMs, null);

        bool fading = false, finished = false;
        form.FormClosing += (_, e) =>
        {
            if (finished || e.Cancel) return;
            if (fading) { e.Cancel = true; return; }   // a second click while fading out: ignore it

            // Don't delay shutdown, and don't animate something nobody can see.
            if (e.CloseReason is CloseReason.WindowsShutDown or CloseReason.TaskManagerClosing or CloseReason.ApplicationExitCall) return;
            if (!form.Visible || form.WindowState == FormWindowState.Minimized || form.Opacity < 0.05) return;

            // Hold the close, fade out, then close again with the same result.
            // (Cancelling a modal form's close resets its DialogResult, so it is remembered here.)
            var result = form.DialogResult;
            e.Cancel = true;
            fading = true;
            Animate(form, form.Opacity, 0, OutMs, () =>
            {
                finished = true;
                if (form.Modal) form.DialogResult = result == DialogResult.None ? DialogResult.Cancel : result;
                else form.Close();
            });
        };
    }

    private static void Animate(Form form, double from, double to, int ms, Action? done)
    {
        var clock = Stopwatch.StartNew();
        var timer = new System.Windows.Forms.Timer { Interval = 15 };
        timer.Tick += (_, _) =>
        {
            if (form.IsDisposed) { timer.Stop(); timer.Dispose(); return; }
            double p = Math.Min(1.0, clock.ElapsedMilliseconds / (double)ms);
            double eased = 1 - Math.Pow(1 - p, 3);   // ease-out: quick start, soft landing
            form.Opacity = from + (to - from) * eased;
            if (p >= 1.0)
            {
                timer.Stop();
                timer.Dispose();
                done?.Invoke();
            }
        };
        timer.Start();
    }
}

/// <summary>
/// Cross-fades between two states of the same area (sidebar pages, Add Pet steps).
/// Usage: PageFade.Swap(contentPanel, () => { ...change what is visible... });
/// The old picture stays on screen while the change (and any data loading in it) happens, then blends into the new one.
/// If anything about the snapshot fails, the change is simply applied without animation.
/// </summary>
internal static class PageFade
{
    private static bool _inside;                 // a Swap inside a Swap (steps calling steps) just runs the change
    private static FadeOverlay? _running;

    public static void Swap(Control host, Action change, int ms = 160)
    {
        var form = host.FindForm();
        if (!Transitions.Enabled || _inside || form is null || !host.IsHandleCreated || !host.Visible
            || host.Width < 2 || host.Height < 2
            || form.WindowState == FormWindowState.Minimized || form.Opacity < 1)
        {
            change();
            return;
        }

        _running?.Finish();   // a quick second click: settle the previous fade first

        Bitmap? before = Capture(host);
        if (before is null) { change(); return; }

        var overlay = new FadeOverlay(before)
        {
            Bounds = form.RectangleToClient(host.RectangleToScreen(host.ClientRectangle)),
        };
        // Added to the form (not the host) so the content scaler never rescales it.
        form.Controls.Add(overlay);
        overlay.BringToFront();
        foreach (var caption in form.Controls.OfType<CaptionButtons>()) caption.BringToFront();
        overlay.Update();

        _inside = true;
        try
        {
            change();
            host.PerformLayout();
        }
        finally { _inside = false; }

        Bitmap? after = Capture(host);
        if (after is null) { overlay.Finish(); return; }

        _running = overlay;
        overlay.Start(after, ms, () => { if (ReferenceEquals(_running, overlay)) _running = null; });
    }

    private static Bitmap? Capture(Control c)
    {
        try
        {
            var bmp = new Bitmap(c.Width, c.Height);
            c.DrawToBitmap(bmp, new Rectangle(Point.Empty, c.Size));
            return bmp;
        }
        catch (Exception ex) when (ex is ArgumentException or InvalidOperationException or System.ComponentModel.Win32Exception or OutOfMemoryException)
        {
            return null;
        }
    }
}

/// <summary>Opaque control that paints the new picture with the old one fading out on top of it.</summary>
internal sealed class FadeOverlay : Control
{
    private Bitmap? _before;
    private Bitmap? _after;
    private readonly ImageAttributes _attrs = new();
    private readonly ColorMatrix _matrix = new() { Matrix00 = 1, Matrix11 = 1, Matrix22 = 1, Matrix33 = 1, Matrix44 = 1 };
    private System.Windows.Forms.Timer? _timer;
    private Action? _onDone;
    private float _alpha = 1f;   // opacity of the OLD picture

    public FadeOverlay(Bitmap before)
    {
        _before = before;
        SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.Opaque, true);
        TabStop = false;
    }

    public void Start(Bitmap after, int ms, Action onDone)
    {
        _after = after;
        _onDone = onDone;
        var clock = Stopwatch.StartNew();
        _timer = new System.Windows.Forms.Timer { Interval = 15 };
        _timer.Tick += (_, _) =>
        {
            double p = Math.Min(1.0, clock.ElapsedMilliseconds / (double)ms);
            _alpha = (float)Math.Pow(1 - p, 2);   // old picture leaves quickly at first, then gently
            Invalidate();
            Update();
            if (p >= 1.0) Finish();
        };
        _timer.Start();
        Invalidate();
    }

    /// <summary>Stops the animation and removes the overlay, leaving the real (already changed) content in view.</summary>
    public void Finish()
    {
        _timer?.Stop();
        _timer?.Dispose();
        _timer = null;
        var parent = Parent;
        parent?.Controls.Remove(this);
        Dispose();
        var done = _onDone;
        _onDone = null;
        done?.Invoke();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.InterpolationMode = InterpolationMode.NearestNeighbor;
        g.CompositingQuality = CompositingQuality.HighSpeed;
        var area = new Rectangle(0, 0, Width, Height);

        if (_after is not null) g.DrawImage(_after, area);
        if (_before is not null && (_alpha > 0.004f || _after is null))
        {
            _matrix.Matrix33 = _after is null ? 1f : _alpha;
            _attrs.SetColorMatrix(_matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
            g.DrawImage(_before, area, 0, 0, _before.Width, _before.Height, GraphicsUnit.Pixel, _attrs);
        }
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _timer?.Dispose();
            _attrs.Dispose();
            _before?.Dispose();
            _after?.Dispose();
            _before = null;
            _after = null;
        }
        base.Dispose(disposing);
    }
}
