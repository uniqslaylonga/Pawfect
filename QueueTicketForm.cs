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

    /// <summary>The ticket is drawn on this panel (laid out for 560 x 540) so the whole thing can be scaled to the screen.</summary>
    private sealed class TicketCanvas : Panel
    {
        public TicketCanvas() { DoubleBuffered = true; ResizeRedraw = true; }
    }

    public QueueTicketForm(QueueTicket ticket)
    {
        Icon = AppIcon.Value;
        _ticket = ticket;

        Text = "Your Queue Ticket";
        Font = Theme.Face(9f);
        BackColor = Theme.Bg;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        StartPosition = FormStartPosition.CenterParent;
        MaximizeBox = false;
        MinimizeBox = false;
        ShowIcon = false;
        ClientSize = new Size(560, 540);

        var canvas = new TicketCanvas { Size = new Size(560, 540), BackColor = Theme.Bg };
        canvas.Paint += (_, e) => PaintTicket(e.Graphics, canvas.Width);

        var done = new PawButton { Text = "Done  \u2192", Kind = PawButtonKind.Primary, Size = new Size(120, 38) };
        done.Location = new Point(560 - done.Width - 28, 540 - done.Height - 24);
        done.Click += (_, _) => DialogResult = DialogResult.OK;
        canvas.Controls.Add(done);
        Controls.Add(canvas);
        AcceptButton = done;
        CancelButton = done;

        FullScreen.Apply(this, centerContent: true);   // full screen; the ticket is scaled up and centred
    }

    private void PaintTicket(Graphics g, int canvasWidth)
    {
        g.SmoothingMode = SmoothingMode.AntiAlias;

        // Everything below is written for a 560 x 540 ticket; k enlarges it to the size it has now.
        float k = Math.Max(0.5f, canvasWidth / 560f);
        int S(float v) => (int)Math.Round(v * k);
        Font F(float pt, FontStyle st = FontStyle.Regular) => Theme.Face(FullScreen.Pt(pt * k), st);

        TextRenderer.DrawText(g, "Pawfect", F(13f, FontStyle.Bold), new Rectangle(S(28), S(20), S(200), S(30)), Theme.Primary, LeftAlign);
        TextRenderer.DrawText(g, "Your Queue Ticket", F(17f, FontStyle.Bold), new Rectangle(S(28), S(64), S(560 - 56), S(36)), Theme.Text, LeftAlign);

        // the ticket card
        var card = new Rectangle(S(28), S(112), S(560 - 56), S(340));
        using (var path = Ui.RoundRect(card, S(14)))
        using (var fill = new SolidBrush(Color.White))
        using (var pen = new Pen(Theme.Border))
        {
            g.FillPath(fill, path);
            g.DrawPath(pen, path);
        }

        // dashed inner border
        var inner = Rectangle.Inflate(card, -S(14), -S(14));
        using (var path = Ui.RoundRect(inner, S(10)))
        using (var dash = new Pen(Theme.Border) { DashStyle = DashStyle.Dash })
            g.DrawPath(dash, path);

        TextRenderer.DrawText(g, "QUEUE #", F(8.5f), new Rectangle(inner.X, inner.Y + S(18), inner.Width, S(18)), Theme.Muted, Centered);
        TextRenderer.DrawText(g, _ticket.QueueNumber, F(40f, FontStyle.Bold),
            new Rectangle(inner.X, inner.Y + S(38), inner.Width, S(70)), Theme.Primary, Centered);

        // pet + service
        int boxW = (inner.Width - S(36) - S(12)) / 2;
        var petBox = new Rectangle(inner.X + S(18), inner.Y + S(126), boxW, S(70));
        var serviceBox = new Rectangle(petBox.Right + S(12), petBox.Y, boxW, S(70));
        DrawInfoBox(g, petBox, k, "Pet", _ticket.PetName, _ticket.PetBreed);
        DrawInfoBox(g, serviceBox, k, "Service", _ticket.Service, "~ " + FormatMinutes(_ticket.DurationMinutes));

        // pending vaccine clearance
        var (fore, fill2, border) = AppointmentStatusStyle.Pill(AppointmentStatus.Pending);
        var banner = new Rectangle(inner.X + S(18), inner.Y + S(212), inner.Width - S(36), S(40));
        using (var path = Ui.RoundRect(banner, S(8)))
        using (var b = new SolidBrush(fill2))
        using (var pen = new Pen(border))
        {
            g.FillPath(b, path);
            g.DrawPath(pen, path);
        }
        TextRenderer.DrawText(g, "Pending Vaccine Clearance", F(9.5f, FontStyle.Bold), banner, fore, Centered);

        TextRenderer.DrawText(g, "A staff member will review your vaccine record before your pet is admitted.", F(8f),
            new Rectangle(inner.X + S(18), banner.Bottom + S(8), inner.Width - S(36), S(34)), Theme.Muted,
            TextFormatFlags.Left | TextFormatFlags.WordBreak | TextFormatFlags.NoPrefix);

        // "Ticket saved" pill
        var pill = new Rectangle(S(28), S(540 - 24 - 30), S(120), S(30));
        using (var path = Ui.RoundRect(pill, S(15)))
        using (var b = new SolidBrush(Theme.GreenSoft))
            g.FillPath(b, path);
        TextRenderer.DrawText(g, "\u2713  Ticket saved", F(8.5f, FontStyle.Bold), pill, Color.FromArgb(22, 101, 52), Centered);
    }

    private static void DrawInfoBox(Graphics g, Rectangle r, float k, string caption, string line1, string line2)
    {
        int S(float v) => (int)Math.Round(v * k);
        Font F(float pt, FontStyle st = FontStyle.Regular) => Theme.Face(FullScreen.Pt(pt * k), st);

        using (var path = Ui.RoundRect(r, S(8)))
        using (var b = new SolidBrush(Theme.Bg))
        using (var pen = new Pen(Theme.Border))
        {
            g.FillPath(b, path);
            g.DrawPath(pen, path);
        }
        TextRenderer.DrawText(g, caption, F(7.5f), new Rectangle(r.X + S(14), r.Y + S(8), r.Width - S(28), S(14)), Theme.Muted, LeftAlign);
        TextRenderer.DrawText(g, line1, F(10f, FontStyle.Bold), new Rectangle(r.X + S(14), r.Y + S(24), r.Width - S(28), S(20)), Theme.Text, LeftAlign);
        TextRenderer.DrawText(g, line2, F(8f), new Rectangle(r.X + S(14), r.Y + S(46), r.Width - S(28), S(16)), Theme.Muted, LeftAlign);
    }

    private static string FormatMinutes(int minutes)
        => minutes >= 60 && minutes % 60 == 0 ? $"{minutes / 60} hr" : $"{minutes} mins";
}
