namespace MyApp;

/// <summary>Philippine mobile numbers: exactly 11 digits, starting with 09 (e.g. 09171234567).</summary>
internal static class PhoneRules
{
    public const int Length = 11;
    public const string Hint = "Enter an 11-digit mobile number starting with 09, like 09171234567.";

    public static bool IsValid(string? phone)
    {
        string p = (phone ?? "").Trim();
        return p.Length == Length && p.StartsWith("09", StringComparison.Ordinal) && p.All(char.IsAsciiDigit);
    }

    /// <summary>Drops everything but digits and keeps at most 11 of them (used while typing and pasting).</summary>
    public static string Clean(string? text) => new((text ?? "").Where(char.IsAsciiDigit).Take(Length).ToArray());
}
