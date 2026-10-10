using System.Globalization;
using MyApp.Controls;

namespace MyApp;


internal partial class MainForm : Form
{
    private readonly AppointmentsPage appointmentsPage;
    private readonly string _dashboardTitle;
    private readonly string _dashboardSubtitle;

    public bool LoggedOut { get; private set; }

    public MainForm()
    {
        InitializeComponent();
        DoubleBuffered = true;

        // Appointments page lives in the same content area as the dashboard body; the sidebar switches between them.
        _dashboardTitle = pageTitle.Text;
        _dashboardSubtitle = pageSubtitle.Text;
        appointmentsPage = new AppointmentsPage { Dock = DockStyle.Fill, Visible = false };
        contentPanel.Controls.Add(appointmentsPage);
        appointmentsPage.BringToFront();
        dashboardNav.Click += (_, _) => ShowPage(appointments: false);
        appointmentsNav.Click += (_, _) => ShowPage(appointments: true);
        viewAppointmentsTile.Click += (_, _) => ShowPage(appointments: true);

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

    private void ShowPage(bool appointments)
    {
        bodyPanel.Visible = !appointments;
        appointmentsPage.Visible = appointments;
        dashboardNav.Active = !appointments;
        appointmentsNav.Active = appointments;
        pageTitle.Text = appointments ? "Appointments" : _dashboardTitle;
        pageSubtitle.Text = appointments
            ? "Manage and track customer appointments and services."
            : _dashboardSubtitle;
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
