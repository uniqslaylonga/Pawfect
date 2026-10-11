using System.Globalization;
using MyApp.Controls;

namespace MyApp;


internal partial class MainForm : Form
{
    private readonly AppointmentsPage appointmentsPage;
    private readonly BillingPage billingPage;
    private readonly LookupPage lookupPage;
    private readonly string _dashboardTitle;
    private readonly string _dashboardSubtitle;

    public bool LoggedOut { get; private set; }

    /// <summary>The signed-in account. Set by <see cref="ApplyUser"/> right after the login succeeds.</summary>
    public UserAccount? CurrentUser { get; private set; }

    public bool IsAdmin => CurrentUser?.Role == UserRole.Admin;

    /// <summary>Shows who is signed in (name + role) and is the place to hide admin-only items from staff.</summary>
    public void ApplyUser(UserAccount user)
    {
        CurrentUser = user;
        userNameLabel.Text = user.Name;
        userRoleLabel.Text = user.Role.ToString();
        // Admin-only pages: when you add them, hide their nav items with  someNav.Visible = IsAdmin;
    }

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

        // Counter POS & Billing lives in the same content area too.
        billingPage = new BillingPage { Dock = DockStyle.Fill, Visible = false };
        contentPanel.Controls.Add(billingPage);
        billingPage.BringToFront();
        billingPage.OngoingAppointmentsRequested += (_, _) => ShowPage(Page.Appointments);

        // Customer & Pet Lookup shares the content area as well.
        lookupPage = new LookupPage { Dock = DockStyle.Fill, Visible = false };
        contentPanel.Controls.Add(lookupPage);
        lookupPage.BringToFront();

        dashboardNav.Click += (_, _) => ShowPage(Page.Dashboard);
        appointmentsNav.Click += (_, _) => ShowPage(Page.Appointments);
        billingNav.Click += (_, _) => ShowPage(Page.Billing);
        lookupNav.Click += (_, _) => ShowPage(Page.Lookup);
        viewAppointmentsTile.Click += (_, _) => ShowPage(Page.Appointments);

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

    private enum Page { Dashboard, Appointments, Billing, Lookup }

    private void ShowPage(Page page)
    {
        bodyPanel.Visible = page == Page.Dashboard;
        appointmentsPage.Visible = page == Page.Appointments;
        billingPage.Visible = page == Page.Billing;
        lookupPage.Visible = page == Page.Lookup;
        dashboardNav.Active = page == Page.Dashboard;
        appointmentsNav.Active = page == Page.Appointments;
        billingNav.Active = page == Page.Billing;
        lookupNav.Active = page == Page.Lookup;

        (pageTitle.Text, pageSubtitle.Text) = page switch
        {
            Page.Appointments => ("Appointments", "Manage and track customer appointments and services."),
            Page.Billing => ("Counter POS & Billing", "Process grooming services and retail sales in one transaction."),
            Page.Lookup => ("Customer & Pet Lookup", "Find customer information and their pets, view visit history, and manage records."),
            _ => (_dashboardTitle, _dashboardSubtitle),
        };

        // The receipt shows whoever is signed in.
        if (page == Page.Billing) billingPage.CashierName = userNameLabel.Text;
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
