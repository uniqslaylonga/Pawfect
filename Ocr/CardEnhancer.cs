using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace MyApp;

/// <summary>
/// Makes a card photo easier for Windows OCR: colored (orange, blue, red...) and pale text is turned into dark text,
/// the contrast is stretched, and small pictures are enlarged. The OCR engine is run on both the original photo and
/// this cleaned-up copy, and <see cref="CardMerge"/> keeps whichever read more.
/// </summary>
internal static class CardEnhancer
{
    private const int MinLongSide = 2400;   // Windows OCR misses letters when the text is only a few pixels tall
    private const double MaxScale = 4.0;

    /// <summary>Returns a PNG: grey-scale, high contrast, dark text on a white background.</summary>
    public static byte[] Enhance(byte[] picture)
    {
        using var source = new MemoryStream(picture);
        using var original = new Bitmap(source);

        int longSide = Math.Max(original.Width, original.Height);
        double scale = longSide >= 1600 ? 1.0 : Math.Min(MaxScale, MinLongSide / (double)longSide);
        int w = Math.Max(1, (int)Math.Round(original.Width * scale));
        int h = Math.Max(1, (int)Math.Round(original.Height * scale));

        using var bitmap = new Bitmap(w, h, PixelFormat.Format32bppArgb);
        using (var g = Graphics.FromImage(bitmap))
        {
            g.Clear(Color.White);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.DrawImage(original, 0, 0, w, h);
        }

        var data = bitmap.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
        try
        {
            int stride = data.Stride;
            var buffer = new byte[Math.Abs(stride) * h];
            Marshal.Copy(data.Scan0, buffer, 0, buffer.Length);

            // Darkest colour channel: black stays black, white stays white, and any saturated colour (orange has almost
            // no blue, red has almost no green...) becomes dark. This is what makes coloured text readable.
            var histogram = new int[256];
            for (int y = 0; y < h; y++)
            {
                int row = y * stride;
                for (int x = 0; x < w; x++)
                {
                    int i = row + x * 4;                       // B, G, R, A
                    byte min = Math.Min(buffer[i], Math.Min(buffer[i + 1], buffer[i + 2]));
                    buffer[i] = min;
                    histogram[min]++;
                }
            }

            // Stretch: the darkest 0.5% of pixels become black, the paper level (90th percentile) becomes white.
            long total = (long)w * h;
            int lo = Percentile(histogram, total, 0.005);
            int hi = Percentile(histogram, total, 0.90);
            if (hi - lo < 40) { lo = 0; hi = 255; }            // already flat; don't blow up noise
            double gain = 255.0 / (hi - lo);

            for (int y = 0; y < h; y++)
            {
                int row = y * stride;
                for (int x = 0; x < w; x++)
                {
                    int i = row + x * 4;
                    int v = (int)Math.Round((buffer[i] - lo) * gain);
                    byte b = (byte)Math.Clamp(v, 0, 255);
                    buffer[i] = buffer[i + 1] = buffer[i + 2] = b;
                    buffer[i + 3] = 255;
                }
            }
            Marshal.Copy(buffer, 0, data.Scan0, buffer.Length);
        }
        finally
        {
            bitmap.UnlockBits(data);
        }

        using var output = new MemoryStream();
        bitmap.Save(output, ImageFormat.Png);
        return output.ToArray();
    }

    private static int Percentile(int[] histogram, long total, double fraction)
    {
        long target = (long)(total * fraction), seen = 0;
        for (int v = 0; v < histogram.Length; v++)
        {
            seen += histogram[v];
            if (seen > target) return v;
        }
        return 255;
    }
}

/// <summary>Combines two reads of the same card: the fuller one wins, and gaps are filled from the other.</summary>
internal static class CardMerge
{
    public static ScannedCard Best(ScannedCard first, ScannedCard second)
    {
        var (a, b) = Score(first) >= Score(second) ? (first, second) : (second, first);
        return a with
        {
            PetName = a.PetName.Length > 0 ? a.PetName : b.PetName,
            Species = a.Species.Length > 0 ? a.Species : b.Species,
            Breed = a.Breed.Length > 0 ? a.Breed : b.Breed,
            Sex = a.Sex.Length > 0 ? a.Sex : b.Sex,
            BirthDate = a.BirthDate ?? b.BirthDate,
            WeightKg = a.WeightKg ?? b.WeightKg,
            Owner = a.Owner.Length > 0 ? a.Owner : b.Owner,
            Address = a.Address.Length > 0 ? a.Address : b.Address,
        };
    }

    private static int Score(ScannedCard c) =>
        (c.PetName.Length > 0 ? 1 : 0) + (c.Species.Length > 0 ? 1 : 0) + (c.Breed.Length > 0 ? 1 : 0) +
        (c.Sex.Length > 0 ? 1 : 0) + (c.BirthDate is null ? 0 : 1) + (c.WeightKg is null ? 0 : 1) +
        (c.Owner.Length > 0 ? 1 : 0) + (c.Address.Length > 0 ? 1 : 0) + c.Vaccinations.Count * 3;
}
