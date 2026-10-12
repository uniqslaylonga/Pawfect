using Microsoft.Data.SqlClient;
using MyApp.Controls;

namespace MyApp;

/// <summary>
/// "My Profile": opened by clicking the name chip in the top bar (employee and admin screens).
/// The signed-in person can edit their own name and email, and optionally change their password.
/// Role is shown but can't be changed here (admins change roles from Employee &amp; User Management).
/// </summary>
internal sealed class ProfileDialog : DialogBase
{
    private readonly string _currentEmail;
    private readonly TextBox _name = new();
    private readonly TextBox _email = new();
    private readonly TextBox _role = new() { ReadOnly = true, TabStop = false, BackColor = Theme.Subtle };
    private readonly TextBox _currentPassword = new() { UseSystemPasswordChar = true, PlaceholderText = "Only to change password" };
    private readonly TextBox _newPassword = new() { UseSystemPasswordChar = true, PlaceholderText = "Leave blank to keep current" };
    private readonly TextBox _confirmPassword = new() { UseSystemPasswordChar = true, PlaceholderText = "Repeat new password" };

    private readonly IconBadge _photo = new("\uE77B", Theme.PrimarySoft, Theme.Primary, Color.White, 72, circle: true, glyphPt: 24f);
    private byte[]? _avatarBytes;
    private bool _avatarChanged;

    /// <summary>The account as saved. Only valid when the dialog returned OK.</summary>
    public UserAccount? UpdatedUser { get; private set; }

    /// <summary>The profile picture after saving (PNG bytes, null = none). Only valid when the dialog returned OK.</summary>
    public byte[]? UpdatedAvatar => _avatarBytes;

    private readonly UserRole _userRole;

    public ProfileDialog(UserAccount user, byte[]? avatar) : base("My Profile", "Save changes")
    {
        _currentEmail = user.Email;
        _userRole = user.Role;
        _avatarBytes = avatar;
        AvatarImages.Show(_photo, avatar);

        AddPhotoRow();

        _name.Text = user.Name;
        _email.Text = user.Email;
        _role.Text = user.Role.ToString();

        AddRow("Full name *", _name);
        AddRow("Email *", _email);
        AddRow("Role", _role);

        var divider = new Label
        {
            Text = "Change password (optional)",
            AutoSize = false,
            Height = 30,
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.BottomLeft,
            ForeColor = Theme.Muted,
            Font = Theme.Face(8.5f, FontStyle.Bold),
            Margin = new Padding(0, 10, 0, 0),
        };
        Grid.Controls.Add(divider);
        Grid.SetColumnSpan(divider, 2);

        AddRow("Current password", _currentPassword);
        AddRow("New password", _newPassword);
        AddRow("Confirm", _confirmPassword);

        FitHeight();
    }

    /// <summary>The picture preview with "Change photo" and "Remove" buttons.</summary>
    private void AddPhotoRow()
    {
        var host = new Panel { Height = 88, Dock = DockStyle.Fill, Margin = new Padding(0, 0, 0, 6), BackColor = Color.White };

        _photo.Location = new Point(0, 8);
        host.Controls.Add(_photo);

        var change = MakeLinkButton("Change photo...", 92, 16);
        var remove = MakeLinkButton("Remove photo", 92, 50);
        change.Click += (_, _) => ChoosePhoto();
        remove.Click += (_, _) =>
        {
            _avatarBytes = null;
            _avatarChanged = true;
            AvatarImages.Show(_photo, null);
        };
        host.Controls.Add(change);
        host.Controls.Add(remove);

        Grid.Controls.Add(host);
        Grid.SetColumnSpan(host, 2);
    }

    private static Button MakeLinkButton(string text, int x, int y) => new()
    {
        Text = text,
        Location = new Point(x, y),
        Size = new Size(130, 28),
        FlatStyle = FlatStyle.Flat,
        BackColor = Color.White,
        ForeColor = Theme.Primary,
        Cursor = Cursors.Hand,
        FlatAppearance = { BorderColor = Theme.Border },
    };

    private void ChoosePhoto()
    {
        using var open = new OpenFileDialog
        {
            Title = "Choose a profile picture",
            Filter = "Pictures (*.png;*.jpg;*.jpeg;*.bmp;*.gif)|*.png;*.jpg;*.jpeg;*.bmp;*.gif",
            CheckFileExists = true,
        };
        if (open.ShowDialog(this) != DialogResult.OK) return;

        var png = AvatarImages.PrepareFromFile(open.FileName, out var error);
        if (png is null)
        {
            ShowError(error ?? "That file couldn't be used.");
            return;
        }

        ShowError("");
        _avatarBytes = png;
        _avatarChanged = true;
        AvatarImages.Show(_photo, png);
    }

    protected override void OnSaveClicked()
    {
        ShowError("");
        string name = _name.Text.Trim();
        string email = _email.Text.Trim();

        if (name.Length == 0) { ShowError("Enter your name."); return; }
        if (email.Length == 0 || !email.Contains('@')) { ShowError("Enter a valid email."); return; }

        string? newPassword = null;
        if (_newPassword.Text.Length > 0 || _confirmPassword.Text.Length > 0)
        {
            if (_currentPassword.Text.Length == 0) { ShowError("Enter your current password."); return; }
            if (_newPassword.Text.Length < 8) { ShowError("New password must be at least 8 characters."); return; }
            if (_newPassword.Text != _confirmPassword.Text) { ShowError("The new passwords don't match."); return; }
            newPassword = _newPassword.Text;
        }

        try
        {
            switch (UserRepository.UpdateOwnProfile(_currentEmail, name, email, _currentPassword.Text, newPassword))
            {
                case UserRepository.ProfileResult.Saved:
                    if (_avatarChanged) UserRepository.SetAvatar(email, _avatarBytes);
                    UpdatedUser = new UserAccount(name, email, _userRole);
                    DialogResult = DialogResult.OK;
                    break;
                case UserRepository.ProfileResult.WrongPassword:
                    ShowError("Current password is incorrect.");
                    break;
                default:
                    ShowError("Your account could not be found. Please sign in again.");
                    break;
            }
        }
        catch (SqlException ex) when (ex.Number is 2627 or 2601)
        {
            ShowError("That email is already used by another account.");
        }
        catch (SqlException ex)
        {
            ShowDatabaseError(this, ex);
        }
    }
}
