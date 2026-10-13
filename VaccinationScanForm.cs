using System.Drawing.Imaging;
using System.Globalization;
using Microsoft.Data.SqlClient;
using MyApp.Controls;

namespace MyApp;

/// <summary>
/// The customer picks a photo of a pet's vaccination card; the app reads it (OCR) and fills in the pet's details and the
/// vaccination table. Nothing is saved until the customer has looked it over and pressed Save.
/// </summary>
internal sealed class VaccinationScanForm : DialogBase
{
    private readonly int _customerId;
    private readonly string _customerName;
    private readonly List<PetRecord> _pets;

    private readonly PawButton _choose = new() { Text = "Choose photo\u2026", Kind = PawButtonKind.Primary, Size = new Size(140, 34) };
    private readonly PawButton _camera = new() { Text = "Use camera", Kind = PawButtonKind.Outline, Size = new Size(120, 34) };
    private readonly Label _status = new() { AutoSize = true, ForeColor = Theme.Muted, Margin = new Padding(12, 9, 0, 0) };
    private readonly Label _engineLabel = new() { AutoSize = false, Height = 44, TextAlign = ContentAlignment.MiddleLeft, Text = "\u2014" };
    private readonly TextBox _name = new();
    private readonly TextBox _species = new();
    private readonly TextBox _breed = new();
    private readonly PawSelect _sex = new() { Placeholder = "Not stated" };
    private readonly TextBox _birth = new() { PlaceholderText = "e.g. Dec 5, 2024" };
    private readonly TextBox _weight = new() { PlaceholderText = "kg, e.g. 3.8" };
    private readonly Label _owner = new() { AutoSize = false, Height = 28, TextAlign = ContentAlignment.MiddleLeft };
    private readonly ListView _list = new()
    {
        Dock = DockStyle.Fill, View = View.Details, FullRowSelect = true, MultiSelect = false,
        HideSelection = false, BorderStyle = BorderStyle.FixedSingle,
    };

    private readonly PictureBox _photoBox = new()
    {
        Size = new Size(96, 96), SizeMode = PictureBoxSizeMode.Zoom, BorderStyle = BorderStyle.FixedSingle,
        BackColor = Theme.Subtle,
    };
    private readonly Label _photoNote = new() { AutoSize = true, ForeColor = Theme.Muted, Margin = new Padding(12, 38, 0, 0) };

    private readonly List<ScannedVaccination> _rows = new();
    private byte[]? _cardJpeg;
    private byte[]? _petPhotoJpeg;

    public int? SavedPetId { get; private set; }

    public VaccinationScanForm(int customerId, string customerName)
        : base("Scan Vaccination Card", "Save", "\uE722", "Use the camera or a photo and we'll read it for you")
    {
        _customerId = customerId;
        _customerName = customerName;
        _pets = EmployeeData.GetPets().Where(p => p.CustomerId == customerId).ToList();

        ClientSize = new Size(700, ClientSize.Height);

        var photoRow = new FlowLayoutPanel { Height = 44, WrapContents = false, FlowDirection = FlowDirection.LeftToRight };
        photoRow.Controls.Add(_camera);
        photoRow.Controls.Add(_choose);
        photoRow.Controls.Add(_status);
        _status.Text = "Details are read from the card automatically.";

        _sex.Items.AddRange(new object[] { "Male", "Female" });

        _list.Columns.Add("Date given", 110);
        _list.Columns.Add("Vaccine", 170);
        _list.Columns.Add("Weight", 70);
        _list.Columns.Add("Batch / Lot", 110);
        _list.Columns.Add("Expires", 110);

        var table = new Panel { Height = 210 };
        table.Controls.Add(_list);

        AddRow("Card photo", photoRow);
        AddRow("Read with", _engineLabel);

        var photoPreview = new FlowLayoutPanel { Height = 104, WrapContents = false, FlowDirection = FlowDirection.LeftToRight };
        photoPreview.Controls.Add(_photoBox);
        photoPreview.Controls.Add(_photoNote);
        _photoNote.Text = "The pet's picture is taken from the card.";
        AddRow("Pet photo", photoPreview);

        AddRow("Pet name *", _name);
        AddRow("Species", _species);
        AddRow("Breed", _breed);
        AddRow("Sex", _sex);
        AddRow("Birthdate", _birth);
        AddRow("Weight (kg)", _weight);
        AddRow("Owner on card", _owner);
        AddRow("Vaccinations", table);
        FitHeight();

        // The card is read by the scanner only. Customers can't type over what it found; a wrong read is fixed by scanning again.
        foreach (var box in new[] { _name, _species, _breed, _birth, _weight })
        {
            box.ReadOnly = true;
            box.TabStop = false;
            box.PlaceholderText = "";
            box.BackColor = Color.White;
        }
        _sex.Enabled = false;

        _camera.Click += async (_, _) => await CameraAndScanAsync();
        _choose.Click += async (_, _) => await ChooseAndScanAsync();
    }

    // ------------------------------------------------------------------ scanning

    private async Task ChooseAndScanAsync()
    {
        using var dialog = new OpenFileDialog
        {
            Title = "Choose a photo of the vaccination card",
            Filter = "Pictures (*.jpg;*.jpeg;*.png;*.bmp)|*.jpg;*.jpeg;*.png;*.bmp",
            CheckFileExists = true,
        };
        if (dialog.ShowDialog(this) != DialogResult.OK) return;

        ShowError("");
        byte[] bytes;
        try
        {
            bytes = File.ReadAllBytes(dialog.FileName);
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            ShowError("That file couldn't be opened as a picture.");
            return;
        }
        await ScanAsync(bytes);
    }

    /// <summary>Opens the webcam, lets the user capture one frame, then reads it the same way as a chosen photo.</summary>
    private async Task CameraAndScanAsync()
    {
        ShowError("");
        byte[]? shot;
        using (var camera = new CameraCaptureForm())
        {
            if (camera.ShowDialog(this) != DialogResult.OK) return;
            shot = camera.Jpeg;
        }
        if (shot is null) return;
        await ScanAsync(shot);
    }

    /// <summary>Straightens and shrinks the picture (from a file or the camera), runs OCR on it and fills in the form.</summary>
    private async Task ScanAsync(byte[] picture)
    {
        byte[] jpeg;
        try
        {
            jpeg = CardImage.PrepareJpeg(picture, 2400);
        }
        catch (Exception ex) when (ex is ArgumentException or IOException or UnauthorizedAccessException
                                      or OutOfMemoryException or System.Runtime.InteropServices.ExternalException)
        {
            ShowError("That file couldn't be opened as a picture.");
            return;
        }

        SetBusy(true);
        try
        {
            var read = await ReadCardAsync(jpeg);
            var card = read.Card;
            _engineLabel.Text = read.Engine;
            _engineLabel.ForeColor = read.Engine == "Google Vision" ? Color.FromArgb(30, 130, 76) : Color.FromArgb(199, 122, 10);

            if (card.IsEmpty)
            {
                Reject("Couldn't read any details. Try a clearer photo or better lighting.");
                return;
            }
            if (card.Vaccinations.Count == 0)
            {
                Reject("No vaccination records were found on this card. Make sure the whole card is in the photo.");
                return;
            }

            // Every vaccination must carry the veterinarian's stamp. Rows with an empty signature cell are not accepted.
            var check = await Task.Run(() => StampVerifier.Check(jpeg, read.Pieces));
            var stamped = StampVerifier.KeepStamped(card.Vaccinations, check);
            if (stamped.Count == 0)
            {
                Reject(check.ColumnFound
                    ? "Card not accepted: there is no veterinarian stamp on the vaccination record. Every vaccination must be stamped by the clinic."
                    : "Couldn't find the Veterinarian's Signature/License column. Retake the photo so the whole table is in view.");
                return;
            }
            int skipped = card.Vaccinations.Count - stamped.Count;

            // The pet's picture is cut out of the PET PHOTO frame (a failure here never blocks the scan).
            byte[]? petPhoto = null;
            try { petPhoto = await Task.Run(() => PetPhotoFinder.Find(jpeg, read.Pieces)); }
            catch (Exception ex) when (ex is ArgumentException or OutOfMemoryException
                                          or System.Runtime.InteropServices.ExternalException) { }

            _cardJpeg = jpeg;
            Fill(card with { Vaccinations = stamped });
            ShowPetPhoto(petPhoto);

            string status = $"Read {stamped.Count} stamped vaccination(s). Check it looks right.";
            if (skipped > 0) status += $"  {skipped} row(s) without a stamp were left out.";
            _status.Text = status;
        }
        catch (Exception ex)
        {
            // WinRT can fail in several ways (no language pack, unsupported picture); all of them end up here.
            ShowError("Couldn't read the card.  " + ex.Message);
        }
        finally
        {
            SetBusy(false);
        }
    }

    /// <summary>The card was not accepted: nothing from it may be saved, so the form is emptied and the reason shown.</summary>
    private void Reject(string reason)
    {
        _cardJpeg = null;
        Fill(new ScannedCard("", "", "", "", null, null, "", "", new List<ScannedVaccination>()));
        ShowPetPhoto(null);
        _status.Text = "Card not accepted.";
        ShowError(reason);
    }

    private void ShowPetPhoto(byte[]? jpeg)
    {
        _petPhotoJpeg = jpeg;
        var old = _photoBox.Image;
        _photoBox.Image = AvatarImages.FromBytes(jpeg);
        old?.Dispose();
        _photoNote.Text = jpeg is null ? "No pet photo found on the card." : "Picked up from the card; it will be saved as the pet's photo.";
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing) _photoBox.Image?.Dispose();
        base.Dispose(disposing);
    }

    private sealed record CardRead(ScannedCard Card, string Engine, List<OcrPiece> Pieces);

    /// <summary>
    /// Google Cloud Vision first when a key is configured (much more accurate). If there is no key, or the internet or
    /// quota fails, the built-in Windows OCR is used instead (the photo plus a cleaned-up copy, keeping whichever read more).
    /// </summary>
    private static async Task<CardRead> ReadCardAsync(byte[] jpeg)
    {
        string engine = "offline: no key found. Put it in vision.key in " + GoogleVisionOcr.KeyFolder;
        if (GoogleVisionOcr.IsConfigured)
        {
            try
            {
                var pieces = await GoogleVisionOcr.ReadPiecesAsync(jpeg);
                var online = VaccinationCardParser.Parse(OcrRows.Merge(pieces));
                if (!online.IsEmpty) return new CardRead(online, "Google Vision", pieces);
                engine = "offline: Google found no text";
            }
            catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException
                                          or System.Text.Json.JsonException or InvalidOperationException)
            {
                // Offline, wrong key, quota used up... fall through to the offline reader.
                string why = ex.Message.Length > 140 ? ex.Message[..140] + "\u2026" : ex.Message;
                engine = "offline: Google failed - " + why;
            }
        }

        var windowsPieces = await WindowsOcr.ReadPiecesAsync(jpeg);
        var card = VaccinationCardParser.Parse(OcrRows.Merge(windowsPieces));
        try
        {
            var enhanced = await Task.Run(() => CardEnhancer.Enhance(jpeg));
            var second = VaccinationCardParser.Parse(await WindowsOcr.ReadLinesAsync(enhanced));
            card = CardMerge.Best(card, second);
        }
        catch (Exception ex) when (ex is ArgumentException or OutOfMemoryException
                                      or System.Runtime.InteropServices.ExternalException or InvalidOperationException)
        {
            // The second look is a bonus; the first read still stands.
        }
        return new CardRead(card, engine, windowsPieces);
    }

    private void SetBusy(bool busy)
    {
        UseWaitCursor = busy;
        _choose.Enabled = !busy;
        _camera.Enabled = !busy;
        SaveButton.Enabled = !busy;
        if (busy) _status.Text = "Reading the card\u2026";
    }

    private void Fill(ScannedCard card)
    {
        _name.Text = card.PetName;
        _species.Text = card.Species;
        _breed.Text = card.Breed;
        _sex.SelectedIndex = card.Sex switch { "Male" => 0, "Female" => 1, _ => -1 };
        _birth.Text = card.BirthDate is { } b ? Fmt(b) : "";
        _weight.Text = card.WeightKg is { } w ? w.ToString("0.##", CultureInfo.InvariantCulture) : "";

        _owner.Text = card.Owner;
        _owner.ForeColor = Theme.Text;
        if (card.Owner.Length > 0 && !SameName(card.Owner, _customerName))
        {
            _owner.Text = card.Owner + "   (different from your account name)";
            _owner.ForeColor = Color.FromArgb(199, 122, 10);
        }

        _rows.Clear();
        _rows.AddRange(card.Vaccinations);
        RefreshList();
    }

    private static bool SameName(string a, string b)
    {
        static string Norm(string s) => new(s.Where(char.IsLetter).Select(char.ToLowerInvariant).ToArray());
        string x = Norm(a), y = Norm(b);
        return x.Length > 0 && y.Length > 0 && (x == y || x.Contains(y) || y.Contains(x));
    }

    // ------------------------------------------------------------------ vaccination rows

    private static string Fmt(DateTime d) => d.ToString("MMM d, yyyy", CultureInfo.InvariantCulture);

    private void RefreshList()
    {
        _list.Items.Clear();
        foreach (var r in _rows)
            _list.Items.Add(new ListViewItem(new[]
            {
                r.Given is { } g ? Fmt(g) : "\u2014", r.Vaccine.Length == 0 ? "\u2014" : r.Vaccine,
                r.WeightKg is { } w ? w.ToString("0.##", CultureInfo.InvariantCulture) + " kg" : "\u2014",
                r.BatchNo.Length == 0 ? "\u2014" : r.BatchNo, r.Expires is { } e ? Fmt(e) : "\u2014",
            }));
    }

    // ------------------------------------------------------------------ saving

    protected override void OnSaveClicked()
    {
        ShowError("");
        string name = _name.Text.Trim();
        if (_cardJpeg is null || _rows.Count == 0) { ShowError("Scan a vaccination card that carries the veterinarian's stamp first (use the camera or choose a photo)."); return; }
        if (name.Length == 0) { ShowError("The pet's name couldn't be read. Scan the card again with a clearer photo."); return; }

        DateTime? birth = null;
        if (_birth.Text.Trim().Length > 0)
        {
            if (!VaccinationCardParser.TryParseDate(_birth.Text, out var b)) { ShowError("The birthdate couldn't be read properly. Scan the card again."); return; }
            birth = b;
        }

        decimal? weight = null;
        if (_weight.Text.Trim().Length > 0)
        {
            if (!VaccinationCardParser.TryParseWeight(_weight.Text, out var w)) { ShowError("The weight couldn't be read properly. Scan the card again."); return; }
            weight = w;
        }

        int? existingId = _pets.FirstOrDefault(p => string.Equals(p.Name, name, StringComparison.OrdinalIgnoreCase))?.Id;
        if (existingId is not null)
        {
            var answer = Msg.Show(this,
                $"You already have a pet named {name}\n\nUpdate that pet with this card? Choose No to add a separate pet instead.",
                "Pawfect", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (answer == DialogResult.Cancel) return;
            if (answer == DialogResult.No) existingId = null;
        }

        try
        {
            SavedPetId = PetVaccinations.SaveScan(_customerId, existingId, name, _species.Text, _breed.Text,
                _sex.SelectedItem ?? "", birth, weight, _cardJpeg, _petPhotoJpeg, _rows);
            DialogResult = DialogResult.OK;
        }
        catch (Exception ex) when (ex is SqlException or InvalidOperationException)
        {
            ShowDatabaseError(this, ex);
        }
    }
}

/// <summary>Makes the chosen photo upright and no bigger than needed, as a JPEG (used for both reading and saving).</summary>
internal static class CardImage
{
    public static byte[] PrepareJpeg(byte[] bytes, int maxSide)
    {
        using var source = new MemoryStream(bytes);
        using var image = Image.FromStream(source);

        // Phone photos are often stored sideways with an EXIF "rotate me" flag.
        if (Array.IndexOf(image.PropertyIdList, 0x0112) >= 0)
        {
            int orientation = BitConverter.ToUInt16(image.GetPropertyItem(0x0112)!.Value!, 0);
            switch (orientation)
            {
                case 3: image.RotateFlip(RotateFlipType.Rotate180FlipNone); break;
                case 6: image.RotateFlip(RotateFlipType.Rotate90FlipNone); break;
                case 8: image.RotateFlip(RotateFlipType.Rotate270FlipNone); break;
            }
        }

        double scale = Math.Min(1.0, maxSide / (double)Math.Max(image.Width, image.Height));
        int w = Math.Max(1, (int)(image.Width * scale)), h = Math.Max(1, (int)(image.Height * scale));

        using var bitmap = new Bitmap(w, h);
        using (var g = Graphics.FromImage(bitmap))
        {
            g.Clear(Color.White);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            g.DrawImage(image, 0, 0, w, h);
        }

        return Encode(bitmap, 88L);
    }

    /// <summary>Cuts a rectangle out of a JPEG (e.g. the pet photo from the card), shrinks it to at most <paramref name="maxSide"/> pixels and returns it as a JPEG.</summary>
    public static byte[] CropJpeg(byte[] jpeg, Rectangle area, int maxSide)
    {
        using var source = new MemoryStream(jpeg);
        using var image = new Bitmap(source);
        area.Intersect(new Rectangle(0, 0, image.Width, image.Height));
        if (area.Width < 8 || area.Height < 8) throw new ArgumentException("The area is outside the picture.");

        double scale = Math.Min(1.0, maxSide / (double)Math.Max(area.Width, area.Height));
        int w = Math.Max(1, (int)(area.Width * scale)), h = Math.Max(1, (int)(area.Height * scale));
        using var bitmap = new Bitmap(w, h);
        using (var g = Graphics.FromImage(bitmap))
        {
            g.Clear(Color.White);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            g.DrawImage(image, new Rectangle(0, 0, w, h), area, GraphicsUnit.Pixel);
        }
        return Encode(bitmap, 90L);
    }

    private static byte[] Encode(Bitmap bitmap, long quality)
    {
        var codec = ImageCodecInfo.GetImageEncoders().First(c => c.FormatID == ImageFormat.Jpeg.Guid);
        using var parameters = new EncoderParameters(1);
        parameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
        using var output = new MemoryStream();
        bitmap.Save(output, codec, parameters);
        return output.ToArray();
    }
}
