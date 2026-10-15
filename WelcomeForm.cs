namespace MyApp;

/// <summary>
/// Landing screen shown before sign-in. Everything on it is a normal control, so it can be edited in the
/// Visual Studio designer (WelcomeForm.Designer.cs). Start = DialogResult.OK (kiosk customer), Login button = DialogResult.OK with WantsLogin, Esc/close = Cancel.
/// </summary>
internal partial class WelcomeForm : Form
{
    /// <summary>True when the Login button was pressed (staff, admin or a customer account) instead of Start.</summary>
    public bool WantsLogin { get; private set; }

    public WelcomeForm()
    {
        InitializeComponent();
        DoubleBuffered = true;

        startButton.Click += (_, _) => Begin();
        loginButton.Click += (_, _) => { WantsLogin = true; Begin(); };

        KeyDown += (_, e) =>
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                Begin();
            }
            else if (e.KeyCode == Keys.Escape)
            {
                DialogResult = DialogResult.Cancel;
                Close();
            }
        };
    }

    private void Begin()
    {
        DialogResult = DialogResult.OK;
        Close();
    }
}