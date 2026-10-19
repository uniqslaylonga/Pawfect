namespace MyApp;

/// <summary>
/// Employee &amp; User Management screen (two sections: Employee Management and User Management).
/// Layout lives in EmployeeManagementPage.Designer.cs, so open this file's designer in Visual Studio to move or restyle anything.
/// Nothing is pre-filled: counts show 0 and the tables show their empty states until records are loaded.
/// </summary>
internal partial class EmployeeManagementPage : UserControl
{
    public event Action<EmployeeRepository.EmployeeRow>? EmployeeScheduleRequested;

    private Dictionary<int, (int Days, int Absences)> _summaries = new();
    private Label? _scheduleHeader;

    private List<UserRepository.UserRow> _allUsers = new();


    /// <summary>The "+ Add Employee" button was clicked.</summary>
    public event EventHandler? AddEmployeeRequested;

    private List<CustomerRepository.CustomerRow> _allCustomers = new();

    public event EventHandler? AddCustomerRequested;
    public event Action<int, bool>? CustomerToggleActiveRequested;
    public event Action<CustomerRepository.CustomerRow>? CustomerEditRequested;

    public event Action<EmployeeRepository.EmployeeRow>? EmployeeEditRequested;
    public event Action<int, bool>? EmployeeToggleActiveRequested;

    private List<EmployeeRepository.EmployeeRow> _allEmployees = new();

    public EmployeeManagementPage()
    {
        InitializeComponent();
        DoubleBuffered = true;

        // Employee roles (Cashier, Groomer...) will be listed here once they exist in the database.
        employeeRoleFilter.SetItems(new[] { "All Roles" }.Concat(EmployeeRepository.Positions));
        // Account roles come straight from the UserRole enum, so this list can't drift from the database.
        userRoleFilter.SetItems(new[] { "All Customers" });

        foreach (var filter in new[] { employeeStatusFilter, userStatusFilter })
            filter.SetItems(new[] { "All Status", "Active", "Inactive" });

        employeeAddButton.Click += (_, _) => AddEmployeeRequested?.Invoke(this, EventArgs.Empty);
        userAddButton.Click += (_, _) => AddCustomerRequested?.Invoke(this, EventArgs.Empty);
        userSearch.Box.TextChanged += (_, _) => ApplyCustomerFilter();
        employeeSearch.Box.TextChanged += (_, _) => ApplyEmployeeFilter();

        userTitle.Text = "Customer Management";
        userSubtitle.Text = "Manage registered customers.";
        userAddButton.Text = "+ Add Customer";
        userColUsername.Text = "Phone";
        userColFullName.Text = "Name";
        userColRole.Text = "Registered";
        userTotalLabel.Text = "Total Customers";
        userAdminLabel.Text = "Active";
        userStaffLabel.Text = "Inactive";
        userCustomerLabel.Text = "New This Month";
        // The old Cashier tile now shows absences this month.
        employeeCashierTile.Visible = true;
        employeeCashierLabel.Text = "Absences This Month";
        AddScheduleColumn();

        // Customers no longer have an address: collapse that column.
        userColLastLogin.Visible = false;
        userColumnsGrid.ColumnStyles[userColumnsGrid.GetColumn(userColLastLogin)] = new ColumnStyle(SizeType.Absolute, 0);
    }

    public void LoadCustomers(List<CustomerRepository.CustomerRow> customers)
    {
        _allCustomers = customers;
        ApplyCustomerFilter();
    }

    private void ApplyCustomerFilter()
    {
        string q = userSearch.Value.Trim();
        var shown = _allCustomers.Where(c =>
            q.Length == 0 ||
            c.Name.Contains(q, StringComparison.OrdinalIgnoreCase) ||
            c.Phone.Contains(q, StringComparison.OrdinalIgnoreCase) ||
            (c.Email?.Contains(q, StringComparison.OrdinalIgnoreCase) ?? false)).ToList();

        userRows.SuspendLayout();

        foreach (var old in userRows.Controls.Cast<Control>().Where(c => c != userEmpty).ToList())
        {
            userRows.Controls.Remove(old);
            old.Dispose();
        }

        userEmpty.Visible = shown.Count == 0;

        for (int i = shown.Count - 1; i >= 0; i--)
            userRows.Controls.Add(BuildCustomerRow(shown[i]));

        var now = DateTime.Now;
        userTotalValue.Text = _allCustomers.Count.ToString();
        userAdminValue.Text = _allCustomers.Count(c => c.Active).ToString();
        userStaffValue.Text = _allCustomers.Count(c => !c.Active).ToString();
        userCustomerValue.Text = _allCustomers.Count(c =>
            c.CreatedAt.Year == now.Year && c.CreatedAt.Month == now.Month).ToString();

        userShowing.Text = shown.Count == 0
            ? "No customers"
            : $"Showing 1 to {shown.Count} of {_allCustomers.Count} customers";

        userRows.ResumeLayout(true);
    }

    private Control BuildCustomerRow(CustomerRepository.CustomerRow c)
    {
        var grid = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.White,
            Margin = Padding.Empty,
            ColumnCount = userColumnsGrid.ColumnCount,
            RowCount = 1,
        };
        foreach (ColumnStyle s in userColumnsGrid.ColumnStyles)
            grid.ColumnStyles.Add(new ColumnStyle(s.SizeType, s.Width));
        grid.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

        void Put(Control header, string text, Color? color = null)
        {
            grid.Controls.Add(new Label
            {
                Text = text,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                AutoEllipsis = true,
                UseMnemonic = false,
                BackColor = Color.White,
                ForeColor = color ?? Color.FromArgb(31, 42, 60),
                Font = new Font("Segoe UI", 9F),
            }, userColumnsGrid.GetColumn(header), 0);
        }

        Put(userColId, c.Id.ToString());
        Put(userColUsername, c.Phone);
        Put(userColFullName, c.Name);
        Put(userColRole, c.CreatedAt.ToString("MMM d, yyyy"));
        Put(userColEmail, c.Email ?? "—");
        Put(userColStatus, c.Active ? "Active" : "Inactive",
            c.Active ? Color.FromArgb(46, 158, 91) : Color.FromArgb(122, 136, 156));

        var actions = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.White,
            WrapContents = false,
            Margin = Padding.Empty,
            Padding = new Padding(0, 14, 0, 0),
        };
        Label Link(string text, Action onClick)
        {
            var l = new Label
            {
                Text = text,
                AutoSize = true,
                Margin = new Padding(0, 0, 14, 0),
                BackColor = Color.White,
                ForeColor = Color.FromArgb(58, 123, 213),
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Cursor = Cursors.Hand,
                UseMnemonic = false,
            };
            l.Click += (_, _) => onClick();
            return l;
        }
        actions.Controls.Add(Link("Edit", () => CustomerEditRequested?.Invoke(c)));
        actions.Controls.Add(Link(c.Active ? "Deactivate" : "Activate",
            () => CustomerToggleActiveRequested?.Invoke(c.Id, !c.Active)));
        grid.Controls.Add(actions, userColumnsGrid.GetColumn(userColActions), 0);

        var row = new Panel { Dock = DockStyle.Top, Height = 44, BackColor = Color.White };
        row.Controls.Add(grid);
        return row;
    }
    private void AddScheduleColumn()
    {
        int at = employeeColumnsGrid.GetColumn(employeeColActions);
        var moving = employeeColumnsGrid.Controls.Cast<Control>()
            .Where(c => employeeColumnsGrid.GetColumn(c) >= at).ToList();

        employeeColumnsGrid.ColumnCount++;
        employeeColumnsGrid.ColumnStyles.Insert(at, new ColumnStyle(SizeType.Absolute, 170));
        foreach (var c in moving)
            employeeColumnsGrid.SetColumn(c, employeeColumnsGrid.GetColumn(c) + 1);

        _scheduleHeader = new Label
        {
            Text = "SCHEDULE / ABSENCES",
            Dock = employeeColStatus.Dock,
            TextAlign = employeeColStatus.TextAlign,
            Font = employeeColStatus.Font,
            ForeColor = employeeColStatus.ForeColor,
            BackColor = employeeColStatus.BackColor,
            Margin = employeeColStatus.Margin,
            UseMnemonic = false,
        };
        employeeColumnsGrid.Controls.Add(_scheduleHeader, at, 0);
    }

    public void LoadEmployees(List<EmployeeRepository.EmployeeRow> employees,
                          Dictionary<int, (int Days, int Absences)> summaries)
    {
        _allEmployees = employees;
        _summaries = summaries;
        ApplyEmployeeFilter();
    }

    private void ApplyEmployeeFilter()
    {
        string q = employeeSearch.Value.Trim();
        var shown = _allEmployees.Where(e =>
            q.Length == 0 ||
            e.Name.Contains(q, StringComparison.OrdinalIgnoreCase) ||
            e.Email.Contains(q, StringComparison.OrdinalIgnoreCase) ||
            e.Position.Contains(q, StringComparison.OrdinalIgnoreCase)).ToList();

        employeeRows.SuspendLayout();

        foreach (var old in employeeRows.Controls.Cast<Control>().Where(c => c != employeeEmpty).ToList())
        {
            employeeRows.Controls.Remove(old);
            old.Dispose();
        }

        employeeEmpty.Visible = shown.Count == 0;

        for (int i = shown.Count - 1; i >= 0; i--)
            employeeRows.Controls.Add(BuildEmployeeRow(shown[i]));

        employeeTotalValue.Text = _allEmployees.Count.ToString();
        employeeAdminValue.Text = _allEmployees.Count(e => e.Position == "Admin").ToString();
        employeeFrontDeskValue.Text = _allEmployees.Count(e => e.Position == "Front Desk").ToString();
        employeeGroomersValue.Text = _allEmployees.Count(e => e.Position == "Groomer").ToString();
        employeeCashierValue.Text = _summaries.Values.Sum(v => v.Absences).ToString();   // absences this month

        employeeShowing.Text = shown.Count == 0
            ? "No employees"
            : $"Showing 1 to {shown.Count} of {_allEmployees.Count} employees";

        employeeRows.ResumeLayout(true);
    }

    private Control BuildEmployeeRow(EmployeeRepository.EmployeeRow emp)
    {
        var grid = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.White,
            Margin = Padding.Empty,
            ColumnCount = employeeColumnsGrid.ColumnCount,
            RowCount = 1,
        };
        foreach (ColumnStyle s in employeeColumnsGrid.ColumnStyles)
            grid.ColumnStyles.Add(new ColumnStyle(s.SizeType, s.Width));
        grid.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

        void Put(Control header, string text, Color? color = null)
        {
            grid.Controls.Add(new Label
            {
                Text = text,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                AutoEllipsis = true,
                UseMnemonic = false,
                BackColor = Color.White,
                ForeColor = color ?? Color.FromArgb(31, 42, 60),
                Font = new Font("Segoe UI", 9F),
            }, employeeColumnsGrid.GetColumn(header), 0);
        }

        Put(employeeColId, emp.Id.ToString());
        Put(employeeColName, emp.Name);
        Put(employeeColRole, emp.Position);
        Put(employeeColDepartment, emp.StartDate.ToString("MMM d, yyyy"));
        Put(employeeColEmail, emp.Email);
        Put(employeeColStatus, emp.Active ? "Active" : "Inactive",
            emp.Active ? Color.FromArgb(46, 158, 91) : Color.FromArgb(122, 136, 156));

        // Schedule / absences: click to open the schedule popup.
        var sum = _summaries.TryGetValue(emp.Id, out var found) ? found : (Days: 0, Absences: 0);
        string text = (sum.Days == 0 ? "No schedule" : $"{sum.Days} day{(sum.Days == 1 ? "" : "s")}/week")
                      + $"  ·  {sum.Absences} absent";
        var scheduleLink = new Label
        {
            Text = text,
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleLeft,
            AutoEllipsis = true,
            UseMnemonic = false,
            BackColor = Color.White,
            ForeColor = Color.FromArgb(58, 123, 213),
            Font = new Font("Segoe UI", 9F, FontStyle.Bold),
            Cursor = Cursors.Hand,
        };
        scheduleLink.Click += (_, _) => EmployeeScheduleRequested?.Invoke(emp);
        grid.Controls.Add(scheduleLink, employeeColumnsGrid.GetColumn(_scheduleHeader!), 0);

        var actions = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.White,
            WrapContents = false,
            Margin = Padding.Empty,
            Padding = new Padding(0, 14, 0, 0),
        };
        Label Link(string t, Action onClick)
        {
            var l = new Label
            {
                Text = t,
                AutoSize = true,
                Margin = new Padding(0, 0, 14, 0),
                BackColor = Color.White,
                ForeColor = Color.FromArgb(58, 123, 213),
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Cursor = Cursors.Hand,
                UseMnemonic = false,
            };
            l.Click += (_, _) => onClick();
            return l;
        }
        actions.Controls.Add(Link("Edit", () => EmployeeEditRequested?.Invoke(emp)));
        actions.Controls.Add(Link(emp.Active ? "Deactivate" : "Activate",
            () => EmployeeToggleActiveRequested?.Invoke(emp.Id, !emp.Active)));
        grid.Controls.Add(actions, employeeColumnsGrid.GetColumn(employeeColActions), 0);

        var row = new Panel { Dock = DockStyle.Top, Height = 44, BackColor = Color.White };
        row.Controls.Add(grid);
        return row;
    }
}
