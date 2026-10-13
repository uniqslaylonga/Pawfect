using Windows.Devices.Enumeration;
using Windows.Graphics.Imaging;
using Windows.Media.Capture;
using Windows.Media.Capture.Frames;
using Windows.Media.Devices;
using Windows.Media.MediaProperties;
using Windows.Storage.Streams;
using MyApp.Controls;

namespace MyApp;

/// <summary>
/// Shows a live picture from a webcam and hands back one still frame (as a JPEG) when the user presses Capture.
/// Uses the camera API built into Windows 10/11, so like <see cref="WindowsOcr"/> it needs no extra package and
/// nothing leaves the PC. The caller sends the result to the same OCR path as a photo chosen from disk.
/// </summary>
internal sealed class CameraCaptureForm : DialogBase
{
    private readonly PawSelect _cameras = new() { Placeholder = "Looking for cameras\u2026" };
    private readonly PictureBox _preview = new()
    {
        SizeMode = PictureBoxSizeMode.Zoom,
        BackColor = Color.Black,
        Height = 367,
    };

    private readonly List<DeviceInformation> _devices = new();
    private MediaCapture? _capture;
    private MediaFrameReader? _reader;
    private byte[]? _latest;
    private readonly Queue<(byte[] Jpeg, double Sharpness)> _recent = new();   // the last few frames, newest last
    private const int RecentFrames = 8;
    private int _encoding;      // 1 while a frame is being turned into a preview picture (extra frames are skipped)
    private int _generation;    // bumped whenever the camera is switched or the window closes
    private volatile bool _closing;

    /// <summary>The captured still, as a JPEG. Null unless the user pressed Capture.</summary>
    public byte[]? Jpeg { get; private set; }

    public CameraCaptureForm()
        : base("Take Photo", "Capture", "\uE722", "Hold the card flat and steady, then press Capture")
    {
        ClientSize = new Size(700, ClientSize.Height);

        AddRow("Camera", _cameras);
        Grid.Controls.Add(_preview, 0, 1);
        Grid.SetColumnSpan(_preview, 2);
        _preview.Dock = DockStyle.Fill;
        _preview.Margin = new Padding(0, 8, 0, 8);
        FitHeight();

        SaveButton.Enabled = false;   // nothing to capture until the first frame arrives
        _cameras.SelectedIndexChanged += async (_, _) => await StartAsync(_cameras.SelectedIndex);
    }

    // ------------------------------------------------------------------ opening / closing

    protected override async void OnShown(EventArgs e)
    {
        base.OnShown(e);
        try
        {
            var found = await DeviceInformation.FindAllAsync(DeviceClass.VideoCapture);
            foreach (var device in found)
            {
                _devices.Add(device);
                _cameras.Items.Add(device.Name);
            }
        }
        catch (Exception ex)
        {
            ShowError("Couldn't look for cameras. " + ex.Message);
            return;
        }

        if (_devices.Count == 0)
        {
            _cameras.Placeholder = "No camera found";
            ShowError("No camera was found. Plug one in, or cancel and choose a photo instead.");
            return;
        }
        _cameras.SelectedIndex = 0;   // raises SelectedIndexChanged, which starts the preview
    }

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        _closing = true;
        _generation++;
        base.OnFormClosing(e);
    }

    protected override void OnFormClosed(FormClosedEventArgs e)
    {
        base.OnFormClosed(e);
        _ = StopAsync();
        _preview.Image?.Dispose();
        _preview.Image = null;
    }

    // ------------------------------------------------------------------ camera

    private async Task StartAsync(int index)
    {
        if (index < 0 || index >= _devices.Count || _closing) return;
        int generation = ++_generation;

        ShowError("");
        SaveButton.Enabled = false;
        await StopAsync();
        _latest = null;
        lock (_recent) _recent.Clear();

        MediaCapture? capture = null;
        MediaFrameReader? reader = null;
        try
        {
            capture = new MediaCapture();
            await capture.InitializeAsync(new MediaCaptureInitializationSettings
            {
                VideoDeviceId = _devices[index].Id,
                StreamingCaptureMode = StreamingCaptureMode.Video,
                SharingMode = MediaCaptureSharingMode.ExclusiveControl,
                MemoryPreference = MediaCaptureMemoryPreference.Cpu,
            });

            var source = capture.FrameSources.Values.FirstOrDefault(s =>
                             s.Info.SourceKind == MediaFrameSourceKind.Color &&
                             s.Info.MediaStreamType is MediaStreamType.VideoPreview or MediaStreamType.VideoRecord)
                         ?? capture.FrameSources.Values.FirstOrDefault(s => s.Info.SourceKind == MediaFrameSourceKind.Color)
                         ?? throw new InvalidOperationException("This camera doesn't give a picture Pawfect can use.");

            // Sharp enough to read small print, but not so big that the preview crawls: largest size up to 1080p at 15+ fps.
            static double Fps(MediaFrameFormat f) => f.FrameRate.Numerator / (double)Math.Max(1u, f.FrameRate.Denominator);
            var usable = source.SupportedFormats
                .Where(f => f.VideoFormat.Width is >= 640 and <= 1920)
                .ToList();
            var smooth = usable.Where(f => Fps(f) >= 15).ToList();
            var best = (smooth.Count > 0 ? smooth : usable)
                .OrderByDescending(f => f.VideoFormat.Width * f.VideoFormat.Height)
                .ThenByDescending(f => Fps(f))
                .FirstOrDefault();
            if (best is not null) await source.SetFormatAsync(best);

            TryEnableAutofocus(capture);

            reader = await capture.CreateFrameReaderAsync(source, MediaEncodingSubtypes.Bgra8);
            reader.AcquisitionMode = MediaFrameReaderAcquisitionMode.Realtime;
            reader.FrameArrived += OnFrameArrived;
            var status = await reader.StartAsync();
            if (status != MediaFrameReaderStartStatus.Success)
                throw new InvalidOperationException($"The camera wouldn't start ({status}). Is another program using it?");

            // The user may have picked another camera, or closed the window, while we were waiting.
            if (generation != _generation || _closing)
            {
                reader.FrameArrived -= OnFrameArrived;
                await reader.StopAsync();
                reader.Dispose();
                capture.Dispose();
                return;
            }

            _capture = capture;
            _reader = reader;
        }
        catch (UnauthorizedAccessException)
        {
            Release(reader, capture);
            ShowError("Camera access is blocked. Allow it in Settings > Privacy & security > Camera.");
        }
        catch (Exception ex)
        {
            // WinRT reports many different failures (camera busy, driver problem, unplugged); all end up here.
            Release(reader, capture);
            if (generation == _generation) ShowError("Couldn't start the camera. " + ex.Message);
        }
    }

    /// <summary>Text is much easier to read when the camera keeps refocusing. Not every webcam supports it, so failure is ignored.</summary>
    private static void TryEnableAutofocus(MediaCapture capture)
    {
        try
        {
            var focus = capture.VideoDeviceController.FocusControl;
            if (!focus.Supported) return;
            var mode = focus.SupportedFocusModes.Contains(FocusMode.Continuous) ? FocusMode.Continuous : FocusMode.Auto;
            focus.Configure(new FocusSettings { Mode = mode });
        }
        catch (Exception)
        {
            // Autofocus is a nicety; the preview still works without it.
        }
    }

    private async Task StopAsync()
    {
        var reader = _reader;
        var capture = _capture;
        _reader = null;
        _capture = null;
        if (reader is not null)
        {
            reader.FrameArrived -= OnFrameArrived;
            try { await reader.StopAsync(); }
            catch (Exception) { /* already stopped / camera unplugged */ }
            reader.Dispose();
        }
        capture?.Dispose();
    }

    private void Release(MediaFrameReader? reader, MediaCapture? capture)
    {
        if (reader is not null)
        {
            reader.FrameArrived -= OnFrameArrived;
            reader.Dispose();
        }
        capture?.Dispose();
    }

    // ------------------------------------------------------------------ frames

    /// <summary>Runs on a background thread for every frame; keeps the newest one as a JPEG and shows it in the preview.</summary>
    private void OnFrameArrived(MediaFrameReader sender, MediaFrameArrivedEventArgs args)
    {
        if (_closing || Interlocked.Exchange(ref _encoding, 1) == 1) return;
        try
        {
            using var frame = sender.TryAcquireLatestFrame();
            var software = frame?.VideoMediaFrame?.SoftwareBitmap;
            if (software is null) return;

            SoftwareBitmap? converted = null;
            var bitmap = software;
            if (bitmap.BitmapPixelFormat != BitmapPixelFormat.Bgra8 || bitmap.BitmapAlphaMode != BitmapAlphaMode.Premultiplied)
                bitmap = converted = SoftwareBitmap.Convert(software, BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied);

            byte[] jpeg;
            try { jpeg = EncodeJpegAsync(bitmap).GetAwaiter().GetResult(); }
            finally { converted?.Dispose(); }

            Bitmap shown;
            using (var memory = new MemoryStream(jpeg))
            using (var decoded = new Bitmap(memory))
                shown = new Bitmap(decoded);   // copy, so the stream can be released

            if (_closing || IsDisposed) { shown.Dispose(); return; }
            _latest = jpeg;
            double sharpness = Sharpness(shown);
            lock (_recent)
            {
                _recent.Enqueue((jpeg, sharpness));
                while (_recent.Count > RecentFrames) _recent.Dequeue();
            }
            try
            {
                BeginInvoke(new Action(() =>
                {
                    var old = _preview.Image;
                    _preview.Image = shown;
                    old?.Dispose();
                    SaveButton.Enabled = true;
                }));
            }
            catch (Exception ex) when (ex is InvalidOperationException or ObjectDisposedException)
            {
                shown.Dispose();   // the window closed between the check and the call
            }
        }
        catch (Exception)
        {
            // One bad frame (driver hiccup) is not worth an error; the next frame will replace it.
        }
        finally
        {
            Interlocked.Exchange(ref _encoding, 0);
        }
    }

    /// <summary>How much fine detail a frame has (average brightness change between neighbouring pixels). Blurry frames score low.</summary>
    private static double Sharpness(Bitmap frame)
    {
        int w = frame.Width, h = frame.Height;
        var data = frame.LockBits(new Rectangle(0, 0, w, h), System.Drawing.Imaging.ImageLockMode.ReadOnly,
            System.Drawing.Imaging.PixelFormat.Format32bppArgb);
        try
        {
            int stride = data.Stride;
            var buffer = new byte[Math.Abs(stride) * h];
            System.Runtime.InteropServices.Marshal.Copy(data.Scan0, buffer, 0, buffer.Length);

            double sum = 0;
            long count = 0;
            for (int y = 0; y < h - 1; y += 2)
            {
                for (int x = 0; x < w - 1; x += 2)
                {
                    int i = y * stride + x * 4 + 1;          // green channel is close enough to brightness
                    sum += Math.Abs(buffer[i] - buffer[i + 4]) + Math.Abs(buffer[i] - buffer[i + stride]);
                    count++;
                }
            }
            return count == 0 ? 0 : sum / count;
        }
        finally
        {
            frame.UnlockBits(data);
        }
    }

    private static async Task<byte[]> EncodeJpegAsync(SoftwareBitmap bitmap)
    {
        using var stream = new InMemoryRandomAccessStream();
        var encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.JpegEncoderId, stream);
        encoder.SetSoftwareBitmap(bitmap);
        await encoder.FlushAsync();

        uint size = (uint)stream.Size;
        var bytes = new byte[size];
        using var reader = new DataReader(stream.GetInputStreamAt(0));
        await reader.LoadAsync(size);
        reader.ReadBytes(bytes);
        return bytes;
    }

    // ------------------------------------------------------------------ capture

    /// <summary>The Capture button: keep the frame that is on screen right now and close.</summary>
    protected override void OnSaveClicked()
    {
        // Pressing a button shakes the card a little, so take the sharpest of the last moments instead of this exact one.
        byte[]? shot;
        lock (_recent) shot = _recent.Count > 0 ? _recent.MaxBy(f => f.Sharpness).Jpeg : _latest;
        if (shot is null) { ShowError("Waiting for the camera\u2026"); return; }
        Jpeg = shot;
        DialogResult = DialogResult.OK;
    }
}
