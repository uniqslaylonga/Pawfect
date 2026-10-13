using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using Microsoft.Data.SqlClient;
using MyApp.Controls;

namespace MyApp;

/// <summary>
/// "View Pets" window opened from Customer &amp; Pet Lookup. The left list shows the owner's pets; picking one shows its
/// photo, details and vaccination card on the right. The pictures are view-only here.
/// </summary>
internal sealed class PetViewerForm : Form
{
    private readonly int _customerId;
    private readonly string _ownerName;

    private readonly Label _listTitle = new();
    private readonly ListBox _list = new();
    private readonly Label _noPets = new();
    private readonly Panel _content = new();
    private readonly Panel _top = new();
    private readonly PictureBox _photoBox = new();
    private readonly Label _nameLabel = new();
    private readonly Label _subLabel = new();
    private readonly TableLayoutPanel _details = new();
    private readonly Label _cardHeader = new();
    private readonly PictureBox _cardBox = new();
    private readonly Label _vaxHeader = new();
    private readonly ListView _vaxList = new();
    private readonly Button _editDetails;
    private readonly Dictionary<int, Image?> _thumbs = new();

    private List<PetProfile> _pets = new();
    private Image? _photoImage;
    private Image? _cardImage;
    private bool _loading;

    /// <summary>True if a pet was added here, so the lookup lists should be reloaded.</summary>
    public bool PetsChanged { get; private set; }

    public PetViewerForm(int customerId, string ownerName, int? selectPetId = null)
    {
        _customerId = customerId;
        _ownerName = ownerName;

        Text = "Pets - " + ownerName;
        Font = Theme.Face(9f);
        BackColor = Color.White;
        ForeColor = Theme.Text;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        StartPosition = FormStartPosition.CenterParent;
        MaximizeBox = false;
        MinimizeBox = false;
        ShowInTaskbar = false;
        ClientSize = new Size(960, 780);

        _editDetails = MakeButton("Edit details", 110, primary: false);
        var addPet = MakeButton("Add pet", 90, primary: false);
        var close = MakeButton("Close", 90, primary: true);
        close.DialogResult = DialogResult.Cancel;
        CancelButton = close;

        _editDetails.Click += (_, _) => EditDetails();
        addPet.Click += (_, _) => AddPet();

        BuildLayout(new[] { close, _editDetails, addPet });
        LoadPets(selectPetId);
    }

    // ------------------------------------------------------------------ layout

    private static Button MakeButton(string text, int width, bool primary)
    {
        var b = new Button
        {
            Text = text,
            Size = new Size(width, 34),
            FlatStyle = FlatStyle.Flat,
            Cursor = Cursors.Hand,
            Margin = new Padding(6, 0, 0, 0),
            UseMnemonic = false,
        };
        if (primary)
        {
            b.FlatAppearance.BorderSize = 0;
            b.BackColor = Theme.Primary;
            b.ForeColor = Color.White;
            b.Font = Theme.Face(9f, FontStyle.Bold);
        }
        else
        {
            b.FlatAppearance.BorderColor = Theme.Border;
            b.BackColor = Color.White;
            b.ForeColor = Theme.TextSoft;
        }
        return b;
    }

    private void BuildLayout(Button[] buttonsRightToLeft)
    {
        // ---- right side: details on top, vaccination card below
        // (Fill controls are added first and edge controls after them, which is the order WinForms docks in.)
        _noPets.Dock = DockStyle.Fill;
        _noPets.TextAlign = ContentAlignment.MiddleCenter;
        _noPets.ForeColor = Theme.Muted;
        _noPets.Font = Theme.Face(11f);
        _noPets.UseMnemonic = false;
        _noPets.Text = _ownerName + " has no pets yet." + Environment.NewLine + "Use \"Add pet\" below to add one.";
        _noPets.Visible = false;

        _cardBox.Dock = DockStyle.Fill;
        _cardBox.SizeMode = PictureBoxSizeMode.Zoom;
        _cardBox.BackColor = Theme.Subtle;
        _cardBox.BorderStyle = BorderStyle.FixedSingle;
        _cardBox.Click += (_, _) => OpenLarge(_cardImage, "Vaccination card");
        PaintEmpty(_cardBox, "No vaccination card uploaded yet.");

        _cardHeader.Dock = DockStyle.Top;
        _cardHeader.Height = 32;
        _cardHeader.Font = Theme.Face(10f, FontStyle.Bold);
        _cardHeader.TextAlign = ContentAlignment.BottomLeft;
        _cardHeader.Text = "Vaccination card  (click the picture to enlarge)";

        // vaccination records read from the scanned card (shown above the card picture)
        _vaxList.Dock = DockStyle.Top;
        _vaxList.Height = 120;
        _vaxList.View = View.Details;
        _vaxList.FullRowSelect = true;
        _vaxList.MultiSelect = false;
        _vaxList.HideSelection = false;
        _vaxList.BorderStyle = BorderStyle.FixedSingle;
        _vaxList.Columns.Add("Vaccine", 190);
        _vaxList.Columns.Add("Date given", 120);
        _vaxList.Columns.Add("Weight", 80);
        _vaxList.Columns.Add("Batch / Lot", 120);
        _vaxList.Columns.Add("Expires", 120);

        _vaxHeader.Dock = DockStyle.Top;
        _vaxHeader.Height = 32;
        _vaxHeader.Font = Theme.Face(10f, FontStyle.Bold);
        _vaxHeader.TextAlign = ContentAlignment.BottomLeft;
        _vaxHeader.UseMnemonic = false;
        _vaxHeader.Text = "Vaccination records";

        // top block: [photo][gap][name / species / details]
        _details.Dock = DockStyle.Fill;
        _details.ColumnCount = 2;
        _details.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100));
        _details.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        _details.AutoScroll = true;

        _subLabel.Dock = DockStyle.Top;
        _subLabel.Height = 26;
        _subLabel.ForeColor = Theme.Muted;
        _subLabel.UseMnemonic = false;

        _nameLabel.Dock = DockStyle.Top;
        _nameLabel.Height = 36;
        _nameLabel.Font = Theme.Face(18f, FontStyle.Bold);
        _nameLabel.UseMnemonic = false;

        var detailsHost = new Panel { Dock = DockStyle.Fill };
        detailsHost.Controls.Add(_details);
        detailsHost.Controls.Add(_subLabel);
        detailsHost.Controls.Add(_nameLabel);

        var gap = new Panel { Dock = DockStyle.Left, Width = 20 };

        _photoBox.Dock = DockStyle.Left;
        _photoBox.Width = 220;
        _photoBox.SizeMode = PictureBoxSizeMode.Zoom;
        _photoBox.BackColor = Theme.Subtle;
        _photoBox.BorderStyle = BorderStyle.FixedSingle;
        _photoBox.Click += (_, _) => OpenLarge(_photoImage, "Photo");
        PaintEmpty(_photoBox, "No photo yet");

        _top.Dock = DockStyle.Top;
        _top.Height = 250;
        _top.Controls.Add(detailsHost);
        _top.Controls.Add(gap);
        _top.Controls.Add(_photoBox);

        _content.Dock = DockStyle.Fill;
        _content.Padding = new Padding(20, 16, 20, 12);
        _content.Controls.Add(_cardBox);
        _content.Controls.Add(_cardHeader);
        _content.Controls.Add(_vaxList);
        _content.Controls.Add(_vaxHeader);
        _content.Controls.Add(_top);
        _content.Controls.Add(_noPets);

        // ---- left side: the list of pets
        _list.Dock = DockStyle.Fill;
        _list.BorderStyle = BorderStyle.None;
        _list.BackColor = Theme.Subtle;
        _list.DrawMode = DrawMode.OwnerDrawFixed;
        _list.ItemHeight = 58;
        _list.IntegralHeight = false;
        _list.DrawItem += List_DrawItem;
        _list.SelectedIndexChanged += (_, _) => { if (!_loading) ShowSelected(); };

        _listTitle.Dock = DockStyle.Top;
        _listTitle.Height = 48;
        _listTitle.Padding = new Padding(14, 0, 8, 0);
        _listTitle.TextAlign = ContentAlignment.MiddleLeft;
        _listTitle.Font = Theme.Face(10f, FontStyle.Bold);
        _listTitle.BackColor = Theme.Subtle;
        _listTitle.UseMnemonic = false;

        var left = new Panel { Dock = DockStyle.Left, Width = 250, BackColor = Theme.Subtle };
        left.Controls.Add(_list);
        left.Controls.Add(_listTitle);

        // ---- bottom bar
        var bar = new FlowLayoutPanel
        {
            Dock = DockStyle.Bottom,
            FlowDirection = FlowDirection.RightToLeft,
            Height = 58,
            Padding = new Padding(16, 12, 16, 8),
            BackColor = Theme.Subtle,
        };
        foreach (var b in buttonsRightToLeft) bar.Controls.Add(b);

        Controls.Add(_content);
        Controls.Add(left);
        Controls.Add(bar);
    }

    private static void PaintEmpty(PictureBox box, string message)
    {
        box.Paint += (_, e) =>
        {
            if (box.Image is not null) return;
            TextRenderer.DrawText(e.Graphics, message, Theme.Face(9f), box.ClientRectangle, Theme.Muted,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.WordBreak |
                TextFormatFlags.NoPrefix);
        };
    }

    // ------------------------------------------------------------------ list

    private PetProfile? Selected
        => _list.SelectedIndex >= 0 && _list.SelectedIndex < _pets.Count ? _pets[_list.SelectedIndex] : null;

    private void List_DrawItem(object? sender, DrawItemEventArgs e)
    {
        if (e.Index < 0 || e.Index >= _pets.Count) return;
        var pet = _pets[e.Index];
        bool selected = (e.State & DrawItemState.Selected) != 0;
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;

        using (var back = new SolidBrush(selected ? Theme.PrimarySoft : Theme.Subtle))
            g.FillRectangle(back, e.Bounds);

        var thumb = new Rectangle(e.Bounds.X + 14, e.Bounds.Y + 9, 40, 40);
        if (_thumbs.TryGetValue(pet.Id, out var img) && img is not null)
        {
            int side = Math.Min(img.Width, img.Height);
            var source = new Rectangle((img.Width - side) / 2, (img.Height - side) / 2, side, side);
            using var clip = new GraphicsPath();
            clip.AddEllipse(thumb);
            var state = g.Save();
            g.SetClip(clip);
            g.DrawImage(img, thumb, source, GraphicsUnit.Pixel);
            g.Restore(state);
        }
        else
        {
            using var fill = new SolidBrush(Theme.Track);
            g.FillEllipse(fill, thumb);
            TextRenderer.DrawText(g, "\uE80F", Theme.GlyphFont(14f), thumb, Theme.Muted,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);
        }

        const TextFormatFlags flags = TextFormatFlags.Left | TextFormatFlags.VerticalCenter |
            TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine | TextFormatFlags.NoPadding;
        int textX = thumb.Right + 12;
        int textW = e.Bounds.Right - textX - 8;
        TextRenderer.DrawText(g, pet.Name, Theme.Face(10f, FontStyle.Bold),
            new Rectangle(textX, e.Bounds.Y + 8, textW, 22), Theme.Text, flags);

        string kind = string.Join(" - ", new[] { pet.Species, pet.Breed }.Where(s => !string.IsNullOrWhiteSpace(s)));
        TextRenderer.DrawText(g, kind.Length == 0 ? "No details" : kind, Theme.Face(8.5f),
            new Rectangle(textX, e.Bounds.Y + 30, textW, 20), Theme.Muted, flags);
    }

    // ------------------------------------------------------------------ loading & showing

    private void LoadPets(int? selectId)
    {
        var pets = PetProfiles.GetForCustomer(_customerId);   // may throw SqlException; callers handle it

        _loading = true;
        foreach (var img in _thumbs.Values) img?.Dispose();
        _thumbs.Clear();
        _pets = pets;

        _list.BeginUpdate();
        _list.Items.Clear();
        foreach (var p in _pets)
        {
            _list.Items.Add(p.Name);
            _thumbs[p.Id] = AvatarImages.FromBytes(p.Photo);
        }
        _list.EndUpdate();

        int index = selectId is int id ? _pets.FindIndex(p => p.Id == id) : -1;
        if (index < 0 && _pets.Count > 0) index = 0;
        if (index >= 0) _list.SelectedIndex = index;
        _loading = false;

        ShowSelected();
    }

    private void ShowSelected()
    {
        var pet = Selected;

        _listTitle.Text = _pets.Count == 1 ? "1 pet" : $"{_pets.Count} pets";
        _noPets.Visible = _pets.Count == 0;
        _top.Visible = _cardHeader.Visible = _cardBox.Visible = _vaxHeader.Visible = _vaxList.Visible = pet is not null;
        _editDetails.Enabled = pet is not null;

        if (pet is null)
        {
            SetImage(_photoBox, ref _photoImage, null);
            SetImage(_cardBox, ref _cardImage, null);
            _vaxList.Items.Clear();
            return;
        }

        SetImage(_photoBox, ref _photoImage, pet.Photo);
        SetImage(_cardBox, ref _cardImage, pet.VaccinationCard);
        ShowVaccinations(pet.Id);

        _nameLabel.Text = pet.Name;
        string kind = string.Join(" - ", new[] { pet.Species, pet.Breed }.Where(s => !string.IsNullOrWhiteSpace(s)));
        _subLabel.Text = kind.Length == 0 ? "Species and breed not set" : kind;

        ShowDetails(new List<(string Key, string Value)>
        {
            ("Owner", pet.OwnerName),
            ("Owner phone", Or(pet.OwnerPhone)),
            ("Gender", Or(pet.Gender)),
            ("Birthday", BirthdayText(pet.BirthDate)),
            ("Weight", pet.WeightKg is decimal kg ? $"{kg:0.##} kg" : "Not set"),
            ("Color", Or(pet.CoatColor)),
            ("Notes", Or(pet.Notes)),
        });
    }

    /// <summary>Lists the vaccinations saved from the scanned card (newest first). Expired ones are shown in red.</summary>
    private void ShowVaccinations(int petId)
    {
        _vaxList.BeginUpdate();
        _vaxList.Items.Clear();
        try
        {
            var records = PetVaccinations.GetForPet(petId);
            _vaxHeader.Text = records.Count == 0 ? "Vaccination records  (none scanned yet)" : $"Vaccination records  ({records.Count})";
            foreach (var r in records)
            {
                var item = new ListViewItem(new[]
                {
                    r.Vaccine,
                    r.Given is { } g ? g.ToString("MMM d, yyyy") : "-",
                    r.WeightKg is { } w ? $"{w:0.##} kg" : "-",
                    r.BatchNo.Length == 0 ? "-" : r.BatchNo,
                    r.Expires is { } e ? e.ToString("MMM d, yyyy") : "-",
                });
                if (r.Expires is { } expiry && expiry.Date < DateTime.Today)
                {
                    item.ForeColor = Color.FromArgb(200, 40, 40);
                    item.SubItems[4].Text += "  (expired)";
                }
                _vaxList.Items.Add(item);
            }
        }
        catch (Exception ex) when (ex is SqlException or InvalidOperationException)
        {
            _vaxHeader.Text = "Vaccination records  (couldn't be loaded)";
        }
        finally
        {
            _vaxList.EndUpdate();
        }
    }

    private static string Or(string value) => string.IsNullOrWhiteSpace(value) ? "Not set" : value;

    private static string BirthdayText(DateTime? birth)
    {
        if (birth is not DateTime b) return "Not set";
        var today = DateTime.Today;
        int months = (today.Year - b.Year) * 12 + today.Month - b.Month;
        if (today.Day < b.Day) months--;
        if (months < 0) months = 0;
        string age = months < 12 ? $"{months} mo"
            : months % 12 == 0 ? $"{months / 12} yr"
            : $"{months / 12} yr {months % 12} mo";
        return $"{b:MMM d, yyyy} ({age})";
    }

    private void ShowDetails(List<(string Key, string Value)> rows)
    {
        _details.SuspendLayout();
        _details.Controls.Clear();
        _details.RowStyles.Clear();
        _details.RowCount = rows.Count;
        for (int i = 0; i < rows.Count; i++)
        {
            _details.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            _details.Controls.Add(Cell(rows[i].Key, key: true), 0, i);
            _details.Controls.Add(Cell(rows[i].Value, key: false), 1, i);
        }
        _details.ResumeLayout(true);
    }

    private static Label Cell(string text, bool key) => new()
    {
        Text = text,
        AutoSize = true,
        UseMnemonic = false,
        MaximumSize = new Size(key ? 100 : 440, 0),
        ForeColor = key ? Theme.Muted : Theme.Text,
        Margin = new Padding(0, 3, 8, 3),
    };

    /// <summary>Shows picture bytes in a box, disposing the picture it showed before.</summary>
    private static void SetImage(PictureBox box, ref Image? slot, byte[]? bytes)
    {
        var old = slot;
        slot = AvatarImages.FromBytes(bytes);
        box.Image = slot;
        old?.Dispose();
    }

    private void OpenLarge(Image? image, string title)
    {
        if (image is null) return;
        using var viewer = new ImageViewerForm(title + " - " + (Selected?.Name ?? ""), image);
        viewer.ShowDialog(this);
    }

    // ------------------------------------------------------------------ actions

    private void EditDetails()
    {
        var pet = Selected;
        if (pet is null) return;
        using var form = new PetDetailsForm(pet);
        if (form.ShowDialog(this) != DialogResult.OK) return;
        SaveThenReload(pet.Id, () => { });   // already saved by the form; this just reloads
    }

    private void AddPet()
    {
        using var form = new PetForm(_customerId, _ownerName);
        if (form.ShowDialog(this) != DialogResult.OK) return;
        PetsChanged = true;
        SaveThenReload(form.CreatedPetId ?? 0, () => { });
    }

    private void SaveThenReload(int keepPetId, Action save)
    {
        try
        {
            save();
            LoadPets(keepPetId);
        }
        catch (Exception ex) when (ex is SqlException or InvalidOperationException)
        {
            Msg.Show(this, "The database could not be updated.\n\n" + ex.Message, "Pawfect",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    protected override void OnFormClosed(FormClosedEventArgs e)
    {
        foreach (var img in _thumbs.Values) img?.Dispose();
        _thumbs.Clear();
        _photoBox.Image = null;
        _cardBox.Image = null;
        _photoImage?.Dispose();
        _cardImage?.Dispose();
        base.OnFormClosed(e);
    }
}

/// <summary>Edits the extra pet details: gender, birthday, weight, color and notes.</summary>
internal sealed class PetDetailsForm : DialogBase
{
    private readonly PetProfile _pet;
    private readonly PawSelect _gender = new();
    private readonly PawDatePicker _birth = new() { AllowEmpty = true, Placeholder = "Not set" };
    private readonly NumericUpDown _weight = new() { DecimalPlaces = 1, Minimum = 0, Maximum = 200, Increment = 0.5m };
    private readonly TextBox _color = new() { MaxLength = 40 };
    private readonly TextBox _notes = new() { MaxLength = 500, Multiline = true, AcceptsReturn = true, Height = 80, ScrollBars = ScrollBars.Vertical };

    public PetDetailsForm(PetProfile pet) : base("Pet details", "Save", "\uE77B", pet.Name)
    {
        _pet = pet;

        _birth.MaxDate = DateTime.Today;
        _gender.Items.AddRange(new object[] { "Not set", "Male", "Female" });
        _gender.SelectedIndex = pet.Gender switch { "Male" => 1, "Female" => 2, _ => 0 };

        if (pet.BirthDate is DateTime b && b >= _birth.MinDate && b <= _birth.MaxDate)
        {
            _birth.Value = b;
            _birth.Checked = true;
        }
        else
        {
            _birth.Checked = false;   // the tick box means "birthday is known"
        }

        _weight.Value = Math.Min(_weight.Maximum, pet.WeightKg ?? 0m);
        _color.Text = pet.CoatColor;
        _notes.Text = pet.Notes;

        AddRow("Pet", new Label { Text = pet.Name, TextAlign = ContentAlignment.MiddleLeft, AutoSize = false, Height = 28 });
        AddRow("Gender", _gender);
        AddRow("Birthday", _birth);
        AddRow("Weight (kg)", _weight);
        AddRow("Color", _color);
        AddRow("Notes", _notes);
        FitHeight();
    }

    protected override void OnSaveClicked()
    {
        if (_birth.Checked && _birth.Value.Date > DateTime.Today)
        {
            ShowError("The birthday can't be in the future.");
            return;
        }

        try
        {
            PetProfiles.SaveDetails(_pet.Id,
                _gender.SelectedIndex switch { 1 => "Male", 2 => "Female", _ => "" },
                _birth.Checked ? _birth.Value.Date : null,
                _weight.Value > 0 ? _weight.Value : null,
                _color.Text, _notes.Text);
            DialogResult = DialogResult.OK;
        }
        catch (Exception ex) when (ex is SqlException or InvalidOperationException)
        {
            ShowDatabaseError(this, ex);
        }
    }
}

/// <summary>Full-size, resizable picture viewer (Esc closes it).</summary>
internal sealed class ImageViewerForm : Form
{
    private readonly Image _copy;

    public ImageViewerForm(string title, Image image)
    {
        _copy = new Bitmap(image);

        Text = title;
        StartPosition = FormStartPosition.CenterParent;
        ShowInTaskbar = false;
        MinimizeBox = false;
        ClientSize = new Size(900, 700);
        BackColor = Color.FromArgb(24, 29, 38);
        KeyPreview = true;

        Controls.Add(new PictureBox { Dock = DockStyle.Fill, SizeMode = PictureBoxSizeMode.Zoom, Image = _copy });
        KeyDown += (_, e) => { if (e.KeyCode == Keys.Escape) Close(); };
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing) _copy.Dispose();
        base.Dispose(disposing);
    }
}

/// <summary>Shrinks picked picture files to JPEGs that are small enough for the database but still readable.</summary>
internal static class PetImages
{
    /// <summary>Reads a picture file, fixes phone-camera rotation, shrinks it to at most <paramref name="maxSide"/> pixels.</summary>
    public static byte[]? PrepareFromFile(string path, int maxSide, out string? error)
    {
        error = null;
        try
        {
            if (new FileInfo(path).Length > 25 * 1024 * 1024)
            {
                error = "That picture is too large (max 25 MB).";
                return null;
            }

            using var original = Image.FromFile(path);
            ApplyOrientation(original);

            float scale = Math.Min(1f, (float)maxSide / Math.Max(original.Width, original.Height));
            int w = Math.Max(1, (int)Math.Round(original.Width * scale));
            int h = Math.Max(1, (int)Math.Round(original.Height * scale));

            using var bmp = new Bitmap(w, h);
            using (var g = Graphics.FromImage(bmp))
            {
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                g.Clear(Color.White);
                g.DrawImage(original, 0, 0, w, h);
            }

            var codec = ImageCodecInfo.GetImageEncoders().First(c => c.FormatID == ImageFormat.Jpeg.Guid);
            using var parameters = new EncoderParameters(1);
            parameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 90L);

            using var ms = new MemoryStream();
            bmp.Save(ms, codec, parameters);
            return ms.ToArray();
        }
        catch (Exception ex) when (ex is OutOfMemoryException or ArgumentException or IOException
                                       or UnauthorizedAccessException or System.Runtime.InteropServices.ExternalException)
        {
            error = "That file couldn't be opened as a picture.";
            return null;
        }
    }

    private static void ApplyOrientation(Image image)
    {
        const int OrientationTag = 0x0112;
        if (!image.PropertyIdList.Contains(OrientationTag)) return;

        var item = image.GetPropertyItem(OrientationTag);
        if (item?.Value is not { Length: > 0 } value) return;

        switch (value[0])
        {
            case 3: image.RotateFlip(RotateFlipType.Rotate180FlipNone); break;
            case 6: image.RotateFlip(RotateFlipType.Rotate90FlipNone); break;
            case 8: image.RotateFlip(RotateFlipType.Rotate270FlipNone); break;
        }
    }
}
