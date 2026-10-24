using System.Drawing.Drawing2D;
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

    private readonly PawButton _choose = new() { Text = "Choose photo\u2026", Kind = PawButtonKind.Primary, Size = new Size(126, 34) };
    private readonly PawButton _camera = new() { Text = "Use camera", Kind = PawButtonKind.Outline, Size = new Size(120, 34) };
    private readonly Label _status = new() { AutoSize = false, ForeColor = Theme.Muted, Font = Theme.Face(8.5f), BackColor = Color.White };
    private readonly Label _engineLabel = new() { AutoSize = false, AutoEllipsis = true, Font = Theme.Face(8f), BackColor = Color.White };
    private readonly ToolTip _tip = new();
    private readonly CardPreview _preview = new();
    private readonly TextBox _name = new();
    private readonly TextBox _species = new();
    private readonly TextBox _breed = new();
    private readonly PawSelect _sex = new() { Placeholder = "Not stated", BackColor = Color.White };
    private readonly TextBox _birth = new() { PlaceholderText = "e.g. Dec 5, 2024" };
    private readonly TextBox _weight = new() { PlaceholderText = "kg, e.g. 3.8" };
    private readonly Label _owner = new() { AutoSize = false, AutoEllipsis = true, TextAlign = ContentAlignment.MiddleLeft, BackColor = Color.White };
    private readonly ListView _list = new()
    {
        View = View.Details,
        FullRowSelect = true,
        MultiSelect = false,
        HideSelection = false,
        BorderStyle = BorderStyle.None,
        BackColor = Color.White,
    };

    private readonly PictureBox _photoBox = new() { SizeMode = PictureBoxSizeMode.Zoom, BackColor = Theme.Subtle };
    private readonly Label _photoNote = new() { AutoSize = false, ForeColor = Theme.Muted, Font = Theme.Face(8f), BackColor = Color.White };

    private readonly List<ScannedVaccination> _rows = new();
    private byte[]? _cardJpeg;
    private byte[]? _petPhotoJpeg;

    private readonly int? _forPetId;
    private readonly PetProfile? _forPet;   // the details entered in the Pet Details pop-up; the card must agree with them

    public int? SavedPetId { get; private set; }

    /// <param name="forPetId">When set, the scan is saved onto this pet instead of matching one by the name on the card.</param>
    public VaccinationScanForm(int customerId, string customerName, int? forPetId = null)
        : base("Scan Vaccination Card", "Save", "\uE722", "Use the camera or a photo and we'll read it for you")
    {
        _customerId = customerId;
        _customerName = customerName;
        _forPetId = forPetId;
        _pets = EmployeeData.GetPets().Where(p => p.CustomerId == customerId).ToList();
        if (forPetId is int petId)
        {
            try { _forPet = PetProfiles.GetForCustomer(customerId).FirstOrDefault(p => p.Id == petId); }
            catch (SqlException) { }   // left null: the card then can't be checked, so ScanAsync refuses it
        }

        // Landscape: card photo + pet details side by side, the vaccination table across the bottom.
        ClientSize = new Size(920, 640);
        Grid.Visible = false;   // DialogBase's tall "label | input" grid is replaced by the cards below

        var body = new Panel { Dock = DockStyle.Fill, BackColor = Theme.Bg };
        Controls.Add(body);
        body.BringToFront();    // docked last, so it fills whatever the header, error strip and button bar leave

        // ---- card photo
        var photoCard = NewCard(body, new Rectangle(16, 16, 290, 276));
        photoCard.Controls.Add(Caption("Card photo", 18, 14));
        _preview.SetBounds(18, 44, 254, 112);
        _camera.Location = new Point(18, 168);
        _choose.Location = new Point(146, 168);
        _status.SetBounds(18, 206, 254, 44);
        _engineLabel.SetBounds(18, 252, 254, 16);
        photoCard.Controls.AddRange(new Control[] { _preview, _camera, _choose, _status, _engineLabel });
        _status.Text = "Details are read from the card automatically.";
        SetEngine("\u2014", Theme.Muted);

        // ---- pet details
        var detailsCard = NewCard(body, new Rectangle(322, 16, 582, 276));
        detailsCard.Controls.Add(Caption("Pet details", 18, 14));

        var photoFrame = new CardPanel
        {
            Radius = 12,
            FillColor = Theme.Subtle,
            CornerColor = Color.White,
            BackColor = Theme.Subtle,
            Padding = new Padding(4),
            Bounds = new Rectangle(18, 48, 104, 104),
        };
        _photoBox.Dock = DockStyle.Fill;
        photoFrame.Controls.Add(_photoBox);
        _photoNote.SetBounds(18, 158, 108, 64);
        _photoNote.Text = "The pet's picture is taken from the card.";
        detailsCard.Controls.Add(photoFrame);
        detailsCard.Controls.Add(_photoNote);

        const int c1 = 142, c2 = 362, fw = 200;
        _sex.Items.AddRange(new object[] { "Male", "Female" });
        AddField(detailsCard, "Pet name *", _name, c1, 44, fw);
        AddField(detailsCard, "Species", _species, c2, 44, fw);
        AddField(detailsCard, "Breed", _breed, c1, 110, fw);
        AddField(detailsCard, "Sex", _sex, c2, 110, fw);
        AddField(detailsCard, "Birthdate", _birth, c1, 176, fw);
        AddField(detailsCard, "Weight (kg)", _weight, c2, 176, fw);

        var ownerCaption = Ui.Text("Owner on card", 8.5f, Theme.TextSoft, Color.White, FontStyle.Bold);
        ownerCaption.SetBounds(c1, 244, 100, 22);
        _owner.SetBounds(c1 + 104, 244, 582 - c1 - 104 - 20, 22);
        detailsCard.Controls.Add(ownerCaption);
        detailsCard.Controls.Add(_owner);

        // ---- vaccinations (grows or shrinks with the window when the error strip shows)
        var tableCard = NewCard(body, new Rectangle(16, 308, 888, 180));
        tableCard.Controls.Add(Caption("Vaccinations", 18, 12));
        _list.Columns.Add("Date given", 140);
        _list.Columns.Add("Vaccine", 280);
        _list.Columns.Add("Weight", 100);
        _list.Columns.Add("Batch / Lot", 160);
        _list.Columns.Add("Expires", 150);
        _list.SetBounds(18, 40, 852, 126);
        tableCard.Controls.Add(_list);
        // Sizes are worked out from the controls' own (possibly scaled) positions, so they hold at any screen size.
        tableCard.SizeChanged += (_, _) => _list.Height = Math.Max(_list.Top * 3 / 2, tableCard.Height - _list.Top - _list.Top * 14 / 40);
        body.Resize += (_, _) => tableCard.Height = Math.Max(tableCard.Left * 15 / 2, body.ClientSize.Height - tableCard.Top - tableCard.Left);
        int[] columnWidths = { 140, 280, 100, 160, 150 };
        _list.SizeChanged += (_, _) =>
        {
            double f = _list.ClientSize.Width / 852.0;
            for (int i = 0; i < columnWidths.Length && i < _list.Columns.Count; i++)
                _list.Columns[i].Width = (int)(columnWidths[i] * f);
        };

        // The card is read by the scanner only. Customers can't type over what it found; a wrong read is fixed by scanning again.
        foreach (var box in new[] { _name, _species, _breed, _birth, _weight })
        {
            box.ReadOnly = true;
            box.TabStop = false;
            box.PlaceholderText = "";
            box.BackColor = Color.White;
        }
        _sex.Enabled = false;

        // Greyed out and unfocusable: it is obvious these are read from the card and can't be typed over.
        foreach (var box in new[] { _name, _species, _breed, _birth, _weight })
        {
            box.BackColor = Theme.Subtle;
            box.ForeColor = Theme.TextSoft;
            box.Cursor = Cursors.Default;
            box.GotFocus += (_, _) => BeginInvoke(() => ActiveControl = null);
            if (box.Parent is CardPanel frame)
            {
                frame.FillColor = Theme.Subtle;
                frame.Cursor = Cursors.Default;
                frame.Click += (_, _) => ActiveControl = null;
            }
        }
        SaveButton.Enabled = false;   // nothing to save until a card has been read and accepted

        _camera.Click += async (_, _) => await CameraAndScanAsync();
        _choose.Click += async (_, _) => await ChooseAndScanAsync();
        _preview.Click += async (_, _) => { if (_choose.Enabled) await ChooseAndScanAsync(); };

        FullScreen.Apply(this, centerContent: true);   // full screen; the 920 x 640 layout is scaled up and centred
    }

    // ------------------------------------------------------------------ layout helpers

    private static CardPanel NewCard(Control parent, Rectangle bounds)
    {
        var card = new CardPanel
        {
            Radius = 14,
            FillColor = Color.White,
            CornerColor = Theme.Bg,
            BorderColor = Theme.Border,
            BackColor = Color.White,
            Bounds = bounds,
        };
        parent.Controls.Add(card);
        return card;
    }

    private static Label Caption(string text, int x, int y)
    {
        var l = Ui.Text(text, 9.5f, Theme.Text, Color.White, FontStyle.Bold);
        l.SetBounds(x, y, 260, 22);
        return l;
    }

    /// <summary>Small bold caption with the (rounded) editor under it.</summary>
    private static void AddField(Control parent, string caption, Control editor, int x, int y, int width)
    {
        var cap = Ui.Text(caption, 8.5f, Theme.TextSoft, Color.White, FontStyle.Bold);
        cap.SetBounds(x, y, width, 18);
        parent.Controls.Add(cap);

        Control cell = editor is TextBox ? FieldFrame.ForEditor(editor) : editor;
        cell.SetBounds(x, y + 20, width, 36);
        parent.Controls.Add(cell);
    }

    private void SetEngine(string engine, Color color)
    {
        _engineLabel.Text = "Read with: " + engine;
        _engineLabel.ForeColor = color;
        _tip.SetToolTip(_engineLabel, engine);   // long "offline: ..." reasons are cut off in the label
    }

    private void ShowCardPicture(byte[]? jpeg)
    {
        Image? thumb = null;
        if (jpeg is not null)
        {
            try
            {
                using var ms = new MemoryStream(jpeg);
                using var source = Image.FromStream(ms);
                double scale = Math.Min(1.0, 640.0 / Math.Max(source.Width, source.Height));
                thumb = new Bitmap(source, new Size(Math.Max(1, (int)(source.Width * scale)), Math.Max(1, (int)(source.Height * scale))));
            }
            catch (Exception ex) when (ex is ArgumentException or OutOfMemoryException) { }
        }
        _preview.Picture = thumb;
    }

    /// <summary>The photo frame: brackets and a camera tile until a card is read, then the card itself.</summary>
    private sealed class CardPreview : Control
    {
        private Image? _picture;

        public CardPreview()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
            BackColor = Color.White;
            Cursor = Cursors.Hand;
        }

        public Image? Picture
        {
            get => _picture;
            set { var old = _picture; _picture = value; old?.Dispose(); Invalidate(); }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            var r = new Rectangle(0, 0, Width - 1, Height - 1);
            float k = Math.Max(1f, Height / 112f);   // how much bigger than the original design this frame is
            int K(int v) => (int)Math.Round(v * k);

            using (var path = Ui.RoundRect(r, K(12)))
            {
                using var fill = new SolidBrush(Theme.PrimarySoft);
                using var pen = new Pen(Theme.Border);
                g.FillPath(fill, path);
                g.DrawPath(pen, path);
            }

            if (_picture is { } pic)
            {
                var area = Rectangle.Inflate(r, -K(14), -K(14));
                float scale = Math.Min(area.Width / (float)pic.Width, area.Height / (float)pic.Height);
                int w = Math.Max(1, (int)(pic.Width * scale)), h = Math.Max(1, (int)(pic.Height * scale));
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(pic, new Rectangle(area.X + (area.Width - w) / 2, area.Y + (area.Height - h) / 2, w, h));
            }
            else
            {
                var tile = new Rectangle(Width / 2 - K(20), K(22), K(40), K(40));
                using (var path = Ui.RoundRect(tile, K(10)))
                {
                    using var white = new SolidBrush(Color.White);
                    using var pen = new Pen(Theme.Border);
                    g.FillPath(white, path);
                    g.DrawPath(pen, path);
                }
                TextRenderer.DrawText(g, "\uE722", Theme.GlyphFont(FullScreen.Pt(14f * k)), tile, Theme.Primary,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);
                TextRenderer.DrawText(g, "Click to choose a photo", Theme.Face(FullScreen.Pt(8.5f * k)), new Rectangle(0, K(70), Width, K(24)), Theme.Muted,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);
            }

            using var br = new Pen(Theme.Primary, 2.5f * k) { StartCap = LineCap.Round, EndCap = LineCap.Round, LineJoin = LineJoin.Round };
            int m = K(8), len = K(16), rw = Width - 1, rh = Height - 1;
            g.DrawLines(br, new[] { new Point(m, m + len), new Point(m, m), new Point(m + len, m) });
            g.DrawLines(br, new[] { new Point(rw - m - len, m), new Point(rw - m, m), new Point(rw - m, m + len) });
            g.DrawLines(br, new[] { new Point(m, rh - m - len), new Point(m, rh - m), new Point(m + len, rh - m) });
            g.DrawLines(br, new[] { new Point(rw - m - len, rh - m), new Point(rw - m, rh - m), new Point(rw - m, rh - m - len) });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) _picture?.Dispose();
            base.Dispose(disposing);
        }
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
            SetEngine(read.Engine, read.Engine == "Google Vision" ? Color.FromArgb(30, 130, 76) : Color.FromArgb(199, 122, 10));

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
                    ? "Card not accepted: there is no veterinarian stamp on the vaccination record. Every vaccination must be stamped by a veterinarian."
                    : "Couldn't find the Veterinarian's Signature/License column. Retake the photo so the whole table is in view.");
                return;
            }
            int skipped = card.Vaccinations.Count - stamped.Count;

            // The card must belong to this pet and this owner. Anything that differs means the card is not accepted.
            var mismatches = new List<PetMismatch>();
            if (_forPetId is not null)
            {
                if (_forPet is null)
                {
                    Reject("Couldn't load the pet's details to check them against the card. Please try again.");
                    return;
                }
                mismatches.AddRange(PetCardMatch.Compare(_forPet, card));
            }
            if (PetCardMatch.CompareOwner(_customerName, card) is { } ownerProblem) mismatches.Add(ownerProblem);
            if (mismatches.Count > 0)
            {
                Reject("Card not accepted: the details don't match the vaccine card.");
                using (var dialog = new CardMismatchDialog(mismatches)) dialog.ShowDialog(this);
                return;
            }

            // A card with any vaccine that is no longer in date is not accepted.
            var expired = PetCardMatch.Expired(stamped);
            if (expired.Count > 0)
            {
                Reject("Card not accepted: the vaccine card has expired.");
                using (var dialog = new CardMismatchDialog(expired, "Vaccine card has expired",
                           "Please scan a card with current vaccinations.", "EXPIRY DATE", "STATUS"))
                    dialog.ShowDialog(this);
                return;
            }

            // Every card must have an anti-rabies vaccination. A card without one is not accepted.
            if (PetCardMatch.MissingRabies(stamped) is { } noRabies)
            {
                Reject("Card not accepted: there is no anti-rabies vaccination on the card.");
                using (var dialog = new CardMismatchDialog(new List<PetMismatch> { noRabies }, "Anti-rabies vaccine missing",
                           "The card must show an anti-rabies vaccination.", "NEEDED", "ON THE CARD"))
                    dialog.ShowDialog(this);
                return;
            }

            // Cats: a vaccine protects only 14 days after the shot. A card with a vaccine still inside that window is not accepted.
            if (PetCardMatch.IsCat(card))
            {
                var pending = PetCardMatch.ImmunityPending(stamped);
                if (pending.Count > 0)
                {
                    Reject("Card not accepted: the 14-day immunity period after vaccination is not done yet.");
                    using (var dialog = new CardMismatchDialog(pending, "14-day immunity not complete",
                               "Immunity takes 14 days after the shot. Try again after that.", "GIVEN ON", "PROTECTED ON"))
                        dialog.ShowDialog(this);
                    return;
                }
            }

            // The pet's picture is cut out of the PET PHOTO frame (a failure here never blocks the scan).
            byte[]? petPhoto = null;
            try { petPhoto = await Task.Run(() => PetPhotoFinder.Find(jpeg, read.Pieces)); }
            catch (Exception ex) when (ex is ArgumentException or OutOfMemoryException
                                          or System.Runtime.InteropServices.ExternalException)
            { }

            _cardJpeg = jpeg;
            ShowCardPicture(jpeg);
            Fill(card with { Vaccinations = stamped });
            ShowPetPhoto(petPhoto);

            string status = $"Read {stamped.Count} stamped vaccination(s). Check it looks right.";
            if (skipped > 0) status += $"  {skipped} row(s) without a stamp were left out.";
            _status.Text = status;
            SaveButton.Enabled = true;
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
        ShowCardPicture(null);
        Fill(new ScannedCard("", "", "", "", null, null, "", "", new List<ScannedVaccination>()));
        ShowPetPhoto(null);
        _status.Text = "Card not accepted.";
        SaveButton.Enabled = false;
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
        SaveButton.Enabled = !busy && _cardJpeg is not null && _rows.Count > 0;
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
        var today = DateTime.Today;
        foreach (var r in _rows)
        {
            string expires = r.Expires is { } e
                ? Fmt(e) + (e.Date < today ? "  (EXPIRED)" : "")
                : "Not read \u2014 check";
            var item = new ListViewItem(new[]
            {
                r.Given is { } g ? Fmt(g) : "\u2014", r.Vaccine.Length == 0 ? "\u2014" : r.Vaccine,
                r.WeightKg is { } w ? w.ToString("0.##", CultureInfo.InvariantCulture) + " kg" : "\u2014",
                r.BatchNo.Length == 0 ? "\u2014" : r.BatchNo, expires,
            });
            if (r.Expires is null) item.ForeColor = Color.DarkOrange;
            else if (r.Expires.Value.Date < today) item.ForeColor = Theme.Danger;
            _list.Items.Add(item);
        }
    }

    // ------------------------------------------------------------------ saving

    protected override void OnSaveClicked()
    {
        ShowError("");
        string name = _name.Text.Trim();
        if (_cardJpeg is null || _rows.Count == 0) { ShowError("Scan a vaccination card that carries the veterinarian's stamp first (use the camera or choose a photo)."); return; }
        if (_rows.All(r => r.Expires is null)) { ShowError("No expiry dates could be read from this card. Scan it again with a clearer photo."); return; }
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

        int? existingId = _forPetId ?? _pets.FirstOrDefault(p => string.Equals(p.Name, name, StringComparison.OrdinalIgnoreCase))?.Id;
        if (existingId is not null && _forPetId is null)
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