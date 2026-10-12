using Microsoft.Data.SqlClient;

namespace MyApp;

internal sealed class EditUserDialog : Form
{
    private readonly int _userId;
    private readonly TextBox nameBox = new() { PlaceholderText = "Full name" };
    private readonly TextBox emailBox = new() { PlaceholderText = "Email" };
    private readonly ComboBox roleBox = new() { DropDownStyle = ComboBoxStyle.DropDownList };

    public EditUserDialog(UserRepository.UserRow user)
    {
        _userId = user.Id;
        Text = "Edit User";
        FormBorderStyle = FormBorderStyle.FixedDialog;
        StartPosition = FormStartPosition.CenterParent;
        MaximizeBox = MinimizeBox = false;
        ClientSize = new Size(320, 200);

        roleBox.DataSource = Enum.GetValues<UserRole>();
        nameBox.Text = user.Name;
        emailBox.Text = user.Email;
        if (Enum.TryParse<UserRole>(user.Role, out var r)) roleBox.SelectedItem = r;

        var save = new Button { Text = "Save" };
        var cancel = new Button { Text = "Cancel", DialogResult = DialogResult.Cancel };
        save.Click += Save_Click;
        AcceptButton = save;
        CancelButton = cancel;

        int y = 16;
        foreach (Control c in new Control[] { nameBox, emailBox, roleBox })
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

        try
        {
            if (!UserRepository.Update(_userId, nameBox.Text, emailBox.Text, (UserRole)roleBox.SelectedItem!))
            {
                MessageBox.Show("You can't change the role of the last active admin.");
                return;
            }
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