namespace MyApp;

/// <summary>
/// Landing screen shown before sign-in. Everything on it is a normal control, so it can be edited in the
/// Visual Studio designer (WelcomeForm.Designer.cs). Start = DialogResult.OK, Esc/close = Cancel.
/// </summary>
internal partial class WelcomeForm : Form
{
    public WelcomeForm()
    {
        InitializeComponent();
        DoubleBuffered = true;

        languageBox.SelectedIndex = 0;
        startButton.Click += (_, _) => Begin();

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
