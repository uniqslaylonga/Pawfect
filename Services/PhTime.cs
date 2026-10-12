namespace Pawfect.Services;

// Philippines is UTC+8 with no daylight saving (same convention the other services use).
public static class PhTime
{
    public static DateTime ToLocal(DateTime utc) => utc.AddHours(8);

    // Npgsql only accepts UTC-kind DateTimes for "timestamp with time zone" columns.
    public static DateTime ToUtc(DateTime local) =>
        DateTime.SpecifyKind(local.AddHours(-8), DateTimeKind.Utc);

    public static DateTime Now => ToLocal(DateTime.UtcNow);
    public static DateOnly Today => DateOnly.FromDateTime(Now);

    // [from 00:00, to+1day 00:00) in UTC for a local date range
    public static (DateTime StartUtc, DateTime EndUtc) RangeUtc(DateOnly from, DateOnly to) =>
        (ToUtc(from.ToDateTime(TimeOnly.MinValue)), ToUtc(to.AddDays(1).ToDateTime(TimeOnly.MinValue)));
}
