using Microsoft.Data.SqlClient;
using MyApp.Controls;

namespace MyApp;

internal sealed class AddUserDialog : DialogBase
{
    private static readonly UserRole[] Roles = Enum.GetValues<UserRole>();

    private readonly TextBox nameBox = new() { PlaceholderText = "Full name" };
    private readonly TextBox emailBox = new() { PlaceholderText = "Email" };
    private readonly TextBox passwordBox = new() { PlaceholderText = "At least 8 characters", UseSystemPasswordChar = true };
    private readonly PawSelect roleBox = new();

    public string? CreatedEmail { get; private set; }

    public AddUserDialog() : base("Add User", "Save", "\uE77B", "Create a login for an employee or admin")
    {
        roleBox.Items.AddRange(Roles.Select(r => (object)r.ToString()).ToArray());
        roleBox.SelectedIndex = 0;

        AddRow("Full name *", nameBox);
        AddRow("Email *", emailBox);
        AddRow("Password *", passwordBox);
        AddRow("Role", roleBox);
        FitHeight();
    }

    protected override void OnSaveClicked()
    {
        if (string.IsNullOrWhiteSpace(nameBox.Text) || string.IsNullOrWhiteSpace(emailBox.Text))
        {
            ShowError("Name and email are required.");
            return;
        }
        if (passwordBox.Text.Length < 8)
        {
            ShowError("Password must be at least 8 characters.");
            return;
        }

        try
        {
            Auth.CreateUser(nameBox.Text, emailBox.Text, passwordBox.Text, Roles[roleBox.SelectedIndex]);
            CreatedEmail = emailBox.Text.Trim();
            DialogResult = DialogResult.OK;
        }
        catch (SqlException ex) when (ex.Number is 2627 or 2601)
        {
            ShowError("That email is already used by another account.");
        }
        catch (SqlException ex)
        {
            ShowError("Could not save: " + ex.Message);
        }
    }
}
