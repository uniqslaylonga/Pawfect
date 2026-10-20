using Microsoft.Data.SqlClient;
using MyApp.Controls;

namespace MyApp;

/// <summary>Add Employee / Edit Employee. Pass an existing row to edit it.</summary>
internal sealed class EmployeeDialog : DialogBase
{
    private readonly EmployeeRepository.Row? _existing;
    private readonly TextBox nameBox = new() { PlaceholderText = "Full name", MaxLength = 100 };
    private readonly PawSelect roleBox = new();
    private readonly TextBox departmentBox = new() { PlaceholderText = "Department", MaxLength = 60 };
    private readonly TextBox emailBox = new() { PlaceholderText = "Email (optional)", MaxLength = 256 };
    private readonly TextBox phoneBox = new() { PlaceholderText = "Mobile number (optional)", MaxLength = 30 };
    private string _lastDefaultDepartment = "";

    /// <summary>What was saved (set after a successful Save).</summary>
    public string SavedName { get; private set; } = "";
    public string SavedRole { get; private set; } = "";

    public EmployeeDialog(EmployeeRepository.Row? existing = null)
        : base(existing is null ? "Add Employee" : "Edit Employee", "Save", "\uE77B",
               existing is null ? "Add someone to the staff list" : existing.Name)
    {
        _existing = existing;

        roleBox.Items.AddRange(EmployeeRepository.Roles.Select(r => (object)r).ToArray());
        int index = existing is null ? Array.IndexOf(EmployeeRepository.Roles, "Cashier") : Array.IndexOf(EmployeeRepository.Roles, existing.Role);
        roleBox.SelectedIndex = index < 0 ? 0 : index;

        if (existing is not null)
        {
            nameBox.Text = existing.Name;
            departmentBox.Text = existing.Department;
            emailBox.Text = existing.Email;
            phoneBox.Text = existing.Phone;
        }
        else
        {
            departmentBox.Text = EmployeeRepository.DefaultDepartment(SelectedRole);
        }
        _lastDefaultDepartment = EmployeeRepository.DefaultDepartment(SelectedRole);

        // Changing the role fills in that role's usual department, unless the department was typed by hand.
        roleBox.SelectedIndexChanged += (_, _) =>
        {
            string next = EmployeeRepository.DefaultDepartment(SelectedRole);
            if (departmentBox.Text.Trim().Length == 0 || departmentBox.Text.Trim() == _lastDefaultDepartment)
                departmentBox.Text = next;
            _lastDefaultDepartment = next;
        };

        AddRow("Full name *", nameBox);
        AddRow("Role *", roleBox);
        AddRow("Department", departmentBox);
        AddRow("Email", emailBox);
        AddRow("Mobile", phoneBox);
        FitHeight();
    }

    private string SelectedRole => roleBox.SelectedIndex >= 0 && roleBox.SelectedIndex < EmployeeRepository.Roles.Length
        ? EmployeeRepository.Roles[roleBox.SelectedIndex]
        : EmployeeRepository.Roles[0];

    protected override void OnSaveClicked()
    {
        string name = nameBox.Text.Trim();
        if (name.Length == 0)
        {
            ShowError("Please enter the employee's full name.");
            return;
        }

        string email = emailBox.Text.Trim();
        if (email.Length > 0 && !IsValidEmail(email))
        {
            ShowError("That email address doesn't look right.");
            return;
        }

        try
        {
            string role = SelectedRole;
            if (_existing is null)
                EmployeeRepository.Add(name, role, departmentBox.Text, email, phoneBox.Text);
            else if (!EmployeeRepository.Update(_existing.Id, name, role, departmentBox.Text, email, phoneBox.Text))
            {
                ShowError("That employee no longer exists.");
                return;
            }

            SavedName = name;
            SavedRole = role;
            DialogResult = DialogResult.OK;
        }
        catch (SqlException ex)
        {
            ShowError("Could not save: " + ex.Message);
        }
    }

    private static bool IsValidEmail(string email)
    {
        try { return new System.Net.Mail.MailAddress(email).Address == email; }
        catch (FormatException) { return false; }
    }
}
