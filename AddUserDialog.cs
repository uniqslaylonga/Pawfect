using Microsoft.Data.SqlClient;

namespace MyApp;

internal sealed class AddUserDialog : Form
{
    private readonly TextBox nameBox = new() { PlaceholderText = "Full name" };
    private readonly TextBox emailBox = new() { PlaceholderText = "Email" };
    private readonly TextBox passwordBox = new() { PlaceholderText = "Password (min 8 characters)", UseSystemPasswordChar = true };
    private readonly ComboBox roleBox = new() { DropDownStyle = ComboBoxStyle.DropDownList };

    public string? CreatedEmail { get; private set; }

    public AddUserDialog()
    {
        Text = "Add User";
        FormBorderStyle = FormBorderStyle.FixedDialog;
        StartPosition = FormStartPosition.CenterParent;
        MaximizeBox = MinimizeBox = false;
        ClientSize = new Size(320, 240);

        roleBox.DataSource = Enum.GetValues<UserRole>();

        var save = new Button { Text = "Save", DialogResult = DialogResult.None };
        var cancel = new Button { Text = "Cancel", DialogResult = DialogResult.Cancel };
        save.Click += Save_Click;
        AcceptButton = save;
        CancelButton = cancel;

        int y = 16;
        foreach (Control c in new Control[] { nameBox, emailBox, passwordBox, roleBox })
        {
            c.SetBounds(16, y, 288, 28);
            Controls.Add(c);
            y += 40;
        }
        save.SetBounds(124, y + 8, 85, 32);
        cancel.SetBounds(219, y + 8, 85, 32);
        Controls.Add(save);
        Controls.Add(cancel);
    }

    private void Save_Click(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(nameBox.Text) || string.IsNullOrWhiteSpace(emailBox.Text))
        {
            MessageBox.Show("Name and email are required.");
            return;
        }
        if (passwordBox.Text.Length < 8)
        {
            MessageBox.Show("Password must be at least 8 characters.");
            return;
        }

        try
        {
            Auth.CreateUser(nameBox.Text, emailBox.Text, passwordBox.Text, (UserRole)roleBox.SelectedItem!);
            CreatedEmail = emailBox.Text.Trim();
            DialogResult = DialogResult.OK;
        }
        catch (SqlException ex) when (ex.Number is 2627 or 2601)
        {
            MessageBox.Show("That email is already used by another account.");
        }
        catch (SqlException ex)
        {
            MessageBox.Show("Could not save: " + ex.Message);
        }
    }
}