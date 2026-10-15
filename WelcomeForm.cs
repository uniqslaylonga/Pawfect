namespace MyApp;

/// <summary>
/// Landing screen shown before sign-in. Everything on it is a normal control, so it can be edited in the
/// Visual Studio designer (WelcomeForm.Designer.cs). Log In / I'm New Here = DialogResult.OK, Esc/close = Cancel.
/// </summary>
internal partial class WelcomeForm : Form
{
    /// <summary>Which button was pressed (valid when the dialog returns OK).</summary>
    public bool IsNewCustomer { get; private set; }

    public WelcomeForm()
    {
        InitializeComponent();
        DoubleBuffered = true;

        languageBox.SelectedIndex = 0;
        startButton.Click += (_, _) => Begin(newCustomer: false);
        newButton.Click += (_, _) => Begin(newCustomer: true);

        KeyDown += (_, e) =>
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                Begin(newCustomer: false);
            }
            else if (e.KeyCode == Keys.Escape)
            {
                DialogResult = DialogResult.Cancel;
                Close();
            }
        };
    }

    private void Begin(bool newCustomer)
    {
        IsNewCustomer = newCustomer;
        DialogResult = DialogResult.OK;
        Close();
    }
}
