using System.Drawing.Drawing2D;

namespace MyApp;

/// <summary>Small helpers so the form code stays readable.</summary>
internal static class Ui
{
    public static GraphicsPath RoundRect(RectangleF r, float radius)
    {
        var path = new GraphicsPath();
        float d = Math.Min(radius * 2, Math.Min(r.Width, r.Height));
        if (d <= 0)
        {
            path.AddRectangle(r);
            return path;
        }
        path.AddArc(r.X, r.Y, d, d, 180, 90);
        path.AddArc(r.Right - d, r.Y, d, d, 270, 90);
        path.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
        path.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
        path.CloseFigure();
        return path;
    }

    public static Label Text(string text, float pt, Color fore, Color back,
        FontStyle style = FontStyle.Regular, ContentAlignment align = ContentAlignment.MiddleLeft)
        => new()
        {
            Text = text,
            AutoSize = false,
            UseMnemonic = false, // so "&" shows up in "Health & Wellness"
            Font = Theme.Face(pt, style),
            ForeColor = fore,
            BackColor = back,
            TextAlign = align,
        };

    public static Label Glyph(string glyph, float pt, Color fore, Color back)
        => new()
        {
            Text = glyph,
            AutoSize = false,
            UseMnemonic = false,
            Font = Theme.GlyphFont(pt),
            ForeColor = fore,
            BackColor = back,
            TextAlign = ContentAlignment.MiddleCenter,
        };
}
