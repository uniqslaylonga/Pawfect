using System.Drawing.Drawing2D;
using System.Globalization;

namespace MyApp.Controls;

/// <summary>
/// One row of the dashboard "Alerts" card. It shows the same thing as a row in the bell dropdown:
/// an appointment that is waiting to be confirmed (pet avatar, customer - pet, service and time, status badge).
/// </summary>
internal sealed class AlertRowView : Control
{
    public const int RowHeight = 56;

    private const TextFormatFlags LineFlags = TextFormatFlags.Left | TextFormatFlags.VerticalCenter |
        TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine | TextFormatFlags.NoPadding;

    private const TextFormatFlags CenterFlags = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter |
        TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine;

    private bool _hover;

    public AlertRowView(AppointmentItem item)
    {
        Item = item;
        SetStyle(ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
        BackColor = Color.White;
        Height = RowHeight;
        Cursor = Cursors.Hand;
        TabStop = false;
    }

    public AppointmentItem Item { get; }

    protected override void OnMouseEnter(EventArgs e) { _hover = true; Invalidate(); base.OnMouseEnter(e); }
    protected override void OnMouseLeave(EventArgs e) { _hover = false; Invalidate(); base.OnMouseLeave(e); }

    /// <summary>"9:00 AM" for today, "Oct 14, 9:00 AM" for another day.</summary>
    private static string When(DateTime start)
        => start.Date == DateTime.Today
            ? start.ToString("h:mm tt", CultureInfo.CurrentCulture)
            : start.ToString("MMM d, h:mm tt", CultureInfo.CurrentCulture);

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;

        using (var bg = new SolidBrush(_hover ? Theme.Subtle : Color.White))
            g.FillRectangle(bg, ClientRectangle);
        using (var line = new Pen(Theme.Border))
            g.DrawLine(line, 0, Height - 1, Width, Height - 1);

        // Pet avatar, same look as the Pending Requests rows and the bell dropdown.
        var avatar = new Rectangle(4, (Height - 36) / 2, 36, 36);
        using (var b = new SolidBrush(Color.FromArgb(254, 243, 224)))
            g.FillEllipse(b, avatar);
        string pet = Item.PetName.Trim();
        TextRenderer.DrawText(g, pet.Length > 0 ? pet[..1].ToUpperInvariant() : "?", Theme.Face(10f, FontStyle.Bold),
            avatar, Color.FromArgb(199, 122, 10), CenterFlags);

        // Status badge on the right ("Waiting" for a request that still needs confirming).
        bool waiting = Item.Status == AppointmentStatus.Pending;
        string label = waiting ? "Waiting" : AppointmentStatusStyle.Label(Item.Status);
        var (fore, fill, _) = AppointmentStatusStyle.Pill(waiting ? AppointmentStatus.Completed : Item.Status);
        var pillFont = Theme.Face(7.5f, FontStyle.Bold);
        int pillW = TextRenderer.MeasureText(label, pillFont).Width + 16;
        var pill = new Rectangle(Width - pillW - 4, (Height - 20) / 2, pillW, 20);
        using (var path = Ui.RoundRect(new RectangleF(pill.X, pill.Y, pill.Width, pill.Height), 10))
        using (var b = new SolidBrush(fill))
            g.FillPath(b, path);
        TextRenderer.DrawText(g, label, pillFont, pill, fore, CenterFlags);

        int textX = avatar.Right + 10;
        int textW = Math.Max(20, pill.X - 8 - textX);
        TextRenderer.DrawText(g, $"{Item.CustomerName} - {Item.PetName}", Theme.Face(9f, FontStyle.Bold),
            new Rectangle(textX, 9, textW, 18), Theme.Text, LineFlags);
        TextRenderer.DrawText(g, $"{Item.Service}  \u2022  {When(Item.Start)}",
            Theme.Face(8f), new Rectangle(textX, 28, textW, 16), Theme.Muted, LineFlags);
    }
}
