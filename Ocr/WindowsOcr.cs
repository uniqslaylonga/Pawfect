using Windows.Globalization;
using Windows.Graphics.Imaging;
using Windows.Media.Ocr;
using Windows.Storage.Streams;

namespace MyApp;

/// <summary>
/// Reads text from a picture with the OCR engine that is built into Windows 10/11 (no extra package, no internet,
/// the photo never leaves the PC). Needs the net10.0-windows10.0.19041.0 target in the .csproj.
/// </summary>
internal static class WindowsOcr
{
    /// <summary>Returns the text of the picture as lines, top to bottom. Text that sits side by side (table cells) is joined into one line.</summary>
    public static async Task<List<string>> ReadLinesAsync(byte[] image) => OcrRows.Merge(await ReadPiecesAsync(image));

    /// <summary>Same as <see cref="ReadLinesAsync"/> but keeps where each line of text sits in the picture (used to find the stamps and the pet photo).</summary>
    public static async Task<List<OcrPiece>> ReadPiecesAsync(byte[] image)
    {
        var engine = OcrEngine.TryCreateFromUserProfileLanguages()
                     ?? OcrEngine.TryCreateFromLanguage(new Language("en-US"));
        if (engine is null)
            throw new InvalidOperationException(
                "Windows has no text-recognition language installed. Add English in Settings > Time & language > Language & region, then try again.");

        using var stream = new InMemoryRandomAccessStream();
        using (var writer = new DataWriter(stream.GetOutputStreamAt(0)))
        {
            writer.WriteBytes(image);
            await writer.StoreAsync();
            await writer.FlushAsync();
            writer.DetachStream();
        }
        stream.Seek(0);

        var decoder = await BitmapDecoder.CreateAsync(stream);
        if (decoder.PixelWidth > OcrEngine.MaxImageDimension || decoder.PixelHeight > OcrEngine.MaxImageDimension)
            throw new InvalidOperationException("That picture is too large to read. Try a smaller photo.");

        using var bitmap = await decoder.GetSoftwareBitmapAsync(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied);
        var result = await engine.RecognizeAsync(bitmap);

        // OCR returns one entry per text line; cells of a table row come back as separate entries.
        var items = new List<OcrPiece>();
        foreach (var line in result.Lines)
        {
            if (line.Words.Count == 0) continue;
            items.Add(new OcrPiece(line.Words.Min(w => w.BoundingRect.Left),
                                   line.Words.Min(w => w.BoundingRect.Top),
                                   line.Words.Max(w => w.BoundingRect.Right),
                                   line.Words.Max(w => w.BoundingRect.Bottom),
                                   line.Text));
        }
        return items;
    }
}
