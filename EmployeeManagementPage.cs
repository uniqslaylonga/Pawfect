using System.Globalization;

namespace MyApp;

/// <summary>
/// Employee &amp; User Management screen (two sections: Employee Management and User Management).
/// Layout lives in EmployeeManagementPage.Designer.cs, so open this file's designer in Visual Studio to move or restyle anything.
/// Everything shown comes from the database; with no records the tables show their empty states and the counts show 0.
/// </summary>
internal partial class EmployeeManagementPage : UserControl
{
    private static readonly Color Ink = Color.FromArgb(31, 42, 60);
    private static readonly Color Quiet = Color.FromArgb(122, 136, 156);
    private static readonly Color Good = Color.FromArgb(46, 158, 91);
    private static readonly Color Link = Color.FromArgb(58, 123, 213);

    private List<UserRepository.UserRow> _allUsers = new();
    private List<EmployeeRepository.Row> _allEmployees = new();

    /// <summary>The "+ Add Employee" button was clicked.</summary>
    public event EventHandler? AddEmployeeRequested;

    /// <summary>The Edit link on an employee row was clicked.</summary>
    public event Action<EmployeeRepository.Row>? EditEmployeeRequested;

    /// <summary>The Activate / Deactivate link on an employee row was clicked.</summary>
    public event Action<int, bool>? ToggleEmployeeActiveRequested;

    /// <summary>The "+ Add User" button was clicked.</summary>
    public event EventHandler? AddUserRequested;

    /// <summary>The Activate / Deactivate link on a user row was clicked.</summary>
    public event Action<int, bool>? ToggleActiveRequested;
    public event Action<UserRepository.UserRow>? EditRequested;

    public EmployeeManagementPage()
    {
        InitializeComponent();
        DoubleBuffered = true;

        // Employee roles and account roles come straight from the code lists, so the filters can't drift from the database.
        employeeRoleFilter.SetItems(new[] { "All Roles" }.Concat(EmployeeRepository.Roles));
        userRoleFilter.SetItems(new[] { "All Roles" }.Concat(Enum.GetNames<UserRole>()));

        foreach (var filter in new[] { employeeStatusFilter, userStatusFilter })
            filter.SetItems(new[] { "All Status", "Active", "Inactive" });

        employeeAddButton.Click += (_, _) => AddEmployeeRequested?.Invoke(this, EventArgs.Empty);
        userAddButton.Click += (_, _) => AddUserRequested?.Invoke(this, EventArgs.Empty);

        employeeSearch.Box.TextChanged += (_, _) => ApplyEmployeeFilter();
        employeeRoleFilter.SelectedIndexChanged += (_, _) => ApplyEmployeeFilter();
        employeeStatusFilter.SelectedIndexChanged += (_, _) => ApplyEmployeeFilter();

        userSearch.Box.TextChanged += (_, _) => ApplyUserFilter();
        userRoleFilter.SelectedIndexChanged += (_, _) => ApplyUserFilter();
        userStatusFilter.SelectedIndexChanged += (_, _) => ApplyUserFilter();
    }

    // ------------------------------------------------------------------ shared helpers

    /// <summary>"+3 vs. last month" (how many were added since the 1st of this month) or "No change vs. last month".</summary>
    private static string AddedText(int addedThisMonth)
        => addedThisMonth <= 0 ? "No change vs. last month" : $"+{addedThisMonth.ToString(CultureInfo.CurrentCulture)} vs. last month";

    private static DateTime MonthStart
    {
        get { var now = DateTime.Now; return new DateTime(now.Year, now.Month, 1); }
    }

    /// <summary>True when the "All Status / Active / Inactive" drop-down lets this record through.</summary>
    private static bool StatusMatches(int selectedIndex, bool active)
        => selectedIndex <= 0 || (selectedIndex == 1 && active) || (selectedIndex == 2 && !active);

    private static TableLayoutPanel NewRowGrid(TableLayoutPanel head)
    {
        var grid = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.White,
            Margin = Padding.Empty,
            ColumnCount = head.ColumnCount,
            RowCount = 1,
        };
        // Same column widths as the header, so cells line up under their titles.
        foreach (ColumnStyle s in head.ColumnStyles)
            grid.ColumnStyles.Add(new ColumnStyle(s.SizeType, s.Width));
        grid.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        return grid;
    }

    private static Label NewCell(string text, Color? color = null)
        => new()
        {
            Text = text,
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleLeft,
            AutoEllipsis = true,
            UseMnemonic = false,
            BackColor = Color.White,
            ForeColor = color ?? Ink,
            Font = new Font("Segoe UI", 9F),
        };

    private static FlowLayoutPanel NewActions(params (string Text, Action OnClick)[] links)
    {
        var actions = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.White,
            WrapContents = false,
            Margin = Padding.Empty,
            Padding = new Padding(0, 14, 0, 0),
        };
        foreach (var (text, onClick) in links)
        {
            var l = new Label
            {
                Text = text,
                AutoSize = true,
                Margin = new Padding(0, 0, 14, 0),
                BackColor = Color.White,
                ForeColor = Link,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Cursor = Cursors.Hand,
                UseMnemonic = false,
            };
            var handler = onClick;
            l.Click += (_, _) => handler();
            actions.Controls.Add(l);
        }
        return actions;
    }

    private static Panel WrapRow(TableLayoutPanel grid)
    {
        var row = new Panel { Dock = DockStyle.Top, Height = 44, BackColor = Color.White };
        row.Controls.Add(grid);
        return row;
    }

    /// <summary>Removes the old rows from a rows panel (everything except its empty-state message).</summary>
    private static void ClearRows(Panel host, Control empty)
    {
        foreach (var old in host.Controls.Cast<Control>().Where(c => c != empty).ToList())
        {
            host.Controls.Remove(old);
            old.Dispose();
        }
    }

    // ------------------------------------------------------------------ employees

    /// <summary>Stores the full employee list from the database, then shows it through the current filters.</summary>
    public void LoadEmployees(List<EmployeeRepository.Row> employees)
    {
        _allEmployees = employees;
        ApplyEmployeeFilter();
    }

    private void ApplyEmployeeFilter()
    {
        string q = employeeSearch.Value.Trim();
        string? role = employeeRoleFilter.SelectedIndex > 0 ? employeeRoleFilter.SelectedItem : null;

        var shown = _allEmployees.Where(e =>
            (role is null || e.Role == role) &&
            StatusMatches(employeeStatusFilter.SelectedIndex, e.Active) &&
            (q.Length == 0 ||
             e.Name.Contains(q, StringComparison.OrdinalIgnoreCase) ||
             e.Email.Contains(q, StringComparison.OrdinalIgnoreCase) ||
             e.Department.Contains(q, StringComparison.OrdinalIgnoreCase) ||
             e.Role.Contains(q, StringComparison.OrdinalIgnoreCase))).ToList();

        employeeRows.SuspendLayout();
        ClearRows(employeeRows, employeeEmpty);
        employeeEmpty.Visible = shown.Count == 0;
        for (int i = shown.Count - 1; i >= 0; i--)
            employeeRows.Controls.Add(BuildEmployeeRow(shown[i]));

        // Tiles always count everyone; the footer shows what is visible.
        var culture = CultureInfo.CurrentCulture;
        DateTime since = MonthStart;
        void Tile(Label value, Label delta, Func<EmployeeRepository.Row, bool> match)
        {
            value.Text = _allEmployees.Count(match).ToString(culture);
            delta.Text = AddedText(_allEmployees.Count(e => match(e) && e.CreatedAt >= since));
        }
        Tile(employeeTotalValue, employeeTotalDelta, _ => true);
        Tile(employeeAdminValue, employeeAdminDelta, e => e.Role == "Admin");
        Tile(employeeCashierValue, employeeCashierDelta, e => e.Role == "Cashier");
        Tile(employeeFrontDeskValue, employeeFrontDeskDelta, e => e.Role == "Front Desk");
        Tile(employeeGroomersValue, employeeGroomersDelta, e => e.Role == "Groomer");

        employeeShowing.Text = shown.Count == 0
            ? "No employees"
            : $"Showing 1 to {shown.Count} of {_allEmployees.Count} employees";

        employeeRows.ResumeLayout(true);
    }

    private Control BuildEmployeeRow(EmployeeRepository.Row e)
    {
        var grid = NewRowGrid(employeeColumnsGrid);

        void Put(Control header, string text, Color? color = null)
            => grid.Controls.Add(NewCell(text, color), employeeColumnsGrid.GetColumn(header), 0);

        Put(employeeColId, e.Id.ToString(CultureInfo.CurrentCulture));
        Put(employeeColName, e.Name);
        Put(employeeColRole, e.Role);
        Put(employeeColDepartment, e.Department.Length == 0 ? "\u2014" : e.Department, e.Department.Length == 0 ? Quiet : null);
        Put(employeeColEmail, e.Email.Length == 0 ? "\u2014" : e.Email, e.Email.Length == 0 ? Quiet : null);
        Put(employeeColStatus, e.Active ? "Active" : "Inactive", e.Active ? Good : Quiet);

        grid.Controls.Add(NewActions(
            ("Edit", () => EditEmployeeRequested?.Invoke(e)),
            (e.Active ? "Deactivate" : "Activate", () => ToggleEmployeeActiveRequested?.Invoke(e.Id, !e.Active))),
            employeeColumnsGrid.GetColumn(employeeColActions), 0);

        return WrapRow(grid);
    }

    // ------------------------------------------------------------------ users

    /// <summary>Stores the full list from the database, then shows it through the current filters.</summary>
    public void LoadUsers(List<UserRepository.UserRow> users)
    {
        _allUsers = users;
        ApplyUserFilter();
    }

    private void ApplyUserFilter()
    {
        string q = userSearch.Value.Trim();
        string? role = userRoleFilter.SelectedIndex > 0 ? userRoleFilter.SelectedItem : null;

        var shown = _allUsers.Where(u =>
            (role is null || u.Role == role) &&
            StatusMatches(userStatusFilter.SelectedIndex, u.Active) &&
            (q.Length == 0 ||
             u.Name.Contains(q, StringComparison.OrdinalIgnoreCase) ||
             u.Email.Contains(q, StringComparison.OrdinalIgnoreCase))).ToList();

        userRows.SuspendLayout();
        ClearRows(userRows, userEmpty);
        userEmpty.Visible = shown.Count == 0;
        for (int i = shown.Count - 1; i >= 0; i--)
            userRows.Controls.Add(BuildUserRow(shown[i]));

        // Tiles always count everyone; the footer shows what is visible. Users.CreatedAt is stored in UTC.
        var culture = CultureInfo.CurrentCulture;
        DateTime since = MonthStart.ToUniversalTime();
        void Tile(Label value, Label delta, Func<UserRepository.UserRow, bool> match)
        {
            value.Text = _allUsers.Count(match).ToString(culture);
            delta.Text = AddedText(_allUsers.Count(u => match(u) && u.CreatedAt >= since));
        }
        Tile(userTotalValue, userTotalDelta, _ => true);
        Tile(userAdminValue, userAdminDelta, u => u.Role == "Admin");
        Tile(userStaffValue, userStaffDelta, u => u.Role == "Employee");
        Tile(userCustomerValue, userCustomerDelta, u => u.Role == "Customer");

        userShowing.Text = shown.Count == 0
            ? "No users"
            : $"Showing 1 to {shown.Count} of {_allUsers.Count} users";

        userRows.ResumeLayout(true);
    }

    private Control BuildUserRow(UserRepository.UserRow u)
    {
        var grid = NewRowGrid(userColumnsGrid);

        void Put(Control header, string text, Color? color = null)
            => grid.Controls.Add(NewCell(text, color), userColumnsGrid.GetColumn(header), 0);

        Put(userColId, u.Id.ToString(CultureInfo.CurrentCulture));
        Put(userColUsername, u.Email.Split('@')[0]);
        Put(userColFullName, u.Name);
        Put(userColRole, u.Role);
        Put(userColEmail, u.Email);
        Put(userColStatus, u.Active ? "Active" : "Inactive", u.Active ? Good : Quiet);
        Put(userColLastLogin,
            u.LastLogin is { } login ? DateTime.SpecifyKind(login, DateTimeKind.Utc).ToLocalTime().ToString("MMM d, h:mm tt", CultureInfo.CurrentCulture) : "\u2014",
            u.LastLogin is null ? Quiet : null);

        grid.Controls.Add(NewActions(
            ("Edit", () => EditRequested?.Invoke(u)),
            (u.Active ? "Deactivate" : "Activate", () => ToggleActiveRequested?.Invoke(u.Id, !u.Active))),
            userColumnsGrid.GetColumn(userColActions), 0);

        return WrapRow(grid);
    }
}
