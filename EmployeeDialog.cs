using Microsoft.Data.SqlClient;
using MyApp.Controls;

namespace MyApp;

internal sealed class EmployeeDialog : ThemedDialog
{
    private readonly EmployeeRepository.EmployeeRow? _existing;
    private readonly InputField nameBox, emailBox, usernameBox, passwordBox;
    private readonly ComboBox positionBox;
    private readonly DateTimePicker startPicker;

    /// <summary>Name of the saved employee, so AdminForm can write it to the audit log.</summary>
    public string? SavedName { get; private set; }

    public EmployeeDialog(EmployeeRepository.EmployeeRow? existing = null)
        : base(existing == null ? "Add Employee" : "Edit Employee",
               existing == null ? "Add a staff member and, optionally, their login."
                                : "Update this employee's details and login.")
    {
        _existing = existing;

        nameBox = AddInput("Full name", "\uE77B", "e.g. Juan Dela Cruz", existing?.Name);
        positionBox = AddCombo("Role", EmployeeRepository.Positions, existing?.Position);
        startPicker = AddDate("Start date", existing?.StartDate ?? DateTime.Today);
        emailBox = AddInput("Email", "\uE715", "name@example.com", existing?.Email);
        usernameBox = AddInput("Username (optional, creates a login)", "\uE77B", "e.g. gabriel.e", existing?.Username);
        passwordBox = AddInput("Password", "\uE72E",
            existing?.UserId != null ? "Leave blank to keep current password" : "Min 8 characters",
            null, password: true);

        AddButtons(existing == null ? "Add Employee" : "Save Changes", Save_Click);
    }

    private void Save_Click(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(nameBox.Value) || string.IsNullOrWhiteSpace(emailBox.Value))
        {
            MessageBox.Show(this, "Name and email are required.");
            return;
        }

        string username = usernameBox.Value.Trim();
        string password = passwordBox.Value;
        bool hasAccount = _existing?.UserId != null;

        if (username.Length == 0 && password.Length > 0)
        {
            MessageBox.Show(this, "Enter a username for the login, or clear the password.");
            return;
        }
        if (username.Length > 0 && !hasAccount && password.Length == 0)
        {
            MessageBox.Show(this, "A password is required to create the login.");
            return;
        }
        if (password.Length > 0 && password.Length < 8)
        {
            MessageBox.Show(this, "Password must be at least 8 characters.");
            return;
        }

        string position = (string)positionBox.SelectedItem!;
        try
        {
            if (_existing == null)
                EmployeeRepository.Add(nameBox.Value, position, startPicker.Value, emailBox.Value, username, password);
            else
                EmployeeRepository.Update(_existing.Id, nameBox.Value, position, startPicker.Value,
                                          emailBox.Value, username, password);

            SavedName = nameBox.Value.Trim();
            DialogResult = DialogResult.OK;
        }
        catch (InvalidOperationException ex)
        {
            MessageBox.Show(this, ex.Message);
        }
        catch (SqlException ex) when (ex.Number is 2627 or 2601)
        {
            MessageBox.Show(this, "That email or username is already used by another account.");
        }
        catch (SqlException ex)
        {
            MessageBox.Show(this, "Could not save: " + ex.Message);
        }
    }
}