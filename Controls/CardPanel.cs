using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace MyApp.Controls;

/// <summary>Rounded card with a thin border. Children should use <see cref="FillColor"/> as their BackColor.</summary>
internal class CardPanel : Panel
{
    private Color _fill = Color.White;
    private Color _border = Theme.Border;
    private Color _corner = Theme.Bg;
    private int _radius = 10;

    public CardPanel()
    {
        DoubleBuffered = true;
        ResizeRedraw = true;
        Margin = Padding.Empty;
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public Color FillColor { get => _fill; set { _fill = value; Invalidate(); } }
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public Color BorderColor { get => _border; set { _border = value; Invalidate(); } }
    /// <summary>Colour behind the rounded corners (the colour of whatever the card sits on).</summary>
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public Color CornerColor { get => _corner; set { _corner = value; Invalidate(); } }
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public int Radius { get => _radius; set { _radius = value; Invalidate(); } }

    protected override void OnPaintBackground(PaintEventArgs e)
    {
        using var b = new SolidBrush(_corner);
        e.Graphics.FillRectangle(b, ClientRectangle);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        var r = new RectangleF(0.5f, 0.5f, Width - 2, Height - 2);
        using var path = Ui.RoundRect(r, _radius);
        using var fill = new SolidBrush(_fill);
        using var pen = new Pen(_border);
        g.FillPath(fill, path);
        g.DrawPath(pen, path);
    }
}

internal enum Edge { Top, Right, Bottom }

/// <summary>Panel with a single 1px divider line along one edge (sidebar / top bar).</summary>
internal class EdgePanel : Panel
{
    public EdgePanel()
    {
        DoubleBuffered = true;
        ResizeRedraw = true;
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public Edge Side { get; set; } = Edge.Bottom;
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public Color EdgeColor { get; set; } = Theme.Border;

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        using var pen = new Pen(EdgeColor);
        var g = e.Graphics;
        switch (Side)
        {
            case Edge.Top:
                g.DrawLine(pen, 0, 0, Width, 0);
                break;
            case Edge.Right:
                g.DrawLine(pen, Width - 1, 0, Width - 1, Height);
                break;
            default:
                g.DrawLine(pen, 0, Height - 1, Width, Height - 1);
                break;
        }
    }
}
