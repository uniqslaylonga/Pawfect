using System.Drawing.Drawing2D;
using System.Globalization;
using Microsoft.Data.SqlClient;

namespace MyApp;

/// <summary>Shared look for the small pop-up forms used on the employee side.</summary>
internal abstract class DialogBase : Form
{
    protected readonly TableLayoutPanel Grid = new()
    {
        Dock = DockStyle.Top,
        AutoSize = true,
        ColumnCount = 2,
        Padding = new Padding(20, 18, 20, 4),
    };

    protected readonly Button SaveButton = new();
    protected readonly Button CancelButtonControl = new();
    private readonly Label _error = new();

    /// <summary>The bottom bar that holds Save and Cancel (buttons added later appear to their left).</summary>
    protected FlowLayoutPanel ButtonBar { get; private set; } = null!;

    protected DialogBase(string title, string saveText)
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
        ClientSize = new Size(440, 300);

        Grid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110));
        Grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

        _error.ForeColor = Theme.Danger;
        _error.AutoSize = false;
        _error.Dock = DockStyle.Top;
        _error.Height = 22;
        _error.Padding = new Padding(20, 0, 20, 0);

        SaveButton.Text = saveText;
        SaveButton.Size = new Size(110, 34);
        SaveButton.FlatStyle = FlatStyle.Flat;
        SaveButton.FlatAppearance.BorderSize = 0;
        SaveButton.BackColor = Theme.Primary;
        SaveButton.ForeColor = Color.White;
        SaveButton.Font = Theme.Face(9f, FontStyle.Bold);
        SaveButton.Cursor = Cursors.Hand;
        SaveButton.Click += (_, _) => OnSaveClicked();

        CancelButtonControl.Text = "Cancel";
        CancelButtonControl.Size = new Size(90, 34);
        CancelButtonControl.FlatStyle = FlatStyle.Flat;
        CancelButtonControl.FlatAppearance.BorderColor = Theme.Border;
        CancelButtonControl.BackColor = Color.White;
        CancelButtonControl.ForeColor = Theme.TextSoft;
        CancelButtonControl.DialogResult = DialogResult.Cancel;
        CancelButtonControl.Cursor = Cursors.Hand;

        var buttons = new FlowLayoutPanel
        {
            Dock = DockStyle.Bottom,
            FlowDirection = FlowDirection.RightToLeft,
            Height = 58,
            Padding = new Padding(16, 12, 16, 8),
            BackColor = Theme.Subtle,
        };
        buttons.Controls.Add(SaveButton);
        buttons.Controls.Add(CancelButtonControl);
        ButtonBar = buttons;

        Controls.Add(_error);
        Controls.Add(Grid);
        Controls.Add(buttons);
        AcceptButton = SaveButton;
        CancelButton = CancelButtonControl;
    }

    protected void AddRow(string label, Control input)
    {
        var l = new Label
        {
            Text = label,
            AutoSize = false,
            Height = 28,
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleLeft,
            ForeColor = Theme.TextSoft,
            Margin = new Padding(0, 4, 0, 4),
        };
        input.Dock = DockStyle.Fill;
        input.Margin = new Padding(0, 4, 0, 4);
        Grid.Controls.Add(l);
        Grid.Controls.Add(input);
    }

    protected void FitHeight()
    {
        Grid.PerformLayout();
        ClientSize = new Size(ClientSize.Width, Grid.PreferredSize.Height + 22 + 58 + 8);
    }

    protected void ShowError(string message) => _error.Text = message;

    protected abstract void OnSaveClicked();

    protected static void ShowDatabaseError(IWin32Window owner, Exception ex)
        => MessageBox.Show(owner, "The database could not be updated.\n\n" + ex.Message, "Pawfect",
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
        : base(existing is null ? "Add Customer" : "Edit Customer", "Save")
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
        : base(existing is null ? "Add Pet" : "Edit Pet", "Save")
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

/// <summary>Books a new appointment for an existing customer and pet.</summary>
internal sealed class NewAppointmentForm : DialogBase
{
    private readonly string _userEmail;
    private readonly List<CustomerRecord> _customers;
    private readonly List<PetRecord> _pets;
    private readonly List<ServiceOffering> _services;

    private readonly ComboBox _customer = new() { DropDownStyle = ComboBoxStyle.DropDownList };
    private readonly ComboBox _pet = new() { DropDownStyle = ComboBoxStyle.DropDownList };
    private readonly ComboBox _service = new() { DropDownStyle = ComboBoxStyle.DropDownList };
    private readonly DateTimePicker _date = new() { Format = DateTimePickerFormat.Long };
    private readonly DateTimePicker _time = new() { Format = DateTimePickerFormat.Custom, CustomFormat = "h:mm tt", ShowUpDown = true };
    private readonly Button _addPet = new() { Text = "+ Pet", Width = 62, Height = 26, FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand };

    public NewAppointmentForm(string userEmail, int? preselectCustomerId = null) : base("New Appointment", "Book")
    {
        _userEmail = userEmail;
        _customers = EmployeeData.GetCustomers();
        _pets = EmployeeData.GetPets();
        _services = EmployeeData.GetServices();

        _customer.Items.AddRange(_customers.Select(c => (object)(string.IsNullOrWhiteSpace(c.Phone) ? c.Name : $"{c.Name}  ·  {c.Phone}")).ToArray());
        _service.Items.AddRange(_services.Select(s => (object)$"{s.Name}  ·  {Money.Format(s.Price)}").ToArray());

        var petRow = new TableLayoutPanel { ColumnCount = 2, Height = 30, Margin = Padding.Empty };
        petRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        petRow.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 70));
        _pet.Dock = DockStyle.Fill;
        _addPet.FlatAppearance.BorderColor = Theme.Border;
        petRow.Controls.Add(_pet, 0, 0);
        petRow.Controls.Add(_addPet, 1, 0);

        AddRow("Customer *", _customer);
        AddRow("Pet *", petRow);
        AddRow("Service *", _service);
        AddRow("Date *", _date);
        AddRow("Time *", _time);

        _date.MinDate = DateTime.Today;
        _date.Value = DateTime.Today;
        _time.Value = DateTime.Today.AddHours(9);

        _customer.SelectedIndexChanged += (_, _) => FillPets();
        _addPet.Click += (_, _) => AddPetClicked();

        if (_customers.Count > 0)
            _customer.SelectedIndex = Math.Max(0, _customers.FindIndex(c => c.Id == preselectCustomerId));

        FitHeight();
        Shown += (_, _) =>
        {
            if (_customers.Count == 0)
                ShowError("Add a customer first (Customer & Pet Look Up).");
            else if (_services.Count == 0)
                ShowError("No services exist yet. An admin needs to add services.");
        };
    }

    private CustomerRecord? SelectedCustomer => _customer.SelectedIndex >= 0 ? _customers[_customer.SelectedIndex] : null;

    private List<PetRecord> PetsOfSelected
        => SelectedCustomer is { } c ? _pets.Where(p => p.CustomerId == c.Id).ToList() : new();

    private void FillPets(int? selectPetId = null)
    {
        var pets = PetsOfSelected;
        _pet.Items.Clear();
        _pet.Items.AddRange(pets.Select(p => (object)(string.IsNullOrWhiteSpace(p.Breed) ? p.Name : $"{p.Name}  ·  {p.Breed}")).ToArray());
        if (pets.Count > 0)
            _pet.SelectedIndex = Math.Max(0, pets.FindIndex(p => p.Id == selectPetId));
    }

    private void AddPetClicked()
    {
        if (SelectedCustomer is not { } customer) return;
        using var form = new PetForm(customer.Id, customer.Name);
        if (form.ShowDialog(this) != DialogResult.OK) return;

        _pets.Clear();
        _pets.AddRange(EmployeeData.GetPets());
        FillPets(form.CreatedPetId);
    }

    protected override void OnSaveClicked()
    {
        var pets = PetsOfSelected;
        if (SelectedCustomer is not { } customer) { ShowError("Choose a customer."); return; }
        if (_pet.SelectedIndex < 0) { ShowError("This customer has no pet yet. Use + Pet to add one."); return; }
        if (_service.SelectedIndex < 0) { ShowError("Choose a service."); return; }

        var start = _date.Value.Date + _time.Value.TimeOfDay;
        if (start < DateTime.Now.AddMinutes(-1)) { ShowError("Pick a time that hasn't passed yet."); return; }

        try
        {
            EmployeeData.AddAppointment(customer.Id, pets[_pet.SelectedIndex].Id, _services[_service.SelectedIndex].Id, start, _userEmail);
            DialogResult = DialogResult.OK;
        }
        catch (Exception ex) when (ex is SqlException or InvalidOperationException)
        {
            ShowDatabaseError(this, ex);
        }
    }
}

/// <summary>Edits an open appointment: which pet, which service, and when.</summary>
internal sealed class EditAppointmentForm : DialogBase
{
    private readonly AppointmentItem _item;
    private readonly List<PetRecord> _pets;
    private readonly List<ServiceOffering> _services;
    private readonly ComboBox _pet = new() { DropDownStyle = ComboBoxStyle.DropDownList };
    private readonly ComboBox _service = new() { DropDownStyle = ComboBoxStyle.DropDownList };
    private readonly DateTimePicker _date = new() { Format = DateTimePickerFormat.Long };
    private readonly DateTimePicker _time = new() { Format = DateTimePickerFormat.Custom, CustomFormat = "h:mm tt", ShowUpDown = true };

    public EditAppointmentForm(AppointmentItem item) : base("Edit Appointment", "Save")
    {
        _item = item;
        var refs = EmployeeData.GetAppointmentRefs(item.Id)
                   ?? throw new InvalidOperationException("That appointment no longer exists.");

        _pets = EmployeeData.GetPets().Where(p => p.CustomerId == refs.CustomerId).ToList();
        _services = EmployeeData.GetServices();

        _pet.Items.AddRange(_pets.Select(p => (object)(string.IsNullOrWhiteSpace(p.Breed) ? p.Name : $"{p.Name}  ·  {p.Breed}")).ToArray());
        _service.Items.AddRange(_services.Select(s => (object)$"{s.Name}  ·  {Money.Format(s.Price)}").ToArray());
        _pet.SelectedIndex = _pets.FindIndex(p => p.Id == refs.PetId);
        _service.SelectedIndex = _services.FindIndex(s => s.Id == refs.ServiceId);

        AddRow("Customer", new Label { Text = item.CustomerName, TextAlign = ContentAlignment.MiddleLeft, AutoSize = false, Height = 28 });
        AddRow("Pet *", _pet);
        AddRow("Service *", _service);
        AddRow("Date *", _date);
        AddRow("Time *", _time);

        // An overdue appointment keeps its own (past) date until the person picks a new one.
        var original = Trim(item.Start);
        _date.MinDate = original.Date < DateTime.Today ? original.Date : DateTime.Today;
        _date.Value = original.Date;
        _time.Value = DateTime.Today.Add(new TimeSpan(original.Hour, original.Minute, 0));
        FitHeight();

        Shown += (_, _) =>
        {
            if (_pet.SelectedIndex < 0 || _service.SelectedIndex < 0)
                ShowError("The old pet or service is no longer available. Please choose again.");
        };
    }

    private static DateTime Trim(DateTime d) => new(d.Year, d.Month, d.Day, d.Hour, d.Minute, 0);

    protected override void OnSaveClicked()
    {
        if (_pet.SelectedIndex < 0) { ShowError("Choose a pet."); return; }
        if (_service.SelectedIndex < 0) { ShowError("Choose a service."); return; }

        var start = _date.Value.Date + new TimeSpan(_time.Value.Hour, _time.Value.Minute, 0);
        if (start != Trim(_item.Start) && start < DateTime.Now.AddMinutes(-1)) { ShowError("Pick a time that hasn't passed yet."); return; }

        try
        {
            EmployeeData.UpdateAppointment(_item.Id, _pets[_pet.SelectedIndex].Id, _services[_service.SelectedIndex].Id, start);
            DialogResult = DialogResult.OK;
        }
        catch (Exception ex) when (ex is SqlException or InvalidOperationException)
        {
            ShowDatabaseError(this, ex);
        }
    }
}

/// <summary>Shows a receipt with its items and lets the cashier refund or void it (receipts are never deleted).</summary>
internal sealed class TransactionDetailsForm : DialogBase
{
    private readonly TransactionItem _item;
    private readonly Button _voidButton = new();

    /// <summary>Set to Refunded or Voided when the sale was changed.</summary>
    public TransactionStatus? NewStatus { get; private set; }

    public TransactionDetailsForm(TransactionItem item) : base("Transaction " + item.ReceiptNumber, "Refund")
    {
        _item = item;

        static Label Info(string text) => new() { Text = text, AutoSize = false, Height = 28, TextAlign = ContentAlignment.MiddleLeft };

        AddRow("Receipt", Info(item.ReceiptNumber));
        AddRow("Date", Info(item.When.ToString("MMM d, yyyy  h:mm tt", CultureInfo.CurrentCulture)));
        AddRow("Customer", Info(item.CustomerName));
        AddRow("Paid by", Info(item.PaymentMethod));
        AddRow("Status", Info(TransactionStatusStyle.Label(item.Status)));
        AddRow("Total", Info(Money.Format(item.Total)));

        var lines = new TextBox
        {
            Multiline = true,
            ReadOnly = true,
            Height = 90,
            ScrollBars = ScrollBars.Vertical,
            BackColor = Color.White,
        };
        try
        {
            lines.Text = string.Join(Environment.NewLine, EmployeeData.GetTransactionLines(item.Id).Select(l =>
                $"{l.Quantity} x {l.Name}{(l.Detail.Length == 0 ? "" : $" ({l.Detail})")}  -  {Money.Format(l.UnitPrice * l.Quantity)}"));
        }
        catch (SqlException)
        {
            lines.Text = "Couldn't load the items.";
        }
        AddRow("Items", lines);

        CancelButtonControl.Text = "Close";

        _voidButton.Text = "Void sale";
        _voidButton.Size = new Size(100, 34);
        _voidButton.FlatStyle = FlatStyle.Flat;
        _voidButton.FlatAppearance.BorderColor = Theme.Danger;
        _voidButton.BackColor = Color.White;
        _voidButton.ForeColor = Theme.Danger;
        _voidButton.Cursor = Cursors.Hand;
        _voidButton.Click += (_, _) => ChangeTo(TransactionStatus.Voided);
        ButtonBar.Controls.Add(_voidButton);

        bool open = item.Status == TransactionStatus.Completed;
        SaveButton.Enabled = open;
        _voidButton.Enabled = open;
        FitHeight();
    }

    protected override void OnSaveClicked() => ChangeTo(TransactionStatus.Refunded);

    private void ChangeTo(TransactionStatus status)
    {
        string verb = status == TransactionStatus.Voided ? "Void" : "Refund";
        if (MessageBox.Show(this, $"{verb} receipt {_item.ReceiptNumber} ({Money.Format(_item.Total)})?\n\nThis can't be undone.",
                "Pawfect", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
            return;

        try
        {
            if (!EmployeeData.ChangeSaleStatus(_item.Id, status))
            {
                ShowError("Only completed sales can be voided or refunded.");
                return;
            }
            NewStatus = status;
            DialogResult = DialogResult.OK;
        }
        catch (SqlException ex)
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

/// <summary>Every sale, searchable. Double-click (or Open) shows the receipt, where it can be refunded or voided.</summary>
internal sealed class TransactionHistoryForm : Form
{
    private readonly ListView _list = new()
    {
        View = View.Details, FullRowSelect = true, HideSelection = false, Dock = DockStyle.Fill, BorderStyle = BorderStyle.None,
    };
    private readonly TextBox _search = new() { Dock = DockStyle.Fill, PlaceholderText = "Search receipt or customer..." };
    private List<TransactionItem> _all = new();

    /// <summary>True when a sale was refunded or voided while this window was open.</summary>
    public bool Changed { get; private set; }

    public TransactionHistoryForm()
    {
        Text = "Transaction History";
        Font = Theme.Face(9f);
        BackColor = Color.White;
        ForeColor = Theme.Text;
        StartPosition = FormStartPosition.CenterParent;
        ClientSize = new Size(880, 520);
        MinimumSize = new Size(700, 360);
        ShowInTaskbar = false;

        _list.Columns.Add("Receipt", 110);
        _list.Columns.Add("Date", 160);
        _list.Columns.Add("Customer", 170);
        _list.Columns.Add("Items", 130);
        _list.Columns.Add("Total", 90);
        _list.Columns.Add("Paid by", 80);
        _list.Columns.Add("Status", 90);

        var top = new Panel { Dock = DockStyle.Top, Height = 44, Padding = new Padding(12, 10, 12, 6) };
        top.Controls.Add(_search);

        var open = new Button { Text = "Open", Size = new Size(90, 32), FlatStyle = FlatStyle.Flat, BackColor = Theme.Primary, ForeColor = Color.White, Cursor = Cursors.Hand };
        open.FlatAppearance.BorderSize = 0;
        var close = new Button { Text = "Close", Size = new Size(90, 32), FlatStyle = FlatStyle.Flat, BackColor = Color.White, ForeColor = Theme.TextSoft, DialogResult = DialogResult.Cancel, Cursor = Cursors.Hand };
        close.FlatAppearance.BorderColor = Theme.Border;
        var bottom = new FlowLayoutPanel
        {
            Dock = DockStyle.Bottom, Height = 52, FlowDirection = FlowDirection.RightToLeft,
            Padding = new Padding(12, 10, 12, 6), BackColor = Theme.Subtle,
        };
        bottom.Controls.Add(close);
        bottom.Controls.Add(open);

        Controls.Add(_list);
        Controls.Add(top);
        Controls.Add(bottom);
        CancelButton = close;

        _search.TextChanged += (_, _) => Fill();
        _list.DoubleClick += (_, _) => OpenSelected();
        open.Click += (_, _) => OpenSelected();

        Reload();
    }

    private void Reload()
    {
        _all = EmployeeData.GetRecentTransactions(1000);
        Fill();
    }

    private void Fill()
    {
        string q = _search.Text.Trim();
        _list.BeginUpdate();
        _list.Items.Clear();
        foreach (var x in _all.Where(x => q.Length == 0
                     || x.ReceiptNumber.Contains(q, StringComparison.CurrentCultureIgnoreCase)
                     || x.CustomerName.Contains(q, StringComparison.CurrentCultureIgnoreCase)))
        {
            var row = new ListViewItem(x.ReceiptNumber) { Tag = x };
            row.SubItems.Add(x.When.ToString("MMM d, yyyy  h:mm tt", CultureInfo.CurrentCulture));
            row.SubItems.Add(x.CustomerName);
            row.SubItems.Add($"{x.ServiceCount} services, {x.ItemCount} items");
            row.SubItems.Add(Money.Format(x.Total));
            row.SubItems.Add(x.PaymentMethod);
            row.SubItems.Add(TransactionStatusStyle.Label(x.Status));
            _list.Items.Add(row);
        }
        _list.EndUpdate();
    }

    private void OpenSelected()
    {
        if (_list.SelectedItems.Count == 0 || _list.SelectedItems[0].Tag is not TransactionItem item) return;

        using var form = new TransactionDetailsForm(item);
        if (form.ShowDialog(this) != DialogResult.OK) return;

        Changed = true;
        try { Reload(); }
        catch (SqlException ex) { MessageBox.Show(this, "Couldn't reload the list.\n\n" + ex.Message, "Pawfect", MessageBoxButtons.OK, MessageBoxIcon.Error); }
    }
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

    public FullCalendarForm(IReadOnlyList<AppointmentItem> items)
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

        Button Nav(string text, int width)
        {
            var b = new Button
            {
                Text = text, Size = new Size(width, 32), FlatStyle = FlatStyle.Flat, BackColor = Color.White,
                ForeColor = Theme.Text, Cursor = Cursors.Hand, Margin = new Padding(0, 0, 6, 0),
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
        _show.Enabled = false;

        var close = new Button
        {
            Text = "Close", Size = new Size(90, 32), FlatStyle = FlatStyle.Flat, BackColor = Color.White,
            ForeColor = Theme.TextSoft, DialogResult = DialogResult.Cancel, Cursor = Cursors.Hand,
        };
        close.FlatAppearance.BorderColor = Theme.Border;

        var hint = new Label
        {
            Text = "Click a day to select it. Double-click to show it in the Appointments list.",
            AutoSize = false, Width = 480, Height = 32, TextAlign = ContentAlignment.MiddleLeft, ForeColor = Theme.TextSoft,
        };

        var bottom = new FlowLayoutPanel
        {
            Dock = DockStyle.Bottom, Height = 52, FlowDirection = FlowDirection.RightToLeft,
            Padding = new Padding(12, 10, 12, 6), BackColor = Theme.Subtle,
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
