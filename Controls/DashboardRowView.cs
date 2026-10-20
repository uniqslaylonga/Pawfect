using System.Drawing.Drawing2D;

namespace MyApp.Controls;

/// <summary>One row of the admin dashboard lists (Alerts, Recent Activity): coloured dot, title, detail line and a small note on the right.</summary>
internal sealed class DashboardRowView : Control
{
    public const int RowHeight = 52;

    private const TextFormatFlags LineFlags = TextFormatFlags.Left | TextFormatFlags.VerticalCenter |
        TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine | TextFormatFlags.NoPadding;

    private const TextFormatFlags RightFlags = TextFormatFlags.Right | TextFormatFlags.VerticalCenter |
        TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine | TextFormatFlags.NoPadding;

    private readonly string _title;
    private readonly string _detail;
    private readonly string _meta;
    private readonly Color _tint;

    public DashboardRowView(string title, string detail, string meta, Color tint)
    {
        _title = title;
        _detail = detail;
        _meta = meta;
        _tint = tint;
        SetStyle(ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
        BackColor = Color.White;
        Height = RowHeight;
        TabStop = false;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;

        using (var bg = new SolidBrush(Color.White))
            g.FillRectangle(bg, ClientRectangle);
        using (var line = new Pen(Theme.Border))
            g.DrawLine(line, 0, Height - 1, Width, Height - 1);
        using (var dot = new SolidBrush(_tint))
            g.FillEllipse(dot, 6, (Height - 10) / 2, 10, 10);

        var metaFont = Theme.Face(8f);
        int metaW = _meta.Length == 0
            ? 0
            : TextRenderer.MeasureText(_meta, metaFont, new Size(int.MaxValue, int.MaxValue), TextFormatFlags.NoPadding).Width + 4;

        const int left = 28;
        var titleRect = new Rectangle(left, 8, Math.Max(0, Width - left - metaW - 10), 18);
        var detailRect = new Rectangle(left, 27, Math.Max(0, Width - left - 4), 18);

        TextRenderer.DrawText(g, _title, Theme.Face(9f, FontStyle.Bold), titleRect, Theme.Text, LineFlags);
        TextRenderer.DrawText(g, _detail, Theme.Face(8.5f), detailRect, Theme.Muted, LineFlags);
        if (metaW > 0)
            TextRenderer.DrawText(g, _meta, metaFont, new Rectangle(Width - metaW - 4, 8, metaW, 18), Theme.Muted, RightFlags);
    }
}
