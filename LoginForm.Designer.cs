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
            brandPanel = new MyApp.Controls.BrandPanel();
            pawBadge = new MyApp.Controls.PawBadge();
            nameLabel = new Label();
            tagLabel = new Label();
            rightPanel = new Panel();
            card = new MyApp.Controls.CardPanel();
            titleLabel = new Label();
            subtitleLabel = new Label();
            emailLabel = new Label();
            emailField = new MyApp.Controls.InputField();
            passwordLabel = new Label();
            passwordField = new MyApp.Controls.InputField();
            errorLabel = new Label();
            signInButton = new MyApp.Controls.PrimaryButton();
            brandPanel.SuspendLayout();
            rightPanel.SuspendLayout();
            card.SuspendLayout();
            SuspendLayout();
            // 
            // brandPanel
            // 
            brandPanel.BackColor = Color.FromArgb(58, 123, 213);
            brandPanel.Controls.Add(pawBadge);
            brandPanel.Controls.Add(nameLabel);
            brandPanel.Controls.Add(tagLabel);
            brandPanel.Dock = DockStyle.Left;
            brandPanel.Location = new Point(0, 0);
            brandPanel.Name = "brandPanel";
            brandPanel.Size = new Size(360, 620);
            brandPanel.TabIndex = 1;
            // 
            // pawBadge
            // 
            pawBadge.BackColor = Color.FromArgb(58, 123, 213);
            pawBadge.FillColor = Color.White;
            pawBadge.Location = new Point(112, 234);
            pawBadge.Name = "pawBadge";
            pawBadge.PawColor = Color.FromArgb(58, 123, 213);
            pawBadge.Radius = 16;
            pawBadge.Size = new Size(64, 64);
            pawBadge.TabIndex = 0;
            pawBadge.TabStop = false;
            pawBadge.Click += pawBadge_Click;
            // 
            // nameLabel
            // 
            nameLabel.BackColor = Color.Transparent;
            nameLabel.Font = new Font("Segoe UI", 20F, FontStyle.Bold, GraphicsUnit.Point, 0);
            nameLabel.ForeColor = Color.White;
            nameLabel.Location = new Point(81, 310);
            nameLabel.Name = "nameLabel";
            nameLabel.Size = new Size(119, 38);
            nameLabel.TabIndex = 1;
            nameLabel.Text = "Pawfect ";
            nameLabel.TextAlign = ContentAlignment.MiddleLeft;
            nameLabel.UseMnemonic = false;
            nameLabel.Click += nameLabel_Click;
            // 
            // tagLabel
            // 
            tagLabel.BackColor = Color.Transparent;
            tagLabel.Font = new Font("Segoe UI", 10.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            tagLabel.ForeColor = Color.FromArgb(214, 230, 251);
            tagLabel.Location = new Point(70, 356);
            tagLabel.Name = "tagLabel";
            tagLabel.Size = new Size(290, 24);
            tagLabel.TabIndex = 2;
            tagLabel.Text = "Pet Care. Made Simple.";
            tagLabel.TextAlign = ContentAlignment.MiddleLeft;
            tagLabel.UseMnemonic = false;
            // 
            // rightPanel
            // 
            rightPanel.BackColor = Color.FromArgb(243, 247, 252);
            rightPanel.Controls.Add(card);
            rightPanel.Dock = DockStyle.Fill;
            rightPanel.Location = new Point(360, 0);
            rightPanel.Name = "rightPanel";
            rightPanel.Size = new Size(540, 620);
            rightPanel.TabIndex = 0;
            // 
            // card
            // 
            card.BorderColor = Color.FromArgb(227, 233, 242);
            card.Controls.Add(titleLabel);
            card.Controls.Add(subtitleLabel);
            card.Controls.Add(emailLabel);
            card.Controls.Add(emailField);
            card.Controls.Add(passwordLabel);
            card.Controls.Add(passwordField);
            card.Controls.Add(errorLabel);
            card.Controls.Add(signInButton);
            card.CornerColor = Color.FromArgb(243, 247, 252);
            card.FillColor = Color.White;
            card.Location = new Point(88, 110);
            card.Margin = new Padding(0);
            card.Name = "card";
            card.Padding = new Padding(32);
            card.Radius = 12;
            card.Size = new Size(390, 385);
            card.TabIndex = 0;
            card.Paint += card_Paint;
            // 
            // titleLabel
            // 
            titleLabel.BackColor = Color.White;
            titleLabel.Font = new Font("Segoe UI", 16F, FontStyle.Bold, GraphicsUnit.Point, 0);
            titleLabel.ForeColor = Color.FromArgb(31, 42, 60);
            titleLabel.Location = new Point(32, 30);
            titleLabel.Name = "titleLabel";
            titleLabel.Size = new Size(316, 30);
            titleLabel.TabIndex = 0;
            titleLabel.Text = "Welcome back";
            titleLabel.TextAlign = ContentAlignment.MiddleLeft;
            titleLabel.UseMnemonic = false;
            // 
            // subtitleLabel
            // 
            subtitleLabel.BackColor = Color.White;
            subtitleLabel.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            subtitleLabel.ForeColor = Color.FromArgb(122, 136, 156);
            subtitleLabel.Location = new Point(32, 62);
            subtitleLabel.Name = "subtitleLabel";
            subtitleLabel.Size = new Size(316, 20);
            subtitleLabel.TabIndex = 1;
            subtitleLabel.Text = "Sign in to your staff account";
            subtitleLabel.TextAlign = ContentAlignment.MiddleLeft;
            subtitleLabel.UseMnemonic = false;
            // 
            // emailLabel
            // 
            emailLabel.BackColor = Color.White;
            emailLabel.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            emailLabel.ForeColor = Color.FromArgb(68, 83, 106);
            emailLabel.Location = new Point(32, 104);
            emailLabel.Name = "emailLabel";
            emailLabel.Size = new Size(316, 18);
            emailLabel.TabIndex = 2;
            emailLabel.Text = "Email";
            emailLabel.TextAlign = ContentAlignment.MiddleLeft;
            emailLabel.UseMnemonic = false;
            // 
            // emailField
            // 
            emailField.BorderColor = Color.FromArgb(227, 233, 242);
            emailField.CornerColor = Color.White;
            emailField.FillColor = Color.White;
            emailField.Glyph = "";
            emailField.Location = new Point(32, 124);
            emailField.Margin = new Padding(0);
            emailField.Name = "emailField";
            emailField.Padding = new Padding(10, 3, 10, 3);
            emailField.Placeholder = "Email address";
            emailField.Radius = 8;
            emailField.Size = new Size(316, 42);
            emailField.TabIndex = 3;
            // 
            // passwordLabel
            // 
            passwordLabel.BackColor = Color.White;
            passwordLabel.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            passwordLabel.ForeColor = Color.FromArgb(68, 83, 106);
            passwordLabel.Location = new Point(32, 180);
            passwordLabel.Name = "passwordLabel";
            passwordLabel.Size = new Size(316, 18);
            passwordLabel.TabIndex = 4;
            passwordLabel.Text = "Password";
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
            passwordField.Location = new Point(32, 200);
            passwordField.Margin = new Padding(0);
            passwordField.Name = "passwordField";
            passwordField.Padding = new Padding(10, 3, 10, 3);
            passwordField.Placeholder = "Password";
            passwordField.Radius = 8;
            passwordField.Size = new Size(316, 42);
            passwordField.TabIndex = 5;
            // 
            // errorLabel
            // 
            errorLabel.BackColor = Color.White;
            errorLabel.Font = new Font("Segoe UI", 8.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            errorLabel.ForeColor = Color.FromArgb(239, 68, 68);
            errorLabel.Location = new Point(32, 250);
            errorLabel.Name = "errorLabel";
            errorLabel.Size = new Size(316, 20);
            errorLabel.TabIndex = 6;
            errorLabel.TextAlign = ContentAlignment.MiddleLeft;
            errorLabel.UseMnemonic = false;
            // 
            // signInButton
            // 
            signInButton.BackColor = Color.White;
            signInButton.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            signInButton.Location = new Point(32, 288);
            signInButton.Name = "signInButton";
            signInButton.Size = new Size(316, 44);
            signInButton.TabIndex = 7;
            signInButton.Text = "Sign In";
            // 
            // LoginForm
            // 
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            BackColor = Color.FromArgb(243, 247, 252);
            ClientSize = new Size(900, 620);
            Controls.Add(rightPanel);
            Controls.Add(brandPanel);
            Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            KeyPreview = true;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "LoginForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Pawfect Employee - Sign in";
            Load += LoginForm_Load;
            brandPanel.ResumeLayout(false);
            rightPanel.ResumeLayout(false);
            card.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private MyApp.Controls.BrandPanel brandPanel;
        private MyApp.Controls.PawBadge pawBadge;
        private System.Windows.Forms.Label nameLabel;
        private System.Windows.Forms.Label tagLabel;
        private System.Windows.Forms.Panel rightPanel;
        private MyApp.Controls.CardPanel card;
        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.Label subtitleLabel;
        private System.Windows.Forms.Label emailLabel;
        private MyApp.Controls.InputField emailField;
        private System.Windows.Forms.Label passwordLabel;
        private MyApp.Controls.InputField passwordField;
        private System.Windows.Forms.Label errorLabel;
        private MyApp.Controls.PrimaryButton signInButton;
    }
}
