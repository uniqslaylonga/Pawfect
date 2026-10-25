using Microsoft.Data.SqlClient;

namespace MyApp;

internal partial class LoginForm : Form
{
    // "Remember me" only ever stores the email address (never the password).
    private static readonly string RememberFile = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Pawfect", "last_email.txt");

    /// <summary>Set when the sign-in succeeds.</summary>
    public UserAccount? SignedInUser { get; private set; }

    /// <summary>True when "Start" was clicked instead of signing in: the caller opens the customer (kiosk) flow.</summary>
    public bool WantsKiosk { get; private set; }

    public LoginForm()
    {
        InitializeComponent();
        Icon = AppIcon.Value;
        DoubleBuffered = true;

        KeyDown += (_, e) =>
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                SignIn();
            }
            else if (e.KeyCode == Keys.Escape)
            {
                DialogResult = DialogResult.Cancel;
                Close();
            }
        };
        signInButton.Click += (_, _) => SignIn();
        signUpLink.Click += (_, _) =>
        {
            WantsKiosk = true;
            DialogResult = DialogResult.OK;
            Close();
        };
        emailField.Box.TextChanged += (_, _) => errorLabel.Text = "";
        passwordField.Box.TextChanged += (_, _) => errorLabel.Text = "";

        bool restored = LoadRememberedEmail();
        Shown += (_, _) => (restored ? passwordField.Box : emailField.Box).Focus();
        FullScreen.Apply(this, centerContent: true);
    }

    private bool LoadRememberedEmail()
    {
        try
        {
            if (!File.Exists(RememberFile)) return false;
            string email = File.ReadAllText(RememberFile).Trim();
            if (email.Length == 0) return false;
            emailField.Box.Text = email;
            rememberCheck.Checked = true;
            return true;
        }
        catch (IOException) { return false; }
        catch (UnauthorizedAccessException) { return false; }
    }

    private void SaveRememberedEmail(string email)
    {
        try
        {
            if (rememberCheck.Checked)
            {
                Directory.CreateDirectory(Path.GetDirectoryName(RememberFile)!);
                File.WriteAllText(RememberFile, email);
            }
            else if (File.Exists(RememberFile))
            {
                File.Delete(RememberFile);
            }
        }
        catch (IOException) { }
        catch (UnauthorizedAccessException) { }
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
            AuditLog.RecordFailedLogin(email, Auth.LastFailure);
            return;
        }

        SaveRememberedEmail(email);
        AuditLog.Record(user, "Login", $"Signed in as {user.Role}");
        SignedInUser = user;
        DialogResult = DialogResult.OK;
        Close();
    }
}
