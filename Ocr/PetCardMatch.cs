using MyApp.Controls;
using System.Globalization;
using System.Text.RegularExpressions;

namespace MyApp;

/// <summary>One detail that differs between what the customer entered and what the vaccine card says.</summary>
internal sealed record PetMismatch(string Field, string Entered, string OnCard);

/// <summary>
/// Checks that the pet details entered in the Pet Details pop-up agree with the vaccination card that was scanned.
/// A card that belongs to a different pet is not accepted. Name and species must always match and must be readable on the card.
/// Breed, sex, age and weight are compared when both sides have a value; a field the card simply doesn't state is not a mismatch.
/// Pure logic (no UI, no OCR), so it can be tested on its own.
/// </summary>
internal static class PetCardMatch
{
    private const RegexOptions Opt = RegexOptions.IgnoreCase | RegexOptions.CultureInvariant;

    /// <summary>Ages are entered as whole years, so the card's birthdate may differ by this many years.</summary>
    private const int AgeToleranceYears = 1;

    /// <summary>A pet's weight changes between the shot and today: allow this share of the larger weight, but never less than the minimum.</summary>
    private const decimal WeightTolerancePercent = 0.30m;
    private const decimal WeightToleranceMinKg = 1.5m;

    /// <summary>Returns every detail that disagrees. An empty list means the card matches the pet.</summary>
    public static List<PetMismatch> Compare(PetProfile pet, ScannedCard card)
    {
        var problems = new List<PetMismatch>();

        // ---- name (required)
        if (card.PetName.Length == 0)
            problems.Add(new PetMismatch("Pet name", pet.Name, "not readable"));
        else if (!SameName(pet.Name, card.PetName))
            problems.Add(new PetMismatch("Pet name", pet.Name, card.PetName));

        // ---- species (required)
        string petSpecies = NormalizeSpecies(pet.Species);
        string cardSpecies = NormalizeSpecies(card.Species);
        if (cardSpecies.Length == 0)
            problems.Add(new PetMismatch("Type of pet", pet.Species, "not readable"));
        else if (petSpecies.Length > 0 && !string.Equals(petSpecies, cardSpecies, StringComparison.OrdinalIgnoreCase))
            problems.Add(new PetMismatch("Type of pet", pet.Species, card.Species));

        // ---- breed
        if (pet.Breed.Length > 0 && card.Breed.Length > 0 && !SameBreed(pet.Breed, card.Breed))
            problems.Add(new PetMismatch("Breed", pet.Breed, card.Breed));

        // ---- sex
        if (pet.Gender.Length > 0 && card.Sex.Length > 0 &&
            !string.Equals(pet.Gender, card.Sex, StringComparison.OrdinalIgnoreCase))
            problems.Add(new PetMismatch("Gender", pet.Gender, card.Sex));

        // ---- age (the pop-up stores an age bracket, so compare whole years)
        if (pet.BirthDate is { } enteredBirth && card.BirthDate is { } cardBirth)
        {
            int enteredYears = AgeInYears(enteredBirth), cardYears = AgeInYears(cardBirth);
            if (Math.Abs(enteredYears - cardYears) > AgeToleranceYears)
                problems.Add(new PetMismatch("Age", AgeText(enteredYears), AgeText(cardYears)));
        }

        // ---- weight
        if (pet.WeightKg is { } enteredKg && card.WeightKg is { } cardKg)
        {
            decimal allowed = Math.Max(WeightToleranceMinKg, Math.Max(enteredKg, cardKg) * WeightTolerancePercent);
            if (Math.Abs(enteredKg - cardKg) > allowed)
                problems.Add(new PetMismatch("Weight", Kg(enteredKg), Kg(cardKg)));
        }

        return problems;
    }

    /// <summary>The owner printed on the card must be the account holder. An owner that can't be read is refused too, since it can't be checked.</summary>
    public static PetMismatch? CompareOwner(string accountName, ScannedCard card)
    {
        if (Tokens(accountName).Count == 0) return null;   // no account name to compare with
        if (card.Owner.Length == 0) return new PetMismatch("Owner", accountName, "not readable");
        return SameOwner(accountName, card.Owner) ? null : new PetMismatch("Owner", accountName, card.Owner);
    }

    /// <summary>
    /// Vaccines on the card that are no longer in date. Each vaccine is judged by its newest record, so an old shot that was already
    /// renewed on the same card doesn't count. An expiry date that couldn't be read can't be shown to be current, so it counts as lapsed.
    /// Rows come back as (vaccine, expiry date, status) for the pop-up.
    /// </summary>
    public static List<PetMismatch> Expired(IEnumerable<ScannedVaccination> rows)
    {
        var today = DateTime.Today;
        var lapsed = new List<PetMismatch>();
        foreach (var group in rows.Where(r => !string.IsNullOrWhiteSpace(r.Vaccine))
                                  .GroupBy(r => r.Vaccine.Trim(), StringComparer.OrdinalIgnoreCase))
        {
            if (group.Any(r => r.Expires is { } e && e.Date >= today)) continue;
            var newest = group.Where(r => r.Expires is not null).OrderByDescending(r => r.Expires).FirstOrDefault();
            lapsed.Add(newest?.Expires is { } d
                ? new PetMismatch(group.Key, d.ToString("MMM d, yyyy", CultureInfo.InvariantCulture), "Expired")
                : new PetMismatch(group.Key, "\u2014", "Expiry not readable"));
        }
        return lapsed;
    }

    /// <summary>A vaccine only protects the pet this many days after the shot was given.</summary>
    internal const int ImmunityDays = 14;

    /// <summary>True when the card is for a cat (the 14-day immunity rule is checked for cats).</summary>
    internal static bool IsCat(ScannedCard card)
        => string.Equals(NormalizeSpecies(card.Species), "Cat", StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// Vaccines whose 14-day immunity period is not finished yet. A vaccine passes when at least one of its records on the card was given
    /// 14 or more days ago, so a fresh booster doesn't block a pet that was already protected. Deworming has no immunity period and is skipped.
    /// A date that couldn't be read can't be shown to be old enough, so it counts as not done.
    /// Rows come back as (vaccine, date given, date protected from) for the pop-up.
    /// </summary>
    public static List<PetMismatch> ImmunityPending(IEnumerable<ScannedVaccination> rows)
    {
        var today = DateTime.Today;
        var pending = new List<PetMismatch>();
        foreach (var group in rows.Where(r => !string.IsNullOrWhiteSpace(r.Vaccine) &&
                                              !r.Vaccine.Contains("deworm", StringComparison.OrdinalIgnoreCase))
                                  .GroupBy(r => r.Vaccine.Trim(), StringComparer.OrdinalIgnoreCase))
        {
            if (group.Any(r => r.Given is { } g && g.Date.AddDays(ImmunityDays) <= today)) continue;
            var newest = group.Where(r => r.Given is not null).OrderByDescending(r => r.Given).FirstOrDefault();
            pending.Add(newest?.Given is { } given
                ? new PetMismatch(group.Key,
                    given.ToString("MMM d, yyyy", CultureInfo.InvariantCulture),
                    given.Date.AddDays(ImmunityDays).ToString("MMM d, yyyy", CultureInfo.InvariantCulture))
                : new PetMismatch(group.Key, "\u2014", "Date not readable"));
        }
        return pending;
    }

    // ------------------------------------------------------------------ comparisons

    private static string Letters(string s)
        => new(s.Where(char.IsLetterOrDigit).Select(char.ToLowerInvariant).ToArray());

    /// <summary>Same name ignoring case, spaces and punctuation. One OCR slip is forgiven only in longer names.</summary>
    internal static bool SameName(string entered, string onCard)
    {
        string a = Letters(entered), b = Letters(onCard);
        if (a.Length == 0 || b.Length == 0) return false;
        if (a == b) return true;
        return Math.Min(a.Length, b.Length) >= 6 && Distance(a, b) <= 1;
    }

    private static List<string> Tokens(string name)
        => Regex.Split(name.ToLowerInvariant(), @"[^\p{L}]+").Where(t => t.Length > 0).ToList();

    /// <summary>
    /// Same person written differently: order doesn't matter ("Santos, Maria"), an abbreviation counts ("Ma." for "Maria", "J." for "Juan"),
    /// and one OCR slip is forgiven in longer words. Every word of the shorter name must be found in the other.
    /// </summary>
    internal static bool SameOwner(string account, string onCard)
    {
        var a = Tokens(account); var b = Tokens(onCard);
        if (a.Count == 0 || b.Count == 0) return false;
        var (shorter, longer) = a.Count <= b.Count ? (a, b) : (b, a);
        var pool = longer.ToList();
        foreach (var word in shorter)
        {
            int hit = pool.FindIndex(w => w == word || w.StartsWith(word) || word.StartsWith(w) ||
                                          (Math.Min(w.Length, word.Length) >= 6 && Distance(w, word) <= 1));
            if (hit < 0) return false;
            pool.RemoveAt(hit);
        }
        return true;
    }

    private static string NormalizeSpecies(string s)
    {
        if (s.Length == 0) return "";
        if (Regex.IsMatch(s, @"feline|\bcat\b|\bpusa\b", Opt)) return "Cat";
        if (Regex.IsMatch(s, @"canine|\bdog\b|\baso\b", Opt)) return "Dog";
        return s.Trim();
    }

    /// <summary>"Aspin (Mixed)" and "Aspin", "Shih-Tzu" and "Shih Tzu" are the same breed; "Other" on the pop-up says nothing, so it matches anything.</summary>
    internal static bool SameBreed(string entered, string onCard)
    {
        string a = BreedKey(entered), b = BreedKey(onCard);
        if (a.Length == 0 || b.Length == 0 || a == "other") return true;
        return a == b || a.Contains(b) || b.Contains(a);
    }

    private static string BreedKey(string breed)
    {
        // Philippine mixed breeds are written several ways on cards
        breed = Regex.Replace(breed, @"\b(?:mixed|mix|breed|crossbreed|native|domestic|short\s*hair(?:ed)?)\b", " ", Opt);
        breed = Regex.Replace(breed, @"\b(?:aspin|asong\s*pinoy|puspin|pusang\s*pinoy)\b", "mixed", Opt);
        return Letters(breed);
    }

    private static int AgeInYears(DateTime birth)
    {
        int years = DateTime.Today.Year - birth.Year;
        if (birth.Date > DateTime.Today.AddYears(-years)) years--;
        return Math.Clamp(years, 0, 15);
    }

    private static string AgeText(int years) => years switch { 0 => "under 1 year", 1 => "1 year", 15 => "15+ years", _ => years + " years" };

    private static string Kg(decimal kg) => kg.ToString("0.##", CultureInfo.InvariantCulture) + " kg";

    private static int Distance(string a, string b)
    {
        var row = Enumerable.Range(0, b.Length + 1).ToArray();
        for (int i = 1; i <= a.Length; i++)
        {
            int diagonal = row[0];
            row[0] = i;
            for (int j = 1; j <= b.Length; j++)
            {
                int above = row[j];
                row[j] = Math.Min(Math.Min(row[j] + 1, row[j - 1] + 1), diagonal + (a[i - 1] == b[j - 1] ? 0 : 1));
                diagonal = above;
            }
        }
        return row[b.Length];
    }
}

/// <summary>The pop-up shown when the card belongs to a different pet: what was entered next to what the card says.</summary>
internal sealed class CardMismatchDialog : Form
{
    private const int W = 520, Pad = 28, RowH = 54, HeaderH = 96, FooterH = 76;
    private static readonly Color DangerSoft = Color.FromArgb(254, 242, 242);
    private static readonly Color DangerText = Color.FromArgb(185, 28, 28);

    private readonly IReadOnlyList<PetMismatch> _rows;

    private readonly string _title, _subtitle, _col2, _col3;

    public CardMismatchDialog(IReadOnlyList<PetMismatch> rows, string title = "Vaccine card doesn't match",
        string subtitle = "The details must be the same as the vaccine card.", string col2 = "YOURS", string col3 = "ON THE CARD")
    {
        _rows = rows;
        _title = title; _subtitle = subtitle; _col2 = col2; _col3 = col3;
        Icon = AppIcon.Value;
        Font = Theme.Face(9f);
        BackColor = Color.White;
        FormBorderStyle = FormBorderStyle.None;
        StartPosition = FormStartPosition.CenterParent;
        ShowInTaskbar = false;
        DoubleBuffered = true;
        int tableH = 30 + rows.Count * RowH;
        ClientSize = new Size(W, HeaderH + tableH + FooterH);

        var ok = new PawButton { Text = "Try again", Kind = PawButtonKind.Primary, Size = new Size(140, 40) };
        ok.Location = new Point(W - Pad - ok.Width, ClientSize.Height - FooterH + 18);
        ok.Click += (_, _) => DialogResult = DialogResult.OK;
        Controls.Add(ok);
        AcceptButton = ok;
        CancelButton = ok;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        var g = e.Graphics;
        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
        const TextFormatFlags L = TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix;

        using (var pen = new Pen(Theme.Border)) g.DrawRectangle(pen, 0, 0, Width - 1, Height - 1);

        // header: red badge, title, one-line explanation
        var badge = new Rectangle(Pad, 26, 44, 44);
        using (var b = new SolidBrush(DangerSoft)) g.FillEllipse(b, badge);
        TextRenderer.DrawText(g, "\uE783", Theme.GlyphFont(15f), badge, Theme.Danger,
            TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);
        TextRenderer.DrawText(g, _title, Theme.Face(13f, FontStyle.Bold), new Rectangle(Pad + 58, 24, W - Pad * 2 - 58, 26), Theme.Text, L);
        TextRenderer.DrawText(g, _subtitle, Theme.Face(9f), new Rectangle(Pad + 58, 52, W - Pad * 2 - 58, 20), Theme.Muted, L);

        // table
        int x = Pad, w = W - Pad * 2, y = HeaderH;
        int c1 = x + 14, c2 = x + 150, c3 = x + 150 + (w - 150) / 2;
        var tableRect = new Rectangle(x, y, w, 30 + _rows.Count * RowH);
        using (var path = Ui.RoundRect(tableRect, 12))
        {
            using var fill = new SolidBrush(Color.White);
            using var pen = new Pen(Theme.Border);
            g.FillPath(fill, path);
            g.DrawPath(pen, path);
        }
        var head = new Rectangle(x + 1, y + 1, w - 2, 29);
        using (var path = Ui.RoundRect(head, 11)) { using var b = new SolidBrush(Theme.Subtle); g.FillPath(b, path); }
        var small = Theme.Face(8f, FontStyle.Bold);
        TextRenderer.DrawText(g, "DETAIL", small, new Rectangle(c1, y, 120, 30), Theme.Muted, L);
        TextRenderer.DrawText(g, _col2, small, new Rectangle(c2, y, 150, 30), Theme.Muted, L);
        TextRenderer.DrawText(g, _col3, small, new Rectangle(c3, y, 150, 30), Theme.Muted, L);

        for (int i = 0; i < _rows.Count; i++)
        {
            var r = _rows[i];
            int top = y + 30 + i * RowH;
            using (var pen = new Pen(Theme.Border)) g.DrawLine(pen, x, top, x + w, top);
            TextRenderer.DrawText(g, r.Field, Theme.Face(9f, FontStyle.Bold), new Rectangle(c1, top, 130, RowH), Theme.Text, L);
            TextRenderer.DrawText(g, r.Entered.Length == 0 ? "\u2014" : r.Entered, Theme.Face(9.5f), new Rectangle(c2, top, c3 - c2 - 8, RowH), Theme.TextSoft, L);

            string card = r.OnCard == "not readable" ? "Couldn't read" : r.OnCard;
            var size = TextRenderer.MeasureText(g, card, Theme.Face(9f, FontStyle.Bold));
            var chip = new Rectangle(c3, top + (RowH - 28) / 2, Math.Min(x + w - 10 - c3, size.Width + 22), 28);
            using (var path = Ui.RoundRect(chip, 14)) { using var b = new SolidBrush(DangerSoft); g.FillPath(b, path); }
            TextRenderer.DrawText(g, card, Theme.Face(9f, FontStyle.Bold), Rectangle.Inflate(chip, -11, 0), DangerText, L);
        }
    }
}