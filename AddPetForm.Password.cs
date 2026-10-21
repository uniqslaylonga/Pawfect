using Microsoft.Data.SqlClient;
using MyApp.Controls;

namespace MyApp;

/// <summary>
/// The second page of step 1 (Contact): after the emailed code is right, a new customer creates a password so they can
/// sign in next time. It is still the Contact step (the step bar keeps "Contact" active); built in code, like the other steps.
/// </summary>
internal sealed partial class AddPetForm
{
    private const int MinPasswordLength = 8;

    private Panel? _passwordPanel;
    private InputField? _passwordField;
    private InputField? _confirmField;
    private Label? _passwordError;
    private PrimaryButton? _passwordButton;

    private bool PasswordStepVisible => _passwordPanel is { Visible: true };

    /// <summary>Only customers without a sign-in yet are asked for a password (a returning one already has it).</summary>
    private bool NeedsPassword()
    {
        try { return !EmployeeData.CustomerHasLogin(_customerId); }
        catch (SqlException) { return false; }   // can't tell: don't block the visit
    }

    private void ShowPasswordStep() => PageFade.Swap(ContentHost, ShowPasswordStepCore);

    private void ShowPasswordStepCore()
    {
        EnsurePasswordPanel();
        resendTimer.Stop();
        _passwordField!.Box.Clear();
        _confirmField!.Box.Clear();
        _passwordError!.Text = "";

        HideVaccineStep();
        selectPanel.Visible = false;
        continueButton.Visible = false;
        contactPanel.Visible = true;
        _passwordPanel!.Visible = true;
        _passwordPanel.BringToFront();

        titleLabel.Text = "Create Your Password";
        subtitleLabel.Text = "Choose a password so you can sign in next time.";
        stepContact.State = StepState.Active;   // still the Contact step
        stepSelect.State = StepState.Pending;
        _passwordField.Box.Focus();
    }

    private void EnsurePasswordPanel()
    {
        if (_passwordPanel is not null) return;

        _passwordPanel = new Panel { BackColor = Theme.Bg, Location = new Point(0, 0), Size = contactPanel.Size, Visible = false };

        const int x = 260, w = 380;   // centred in the 900 px design width
        Label Caption(string text, int y) => new()
        {
            Text = text, BackColor = Theme.Bg, ForeColor = Color.FromArgb(68, 83, 106), AutoSize = false,
            Font = new Font("Segoe UI", 7.5F, FontStyle.Bold), Location = new Point(x, y), Size = new Size(w, 16),
            TextAlign = ContentAlignment.MiddleLeft, UseMnemonic = false,
        };
        InputField Field(string placeholder, int y) => new()
        {
            BorderColor = Theme.Border, CornerColor = Theme.Bg, FillColor = Color.White, Glyph = "",
            Placeholder = placeholder, Radius = 12, IsPassword = true, Margin = Padding.Empty,
            Padding = new Padding(10, 3, 10, 3), Location = new Point(x, y), Size = new Size(w, 46),
        };

        _passwordField = Field("At least 8 characters", 22);
        _confirmField = Field("Re-enter your password", 90);
        _passwordError = new Label
        {
            BackColor = Theme.Bg, ForeColor = Color.FromArgb(239, 68, 68), AutoSize = false,
            Font = new Font("Segoe UI", 8.5F), Location = new Point(x, 142), Size = new Size(w, 34),
            TextAlign = ContentAlignment.TopLeft, UseMnemonic = false,
        };
        _passwordButton = new PrimaryButton
        {
            BackColor = Color.White, Font = new Font("Segoe UI", 10.5F, FontStyle.Bold), Location = new Point(x, 182),
            Radius = 12, Size = new Size(w, 48), Text = "Create Account  →",
        };
        _passwordButton.Click += (_, _) => SavePassword();
        _passwordField.Box.TextChanged += (_, _) => _passwordError.Text = "";
        _confirmField.Box.TextChanged += (_, _) => _passwordError.Text = "";

        _passwordPanel.Controls.Add(Caption("PASSWORD", 4));
        _passwordPanel.Controls.Add(_passwordField);
        _passwordPanel.Controls.Add(Caption("CONFIRM PASSWORD", 72));
        _passwordPanel.Controls.Add(_confirmField);
        _passwordPanel.Controls.Add(_passwordError);
        _passwordPanel.Controls.Add(_passwordButton);
        contactPanel.Controls.Add(_passwordPanel);
    }

    private bool _welcomeNewAccount;   // the next Select a Pet screen greets a customer whose account was just created

    private async void SavePassword()
    {
        if (_busy || !PasswordStepVisible) return;

        string password = _passwordField!.Value, confirm = _confirmField!.Value;
        if (password.Length < MinPasswordLength) { _passwordError!.Text = $"Password must be at least {MinPasswordLength} characters."; return; }
        if (password.Length > 100) { _passwordError!.Text = "That password is too long (100 characters at most)."; return; }
        if (password != confirm)
        {
            _passwordError!.Text = "The passwords don't match.";
            _confirmField.Box.Focus();
            return;
        }

        // Hashing the password and saving the account takes a moment: show that something is happening instead of
        // freezing on the password page, and do the work off the UI thread so the screen keeps painting.
        int customerId = _customerId;
        string email = ContactEmail;
        string? error = null;
        SetBusy(true);
        _passwordButton!.Enabled = false;
        _passwordButton.Text = "Creating your account…";
        _passwordButton.Update();
        try
        {
            bool taken = await Task.Run(() => UserRepository.GetByEmail(email) is not null);
            if (taken) error = "That email is already used by another account.";
            else await Task.Run(() => EmployeeData.CreateCustomerLogin(customerId, email, password));
        }
        catch (SqlException ex) when (ex.Number is 2627 or 2601)
        {
            error = "That email is already used by another account.";
        }
        catch (SqlException ex) when (ex.Number == 547)
        {
            error = "The database doesn't allow the Customer role yet. Run Sql/customer_account.sql once.";
        }
        catch (Exception ex) when (ex is SqlException or InvalidOperationException)
        {
            error = "Couldn't create your account. " + ex.Message;
        }

        if (IsDisposed) return;   // closed while the account was being created
        SetBusy(false);

        if (error is not null)
        {
            _passwordButton.Enabled = true;
            _passwordButton.Text = "Create Account  →";
            _passwordError!.Text = error;
            return;
        }

        _welcomeNewAccount = true;
        ShowSelectStep();   // the button is reset behind the fade, so the old picture never shows a stuck "Creating…"
        _passwordButton.Enabled = true;
        _passwordButton.Text = "Create Account  →";
    }
}
