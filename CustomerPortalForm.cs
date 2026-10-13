using System.Globalization;
using Microsoft.Data.SqlClient;
using MyApp.Controls;

namespace MyApp;

/// <summary>What a signed-in customer sees: only their own pets, appointments and purchases.</summary>
internal sealed class CustomerPortalForm : Form
{
    private readonly UserAccount _user;
    private (int Id, string Name)? _customer;

    private readonly ListView _appointments = NewList("When", 170, "Pet", 110, "Service", 170, "Status", 100, "Groomer", 110, "Room", 110);
    private readonly ListView _pets = NewList("Pet", 160, "Species", 120, "Breed", 200);
    private readonly ListView _history = NewList("Receipt", 120, "Date", 170, "Services", 80, "Items", 80, "Total", 110, "Paid by", 100);
    private readonly Label _welcome = new();
    private readonly Button _bookButton = ActionButton("Request Appointment", primary: true);
    private readonly Button _cancelButton = ActionButton("Cancel Appointment", primary: false);
    private readonly Button _addPetButton = ActionButton("Add Pet", primary: false);
    private readonly Button _scanCardButton = ActionButton("Scan Vaccination Card", primary: false);
    private List<AppointmentItem> _appointmentData = new();

    public bool LoggedOut { get; private set; }

    public CustomerPortalForm(UserAccount user)
    {
        _user = user;

        Text = "Pawfect";
        Font = Theme.Face(9f);
        BackColor = Theme.Bg;
        ClientSize = new Size(900, 580);
        MinimumSize = new Size(760, 480);
        StartPosition = FormStartPosition.CenterScreen;

        var header = new Panel { Dock = DockStyle.Top, Height = 72, BackColor = Color.White, Padding = new Padding(24, 0, 24, 0) };
        _welcome.Dock = DockStyle.Fill;
        _welcome.Font = Theme.Face(13f, FontStyle.Bold);
        _welcome.ForeColor = Theme.Text;
        _welcome.TextAlign = ContentAlignment.MiddleLeft;
        _welcome.Text = $"Welcome, {user.Name}";

        var logout = ActionButton("Log Out", primary: false);
        logout.Dock = DockStyle.Right;
        logout.Width = 100;
        logout.Click += (_, _) => { LoggedOut = true; Close(); };
        header.Controls.Add(_welcome);
        header.Controls.Add(logout);

        var tabs = new TabControl { Dock = DockStyle.Fill, Padding = new Point(18, 6) };
        tabs.TabPages.Add(Tab("My Appointments", _appointments));
        tabs.TabPages.Add(Tab("My Pets", _pets));
        tabs.TabPages.Add(Tab("Purchase History", _history));

        var buttons = new FlowLayoutPanel { Dock = DockStyle.Bottom, Height = 56, Padding = new Padding(20, 10, 20, 8), BackColor = Color.White };
        buttons.Controls.AddRange(new Control[] { _bookButton, _cancelButton, _addPetButton, _scanCardButton });

        var body = new Panel { Dock = DockStyle.Fill, Padding = new Padding(20, 16, 20, 12) };
        body.Controls.Add(tabs);

        Controls.Add(body);
        Controls.Add(buttons);
        Controls.Add(header);

        _bookButton.Click += (_, _) => Book();
        _cancelButton.Click += (_, _) => CancelSelected();
        _addPetButton.Click += (_, _) => AddPet();
        _scanCardButton.Width = 190;
        _scanCardButton.Click += (_, _) => ScanCard();
        _appointments.SelectedIndexChanged += (_, _) => UpdateButtons();

        Shown += (_, _) => Reload();
    }

    private static ListView NewList(params object[] columns)
    {
        var list = new ListView { Dock = DockStyle.Fill, View = View.Details, FullRowSelect = true, MultiSelect = false, HideSelection = false, BorderStyle = BorderStyle.None };
        for (int i = 0; i < columns.Length; i += 2)
            list.Columns.Add((string)columns[i], (int)columns[i + 1]);
        return list;
    }

    private static TabPage Tab(string title, Control content)
    {
        var page = new TabPage(title) { BackColor = Color.White };
        page.Controls.Add(content);
        return page;
    }

    private static Button ActionButton(string text, bool primary)
    {
        var b = new Button
        {
            Text = text, Size = new Size(150, 34), FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand,
            BackColor = primary ? Theme.Primary : Color.White,
            ForeColor = primary ? Color.White : Theme.TextSoft,
            Font = Theme.Face(9f, FontStyle.Bold),
        };
        b.FlatAppearance.BorderSize = primary ? 0 : 1;
        b.FlatAppearance.BorderColor = Theme.Border;
        return b;
    }

    private void Reload()
    {
        try
        {
            _customer = EmployeeData.GetCustomerForLogin(_user.Email);
            if (_customer is not { } c)
            {
                _bookButton.Enabled = _addPetButton.Enabled = _scanCardButton.Enabled = _cancelButton.Enabled = false;
                _welcome.Text = $"Welcome, {_user.Name}  \u2014  this login isn't linked to a customer record yet.";
                return;
            }

            _appointmentData = EmployeeData.GetAppointmentsForCustomer(c.Id);
            var culture = CultureInfo.CurrentCulture;

            _appointments.Items.Clear();
            foreach (var a in _appointmentData)
                _appointments.Items.Add(new ListViewItem(new[]
                {
                    a.Start.ToString("ddd, MMM d, yyyy  h:mm tt", culture), a.PetName, a.Service, AppointmentStatusStyle.Label(a.Status),
                    a.Groomer.Length == 0 ? "\u2014" : a.Groomer, a.Room.Length == 0 ? "\u2014" : a.Room,
                }) { Tag = a });

            _pets.Items.Clear();
            foreach (var p in EmployeeData.GetPets().Where(p => p.CustomerId == c.Id))
                _pets.Items.Add(new ListViewItem(new[] { p.Name, p.Species, p.Breed }));

            _history.Items.Clear();
            foreach (var t in EmployeeData.GetTransactionsForCustomer(c.Id))
                _history.Items.Add(new ListViewItem(new[]
                {
                    t.ReceiptNumber, t.When.ToString("MMM d, yyyy  h:mm tt", culture), t.ServiceCount.ToString(culture),
                    t.ItemCount.ToString(culture), Money.Format(t.Total), t.PaymentMethod,
                }));

            UpdateButtons();
        }
        catch (SqlException ex)
        {
            Msg.Show(this, "Couldn't load your data.\n\n" + ex.Message, "Pawfect", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void UpdateButtons()
    {
        bool linked = _customer is not null;
        _bookButton.Enabled = linked;
        _addPetButton.Enabled = linked;
        _scanCardButton.Enabled = linked;
        _cancelButton.Enabled = linked && _appointments.SelectedItems.Count == 1 &&
            _appointments.SelectedItems[0].Tag is AppointmentItem { Status: AppointmentStatus.Pending or AppointmentStatus.Confirmed or AppointmentStatus.Rescheduled };
    }

    private void Book()
    {
        if (_customer is not { } c) return;
        try
        {
            using var form = new CustomerBookingForm(c.Id, c.Name, _user.Email);
            if (form.ShowDialog(this) == DialogResult.OK)
            {
                Reload();
                Msg.Show(this,
                    "Your request was sent.\n\nThe shop will set the day, time, groomer and room, and it will show here once it is confirmed.",
                    "Pawfect", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        catch (SqlException ex)
        {
            Msg.Show(this, "Couldn't open the request form.\n\n" + ex.Message, "Pawfect", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void AddPet()
    {
        if (_customer is not { } c) return;
        using var form = new PetForm(c.Id, c.Name);
        if (form.ShowDialog(this) == DialogResult.OK) Reload();
    }

    private void ScanCard()
    {
        if (_customer is not { } c) return;
        try
        {
            using var form = new VaccinationScanForm(c.Id, c.Name);
            if (form.ShowDialog(this) == DialogResult.OK)
            {
                Reload();
                Msg.Show(this, "Vaccination card saved.\n\nYour pet's details and vaccinations were added to your account.",
                    "Pawfect", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        catch (SqlException ex)
        {
            Msg.Show(this, "Couldn't open the scanner.\n\n" + ex.Message, "Pawfect", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void CancelSelected()
    {
        if (_customer is not { } c || _appointments.SelectedItems.Count != 1) return;
        if (_appointments.SelectedItems[0].Tag is not AppointmentItem a) return;

        if (Msg.Show(this, $"Cancel the appointment for {a.PetName} on {a.Start:MMM d, h:mm tt}?", "Pawfect",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

        try
        {
            if (!EmployeeData.CancelOwnAppointment(c.Id, a.Id))
                Msg.Show(this, "That appointment can't be cancelled any more.", "Pawfect", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (SqlException ex)
        {
            Msg.Show(this, "Couldn't cancel.\n\n" + ex.Message, "Pawfect", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }
        Reload();
    }
}

/// <summary>A customer asks for an appointment (pet, service, preferred day/time, notes). The shop schedules it.</summary>
internal sealed class CustomerBookingForm : DialogBase
{
    private readonly int _customerId;
    private readonly string _email;
    private readonly string _customerName;
    private readonly List<PetRecord> _pets;
    private readonly List<ServiceOffering> _services;

    private readonly PawSelect _pet = new() { Placeholder = "Choose a pet" };
    private readonly PawSelect _service = new() { Placeholder = "Choose a service" };
    private readonly PawDatePicker _date = new();
    private readonly PawTimePicker _time = new();
    private readonly TextBox _notes = new() { Multiline = true, Height = 60, MaxLength = 300, ScrollBars = ScrollBars.Vertical };

    public CustomerBookingForm(int customerId, string customerName, string loginEmail) : base("Request Appointment", "Send Request", "\uE787", "Tell us when you'd like to come in")
    {
        _customerId = customerId;
        _customerName = customerName;
        _email = loginEmail;
        _pets = EmployeeData.GetPets().Where(p => p.CustomerId == customerId).ToList();
        _services = EmployeeData.GetServices();

        _pet.Items.AddRange(_pets.Select(p => (object)p.Name).ToArray());
        _service.Items.AddRange(_services.Select(s => (object)$"{s.Name}  \u00B7  {Money.Format(s.Price)}").ToArray());

        AddRow("Pet *", _pet);
        AddRow("Service *", _service);
        AddRow("Preferred date *", _date);
        AddRow("Preferred time *", _time);
        AddRow("Notes", _notes);

        _date.MinDate = DateTime.Today;
        _date.Value = DateTime.Today;
        _time.Value = TimeSpan.FromHours(9);
        if (_pets.Count > 0) _pet.SelectedIndex = 0;
        FitHeight();

        Shown += (_, _) =>
        {
            if (_pets.Count == 0) ShowError("Add a pet first (Add Pet button).");
            else if (_services.Count == 0) ShowError("No services are available yet.");
        };
    }

    protected override void OnSaveClicked()
    {
        if (_pet.SelectedIndex < 0) { ShowError("Choose a pet."); return; }
        if (_service.SelectedIndex < 0) { ShowError("Choose a service."); return; }

        var start = _date.Value.Date + _time.Value;
        if (start < DateTime.Now.AddMinutes(-1)) { ShowError("Pick a time that hasn't passed yet."); return; }

        try
        {
            EmployeeData.AddOwnAppointment(_customerId, _pets[_pet.SelectedIndex].Id, _services[_service.SelectedIndex].Id, start, _email, _notes.Text);
            DialogResult = DialogResult.OK;
        }
        catch (Exception ex) when (ex is SqlException or InvalidOperationException)
        {
            ShowDatabaseError(this, ex);
        }
    }
}
