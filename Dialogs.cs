using System.Drawing.Drawing2D;
using System.Globalization;
using Microsoft.Data.SqlClient;
using MyApp.Controls;

namespace MyApp;

/// <summary>
/// Shared look for the pop-up forms: icon + title header, rounded fields, soft button bar.
/// Subclasses add rows with <see cref="AddRow"/> and finish with <see cref="FitHeight"/>.
/// </summary>
internal abstract class DialogBase : Form
{
    private const int HeaderHeight = 72;
    private const int BarHeight = 64;
    private const int ErrorHeight = 34;

    protected readonly TableLayoutPanel Grid = new()
    {
        Dock = DockStyle.Top,
        AutoSize = true,
        ColumnCount = 2,
        Padding = new Padding(24, 18, 24, 4),
        BackColor = Color.White,
    };

    protected readonly PawButton SaveButton = new();
    protected readonly PawButton CancelButtonControl = new();
    private readonly Label _error = new();

    /// <summary>The bottom bar that holds Save and Cancel (buttons added later appear to their left).</summary>
    protected FlowLayoutPanel ButtonBar { get; private set; } = null!;

    /// <param name="glyph">Icon (Segoe MDL2 Assets) shown in the header badge.</param>
    /// <param name="subtitle">Small grey line under the title.</param>
    protected DialogBase(string title, string saveText, string glyph = "\uE77B", string subtitle = "")
    {
        Text = title;
        Font = Theme.Face(9f);
        BackColor = Color.White;
        ForeColor = Theme.Text;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        StartPosition = FormStartPosition.CenterParent;
        MaximizeBox = false;
        MinimizeBox = false;
        ShowInTaskbar = false;
        ShowIcon = false;
        ClientSize = new Size(460, 300);

        Grid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120));
        Grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

        // ---- header
        var header = new Panel { Dock = DockStyle.Top, Height = HeaderHeight, BackColor = Color.White };
        var badge = new IconBadge(glyph, Theme.PrimarySoft, Theme.Primary, Color.White, 40, radius: 10, glyphPt: 13f)
        {
            Location = new Point(24, 16),
        };
        bool hasSub = subtitle.Length > 0;
        var heading = Ui.Text(title, 12f, Theme.Text, Color.White, FontStyle.Bold);
        heading.SetBounds(78, hasSub ? 14 : 22, 340, 24);
        header.Controls.Add(badge);
        header.Controls.Add(heading);
        if (hasSub)
        {
            var sub = Ui.Text(subtitle, 8.5f, Theme.Muted, Color.White);
            sub.SetBounds(78, 38, 340, 18);
            header.Controls.Add(sub);
        }
        header.Paint += (_, e) =>
        {
            using var pen = new Pen(Theme.Border);
            e.Graphics.DrawLine(pen, 0, header.Height - 1, header.Width, header.Height - 1);
        };

        // ---- error strip (hidden until there is something to say)
        _error.AutoSize = false;
        _error.Dock = DockStyle.Top;
        _error.Height = ErrorHeight;
        _error.Font = Theme.Face(8.75f);
        _error.ForeColor = Color.FromArgb(185, 40, 40);
        _error.BackColor = Color.FromArgb(253, 238, 238);
        _error.TextAlign = ContentAlignment.MiddleLeft;
        _error.Padding = new Padding(24, 0, 24, 0);
        _error.Visible = false;

        // ---- buttons
        SaveButton.Text = saveText;
        SaveButton.Size = new Size(Math.Max(100, TextRenderer.MeasureText(saveText, SaveButton.Font).Width + 36), 36);
        SaveButton.Kind = PawButtonKind.Primary;
        SaveButton.Margin = new Padding(8, 0, 0, 0);
        SaveButton.Click += (_, _) => OnSaveClicked();

        CancelButtonControl.Text = "Cancel";
        CancelButtonControl.Size = new Size(92, 36);
        CancelButtonControl.Kind = PawButtonKind.Outline;
        CancelButtonControl.Margin = new Padding(8, 0, 0, 0);
        CancelButtonControl.DialogResult = DialogResult.Cancel;

        var buttons = new FlowLayoutPanel
        {
            Dock = DockStyle.Bottom,
            FlowDirection = FlowDirection.RightToLeft,
            Height = BarHeight,
            Padding = new Padding(16, 14, 16, 10),
            BackColor = Theme.Subtle,
        };
        buttons.Paint += (_, e) =>
        {
            using var pen = new Pen(Theme.Border);
            e.Graphics.DrawLine(pen, 0, 0, buttons.Width, 0);
        };
        buttons.Controls.Add(SaveButton);
        buttons.Controls.Add(CancelButtonControl);
        ButtonBar = buttons;

        // Docking order: the control added last is placed first (outermost).
        Controls.Add(_error);
        Controls.Add(Grid);
        Controls.Add(buttons);
        Controls.Add(header);
        AcceptButton = SaveButton;
        CancelButton = CancelButtonControl;
    }

    /// <summary>
    /// Adds a "label | input" row. Text boxes and number boxes are put in a rounded frame automatically;
    /// <see cref="PawSelect"/>, <see cref="PawDatePicker"/> and <see cref="PawTimePicker"/> draw their own.
    /// </summary>
    protected void AddRow(string label, Control input)
    {
        var l = new Label
        {
            Text = label,
            AutoSize = false,
            Height = 36,
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleLeft,
            ForeColor = Theme.TextSoft,
            Margin = new Padding(0, 4, 0, 4),
            UseMnemonic = false,
        };

        Control cell = input;
        if (input is TextBox or NumericUpDown)
            cell = FieldFrame.ForEditor(input);
        else if (input is Label value)
        {
            value.Height = Math.Max(value.Height, 36);
            if (value.ForeColor == SystemColors.ControlText) value.ForeColor = Theme.Text;
        }

        // Tall rows (notes) line their caption up with the first line instead of the middle.
        if (cell.Height > 60) l.TextAlign = ContentAlignment.TopLeft;
        if (cell.Height > 60) l.Padding = new Padding(0, 9, 0, 0);

        cell.Dock = DockStyle.Fill;
        cell.Margin = new Padding(0, 4, 0, 4);
        Grid.Controls.Add(l);
        Grid.Controls.Add(cell);
    }

    protected void FitHeight()
    {
        Grid.PerformLayout();
        ClientSize = new Size(ClientSize.Width, HeaderHeight + Grid.PreferredSize.Height + 8 + ErrorHeight + BarHeight);
    }

    protected void ShowError(string message)
    {
        _error.Text = message;
        _error.Visible = message.Length > 0;
    }

    protected abstract void OnSaveClicked();

    protected static void ShowDatabaseError(IWin32Window owner, Exception ex)
        => Msg.Show(owner, "The database could not be updated.\n\n" + ex.Message, "Pawfect",
            MessageBoxButtons.OK, MessageBoxIcon.Error);
}

/// <summary>Adds a customer (and optionally their first pet), or edits an existing customer.</summary>
internal sealed class CustomerForm : DialogBase
{
    private readonly CustomerRecord? _existing;
    private readonly TextBox _name = new();
    private readonly TextBox _phone = new();
    private readonly TextBox _email = new();
    private readonly TextBox _address = new();
    private readonly TextBox _petName = new();
    private readonly TextBox _petSpecies = new();
    private readonly TextBox _petBreed = new();

    public int? CreatedCustomerId { get; private set; }

    public CustomerForm(CustomerRecord? existing = null)
        : base(existing is null ? "Add Customer" : "Edit Customer", "Save", "\uE77B",
              existing is null ? "Enter the owner's details (and a first pet, if you like)" : "Update the owner's details")
    {
        _existing = existing;

        AddRow("Full name *", _name);
        AddRow("Phone", _phone);
        AddRow("Email", _email);
        AddRow("Address", _address);

        if (existing is null)
        {
            AddRow("First pet", _petName);
            AddRow("Species", _petSpecies);
            AddRow("Breed", _petBreed);
            _petName.PlaceholderText = "Optional";
            _petSpecies.PlaceholderText = "e.g. Dog, Cat";
        }
        else
        {
            _name.Text = existing.Name;
            _phone.Text = existing.Phone;
            _email.Text = existing.Email;
            _address.Text = existing.Address;
        }
        FitHeight();
    }

    protected override void OnSaveClicked()
    {
        if (string.IsNullOrWhiteSpace(_name.Text)) { ShowError("Enter the customer's name."); return; }
        if (!string.IsNullOrWhiteSpace(_email.Text) && !_email.Text.Contains('@')) { ShowError("That email doesn't look right."); return; }

        try
        {
            if (_existing is null)
            {
                CreatedCustomerId = EmployeeData.AddCustomer(_name.Text, _phone.Text, _email.Text, _address.Text,
                    _petName.Text, _petSpecies.Text, _petBreed.Text);
            }
            else
            {
                EmployeeData.UpdateCustomer(_existing.Id, _name.Text, _phone.Text, _email.Text, _address.Text);
                CreatedCustomerId = _existing.Id;
            }
            DialogResult = DialogResult.OK;
        }
        catch (Exception ex) when (ex is SqlException or InvalidOperationException)
        {
            ShowDatabaseError(this, ex);
        }
    }
}

/// <summary>Adds a pet to an existing customer, or edits an existing pet.</summary>
internal sealed class PetForm : DialogBase
{
    private readonly int _customerId;
    private readonly PetRecord? _existing;
    private readonly TextBox _name = new();
    private readonly TextBox _species = new();
    private readonly TextBox _breed = new();

    public int? CreatedPetId { get; private set; }

    public PetForm(int customerId, string ownerName, PetRecord? existing = null)
        : base(existing is null ? "Add Pet" : "Edit Pet", "Save", "\uE77B", "Owner: " + ownerName)
    {
        _customerId = customerId;
        _existing = existing;
        AddRow("Owner", new Label { Text = ownerName, TextAlign = ContentAlignment.MiddleLeft, AutoSize = false, Height = 28 });
        AddRow("Pet name *", _name);
        AddRow("Species", _species);
        AddRow("Breed", _breed);
        _species.PlaceholderText = "e.g. Dog, Cat";

        if (existing is not null)
        {
            _name.Text = existing.Name;
            _species.Text = existing.Species;
            _breed.Text = existing.Breed;
        }
        FitHeight();
    }

    protected override void OnSaveClicked()
    {
        if (string.IsNullOrWhiteSpace(_name.Text)) { ShowError("Enter the pet's name."); return; }
        try
        {
            if (_existing is null)
            {
                CreatedPetId = EmployeeData.AddPet(_customerId, _name.Text, _species.Text, _breed.Text);
            }
            else
            {
                EmployeeData.UpdatePet(_existing.Id, _name.Text, _species.Text, _breed.Text);
                CreatedPetId = _existing.Id;
            }
            DialogResult = DialogResult.OK;
        }
        catch (Exception ex) when (ex is SqlException or InvalidOperationException)
        {
            ShowDatabaseError(this, ex);
        }
    }
}

/// <summary>
/// Edits an open appointment. For a customer's request (Pending) this is where the employee sets the schedule:
/// day, time, groomer and room, then confirms it.
/// </summary>
internal sealed class EditAppointmentForm : DialogBase
{
    private const string NotAssigned = "\u2014 Not assigned \u2014";

    private readonly AppointmentItem _item;
    private readonly IReadOnlyList<DayOfWeek> _preferred = Array.Empty<DayOfWeek>();   // the customer's preferred days (empty = any day)
    private readonly List<PetRecord> _pets;
    private readonly List<ServiceOffering> _services;
    private readonly List<GroomerOption> _groomers;
    private readonly List<RoomOption> _rooms;
    private readonly PawSelect _pet = new() { Placeholder = "Choose a pet" };
    private readonly PawSelect _service = new() { Placeholder = "Choose a service" };
    private readonly PawSelect _groomer = new();
    private readonly PawSelect _room = new();
    private readonly PawDatePicker _date = new();
    private readonly PawTimePicker _time = new();
    private readonly PawCheckBox _confirm = new() { Text = "Confirm this appointment", Checked = true };
    private readonly PetPictureBox _petPhoto = new("Pet photo", "No photo", new Size(96, 96));
    private readonly PetPictureBox _petCard = new("Vaccination card", "No vaccination card on file", new Size(180, 120));

    public EditAppointmentForm(AppointmentItem item)
        : base(item.Status is AppointmentStatus.Pending or AppointmentStatus.Rescheduled ? "Schedule Appointment" : "Edit Appointment", "Save", "\uE787",
              item.Status is AppointmentStatus.Pending or AppointmentStatus.Rescheduled
                  ? "Set the day, time, groomer and room"
                  : "Change the booking details")
    {
        _item = item;
        var refs = EmployeeData.GetAppointmentRefs(item.Id)
                   ?? throw new InvalidOperationException("That appointment no longer exists.");

        _pets = EmployeeData.GetPets().Where(p => p.CustomerId == refs.CustomerId).ToList();
        _services = EmployeeData.GetServices();
        _groomers = EmployeeData.GetGroomers();
        _rooms = EmployeeData.GetRooms();

        _pet.Items.AddRange(_pets.Select(p => (object)(string.IsNullOrWhiteSpace(p.Breed) ? p.Name : $"{p.Name}  ·  {p.Breed}")).ToArray());
        _service.Items.AddRange(_services.Select(s => (object)$"{s.Name}  ·  {Money.Format(s.Price)}").ToArray());
        _pet.SelectedIndex = _pets.FindIndex(p => p.Id == refs.PetId);
        _service.SelectedIndex = _services.FindIndex(s => s.Id == refs.ServiceId);

        _groomer.Items.Add(NotAssigned);
        _groomer.Items.AddRange(_groomers.Select(g => (object)g.Name).ToArray());
        _groomer.SelectedIndex = refs.GroomerId is { } gid ? Math.Max(0, _groomers.FindIndex(g => g.Id == gid) + 1) : 0;

        _room.Items.Add(NotAssigned);
        _room.Items.AddRange(_rooms.Select(r => (object)r.Name).ToArray());
        _room.SelectedIndex = refs.RoomId is { } rid ? Math.Max(0, _rooms.FindIndex(r => r.Id == rid) + 1) : 0;

        AddRow("Customer", new Label { Text = item.CustomerName, TextAlign = ContentAlignment.MiddleLeft, AutoSize = false, Height = 28 });

        // What the customer asked for, so the employee can decide the real schedule.
        bool isRequest = item.Status == AppointmentStatus.Pending;
        _preferred = item.PreferredDayList;
        if (_preferred.Count > 0)
        {
            string days = PreferredDaysText.Readable(_preferred, full: true);
            AddRow("Preferred days", new Label
            {
                Text = days,
                TextAlign = ContentAlignment.MiddleLeft,
                AutoSize = false,
                Height = days.Length > 34 ? 46 : 28,
                ForeColor = Theme.Primary,
            });
        }
        else if (isRequest)
        {
            string asked = $"Preferred: {item.Start.ToString("ddd, MMM d  h:mm tt", CultureInfo.CurrentCulture)}";
            AddRow("Request", new Label { Text = asked, TextAlign = ContentAlignment.MiddleLeft, AutoSize = false, Height = 28, ForeColor = Theme.TextSoft });
        }
        if (!string.IsNullOrWhiteSpace(item.Notes))
            AddRow("Notes", new Label { Text = item.Notes, TextAlign = ContentAlignment.TopLeft, AutoSize = false, Height = 46, ForeColor = Theme.TextSoft });

        // The employee only sets the schedule: the pet and service are what the customer asked for, shown but not editable.
        AddRow("Pet", new Label { Text = _pet.SelectedItem ?? "\u2014", TextAlign = ContentAlignment.MiddleLeft, AutoSize = false, Height = 28 });
        var pics = new FlowLayoutPanel { Height = 128, AutoSize = false, BackColor = Color.White, WrapContents = false };
        _petPhoto.Margin = new Padding(0, 4, 12, 4);
        _petCard.Margin = new Padding(0, 4, 0, 4);
        pics.Controls.Add(_petPhoto);
        pics.Controls.Add(_petCard);
        AddRow("Photo & card", pics);
        LoadPetPictures();
        AddRow("Service", new Label { Text = _service.SelectedItem ?? "\u2014", TextAlign = ContentAlignment.MiddleLeft, AutoSize = false, Height = 28 });
        AddRow("Date *", _date);
        AddRow("Time *", _time);
        AddRow("Groomer", _groomer);
        AddRow("Room", _room);

        bool canConfirm = item.Status is AppointmentStatus.Pending or AppointmentStatus.Rescheduled;
        if (canConfirm) AddRow("", _confirm);

        var original = Trim(item.Start);
        bool restricted = _preferred.Count > 0;
        // An overdue appointment keeps its own (past) date until a new one is picked, unless the customer's preferred days limit the choice.
        _date.MinDate = !restricted && original.Date < DateTime.Today ? original.Date : DateTime.Today;
        // Only the customer's preferred weekdays can be picked; the others are greyed out in the calendar.
        _date.AllowedDays = restricted ? _preferred.ToList() : null;
        // A request that has no real date yet starts on the next day the customer prefers.
        _date.Value = restricted && (original.Date < DateTime.Today || !_preferred.Contains(original.DayOfWeek))
            ? PreferredDaysText.NextOnOrAfter(DateTime.Today, _preferred)
            : original.Date;
        _time.Value = new TimeSpan(original.Hour, original.Minute, 0);
        FitHeight();

        Shown += (_, _) =>
        {
            if (_pet.SelectedIndex < 0 || _service.SelectedIndex < 0)
                ShowError("The pet or service on this request is no longer available. Ask the customer to book again.");
            else if (_groomers.Count == 0 || _rooms.Count == 0)
                ShowError("No groomers or rooms exist yet. Run Sql/appointment_scheduling.sql.");
        };
    }

    private static DateTime Trim(DateTime d) => new(d.Year, d.Month, d.Day, d.Hour, d.Minute, 0);

    private void LoadPetPictures()
    {
        byte[]? photo = null, card = null;
        if (_pet.SelectedIndex >= 0 && _pet.SelectedIndex < _pets.Count)
        {
            try { (photo, card) = PetProfiles.GetPictures(_pets[_pet.SelectedIndex].Id); }
            catch (Exception ex) when (ex is SqlException or InvalidOperationException) { }
        }
        _petPhoto.SetBytes(photo);
        _petCard.SetBytes(card);
    }

    protected override void OnSaveClicked()
    {
        if (_pet.SelectedIndex < 0 || _service.SelectedIndex < 0)
        {
            ShowError("The pet or service on this request is no longer available. Ask the customer to book again.");
            return;
        }

        var start = _date.Value.Date + _time.Value;
        if (_preferred.Count > 0 && !_preferred.Contains(start.DayOfWeek))
        {
            ShowError($"The customer can only come on {PreferredDaysText.Readable(_preferred, full: true)}. " +
                      $"{start:dddd} isn't one of their preferred days - pick a date on one of those days.");
            return;
        }
        if (start != Trim(_item.Start) && start < DateTime.Now.AddMinutes(-1)) { ShowError("Pick a time that hasn't passed yet."); return; }

        int? groomerId = _groomer.SelectedIndex > 0 ? _groomers[_groomer.SelectedIndex - 1].Id : null;
        int? roomId = _room.SelectedIndex > 0 ? _rooms[_room.SelectedIndex - 1].Id : null;
        bool confirm = _confirm.Parent is not null && _confirm.Checked;

        if (confirm && (groomerId is null || roomId is null))
        {
            ShowError("Choose a groomer and a room to confirm, or untick Confirm.");
            return;
        }

        try
        {
            if (EmployeeData.FindScheduleConflict(_item.Id, start, groomerId, roomId) is { } conflict)
            {
                ShowError(conflict);
                return;
            }

            EmployeeData.UpdateAppointment(_item.Id, _pets[_pet.SelectedIndex].Id, _services[_service.SelectedIndex].Id,
                start, groomerId, roomId, confirm);
            DialogResult = DialogResult.OK;
        }
        catch (Exception ex) when (ex is SqlException or InvalidOperationException)
        {
            ShowDatabaseError(this, ex);
        }
    }
}

/// <summary>Read-only list of the services an admin has set up (employees can't change prices).</summary>
internal sealed class ServiceListForm : DialogBase
{
    public ServiceListForm() : base("Services", "Close")
    {
        var box = new TextBox
        {
            Multiline = true,
            ReadOnly = true,
            Height = 240,
            ScrollBars = ScrollBars.Vertical,
            BackColor = Color.White,
        };
        var services = EmployeeData.GetServices();
        box.Text = services.Count == 0
            ? "No services exist yet. An admin needs to add them."
            : string.Join(Environment.NewLine, services.Select(s => $"{s.Name}  -  {Money.Format(s.Price)}"));
        AddRow("Services", box);
        SaveButton.Visible = false;
        CancelButtonControl.Text = "Close";
        FitHeight();
    }

    protected override void OnSaveClicked() => DialogResult = DialogResult.OK;
}

/// <summary>A big month grid. Each day lists its appointments (time + pet), coloured by status.</summary>
internal sealed class FullCalendarGrid : Control
{
    private const int HeadHeight = 28;
    private const int LineHeight = 16;

    private const TextFormatFlags Center = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter |
        TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine;
    private const TextFormatFlags LeftLine = TextFormatFlags.Left | TextFormatFlags.VerticalCenter |
        TextFormatFlags.EndEllipsis | TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine;

    private IReadOnlyList<AppointmentItem> _items = Array.Empty<AppointmentItem>();
    private DateTime _month = new(DateTime.Today.Year, DateTime.Today.Month, 1);
    private DateTime? _selected;

    public event EventHandler<DateTime>? DaySelected;
    public event EventHandler<DateTime>? DayActivated;

    public FullCalendarGrid()
    {
        SetStyle(ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
        BackColor = Color.White;
        Font = Theme.Face(8.5f);
        Cursor = Cursors.Hand;
    }

    public void SetItems(IReadOnlyList<AppointmentItem> items) { _items = items; Invalidate(); }

    public DateTime Month
    {
        get => _month;
        set { _month = new DateTime(value.Year, value.Month, 1); Invalidate(); }
    }

    public DateTime? SelectedDate
    {
        get => _selected;
        set { _selected = value?.Date; Invalidate(); }
    }

    private int Offset => (int)_month.DayOfWeek;
    private int DaysInMonth => DateTime.DaysInMonth(_month.Year, _month.Month);
    private int Weeks => (Offset + DaysInMonth + 6) / 7;

    private Rectangle CellRect(int index)
    {
        float cw = Width / 7f;
        float ch = (Height - HeadHeight) / (float)Weeks;
        int col = index % 7, row = index / 7;
        return new Rectangle((int)(col * cw), HeadHeight + (int)(row * ch), (int)Math.Ceiling(cw), (int)Math.Ceiling(ch));
    }

    private DateTime? DayAt(Point p)
    {
        if (p.Y < HeadHeight || Width <= 0) return null;
        int col = Math.Min(6, (int)(p.X / (Width / 7f)));
        int row = (int)((p.Y - HeadHeight) / ((Height - HeadHeight) / (float)Weeks));
        int day = row * 7 + col - Offset + 1;
        return day >= 1 && day <= DaysInMonth ? new DateTime(_month.Year, _month.Month, day) : null;
    }

    protected override void OnMouseClick(MouseEventArgs e)
    {
        base.OnMouseClick(e);
        if (e.Button != MouseButtons.Left || DayAt(e.Location) is not { } day) return;
        _selected = day;
        Invalidate();
        DaySelected?.Invoke(this, day);
    }

    protected override void OnMouseDoubleClick(MouseEventArgs e)
    {
        base.OnMouseDoubleClick(e);
        if (e.Button == MouseButtons.Left && DayAt(e.Location) is { } day)
            DayActivated?.Invoke(this, day);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.Clear(Color.White);

        string[] names = { "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat" };
        float cw = Width / 7f;
        for (int c = 0; c < 7; c++)
            TextRenderer.DrawText(g, names[c], Theme.Face(8.5f, FontStyle.Bold),
                new Rectangle((int)(c * cw), 0, (int)cw, HeadHeight), Theme.TextSoft, Center);

        var byDay = _items
            .Where(a => a.Start.Year == _month.Year && a.Start.Month == _month.Month)
            .GroupBy(a => a.Start.Day)
            .ToDictionary(x => x.Key, x => x.OrderBy(a => a.Start).ToList());

        using var border = new Pen(Theme.Border);
        for (int i = 0; i < Weeks * 7; i++)
        {
            var rect = CellRect(i);
            int day = i - Offset + 1;
            bool inMonth = day >= 1 && day <= DaysInMonth;
            var date = inMonth ? new DateTime(_month.Year, _month.Month, day) : DateTime.MinValue;
            bool selected = inMonth && _selected == date;

            using (var fill = new SolidBrush(!inMonth ? Theme.Subtle : selected ? Theme.PrimarySoft : Color.White))
                g.FillRectangle(fill, rect);
            g.DrawRectangle(border, rect.X, rect.Y, rect.Width - 1, rect.Height - 1);
            if (!inMonth) continue;

            if (selected)
            {
                using var pen = new Pen(Theme.Primary, 2f);
                g.DrawRectangle(pen, rect.X + 1, rect.Y + 1, rect.Width - 3, rect.Height - 3);
            }

            bool today = date == DateTime.Today;
            var num = new Rectangle(rect.X + 6, rect.Y + 5, 22, 22);
            if (today)
            {
                using var tb = new SolidBrush(Theme.Primary);
                g.FillEllipse(tb, num);
            }
            TextRenderer.DrawText(g, day.ToString(CultureInfo.CurrentCulture), Theme.Face(8.5f, FontStyle.Bold), num,
                today ? Color.White : Theme.Text, Center);

            if (!byDay.TryGetValue(day, out var list)) continue;

            int top = rect.Y + 31;
            int maxLines = Math.Max(0, (rect.Bottom - top - 2) / LineHeight);
            int shown = list.Count <= maxLines ? list.Count : Math.Max(0, maxLines - 1);

            for (int k = 0; k < shown; k++)
            {
                var a = list[k];
                int y = top + k * LineHeight;
                using (var dot = new SolidBrush(AppointmentStatusStyle.Marker(a.Status)))
                    g.FillEllipse(dot, rect.X + 8, y + 5, 7, 7);
                TextRenderer.DrawText(g, $"{a.Start.ToString("h:mm tt", CultureInfo.CurrentCulture)}  {a.PetName}",
                    Theme.Face(7.5f), new Rectangle(rect.X + 19, y, rect.Width - 23, LineHeight), Theme.TextSoft, LeftLine);
            }
            if (shown < list.Count && maxLines > 0)
                TextRenderer.DrawText(g, $"+{list.Count - shown} more", Theme.Face(7.5f, FontStyle.Bold),
                    new Rectangle(rect.X + 8, top + shown * LineHeight, rect.Width - 12, LineHeight), Theme.Primary, LeftLine);
        }
    }
}

/// <summary>Full-size month calendar of every appointment. Pick a day to jump to it in the Appointments list.</summary>
internal sealed class FullCalendarForm : Form
{
    private readonly FullCalendarGrid _grid = new() { Dock = DockStyle.Fill };
    private readonly Label _title = new();
    private readonly Button _show = new();

    /// <summary>The day chosen with "Show in list" (or a double-click), or null if the window was just closed.</summary>
    public DateTime? PickedDate { get; private set; }

    public FullCalendarForm(IReadOnlyList<AppointmentItem> items, DateTime? startDay = null)
    {
        Text = "Appointment Calendar";
        Font = Theme.Face(9f);
        BackColor = Color.White;
        ForeColor = Theme.Text;
        StartPosition = FormStartPosition.CenterParent;
        ClientSize = new Size(1000, 660);
        MinimumSize = new Size(760, 520);
        ShowInTaskbar = false;

        _grid.SetItems(items);
        if (startDay is { } start)
        {
            _grid.Month = start;
            _grid.SelectedDate = start.Date;
        }

        Button Nav(string text, int width)
        {
            var b = new Button
            {
                Text = text,
                Size = new Size(width, 32),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.White,
                ForeColor = Theme.Text,
                Cursor = Cursors.Hand,
                Margin = new Padding(0, 0, 6, 0),
            };
            b.FlatAppearance.BorderColor = Theme.Border;
            return b;
        }

        var prev = Nav("<", 40);
        var next = Nav(">", 40);
        var todayButton = Nav("Today", 70);
        _title.AutoSize = false;
        _title.Size = new Size(220, 32);
        _title.TextAlign = ContentAlignment.MiddleCenter;
        _title.Font = Theme.Face(11f, FontStyle.Bold);
        _title.Margin = new Padding(0, 0, 6, 0);

        var top = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 52, Padding = new Padding(12, 10, 12, 6) };
        top.Controls.Add(prev);
        top.Controls.Add(_title);
        top.Controls.Add(next);
        top.Controls.Add(todayButton);

        _show.Text = "Show in list";
        _show.Size = new Size(110, 32);
        _show.FlatStyle = FlatStyle.Flat;
        _show.FlatAppearance.BorderSize = 0;
        _show.BackColor = Theme.Primary;
        _show.ForeColor = Color.White;
        _show.Cursor = Cursors.Hand;
        _show.Enabled = startDay is not null;

        var close = new Button
        {
            Text = "Close",
            Size = new Size(90, 32),
            FlatStyle = FlatStyle.Flat,
            BackColor = Color.White,
            ForeColor = Theme.TextSoft,
            DialogResult = DialogResult.Cancel,
            Cursor = Cursors.Hand,
        };
        close.FlatAppearance.BorderColor = Theme.Border;

        var hint = new Label
        {
            Text = "Click a day to select it. Double-click to show it in the Appointments list.",
            AutoSize = false,
            Width = 480,
            Height = 32,
            TextAlign = ContentAlignment.MiddleLeft,
            ForeColor = Theme.TextSoft,
        };

        var bottom = new FlowLayoutPanel
        {
            Dock = DockStyle.Bottom,
            Height = 52,
            FlowDirection = FlowDirection.RightToLeft,
            Padding = new Padding(12, 10, 12, 6),
            BackColor = Theme.Subtle,
        };
        bottom.Controls.Add(close);
        bottom.Controls.Add(_show);
        bottom.Controls.Add(hint);

        Controls.Add(_grid);
        Controls.Add(top);
        Controls.Add(bottom);
        CancelButton = close;

        void ShowMonth() => _title.Text = _grid.Month.ToString("MMMM yyyy", CultureInfo.CurrentCulture);
        ShowMonth();

        prev.Click += (_, _) => { _grid.Month = _grid.Month.AddMonths(-1); ShowMonth(); };
        next.Click += (_, _) => { _grid.Month = _grid.Month.AddMonths(1); ShowMonth(); };
        todayButton.Click += (_, _) => { _grid.Month = DateTime.Today; _grid.SelectedDate = DateTime.Today; _show.Enabled = true; ShowMonth(); };

        _grid.DaySelected += (_, _) => _show.Enabled = true;
        _grid.DayActivated += (_, day) => Pick(day);
        _show.Click += (_, _) => { if (_grid.SelectedDate is { } d) Pick(d); };
    }

    private void Pick(DateTime day)
    {
        PickedDate = day;
        DialogResult = DialogResult.OK;
    }
}