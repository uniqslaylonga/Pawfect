using System.Globalization;
using System.Text.RegularExpressions;

namespace MyApp;

/// <summary>One row of the vaccination table, as read from the card photo.</summary>
internal sealed record ScannedVaccination(DateTime? Given, string Vaccine, decimal? WeightKg, string BatchNo, DateTime? Expires);

/// <summary>Everything read from a vaccination card photo. Empty text / null means "not found".</summary>
internal sealed record ScannedCard(
    string PetName, string Species, string Breed, string Sex, DateTime? BirthDate, decimal? WeightKg,
    string Owner, string Address, List<ScannedVaccination> Vaccinations)
{
    public bool IsEmpty =>
        PetName.Length == 0 && Species.Length == 0 && Breed.Length == 0 && Sex.Length == 0 &&
        BirthDate is null && WeightKg is null && Owner.Length == 0 && Vaccinations.Count == 0;
}

/// <summary>
/// Turns the lines of text that OCR found on a vaccination card into pet details and vaccination rows.
/// It looks for the printed labels (Pet Name, Species, Breed, Sex, Weight, Birthdate, Owner, Address) in the
/// top part of the card, then reads the table underneath one row at a time (anchored on the dates).
/// Pure text logic, so it does not depend on Windows OCR and can be tested on its own.
/// </summary>
internal static class VaccinationCardParser
{
    private const RegexOptions Opt = RegexOptions.IgnoreCase | RegexOptions.CultureInvariant;

    // ------------------------------------------------------------------ patterns

    private static readonly (string Key, Regex Rx)[] Labels =
    {
        ("name",    new Regex(@"(?<![A-Za-z])(?:pet(?:'?s)?\s*name|name\s*of\s*pet)\s*[:;]?", Opt)),
        ("sex",     new Regex(@"(?<![A-Za-z])(?:sex|gender)(?![A-Za-z])\s*[:;]?", Opt)),
        ("species", new Regex(@"(?<![A-Za-z])species(?![A-Za-z])\s*[:;]?", Opt)),
        ("weight",  new Regex(@"(?<![A-Za-z])weight(?![A-Za-z])\s*[:;]?", Opt)),
        ("breed",   new Regex(@"(?<![A-Za-z])breed(?![A-Za-z])\s*[:;]?", Opt)),
        ("birth",   new Regex(@"(?<![A-Za-z])(?:birth\s*date|date\s*of\s*birth|birthday|d\.?o\.?b\.?)(?![A-Za-z])\s*[:;]?", Opt)),
        // The colon is required here so the heading "PET & OWNER INFORMATION" is not mistaken for the label.
        ("owner",   new Regex(@"(?<![A-Za-z])(?:owner(?:'?s)?(?:\s*name)?|client)\s*[:;]", Opt)),
        ("address", new Regex(@"(?<![A-Za-z])address\s*[:;]?", Opt)),
    };

    // Lines that only exist in the pet/owner block (never in the table), used to find where the table starts.
    private static readonly Regex InfoLine = new(
        @"pet\s*name|species|breed|birth\s*date|date\s*of\s*birth|owner\s*[:;]|address\s*[:;]", Opt);

    private static readonly Regex TableHeading = new(
        @"vaccin\w*\s*(?:&|and|record|history)|deworm\w*\s*record", Opt);

    private static readonly Regex NumericDate = new(
        @"(?<!\d)(\d{1,2})\s*[/\-.]\s*(\d{1,2})\s*[/\-.]\s*(\d{4}|\d{2})(?!\d)", Opt);

    private static readonly Regex TextDate = new(
        @"(?<![A-Za-z])(jan|feb|mar|apr|may|jun|jul|aug|sept?|oct|nov|dec)[a-z]*\.?\s+(\d{1,2})(?:st|nd|rd|th)?,?\s+(\d{4})(?!\d)", Opt);

    private static readonly Regex WeightNumber = new(
        @"(?<![\d.])(\d{1,3}(?:\s*[.,\-]\s*\d{1,2})?)\s*(kgs?|kilos?|ka|kq|lbs?|pounds?)?(?![A-Za-z])", Opt);

    // A weight inside a table row must carry a unit, otherwise dates / batch numbers would look like weights.
    private static readonly Regex RowWeight = new(
        @"(?<![\d.])(\d{1,3}(?:\s*[.,\-]\s*\d{1,2})?)\s*(kgs?|kilos?|ka|kq|lbs?)(?![A-Za-z])", Opt);

    private static readonly Regex BatchNumber = new(
        @"(?<![A-Za-z0-9])([A-Z]{1,4})\s?-\s?(\d{3,8})(?![A-Za-z0-9])", Opt);

    private static readonly (Regex Rx, Func<Match, string> Name)[] VaccineTokens =
    {
        (new Regex(@"(?<![A-Za-z0-9])(\d)\s*-?\s*in\s*-?\s*1(?![A-Za-z0-9])", Opt), m => m.Groups[1].Value + "-in-1"),
        (new Regex(@"(?<![A-Za-z])fvrcp(?![A-Za-z])", Opt), _ => "FVRCP"),
        (new Regex(@"(?<![A-Za-z])(?:dhlpp|dhpp|dhppi|dapp)(?![A-Za-z])", Opt), m => m.Value.ToUpperInvariant()),
        (new Regex(@"(?<![A-Za-z])(?:anti\s*-?\s*rabies|rabies|rabisin)(?![A-Za-z])", Opt), _ => "Anti-Rabies"),
        (new Regex(@"(?<![A-Za-z])(?:bordetella|kennel\s*cough)(?![A-Za-z])", Opt), _ => "Bordetella"),
        (new Regex(@"(?<![A-Za-z])deworm\w*", Opt), _ => "Deworming"),
        (new Regex(@"(?<![A-Za-z])(?:felv|feline\s*leukemia)(?![A-Za-z])", Opt), _ => "FeLV"),
        (new Regex(@"(?<![A-Za-z])lepto\w*", Opt), _ => "Leptospirosis"),
        (new Regex(@"(?<![A-Za-z])parvo\w*", Opt), _ => "Parvovirus"),
        (new Regex(@"(?<![A-Za-z])corona\w*", Opt), _ => "Coronavirus"),
    };

    // ------------------------------------------------------------------ entry point

    public static ScannedCard Parse(IReadOnlyList<string> rawLines)
    {
        var lines = rawLines.Select(l => l.Trim()).Where(l => l.Length > 0).ToList();

        // Where does the vaccination table start? After the last pet/owner label, at the table heading.
        int lastInfo = lines.FindLastIndex(l => InfoLine.IsMatch(l));
        int tableStart = -1;
        for (int i = Math.Max(lastInfo + 1, 0); i < lines.Count; i++)
        {
            if (TableHeading.IsMatch(lines[i])) { tableStart = i; break; }
        }
        if (tableStart < 0) tableStart = lastInfo + 1;

        var values = ReadLabelledValues(string.Join(" ", lines.Take(tableStart)));
        string Get(string key) => values.TryGetValue(key, out var v) ? v : "";

        var birthDates = FindDates(Get("birth"), out _);
        DateTime? birth = birthDates.Count > 0 ? birthDates[0] : null;

        return new ScannedCard(
            PetName: Get("name"),
            Species: NormalizeSpecies(Get("species")),
            Breed: Get("breed"),
            Sex: NormalizeSex(Get("sex")),
            BirthDate: birth,
            WeightKg: TryParseWeight(Get("weight"), out var w) ? w : null,
            Owner: Get("owner"),
            Address: Get("address"),
            Vaccinations: ReadTable(lines, tableStart));
    }

    // ------------------------------------------------------------------ pet & owner block

    private static Dictionary<string, string> ReadLabelledValues(string zone)
    {
        var found = new List<(string Key, int Start, int End)>();
        foreach (var (key, rx) in Labels)
        {
            var matches = rx.Matches(zone);
            if (matches.Count == 0) continue;
            // The owner's address is the last "Address:" on the card; the clinic's own address comes first.
            var m = key == "address" ? matches[matches.Count - 1] : matches[0];
            found.Add((key, m.Index, m.Index + m.Length));
        }
        found.Sort((a, b) => a.Start.CompareTo(b.Start));

        var values = new Dictionary<string, string>();
        for (int i = 0; i < found.Count; i++)
        {
            int valueStart = found[i].End;
            int valueEnd = i + 1 < found.Count ? found[i + 1].Start : zone.Length;
            if (valueEnd < valueStart) continue;
            values[found[i].Key] = Clean(zone.Substring(valueStart, valueEnd - valueStart));
        }
        return values;
    }

    private static string Clean(string s)
    {
        s = Regex.Replace(s, @"[_|\[\]{}]+", " ");
        s = Regex.Replace(s, @"\s+", " ");
        return s.Trim(' ', ':', ';', '.', ',', '-', '\'', '"');
    }

    private static string NormalizeSpecies(string s)
    {
        if (s.Length == 0) return "";
        if (Regex.IsMatch(s, @"feline|\bcat\b|\bpusa\b", Opt)) return "Cat";
        if (Regex.IsMatch(s, @"canine|\bdog\b|\baso\b", Opt)) return "Dog";
        return s;
    }

    private static string NormalizeSex(string s)
    {
        if (Regex.IsMatch(s, @"\bfemale\b|\bspayed\b|^f$", Opt)) return "Female";
        if (Regex.IsMatch(s, @"\bmale\b|\bneutered\b|^m$", Opt)) return "Male";
        return "";
    }

    // ------------------------------------------------------------------ vaccination table

    private static List<ScannedVaccination> ReadTable(List<string> lines, int tableStart)
    {
        var result = new List<ScannedVaccination>();

        // Every line with a date is the "middle" of one table row.
        var anchors = new List<int>();
        for (int i = tableStart; i < lines.Count; i++)
        {
            if (FindDates(lines[i], out _).Count > 0) anchors.Add(i);
        }

        for (int k = 0; k < anchors.Count; k++)
        {
            // A row also owns the cell text just above / below its date line, up to halfway to the next row.
            int start = k == 0 ? Math.Max(tableStart + 1, anchors[k] - 2) : (anchors[k - 1] + anchors[k] + 1) / 2;
            int end = k == anchors.Count - 1 ? Math.Min(lines.Count - 1, anchors[k] + 3) : (anchors[k] + anchors[k + 1]) / 2;
            if (end < start) end = start;

            var row = ParseRow(string.Join(" ", lines.Skip(start).Take(end - start + 1)));
            // A line with a date but no vaccine or batch (e.g. a stray birthdate) is not a vaccination.
            if (row.Vaccine.Length > 0 || row.BatchNo.Length > 0) result.Add(row);
        }
        return result;
    }

    private static ScannedVaccination ParseRow(string text)
    {
        var dates = FindDates(text, out string rest);
        DateTime? given = dates.Count > 0 ? dates[0] : null;
        DateTime? expires = dates.Count > 1 ? dates[dates.Count - 1] : null;
        if (given is { } g && expires is { } e && e < g) (given, expires) = (e, g);

        decimal? weight = null;
        var wm = RowWeight.Match(rest);
        if (wm.Success)
        {
            if (TryParseWeight(wm.Value, out var w)) weight = w;
            rest = rest.Remove(wm.Index, wm.Length).Insert(wm.Index, " ");
        }

        string batch = "";
        foreach (Match bm in BatchNumber.Matches(rest))
        {
            string prefix = bm.Groups[1].Value.ToUpperInvariant();
            if (prefix is "PRC" or "LIC" or "NO") continue; // licence numbers printed in the vet's signature block
            batch = prefix + "-" + bm.Groups[2].Value;
            rest = rest.Remove(bm.Index, bm.Length).Insert(bm.Index, " ");
            break;
        }

        var tokens = new List<(int Index, string Name)>();
        foreach (var (rx, name) in VaccineTokens)
            foreach (Match m in rx.Matches(rest))
                tokens.Add((m.Index, name(m)));

        // "Feline Distemper" is usually just the sub-title of FVRCP, so it only counts when nothing else was found.
        if (tokens.Count == 0)
        {
            var d = Regex.Match(rest, @"(?<![A-Za-z])distemper(?![A-Za-z])", Opt);
            if (d.Success) tokens.Add((d.Index, "Distemper"));
        }

        // "5-in-1 DHPPi (Parvo/Distemper)": the bracket only lists what the combined shot covers, so it is not a vaccine of its own.
        if (tokens.Any(t => t.Name.EndsWith("-in-1") || t.Name is "FVRCP" or "DHLPP" or "DHPP" or "DHPPI" or "DAPP"))
            tokens.RemoveAll(t => t.Name is "Parvovirus" or "Coronavirus");

        string vaccine = string.Join(" ", tokens.OrderBy(t => t.Index).Select(t => t.Name).Distinct());
        return new ScannedVaccination(given, vaccine, weight, batch, expires);
    }

    // ------------------------------------------------------------------ small parsers (also used by the form)

    /// <summary>Finds dates written as 02/15/2026, 2-15-26 or "Feb 15, 2026". Returns them in reading order and the text without them.</summary>
    public static List<DateTime> FindDates(string text, out string remainder)
    {
        var found = new List<DateTime>();

        string rest = NumericDate.Replace(text, m =>
        {
            var date = MakeDate(int.Parse(m.Groups[1].Value, CultureInfo.InvariantCulture),
                                int.Parse(m.Groups[2].Value, CultureInfo.InvariantCulture),
                                int.Parse(m.Groups[3].Value, CultureInfo.InvariantCulture));
            if (date is null) return m.Value;
            found.Add(date.Value);
            return " ";
        });

        rest = TextDate.Replace(rest, m =>
        {
            int month = DateTime.ParseExact(m.Groups[1].Value[..3], "MMM", CultureInfo.InvariantCulture).Month;
            var date = MakeDate(month, int.Parse(m.Groups[2].Value, CultureInfo.InvariantCulture),
                                int.Parse(m.Groups[3].Value, CultureInfo.InvariantCulture));
            if (date is null) return m.Value;
            found.Add(date.Value);
            return " ";
        });

        remainder = rest;
        return found;
    }

    public static bool TryParseDate(string text, out DateTime date)
    {
        var dates = FindDates(text, out _);
        date = dates.Count > 0 ? dates[0] : default;
        return dates.Count > 0;
    }

    /// <summary>Reads "3.8 kg", "3-8 ka" (OCR slip), "3,8" or "8 lbs" and returns kilograms.</summary>
    public static bool TryParseWeight(string text, out decimal kg)
    {
        kg = 0;
        var m = WeightNumber.Match(text);
        if (!m.Success) return false;

        string number = Regex.Replace(m.Groups[1].Value, @"\s+", "").Replace(',', '.').Replace('-', '.');
        if (!decimal.TryParse(number, NumberStyles.Number, CultureInfo.InvariantCulture, out var value)) return false;

        if (m.Groups[2].Value.StartsWith("lb", StringComparison.OrdinalIgnoreCase) ||
            m.Groups[2].Value.StartsWith("pound", StringComparison.OrdinalIgnoreCase))
            value = Math.Round(value * 0.45359237m, 2);

        if (value < 0.05m || value > 150m) return false;
        kg = value;
        return true;
    }

    /// <summary>Month first (02/15/2026) like the cards in the shop; day first only when the first number can't be a month.</summary>
    private static DateTime? MakeDate(int first, int second, int year)
    {
        if (year < 100) year += 2000;
        if (year is < 1990 or > 2100) return null;

        int month = first, day = second;
        if (first > 12 && second <= 12) { month = second; day = first; }
        if (month is < 1 or > 12) return null;
        if (day < 1 || day > DateTime.DaysInMonth(year, month)) return null;
        return new DateTime(year, month, day);
    }
}
