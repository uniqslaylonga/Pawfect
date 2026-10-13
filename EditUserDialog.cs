using Microsoft.Data.SqlClient;
using MyApp.Controls;

namespace MyApp;

internal sealed class EditUserDialog : DialogBase
{
    private static readonly UserRole[] Roles = Enum.GetValues<UserRole>();

    private readonly int _userId;
    private readonly TextBox nameBox = new() { PlaceholderText = "Full name" };
    private readonly TextBox emailBox = new() { PlaceholderText = "Email" };
    private readonly PawSelect roleBox = new();

    public EditUserDialog(UserRepository.UserRow user) : base("Edit User", "Save", "\uE77B", user.Email)
    {
        _userId = user.Id;

        roleBox.Items.AddRange(Roles.Select(r => (object)r.ToString()).ToArray());
        roleBox.SelectedIndex = Enum.TryParse<UserRole>(user.Role, out var r) ? Array.IndexOf(Roles, r) : 0;
        nameBox.Text = user.Name;
        emailBox.Text = user.Email;

        AddRow("Full name *", nameBox);
        AddRow("Email *", emailBox);
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

        try
        {
            if (!UserRepository.Update(_userId, nameBox.Text, emailBox.Text, Roles[roleBox.SelectedIndex]))
            {
                ShowError("You can't change the role of the last active admin.");
                return;
            }
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
