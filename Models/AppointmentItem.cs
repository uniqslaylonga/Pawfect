namespace MyApp;

internal enum AppointmentStatus
{
    Pending,
    Confirmed,
    CheckedIn,
    Rescheduled,
    Cancelled,
}

/// <summary>One booked appointment. Fill these from your data source and pass them to <c>AppointmentsPage.SetAppointments</c>.</summary>
internal sealed record AppointmentItem(
    int Id,
    DateTime Start,
    string CustomerName,
    string CustomerPhone,
    string PetName,
    string PetBreed,
    string Service,
    AppointmentStatus Status);

/// <summary>Label and colours for each appointment status.</summary>
internal static class AppointmentStatusStyle
{
    private static Color C(string hex) => ColorTranslator.FromHtml(hex);

    public static string Label(AppointmentStatus status) => status switch
    {
        AppointmentStatus.Pending => "Pending",
        AppointmentStatus.Confirmed => "Confirmed",
        AppointmentStatus.CheckedIn => "Checked In",
        AppointmentStatus.Rescheduled => "Rescheduled",
        AppointmentStatus.Cancelled => "Cancelled",
        _ => status.ToString(),
    };

    public static (Color Fore, Color Fill, Color Border) Pill(AppointmentStatus status) => status switch
    {
        AppointmentStatus.Confirmed => (C("#2E9E5B"), C("#E5F6EC"), C("#BFE5CE")),
        AppointmentStatus.CheckedIn => (C("#0E8F9E"), C("#E0F5F7"), C("#B5E3E8")),
        AppointmentStatus.Pending => (C("#C77A0A"), C("#FEF3E0"), C("#F8D9A0")),
        AppointmentStatus.Rescheduled => (C("#7C4DDB"), C("#F1EAFD"), C("#D9C8F8")),
        AppointmentStatus.Cancelled => (C("#D93636"), C("#FDE8E8"), C("#F6C0C0")),
        _ => (Theme.TextSoft, Theme.Subtle, Theme.Border),
    };

    /// <summary>Colour of the small dot shown on the calendar and in its legend.</summary>
    public static Color Marker(AppointmentStatus status) => status switch
    {
        AppointmentStatus.Confirmed or AppointmentStatus.CheckedIn => Theme.Green,
        AppointmentStatus.Pending => Theme.Orange,
        AppointmentStatus.Rescheduled => Theme.Purple,
        AppointmentStatus.Cancelled => Theme.Danger,
        _ => Theme.Muted,
    };

    /// <summary>Draws the rounded status badge and returns its width.</summary>
    public static int DrawPill(Graphics g, AppointmentStatus status, int x, int centerY, int maxWidth)
    {
        var (fore, fill, border) = Pill(status);
        var font = Theme.Face(8f, FontStyle.Bold);
        string text = Label(status);
        int w = Math.Min(maxWidth, TextRenderer.MeasureText(text, font).Width + 18);
        var rect = new Rectangle(x, centerY - 11, Math.Max(10, w), 22);

        var old = g.SmoothingMode;
        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
        using (var path = Ui.RoundRect(new RectangleF(rect.X + 0.5f, rect.Y + 0.5f, rect.Width - 1, rect.Height - 1), 11))
        using (var b = new SolidBrush(fill))
        using (var pen = new Pen(border))
        {
            g.FillPath(b, path);
            g.DrawPath(pen, path);
        }
        g.SmoothingMode = old;

        TextRenderer.DrawText(g, text, font, rect, fore,
            TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding |
            TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine | TextFormatFlags.EndEllipsis);
        return rect.Width;
    }
}
