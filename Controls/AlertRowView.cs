using System.Drawing.Drawing2D;

namespace MyApp.Controls;

/// <summary>
/// One row of the dashboard "Alerts" card: a round icon (or the pet's initial for an approval request), the title,
/// a detail line and a coloured tag on the right (Out of stock, Expired, Low stock, Near expiry, Approval).
/// Rows show a hand cursor and a highlight under the mouse; the owner handles Click.
/// </summary>
internal sealed class AlertRowView : Control
{
    public const int RowHeight = 56;

    private const TextFormatFlags LineFlags = TextFormatFlags.Left | TextFormatFlags.VerticalCenter |
        TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine | TextFormatFlags.NoPadding;

    private const TextFormatFlags CenterFlags = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter |
        TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine;

    private bool _hover;

    public AlertRowView(StaffAlert alert)
    {
        Alert = alert;
        SetStyle(ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
        BackColor = Color.White;
        Height = RowHeight;
        Cursor = Cursors.Hand;
        TabStop = false;
    }

    public StaffAlert Alert { get; }

    protected override void OnMouseEnter(EventArgs e) { _hover = true; Invalidate(); base.OnMouseEnter(e); }
    protected override void OnMouseLeave(EventArgs e) { _hover = false; Invalidate(); base.OnMouseLeave(e); }

    private static readonly Color Amber = Color.FromArgb(199, 122, 10);
    private static readonly Color AmberSoft = Color.FromArgb(254, 243, 224);
    private static readonly Color RedSoft = Color.FromArgb(253, 233, 233);

    /// <summary>Icon colour, soft background and icon glyph for each kind of alert.</summary>
    private static (Color Fore, Color Soft, string Glyph) Look(StaffAlertKind kind) => kind switch
    {
        StaffAlertKind.OutOfStock => (Theme.Danger, RedSoft, "\uE783"),
        StaffAlertKind.Expired => (Theme.Danger, RedSoft, "\uE7BA"),
        StaffAlertKind.LowStock => (Amber, AmberSoft, "\uE7BA"),
        StaffAlertKind.NearExpiry => (Amber, AmberSoft, "\uE121"),
        _ => (Amber, AmberSoft, ""),   // approval: shows the pet's initial instead
    };

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;

        using (var bg = new SolidBrush(_hover ? Theme.Subtle : Color.White))
            g.FillRectangle(bg, ClientRectangle);
        using (var line = new Pen(Theme.Border))
            g.DrawLine(line, 0, Height - 1, Width, Height - 1);

        var (fore, soft, glyph) = Look(Alert.Kind);

        // Round icon on the left; approvals show the pet's initial like the Pending Requests rows and the bell.
        var avatar = new Rectangle(4, (Height - 36) / 2, 36, 36);
        using (var b = new SolidBrush(soft))
            g.FillEllipse(b, avatar);
        if (Alert.Kind == StaffAlertKind.Approval)
        {
            string pet = Alert.Appointment?.PetName.Trim() ?? "";
            TextRenderer.DrawText(g, pet.Length > 0 ? pet[..1].ToUpperInvariant() : "?", Theme.Face(10f, FontStyle.Bold),
                avatar, fore, CenterFlags);
        }
        else
        {
            TextRenderer.DrawText(g, glyph, Theme.GlyphFont(11f), avatar, fore, CenterFlags);
        }

        // Tag on the right.
        var (tagFore, tagFill) = Alert.Kind == StaffAlertKind.Approval ? (Theme.Primary, Theme.PrimarySoft) : (fore, soft);
        var pillFont = Theme.Face(7.5f, FontStyle.Bold);
        int pillW = TextRenderer.MeasureText(Alert.Tag, pillFont).Width + 16;
        var pill = new Rectangle(Width - pillW - 4, (Height - 20) / 2, pillW, 20);
        using (var path = Ui.RoundRect(new RectangleF(pill.X, pill.Y, pill.Width, pill.Height), 10))
        using (var b = new SolidBrush(tagFill))
            g.FillPath(b, path);
        TextRenderer.DrawText(g, Alert.Tag, pillFont, pill, tagFore, CenterFlags);

        int textX = avatar.Right + 10;
        int textW = Math.Max(20, pill.X - 8 - textX);
        TextRenderer.DrawText(g, Alert.Title, Theme.Face(9f, FontStyle.Bold),
            new Rectangle(textX, 9, textW, 18), Theme.Text, LineFlags);
        TextRenderer.DrawText(g, Alert.Detail, Theme.Face(8f),
            new Rectangle(textX, 28, textW, 16), Theme.Muted, LineFlags);
    }
}
