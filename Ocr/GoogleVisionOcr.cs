using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace MyApp;

/// <summary>
/// Reads text from a picture with Google Cloud Vision (DOCUMENT_TEXT_DETECTION), which is much better than the
/// Windows engine on camera photos, coloured or small print, and neat handwriting. The picture IS sent to Google.
/// Needs internet and an API key: set the PAWFECT_VISION_KEY environment variable, or put the key alone in a file
/// called vision.key next to Pawfect.exe. Without a key the app silently keeps using <see cref="WindowsOcr"/>.
/// </summary>
internal static class GoogleVisionOcr
{
    private const string Endpoint = "https://vision.googleapis.com/v1/images:annotate";

    private static readonly HttpClient Http = new() { Timeout = TimeSpan.FromSeconds(30) };
    private static string? _key;
    private static bool _keyLoaded;

    public static bool IsConfigured => !string.IsNullOrEmpty(Key);

    /// <summary>Where the app looks first, shown to the user when no key is found.</summary>
    public static string KeyFolder => AppContext.BaseDirectory;

    private static string? Key
    {
        get
        {
            if (!_keyLoaded) { _key = FindKey(); _keyLoaded = _key is not null; }
            return _key;
        }
    }

    /// <summary>
    /// The environment variable (read from Windows directly, so no restart is needed), then a vision.key file next to the
    /// exe or in any folder above it (so it is also found when running from Visual Studio). vision.key.txt is accepted
    /// because Windows hides file extensions and Notepad often adds .txt.
    /// </summary>
    private static string? FindKey()
    {
        foreach (var scope in new[] { EnvironmentVariableTarget.Process, EnvironmentVariableTarget.User, EnvironmentVariableTarget.Machine })
        {
            var value = Clean(Environment.GetEnvironmentVariable("PAWFECT_VISION_KEY", scope));
            if (value is not null) return value;
        }

        try
        {
            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            for (int depth = 0; dir is not null && depth < 6; depth++, dir = dir.Parent)
            {
                foreach (var name in new[] { "vision.key", "vision.key.txt" })
                {
                    var file = Path.Combine(dir.FullName, name);
                    if (File.Exists(file) && Clean(File.ReadAllText(file)) is { } value) return value;
                }
            }
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            // Unreadable folder; treat as "no key".
        }
        return null;
    }

    /// <summary>First non-empty line, without spaces or quotes.</summary>
    private static string? Clean(string? text)
    {
        var line = text?.Split('\n', '\r').Select(l => l.Trim().Trim('"', '\'')).FirstOrDefault(l => l.Length > 0);
        return string.IsNullOrEmpty(line) ? null : line;
    }

    /// <summary>Returns the text of the picture as lines, top to bottom (side-by-side table cells joined into one line).</summary>
    /// <exception cref="HttpRequestException">No internet, bad key, quota used up, or Google refused the request.</exception>
    public static async Task<List<string>> ReadLinesAsync(byte[] image) => OcrRows.Merge(await ReadPiecesAsync(image));

    /// <summary>Same as <see cref="ReadLinesAsync"/> but keeps where each line of text sits in the picture (used to find the stamps and the pet photo).</summary>
    /// <exception cref="HttpRequestException">No internet, bad key, quota used up, or Google refused the request.</exception>
    public static async Task<List<OcrPiece>> ReadPiecesAsync(byte[] image)
    {
        if (Key is not { } key) throw new InvalidOperationException("No Google Vision key is set.");

        var body = JsonSerializer.Serialize(new
        {
            requests = new[]
            {
                new
                {
                    image = new { content = Convert.ToBase64String(image) },
                    features = new[] { new { type = "DOCUMENT_TEXT_DETECTION" } },
                    imageContext = new { languageHints = new[] { "en" } },
                },
            },
        });

        using var request = new HttpRequestMessage(HttpMethod.Post, Endpoint)
        {
            Content = new StringContent(body, Encoding.UTF8, new MediaTypeHeaderValue("application/json")),
        };
        request.Headers.Add("X-Goog-Api-Key", key);   // in a header, so it never ends up in a URL or a log

        using var response = await Http.SendAsync(request);
        string json = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
            throw new HttpRequestException($"Google Vision refused the request ({(int)response.StatusCode}). {ErrorText(json)}");

        using var doc = JsonDocument.Parse(json);
        var first = doc.RootElement.GetProperty("responses")[0];
        if (first.TryGetProperty("error", out var error))
            throw new HttpRequestException("Google Vision couldn't read the picture. " + Text(error, "message"));
        if (!first.TryGetProperty("fullTextAnnotation", out var full)) return new List<OcrPiece>();   // no text found

        // Words come in reading order. A line ends where Google reports a line break after a symbol.
        var items = new List<OcrPiece>();
        foreach (var page in Items(full, "pages"))
        foreach (var block in Items(page, "blocks"))
        foreach (var paragraph in Items(block, "paragraphs"))
        {
            var line = new StringBuilder();
            double top = double.MaxValue, bottom = double.MinValue, left = double.MaxValue, right = double.MinValue;

            void Flush()
            {
                string text = line.ToString().Trim();
                if (text.Length > 0) items.Add(new OcrPiece(left, top, right, bottom, text));
                line.Clear();
                top = double.MaxValue; bottom = double.MinValue; left = double.MaxValue; right = double.MinValue;
            }

            foreach (var word in Items(paragraph, "words"))
            {
                if (word.TryGetProperty("boundingBox", out var box) && box.TryGetProperty("vertices", out var vertices))
                {
                    foreach (var v in vertices.EnumerateArray())
                    {
                        double x = v.TryGetProperty("x", out var vx) ? vx.GetDouble() : 0;   // zero is omitted by the API
                        double y = v.TryGetProperty("y", out var vy) ? vy.GetDouble() : 0;
                        left = Math.Min(left, x);
                        right = Math.Max(right, x);
                        top = Math.Min(top, y);
                        bottom = Math.Max(bottom, y);
                    }
                }

                foreach (var symbol in Items(word, "symbols"))
                {
                    line.Append(Text(symbol, "text"));
                    if (!symbol.TryGetProperty("property", out var prop) ||
                        !prop.TryGetProperty("detectedBreak", out var brk)) continue;

                    switch (Text(brk, "type"))
                    {
                        case "SPACE" or "SURE_SPACE": line.Append(' '); break;
                        case "EOL_SURE_SPACE" or "LINE_BREAK": Flush(); break;
                        case "HYPHEN": line.Append('-'); Flush(); break;
                    }
                }
            }
            Flush();
        }

        return items;
    }

    private static IEnumerable<JsonElement> Items(JsonElement parent, string name) =>
        parent.TryGetProperty(name, out var array) && array.ValueKind == JsonValueKind.Array
            ? array.EnumerateArray()
            : Enumerable.Empty<JsonElement>();

    private static string Text(JsonElement parent, string name) =>
        parent.TryGetProperty(name, out var value) ? value.GetString() ?? "" : "";

    private static string ErrorText(string json)
    {
        try
        {
            using var doc = JsonDocument.Parse(json);
            return doc.RootElement.TryGetProperty("error", out var error) ? Text(error, "message") : "";
        }
        catch (JsonException)
        {
            return "";
        }
    }
}

/// <summary>Puts text pieces that sit side by side (the cells of one table row) back into a single line.</summary>
internal static class OcrRows
{
    public static List<string> Merge(IEnumerable<OcrPiece> pieces)
    {
        var items = pieces.OrderBy(p => p.Top + p.Bottom).ToList();

        var rows = new List<List<OcrPiece>>();
        foreach (var item in items)
        {
            if (rows.Count > 0)
            {
                var last = rows[^1];
                double rowTop = last.Min(x => x.Top), rowBottom = last.Max(x => x.Bottom);
                double overlap = Math.Min(rowBottom, item.Bottom) - Math.Max(rowTop, item.Top);
                double smaller = Math.Min(rowBottom - rowTop, item.Bottom - item.Top);
                if (smaller > 0 && overlap >= 0.5 * smaller) { last.Add(item); continue; }
            }
            rows.Add(new() { item });
        }

        return rows.Select(r => string.Join(" ", r.OrderBy(x => x.Left).Select(x => x.Text))).ToList();
    }
}
