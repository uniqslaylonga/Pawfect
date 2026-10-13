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
            stepLine1 = new System.Windows.Forms.Panel();
            stepSelect = new MyApp.Controls.StepDot();
            stepLine2 = new System.Windows.Forms.Panel();
            stepVaccine = new MyApp.Controls.StepDot();
            stepLine3 = new System.Windows.Forms.Panel();
            stepGrooming = new MyApp.Controls.StepDot();
            stepLine4 = new System.Windows.Forms.Panel();
            stepTicket = new MyApp.Controls.StepDot();
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
            tabMyPets = new MyApp.Controls.SegmentButton();
            tabAddPet = new MyApp.Controls.SegmentButton();
            newPetPaw = new MyApp.Controls.PawBadge();
            newPetTitle = new System.Windows.Forms.Label();
            newPetHint = new System.Windows.Forms.Label();
            newPetArrowLabel = new System.Windows.Forms.Label();
            newPetArrow = new MyApp.Controls.CardPanel();
            newPetCard = new MyApp.Controls.CardPanel();
            petsFlow = new System.Windows.Forms.FlowLayoutPanel();
            myPetsView = new System.Windows.Forms.Panel();
            ownerLabel = new System.Windows.Forms.Label();
            petNameLabel = new System.Windows.Forms.Label();
            petNameField = new MyApp.Controls.InputField();
            speciesLabel = new System.Windows.Forms.Label();
            speciesField = new MyApp.Controls.InputField();
            breedLabel = new System.Windows.Forms.Label();
            breedField = new MyApp.Controls.InputField();
            petErrorLabel = new System.Windows.Forms.Label();
            savePetButton = new MyApp.Controls.PrimaryButton();
            addPetCard = new MyApp.Controls.CardPanel();
            addPetView = new System.Windows.Forms.Panel();
            selectPanel = new System.Windows.Forms.Panel();
            backButton = new System.Windows.Forms.Button();
            continueButton = new MyApp.Controls.PrimaryButton();
            resendTimer = new System.Windows.Forms.Timer(components);
            orBadge.SuspendLayout();
            otpCard.SuspendLayout();
            contactPanel.SuspendLayout();
            newPetArrow.SuspendLayout();
            newPetCard.SuspendLayout();
            petsFlow.SuspendLayout();
            myPetsView.SuspendLayout();
            addPetCard.SuspendLayout();
            addPetView.SuspendLayout();
            selectPanel.SuspendLayout();
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
            stepContact.Location = new Point(340, 14);
            stepContact.Name = "stepContact";
            stepContact.Number = 1;
            stepContact.Size = new Size(80, 48);
            stepContact.State = MyApp.Controls.StepState.Active;
            stepContact.TabStop = false;
            stepContact.Text = "Contact";
            // 
            // stepLine1
            // 
            stepLine1.BackColor = Color.FromArgb(227, 233, 242);
            stepLine1.Location = new Point(420, 25);
            stepLine1.Name = "stepLine1";
            stepLine1.Size = new Size(30, 1);
            stepLine1.TabIndex = 3;
            // 
            // stepSelect
            // 
            stepSelect.BackColor = Color.FromArgb(243, 247, 252);
            stepSelect.Location = new Point(450, 14);
            stepSelect.Name = "stepSelect";
            stepSelect.Number = 2;
            stepSelect.Size = new Size(80, 48);
            stepSelect.State = MyApp.Controls.StepState.Pending;
            stepSelect.TabStop = false;
            stepSelect.Text = "Select Pet";
            // 
            // stepLine2
            // 
            stepLine2.BackColor = Color.FromArgb(227, 233, 242);
            stepLine2.Location = new Point(530, 25);
            stepLine2.Name = "stepLine2";
            stepLine2.Size = new Size(30, 1);
            stepLine2.TabIndex = 5;
            // 
            // stepVaccine
            // 
            stepVaccine.BackColor = Color.FromArgb(243, 247, 252);
            stepVaccine.Location = new Point(560, 14);
            stepVaccine.Name = "stepVaccine";
            stepVaccine.Number = 3;
            stepVaccine.Size = new Size(80, 48);
            stepVaccine.State = MyApp.Controls.StepState.Pending;
            stepVaccine.TabStop = false;
            stepVaccine.Text = "Vaccine Check";
            // 
            // stepLine3
            // 
            stepLine3.BackColor = Color.FromArgb(227, 233, 242);
            stepLine3.Location = new Point(640, 25);
            stepLine3.Name = "stepLine3";
            stepLine3.Size = new Size(30, 1);
            stepLine3.TabIndex = 7;
            // 
            // stepGrooming
            // 
            stepGrooming.BackColor = Color.FromArgb(243, 247, 252);
            stepGrooming.Location = new Point(670, 14);
            stepGrooming.Name = "stepGrooming";
            stepGrooming.Number = 4;
            stepGrooming.Size = new Size(80, 48);
            stepGrooming.State = MyApp.Controls.StepState.Pending;
            stepGrooming.TabStop = false;
            stepGrooming.Text = "Grooming";
            // 
            // stepLine4
            // 
            stepLine4.BackColor = Color.FromArgb(227, 233, 242);
            stepLine4.Location = new Point(750, 25);
            stepLine4.Name = "stepLine4";
            stepLine4.Size = new Size(30, 1);
            stepLine4.TabIndex = 9;
            // 
            // stepTicket
            // 
            stepTicket.BackColor = Color.FromArgb(243, 247, 252);
            stepTicket.Location = new Point(780, 14);
            stepTicket.Name = "stepTicket";
            stepTicket.Number = 5;
            stepTicket.Size = new Size(80, 48);
            stepTicket.State = MyApp.Controls.StepState.Pending;
            stepTicket.TabStop = false;
            stepTicket.Text = "Ticket";
            // 
            // titleLabel
            // 
            titleLabel.BackColor = Color.FromArgb(243, 247, 252);
            titleLabel.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            titleLabel.ForeColor = Color.FromArgb(19, 41, 75);
            titleLabel.Location = new Point(40, 84);
            titleLabel.Name = "titleLabel";
            titleLabel.Size = new Size(820, 36);
            titleLabel.TabIndex = 11;
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
            subtitleLabel.TabIndex = 12;
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
            contactPanel.TabIndex = 13;
            // 
            // tabMyPets
            // 
            tabMyPets.Active = true;
            tabMyPets.BackColor = Color.FromArgb(243, 247, 252);
            tabMyPets.Glyph = "\uE77B";
            tabMyPets.Location = new Point(40, 0);
            tabMyPets.Name = "tabMyPets";
            tabMyPets.Size = new Size(403, 44);
            tabMyPets.TabIndex = 0;
            tabMyPets.Text = "My Pets";
            // 
            // tabAddPet
            // 
            tabAddPet.BackColor = Color.FromArgb(243, 247, 252);
            tabAddPet.Glyph = "\uE710";
            tabAddPet.Location = new Point(457, 0);
            tabAddPet.Name = "tabAddPet";
            tabAddPet.Size = new Size(403, 44);
            tabAddPet.TabIndex = 1;
            tabAddPet.Text = "Add New Pet";
            // 
            // newPetPaw
            // 
            newPetPaw.BackColor = Color.FromArgb(250, 252, 255);
            newPetPaw.FillColor = Color.FromArgb(250, 252, 255);
            newPetPaw.Location = new Point(110, 30);
            newPetPaw.Name = "newPetPaw";
            newPetPaw.PawColor = Color.FromArgb(96, 165, 250);
            newPetPaw.Radius = 10;
            newPetPaw.Size = new Size(40, 40);
            newPetPaw.TabStop = false;
            // 
            // newPetTitle
            // 
            newPetTitle.BackColor = Color.FromArgb(250, 252, 255);
            newPetTitle.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            newPetTitle.ForeColor = Color.FromArgb(31, 42, 60);
            newPetTitle.Location = new Point(0, 84);
            newPetTitle.Name = "newPetTitle";
            newPetTitle.Size = new Size(260, 24);
            newPetTitle.TabIndex = 1;
            newPetTitle.Text = "New Pet?";
            newPetTitle.TextAlign = ContentAlignment.MiddleCenter;
            newPetTitle.UseMnemonic = false;
            // 
            // newPetHint
            // 
            newPetHint.BackColor = Color.FromArgb(250, 252, 255);
            newPetHint.Font = new Font("Segoe UI", 8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            newPetHint.ForeColor = Color.FromArgb(122, 136, 156);
            newPetHint.Location = new Point(0, 110);
            newPetHint.Name = "newPetHint";
            newPetHint.Size = new Size(260, 22);
            newPetHint.TabIndex = 2;
            newPetHint.Text = "Add your pet's details.";
            newPetHint.TextAlign = ContentAlignment.MiddleCenter;
            newPetHint.UseMnemonic = false;
            // 
            // newPetArrowLabel
            // 
            newPetArrowLabel.BackColor = Color.FromArgb(96, 165, 250);
            newPetArrowLabel.Dock = DockStyle.Fill;
            newPetArrowLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            newPetArrowLabel.ForeColor = Color.White;
            newPetArrowLabel.Name = "newPetArrowLabel";
            newPetArrowLabel.Text = "\u2192";
            newPetArrowLabel.TextAlign = ContentAlignment.MiddleCenter;
            newPetArrowLabel.UseMnemonic = false;
            // 
            // newPetArrow
            // 
            newPetArrow.Controls.Add(newPetArrowLabel);
            newPetArrow.BackColor = Color.FromArgb(250, 252, 255);
            newPetArrow.BorderColor = Color.FromArgb(96, 165, 250);
            newPetArrow.CornerColor = Color.FromArgb(250, 252, 255);
            newPetArrow.FillColor = Color.FromArgb(96, 165, 250);
            newPetArrow.Location = new Point(116, 152);
            newPetArrow.Margin = new Padding(0);
            newPetArrow.Name = "newPetArrow";
            newPetArrow.Radius = 14;
            newPetArrow.Size = new Size(28, 28);
            newPetArrow.TabIndex = 3;
            // 
            // newPetCard
            // 
            newPetCard.Controls.Add(newPetArrow);
            newPetCard.Controls.Add(newPetHint);
            newPetCard.Controls.Add(newPetTitle);
            newPetCard.Controls.Add(newPetPaw);
            newPetCard.BackColor = Color.FromArgb(243, 247, 252);
            newPetCard.BorderColor = Color.FromArgb(214, 230, 251);
            newPetCard.CornerColor = Color.FromArgb(243, 247, 252);
            newPetCard.Cursor = Cursors.Hand;
            newPetCard.FillColor = Color.FromArgb(250, 252, 255);
            newPetCard.Location = new Point(0, 0);
            newPetCard.Margin = new Padding(0, 0, 20, 20);
            newPetCard.Name = "newPetCard";
            newPetCard.Radius = 14;
            newPetCard.Size = new Size(260, 200);
            newPetCard.TabIndex = 0;
            // 
            // petsFlow
            // 
            petsFlow.Controls.Add(newPetCard);
            petsFlow.AutoScroll = true;
            petsFlow.BackColor = Color.FromArgb(243, 247, 252);
            petsFlow.Dock = DockStyle.Fill;
            petsFlow.Location = new Point(0, 0);
            petsFlow.Name = "petsFlow";
            petsFlow.Size = new Size(820, 262);
            petsFlow.TabIndex = 0;
            // 
            // myPetsView
            // 
            myPetsView.Controls.Add(petsFlow);
            myPetsView.BackColor = Color.FromArgb(243, 247, 252);
            myPetsView.Location = new Point(40, 60);
            myPetsView.Name = "myPetsView";
            myPetsView.Size = new Size(820, 262);
            myPetsView.TabIndex = 2;
            // 
            // ownerLabel
            // 
            ownerLabel.BackColor = Color.White;
            ownerLabel.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ownerLabel.ForeColor = Color.FromArgb(122, 136, 156);
            ownerLabel.Location = new Point(24, 12);
            ownerLabel.Name = "ownerLabel";
            ownerLabel.Size = new Size(772, 20);
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
            petNameLabel.Location = new Point(24, 40);
            petNameLabel.Name = "petNameLabel";
            petNameLabel.Size = new Size(236, 16);
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
            petNameField.Location = new Point(24, 58);
            petNameField.Margin = new Padding(0);
            petNameField.Name = "petNameField";
            petNameField.Padding = new Padding(10, 3, 10, 3);
            petNameField.Placeholder = "e.g. Bella";
            petNameField.Radius = 12;
            petNameField.Size = new Size(236, 46);
            petNameField.TabIndex = 2;
            // 
            // speciesLabel
            // 
            speciesLabel.BackColor = Color.White;
            speciesLabel.Font = new Font("Segoe UI", 7.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            speciesLabel.ForeColor = Color.FromArgb(68, 83, 106);
            speciesLabel.Location = new Point(292, 40);
            speciesLabel.Name = "speciesLabel";
            speciesLabel.Size = new Size(236, 16);
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
            speciesField.Location = new Point(292, 58);
            speciesField.Margin = new Padding(0);
            speciesField.Name = "speciesField";
            speciesField.Padding = new Padding(10, 3, 10, 3);
            speciesField.Placeholder = "e.g. Dog, Cat";
            speciesField.Radius = 12;
            speciesField.Size = new Size(236, 46);
            speciesField.TabIndex = 4;
            // 
            // breedLabel
            // 
            breedLabel.BackColor = Color.White;
            breedLabel.Font = new Font("Segoe UI", 7.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            breedLabel.ForeColor = Color.FromArgb(68, 83, 106);
            breedLabel.Location = new Point(560, 40);
            breedLabel.Name = "breedLabel";
            breedLabel.Size = new Size(236, 16);
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
            breedField.Location = new Point(560, 58);
            breedField.Margin = new Padding(0);
            breedField.Name = "breedField";
            breedField.Padding = new Padding(10, 3, 10, 3);
            breedField.Placeholder = "e.g. Shih Tzu";
            breedField.Radius = 12;
            breedField.Size = new Size(236, 46);
            breedField.TabIndex = 6;
            // 
            // petErrorLabel
            // 
            petErrorLabel.BackColor = Color.White;
            petErrorLabel.Font = new Font("Segoe UI", 8.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            petErrorLabel.ForeColor = Color.FromArgb(239, 68, 68);
            petErrorLabel.Location = new Point(24, 114);
            petErrorLabel.Name = "petErrorLabel";
            petErrorLabel.Size = new Size(772, 20);
            petErrorLabel.TabIndex = 7;
            petErrorLabel.Text = "";
            petErrorLabel.TextAlign = ContentAlignment.MiddleLeft;
            petErrorLabel.UseMnemonic = false;
            // 
            // savePetButton
            // 
            savePetButton.BackColor = Color.White;
            savePetButton.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            savePetButton.Location = new Point(24, 142);
            savePetButton.Name = "savePetButton";
            savePetButton.Radius = 12;
            savePetButton.Size = new Size(260, 44);
            savePetButton.TabIndex = 8;
            savePetButton.Text = "Add Pet  \u2192";
            // 
            // addPetCard
            // 
            addPetCard.Controls.Add(savePetButton);
            addPetCard.Controls.Add(petErrorLabel);
            addPetCard.Controls.Add(breedField);
            addPetCard.Controls.Add(breedLabel);
            addPetCard.Controls.Add(speciesField);
            addPetCard.Controls.Add(speciesLabel);
            addPetCard.Controls.Add(petNameField);
            addPetCard.Controls.Add(petNameLabel);
            addPetCard.Controls.Add(ownerLabel);
            addPetCard.BackColor = Color.FromArgb(243, 247, 252);
            addPetCard.BorderColor = Color.FromArgb(227, 233, 242);
            addPetCard.CornerColor = Color.FromArgb(243, 247, 252);
            addPetCard.FillColor = Color.White;
            addPetCard.Location = new Point(0, 0);
            addPetCard.Margin = new Padding(0);
            addPetCard.Name = "addPetCard";
            addPetCard.Radius = 14;
            addPetCard.Size = new Size(820, 200);
            addPetCard.TabIndex = 0;
            // 
            // addPetView
            // 
            addPetView.Controls.Add(addPetCard);
            addPetView.BackColor = Color.FromArgb(243, 247, 252);
            addPetView.Location = new Point(40, 60);
            addPetView.Name = "addPetView";
            addPetView.Size = new Size(820, 262);
            addPetView.TabIndex = 3;
            addPetView.Visible = false;
            // 
            // selectPanel
            // 
            selectPanel.Controls.Add(addPetView);
            selectPanel.Controls.Add(myPetsView);
            selectPanel.Controls.Add(tabAddPet);
            selectPanel.Controls.Add(tabMyPets);
            selectPanel.BackColor = Color.FromArgb(243, 247, 252);
            selectPanel.Location = new Point(0, 150);
            selectPanel.Name = "selectPanel";
            selectPanel.Size = new Size(900, 330);
            selectPanel.TabIndex = 14;
            selectPanel.Visible = false;
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
            backButton.TabIndex = 15;
            backButton.Text = "\u2190  Back";
            backButton.UseVisualStyleBackColor = false;
            // 
            // continueButton
            // 
            continueButton.BackColor = Color.White;
            continueButton.Enabled = false;
            continueButton.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            continueButton.Location = new Point(730, 486);
            continueButton.Name = "continueButton";
            continueButton.Radius = 12;
            continueButton.Size = new Size(130, 42);
            continueButton.TabIndex = 16;
            continueButton.Text = "Continue  \u2192";
            continueButton.Visible = false;
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
            Controls.Add(continueButton);
            Controls.Add(backButton);
            Controls.Add(selectPanel);
            Controls.Add(contactPanel);
            Controls.Add(subtitleLabel);
            Controls.Add(titleLabel);
            Controls.Add(stepTicket);
            Controls.Add(stepLine4);
            Controls.Add(stepGrooming);
            Controls.Add(stepLine3);
            Controls.Add(stepVaccine);
            Controls.Add(stepLine2);
            Controls.Add(stepSelect);
            Controls.Add(stepLine1);
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
            newPetArrow.ResumeLayout(false);
            newPetCard.ResumeLayout(false);
            petsFlow.ResumeLayout(false);
            myPetsView.ResumeLayout(false);
            addPetCard.ResumeLayout(false);
            addPetView.ResumeLayout(false);
            selectPanel.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private MyApp.Controls.PawBadge logoBadge;
        private System.Windows.Forms.Label brandLabel;
        private MyApp.Controls.StepDot stepContact;
        private System.Windows.Forms.Panel stepLine1;
        private MyApp.Controls.StepDot stepSelect;
        private System.Windows.Forms.Panel stepLine2;
        private MyApp.Controls.StepDot stepVaccine;
        private System.Windows.Forms.Panel stepLine3;
        private MyApp.Controls.StepDot stepGrooming;
        private System.Windows.Forms.Panel stepLine4;
        private MyApp.Controls.StepDot stepTicket;
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
        private MyApp.Controls.SegmentButton tabMyPets;
        private MyApp.Controls.SegmentButton tabAddPet;
        private MyApp.Controls.PawBadge newPetPaw;
        private System.Windows.Forms.Label newPetTitle;
        private System.Windows.Forms.Label newPetHint;
        private System.Windows.Forms.Label newPetArrowLabel;
        private MyApp.Controls.CardPanel newPetArrow;
        private MyApp.Controls.CardPanel newPetCard;
        private System.Windows.Forms.FlowLayoutPanel petsFlow;
        private System.Windows.Forms.Panel myPetsView;
        private System.Windows.Forms.Label ownerLabel;
        private System.Windows.Forms.Label petNameLabel;
        private MyApp.Controls.InputField petNameField;
        private System.Windows.Forms.Label speciesLabel;
        private MyApp.Controls.InputField speciesField;
        private System.Windows.Forms.Label breedLabel;
        private MyApp.Controls.InputField breedField;
        private System.Windows.Forms.Label petErrorLabel;
        private MyApp.Controls.PrimaryButton savePetButton;
        private MyApp.Controls.CardPanel addPetCard;
        private System.Windows.Forms.Panel addPetView;
        private System.Windows.Forms.Panel selectPanel;
        private System.Windows.Forms.Button backButton;
        private MyApp.Controls.PrimaryButton continueButton;
        private System.Windows.Forms.Timer resendTimer;
    }
}
