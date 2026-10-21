using System.Net.Mail;
using Microsoft.Data.SqlClient;
using MyApp.Controls;

namespace MyApp;

/// <summary>
/// Customer flow: (1) confirm it's them with a 6-digit code emailed to them, (2) pick one of their pets (Continue opens
/// the appointment request) or register a new one on the Add New Pet tab. The layout lives in AddPetForm.Designer.cs, so it can be edited in Visual Studio's
/// designer; this file only holds the behaviour.
/// </summary>
internal sealed partial class AddPetForm : Form
{
    private readonly TextBox[] _otpBoxes;
    private int _customerId;
    private string _ownerName = "";
    private readonly string _loginEmail = "";
    private readonly string _profileEmail = "";
    private string _profilePhone = "";

    private readonly bool _kiosk;          // walk-in at the kiosk: no login, the customer is found (or created) from the email
    private readonly bool _skipContactStep;   // signed in with an account staff set up: step 1 (email code) is already done
    private string _contactMobile = "";
    private string _typedName = "";        // kiosk: the name typed in step 1 (saved on the customer record once the code is right)

    private readonly List<PetCard> _petCards = new();
    private PetCard? _chosenCard;

    private OtpChallenge? _challenge;
    private int _secondsLeft;
    private bool _busy;
    private bool _fillingOtp;

    /// <summary>Set when a new pet was saved.</summary>
    public int? CreatedPetId { get; private set; }

    /// <summary>The customer this visit belongs to (kiosk: known once the code is verified).</summary>
    public int CustomerId => _customerId;

    /// <summary>The email the code was sent to and verified (kiosk).</summary>
    public string ContactEmail { get; private set; } = "";

    /// <summary>Set when the customer picked an existing pet and pressed Continue: the caller opens the appointment request.</summary>
    public int? ChosenPetId { get; private set; }

    /// <summary>Used by Visual Studio's designer.</summary>
    public AddPetForm()
    {
        InitializeComponent();
        Icon = AppIcon.Value;
        _otpBoxes = new[] { otpBox1.Box, otpBox2.Box, otpBox3.Box, otpBox4.Box, otpBox5.Box, otpBox6.Box };
        foreach (var b in _otpBoxes) b.Enabled = false;   // switched on once the code has been sent
        WireUp();
        ShowContactStep();
        _host = FullScreen.Apply(this, centerContent: true, out _scaler);   // full screen; the fixed-size steps are scaled up and centred
    }

    private Control? _host;                       // the centred panel that holds the steps (the form itself when not wrapped)
    private ContentScaler? _scaler;
    private Control ContentHost => _host ?? this;

    /// <summary>A point written in the original 900 x 540 design, moved to where it belongs at the current size.</summary>
    private Point Px(int x, int y)
    {
        float k = _scaler?.LayoutScale ?? 1f;   // design pixels x screen DPI x content scale
        return new Point((int)Math.Round(x * k), (int)Math.Round(y * k));
    }

    /// <summary>Kiosk mode: opens on step 1 with nothing filled in.</summary>
    public AddPetForm(bool kiosk) : this()
    {
        _kiosk = kiosk;
        ownerLabel.Text = "";
    }

    public AddPetForm(int customerId, string ownerName, string loginEmail) : this()
    {
        _customerId = customerId;
        _ownerName = ownerName;
        _loginEmail = loginEmail.Trim();
        emailField.Box.Text = _loginEmail;
        ownerLabel.Text = "Registering a new pet for " + ownerName;

        try { (_profileEmail, _profilePhone) = EmployeeData.GetCustomerContact(customerId); }
        catch (SqlException) { /* the check below falls back to the login email only */ }

        // The account was set up (and linked to this customer) by staff and the password was just checked at login,
        // so there is no email code to enter: go straight to step 2, Select a Pet.
        _skipContactStep = true;
        ShowSelectStep();
    }

    // ------------------------------------------------------------------ wiring

    private void WireUp()
    {
        sendOtpButton.Click += (_, _) => SendOtp();
        verifyButton.Click += (_, _) => VerifyCode();
        resendLabel.Click += (_, _) => { if (_secondsLeft == 0) SendOtp(); };
        savePetButton.Click += (_, _) => SavePet();
        continueButton.Click += (_, _) => ContinueWithPet();
        tabMyPets.Click += (_, _) => ShowTab(addNew: false);
        tabAddPet.Click += (_, _) => OpenNewPetFlow();   // Add New Pet: Pet Details pop-up, then the vaccine step
        foreach (Control c in new Control[] { newPetCard, newPetPaw, newPetTitle, newPetHint, newPetArrow, newPetArrowLabel })
        {
            c.Cursor = Cursors.Hand;
            c.Click += (_, _) => OpenNewPetFlow();
        }
        backButton.Click += (_, _) => GoBack();
        resendTimer.Tick += (_, _) => TickResend();

        nameField.Box.TextChanged += (_, _) => contactErrorLabel.Text = "";
        emailField.Box.TextChanged += (_, _) => contactErrorLabel.Text = "";
        mobileField.Box.TextChanged += (_, _) => contactErrorLabel.Text = "";
        mobileField.Box.MaxLength = 20;
        mobileField.Box.KeyPress += MobileBox_KeyPress;      // no letters while typing
        mobileField.Box.TextChanged += MobileBox_TextChanged; // ...or when pasting
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
                if (PasswordStepVisible) SavePassword();
                else if (VaccineStepVisible || ServiceStepVisible) ContinueWithPet();
                else if (selectPanel.Visible) { if (addPetView.Visible) SavePet(); else ContinueWithPet(); }
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
        string typedName = nameField.Value.Trim();
        if (_kiosk && !LookUpKioskCustomer(email)) return;
        if (_kiosk)
        {
            // a returning customer can leave the name blank (the one on file is used); a new customer must give one
            if (typedName.Length == 0 && _customerId == 0)
            {
                ShowContactError("Please enter your full name.");
                return;
            }
            if (typedName.Length > 100)
            {
                ShowContactError("That name is too long (100 characters at most).");
                return;
            }
            if (typedName.Length == 0) nameField.Box.Text = typedName = _ownerName;   // show the name on file
            _typedName = typedName;
            if (_customerId == 0) _ownerName = typedName;                              // the email greets them by name
        }
        string mobile = mobileField.Value.Trim();
        _contactMobile = mobile;
        // The mobile number is optional: only check it when something was typed.
        if (mobile.Length > 0 && !PhoneRules.IsValid(mobile))
        {
            ShowContactError(PhoneRules.Hint);
            mobileField.Box.Focus();
            return;
        }
        if (mobile.Length > 0 && !MobileMatches(mobile))
        {
            ShowContactError("That mobile number doesn't match the one on your account.");
            return;
        }

        SetBusy(true);
        sendOtpButton.Text = "Sending…";
        try
        {
            _challenge = await EmailOtp.SendAsync(email, _ownerName.Length > 0 ? _ownerName : "there");
            ContactEmail = email;
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
        if (_kiosk && !EnsureKioskCustomer()) return;
        if (_kiosk && NeedsPassword()) { ShowPasswordStep(); return; }   // still step 1 (Contact): now the password page
        ShowSelectStep();
    }

    // ---- kiosk: the customer is found by email (or created once the code is right)

    private bool LookUpKioskCustomer(string email)
    {
        try
        {
            if (EmployeeData.FindCustomerByEmail(email) is { } found)
            {
                _customerId = found.Id;
                _ownerName = found.Name;
                _profilePhone = EmployeeData.GetCustomerContact(found.Id).Phone;
            }
            else
            {
                _customerId = 0;
                _ownerName = "";
                _profilePhone = "";
            }
            return true;
        }
        catch (SqlException)
        {
            ShowContactError("Can't reach the database.");
            return false;
        }
    }

    private bool EnsureKioskCustomer()
    {
        try
        {
            if (_customerId == 0)
            {
                // first visit: a record is made with the name typed in step 1 (the email's first part only if it was somehow left empty)
                string name = _typedName.Length > 0 ? _typedName
                    : ContactEmail.Contains('@') ? ContactEmail[..ContactEmail.IndexOf('@')] : ContactEmail;
                _ownerName = name.Length > 100 ? name[..100] : name;
                _customerId = EmployeeData.AddCustomer(_ownerName, _contactMobile, ContactEmail, "");
            }
            else if (_typedName.Length > 0 && !string.Equals(_typedName, _ownerName, StringComparison.Ordinal))
            {
                // returning customer corrected their name: the employee side shows the new one
                EmployeeData.UpdateCustomerName(_customerId, _typedName);
                _ownerName = _typedName;
            }
            ownerLabel.Text = "Registering a new pet for " + _ownerName;
            return true;
        }
        catch (SqlException ex)
        {
            otpErrorLabel.Text = "Couldn't set up your record. " + ex.Message;
            return false;
        }
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

    // ---- mobile number: digits only, 11 of them, starting with 09 (checked on Send OTP)

    private static bool IsPhoneChar(char c) => char.IsAsciiDigit(c);

    private void MobileBox_KeyPress(object? sender, KeyPressEventArgs e)
    {
        if (!char.IsControl(e.KeyChar) && !IsPhoneChar(e.KeyChar)) e.Handled = true;
    }

    private void MobileBox_TextChanged(object? sender, EventArgs e)
    {
        var box = mobileField.Box;
        string clean = PhoneRules.Clean(box.Text);
        if (clean == box.Text) return;

        // Pasted letters, spaces and digits past the 11th are dropped; keep the cursor where the person expects it.
        int caret = box.Text.Take(box.SelectionStart).Count(IsPhoneChar);
        box.Text = clean;
        box.SelectionStart = Math.Clamp(caret, 0, clean.Length);
    }

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

    // ------------------------------------------------------------------ step 2: select a pet

    private void LoadPets()
    {
        foreach (var card in _petCards) { petsFlow.Controls.Remove(card); card.Dispose(); }
        _petCards.Clear();
        _chosenCard = null;
        continueButton.Enabled = false;

        List<PetRecord> pets;
        try { pets = EmployeeData.GetPets().Where(p => p.CustomerId == _customerId).ToList(); }
        catch (SqlException ex)
        {
            petErrorLabel.Text = "Couldn't load your pets. " + ex.Message;
            pets = new();
        }

        int index = 0;
        foreach (var pet in pets)
        {
            var card = new PetCard
            {
                Text = pet.Name,
                Species = pet.Species,
                Detail = string.Join(" \u2022 ", new[] { pet.Species, pet.Breed }.Where(x => !string.IsNullOrWhiteSpace(x))),
                Tag = pet.Id,
            };
            card.Click += (_, _) => { ChooseCard(card); OpenExistingPetFlow(card); };
            petsFlow.Controls.Add(card);
            petsFlow.Controls.SetChildIndex(card, index++);   // keep the "New Pet?" tile last
            _petCards.Add(card);
        }
        if (_petCards.Count == 1) ChooseCard(_petCards[0]);
    }

    private void ChooseCard(PetCard card)
    {
        foreach (var c in _petCards) c.Selected = c == card;
        _chosenCard = card;
        continueButton.Enabled = true;
    }

    private void ShowTab(bool addNew)
    {
        tabMyPets.Active = !addNew;
        tabAddPet.Active = addNew;
        myPetsView.Visible = !addNew;
        addPetView.Visible = addNew;
        continueButton.Visible = !addNew;
        if (addNew) petNameField.Box.Focus();
    }

    private void ContinueWithPet()
    {
        if (ServiceStepVisible)
        {
            FinishServiceStep();   // step 4: on to the appointment request with this service
            return;
        }

        // on the vaccine step Continue scans the card first, then moves on (a card that is already on file and valid just moves on)
        if (VaccineStepVisible)
        {
            if (_vaccineDone || _vaccineState == VaccineState.Valid) FinishVaccineStep(); else ScanVaccineCard();
            return;
        }

        if (_chosenCard?.Tag is not int petId)
        {
            return;
        }

        OpenExistingPetFlow(_chosenCard);   // Pet Details first, then the vaccine step if needed
    }

    // ---- step 2: add a new pet

    private void SavePet()
    {
        if (string.IsNullOrWhiteSpace(petNameField.Value))
        {
            petErrorLabel.Text = "Enter your pet's name.";
            petNameField.Box.Focus();
            return;
        }
        if (!PetNameRules.IsValid(petNameField.Value))
        {
            petErrorLabel.Text = PetNameRules.Hint;
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

    private void ShowContactStep() => PageFade.Swap(ContentHost, ShowContactStepCore);

    private void ShowContactStepCore()
    {
        HideVaccineStep();
        selectPanel.Visible = false;
        continueButton.Visible = false;
        contactPanel.Visible = true;
        if (_passwordPanel is not null) _passwordPanel.Visible = false;
        titleLabel.Text = "Enter Your Contact Information";
        subtitleLabel.Text = "We'll email you a 6-digit code to confirm it's you.";
        stepContact.State = StepState.Active;
        stepSelect.State = StepState.Pending;
    }

    private void ShowSelectStep() => PageFade.Swap(ContentHost, ShowSelectStepCore);

    private void ShowSelectStepCore()
    {
        resendTimer.Stop();
        HideVaccineStep();
        if (_passwordPanel is not null) _passwordPanel.Visible = false;
        contactPanel.Visible = false;
        selectPanel.Visible = true;
        titleLabel.Text = "Select a Pet";
        if (_welcomeNewAccount)
        {
            // just came from Create Your Password: say the account exists, then move on to the pet
            string first = _ownerName.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries).FirstOrDefault() ?? "";
            subtitleLabel.Text = (first.Length > 0 ? $"Welcome, {first}! " : "Welcome! ") + "Your account is ready. Choose a pet or register a new one.";
            _welcomeNewAccount = false;
        }
        else subtitleLabel.Text = "Choose an existing pet or register a new one.";
        stepContact.State = StepState.Done;
        stepSelect.State = StepState.Active;
        LoadPets();
        ShowTab(addNew: false);   // no pets yet: the list just shows the "New Pet?" tile
    }

    private void GoBack()
    {
        if (PasswordStepVisible)
        {
            ResetContactPage();   // back to the contact page: the code has to be entered again
            ShowContactStep();
            return;
        }
        if (ServiceStepVisible)
        {
            BackToVaccineStep();
            return;
        }
        if (VaccineStepVisible)
        {
            ShowSelectStep();   // back to the pet list (a pet saved a moment ago is in it now)
            return;
        }
        if (selectPanel.Visible && _skipContactStep)
        {
            // Signed in: there is no step 1 to go back to, so Back returns to the landing screen.
            DialogResult = DialogResult.Cancel;
            Close();
            return;
        }
        if (selectPanel.Visible)
        {
            // Back to step 1 means proving it again: the old code is thrown away.
            ResetContactPage();
            ShowContactStep();
            return;
        }
        DialogResult = DialogResult.Cancel;
        Close();
    }

    /// <summary>Throws the emailed code away and puts the contact page back to "send a code".</summary>
    private void ResetContactPage()
    {
        _challenge = null;
        ClearOtpBoxes();
        foreach (var b in _otpBoxes) b.Enabled = false;
        resendLabel.Visible = false;
        otpHint.Text = "We'll send a 6-digit code to your email.";
        sendOtpButton.Text = "Send OTP  \u2192";
    }

    // ------------------------------------------------------------------ checks

    private static bool IsValidEmail(string email)
        => email.Length <= 256 && email.Contains('.') && MailAddress.TryCreate(email, out var a) && a.Address == email;

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