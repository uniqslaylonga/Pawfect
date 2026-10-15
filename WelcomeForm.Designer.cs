#nullable disable
namespace MyApp
{
    partial class WelcomeForm
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
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(WelcomeForm));
            logoBadge = new MyApp.Controls.PawBadge();
            brandLabel = new Label();
            taglineLabel = new Label();
            faintPawTop = new MyApp.Controls.PawBadge();
            faintPawBottom = new MyApp.Controls.PawBadge();
            welcomeLabel = new Label();
            nameLabel = new Label();
            namePawA = new MyApp.Controls.PawBadge();
            namePawB = new MyApp.Controls.PawBadge();
            subtitleLabel = new Label();
            startButton = new MyApp.Controls.PrimaryButton();
            loginButton = new MyApp.Controls.SecondaryButton();
            scriptLabel = new Label();
            blobPanel = new MyApp.Controls.BlobPanel();
            petsPicture = new PictureBox();
            blobPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)petsPicture).BeginInit();
            SuspendLayout();
            // 
            // logoBadge
            // 
            logoBadge.BackColor = Color.FromArgb(243, 247, 252);
            logoBadge.FillColor = Color.FromArgb(58, 123, 213);
            logoBadge.Location = new Point(40, 30);
            logoBadge.Name = "logoBadge";
            logoBadge.PawColor = Color.White;
            logoBadge.Radius = 10;
            logoBadge.Size = new Size(40, 40);
            logoBadge.TabIndex = 0;
            logoBadge.TabStop = false;
            // 
            // brandLabel
            // 
            brandLabel.BackColor = Color.Transparent;
            brandLabel.Font = new Font("Segoe UI", 15F, FontStyle.Bold, GraphicsUnit.Point, 0);
            brandLabel.ForeColor = Color.FromArgb(31, 42, 60);
            brandLabel.Location = new Point(90, 28);
            brandLabel.Name = "brandLabel";
            brandLabel.Size = new Size(150, 28);
            brandLabel.TabIndex = 1;
            brandLabel.Text = "Pawfect";
            brandLabel.TextAlign = ContentAlignment.MiddleLeft;
            brandLabel.UseMnemonic = false;
            // 
            // taglineLabel
            // 
            taglineLabel.BackColor = Color.Transparent;
            taglineLabel.Font = new Font("Segoe UI", 6.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            taglineLabel.ForeColor = Color.FromArgb(122, 136, 156);
            taglineLabel.Location = new Point(92, 55);
            taglineLabel.Name = "taglineLabel";
            taglineLabel.Size = new Size(230, 16);
            taglineLabel.TabIndex = 2;
            taglineLabel.Text = "GROOMING • HEALTH • HAPPINESS";
            taglineLabel.TextAlign = ContentAlignment.MiddleLeft;
            taglineLabel.UseMnemonic = false;
            // 
            // faintPawTop
            // 
            faintPawTop.BackColor = Color.FromArgb(243, 247, 252);
            faintPawTop.FillColor = Color.FromArgb(243, 247, 252);
            faintPawTop.Location = new Point(456, 120);
            faintPawTop.Name = "faintPawTop";
            faintPawTop.PawColor = Color.FromArgb(214, 230, 251);
            faintPawTop.Radius = 10;
            faintPawTop.Size = new Size(40, 40);
            faintPawTop.TabIndex = 4;
            faintPawTop.TabStop = false;
            // 
            // faintPawBottom
            // 
            faintPawBottom.BackColor = Color.FromArgb(243, 247, 252);
            faintPawBottom.FillColor = Color.FromArgb(243, 247, 252);
            faintPawBottom.Location = new Point(400, 500);
            faintPawBottom.Name = "faintPawBottom";
            faintPawBottom.PawColor = Color.FromArgb(214, 230, 251);
            faintPawBottom.Radius = 10;
            faintPawBottom.Size = new Size(56, 56);
            faintPawBottom.TabIndex = 5;
            faintPawBottom.TabStop = false;
            // 
            // welcomeLabel
            // 
            welcomeLabel.BackColor = Color.Transparent;
            welcomeLabel.Font = new Font("Segoe UI", 28F, FontStyle.Bold, GraphicsUnit.Point, 0);
            welcomeLabel.ForeColor = Color.FromArgb(28, 48, 86);
            welcomeLabel.Location = new Point(88, 112);
            welcomeLabel.Name = "welcomeLabel";
            welcomeLabel.Size = new Size(440, 56);
            welcomeLabel.TabIndex = 6;
            welcomeLabel.Text = "Welcome to";
            welcomeLabel.TextAlign = ContentAlignment.MiddleLeft;
            welcomeLabel.UseMnemonic = false;
            // 
            // nameLabel
            // 
            nameLabel.BackColor = Color.Transparent;
            nameLabel.Font = new Font("Segoe UI", 32F, FontStyle.Bold, GraphicsUnit.Point, 0);
            nameLabel.ForeColor = Color.FromArgb(28, 48, 86);
            nameLabel.Location = new Point(88, 166);
            nameLabel.Name = "nameLabel";
            nameLabel.Size = new Size(200, 62);
            nameLabel.TabIndex = 7;
            nameLabel.Text = "Pawfect";
            nameLabel.TextAlign = ContentAlignment.MiddleLeft;
            nameLabel.UseMnemonic = false;
            // 
            // namePawA
            // 
            namePawA.BackColor = Color.FromArgb(243, 247, 252);
            namePawA.FillColor = Color.FromArgb(243, 247, 252);
            namePawA.Location = new Point(292, 172);
            namePawA.Name = "namePawA";
            namePawA.PawColor = Color.FromArgb(28, 48, 86);
            namePawA.Radius = 4;
            namePawA.Size = new Size(26, 26);
            namePawA.TabIndex = 8;
            namePawA.TabStop = false;
            // 
            // namePawB
            // 
            namePawB.BackColor = Color.FromArgb(243, 247, 252);
            namePawB.FillColor = Color.FromArgb(243, 247, 252);
            namePawB.Location = new Point(316, 192);
            namePawB.Name = "namePawB";
            namePawB.PawColor = Color.FromArgb(28, 48, 86);
            namePawB.Radius = 4;
            namePawB.Size = new Size(26, 26);
            namePawB.TabIndex = 9;
            namePawB.TabStop = false;
            // 
            // subtitleLabel
            // 
            subtitleLabel.BackColor = Color.Transparent;
            subtitleLabel.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            subtitleLabel.ForeColor = Color.FromArgb(122, 136, 156);
            subtitleLabel.Location = new Point(92, 250);
            subtitleLabel.Name = "subtitleLabel";
            subtitleLabel.Size = new Size(320, 28);
            subtitleLabel.TabIndex = 10;
            subtitleLabel.Text = "Start your grooming visit";
            subtitleLabel.TextAlign = ContentAlignment.MiddleLeft;
            subtitleLabel.UseMnemonic = false;
            // 
            // startButton
            // 
            startButton.BackColor = Color.FromArgb(243, 247, 252);
            startButton.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            startButton.Location = new Point(94, 306);
            startButton.Name = "startButton";
            startButton.Radius = 27;
            startButton.Size = new Size(222, 54);
            startButton.TabIndex = 11;
            startButton.Text = "Start   →";
            // 
            // loginButton
            // 
            loginButton.BackColor = Color.FromArgb(243, 247, 252);
            loginButton.Font = new Font("Segoe UI", 11F, FontStyle.Bold, GraphicsUnit.Point, 0);
            loginButton.Location = new Point(94, 376);
            loginButton.Name = "loginButton";
            loginButton.Radius = 23;
            loginButton.Size = new Size(222, 46);
            loginButton.TabIndex = 14;
            loginButton.Text = "Login";
            // 
            // scriptLabel
            // 
            scriptLabel.BackColor = Color.Transparent;
            scriptLabel.Font = new Font("Segoe Script", 16F, FontStyle.Regular, GraphicsUnit.Point, 0);
            scriptLabel.ForeColor = Color.FromArgb(74, 126, 205);
            scriptLabel.Location = new Point(80, 440);
            scriptLabel.Name = "scriptLabel";
            scriptLabel.Size = new Size(200, 140);
            scriptLabel.TabIndex = 12;
            scriptLabel.Text = "Happy\r\n  Pets\r\n  Happy ♡\r\n     People";
            scriptLabel.UseMnemonic = false;
            // 
            // blobPanel
            // 
            blobPanel.AccentColor = Color.FromArgb(96, 165, 250);
            blobPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            blobPanel.Controls.Add(petsPicture);
            blobPanel.CornerColor = Color.FromArgb(243, 247, 252);
            blobPanel.FillBottom = Color.FromArgb(229, 241, 254);
            blobPanel.FillTop = Color.FromArgb(240, 246, 255);
            blobPanel.Location = new Point(550, 102);
            blobPanel.Name = "blobPanel";
            blobPanel.Size = new Size(450, 518);
            blobPanel.TabIndex = 13;
            // 
            // petsPicture
            // 
            petsPicture.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            petsPicture.BackColor = Color.Transparent;
            petsPicture.Image = (Image)resources.GetObject("petsPicture.Image");
            petsPicture.Location = new Point(90, 158);
            petsPicture.Name = "petsPicture";
            petsPicture.Size = new Size(360, 360);
            petsPicture.SizeMode = PictureBoxSizeMode.Zoom;
            petsPicture.TabIndex = 0;
            petsPicture.TabStop = false;
            // 
            // WelcomeForm
            // 
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            BackColor = Color.FromArgb(243, 247, 252);
            ClientSize = new Size(1000, 620);
            Controls.Add(blobPanel);
            Controls.Add(scriptLabel);
            Controls.Add(loginButton);
            Controls.Add(startButton);
            Controls.Add(subtitleLabel);
            Controls.Add(namePawB);
            Controls.Add(namePawA);
            Controls.Add(nameLabel);
            Controls.Add(welcomeLabel);
            Controls.Add(faintPawBottom);
            Controls.Add(faintPawTop);
            Controls.Add(taglineLabel);
            Controls.Add(brandLabel);
            Controls.Add(logoBadge);
            Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            KeyPreview = true;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "WelcomeForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Pawfect - Welcome";
            blobPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)petsPicture).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private MyApp.Controls.PawBadge logoBadge;
        private System.Windows.Forms.Label brandLabel;
        private System.Windows.Forms.Label taglineLabel;
        private MyApp.Controls.PawBadge faintPawTop;
        private MyApp.Controls.PawBadge faintPawBottom;
        private System.Windows.Forms.Label welcomeLabel;
        private System.Windows.Forms.Label nameLabel;
        private MyApp.Controls.PawBadge namePawA;
        private MyApp.Controls.PawBadge namePawB;
        private System.Windows.Forms.Label subtitleLabel;
        private MyApp.Controls.PrimaryButton startButton;
        private MyApp.Controls.SecondaryButton loginButton;
        private System.Windows.Forms.Label scriptLabel;
        private MyApp.Controls.BlobPanel blobPanel;
        private System.Windows.Forms.PictureBox petsPicture;
    }
}