namespace MyApp;

/// <summary>
/// Employee &amp; User Management screen (two sections: Employee Management and User Management).
/// Layout lives in EmployeeManagementPage.Designer.cs, so open this file's designer in Visual Studio to move or restyle anything.
/// Nothing is pre-filled: counts show 0 and the tables show their empty states until records are loaded.
/// </summary>
internal partial class EmployeeManagementPage : UserControl
{
    private List<UserRepository.UserRow> _allUsers = new();


    /// <summary>The "+ Add Employee" button was clicked.</summary>
    public event EventHandler? AddEmployeeRequested;

    /// <summary>The "+ Add User" button was clicked.</summary>
    public event EventHandler? AddUserRequested;

    /// <summary>The Activate / Deactivate link on a user row was clicked.</summary>
    public event Action<int, bool>? ToggleActiveRequested;
    public event Action<UserRepository.UserRow>? EditRequested;

    public EmployeeManagementPage()
    {
        InitializeComponent();
        DoubleBuffered = true;

        // Employee roles (Cashier, Groomer...) will be listed here once they exist in the database.
        employeeRoleFilter.SetItems(new[] { "All Roles" });
        // Account roles come straight from the UserRole enum, so this list can't drift from the database.
        userRoleFilter.SetItems(new[] { "All Roles" }.Concat(Enum.GetNames<UserRole>()));

        foreach (var filter in new[] { employeeStatusFilter, userStatusFilter })
            filter.SetItems(new[] { "All Status", "Active", "Inactive" });

        employeeAddButton.Click += (_, _) => AddEmployeeRequested?.Invoke(this, EventArgs.Empty);
        userAddButton.Click += (_, _) => AddUserRequested?.Invoke(this, EventArgs.Empty);
        userSearch.Box.TextChanged += (_, _) => ApplyUserFilter();
    }

    /// <summary>Fills the User Management table, tiles and footer from the database rows.</summary>
    /// <summary>Stores the full list from the database, then shows it through the current filters.</summary>
    public void LoadUsers(List<UserRepository.UserRow> users)
    {
        _allUsers = users;
        ApplyUserFilter();
    }

    private void ApplyUserFilter()
    {
        string q = userSearch.Value.Trim();
        var shown = _allUsers.Where(u =>
            q.Length == 0 ||
            u.Name.Contains(q, StringComparison.OrdinalIgnoreCase) ||
            u.Email.Contains(q, StringComparison.OrdinalIgnoreCase)).ToList();

        userRows.SuspendLayout();

        foreach (var old in userRows.Controls.Cast<Control>().Where(c => c != userEmpty).ToList())
        {
            userRows.Controls.Remove(old);
            old.Dispose();
        }

        userEmpty.Visible = shown.Count == 0;

        for (int i = shown.Count - 1; i >= 0; i--)
            userRows.Controls.Add(BuildUserRow(shown[i]));

        // Tiles always show the totals for everyone; the footer shows what is visible.
        userTotalValue.Text = _allUsers.Count.ToString();
        userAdminValue.Text = _allUsers.Count(u => u.Role == "Admin").ToString();
        userStaffValue.Text = _allUsers.Count(u => u.Role == "Employee").ToString();
        userShowing.Text = shown.Count == 0
            ? "No users"
            : $"Showing 1 to {shown.Count} of {_allUsers.Count} users";

        userRows.ResumeLayout(true);
    }

    private Control BuildUserRow(UserRepository.UserRow u)
    {
        var grid = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.White,
            Margin = Padding.Empty,
            ColumnCount = userColumnsGrid.ColumnCount,
            RowCount = 1,
        };
        // Same column widths as the header, so cells line up under their titles.
        foreach (ColumnStyle s in userColumnsGrid.ColumnStyles)
            grid.ColumnStyles.Add(new ColumnStyle(s.SizeType, s.Width));
        grid.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

        void Put(Control header, string text, Color? color = null, bool link = false)
        {
            var cell = new Label
            {
                Text = text,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                AutoEllipsis = true,
                UseMnemonic = false,
                BackColor = Color.White,
                ForeColor = color ?? Color.FromArgb(31, 42, 60),
                Font = new Font("Segoe UI", 9F, link ? FontStyle.Bold : FontStyle.Regular),
                Cursor = link ? Cursors.Hand : Cursors.Default,
            };
            if (link) cell.Click += (_, _) => ToggleActiveRequested?.Invoke(u.Id, !u.Active);
            grid.Controls.Add(cell, userColumnsGrid.GetColumn(header), 0);
        }

        Put(userColId, u.Id.ToString());
        Put(userColUsername, u.Email.Split('@')[0]);
        Put(userColFullName, u.Name);
        Put(userColRole, u.Role);
        Put(userColEmail, u.Email);
        Put(userColStatus, u.Active ? "Active" : "Inactive",
            u.Active ? Color.FromArgb(46, 158, 91) : Color.FromArgb(122, 136, 156));
        Put(userColLastLogin, "—", Color.FromArgb(122, 136, 156));
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
        actions.Controls.Add(Link("Edit", () => EditRequested?.Invoke(u)));
        actions.Controls.Add(Link(u.Active ? "Deactivate" : "Activate",
            () => ToggleActiveRequested?.Invoke(u.Id, !u.Active)));
        grid.Controls.Add(actions, userColumnsGrid.GetColumn(userColActions), 0);

        var row = new Panel { Dock = DockStyle.Top, Height = 44, BackColor = Color.White };
        row.Controls.Add(grid);
        return row;
    }
}
