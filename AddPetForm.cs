using System.Net.Mail;
using Microsoft.Data.SqlClient;
using MyApp.Controls;

namespace MyApp;

/// <summary>
/// A customer adds a pet in two steps: (1) confirm it's them with a 6-digit code emailed to the address on their
/// account, (2) enter the pet. The layout lives in AddPetForm.Designer.cs, so it can be edited in Visual Studio's
/// designer; this file only holds the behaviour.
/// </summary>
internal sealed partial class AddPetForm : Form
{
    private readonly TextBox[] _otpBoxes;
    private readonly int _customerId;
    private readonly string _ownerName = "";
    private readonly string _loginEmail = "";
    private readonly string _profileEmail = "";
    private readonly string _profilePhone = "";

    private OtpChallenge? _challenge;
    private int _secondsLeft;
    private bool _busy;
    private bool _fillingOtp;

    /// <summary>Set when the pet was saved.</summary>
    public int? CreatedPetId { get; private set; }

    /// <summary>Used by Visual Studio's designer.</summary>
    public AddPetForm()
    {
        InitializeComponent();
        _otpBoxes = new[] { otpBox1, otpBox2, otpBox3, otpBox4, otpBox5, otpBox6 };
        WireUp();
        ShowContactStep();
    }

    public AddPetForm(int customerId, string ownerName, string loginEmail) : this()
    {
        _customerId = customerId;
        _ownerName = ownerName;
        _loginEmail = loginEmail.Trim();
        emailField.Box.Text = _loginEmail;
        ownerLabel.Text = "Adding a pet for " + ownerName;

        try { (_profileEmail, _profilePhone) = EmployeeData.GetCustomerContact(customerId); }
        catch (SqlException) { /* the check below falls back to the login email only */ }
    }

    // ------------------------------------------------------------------ wiring

    private void WireUp()
    {
        sendOtpButton.Click += (_, _) => SendOtp();
        verifyButton.Click += (_, _) => VerifyCode();
        resendLabel.Click += (_, _) => { if (_secondsLeft == 0) SendOtp(); };
        savePetButton.Click += (_, _) => SavePet();
        backButton.Click += (_, _) => GoBack();
        resendTimer.Tick += (_, _) => TickResend();

        emailField.Box.TextChanged += (_, _) => contactErrorLabel.Text = "";
        mobileField.Box.TextChanged += (_, _) => contactErrorLabel.Text = "";
        petNameField.Box.TextChanged += (_, _) => petErrorLabel.Text = "";
        petNameField.Box.MaxLength = 60;
        speciesField.Box.MaxLength = 30;
        breedField.Box.MaxLength = 60;

        foreach (var box in _otpBoxes)
        {
            box.TextChanged += OtpBox_TextChanged;
            box.KeyPress += OtpBox_KeyPress;
            box.KeyDown += OtpBox_KeyDown;
            box.Enter += (_, _) => box.SelectAll();
        }

        KeyDown += (_, e) =>
        {
            if (e.KeyCode == Keys.Escape)
            {
                DialogResult = DialogResult.Cancel;
                Close();
            }
            else if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                if (petPanel.Visible) SavePet();
                else if (_otpBoxes.Any(b => b.Focused) || verifyButton.Enabled) VerifyCode();
                else SendOtp();
            }
        };
    }

    // ------------------------------------------------------------------ step 1: contact + code

    private async void SendOtp()
    {
        if (_busy) return;

        string email = emailField.Value.Trim();
        if (!IsValidEmail(email))
        {
            ShowContactError("Enter a valid email address, like example@email.com.");
            return;
        }
        if (!IsOwnEmail(email))
        {
            ShowContactError("That email isn't on your Pawfect account. Use the email you signed in with.");
            return;
        }
        string mobile = mobileField.Value.Trim();
        if (mobile.Length > 0 && !MobileMatches(mobile))
        {
            ShowContactError("That mobile number doesn't match the one on your account.");
            return;
        }

        SetBusy(true);
        sendOtpButton.Text = "Sending…";
        try
        {
            _challenge = await EmailOtp.SendAsync(email, _ownerName);
            otpHint.Text = $"We've sent a 6-digit code to {MaskEmail(email)}.";
            otpErrorLabel.Text = "";
            contactErrorLabel.Text = "";
            ClearOtpBoxes();
            foreach (var b in _otpBoxes) b.Enabled = true;
            resendLabel.Visible = true;
            StartCountdown();
            _otpBoxes[0].Focus();
        }
        catch (OtpException ex)
        {
            ShowContactError(ex.Message);
        }
        finally
        {
            sendOtpButton.Text = _challenge is null ? "Send OTP  →" : "Send a new code  →";
            SetBusy(false);
        }
    }

    private void VerifyCode()
    {
        if (_busy || _challenge is null || !verifyButton.Enabled) return;
        try
        {
            EmailOtp.Verify(_challenge, string.Concat(_otpBoxes.Select(b => b.Text)));
        }
        catch (OtpException ex)
        {
            otpErrorLabel.Text = ex.Message;
            ClearOtpBoxes();
            _otpBoxes[0].Focus();
            return;
        }
        ShowPetStep();
    }

    private void StartCountdown()
    {
        _secondsLeft = EmailOtp.ResendSeconds;
        UpdateResendText();
        resendTimer.Start();
    }

    private void TickResend()
    {
        if (_secondsLeft > 0) _secondsLeft--;
        if (_secondsLeft == 0) resendTimer.Stop();
        UpdateResendText();
    }

    private void UpdateResendText()
    {
        bool waiting = _secondsLeft > 0;
        resendLabel.Text = waiting ? $"Resend OTP ({_secondsLeft / 60:00}:{_secondsLeft % 60:00})" : "Resend OTP";
        resendLabel.ForeColor = waiting ? Theme.Muted : Theme.Primary;
        resendLabel.Cursor = waiting ? Cursors.Default : Cursors.Hand;
    }

    private void SetBusy(bool busy)
    {
        _busy = busy;
        sendOtpButton.Enabled = !busy;
        UseWaitCursor = busy;
    }

    private void ShowContactError(string message) => contactErrorLabel.Text = message;

    // ---- the six boxes

    private void OtpBox_TextChanged(object? sender, EventArgs e)
    {
        if (_fillingOtp || sender is not TextBox box) return;
        otpErrorLabel.Text = "";

        if (box.Text.Length > 0 && !char.IsDigit(box.Text[0]))
        {
            box.Text = "";
            return;
        }
        if (box.Text.Length == 1)
        {
            int next = Array.IndexOf(_otpBoxes, box) + 1;
            if (next < _otpBoxes.Length) _otpBoxes[next].Focus();
        }
        verifyButton.Enabled = _otpBoxes.All(b => b.Text.Length == 1);
    }

    private void OtpBox_KeyPress(object? sender, KeyPressEventArgs e)
    {
        if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) e.Handled = true;
    }

    private void OtpBox_KeyDown(object? sender, KeyEventArgs e)
    {
        if (sender is not TextBox box) return;
        int i = Array.IndexOf(_otpBoxes, box);

        if (e.KeyCode == Keys.Back && box.Text.Length == 0 && i > 0)
        {
            _otpBoxes[i - 1].Focus();
            _otpBoxes[i - 1].Clear();
            e.SuppressKeyPress = true;
        }
        else if (e.KeyCode == Keys.Left && i > 0) { _otpBoxes[i - 1].Focus(); e.SuppressKeyPress = true; }
        else if (e.KeyCode == Keys.Right && i < _otpBoxes.Length - 1) { _otpBoxes[i + 1].Focus(); e.SuppressKeyPress = true; }
        else if (e.Control && e.KeyCode == Keys.V)
        {
            // Pasting "123456" fills all six boxes.
            e.SuppressKeyPress = true;
            string digits = new(Clipboard.GetText().Where(char.IsDigit).Take(_otpBoxes.Length).ToArray());
            if (digits.Length == 0) return;
            _fillingOtp = true;
            for (int k = 0; k < _otpBoxes.Length; k++) _otpBoxes[k].Text = k < digits.Length ? digits[k].ToString() : "";
            _fillingOtp = false;
            verifyButton.Enabled = digits.Length == _otpBoxes.Length;
            _otpBoxes[Math.Min(digits.Length, _otpBoxes.Length - 1)].Focus();
        }
    }

    private void ClearOtpBoxes()
    {
        _fillingOtp = true;
        foreach (var b in _otpBoxes) b.Clear();
        _fillingOtp = false;
        verifyButton.Enabled = false;
    }

    // ------------------------------------------------------------------ step 2: the pet

    private void SavePet()
    {
        if (string.IsNullOrWhiteSpace(petNameField.Value))
        {
            petErrorLabel.Text = "Enter your pet's name.";
            petNameField.Box.Focus();
            return;
        }
        try
        {
            CreatedPetId = EmployeeData.AddPet(_customerId, petNameField.Value, speciesField.Value, breedField.Value);
            DialogResult = DialogResult.OK;
        }
        catch (Exception ex) when (ex is SqlException or InvalidOperationException)
        {
            petErrorLabel.Text = "Couldn't save the pet. " + ex.Message;
        }
    }

    // ------------------------------------------------------------------ moving between steps

    private void ShowContactStep()
    {
        petPanel.Visible = false;
        contactPanel.Visible = true;
        titleLabel.Text = "Enter Your Contact Information";
        subtitleLabel.Text = "We'll email you a 6-digit code to confirm it's you.";
        stepContact.State = StepState.Active;
        stepPet.State = StepState.Pending;
    }

    private void ShowPetStep()
    {
        resendTimer.Stop();
        contactPanel.Visible = false;
        petPanel.Visible = true;
        titleLabel.Text = "Tell Us About Your Pet";
        subtitleLabel.Text = "Add your pet's details to your account.";
        stepContact.State = StepState.Done;
        stepPet.State = StepState.Active;
        petNameField.Box.Focus();
    }

    private void GoBack()
    {
        if (petPanel.Visible)
        {
            // Back to step 1 means proving it again: the old code is thrown away.
            _challenge = null;
            ClearOtpBoxes();
            foreach (var b in _otpBoxes) b.Enabled = false;
            resendLabel.Visible = false;
            otpHint.Text = "We'll send a 6-digit code to your email.";
            sendOtpButton.Text = "Send OTP  →";
            ShowContactStep();
            return;
        }
        DialogResult = DialogResult.Cancel;
        Close();
    }

    // ------------------------------------------------------------------ checks

    private static bool IsValidEmail(string email)
        => email.Length <= 256 && email.Contains('.') && MailAddress.TryCreate(email, out var a) && a.Address == email;

    /// <summary>The code only goes to an address that belongs to this customer, never to whatever is typed.</summary>
    private bool IsOwnEmail(string email)
        => string.Equals(email, _loginEmail, StringComparison.OrdinalIgnoreCase)
           || (_profileEmail.Length > 0 && string.Equals(email, _profileEmail, StringComparison.OrdinalIgnoreCase));

    private bool MobileMatches(string typed)
    {
        string a = Digits(typed), b = Digits(_profilePhone);
        if (b.Length == 0) return true;               // nothing on file to compare against
        int n = Math.Min(9, Math.Min(a.Length, b.Length));
        return n >= 7 && a[^n..] == b[^n..];
    }

    private static string Digits(string s) => new(s.Where(char.IsDigit).ToArray());

    /// <summary>jane.doe@gmail.com -> j*****e@gmail.com</summary>
    private static string MaskEmail(string email)
    {
        int at = email.IndexOf('@');
        if (at <= 1) return email;
        return email[0] + new string('*', Math.Min(at - 2, 6)) + email[at - 1] + email[at..];
    }
}
