using Microsoft.Data.SqlClient;
using MyApp.Controls;

namespace MyApp;

/// <summary>
/// Staff books an appointment for a customer (walk-in or phone call): customer, pet, service, day and time, groomer and room.
/// With "Confirm" ticked (and a groomer and room chosen) it is saved as Confirmed, otherwise as Pending.
/// </summary>
internal sealed class NewAppointmentForm : DialogBase
{
    private const string NotAssigned = "\u2014 Not assigned \u2014";

    // Layout (in 96-dpi pixels; the form scales them with the screen).
    private const int DialogWidth = 580;
    private const int CaptionHeight = 18;
    private const int FieldHeight = 36;
    private const int RowGap = 8;

    private readonly string _staffEmail;
    private readonly List<CustomerRecord> _customers;
    private readonly List<PetRecord> _allPets;
    private List<ServiceOffering> _services = new();   // the services that fit the chosen pet (its weight band)
    private bool _noServicesAtAll;
    private readonly List<GroomerOption> _groomers;
    private readonly List<RoomOption> _rooms;
    private List<PetRecord> _pets = new();

    private readonly PawSelect _customer = new() { Placeholder = "Choose a customer", Searchable = true, SearchPlaceholder = "Search customers\u2026", EmptyText = "No customers to choose from" };
    private readonly PawSelect _pet = new() { Placeholder = "Choose a pet", Searchable = true, SearchPlaceholder = "Search pets\u2026", EmptyText = "No pets to choose from" };
    private readonly PawSelect _service = new() { Placeholder = "Choose a service", Searchable = true, SearchPlaceholder = "Search services\u2026", EmptyText = "No services for this pet" };
    private readonly PawSelect _groomer = new();
    private readonly PawSelect _room = new();
    private readonly PawDatePicker _date = new();
    private readonly PawTimePicker _time = new();
    private readonly TextBox _notes = new() { PlaceholderText = "Optional notes", MaxLength = 300 };
    private readonly PawCheckBox _confirm = new() { Text = "Confirm this appointment", Checked = true };
    private readonly PawCheckBox _walkIn = new() { Text = "Walk-in (customer is here now)" };

    public int? CreatedAppointmentId { get; private set; }

    public NewAppointmentForm(string staffEmail, int? customerId = null)
        : base("New Appointment", "Save", "\uE787", "Book a customer in (walk-in or phone call)")
    {
        _staffEmail = staffEmail;
        ClientSize = new Size(DialogWidth, ClientSize.Height);
        _customers = EmployeeData.GetCustomers().OrderBy(c => c.Name).ToList();
        _allPets = EmployeeData.GetPets();
        _noServicesAtAll = EmployeeData.GetServices().Count == 0;
        _groomers = EmployeeData.GetGroomers();
        _rooms = EmployeeData.GetRooms();

        _customer.Items.AddRange(_customers.Select(c => (object)(string.IsNullOrWhiteSpace(c.Phone) ? c.Name : $"{c.Name}  \u00b7  {c.Phone}")).ToArray());
        _groomer.Items.Add(NotAssigned);
        _groomer.Items.AddRange(_groomers.Select(g => (object)g.Name).ToArray());
        _groomer.SelectedIndex = 0;
        _room.Items.Add(NotAssigned);
        _room.Items.AddRange(_rooms.Select(r => (object)r.Name).ToArray());
        _room.SelectedIndex = 0;

        BuildLayout();

        _date.MinDate = DateTime.Today;
        _date.Value = DateTime.Today;
        var next = DateTime.Now.AddHours(1);
        _time.Value = new TimeSpan(Math.Clamp(next.Hour, 6, 22), 0, 0);

        _customer.SelectedIndexChanged += (_, _) => LoadPets();
        _pet.SelectedIndexChanged += (_, _) => LoadServices();   // the price depends on the pet's weight
        if (customerId is int id)
            _customer.SelectedIndex = _customers.FindIndex(c => c.Id == id);

        FitHeight();

        // Scale with the screen like the main window does, so the dialog looks the same at 100% (1920 x 1080)
        // and 125% (1536 x 864) instead of staying small while the text grows.
        AutoScaleDimensions = new SizeF(96F, 96F);
        AutoScaleMode = AutoScaleMode.Dpi;

        Shown += (_, _) =>
        {
            if (_customers.Count == 0) ShowError("There are no customers yet. Add a customer first.");
            else if (_noServicesAtAll) ShowError("There are no services yet. Ask an admin to add them to the catalog.");
            else if (_groomers.Count == 0 || _rooms.Count == 0) ShowError("No groomers or rooms exist yet. Run Sql/appointment_scheduling.sql.");
        };
    }

    /// <summary>
    /// Captions sit above their fields so every field gets the full width (long service names fit),
    /// related fields share a row (date + time, groomer + room) and the dialog stays short.
    /// </summary>
    private void BuildLayout()
    {
        Grid.ColumnCount = 1;
        Grid.ColumnStyles.Clear();
        Grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        Grid.Padding = new Padding(28, 16, 28, 0);

        AddFull(Labeled("Customer", _customer, required: true));
        AddFull(Labeled("Pet", _pet, required: true));
        AddFull(Labeled("Service", _service, required: true));
        AddFull(TwoUp(Labeled("Date", _date, required: true), Labeled("Time", _time, required: true)));
        AddFull(TwoUp(Labeled("Groomer", _groomer), Labeled("Room", _room)));
        AddFull(Labeled("Notes", _notes));

        _confirm.Dock = DockStyle.Fill;
        var hint = new Label
        {
            Text = "Confirming needs a groomer and a room",
            AutoSize = false,
            Dock = DockStyle.Right,
            Width = 230,
            TextAlign = ContentAlignment.MiddleRight,
            ForeColor = Theme.Muted,
            Font = Theme.Face(8f),
            BackColor = Color.White,
        };
        var confirmRow = new Panel { Height = 30, Margin = new Padding(0, 2, 0, 0), BackColor = Color.White };
        confirmRow.Controls.Add(_confirm);   // fills what is left
        confirmRow.Controls.Add(hint);       // docked to the right first
        AddFull(confirmRow);

        _walkIn.Dock = DockStyle.Fill;
        var walkInRow = new Panel { Height = 30, Margin = new Padding(0, 2, 0, 0), BackColor = Color.White };
        walkInRow.Controls.Add(_walkIn);
        AddFull(walkInRow);
    }

    private void AddFull(Control row)
    {
        row.Dock = DockStyle.Fill;
        Grid.Controls.Add(row);
    }

    private static Control Labeled(string caption, Control input, bool required = false)
    {
        Control cell = input is TextBox ? FieldFrame.ForEditor(input) : input;
        cell.Dock = DockStyle.Top;
        cell.Height = FieldHeight;
        cell.Margin = Padding.Empty;

        var host = new Panel
        {
            Height = CaptionHeight + FieldHeight,
            Margin = new Padding(0, 0, 0, RowGap),
            BackColor = Color.White,
        };
        host.Controls.Add(cell);                                    // docked below the caption
        host.Controls.Add(new FieldCaption(caption, required));     // docked on top
        return host;
    }

    private static Control TwoUp(Control left, Control right)
    {
        var row = new TableLayoutPanel
        {
            ColumnCount = 3,
            RowCount = 1,
            Height = CaptionHeight + FieldHeight + RowGap,
            Margin = Padding.Empty,
            Padding = Padding.Empty,
            BackColor = Color.White,
        };
        row.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        row.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 16));
        row.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        row.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        left.Dock = DockStyle.Fill;
        right.Dock = DockStyle.Fill;
        row.Controls.Add(left, 0, 0);
        row.Controls.Add(right, 2, 0);
        return row;
    }

    /// <summary>Small bold caption above a field; required fields get a red star.</summary>
    private sealed class FieldCaption : Control
    {
        private const TextFormatFlags Flags = TextFormatFlags.Left | TextFormatFlags.VerticalCenter |
            TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine;

        private readonly string _text;
        private readonly bool _required;

        public FieldCaption(string text, bool required)
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
            _text = text;
            _required = required;
            Dock = DockStyle.Top;
            Height = CaptionHeight;
            TabStop = false;
            BackColor = Color.White;
            Font = Theme.Face(8.5f, FontStyle.Bold);
            ForeColor = Theme.TextSoft;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.Clear(BackColor);
            TextRenderer.DrawText(e.Graphics, _text, Font, new Rectangle(0, 0, Width, Height), ForeColor, Flags);
            if (!_required) return;

            int w = TextRenderer.MeasureText(e.Graphics, _text, Font, new Size(int.MaxValue, Height), Flags).Width;
            TextRenderer.DrawText(e.Graphics, "*", Font, new Rectangle(w + 2, 0, 16, Height), Theme.Danger, Flags);
        }
    }

    private void LoadPets()
    {
        _pet.Items.Clear();
        _pets = _customer.SelectedIndex < 0
            ? new List<PetRecord>()
            : _allPets.Where(p => p.CustomerId == _customers[_customer.SelectedIndex].Id).ToList();
        _pet.Items.AddRange(_pets.Select(p => (object)(string.IsNullOrWhiteSpace(p.Breed) ? p.Name : $"{p.Name}  \u00b7  {p.Breed}")).ToArray());
        _pet.SelectedIndex = _pets.Count > 0 ? 0 : -1;
        LoadServices();   // also when no pet is left to pick (the pet drop-down raises no change then)
        if (_customer.SelectedIndex >= 0 && _pets.Count == 0)
            ShowError("This customer has no pets yet. Add a pet first (Customer & Pet Look Up \u2192 ... \u2192 Add pet).");
        else
            ShowError("");
    }

    /// <summary>Lists the services priced for the chosen pet's species and weight (the name is the same for every weight).</summary>
    private void LoadServices()
    {
        _service.Items.Clear();
        try
        {
            _services = _pet.SelectedIndex >= 0 && _pet.SelectedIndex < _pets.Count
                ? EmployeeData.GetServicesForPet(_pets[_pet.SelectedIndex].Id)
                : new List<ServiceOffering>();
        }
        catch (SqlException ex)
        {
            _services = new List<ServiceOffering>();
            ShowDatabaseError(this, ex);
        }
        _service.Items.AddRange(_services.Select(s => (object)(s.Detail.Length > 0
            ? $"{s.Name} ({s.Detail})  \u00b7  {Money.Format(s.Price)}"
            : $"{s.Name}  \u00b7  {Money.Format(s.Price)}")).ToArray());
        _service.SelectedIndex = -1;
    }

    protected override void OnSaveClicked()
    {
        if (_customer.SelectedIndex < 0) { ShowError("Choose a customer."); return; }
        if (_pet.SelectedIndex < 0) { ShowError("Choose a pet."); return; }
        if (_service.SelectedIndex < 0) { ShowError("Choose a service."); return; }

        var start = _date.Value.Date + _time.Value;
        if (start < DateTime.Now.AddMinutes(-1)) { ShowError("Pick a time that hasn't passed yet."); return; }

        int? groomerId = _groomer.SelectedIndex > 0 ? _groomers[_groomer.SelectedIndex - 1].Id : null;
        int? roomId = _room.SelectedIndex > 0 ? _rooms[_room.SelectedIndex - 1].Id : null;
        bool confirm = _confirm.Checked;
        if (confirm && (groomerId is null || roomId is null))
        {
            ShowError("Choose a groomer and a room to confirm, or untick Confirm.");
            return;
        }

        try
        {
            if (EmployeeData.FindScheduleConflict(0, start, _services[_service.SelectedIndex].Id, groomerId, roomId) is { } conflict)
            {
                ShowError(conflict);
                return;
            }

            CreatedAppointmentId = EmployeeData.AddStaffAppointment(
                _customers[_customer.SelectedIndex].Id, _pets[_pet.SelectedIndex].Id, _services[_service.SelectedIndex].Id,
                start, groomerId, roomId, _notes.Text, confirm, _staffEmail, _walkIn.Checked);
            DialogResult = DialogResult.OK;
        }
        catch (Exception ex) when (ex is SqlException or InvalidOperationException)
        {
            ShowDatabaseError(this, ex);
        }
    }
}

/// <summary>Staff creates the sign-in for a customer who doesn't have one yet (role Customer, linked to that customer record).</summary>
internal sealed class CreateCustomerLoginForm : DialogBase
{
    private readonly CustomerRecord _customer;
    private readonly TextBox _email = new() { PlaceholderText = "Email the customer signs in with" };
    private readonly TextBox _password = new() { PlaceholderText = "At least 8 characters", UseSystemPasswordChar = true };

    public CreateCustomerLoginForm(CustomerRecord customer)
        : base("Create Customer Login", "Create", "\uE77B", "Lets the customer sign in and book their own grooming")
    {
        _customer = customer;
        _email.Text = customer.Email;

        AddRow("Customer", new Label { Text = customer.Name, TextAlign = ContentAlignment.MiddleLeft, AutoSize = false, Height = 28 });
        AddRow("Email *", _email);
        AddRow("Password *", _password);
        FitHeight();
    }

    protected override void OnSaveClicked()
    {
        string email = _email.Text.Trim();
        if (email.Length == 0 || !email.Contains('@')) { ShowError("Enter a valid email address."); return; }
        if (_password.Text.Length < 8) { ShowError("Password must be at least 8 characters."); return; }

        try
        {
            if (UserRepository.GetByEmail(email) is not null) { ShowError("That email is already used by another account."); return; }
            EmployeeData.CreateCustomerLogin(_customer.Id, email, _password.Text);
            DialogResult = DialogResult.OK;
        }
        catch (SqlException ex) when (ex.Number is 2627 or 2601)
        {
            ShowError("That email is already used by another account.");
        }
        catch (SqlException ex) when (ex.Number == 547)
        {
            ShowError("The database doesn't allow the Customer role yet. Run Sql/customer_account.sql once.");
        }
        catch (Exception ex) when (ex is SqlException or InvalidOperationException)
        {
            ShowError(ex.Message);
        }
    }
}
