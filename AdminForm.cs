using MyApp.Controls;

namespace MyApp;

/// <summary>
/// The admin dashboard. Layout lives in AdminForm.Designer.cs, so open this form in the Visual Studio
/// designer to move or restyle anything. Every number, chart and list starts empty: nothing here is sample data.
/// </summary>
internal partial class AdminForm : Form
{
    /// <summary>Set when the admin picks "Log Out", so Program.cs can show the login again.</summary>
    public bool LoggedOut { get; private set; }

    /// <summary>The signed-in account. Set by <see cref="ApplyUser"/> right after the login succeeds.</summary>
    public UserAccount? CurrentUser { get; private set; }

    private enum AdminPage { Dashboard, Employees, Services, Inventory, Reports, Audit }

    private readonly EmployeeManagementPage employeesPage;
    private readonly ServiceCatalogPage servicesPage;
    private readonly InventoryFifoPage inventoryPage;
    private readonly ReportsAuditingPage reportsPage;
    private readonly AuditTrailPage auditPage; 
    private readonly string _dashboardTitle;
    private string _dashboardSubtitle;

    public AdminForm()
    {
        InitializeComponent();
        DoubleBuffered = true;

        // Pages share the area under the top bar; the sidebar switches between them.
        _dashboardTitle = pageTitle.Text;
        _dashboardSubtitle = pageSubtitle.Text;
        employeesPage = new EmployeeManagementPage { Dock = DockStyle.Fill, Visible = false };
        contentPanel.Controls.Add(employeesPage);
        employeesPage.BringToFront();
        servicesPage = new ServiceCatalogPage { Dock = DockStyle.Fill, Visible = false };
        contentPanel.Controls.Add(servicesPage);
        servicesPage.BringToFront();
        inventoryPage = new InventoryFifoPage { Dock = DockStyle.Fill, Visible = false };
        contentPanel.Controls.Add(inventoryPage);
        inventoryPage.BringToFront();
        reportsPage = new ReportsAuditingPage { Dock = DockStyle.Fill, Visible = false };
        contentPanel.Controls.Add(reportsPage);
        reportsPage.BringToFront();
        auditPage = new AuditTrailPage { Dock = DockStyle.Fill, Visible = false };
        contentPanel.Controls.Add(auditPage);
        auditPage.BringToFront();

        employeesPage.AddUserRequested += (_, _) =>
        {
            using var dlg = new AddUserDialog();
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                RefreshUsers();
                RefreshDashboard();
            }
        };

        employeesPage.ToggleActiveRequested += (id, active) =>
        {
            if (!UserRepository.SetActive(id, active))
                MessageBox.Show("You can't deactivate the last active admin.");
            RefreshUsers();
            RefreshDashboard();
        };

        employeesPage.EditRequested += user =>
        {
            using var dlg = new EditUserDialog(user);
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                RefreshUsers();
                RefreshDashboard();
            }
        };

        dashboardNav.Click += (_, _) => ShowPage(AdminPage.Dashboard);
        employeesNav.Click += (_, _) => ShowPage(AdminPage.Employees);
        servicesNav.Click += (_, _) => ShowPage(AdminPage.Services);
        inventoryNav.Click += (_, _) => ShowPage(AdminPage.Inventory);
        reportsNav.Click += (_, _) => ShowPage(AdminPage.Reports);
        auditNav.Click += (_, _) => ShowPage(AdminPage.Audit);

        // Empty grey ring until real appointment / retail numbers are fed in.
        splitDonut.SetData(Array.Empty<(double, Color)>(), "0%", "Grooming");

        // Daily / Weekly / Monthly switch for the revenue summary (selection only, until data is wired up).
        SelectOne(grossDailyTab, grossDailyTab, grossWeeklyTab, grossMonthlyTab);
        foreach (var tab in new[] { grossDailyTab, grossWeeklyTab, grossMonthlyTab })
        {
            var current = tab;
            current.Click += (_, _) => SelectOne(current, grossDailyTab, grossWeeklyTab, grossMonthlyTab);
        }
    }

    /// <summary>Shows who is signed in (name + role) and greets them by first name.</summary>
    public void ApplyUser(UserAccount user)
    {
        CurrentUser = user;
        userNameLabel.Text = user.Name;
        userRoleLabel.Text = user.Role.ToString();

        string first = user.Name.Split(' ', StringSplitOptions.RemoveEmptyEntries).FirstOrDefault() ?? user.Name;
        _dashboardSubtitle = $"Welcome back, {first}! Here's what's happening today.";
        if (bodyPanel.Visible) pageSubtitle.Text = _dashboardSubtitle;
    }

    private void ShowPage(AdminPage page)
    {
        bodyPanel.Visible = page == AdminPage.Dashboard;
        employeesPage.Visible = page == AdminPage.Employees;
        servicesPage.Visible = page == AdminPage.Services;
        inventoryPage.Visible = page == AdminPage.Inventory;
        reportsPage.Visible = page == AdminPage.Reports;
        auditPage.Visible = page == AdminPage.Audit;
        dashboardNav.Active = page == AdminPage.Dashboard;
        employeesNav.Active = page == AdminPage.Employees;
        servicesNav.Active = page == AdminPage.Services;
        inventoryNav.Active = page == AdminPage.Inventory;
        reportsNav.Active = page == AdminPage.Reports;
        auditNav.Active = page == AdminPage.Audit;

        (pageTitle.Text, pageSubtitle.Text) = page switch
        {
            AdminPage.Employees => ("Employee & User Management", "Manage your staff and system users."),
            AdminPage.Services => ("Service Catalog Management", "Manage grooming services and pricing."),
            AdminPage.Inventory => ("Inventory & FIFO Management",
                "Manage your stock, track batch entries, and ensure FIFO (First In, First Out)"),
            AdminPage.Reports => ("Reports & Financial Auditing",
                "Access sales audit logs, track inventory movement, and download audit-ready reports."),
            AdminPage.Audit => ("Audit Trail / Activity Logs",
                "Track all system actions, approvals, updates, and record changes."),
            _ => (_dashboardTitle, _dashboardSubtitle),
        };

        if (page == AdminPage.Dashboard) RefreshDashboard();    
        if (page == AdminPage.Employees) RefreshUsers();
    }

    private static void SelectOne(TabPill chosen, params TabPill[] group)
    {
        foreach (var tab in group)
            tab.Active = ReferenceEquals(tab, chosen);
    }

    private void logoutNav_Click(object? sender, EventArgs e)
    {
        LoggedOut = true;
        Close();
    }

    private void servicesNav_Click(object sender, EventArgs e)
    {

    }

    private void AdminForm_Load(object sender, EventArgs e) => RefreshDashboard();

    private void RefreshDashboard()
    {
        try
        {
            string staff = DashboardStats.CountActiveStaff().ToString();
            staffValue.Text = staff;          // big card
            metricStaffValue.Text = staff;    // Quick Metrics card
        }
        catch (Microsoft.Data.SqlClient.SqlException ex)
        {
            MessageBox.Show("Could not load dashboard: " + ex.Message);
        }
    }
    private void RefreshUsers()
    {
        try { employeesPage.LoadUsers(UserRepository.GetAll()); }
        catch (Microsoft.Data.SqlClient.SqlException ex)
        {
            MessageBox.Show("Could not load users: " + ex.Message);
        }
    }
}
