using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using MyApp.Controls;

namespace MyApp;

/// <summary>Turns picked picture files into small square PNGs for the database, and back into images for the screen.</summary>
internal static class AvatarImages
{
    private const int Size = 256;

    /// <summary>Centre-crops to a square, shrinks to 256x256 and returns PNG bytes.</summary>
    public static byte[] Prepare(Image source)
    {
        using var square = new Bitmap(Size, Size);
        using (var g = Graphics.FromImage(square))
        {
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.Clear(Color.White);

            float scale = Math.Max((float)Size / source.Width, (float)Size / source.Height);
            float w = source.Width * scale, h = source.Height * scale;
            g.DrawImage(source, (Size - w) / 2f, (Size - h) / 2f, w, h);
        }

        using var ms = new MemoryStream();
        square.Save(ms, ImageFormat.Png);
        return ms.ToArray();
    }

    /// <summary>Reads a picture file. Returns null (and a message) if it isn't a usable image.</summary>
    public static byte[]? PrepareFromFile(string path, out string? error)
    {
        error = null;
        try
        {
            if (new FileInfo(path).Length > 15 * 1024 * 1024)
            {
                error = "That picture is too large (max 15 MB).";
                return null;
            }
            using var original = Image.FromFile(path);
            return Prepare(original);
        }
        catch (Exception ex) when (ex is OutOfMemoryException or ArgumentException or IOException or UnauthorizedAccessException)
        {
            error = "That file couldn't be opened as a picture.";
            return null;
        }
    }

    /// <summary>Builds an image that doesn't keep a stream open. Returns null for empty or unreadable data.</summary>
    public static Image? FromBytes(byte[]? bytes)
    {
        if (bytes is null || bytes.Length == 0) return null;
        try
        {
            using var ms = new MemoryStream(bytes);
            using var img = Image.FromStream(ms);
            return new Bitmap(img);
        }
        catch (Exception ex) when (ex is ArgumentException or OutOfMemoryException)
        {
            return null;
        }
    }

    /// <summary>Shows the picture in a round badge, or the plain person icon when there is none.</summary>
    public static void Show(IconBadge badge, byte[]? bytes) => badge.Photo = FromBytes(bytes);
}
