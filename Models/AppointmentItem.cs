namespace MyApp;

internal enum AppointmentStatus
{
    Pending,
    Confirmed,
    CheckedIn,
    Rescheduled,
    Cancelled,
    Completed,
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
    AppointmentStatus Status,
    string Category = "",
    string Groomer = "",
    string Room = "",
    string Notes = "",
    string QueueNumber = "",
    byte[]? PetPhoto = null,
    string PreferredDays = "",   // comma list the customer picked, e.g. "Tue,Wed" (empty = no preference / older requests)
    int ServiceId = 0)           // the exact dbo.Services row booked (the weight band, so billing uses the same price)
{
    /// <summary>The preferred days as day-of-week values (empty when the customer didn't pick any).</summary>
    public IReadOnlyList<DayOfWeek> PreferredDayList => PreferredDaysText.Parse(PreferredDays);
}

/// <summary>Reads/writes the preferred-days list stored on an appointment ("Mon,Tue,...") and builds readable text.</summary>
internal static class PreferredDaysText
{
    private static readonly string[] Short = { "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat" };
    private static readonly string[] Long = { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };

    public static string ShortName(DayOfWeek d) => Short[(int)d];

    public static string Join(IEnumerable<DayOfWeek> days)
        => string.Join(",", days.Distinct().OrderBy(d => ((int)d + 6) % 7).Select(ShortName));   // Monday first

    public static List<DayOfWeek> Parse(string? text)
    {
        var list = new List<DayOfWeek>();
        if (string.IsNullOrWhiteSpace(text)) return list;
        foreach (var part in text.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
        {
            int i = Array.FindIndex(Short, x => string.Equals(x, part, StringComparison.OrdinalIgnoreCase));
            if (i >= 0 && !list.Contains((DayOfWeek)i)) list.Add((DayOfWeek)i);
        }
        return list;
    }

    /// <summary>"Tuesday and Wednesday" / "Mon, Wed and Fri".</summary>
    public static string Readable(IReadOnlyList<DayOfWeek> days, bool full = false)
    {
        var names = days.OrderBy(d => ((int)d + 6) % 7).Select(d => full ? Long[(int)d] : Short[(int)d]).ToList();
        return names.Count switch
        {
            0 => "",
            1 => names[0],
            _ => string.Join(", ", names.Take(names.Count - 1)) + " and " + names[^1],
        };
    }

    /// <summary>The first date on or after <paramref name="from"/> that falls on one of the days.</summary>
    public static DateTime NextOnOrAfter(DateTime from, IReadOnlyList<DayOfWeek> days)
    {
        if (days.Count == 0) return from.Date;
        for (int i = 0; i < 7; i++)
            if (days.Contains(from.Date.AddDays(i).DayOfWeek)) return from.Date.AddDays(i);
        return from.Date;
    }
}

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
        AppointmentStatus.Completed => "Completed",
        _ => status.ToString(),
    };

    public static (Color Fore, Color Fill, Color Border) Pill(AppointmentStatus status) => status switch
    {
        AppointmentStatus.Confirmed => (C("#2E9E5B"), C("#E5F6EC"), C("#BFE5CE")),
        AppointmentStatus.CheckedIn => (C("#0E8F9E"), C("#E0F5F7"), C("#B5E3E8")),
        AppointmentStatus.Pending => (C("#C77A0A"), C("#FEF3E0"), C("#F8D9A0")),
        AppointmentStatus.Rescheduled => (C("#7C4DDB"), C("#F1EAFD"), C("#D9C8F8")),
        AppointmentStatus.Cancelled => (C("#D93636"), C("#FDE8E8"), C("#F6C0C0")),
        AppointmentStatus.Completed => (C("#3A7BD5"), C("#EAF2FD"), C("#C5DAF6")),
        _ => (Theme.TextSoft, Theme.Subtle, Theme.Border),
    };

    /// <summary>Colour of the small dot shown on the calendar and in its legend.</summary>
    public static Color Marker(AppointmentStatus status) => status switch
    {
        AppointmentStatus.Confirmed or AppointmentStatus.CheckedIn => Theme.Green,
        AppointmentStatus.Pending => Theme.Orange,
        AppointmentStatus.Rescheduled => Theme.Purple,
        AppointmentStatus.Cancelled => Theme.Danger,
        AppointmentStatus.Completed => Theme.Primary,
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
