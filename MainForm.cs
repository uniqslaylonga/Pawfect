using System.Globalization;
using MyApp.Controls;

namespace MyApp;


internal partial class MainForm : Form
{
    public bool LoggedOut { get; private set; }

    public MainForm()
    {
        InitializeComponent();
        DoubleBuffered = true;

        greetingLabel.Text = GreetingText();
        dateLabel.Text = DateTime.Now.ToString("MMM d, yyyy", CultureInfo.CurrentCulture) + "   \u00B7   Today";

        serviceDonut.SetData(Array.Empty<(double, Color)>(), "0", "Completed");

        var tabs = new[] { allTab, walkInTab, appointmentTab, addOnTab };
        foreach (var tab in tabs)
        {
            tab.Click += (sender, _) =>
            {
                foreach (var other in tabs)
                    other.Active = ReferenceEquals(other, sender);
            };
        }
    }

    private static string GreetingText()
    {
        int h = DateTime.Now.Hour;
        return h < 12 ? "Good morning!" : h < 18 ? "Good afternoon!" : "Good evening!";
    }

    private void logoutNav_Click(object? sender, EventArgs e)
    {
        LoggedOut = true;
        Close();
    }

    private void notificationButton_Click(object sender, EventArgs e)
    {

    }

    private void serviceDonut_Click(object sender, EventArgs e)
    {

    }
}
