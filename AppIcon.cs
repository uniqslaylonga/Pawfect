namespace MyApp;

/// <summary>The Pawfect paw icon (pawfect.ico, embedded in the exe). Shown in the taskbar and in window title bars.</summary>
internal static class AppIcon
{
    private static Icon? _icon;

    public static Icon? Value => _icon ??= Load();

    private static Icon? Load()
    {
        try
        {
            using var stream = typeof(AppIcon).Assembly.GetManifestResourceStream("pawfect.ico");
            return stream is null ? null : new Icon(stream);
        }
        catch
        {
            return null;   // never stop the app because the icon is missing
        }
    }
}
