using System.Drawing.Drawing2D;
using MyApp.Controls;

namespace MyApp;

/// <summary>
/// Step 5 of the customer flow: "Your Queue Ticket". Shown right after an appointment request is saved. The same request is
/// saved as a Pending appointment with this queue number, so it appears on the staff Appointments page as well.
/// </summary>
internal sealed class QueueTicketForm : Form
{
    private const TextFormatFlags Centered = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter |
        TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine | TextFormatFlags.NoPadding | TextFormatFlags.EndEllipsis;

    private const TextFormatFlags LeftAlign = TextFormatFlags.Left | TextFormatFlags.VerticalCenter |
        TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine | TextFormatFlags.NoPadding | TextFormatFlags.EndEllipsis;

    private readonly QueueTicket _ticket;

    public QueueTicketForm(QueueTicket ticket)
    {
        _ticket = ticket;

        Text = "Your Queue Ticket";
        Font = Theme.Face(9f);
        BackColor = Theme.Bg;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        StartPosition = FormStartPosition.CenterParent;
        MaximizeBox = false;
        MinimizeBox = false;
        ShowInTaskbar = false;
        ShowIcon = false;
        ClientSize = new Size(560, 540);
        DoubleBuffered = true;

        var done = new PawButton { Text = "Done  \u2192", Kind = PawButtonKind.Primary, Size = new Size(120, 38) };
        done.Location = new Point(ClientSize.Width - done.Width - 28, ClientSize.Height - done.Height - 24);
        done.Click += (_, _) => DialogResult = DialogResult.OK;
        Controls.Add(done);
        AcceptButton = done;
        CancelButton = done;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;

        int w = ClientSize.Width;
        TextRenderer.DrawText(g, "Pawfect", Theme.Face(13f, FontStyle.Bold), new Rectangle(28, 20, 200, 30), Theme.Primary, LeftAlign);
        TextRenderer.DrawText(g, "Your Queue Ticket", Theme.Face(17f, FontStyle.Bold), new Rectangle(28, 64, w - 56, 36), Theme.Text, LeftAlign);

        // the ticket card
        var card = new Rectangle(28, 112, w - 56, 340);
        using (var path = Ui.RoundRect(card, 14))
        using (var fill = new SolidBrush(Color.White))
        using (var pen = new Pen(Theme.Border))
        {
            g.FillPath(fill, path);
            g.DrawPath(pen, path);
        }

        // dashed inner border
        var inner = Rectangle.Inflate(card, -14, -14);
        using (var path = Ui.RoundRect(inner, 10))
        using (var dash = new Pen(Theme.Border) { DashStyle = DashStyle.Dash })
            g.DrawPath(dash, path);

        TextRenderer.DrawText(g, "QUEUE #", Theme.Face(8.5f), new Rectangle(inner.X, inner.Y + 18, inner.Width, 18), Theme.Muted, Centered);
        TextRenderer.DrawText(g, _ticket.QueueNumber, Theme.Face(40f, FontStyle.Bold),
            new Rectangle(inner.X, inner.Y + 38, inner.Width, 70), Theme.Primary, Centered);

        // pet + service
        int boxW = (inner.Width - 36 - 12) / 2;
        var petBox = new Rectangle(inner.X + 18, inner.Y + 126, boxW, 70);
        var serviceBox = new Rectangle(petBox.Right + 12, petBox.Y, boxW, 70);
        DrawInfoBox(g, petBox, "Pet", _ticket.PetName, _ticket.PetBreed);
        DrawInfoBox(g, serviceBox, "Service", _ticket.Service, "~ " + FormatMinutes(_ticket.DurationMinutes));

        // pending vaccine clearance
        var (fore, fill2, border) = AppointmentStatusStyle.Pill(AppointmentStatus.Pending);
        var banner = new Rectangle(inner.X + 18, inner.Y + 212, inner.Width - 36, 40);
        using (var path = Ui.RoundRect(banner, 8))
        using (var b = new SolidBrush(fill2))
        using (var pen = new Pen(border))
        {
            g.FillPath(b, path);
            g.DrawPath(pen, path);
        }
        TextRenderer.DrawText(g, "Pending Vaccine Clearance", Theme.Face(9.5f, FontStyle.Bold), banner, fore, Centered);

        TextRenderer.DrawText(g, "A staff member will review your vaccine record before your pet is admitted.", Theme.Face(8f),
            new Rectangle(inner.X + 18, banner.Bottom + 8, inner.Width - 36, 34), Theme.Muted,
            TextFormatFlags.Left | TextFormatFlags.WordBreak | TextFormatFlags.NoPrefix);

        // "Ticket saved" pill
        var pill = new Rectangle(28, ClientSize.Height - 24 - 30, 120, 30);
        using (var path = Ui.RoundRect(pill, 15))
        using (var b = new SolidBrush(Theme.GreenSoft))
            g.FillPath(b, path);
        TextRenderer.DrawText(g, "\u2713  Ticket saved", Theme.Face(8.5f, FontStyle.Bold), pill, Color.FromArgb(22, 101, 52), Centered);
    }

    private static void DrawInfoBox(Graphics g, Rectangle r, string caption, string line1, string line2)
    {
        using (var path = Ui.RoundRect(r, 8))
        using (var b = new SolidBrush(Theme.Bg))
        using (var pen = new Pen(Theme.Border))
        {
            g.FillPath(b, path);
            g.DrawPath(pen, path);
        }
        TextRenderer.DrawText(g, caption, Theme.Face(7.5f), new Rectangle(r.X + 14, r.Y + 8, r.Width - 28, 14), Theme.Muted, LeftAlign);
        TextRenderer.DrawText(g, line1, Theme.Face(10f, FontStyle.Bold), new Rectangle(r.X + 14, r.Y + 24, r.Width - 28, 20), Theme.Text, LeftAlign);
        TextRenderer.DrawText(g, line2, Theme.Face(8f), new Rectangle(r.X + 14, r.Y + 46, r.Width - 28, 16), Theme.Muted, LeftAlign);
    }

    private static string FormatMinutes(int minutes)
        => minutes >= 60 && minutes % 60 == 0 ? $"{minutes / 60} hr" : $"{minutes} mins";
}
