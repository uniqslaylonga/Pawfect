namespace MyApp;

/// <summary>
/// Employee &amp; User Management screen (two sections: Employee Management and User Management).
/// Layout lives in EmployeeManagementPage.Designer.cs, so open this file's designer in Visual Studio to move or restyle anything.
/// Nothing is pre-filled: counts show 0 and the tables show their empty states until records are loaded.
/// </summary>
internal partial class EmployeeManagementPage : UserControl
{
    /// <summary>The "+ Add Employee" button was clicked.</summary>
    public event EventHandler? AddEmployeeRequested;

    /// <summary>The "+ Add User" button was clicked.</summary>
    public event EventHandler? AddUserRequested;

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
    }
}
