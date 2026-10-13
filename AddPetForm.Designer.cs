#nullable disable
namespace MyApp
{
    partial class AddPetForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            logoBadge = new MyApp.Controls.PawBadge();
            brandLabel = new System.Windows.Forms.Label();
            stepContact = new MyApp.Controls.StepDot();
            stepLine = new System.Windows.Forms.Panel();
            stepPet = new MyApp.Controls.StepDot();
            titleLabel = new System.Windows.Forms.Label();
            subtitleLabel = new System.Windows.Forms.Label();
            emailLabel = new System.Windows.Forms.Label();
            emailField = new MyApp.Controls.InputField();
            mobileLabel = new System.Windows.Forms.Label();
            mobileField = new MyApp.Controls.InputField();
            contactErrorLabel = new System.Windows.Forms.Label();
            sendOtpButton = new MyApp.Controls.PrimaryButton();
            dividerLine = new System.Windows.Forms.Panel();
            orLabel = new System.Windows.Forms.Label();
            orBadge = new MyApp.Controls.CardPanel();
            otpIcon = new System.Windows.Forms.Label();
            otpTitle = new System.Windows.Forms.Label();
            otpHint = new System.Windows.Forms.Label();
            otpBox1 = new System.Windows.Forms.TextBox();
            otpBox2 = new System.Windows.Forms.TextBox();
            otpBox3 = new System.Windows.Forms.TextBox();
            otpBox4 = new System.Windows.Forms.TextBox();
            otpBox5 = new System.Windows.Forms.TextBox();
            otpBox6 = new System.Windows.Forms.TextBox();
            otpErrorLabel = new System.Windows.Forms.Label();
            verifyButton = new MyApp.Controls.PrimaryButton();
            resendLabel = new System.Windows.Forms.Label();
            otpCard = new MyApp.Controls.CardPanel();
            contactPanel = new System.Windows.Forms.Panel();
            ownerLabel = new System.Windows.Forms.Label();
            petNameLabel = new System.Windows.Forms.Label();
            petNameField = new MyApp.Controls.InputField();
            speciesLabel = new System.Windows.Forms.Label();
            speciesField = new MyApp.Controls.InputField();
            breedLabel = new System.Windows.Forms.Label();
            breedField = new MyApp.Controls.InputField();
            petErrorLabel = new System.Windows.Forms.Label();
            savePetButton = new MyApp.Controls.PrimaryButton();
            petCard = new MyApp.Controls.CardPanel();
            petPanel = new System.Windows.Forms.Panel();
            backButton = new System.Windows.Forms.Button();
            cornerPaw = new MyApp.Controls.PawBadge();
            resendTimer = new System.Windows.Forms.Timer(components);
            orBadge.SuspendLayout();
            otpCard.SuspendLayout();
            contactPanel.SuspendLayout();
            petCard.SuspendLayout();
            petPanel.SuspendLayout();
            SuspendLayout();
            // 
            // logoBadge
            // 
            logoBadge.BackColor = Color.FromArgb(243, 247, 252);
            logoBadge.FillColor = Color.FromArgb(243, 247, 252);
            logoBadge.Location = new Point(40, 20);
            logoBadge.Name = "logoBadge";
            logoBadge.PawColor = Color.FromArgb(58, 123, 213);
            logoBadge.Radius = 8;
            logoBadge.Size = new Size(30, 30);
            logoBadge.TabStop = false;
            // 
            // brandLabel
            // 
            brandLabel.BackColor = Color.FromArgb(243, 247, 252);
            brandLabel.Font = new Font("Segoe UI", 13F, FontStyle.Bold, GraphicsUnit.Point, 0);
            brandLabel.ForeColor = Color.FromArgb(31, 42, 60);
            brandLabel.Location = new Point(78, 20);
            brandLabel.Name = "brandLabel";
            brandLabel.Size = new Size(140, 30);
            brandLabel.TabIndex = 1;
            brandLabel.Text = "Pawfect";
            brandLabel.TextAlign = ContentAlignment.MiddleLeft;
            brandLabel.UseMnemonic = false;
            // 
            // stepContact
            // 
            stepContact.BackColor = Color.FromArgb(243, 247, 252);
            stepContact.Location = new Point(610, 14);
            stepContact.Name = "stepContact";
            stepContact.Number = 1;
            stepContact.Size = new Size(80, 48);
            stepContact.State = MyApp.Controls.StepState.Active;
            stepContact.TabStop = false;
            stepContact.Text = "Contact";
            // 
            // stepLine
            // 
            stepLine.BackColor = Color.FromArgb(227, 233, 242);
            stepLine.Location = new Point(690, 25);
            stepLine.Name = "stepLine";
            stepLine.Size = new Size(80, 1);
            stepLine.TabIndex = 3;
            // 
            // stepPet
            // 
            stepPet.BackColor = Color.FromArgb(243, 247, 252);
            stepPet.Location = new Point(770, 14);
            stepPet.Name = "stepPet";
            stepPet.Number = 2;
            stepPet.Size = new Size(80, 48);
            stepPet.State = MyApp.Controls.StepState.Pending;
            stepPet.TabStop = false;
            stepPet.Text = "Pet Details";
            // 
            // titleLabel
            // 
            titleLabel.BackColor = Color.FromArgb(243, 247, 252);
            titleLabel.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            titleLabel.ForeColor = Color.FromArgb(19, 41, 75);
            titleLabel.Location = new Point(40, 84);
            titleLabel.Name = "titleLabel";
            titleLabel.Size = new Size(820, 36);
            titleLabel.TabIndex = 5;
            titleLabel.Text = "Enter Your Contact Information";
            titleLabel.TextAlign = ContentAlignment.MiddleLeft;
            titleLabel.UseMnemonic = false;
            // 
            // subtitleLabel
            // 
            subtitleLabel.BackColor = Color.FromArgb(243, 247, 252);
            subtitleLabel.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            subtitleLabel.ForeColor = Color.FromArgb(122, 136, 156);
            subtitleLabel.Location = new Point(40, 122);
            subtitleLabel.Name = "subtitleLabel";
            subtitleLabel.Size = new Size(820, 22);
            subtitleLabel.TabIndex = 6;
            subtitleLabel.Text = "We'll email you a 6-digit code to confirm it's you.";
            subtitleLabel.TextAlign = ContentAlignment.MiddleLeft;
            subtitleLabel.UseMnemonic = false;
            // 
            // emailLabel
            // 
            emailLabel.BackColor = Color.FromArgb(243, 247, 252);
            emailLabel.Font = new Font("Segoe UI", 7.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            emailLabel.ForeColor = Color.FromArgb(68, 83, 106);
            emailLabel.Location = new Point(40, 14);
            emailLabel.Name = "emailLabel";
            emailLabel.Size = new Size(380, 16);
            emailLabel.TabIndex = 0;
            emailLabel.Text = "EMAIL ADDRESS";
            emailLabel.TextAlign = ContentAlignment.MiddleLeft;
            emailLabel.UseMnemonic = false;
            // 
            // emailField
            // 
            emailField.BorderColor = Color.FromArgb(227, 233, 242);
            emailField.CornerColor = Color.FromArgb(243, 247, 252);
            emailField.FillColor = Color.White;
            emailField.Glyph = "\uE715";
            emailField.Location = new Point(40, 32);
            emailField.Margin = new Padding(0);
            emailField.Name = "emailField";
            emailField.Padding = new Padding(10, 3, 10, 3);
            emailField.Placeholder = "example@email.com";
            emailField.Radius = 12;
            emailField.Size = new Size(380, 46);
            emailField.TabIndex = 1;
            // 
            // mobileLabel
            // 
            mobileLabel.BackColor = Color.FromArgb(243, 247, 252);
            mobileLabel.Font = new Font("Segoe UI", 7.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            mobileLabel.ForeColor = Color.FromArgb(68, 83, 106);
            mobileLabel.Location = new Point(40, 92);
            mobileLabel.Name = "mobileLabel";
            mobileLabel.Size = new Size(380, 16);
            mobileLabel.TabIndex = 2;
            mobileLabel.Text = "MOBILE NUMBER (OPTIONAL)";
            mobileLabel.TextAlign = ContentAlignment.MiddleLeft;
            mobileLabel.UseMnemonic = false;
            // 
            // mobileField
            // 
            mobileField.BorderColor = Color.FromArgb(227, 233, 242);
            mobileField.CornerColor = Color.FromArgb(243, 247, 252);
            mobileField.FillColor = Color.White;
            mobileField.Glyph = "\uE717";
            mobileField.Location = new Point(40, 110);
            mobileField.Margin = new Padding(0);
            mobileField.Name = "mobileField";
            mobileField.Padding = new Padding(10, 3, 10, 3);
            mobileField.Placeholder = "09XX XXX XXXX";
            mobileField.Radius = 12;
            mobileField.Size = new Size(380, 46);
            mobileField.TabIndex = 3;
            // 
            // contactErrorLabel
            // 
            contactErrorLabel.BackColor = Color.FromArgb(243, 247, 252);
            contactErrorLabel.Font = new Font("Segoe UI", 8.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            contactErrorLabel.ForeColor = Color.FromArgb(239, 68, 68);
            contactErrorLabel.Location = new Point(40, 166);
            contactErrorLabel.Name = "contactErrorLabel";
            contactErrorLabel.Size = new Size(380, 36);
            contactErrorLabel.TabIndex = 4;
            contactErrorLabel.Text = "";
            contactErrorLabel.TextAlign = ContentAlignment.TopLeft;
            contactErrorLabel.UseMnemonic = false;
            // 
            // sendOtpButton
            // 
            sendOtpButton.BackColor = Color.White;
            sendOtpButton.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            sendOtpButton.Location = new Point(40, 206);
            sendOtpButton.Name = "sendOtpButton";
            sendOtpButton.Radius = 12;
            sendOtpButton.Size = new Size(380, 48);
            sendOtpButton.TabIndex = 5;
            sendOtpButton.Text = "Send OTP  \u2192";
            // 
            // dividerLine
            // 
            dividerLine.BackColor = Color.FromArgb(227, 233, 242);
            dividerLine.Location = new Point(449, 0);
            dividerLine.Name = "dividerLine";
            dividerLine.Size = new Size(1, 300);
            dividerLine.TabIndex = 6;
            // 
            // orLabel
            // 
            orLabel.BackColor = Color.White;
            orLabel.Dock = DockStyle.Fill;
            orLabel.Font = new Font("Segoe UI", 7F, FontStyle.Bold, GraphicsUnit.Point, 0);
            orLabel.ForeColor = Color.FromArgb(122, 136, 156);
            orLabel.Name = "orLabel";
            orLabel.Text = "OR";
            orLabel.TextAlign = ContentAlignment.MiddleCenter;
            orLabel.UseMnemonic = false;
            // 
            // orBadge
            // 
            orBadge.Controls.Add(orLabel);
            orBadge.BackColor = Color.White;
            orBadge.BorderColor = Color.FromArgb(227, 233, 242);
            orBadge.CornerColor = Color.FromArgb(243, 247, 252);
            orBadge.FillColor = Color.White;
            orBadge.Location = new Point(435, 136);
            orBadge.Margin = new Padding(0);
            orBadge.Name = "orBadge";
            orBadge.Radius = 14;
            orBadge.Size = new Size(30, 30);
            orBadge.TabIndex = 7;
            // 
            // otpIcon
            // 
            otpIcon.BackColor = Color.FromArgb(234, 242, 253);
            otpIcon.Font = new Font("Segoe MDL2 Assets", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            otpIcon.ForeColor = Color.FromArgb(58, 123, 213);
            otpIcon.Location = new Point(24, 22);
            otpIcon.Name = "otpIcon";
            otpIcon.Size = new Size(36, 36);
            otpIcon.TabIndex = 0;
            otpIcon.Text = "\uE715";
            otpIcon.TextAlign = ContentAlignment.MiddleCenter;
            otpIcon.UseMnemonic = false;
            // 
            // otpTitle
            // 
            otpTitle.BackColor = Color.White;
            otpTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            otpTitle.ForeColor = Color.FromArgb(31, 42, 60);
            otpTitle.Location = new Point(72, 22);
            otpTitle.Name = "otpTitle";
            otpTitle.Size = new Size(240, 36);
            otpTitle.TabIndex = 1;
            otpTitle.Text = "Enter OTP";
            otpTitle.TextAlign = ContentAlignment.MiddleLeft;
            otpTitle.UseMnemonic = false;
            // 
            // otpHint
            // 
            otpHint.BackColor = Color.White;
            otpHint.Font = new Font("Segoe UI", 8.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            otpHint.ForeColor = Color.FromArgb(122, 136, 156);
            otpHint.Location = new Point(24, 70);
            otpHint.Name = "otpHint";
            otpHint.Size = new Size(332, 20);
            otpHint.TabIndex = 2;
            otpHint.Text = "We'll send a 6-digit code to your email.";
            otpHint.TextAlign = ContentAlignment.MiddleLeft;
            otpHint.UseMnemonic = false;
            // 
            // otpBox1
            // 
            otpBox1.BackColor = Color.White;
            otpBox1.BorderStyle = BorderStyle.FixedSingle;
            otpBox1.Enabled = false;
            otpBox1.Font = new Font("Segoe UI", 16F, FontStyle.Bold, GraphicsUnit.Point, 0);
            otpBox1.ForeColor = Color.FromArgb(31, 42, 60);
            otpBox1.Location = new Point(25, 104);
            otpBox1.MaxLength = 1;
            otpBox1.Name = "otpBox1";
            otpBox1.Size = new Size(44, 38);
            otpBox1.TabIndex = 3;
            otpBox1.TextAlign = HorizontalAlignment.Center;
            // 
            // otpBox2
            // 
            otpBox2.BackColor = Color.White;
            otpBox2.BorderStyle = BorderStyle.FixedSingle;
            otpBox2.Enabled = false;
            otpBox2.Font = new Font("Segoe UI", 16F, FontStyle.Bold, GraphicsUnit.Point, 0);
            otpBox2.ForeColor = Color.FromArgb(31, 42, 60);
            otpBox2.Location = new Point(82, 104);
            otpBox2.MaxLength = 1;
            otpBox2.Name = "otpBox2";
            otpBox2.Size = new Size(44, 38);
            otpBox2.TabIndex = 4;
            otpBox2.TextAlign = HorizontalAlignment.Center;
            // 
            // otpBox3
            // 
            otpBox3.BackColor = Color.White;
            otpBox3.BorderStyle = BorderStyle.FixedSingle;
            otpBox3.Enabled = false;
            otpBox3.Font = new Font("Segoe UI", 16F, FontStyle.Bold, GraphicsUnit.Point, 0);
            otpBox3.ForeColor = Color.FromArgb(31, 42, 60);
            otpBox3.Location = new Point(139, 104);
            otpBox3.MaxLength = 1;
            otpBox3.Name = "otpBox3";
            otpBox3.Size = new Size(44, 38);
            otpBox3.TabIndex = 5;
            otpBox3.TextAlign = HorizontalAlignment.Center;
            // 
            // otpBox4
            // 
            otpBox4.BackColor = Color.White;
            otpBox4.BorderStyle = BorderStyle.FixedSingle;
            otpBox4.Enabled = false;
            otpBox4.Font = new Font("Segoe UI", 16F, FontStyle.Bold, GraphicsUnit.Point, 0);
            otpBox4.ForeColor = Color.FromArgb(31, 42, 60);
            otpBox4.Location = new Point(196, 104);
            otpBox4.MaxLength = 1;
            otpBox4.Name = "otpBox4";
            otpBox4.Size = new Size(44, 38);
            otpBox4.TabIndex = 6;
            otpBox4.TextAlign = HorizontalAlignment.Center;
            // 
            // otpBox5
            // 
            otpBox5.BackColor = Color.White;
            otpBox5.BorderStyle = BorderStyle.FixedSingle;
            otpBox5.Enabled = false;
            otpBox5.Font = new Font("Segoe UI", 16F, FontStyle.Bold, GraphicsUnit.Point, 0);
            otpBox5.ForeColor = Color.FromArgb(31, 42, 60);
            otpBox5.Location = new Point(253, 104);
            otpBox5.MaxLength = 1;
            otpBox5.Name = "otpBox5";
            otpBox5.Size = new Size(44, 38);
            otpBox5.TabIndex = 7;
            otpBox5.TextAlign = HorizontalAlignment.Center;
            // 
            // otpBox6
            // 
            otpBox6.BackColor = Color.White;
            otpBox6.BorderStyle = BorderStyle.FixedSingle;
            otpBox6.Enabled = false;
            otpBox6.Font = new Font("Segoe UI", 16F, FontStyle.Bold, GraphicsUnit.Point, 0);
            otpBox6.ForeColor = Color.FromArgb(31, 42, 60);
            otpBox6.Location = new Point(310, 104);
            otpBox6.MaxLength = 1;
            otpBox6.Name = "otpBox6";
            otpBox6.Size = new Size(44, 38);
            otpBox6.TabIndex = 8;
            otpBox6.TextAlign = HorizontalAlignment.Center;
            // 
            // otpErrorLabel
            // 
            otpErrorLabel.BackColor = Color.White;
            otpErrorLabel.Font = new Font("Segoe UI", 8.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            otpErrorLabel.ForeColor = Color.FromArgb(239, 68, 68);
            otpErrorLabel.Location = new Point(24, 156);
            otpErrorLabel.Name = "otpErrorLabel";
            otpErrorLabel.Size = new Size(332, 36);
            otpErrorLabel.TabIndex = 9;
            otpErrorLabel.Text = "";
            otpErrorLabel.TextAlign = ContentAlignment.TopLeft;
            otpErrorLabel.UseMnemonic = false;
            // 
            // verifyButton
            // 
            verifyButton.BackColor = Color.White;
            verifyButton.Enabled = false;
            verifyButton.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            verifyButton.Location = new Point(24, 198);
            verifyButton.Name = "verifyButton";
            verifyButton.Radius = 12;
            verifyButton.Size = new Size(332, 44);
            verifyButton.TabIndex = 10;
            verifyButton.Text = "Verify";
            // 
            // resendLabel
            // 
            resendLabel.BackColor = Color.White;
            resendLabel.Cursor = Cursors.Hand;
            resendLabel.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            resendLabel.ForeColor = Color.FromArgb(58, 123, 213);
            resendLabel.Location = new Point(24, 254);
            resendLabel.Name = "resendLabel";
            resendLabel.Size = new Size(332, 22);
            resendLabel.TabIndex = 11;
            resendLabel.Text = "Resend OTP";
            resendLabel.TextAlign = ContentAlignment.MiddleCenter;
            resendLabel.UseMnemonic = false;
            resendLabel.Visible = false;
            // 
            // otpCard
            // 
            otpCard.Controls.Add(resendLabel);
            otpCard.Controls.Add(verifyButton);
            otpCard.Controls.Add(otpErrorLabel);
            otpCard.Controls.Add(otpBox6);
            otpCard.Controls.Add(otpBox5);
            otpCard.Controls.Add(otpBox4);
            otpCard.Controls.Add(otpBox3);
            otpCard.Controls.Add(otpBox2);
            otpCard.Controls.Add(otpBox1);
            otpCard.Controls.Add(otpHint);
            otpCard.Controls.Add(otpTitle);
            otpCard.Controls.Add(otpIcon);
            otpCard.BackColor = Color.FromArgb(243, 247, 252);
            otpCard.BorderColor = Color.FromArgb(227, 233, 242);
            otpCard.CornerColor = Color.FromArgb(243, 247, 252);
            otpCard.FillColor = Color.White;
            otpCard.Location = new Point(480, 0);
            otpCard.Margin = new Padding(0);
            otpCard.Name = "otpCard";
            otpCard.Radius = 18;
            otpCard.Size = new Size(380, 300);
            otpCard.TabIndex = 8;
            // 
            // contactPanel
            // 
            contactPanel.Controls.Add(otpCard);
            contactPanel.Controls.Add(orBadge);
            contactPanel.Controls.Add(dividerLine);
            contactPanel.Controls.Add(sendOtpButton);
            contactPanel.Controls.Add(contactErrorLabel);
            contactPanel.Controls.Add(mobileField);
            contactPanel.Controls.Add(mobileLabel);
            contactPanel.Controls.Add(emailField);
            contactPanel.Controls.Add(emailLabel);
            contactPanel.BackColor = Color.FromArgb(243, 247, 252);
            contactPanel.Location = new Point(0, 150);
            contactPanel.Name = "contactPanel";
            contactPanel.Size = new Size(900, 330);
            contactPanel.TabIndex = 7;
            // 
            // ownerLabel
            // 
            ownerLabel.BackColor = Color.White;
            ownerLabel.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ownerLabel.ForeColor = Color.FromArgb(122, 136, 156);
            ownerLabel.Location = new Point(24, 18);
            ownerLabel.Name = "ownerLabel";
            ownerLabel.Size = new Size(332, 20);
            ownerLabel.TabIndex = 0;
            ownerLabel.Text = "";
            ownerLabel.TextAlign = ContentAlignment.MiddleLeft;
            ownerLabel.UseMnemonic = false;
            // 
            // petNameLabel
            // 
            petNameLabel.BackColor = Color.White;
            petNameLabel.Font = new Font("Segoe UI", 7.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            petNameLabel.ForeColor = Color.FromArgb(68, 83, 106);
            petNameLabel.Location = new Point(24, 46);
            petNameLabel.Name = "petNameLabel";
            petNameLabel.Size = new Size(332, 16);
            petNameLabel.TabIndex = 1;
            petNameLabel.Text = "PET NAME";
            petNameLabel.TextAlign = ContentAlignment.MiddleLeft;
            petNameLabel.UseMnemonic = false;
            // 
            // petNameField
            // 
            petNameField.BorderColor = Color.FromArgb(227, 233, 242);
            petNameField.CornerColor = Color.White;
            petNameField.FillColor = Color.White;
            petNameField.Glyph = "\uE77B";
            petNameField.Location = new Point(24, 64);
            petNameField.Margin = new Padding(0);
            petNameField.Name = "petNameField";
            petNameField.Padding = new Padding(10, 3, 10, 3);
            petNameField.Placeholder = "e.g. Bella";
            petNameField.Radius = 12;
            petNameField.Size = new Size(332, 46);
            petNameField.TabIndex = 2;
            // 
            // speciesLabel
            // 
            speciesLabel.BackColor = Color.White;
            speciesLabel.Font = new Font("Segoe UI", 7.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            speciesLabel.ForeColor = Color.FromArgb(68, 83, 106);
            speciesLabel.Location = new Point(24, 118);
            speciesLabel.Name = "speciesLabel";
            speciesLabel.Size = new Size(332, 16);
            speciesLabel.TabIndex = 3;
            speciesLabel.Text = "SPECIES";
            speciesLabel.TextAlign = ContentAlignment.MiddleLeft;
            speciesLabel.UseMnemonic = false;
            // 
            // speciesField
            // 
            speciesField.BorderColor = Color.FromArgb(227, 233, 242);
            speciesField.CornerColor = Color.White;
            speciesField.FillColor = Color.White;
            speciesField.Glyph = "\uE8EC";
            speciesField.Location = new Point(24, 136);
            speciesField.Margin = new Padding(0);
            speciesField.Name = "speciesField";
            speciesField.Padding = new Padding(10, 3, 10, 3);
            speciesField.Placeholder = "e.g. Dog, Cat";
            speciesField.Radius = 12;
            speciesField.Size = new Size(332, 46);
            speciesField.TabIndex = 4;
            // 
            // breedLabel
            // 
            breedLabel.BackColor = Color.White;
            breedLabel.Font = new Font("Segoe UI", 7.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            breedLabel.ForeColor = Color.FromArgb(68, 83, 106);
            breedLabel.Location = new Point(24, 190);
            breedLabel.Name = "breedLabel";
            breedLabel.Size = new Size(332, 16);
            breedLabel.TabIndex = 5;
            breedLabel.Text = "BREED";
            breedLabel.TextAlign = ContentAlignment.MiddleLeft;
            breedLabel.UseMnemonic = false;
            // 
            // breedField
            // 
            breedField.BorderColor = Color.FromArgb(227, 233, 242);
            breedField.CornerColor = Color.White;
            breedField.FillColor = Color.White;
            breedField.Glyph = "\uE734";
            breedField.Location = new Point(24, 208);
            breedField.Margin = new Padding(0);
            breedField.Name = "breedField";
            breedField.Padding = new Padding(10, 3, 10, 3);
            breedField.Placeholder = "e.g. Shih Tzu";
            breedField.Radius = 12;
            breedField.Size = new Size(332, 46);
            breedField.TabIndex = 6;
            // 
            // petErrorLabel
            // 
            petErrorLabel.BackColor = Color.White;
            petErrorLabel.Font = new Font("Segoe UI", 8.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            petErrorLabel.ForeColor = Color.FromArgb(239, 68, 68);
            petErrorLabel.Location = new Point(24, 258);
            petErrorLabel.Name = "petErrorLabel";
            petErrorLabel.Size = new Size(332, 20);
            petErrorLabel.TabIndex = 7;
            petErrorLabel.Text = "";
            petErrorLabel.TextAlign = ContentAlignment.MiddleLeft;
            petErrorLabel.UseMnemonic = false;
            // 
            // savePetButton
            // 
            savePetButton.BackColor = Color.White;
            savePetButton.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            savePetButton.Location = new Point(24, 280);
            savePetButton.Name = "savePetButton";
            savePetButton.Radius = 12;
            savePetButton.Size = new Size(332, 40);
            savePetButton.TabIndex = 8;
            savePetButton.Text = "Add Pet  \u2192";
            // 
            // petCard
            // 
            petCard.Controls.Add(savePetButton);
            petCard.Controls.Add(petErrorLabel);
            petCard.Controls.Add(breedField);
            petCard.Controls.Add(breedLabel);
            petCard.Controls.Add(speciesField);
            petCard.Controls.Add(speciesLabel);
            petCard.Controls.Add(petNameField);
            petCard.Controls.Add(petNameLabel);
            petCard.Controls.Add(ownerLabel);
            petCard.BackColor = Color.FromArgb(243, 247, 252);
            petCard.BorderColor = Color.FromArgb(227, 233, 242);
            petCard.CornerColor = Color.FromArgb(243, 247, 252);
            petCard.FillColor = Color.White;
            petCard.Location = new Point(260, 0);
            petCard.Margin = new Padding(0);
            petCard.Name = "petCard";
            petCard.Radius = 18;
            petCard.Size = new Size(380, 330);
            petCard.TabIndex = 0;
            // 
            // petPanel
            // 
            petPanel.Controls.Add(petCard);
            petPanel.BackColor = Color.FromArgb(243, 247, 252);
            petPanel.Location = new Point(0, 150);
            petPanel.Name = "petPanel";
            petPanel.Size = new Size(900, 330);
            petPanel.TabIndex = 8;
            petPanel.Visible = false;
            // 
            // backButton
            // 
            backButton.BackColor = Color.White;
            backButton.Cursor = Cursors.Hand;
            backButton.FlatStyle = FlatStyle.Flat;
            backButton.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            backButton.ForeColor = Color.FromArgb(68, 83, 106);
            backButton.Location = new Point(40, 490);
            backButton.Name = "backButton";
            backButton.Size = new Size(96, 36);
            backButton.TabIndex = 9;
            backButton.Text = "\u2190  Back";
            backButton.UseVisualStyleBackColor = false;
            // 
            // cornerPaw
            // 
            cornerPaw.BackColor = Color.FromArgb(243, 247, 252);
            cornerPaw.FillColor = Color.FromArgb(243, 247, 252);
            cornerPaw.Location = new Point(812, 484);
            cornerPaw.Name = "cornerPaw";
            cornerPaw.PawColor = Color.FromArgb(214, 230, 251);
            cornerPaw.Radius = 10;
            cornerPaw.Size = new Size(40, 40);
            cornerPaw.TabStop = false;
            // 
            // resendTimer
            // 
            resendTimer.Interval = 1000;
            // 
            // AddPetForm
            // 
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            BackColor = Color.FromArgb(243, 247, 252);
            ClientSize = new Size(900, 540);
            Controls.Add(cornerPaw);
            Controls.Add(backButton);
            Controls.Add(petPanel);
            Controls.Add(contactPanel);
            Controls.Add(subtitleLabel);
            Controls.Add(titleLabel);
            Controls.Add(stepPet);
            Controls.Add(stepLine);
            Controls.Add(stepContact);
            Controls.Add(brandLabel);
            Controls.Add(logoBadge);
            Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            KeyPreview = true;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "AddPetForm";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Pawfect - Add Pet";
            orBadge.ResumeLayout(false);
            otpCard.ResumeLayout(false);
            contactPanel.ResumeLayout(false);
            petCard.ResumeLayout(false);
            petPanel.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private MyApp.Controls.PawBadge logoBadge;
        private System.Windows.Forms.Label brandLabel;
        private MyApp.Controls.StepDot stepContact;
        private System.Windows.Forms.Panel stepLine;
        private MyApp.Controls.StepDot stepPet;
        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.Label subtitleLabel;
        private System.Windows.Forms.Label emailLabel;
        private MyApp.Controls.InputField emailField;
        private System.Windows.Forms.Label mobileLabel;
        private MyApp.Controls.InputField mobileField;
        private System.Windows.Forms.Label contactErrorLabel;
        private MyApp.Controls.PrimaryButton sendOtpButton;
        private System.Windows.Forms.Panel dividerLine;
        private System.Windows.Forms.Label orLabel;
        private MyApp.Controls.CardPanel orBadge;
        private System.Windows.Forms.Label otpIcon;
        private System.Windows.Forms.Label otpTitle;
        private System.Windows.Forms.Label otpHint;
        private System.Windows.Forms.TextBox otpBox1;
        private System.Windows.Forms.TextBox otpBox2;
        private System.Windows.Forms.TextBox otpBox3;
        private System.Windows.Forms.TextBox otpBox4;
        private System.Windows.Forms.TextBox otpBox5;
        private System.Windows.Forms.TextBox otpBox6;
        private System.Windows.Forms.Label otpErrorLabel;
        private MyApp.Controls.PrimaryButton verifyButton;
        private System.Windows.Forms.Label resendLabel;
        private MyApp.Controls.CardPanel otpCard;
        private System.Windows.Forms.Panel contactPanel;
        private System.Windows.Forms.Label ownerLabel;
        private System.Windows.Forms.Label petNameLabel;
        private MyApp.Controls.InputField petNameField;
        private System.Windows.Forms.Label speciesLabel;
        private MyApp.Controls.InputField speciesField;
        private System.Windows.Forms.Label breedLabel;
        private MyApp.Controls.InputField breedField;
        private System.Windows.Forms.Label petErrorLabel;
        private MyApp.Controls.PrimaryButton savePetButton;
        private MyApp.Controls.CardPanel petCard;
        private System.Windows.Forms.Panel petPanel;
        private System.Windows.Forms.Button backButton;
        private MyApp.Controls.PawBadge cornerPaw;
        private System.Windows.Forms.Timer resendTimer;
    }
}
