using System.Text.RegularExpressions;

namespace MyApp;

/// <summary>What counts as a pet name, and how a long one is shortened for banners.</summary>
internal static partial class PetNameRules
{
    // letters (any language), spaces and the few marks real pet names use: Rex, Mr. Whiskers, Luna-Belle, O'Malley
    [GeneratedRegex(@"^(?=.*\p{L})[\p{L}\p{M}0-9 .'\-]+$")]
    private static partial Regex Allowed();

    public const string Hint = "Pet names can only use letters, spaces, apostrophes, hyphens and periods.";

    public static bool IsValid(string name) => Allowed().IsMatch(name.Trim());

    /// <summary>The name as it should appear inside a sentence: trimmed, and cut with "..." when very long.</summary>
    public static string Short(string name, int max = 24)
    {
        name = name.Trim();
        return name.Length <= max ? name : name[..(max - 1)].TrimEnd() + "\u2026";
    }
}
