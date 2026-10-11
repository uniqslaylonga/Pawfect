using Microsoft.Data.SqlClient;

namespace MyApp;


internal partial class LoginForm : Form
{
    /// <summary>Set when the sign-in succeeds.</summary>
    public UserAccount? SignedInUser { get; private set; }

    public LoginForm()
    {
        InitializeComponent();
        DoubleBuffered = true;

        KeyDown += (_, e) =>
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                SignIn();
            }
        };
        signInButton.Click += (_, _) => SignIn();
        emailField.Box.TextChanged += (_, _) => errorLabel.Text = "";
        passwordField.Box.TextChanged += (_, _) => errorLabel.Text = "";

        rightPanel.Resize += (_, _) => CenterCard();
        CenterCard();
    }

    private void CenterCard()
    {
        card.Location = new Point(
            Math.Max(0, (rightPanel.Width - card.Width) / 2),
            Math.Max(0, (rightPanel.Height - card.Height) / 2));
    }

    private void SignIn()
    {
        string email = emailField.Value.Trim();

        if (email.Length == 0 || passwordField.Value.Length == 0)
        {
            errorLabel.Text = "Enter your email and password.";
            return;
        }
        if (!email.Contains('@'))
        {
            errorLabel.Text = "Enter a valid email address.";
            return;
        }

        UserAccount? user;
        try
        {
            user = Auth.SignIn(email, passwordField.Value);
        }
        catch (SqlException)
        {
            errorLabel.Text = "Can't reach the database.";
            return;
        }

        if (user is null)
        {
            errorLabel.Text = "Incorrect email or password.";
            return;
        }

        SignedInUser = user;
        DialogResult = DialogResult.OK;
        Close();
    }

    private void LoginForm_Load(object sender, EventArgs e)
    {

    }

    private void button1_Click(object sender, EventArgs e)
    {

    }

    private void hintLabel_Click(object sender, EventArgs e)
    {

    }

    private void nameLabel_Click(object sender, EventArgs e)
    {

    }

    private void pawBadge_Click(object sender, EventArgs e)
    {

    }

    private void card_Paint(object sender, PaintEventArgs e)
    {

    }
}
