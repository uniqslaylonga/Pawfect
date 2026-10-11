using Microsoft.Data.SqlClient;

namespace MyApp;

/// <summary>Shown once, on the very first run, to create the first admin account.</summary>
internal partial class CreateAdminForm : Form
{
    public CreateAdminForm()
    {
        InitializeComponent();
    }

    private void createButton_Click(object? sender, EventArgs e)
    {
        string name = nameBox.Text.Trim();
        string email = emailBox.Text.Trim();

        if (name.Length == 0) { errorLabel.Text = "Enter a name."; return; }
        if (!email.Contains('@')) { errorLabel.Text = "Enter a valid email address."; return; }
        if (passwordBox.Text.Length < 8) { errorLabel.Text = "Password must be at least 8 characters."; return; }
        if (passwordBox.Text != confirmBox.Text) { errorLabel.Text = "The passwords don't match."; return; }

        try
        {
            Auth.CreateUser(name, email, passwordBox.Text, UserRole.Admin);
        }
        catch (SqlException ex) when (ex.Number is 2627 or 2601)
        {
            errorLabel.Text = "That email is already registered.";
            return;
        }
        catch (SqlException)
        {
            errorLabel.Text = "Can't reach the database.";
            return;
        }

        DialogResult = DialogResult.OK;
        Close();
    }
}
