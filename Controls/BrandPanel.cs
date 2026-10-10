using System.Drawing.Drawing2D;

namespace MyApp.Controls;

/// <summary>Solid primary-blue panel with soft decorative circles (login side panel).</summary>
internal sealed class BrandPanel : Panel
{
    public BrandPanel()
    {
        DoubleBuffered = true;
        ResizeRedraw = true;
        BackColor = Theme.Primary;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        using var soft = new SolidBrush(Color.FromArgb(28, Color.White));
        g.FillEllipse(soft, -200, -200, 400, 400);
        g.FillEllipse(soft, Width - 110, Height - 20, 440, 440);
        g.FillEllipse(soft, Width - 190, -120, 220, 220);
    }
}
