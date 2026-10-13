using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace MyApp;

/// <summary>One piece of text the OCR found, with where it sits in the picture (pixels).</summary>
internal readonly record struct OcrPiece(double Left, double Top, double Right, double Bottom, string Text)
{
    public double CenterX => (Left + Right) / 2;
    public double CenterY => (Top + Bottom) / 2;
    public double Width => Right - Left;
    public double Height => Bottom - Top;
}

/// <summary>One table row found on the card, and whether the veterinarian's stamp is in its signature cell.</summary>
internal sealed record StampRow(DateTime? Given, bool Stamped, string How);

/// <summary>Result of looking for the vet stamps. <see cref="ColumnFound"/> is false when the signature column could not be located.</summary>
internal sealed record StampCheck(bool ColumnFound, List<StampRow> Rows);

/// <summary>The picture as raw pixels (B, G, R, A) so regions can be measured quickly.</summary>
internal sealed class PixelGrid
{
    public int Width { get; }
    public int Height { get; }
    public int Stride { get; }
    public byte[] Data { get; }

    private PixelGrid(int width, int height, int stride, byte[] data)
    {
        Width = width; Height = height; Stride = stride; Data = data;
    }

    public static PixelGrid Load(byte[] image)
    {
        using var source = new MemoryStream(image);
        using var bitmap = new Bitmap(source);
        var data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
        try
        {
            var buffer = new byte[Math.Abs(data.Stride) * bitmap.Height];
            Marshal.Copy(data.Scan0, buffer, 0, buffer.Length);
            return new PixelGrid(bitmap.Width, bitmap.Height, data.Stride, buffer);
        }
        finally
        {
            bitmap.UnlockBits(data);
        }
    }

    public void Get(int x, int y, out int r, out int g, out int b)
    {
        int i = y * Stride + x * 4;
        b = Data[i]; g = Data[i + 1]; r = Data[i + 2];
    }

    public Rectangle Clip(Rectangle area)
    {
        area.Intersect(new Rectangle(0, 0, Width, Height));
        return area;
    }
}

/// <summary>
/// Checks that every vaccination row on the card carries the veterinarian's stamp / signature in the
/// "Veterinarian's Signature/License" column. A row with an empty signature cell is not accepted.
/// The column and the rows are located from the OCR text positions; the cell is then looked at as a picture:
/// a stamp is a large patch of blue (or red) ink spread over the cell. If the stamp text
/// itself was read by the OCR (DVM, PRC No.) that counts too, which also covers stamps in other ink colours.
/// </summary>
internal static class StampVerifier
{
    private const RegexOptions Opt = RegexOptions.IgnoreCase | RegexOptions.CultureInvariant;

    // The words of the column heading. "License" is left out on purpose: the clinic's own heading at the top of the card has it.
    private static readonly Regex HeaderWord = new(@"signature|veterinarian'?s?(?![A-Za-z])", Opt);

    // Text that only appears in a vet's stamp.
    private static readonly Regex StampText = new(
        @"(?<![A-Za-z])(?:d\.?\s?v\.?\s?m\.?|prc)(?![A-Za-z])", Opt);

    public static StampCheck Check(byte[] jpeg, IReadOnlyList<OcrPiece> pieces)
    {
        // 1. The column heading (the lowest "Veterinarian's / Signature" text on the card).
        var headers = pieces.Where(p => HeaderWord.IsMatch(p.Text)).ToList();
        if (headers.Count == 0) return new StampCheck(false, new List<StampRow>());

        var lowest = headers.OrderByDescending(p => p.Top).First();
        double reach = Math.Max(lowest.Height, 8) * 2.5;
        var head = headers.Where(p => p.Top >= lowest.Top - reach).ToList();
        double headBottom = head.Max(p => p.Bottom);
        double headLeft = head.Min(p => p.Left), headRight = head.Max(p => p.Right);
        double pad = (headRight - headLeft) * 0.06;
        double colLeft = headLeft - pad, colRight = headRight + pad;

        // 2. The table rows: lines of the table that hold a date, grouped by height.
        var dateCells = pieces
            .Where(p => p.Top >= headBottom - 2 && VaccinationCardParser.FindDates(p.Text, out _).Count > 0)
            .OrderBy(p => p.CenterY)
            .ToList();

        var clusters = new List<List<OcrPiece>>();
        foreach (var cell in dateCells)
        {
            if (clusters.Count > 0 &&
                Math.Abs(cell.CenterY - clusters[^1].Average(x => x.CenterY)) < Math.Max(cell.Height, 10) * 0.8)
                clusters[^1].Add(cell);
            else
                clusters.Add(new List<OcrPiece> { cell });
        }
        if (clusters.Count == 0) return new StampCheck(true, new List<StampRow>());

        var centers = clusters.Select(c => c.Average(x => x.CenterY)).ToList();
        var givens = clusters.Select(c =>
        {
            var first = c.OrderBy(x => x.Left).First();
            var dates = VaccinationCardParser.FindDates(first.Text, out _);
            return dates.Count > 0 ? (DateTime?)dates[0] : null;
        }).ToList();

        // How tall one row is: the distance between rows, or (single row) about twice the gap from the heading to the row.
        double pitch;
        if (centers.Count > 1)
        {
            var gaps = centers.Zip(centers.Skip(1), (a, b) => b - a).OrderBy(g => g).ToList();
            pitch = gaps[gaps.Count / 2];
        }
        else
        {
            pitch = 2 * (centers[0] - headBottom) * 0.85;
        }
        pitch = Math.Max(pitch, 20);

        // 3. Look at the signature cell of every row.
        PixelGrid? pixels = null;
        try { pixels = PixelGrid.Load(jpeg); }
        catch (Exception ex) when (ex is ArgumentException or OutOfMemoryException or ExternalException) { }

        var rows = new List<StampRow>();
        for (int i = 0; i < centers.Count; i++)
        {
            double top = i == 0 ? centers[i] - pitch / 2 : (centers[i - 1] + centers[i]) / 2;
            double bottom = i == centers.Count - 1 ? centers[i] + pitch / 2 : (centers[i] + centers[i + 1]) / 2;
            top = Math.Max(top, headBottom + 4) + 3;
            bottom -= 3;

            // Text read inside the cell (stamp lettering)?
            bool text = pieces.Any(p => p.CenterX >= colLeft && p.CenterX <= colRight &&
                                        p.CenterY >= top && p.CenterY <= bottom && StampText.IsMatch(p.Text));
            if (text) { rows.Add(new StampRow(givens[i], true, "text")); continue; }

            bool ink = pixels is not null && HasInk(pixels, new Rectangle(
                (int)colLeft + 6, (int)top, (int)(colRight - colLeft) - 12, (int)(bottom - top)));
            rows.Add(new StampRow(givens[i], ink, ink ? "ink" : "none"));
        }
        return new StampCheck(true, rows);
    }

    /// <summary>
    /// Keeps only the vaccinations whose row carries a stamp. A row that can't be matched to a row on the card
    /// (so its stamp can't be checked) is dropped as well.
    /// </summary>
    public static List<ScannedVaccination> KeepStamped(IEnumerable<ScannedVaccination> vaccinations, StampCheck check)
    {
        var pool = check.Rows.ToList();
        var kept = new List<ScannedVaccination>();
        foreach (var row in vaccinations)
        {
            int index = pool.FindIndex(r => r.Given == row.Given);
            if (index < 0) continue;
            var match = pool[index];
            pool.RemoveAt(index);
            if (match.Stamped) kept.Add(row);
        }
        return kept;
    }

    /// <summary>A stamp is a lot of coloured ink spread over the cell; an empty cell has almost none.</summary>
    private static bool HasInk(PixelGrid px, Rectangle area)
    {
        area = px.Clip(area);
        if (area.Width < 30 || area.Height < 20) return false;

        const int G = 6;
        var cells = new int[G * G];
        long ink = 0;
        for (int y = area.Top; y < area.Bottom; y++)
        {
            int gy = (y - area.Top) * G / area.Height;
            for (int x = area.Left; x < area.Right; x++)
            {
                px.Get(x, y, out int r, out int g, out int b);
                int lum = (r + g + b) / 3;
                // Stamp ink photographs as a dark blue-grey (blue ahead of red), or dark red for red stamps. Black table lines,
                // grey shadows, yellowish paper and brown wood match neither.
                bool blueInk = b - r >= 10 && lum < 150;
                bool redInk = r - Math.Max(g, b) >= 45 && lum < 170;
                if (blueInk || redInk)
                {
                    ink++;
                    cells[gy * G + (x - area.Left) * G / area.Width]++;
                }
            }
        }

        double cellArea = (double)area.Width * area.Height / (G * G);
        int needed = (int)Math.Max(5, cellArea * 0.01);
        int occupied = cells.Count(c => c >= needed);
        return ink >= area.Width * (long)area.Height * 0.02 && occupied >= 8;
    }
}

/// <summary>Cuts the pet's picture out of the framed "PET PHOTO" box printed on the card.</summary>
internal static class PetPhotoFinder
{
    private static readonly Regex Label = new(@"pet\s*photo", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

    /// <summary>Returns the pet's picture as a small JPEG, or null when the "PET PHOTO" box can't be found.</summary>
    public static byte[]? Find(byte[] jpeg, IReadOnlyList<OcrPiece> pieces)
    {
        OcrPiece? label = null;
        Match? hit = null;
        foreach (var piece in pieces.OrderBy(p => p.Top))
        {
            var m = Label.Match(piece.Text);
            if (m.Success) { label = piece; hit = m; break; }
        }
        if (label is not { } l || hit is null) return null;

        // The label may share an OCR line with other text, so use only the part of the line that spells it.
        double perChar = l.Width / Math.Max(1, l.Text.Length);
        double labelLeft = l.Left + perChar * hit.Index;
        double labelWidth = perChar * hit.Length;
        double cx = labelLeft + labelWidth / 2;

        // The frame sits right under the label and is a little wider than it.
        var region = new Rectangle(
            (int)(cx - labelWidth * 1.05), (int)(l.Bottom + labelWidth * 0.05),
            (int)(labelWidth * 2.1), (int)(labelWidth * 1.8));

        PixelGrid px;
        try { px = PixelGrid.Load(jpeg); }
        catch (Exception ex) when (ex is ArgumentException or OutOfMemoryException or ExternalException) { return null; }

        region = px.Clip(region);
        if (region.Width < 40 || region.Height < 40) return null;

        var photo = Trim(px, region);
        try { return CardImage.CropJpeg(jpeg, photo, 480); }
        catch (Exception ex) when (ex is ArgumentException or OutOfMemoryException or ExternalException) { return null; }
    }

    /// <summary>Finds the picture inside the frame: the biggest block of rows and columns that is not blank paper.</summary>
    private static Rectangle Trim(PixelGrid px, Rectangle region)
    {
        int paper = PaperLevel(px, region);

        var rowFill = new double[region.Height];
        for (int y = 0; y < region.Height; y++)
        {
            int content = 0;
            for (int x = 0; x < region.Width; x++)
                if (IsContent(px, region.Left + x, region.Top + y, paper)) content++;
            rowFill[y] = content / (double)region.Width;
        }
        var (top, bottom) = LongestRun(rowFill, 0.35);

        var fallback = Rectangle.Inflate(region, -(int)(region.Width * 0.1), -(int)(region.Height * 0.1));
        if (bottom - top < region.Height * 0.35) return fallback;

        var colFill = new double[region.Width];
        for (int x = 0; x < region.Width; x++)
        {
            int content = 0;
            for (int y = top; y <= bottom; y++)
                if (IsContent(px, region.Left + x, region.Top + y, paper)) content++;
            colFill[x] = content / (double)(bottom - top + 1);
        }
        var (left, right) = LongestRun(colFill, 0.5);
        if (right - left < region.Width * 0.35) return fallback;

        // Step in a few pixels so the frame line is never part of the picture.
        const int Inset = 3;
        var result = Rectangle.FromLTRB(region.Left + left + Inset, region.Top + top + Inset,
                                        region.Left + right - Inset, region.Top + bottom - Inset);
        double aspect = result.Width / (double)Math.Max(1, result.Height);
        return result.Width < 20 || result.Height < 20 || aspect < 0.5 || aspect > 2.0 ? fallback : result;
    }

    /// <summary>
    /// How bright the blank paper around the picture is in this photo: the most common brightness among the light pixels
    /// (the padding inside the frame is a big flat area, a photograph is spread over many brightness values).
    /// </summary>
    private static int PaperLevel(PixelGrid px, Rectangle region)
    {
        var histogram = new int[64];
        for (int y = region.Top; y < region.Bottom; y++)
            for (int x = region.Left; x < region.Right; x++)
            {
                px.Get(x, y, out int r, out int g, out int b);
                int lum = (r + g + b) / 3;
                if (lum > 150) histogram[lum / 4]++;
            }

        int best = 0, bestCount = -1;
        for (int i = 0; i < histogram.Length; i++)
        {
            int count = histogram[i] + (i > 0 ? histogram[i - 1] : 0) + (i < histogram.Length - 1 ? histogram[i + 1] : 0);
            if (count > bestCount) { bestCount = count; best = i; }
        }
        return best * 4 + 2;
    }

    /// <summary>A pixel belongs to the picture when it is clearly different from the blank paper (darker, brighter or coloured).</summary>
    private static bool IsContent(PixelGrid px, int x, int y, int paper)
    {
        px.Get(x, y, out int r, out int g, out int b);
        int max = Math.Max(r, Math.Max(g, b)), min = Math.Min(r, Math.Min(g, b));
        return Math.Abs((r + g + b) / 3 - paper) > 25 || max - min > 45;
    }

    /// <summary>The longest stretch of values at or above the threshold (a gap of one value is bridged).</summary>
    private static (int Start, int End) LongestRun(double[] values, double threshold)
    {
        int bestStart = 0, bestEnd = -1, start = -1, gap = 0;
        for (int i = 0; i <= values.Length; i++)
        {
            bool on = i < values.Length && values[i] >= threshold;
            if (on)
            {
                if (start < 0) start = i;
                gap = 0;
                continue;
            }
            if (start < 0) continue;
            if (i < values.Length && ++gap <= 1) continue;

            int end = i < values.Length ? i - gap : i - 1 - gap;   // last value that was on
            if (end - start > bestEnd - bestStart) { bestStart = start; bestEnd = end; }
            start = -1; gap = 0;
        }
        return (bestStart, bestEnd);
    }
}
