#nullable disable
namespace MyApp
{
    partial class LoginForm
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
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginForm));
            backdrop = new MyApp.Controls.BackdropPanel();
            logoBadge = new MyApp.Controls.PawBadge();
            brandLabel = new Label();
            taglineLabel = new Label();
            welcomeLabel = new Label();
            introLabel = new Label();
            petsPicture = new PictureBox();
            heartLabel = new Label();
            scriptLabel = new Label();
            card = new MyApp.Controls.CardPanel();
            cardBadge = new MyApp.Controls.PawBadge();
            titleLabel = new Label();
            subtitleLabel = new Label();
            emailLabel = new Label();
            emailField = new MyApp.Controls.InputField();
            passwordLabel = new Label();
            passwordField = new MyApp.Controls.InputField();
            rememberCheck = new CheckBox();
            errorLabel = new Label();
            signInButton = new MyApp.Controls.PrimaryButton();
            signUpPrompt = new Label();
            signUpLink = new Label();
            backdrop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)petsPicture).BeginInit();
            card.SuspendLayout();
            SuspendLayout();
            // 
            // backdrop
            // 
            backdrop.Controls.Add(card);
            backdrop.Controls.Add(scriptLabel);
            backdrop.Controls.Add(heartLabel);
            backdrop.Controls.Add(petsPicture);
            backdrop.Controls.Add(introLabel);
            backdrop.Controls.Add(welcomeLabel);
            backdrop.Controls.Add(taglineLabel);
            backdrop.Controls.Add(brandLabel);
            backdrop.Controls.Add(logoBadge);
            backdrop.Dock = DockStyle.Fill;
            backdrop.GradientEnd = Color.FromArgb(225, 237, 254);
            backdrop.GradientStart = Color.FromArgb(242, 248, 255);
            backdrop.Location = new Point(0, 0);
            backdrop.Name = "backdrop";
            backdrop.PawColor = Color.FromArgb(205, 224, 250);
            backdrop.ShadowTarget = card;
            backdrop.Size = new Size(1000, 620);
            backdrop.TabIndex = 0;
            // 
            // logoBadge
            // 
            logoBadge.BackColor = Color.Transparent;
            logoBadge.FillColor = Color.FromArgb(58, 123, 213);
            logoBadge.Location = new Point(58, 58);
            logoBadge.Name = "logoBadge";
            logoBadge.PawColor = Color.White;
            logoBadge.Radius = 12;
            logoBadge.Size = new Size(42, 42);
            logoBadge.TabIndex = 0;
            logoBadge.TabStop = false;
            // 
            // brandLabel
            // 
            brandLabel.BackColor = Color.Transparent;
            brandLabel.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            brandLabel.ForeColor = Color.FromArgb(19, 41, 75);
            brandLabel.Location = new Point(108, 54);
            brandLabel.Name = "brandLabel";
            brandLabel.Size = new Size(180, 32);
            brandLabel.TabIndex = 1;
            brandLabel.Text = "Pawfect";
            brandLabel.TextAlign = ContentAlignment.MiddleLeft;
            brandLabel.UseMnemonic = false;
            // 
            // taglineLabel
            // 
            taglineLabel.BackColor = Color.Transparent;
            taglineLabel.Font = new Font("Segoe UI", 7.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            taglineLabel.ForeColor = Color.FromArgb(58, 123, 213);
            taglineLabel.Location = new Point(110, 87);
            taglineLabel.Name = "taglineLabel";
            taglineLabel.Size = new Size(260, 16);
            taglineLabel.TabIndex = 2;
            taglineLabel.Text = "GROOMING • HEALTH • HAPPINESS";
            taglineLabel.TextAlign = ContentAlignment.MiddleLeft;
            taglineLabel.UseMnemonic = false;
            // 
            // welcomeLabel
            // 
            welcomeLabel.BackColor = Color.Transparent;
            welcomeLabel.Font = new Font("Segoe UI", 30F, FontStyle.Bold, GraphicsUnit.Point, 0);
            welcomeLabel.ForeColor = Color.FromArgb(19, 41, 75);
            welcomeLabel.Location = new Point(56, 126);
            welcomeLabel.Name = "welcomeLabel";
            welcomeLabel.Size = new Size(440, 60);
            welcomeLabel.TabIndex = 3;
            welcomeLabel.Text = "Welcome Back!";
            welcomeLabel.TextAlign = ContentAlignment.MiddleLeft;
            welcomeLabel.UseMnemonic = false;
            // 
            // introLabel
            // 
            introLabel.BackColor = Color.Transparent;
            introLabel.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            introLabel.ForeColor = Color.FromArgb(80, 96, 122);
            introLabel.Location = new Point(60, 190);
            introLabel.Name = "introLabel";
            introLabel.Size = new Size(380, 64);
            introLabel.TabIndex = 4;
            introLabel.Text = "Log in to keep your pet's health and happiness on track.";
            introLabel.UseMnemonic = false;
            // 
            // petsPicture
            // 
            petsPicture.BackColor = Color.Transparent;
            petsPicture.Image = (Image)resources.GetObject("petsPicture.Image");
            petsPicture.Location = new Point(160, 296);
            petsPicture.Name = "petsPicture";
            petsPicture.Size = new Size(190, 190);
            petsPicture.SizeMode = PictureBoxSizeMode.Zoom;
            petsPicture.TabIndex = 5;
            petsPicture.TabStop = false;
            // 
            // heartLabel
            // 
            heartLabel.BackColor = Color.Transparent;
            heartLabel.Font = new Font("Segoe UI Symbol", 16F, FontStyle.Regular, GraphicsUnit.Point, 0);
            heartLabel.ForeColor = Color.FromArgb(58, 123, 213);
            heartLabel.Location = new Point(235, 494);
            heartLabel.Name = "heartLabel";
            heartLabel.Size = new Size(40, 32);
            heartLabel.TabIndex = 6;
            heartLabel.Text = "♡";
            heartLabel.TextAlign = ContentAlignment.MiddleCenter;
            heartLabel.UseMnemonic = false;
            // 
            // scriptLabel
            // 
            scriptLabel.BackColor = Color.Transparent;
            scriptLabel.Font = new Font("Segoe Script", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            scriptLabel.ForeColor = Color.FromArgb(47, 107, 200);
            scriptLabel.Location = new Point(105, 526);
            scriptLabel.Name = "scriptLabel";
            scriptLabel.Size = new Size(300, 36);
            scriptLabel.TabIndex = 7;
            scriptLabel.Text = "Happy Pets, Happy People ♥";
            scriptLabel.TextAlign = ContentAlignment.MiddleCenter;
            scriptLabel.UseMnemonic = false;
            // 
            // card
            // 
            card.BackColor = Color.Transparent;
            card.BorderColor = Color.FromArgb(232, 239, 250);
            card.Controls.Add(cardBadge);
            card.Controls.Add(titleLabel);
            card.Controls.Add(subtitleLabel);
            card.Controls.Add(emailLabel);
            card.Controls.Add(emailField);
            card.Controls.Add(passwordLabel);
            card.Controls.Add(passwordField);
            card.Controls.Add(rememberCheck);
            card.Controls.Add(errorLabel);
            card.Controls.Add(signInButton);
            card.Controls.Add(signUpPrompt);
            card.Controls.Add(signUpLink);
            card.CornerColor = Color.FromArgb(233, 242, 254);
            card.FillColor = Color.White;
            card.Location = new Point(527, 70);
            card.Margin = new Padding(0);
            card.Name = "card";
            card.Padding = new Padding(40);
            card.Radius = 22;
            card.Size = new Size(410, 480);
            card.TabIndex = 8;
            card.TransparentCorners = true;
            // 
            // cardBadge
            // 
            cardBadge.BackColor = Color.White;
            cardBadge.FillColor = Color.FromArgb(234, 243, 254);
            cardBadge.Location = new Point(40, 32);
            cardBadge.Name = "cardBadge";
            cardBadge.PawColor = Color.FromArgb(58, 123, 213);
            cardBadge.Radius = 12;
            cardBadge.Size = new Size(42, 42);
            cardBadge.TabIndex = 0;
            cardBadge.TabStop = false;
            // 
            // titleLabel
            // 
            titleLabel.BackColor = Color.White;
            titleLabel.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            titleLabel.ForeColor = Color.FromArgb(19, 41, 75);
            titleLabel.Location = new Point(40, 84);
            titleLabel.Name = "titleLabel";
            titleLabel.Size = new Size(330, 36);
            titleLabel.TabIndex = 1;
            titleLabel.Text = "Login to Pawfect";
            titleLabel.TextAlign = ContentAlignment.MiddleLeft;
            titleLabel.UseMnemonic = false;
            // 
            // subtitleLabel
            // 
            subtitleLabel.BackColor = Color.White;
            subtitleLabel.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            subtitleLabel.ForeColor = Color.FromArgb(122, 136, 156);
            subtitleLabel.Location = new Point(40, 120);
            subtitleLabel.Name = "subtitleLabel";
            subtitleLabel.Size = new Size(330, 22);
            subtitleLabel.TabIndex = 2;
            subtitleLabel.Text = "Enter your email and password to continue.";
            subtitleLabel.TextAlign = ContentAlignment.MiddleLeft;
            subtitleLabel.UseMnemonic = false;
            // 
            // emailLabel
            // 
            emailLabel.BackColor = Color.White;
            emailLabel.Font = new Font("Segoe UI", 7.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            emailLabel.ForeColor = Color.FromArgb(68, 83, 106);
            emailLabel.Location = new Point(40, 160);
            emailLabel.Name = "emailLabel";
            emailLabel.Size = new Size(330, 16);
            emailLabel.TabIndex = 3;
            emailLabel.Text = "EMAIL ADDRESS";
            emailLabel.TextAlign = ContentAlignment.MiddleLeft;
            emailLabel.UseMnemonic = false;
            // 
            // emailField
            // 
            emailField.BorderColor = Color.FromArgb(227, 233, 242);
            emailField.CornerColor = Color.White;
            emailField.FillColor = Color.White;
            emailField.Glyph = "";
            emailField.Location = new Point(40, 178);
            emailField.Margin = new Padding(0);
            emailField.Name = "emailField";
            emailField.Padding = new Padding(10, 3, 10, 3);
            emailField.Placeholder = "example@email.com";
            emailField.Radius = 12;
            emailField.Size = new Size(330, 46);
            emailField.TabIndex = 0;
            // 
            // passwordLabel
            // 
            passwordLabel.BackColor = Color.White;
            passwordLabel.Font = new Font("Segoe UI", 7.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            passwordLabel.ForeColor = Color.FromArgb(68, 83, 106);
            passwordLabel.Location = new Point(40, 236);
            passwordLabel.Name = "passwordLabel";
            passwordLabel.Size = new Size(330, 16);
            passwordLabel.TabIndex = 5;
            passwordLabel.Text = "PASSWORD";
            passwordLabel.TextAlign = ContentAlignment.MiddleLeft;
            passwordLabel.UseMnemonic = false;
            // 
            // passwordField
            // 
            passwordField.BorderColor = Color.FromArgb(227, 233, 242);
            passwordField.CornerColor = Color.White;
            passwordField.FillColor = Color.White;
            passwordField.Glyph = "";
            passwordField.IsPassword = true;
            passwordField.Location = new Point(40, 254);
            passwordField.Margin = new Padding(0);
            passwordField.Name = "passwordField";
            passwordField.Padding = new Padding(10, 3, 10, 3);
            passwordField.Placeholder = "Your password";
            passwordField.Radius = 12;
            passwordField.Size = new Size(330, 46);
            passwordField.TabIndex = 1;
            // 
            // rememberCheck
            // 
            rememberCheck.AutoSize = true;
            rememberCheck.BackColor = Color.White;
            rememberCheck.Cursor = Cursors.Hand;
            rememberCheck.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            rememberCheck.ForeColor = Color.FromArgb(68, 83, 106);
            rememberCheck.Location = new Point(40, 314);
            rememberCheck.Name = "rememberCheck";
            rememberCheck.Size = new Size(109, 19);
            rememberCheck.TabIndex = 2;
            rememberCheck.Text = "Remember me";
            rememberCheck.UseVisualStyleBackColor = false;
            // 
            // errorLabel
            // 
            errorLabel.BackColor = Color.White;
            errorLabel.Font = new Font("Segoe UI", 8.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            errorLabel.ForeColor = Color.FromArgb(239, 68, 68);
            errorLabel.Location = new Point(40, 340);
            errorLabel.Name = "errorLabel";
            errorLabel.Size = new Size(330, 20);
            errorLabel.TabIndex = 7;
            errorLabel.TextAlign = ContentAlignment.MiddleLeft;
            errorLabel.UseMnemonic = false;
            // 
            // signInButton
            // 
            signInButton.BackColor = Color.White;
            signInButton.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            signInButton.Location = new Point(40, 368);
            signInButton.Name = "signInButton";
            signInButton.Radius = 12;
            signInButton.Size = new Size(330, 48);
            signInButton.TabIndex = 3;
            signInButton.Text = "Log In  →";
            // 
            // signUpPrompt
            // 
            signUpPrompt.BackColor = Color.White;
            signUpPrompt.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            signUpPrompt.ForeColor = Color.FromArgb(80, 96, 122);
            signUpPrompt.Location = new Point(70, 436);
            signUpPrompt.Name = "signUpPrompt";
            signUpPrompt.Size = new Size(170, 22);
            signUpPrompt.TabIndex = 9;
            signUpPrompt.Text = "Don't have an account?";
            signUpPrompt.TextAlign = ContentAlignment.MiddleRight;
            signUpPrompt.UseMnemonic = false;
            // 
            // signUpLink
            // 
            signUpLink.BackColor = Color.White;
            signUpLink.Cursor = Cursors.Hand;
            signUpLink.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            signUpLink.ForeColor = Color.FromArgb(58, 123, 213);
            signUpLink.Location = new Point(242, 436);
            signUpLink.Name = "signUpLink";
            signUpLink.Size = new Size(70, 22);
            signUpLink.TabIndex = 10;
            signUpLink.Text = "Sign Up";
            signUpLink.TextAlign = ContentAlignment.MiddleLeft;
            signUpLink.UseMnemonic = false;
            // 
            // LoginForm
            // 
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            BackColor = Color.FromArgb(235, 243, 254);
            ClientSize = new Size(1000, 620);
            Controls.Add(backdrop);
            Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            KeyPreview = true;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "LoginForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Pawfect - Sign in";
            backdrop.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)petsPicture).EndInit();
            card.ResumeLayout(false);
            card.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private MyApp.Controls.BackdropPanel backdrop;
        private MyApp.Controls.PawBadge logoBadge;
        private System.Windows.Forms.Label brandLabel;
        private System.Windows.Forms.Label taglineLabel;
        private System.Windows.Forms.Label welcomeLabel;
        private System.Windows.Forms.Label introLabel;
        private System.Windows.Forms.PictureBox petsPicture;
        private System.Windows.Forms.Label heartLabel;
        private System.Windows.Forms.Label scriptLabel;
        private MyApp.Controls.CardPanel card;
        private MyApp.Controls.PawBadge cardBadge;
        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.Label subtitleLabel;
        private System.Windows.Forms.Label emailLabel;
        private MyApp.Controls.InputField emailField;
        private System.Windows.Forms.Label passwordLabel;
        private MyApp.Controls.InputField passwordField;
        private System.Windows.Forms.CheckBox rememberCheck;
        private System.Windows.Forms.Label errorLabel;
        private MyApp.Controls.PrimaryButton signInButton;
        private System.Windows.Forms.Label signUpPrompt;
        private System.Windows.Forms.Label signUpLink;
    }
}
