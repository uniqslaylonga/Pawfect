using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace MyApp.Controls;

internal enum StepState { Pending, Active, Done }

/// <summary>Numbered circle with a caption underneath, used in the step bar ("1 Contact", "2 Pet Details").</summary>
internal sealed class StepDot : Control
{
    private int _number = 1;
    private StepState _state = StepState.Pending;

    public StepDot()
    {
        SetStyle(ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw
                 | ControlStyles.SupportsTransparentBackColor, true);
        Font = Theme.Face(7.5f, FontStyle.Bold);
        BackColor = Theme.Bg;
        Size = new Size(80, 48);
        TabStop = false;
    }

    [Category("Pawfect"), Description("Number shown inside the circle.")]
    [DefaultValue(1)]
    public int Number { get => _number; set { _number = value; Invalidate(); } }

    [Category("Pawfect"), Description("Pending = white circle, Active = blue circle, Done = blue circle with a tick.")]
    [DefaultValue(StepState.Pending)]
    public StepState State { get => _state; set { _state = value; Invalidate(); } }

    protected override void OnTextChanged(EventArgs e) { Invalidate(); base.OnTextChanged(e); }

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;

        const int d = 22;
        var circle = new RectangleF((Width - d) / 2f, 1, d, d);
        bool filled = _state != StepState.Pending;

        using (var fill = new SolidBrush(filled ? Theme.Primary : Color.White))
            g.FillEllipse(fill, circle);
        if (!filled)
        {
            using var pen = new Pen(Theme.Border);
            g.DrawEllipse(pen, circle);
        }

        bool tick = _state == StepState.Done;
        TextRenderer.DrawText(g, tick ? "" : _number.ToString(), tick ? Theme.GlyphFont(8f) : Font,
            Rectangle.Round(circle), filled ? Color.White : Theme.Muted,
            TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter |
            TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine);

        TextRenderer.DrawText(g, Text, Font, new Rectangle(0, d + 6, Width, Height - d - 6),
            _state == StepState.Active ? Theme.Primary : Theme.Muted,
            TextFormatFlags.HorizontalCenter | TextFormatFlags.Top |
            TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine);
    }
}
