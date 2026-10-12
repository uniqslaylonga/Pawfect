#nullable disable
namespace MyApp
{
    partial class CreateAdminForm
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
            titleLabel = new Label();
            subtitleLabel = new Label();
            nameLabel = new Label();
            nameBox = new TextBox();
            emailLabel = new Label();
            emailBox = new TextBox();
            passwordLabel = new Label();
            passwordBox = new TextBox();
            confirmLabel = new Label();
            confirmBox = new TextBox();
            errorLabel = new Label();
            createButton = new Button();
            SuspendLayout();
            // 
            // titleLabel
            // 
            titleLabel.AutoSize = false;
            titleLabel.Font = new Font("Segoe UI", 13F, FontStyle.Bold, GraphicsUnit.Point, 0);
            titleLabel.ForeColor = Color.FromArgb(31, 42, 60);
            titleLabel.Location = new Point(32, 24);
            titleLabel.Name = "titleLabel";
            titleLabel.Size = new Size(316, 30);
            titleLabel.TabIndex = 0;
            titleLabel.Text = "Create the admin account";
            titleLabel.UseMnemonic = false;
            // 
            // subtitleLabel
            // 
            subtitleLabel.AutoSize = false;
            subtitleLabel.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            subtitleLabel.ForeColor = Color.FromArgb(122, 136, 156);
            subtitleLabel.Location = new Point(32, 56);
            subtitleLabel.Name = "subtitleLabel";
            subtitleLabel.Size = new Size(316, 36);
            subtitleLabel.TabIndex = 1;
            subtitleLabel.Text = "No accounts exist yet. This account will have full admin access.";
            subtitleLabel.UseMnemonic = false;
            // 
            // nameLabel
            // 
            nameLabel.AutoSize = false;
            nameLabel.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            nameLabel.ForeColor = Color.FromArgb(68, 83, 106);
            nameLabel.Location = new Point(32, 104);
            nameLabel.Name = "nameLabel";
            nameLabel.Size = new Size(316, 18);
            nameLabel.TabIndex = 2;
            nameLabel.Text = "Full name";
            nameLabel.UseMnemonic = false;
            // 
            // nameBox
            // 
            nameBox.BorderStyle = BorderStyle.FixedSingle;
            nameBox.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            nameBox.Location = new Point(32, 124);
            nameBox.Name = "nameBox";
            nameBox.Size = new Size(316, 25);
            nameBox.TabIndex = 3;
            // 
            // emailLabel
            // 
            emailLabel.AutoSize = false;
            emailLabel.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            emailLabel.ForeColor = Color.FromArgb(68, 83, 106);
            emailLabel.Location = new Point(32, 168);
            emailLabel.Name = "emailLabel";
            emailLabel.Size = new Size(316, 18);
            emailLabel.TabIndex = 4;
            emailLabel.Text = "Email";
            emailLabel.UseMnemonic = false;
            // 
            // emailBox
            // 
            emailBox.BorderStyle = BorderStyle.FixedSingle;
            emailBox.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            emailBox.Location = new Point(32, 188);
            emailBox.Name = "emailBox";
            emailBox.Size = new Size(316, 25);
            emailBox.TabIndex = 5;
            // 
            // passwordLabel
            // 
            passwordLabel.AutoSize = false;
            passwordLabel.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            passwordLabel.ForeColor = Color.FromArgb(68, 83, 106);
            passwordLabel.Location = new Point(32, 232);
            passwordLabel.Name = "passwordLabel";
            passwordLabel.Size = new Size(316, 18);
            passwordLabel.TabIndex = 6;
            passwordLabel.Text = "Password (at least 8 characters)";
            passwordLabel.UseMnemonic = false;
            // 
            // passwordBox
            // 
            passwordBox.BorderStyle = BorderStyle.FixedSingle;
            passwordBox.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            passwordBox.Location = new Point(32, 252);
            passwordBox.Name = "passwordBox";
            passwordBox.UseSystemPasswordChar = true;
            passwordBox.Size = new Size(316, 25);
            passwordBox.TabIndex = 7;
            // 
            // confirmLabel
            // 
            confirmLabel.AutoSize = false;
            confirmLabel.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            confirmLabel.ForeColor = Color.FromArgb(68, 83, 106);
            confirmLabel.Location = new Point(32, 296);
            confirmLabel.Name = "confirmLabel";
            confirmLabel.Size = new Size(316, 18);
            confirmLabel.TabIndex = 8;
            confirmLabel.Text = "Confirm password";
            confirmLabel.UseMnemonic = false;
            // 
            // confirmBox
            // 
            confirmBox.BorderStyle = BorderStyle.FixedSingle;
            confirmBox.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            confirmBox.Location = new Point(32, 316);
            confirmBox.Name = "confirmBox";
            confirmBox.UseSystemPasswordChar = true;
            confirmBox.Size = new Size(316, 25);
            confirmBox.TabIndex = 9;
            // 
            // errorLabel
            // 
            errorLabel.AutoSize = false;
            errorLabel.Font = new Font("Segoe UI", 8.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            errorLabel.ForeColor = Color.FromArgb(239, 68, 68);
            errorLabel.Location = new Point(32, 362);
            errorLabel.Name = "errorLabel";
            errorLabel.Size = new Size(316, 22);
            errorLabel.TabIndex = 10;
            errorLabel.Text = "";
            errorLabel.UseMnemonic = false;
            // 
            // createButton
            // 
            createButton.BackColor = Color.FromArgb(58, 123, 213);
            createButton.Cursor = Cursors.Hand;
            createButton.FlatAppearance.BorderSize = 0;
            createButton.FlatStyle = FlatStyle.Flat;
            createButton.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            createButton.ForeColor = Color.White;
            createButton.Location = new Point(32, 392);
            createButton.Name = "createButton";
            createButton.Size = new Size(316, 38);
            createButton.TabIndex = 11;
            createButton.Text = "Create admin account";
            createButton.UseVisualStyleBackColor = false;
            createButton.Click += createButton_Click;
            // 
            // CreateAdminForm
            // 
            AcceptButton = createButton;
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            BackColor = Color.White;
            ClientSize = new Size(380, 452);
            Controls.Add(createButton);
            Controls.Add(errorLabel);
            Controls.Add(confirmBox);
            Controls.Add(confirmLabel);
            Controls.Add(passwordBox);
            Controls.Add(passwordLabel);
            Controls.Add(emailBox);
            Controls.Add(emailLabel);
            Controls.Add(nameBox);
            Controls.Add(nameLabel);
            Controls.Add(subtitleLabel);
            Controls.Add(titleLabel);
            Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "CreateAdminForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Pawfect - Create admin account";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.Label subtitleLabel;
        private System.Windows.Forms.Label nameLabel;
        private System.Windows.Forms.TextBox nameBox;
        private System.Windows.Forms.Label emailLabel;
        private System.Windows.Forms.TextBox emailBox;
        private System.Windows.Forms.Label passwordLabel;
        private System.Windows.Forms.TextBox passwordBox;
        private System.Windows.Forms.Label confirmLabel;
        private System.Windows.Forms.TextBox confirmBox;
        private System.Windows.Forms.Label errorLabel;
        private System.Windows.Forms.Button createButton;
    }
}
