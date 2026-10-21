using System.Drawing.Drawing2D;
using System.Globalization;
using Microsoft.Data.SqlClient;
using MyApp.Controls;

namespace MyApp;

/// <summary>
/// The "Pet Details" pop-up that opens from the Select a Pet step (Add New Pet, the "New Pet?" tile, or an existing pet).
/// Saving creates the pet (or updates the picked one); the caller then goes on to the vaccine-card step.
/// </summary>
internal sealed class NewPetDetailsDialog : Form
{
    private static readonly string[] DogBreeds =
    {
        "Aspin (Mixed)", "Shih Tzu", "Poodle", "Pomeranian", "Chihuahua", "Labrador Retriever", "Golden Retriever",
        "Siberian Husky", "Pug", "Beagle", "Corgi", "Dachshund", "Maltese", "Yorkshire Terrier", "German Shepherd", "Other",
    };

    private static readonly string[] CatBreeds =
    {
        "Puspin (Mixed)", "Persian", "Siamese", "Maine Coon", "British Shorthair", "Ragdoll", "Bengal", "Other",
    };

    private readonly int _customerId;
    private readonly PetProfile? _existing;

    // A pet whose vaccination card is on file got its details from the card scan, so the customer can't change them here
    // (a wrong read is fixed by scanning again). Only fields the card actually gave are locked; an empty one stays editable.
    private readonly bool _fromCard;
    private bool _lockName, _lockSpecies, _lockBreed, _lockGender, _lockAge, _lockWeight;

    private readonly FieldBox _name = new(250, 40);
    private readonly FieldBox _weight = new(250, 40) { Suffix = "kg" };
    private readonly FieldBox _notes = new(520, 64, multiline: true);
    private readonly PawSelect _breed = new() { Placeholder = "Select breed" };
    private readonly PawSelect _age = new() { Placeholder = "e.g. 2 years" };
    private readonly ChoiceTile _dog = new("Dog", radio: true);
    private readonly ChoiceTile _cat = new("Cat", radio: true);
    private readonly ChoiceTile _male = new("Male", radio: false);
    private readonly ChoiceTile _female = new("Female", radio: false);
    private readonly Label _error = new();

    public int PetId { get; private set; }
    public string PetName { get; private set; } = "";

    /// <param name="existing">The picked pet to show and update, or null to register a new one.</param>
    public NewPetDetailsDialog(int customerId, PetProfile? existing = null)
    {
        Icon = AppIcon.Value;
        _customerId = customerId;
        _existing = existing;
        _fromCard = existing?.VaccinationCard is { Length: > 0 };

        Font = Theme.Face(9f);
        BackColor = Color.White;
        ForeColor = Theme.Text;
        FormBorderStyle = FormBorderStyle.None;
        StartPosition = FormStartPosition.CenterParent;
        ShowInTaskbar = false;
        KeyPreview = true;
        ClientSize = new Size(580, 510);

        BuildHeader();

        Label Caption(string text, string? optional, int x, int y)
        {
            var l = new Label { AutoSize = true, Location = new Point(x, y), BackColor = Color.White };
            l.Font = Theme.Face(8.5f, FontStyle.Bold);
            l.ForeColor = Theme.Text;
            l.Text = text;
            Controls.Add(l);
            if (optional is not null)
            {
                var o = new Label { AutoSize = true, BackColor = Color.White, ForeColor = Theme.Muted, Font = Theme.Face(8f), Text = optional };
                Controls.Add(o);
                o.Location = new Point(x + TextRenderer.MeasureText(text, l.Font).Width - 2, y + 1);
            }
            return l;
        }

        const int x1 = 30, x2 = 300;

        // row 1: name + type
        Caption("Pet Name *", null, x1, 100);
        _name.Location = new Point(x1, 122);
        _name.Box.MaxLength = 60;
        Caption("Type of Pet *", null, x2, 100);
        _dog.SetBounds(x2, 122, 120, 44);
        _cat.SetBounds(x2 + 130, 122, 120, 44);

        // row 2: breed + weight
        Caption("Breed", "(optional)", x1, 182);
        _breed.SetBounds(x1, 204, 250, 40);
        Caption("Weight *", null, x2, 182);
        _weight.Location = new Point(x2, 204);
        _weight.Box.MaxLength = 6;
        _weight.Box.PlaceholderText = "e.g. 5";

        // row 3: age + gender
        Caption("Age", "(optional)", x1, 264);
        _age.SetBounds(x1, 286, 250, 40);
        _age.Items.AddRange(AgeLabels().Cast<object>().ToArray());
        Caption("Gender", "(optional)", x2, 264);
        _male.SetBounds(x2, 286, 120, 40);
        _female.SetBounds(x2 + 130, 286, 120, 40);

        // row 4: notes
        Caption("Additional Notes", "(optional)", x1, 346);
        _notes.Location = new Point(x1, 368);
        _notes.Box.MaxLength = 500;
        _notes.Box.PlaceholderText = "e.g. allergies, special instructions, or health concerns...";

        _error.SetBounds(x1, 440, 520, 20);
        _error.ForeColor = Theme.Danger;
        _error.BackColor = Color.White;
        _error.Font = Theme.Face(8.5f, FontStyle.Bold);

        var cancel = new PawButton { Text = "Cancel", Kind = PawButtonKind.Outline, Size = new Size(120, 40), Location = new Point(x1, 464) };
        var save = new PawButton { Text = "Save Pet", Kind = PawButtonKind.Primary, Size = new Size(140, 40), Location = new Point(410, 464) };
        cancel.Click += (_, _) => DialogResult = DialogResult.Cancel;
        save.Click += (_, _) => SavePet();
        CancelButton = cancel;

        Controls.AddRange(new Control[] { _name, _dog, _cat, _breed, _weight, _age, _male, _female, _notes, _error, cancel, save });

        // species / gender behave like groups
        _dog.Click += (_, _) => PickSpecies(dog: true);
        _cat.Click += (_, _) => PickSpecies(dog: false);
        _male.Click += (_, _) => { _male.Selected = !_male.Selected; if (_male.Selected) _female.Selected = false; };
        _female.Click += (_, _) => { _female.Selected = !_female.Selected; if (_female.Selected) _male.Selected = false; };
        _name.Box.TextChanged += (_, _) => _error.Text = "";
        _weight.Box.TextChanged += (_, _) => _error.Text = "";
        _weight.Box.KeyPress += (_, e) => { if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.' && e.KeyChar != ',') e.Handled = true; };

        if (existing is not null) Prefill(existing);
        if (_fromCard) LockCardDetails(existing!);
        Shown += (_, _) => { if (!_lockName) _name.Box.Focus(); };

        FullScreen.Apply(this, centerContent: true);   // full screen; the 580 x 510 form is scaled up and centred
    }

    // ------------------------------------------------------------------ chrome

    private void BuildHeader()
    {
        var close = new Label
        {
            Text = "\uE711",
            Font = Theme.GlyphFont(10f),
            ForeColor = Theme.Muted,
            BackColor = Color.White,
            TextAlign = ContentAlignment.MiddleCenter,
            Cursor = Cursors.Hand,
            Bounds = new Rectangle(534, 22, 28, 28),
        };
        close.Click += (_, _) => DialogResult = DialogResult.Cancel;

        var title = new Label
        {
            Text = "Pet Details",
            Font = Theme.Face(14f, FontStyle.Bold),
            ForeColor = Theme.Text,
            BackColor = Color.White,
            AutoSize = true,
            Location = new Point(88, 20),
        };
        var sub = new Label
        {
            Text = _existing is null ? "Please answer a few quick questions about your pet."
                 : _fromCard ? $"{_existing.Name}'s details were read from the vaccination card and can't be changed here."
                 : $"Check {_existing.Name}'s details before continuing.",
            Font = Theme.Face(8.5f),
            ForeColor = Theme.Muted,
            BackColor = Color.White,
            AutoSize = true,
            Location = new Point(88, 50),
        };
        var badge = new BadgeCircle { Bounds = new Rectangle(30, 16, 48, 48), Font = Theme.GlyphFont(16f) };
        Controls.Add(badge);
        Controls.Add(close);
        Controls.Add(title);
        Controls.Add(sub);
    }

    /// <summary>The round person icon in the header (a control, so it scales with the rest of the form).</summary>
    private sealed class BadgeCircle : Control
    {
        public BadgeCircle()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
            BackColor = Color.White;
            TabStop = false;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            var area = new Rectangle(0, 0, Width - 1, Height - 1);
            using (var b = new SolidBrush(Theme.PrimarySoft)) g.FillEllipse(b, area);
            TextRenderer.DrawText(g, "\uE77B", Font, area, Theme.Primary,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);
        }
    }

    // ------------------------------------------------------------------ behaviour

    private void PickSpecies(bool dog)
    {
        if (_dog.Selected == dog && _cat.Selected != dog) return;
        _dog.Selected = dog;
        _cat.Selected = !dog;
        FillBreeds(dog ? DogBreeds : CatBreeds, null);
        _error.Text = "";
    }

    private void FillBreeds(string[] list, string? keep)
    {
        _breed.Items.Clear();
        _breed.Items.AddRange(list.Cast<object>().ToArray());
        if (!string.IsNullOrWhiteSpace(keep))
        {
            int i = Array.FindIndex(list, b => string.Equals(b, keep, StringComparison.OrdinalIgnoreCase));
            if (i < 0) { _breed.Items.Add(keep); i = _breed.Items.Count - 1; }
            _breed.SelectedIndex = i;
        }
        else _breed.SelectedIndex = -1;
    }

    private void Prefill(PetProfile pet)
    {
        _name.Box.Text = pet.Name;
        bool cat = pet.Species.Contains("cat", StringComparison.OrdinalIgnoreCase);
        bool dog = pet.Species.Contains("dog", StringComparison.OrdinalIgnoreCase);
        if (cat || dog) { _dog.Selected = dog; _cat.Selected = cat; FillBreeds(cat ? CatBreeds : DogBreeds, pet.Breed); }
        if (pet.WeightKg is { } w) _weight.Box.Text = w.ToString("0.##", CultureInfo.CurrentCulture);
        if (pet.BirthDate is { } b) _age.SelectedIndex = AgeIndexFor(b);
        _male.Selected = pet.Gender == "Male";
        _female.Selected = pet.Gender == "Female";
        _notes.Box.Text = pet.Notes;
    }

    /// <summary>Makes the fields that came from the card scan read-only. Notes stay editable.</summary>
    private void LockCardDetails(PetProfile pet)
    {
        _lockName = true;
        _lockSpecies = _dog.Selected || _cat.Selected;
        _lockBreed = pet.Breed.Length > 0;
        _lockGender = pet.Gender.Length > 0;
        _lockAge = pet.BirthDate is not null;
        _lockWeight = pet.WeightKg is not null;

        _name.Box.ReadOnly = true;
        if (_lockSpecies) { _dog.Enabled = false; _cat.Enabled = false; }
        if (_lockBreed) _breed.Enabled = false;
        if (_lockGender) { _male.Enabled = false; _female.Enabled = false; }
        if (_lockAge) _age.Enabled = false;
        if (_lockWeight) _weight.Box.ReadOnly = true;
    }

    private static List<string> AgeLabels()
    {
        var list = new List<string> { "Under 1 year" };
        for (int y = 1; y <= 14; y++) list.Add(y == 1 ? "1 year" : y + " years");
        list.Add("15+ years");
        return list;
    }

    private static int AgeIndexFor(DateTime birth)
    {
        int years = DateTime.Today.Year - birth.Year;
        if (birth.Date > DateTime.Today.AddYears(-years)) years--;
        return Math.Clamp(years, 0, 15);
    }

    private static DateTime BirthFromAgeIndex(int index)
        => index == 0 ? DateTime.Today.AddMonths(-6) : DateTime.Today.AddYears(-index);

    private void SavePet()
    {
        string name = _name.Box.Text.Trim();
        if (name.Length == 0) { Fail("Enter your pet's name.", _name.Box); return; }
        if (!_lockName && !PetNameRules.IsValid(name)) { Fail(PetNameRules.Hint, _name.Box); return; }
        if (!_lockSpecies && !_dog.Selected && !_cat.Selected) { Fail("Choose Dog or Cat."); return; }

        decimal weight = _lockWeight ? _existing!.WeightKg!.Value : 0;
        if (!_lockWeight &&
            (!decimal.TryParse(_weight.Box.Text.Trim().Replace(',', '.'), NumberStyles.Number, CultureInfo.InvariantCulture, out weight)
             || weight <= 0 || weight > 200))
        {
            Fail("Enter your pet's weight in kg (for example 5).", _weight.Box);
            return;
        }

        // Locked fields keep exactly what the card scan saved (the age list is only approximate, so it must never overwrite a birthdate).
        string species = _lockSpecies ? _existing!.Species : _dog.Selected ? "Dog" : "Cat";
        string breed = _lockBreed ? _existing!.Breed : _breed.SelectedItem ?? "";
        string gender = _lockGender ? _existing!.Gender : _male.Selected ? "Male" : _female.Selected ? "Female" : "";
        DateTime? birth = _lockAge ? _existing!.BirthDate
            : _age.SelectedIndex >= 0 ? BirthFromAgeIndex(_age.SelectedIndex) : _existing?.BirthDate;

        try
        {
            if (_existing is null) PetId = EmployeeData.AddPet(_customerId, name, species, breed);
            else { PetId = _existing.Id; EmployeeData.UpdatePet(PetId, name, species, breed); }

            PetProfiles.SaveDetails(PetId, gender, birth, weight, _existing?.CoatColor ?? "", _notes.Box.Text.Trim());
            PetName = name;
            DialogResult = DialogResult.OK;
        }
        catch (Exception ex) when (ex is SqlException or InvalidOperationException)
        {
            _error.Text = "Couldn't save the pet. " + ex.Message;
        }
    }

    private void Fail(string message, Control? focus = null)
    {
        _error.Text = message;
        focus?.Focus();
    }

    // ------------------------------------------------------------------ small controls

    /// <summary>Rounded text box (single or multi line) with an optional unit such as "kg" on the right.</summary>
    private sealed class FieldBox : Control
    {
        public TextBox Box { get; }
        public string Suffix { get; init; } = "";

        public FieldBox(int width, int height, bool multiline = false)
        {
            Size = new Size(width, height);
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
            BackColor = Color.White;
            Box = new TextBox
            {
                BorderStyle = BorderStyle.None,
                Font = Theme.Face(9.5f),
                BackColor = Color.White,
                ForeColor = Theme.Text,
                Multiline = multiline,
                AcceptsReturn = multiline,
            };
            Controls.Add(Box);
            Box.GotFocus += (_, _) => Invalidate();
            Box.LostFocus += (_, _) => Invalidate();
            PlaceBox();
        }

        // The inner TextBox used to be positioned by a Layout handler that was attached too late to ever run,
        // leaving it at (0,0) at its default size. Position it from the overrides instead, so it is also
        // re-placed once Suffix (an init-only property set after construction) is known.
        protected override void OnLayout(LayoutEventArgs e) { base.OnLayout(e); PlaceBox(); }
        protected override void OnHandleCreated(EventArgs e) { base.OnHandleCreated(e); PlaceBox(); }

        private void PlaceBox()
        {
            if (Box is null) return; // OnLayout can fire from the constructor's Size assignment, before Box exists
            int right = Suffix.Length > 0 ? 40 : 14;
            if (Box.Multiline) Box.SetBounds(14, 10, Width - 14 - right, Height - 18);
            else Box.SetBounds(14, (Height - Box.PreferredHeight) / 2, Width - 14 - right, Box.PreferredHeight);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            using var path = Ui.RoundRect(new Rectangle(0, 0, Width - 1, Height - 1), 10);
            using var pen = new Pen(Box.Focused ? Theme.Primary : Theme.Border, Box.Focused ? 1.5f : 1f);
            g.DrawPath(pen, path);
            if (Suffix.Length > 0)
                TextRenderer.DrawText(g, Suffix, Theme.Face(8.5f), new Rectangle(Width - 40, 0, 30, Height), Theme.Muted,
                    TextFormatFlags.Right | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);
        }

        protected override void OnClick(EventArgs e) { Box.Focus(); base.OnClick(e); }
    }

    /// <summary>A selectable card (Dog / Cat, Male / Female, a service). Radio-style ones show a check circle at the right, vertically centered.</summary>
    private sealed class ChoiceTile : Control
    {
        private bool _selected;
        private bool _hover;
        private readonly bool _radio;

        public ChoiceTile(string text, bool radio)
        {
            Text = text;
            _radio = radio;
            Cursor = Cursors.Hand;
            Font = Theme.Face(9f, FontStyle.Bold);
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
            BackColor = Color.White;
        }

        public bool Selected { get => _selected; set { _selected = value; Invalidate(); } }

        protected override void OnMouseEnter(EventArgs e) { _hover = true; Invalidate(); base.OnMouseEnter(e); }
        protected override void OnMouseLeave(EventArgs e) { _hover = false; Invalidate(); base.OnMouseLeave(e); }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            var r = new Rectangle(0, 0, Width - 1, Height - 1);
            using (var path = Ui.RoundRect(r, 10))
            {
                using var fill = new SolidBrush(_selected ? Theme.PrimarySoft : _hover ? Theme.Subtle : Color.White);
                using var pen = new Pen(_selected ? Theme.Primary : Theme.Border, _selected ? 1.5f : 1f);
                g.FillPath(fill, path);
                g.DrawPath(pen, path);
            }

            // Radio tiles keep the right-hand strip free for the circle so the label never runs under it.
            const int dotSize = 16, dotMargin = 8;
            int textRight = _radio ? dotSize + dotMargin + 4 : 4;
            TextRenderer.DrawText(g, Text, Font, new Rectangle(4, 0, Width - 4 - textRight, Height), _selected ? Theme.Primary : Theme.TextSoft,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding | TextFormatFlags.EndEllipsis);

            if (!_radio) return;
            var dot = new Rectangle(Width - dotMargin - dotSize - 1, (Height - dotSize) / 2, dotSize, dotSize);
            if (_selected)
            {
                using var b = new SolidBrush(Theme.Primary);
                g.FillEllipse(b, dot);
                TextRenderer.DrawText(g, "\uE73E", Theme.GlyphFont(7f), dot, Color.White,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);
            }
            else
            {
                using var pen = new Pen(Color.FromArgb(197, 210, 228), 1.2f);
                g.DrawEllipse(pen, dot);
            }
        }
    }
}