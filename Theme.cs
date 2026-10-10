namespace MyApp;

/// <summary>Colours and fonts used across the dashboard.</summary>
internal static class Theme
{
    private static Color C(string hex) => ColorTranslator.FromHtml(hex);

    public static readonly Color Bg           = C("#F3F7FC");
    public static readonly Color Border       = C("#E3E9F2");
    public static readonly Color Primary      = C("#3A7BD5");
    public static readonly Color PrimarySoft  = C("#EAF2FD");
    public static readonly Color Text         = C("#1F2A3C");
    public static readonly Color TextSoft     = C("#44536A");
    public static readonly Color Muted        = C("#7A889C");
    public static readonly Color Subtle       = C("#F6F8FB");
    public static readonly Color SubtleHover  = C("#E8EFF9");
    public static readonly Color Track        = C("#EDF1F7");
    public static readonly Color Green        = C("#2E9E5B");
    public static readonly Color GreenSoft    = C("#E5F6EC");
    public static readonly Color Purple       = C("#8B5CF6");
    public static readonly Color Orange       = C("#F59E0B");
    public static readonly Color Danger       = C("#EF4444");

    private static readonly Dictionary<(float, FontStyle, bool), Font> Fonts = new();

    /// <summary>Segoe UI text font.</summary>
    public static Font Face(float pt, FontStyle style = FontStyle.Regular) => Get(pt, style, false);

    /// <summary>Segoe MDL2 Assets icon font (built into Windows 10/11).</summary>
    public static Font GlyphFont(float pt) => Get(pt, FontStyle.Regular, true);

    private static Font Get(float pt, FontStyle style, bool glyph)
    {
        var key = (pt, style, glyph);
        if (!Fonts.TryGetValue(key, out var font))
        {
            font = new Font(glyph ? "Segoe MDL2 Assets" : "Segoe UI", pt, style, GraphicsUnit.Point);
            Fonts[key] = font;
        }
        return font;
    }
}
