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

    private readonly string _staffEmail;
    private readonly List<CustomerRecord> _customers;
    private readonly List<PetRecord> _allPets;
    private readonly List<ServiceOffering> _services;
    private readonly List<GroomerOption> _groomers;
    private readonly List<RoomOption> _rooms;
    private List<PetRecord> _pets = new();

    private readonly PawSelect _customer = new() { Placeholder = "Choose a customer" };
    private readonly PawSelect _pet = new() { Placeholder = "Choose a pet" };
    private readonly PawSelect _service = new() { Placeholder = "Choose a service" };
    private readonly PawSelect _groomer = new();
    private readonly PawSelect _room = new();
    private readonly PawDatePicker _date = new();
    private readonly PawTimePicker _time = new();
    private readonly TextBox _notes = new() { PlaceholderText = "Optional notes", MaxLength = 300 };
    private readonly PawCheckBox _confirm = new() { Text = "Confirm this appointment", Checked = true };

    public int? CreatedAppointmentId { get; private set; }

    public NewAppointmentForm(string staffEmail, int? customerId = null)
        : base("New Appointment", "Save", "\uE787", "Book a customer in (walk-in or phone call)")
    {
        _staffEmail = staffEmail;
        _customers = EmployeeData.GetCustomers().OrderBy(c => c.Name).ToList();
        _allPets = EmployeeData.GetPets();
        _services = EmployeeData.GetServices();
        _groomers = EmployeeData.GetGroomers();
        _rooms = EmployeeData.GetRooms();

        _customer.Items.AddRange(_customers.Select(c => (object)(string.IsNullOrWhiteSpace(c.Phone) ? c.Name : $"{c.Name}  \u00b7  {c.Phone}")).ToArray());
        _service.Items.AddRange(_services.Select(s => (object)$"{s.Name}  \u00b7  {Money.Format(s.Price)}").ToArray());
        _groomer.Items.Add(NotAssigned);
        _groomer.Items.AddRange(_groomers.Select(g => (object)g.Name).ToArray());
        _groomer.SelectedIndex = 0;
        _room.Items.Add(NotAssigned);
        _room.Items.AddRange(_rooms.Select(r => (object)r.Name).ToArray());
        _room.SelectedIndex = 0;

        AddRow("Customer *", _customer);
        AddRow("Pet *", _pet);
        AddRow("Service *", _service);
        AddRow("Date *", _date);
        AddRow("Time *", _time);
        AddRow("Groomer", _groomer);
        AddRow("Room", _room);
        AddRow("Notes", _notes);
        AddRow("", _confirm);

        _date.MinDate = DateTime.Today;
        _date.Value = DateTime.Today;
        var next = DateTime.Now.AddHours(1);
        _time.Value = new TimeSpan(Math.Clamp(next.Hour, 6, 22), 0, 0);

        _customer.SelectedIndexChanged += (_, _) => LoadPets();
        if (customerId is int id)
            _customer.SelectedIndex = _customers.FindIndex(c => c.Id == id);

        FitHeight();

        Shown += (_, _) =>
        {
            if (_customers.Count == 0) ShowError("There are no customers yet. Add a customer first.");
            else if (_services.Count == 0) ShowError("There are no services yet. Ask an admin to add them to the catalog.");
            else if (_groomers.Count == 0 || _rooms.Count == 0) ShowError("No groomers or rooms exist yet. Run Sql/appointment_scheduling.sql.");
        };
    }

    private void LoadPets()
    {
        _pet.Items.Clear();
        _pets = _customer.SelectedIndex < 0
            ? new List<PetRecord>()
            : _allPets.Where(p => p.CustomerId == _customers[_customer.SelectedIndex].Id).ToList();
        _pet.Items.AddRange(_pets.Select(p => (object)(string.IsNullOrWhiteSpace(p.Breed) ? p.Name : $"{p.Name}  \u00b7  {p.Breed}")).ToArray());
        _pet.SelectedIndex = _pets.Count > 0 ? 0 : -1;
        if (_customer.SelectedIndex >= 0 && _pets.Count == 0)
            ShowError("This customer has no pets yet. Add a pet first (Customer & Pet Look Up \u2192 ... \u2192 Add pet).");
        else
            ShowError("");
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
                start, groomerId, roomId, _notes.Text, confirm, _staffEmail);
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
