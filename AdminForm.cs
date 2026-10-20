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
        Icon = AppIcon.Value;
        DoubleBuffered = true;
        FullScreen.Apply(this, centerContent: false);
        _ = new SidebarCollapser(sidebar, navPanel, logoutNav, brandName, brandTagline);

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
                AuditLog.Record(CurrentUser, "User Created", $"Created account {dlg.CreatedEmail}");
                RefreshUsers();
                RefreshDashboard();
            }
        };

        employeesPage.ToggleActiveRequested += (id, active) =>
        {
            if (!active && Msg.Show(this, "Deactivate this account?\n\nThey will no longer be able to sign in until you activate it again.",
                    "Pawfect", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;
            if (UserRepository.SetActive(id, active))
                AuditLog.Record(CurrentUser, active ? "User Activated" : "User Deactivated", $"User #{id}");
            else
                Msg.Show("You can't deactivate the last active admin.");
            RefreshUsers();
            RefreshDashboard();
        };

        employeesPage.EditRequested += user =>
        {
            using var dlg = new EditUserDialog(user);
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                AuditLog.Record(CurrentUser, "User Edited", $"Edited account {user.Email} (#{user.Id})");
                RefreshUsers();
                RefreshDashboard();
            }
        };

        employeesPage.AddEmployeeRequested += (_, _) =>
        {
            using var dlg = new EmployeeDialog();
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                AuditLog.Record(CurrentUser, "Employee Created", $"Added {dlg.SavedName} ({dlg.SavedRole})");
                if (dlg.CreatedLoginEmail is { } login)
                    AuditLog.Record(CurrentUser, "User Created", $"Created account {login}");
                RefreshUsers();
                RefreshDashboard();
            }
        };

        employeesPage.EditEmployeeRequested += employee =>
        {
            using var dlg = new EmployeeDialog(employee);
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                AuditLog.Record(CurrentUser, "Employee Edited", $"Edited {dlg.SavedName} ({dlg.SavedRole}) (#{employee.Id})");
                RefreshUsers();
                RefreshDashboard();
            }
        };

        employeesPage.ToggleEmployeeActiveRequested += (id, active) =>
        {
            if (!active && Msg.Show(this, "Deactivate this employee?\n\nThey stay in the list and can be activated again. Groomers are no longer offered for appointments.",
                    "Pawfect", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;
            try
            {
                EmployeeRepository.SetActive(id, active);
                AuditLog.Record(CurrentUser, active ? "Employee Activated" : "Employee Deactivated", $"Employee #{id}");
            }
            catch (Microsoft.Data.SqlClient.SqlException ex)
            {
                Msg.Show("Could not update the employee: " + ex.Message);
            }
            RefreshUsers();
            RefreshDashboard();
        };

        servicesPage.AddServiceRequested += (_, _) =>
        {
            using var dlg = new ServiceDialog();
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                AuditLog.Record(CurrentUser, "Service Created", $"Added service {dlg.SavedName}");
                RefreshServices();
            }
        };

        servicesPage.EditServiceRequested += service =>
        {
            using var dlg = new ServiceDialog(service);
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                AuditLog.Record(CurrentUser, "Service Edited", $"Edited service {dlg.SavedName} (#{service.Id})");
                RefreshServices();
            }
        };

        servicesPage.ToggleServiceActiveRequested += (id, active) =>
        {
            try
            {
                ServiceRepository.SetActive(id, active);
                AuditLog.Record(CurrentUser, active ? "Service Activated" : "Service Deactivated", $"Service #{id}");
            }
            catch (Microsoft.Data.SqlClient.SqlException ex)
            {
                Msg.Show("Could not update the service: " + ex.Message);
            }
            RefreshServices();
        };

        // Clicking the name chip (avatar, name, role or arrow) opens "My Profile".
        Ui.MakeClickable(userChip, (_, _) => ShowProfile());

        dashboardNav.Click += (_, _) => ShowPage(AdminPage.Dashboard);
        employeesNav.Click += (_, _) => ShowPage(AdminPage.Employees);
        servicesNav.Click += (_, _) => ShowPage(AdminPage.Services);
        inventoryNav.Click += (_, _) => ShowPage(AdminPage.Inventory);
        reportsNav.Click += (_, _) => ShowPage(AdminPage.Reports);
        auditNav.Click += (_, _) => ShowPage(AdminPage.Audit);

        // Empty grey ring until real appointment / retail numbers are fed in.
        splitDonut.SetData(Array.Empty<(double, Color)>(), "0%", "Grooming");

        // Daily / Weekly / Monthly switch for the Gross Revenue Summary line.
        SelectOne(grossDailyTab, grossDailyTab, grossWeeklyTab, grossMonthlyTab);
        foreach (var tab in new[] { grossDailyTab, grossWeeklyTab, grossMonthlyTab })
        {
            var current = tab;
            current.Click += (_, _) =>
            {
                SelectOne(current, grossDailyTab, grossWeeklyTab, grossMonthlyTab);
                _grossPeriod = ReferenceEquals(current, grossWeeklyTab) ? GrossPeriod.Weekly
                             : ReferenceEquals(current, grossMonthlyTab) ? GrossPeriod.Monthly
                             : GrossPeriod.Daily;
                ApplyGross();
            };
        }

        // Revenue Overview: services vs. retail for each of the last six months (two bars per month).
        revenueSubtitle.Text = "Services vs. retail gross revenue (₱), last 6 months";
        dailyLegend.Text = "Services";
        weeklyLegend.Text = "Retail";
        monthlyDot.Visible = false;
        monthlyLegend.Visible = false;

        // Alerts and Recent Activity: a scrolling list that replaces the empty-state message once there is something to show.
        _alertsList = AddList(alertsCard);
        _activityList = AddList(activityCard);
        Ui.MakeClickable(activityLink, (_, _) => ShowPage(AdminPage.Audit));   // "View All →"
    }

    private enum GrossPeriod { Daily, Weekly, Monthly }

    private GrossPeriod _grossPeriod = GrossPeriod.Daily;
    private AdminSnapshot? _snapshot;
    private readonly ToolTip _tips = new();
    private Panel? _alertsList;
    private Panel? _activityList;

    /// <summary>A scrolling list that fills the rest of a dashboard card (below its header). Hidden until it has rows.</summary>
    private static Panel AddList(CardPanel card)
    {
        var list = new Panel { Dock = DockStyle.Fill, AutoScroll = true, BackColor = Color.White, Visible = false };
        card.Controls.Add(list);
        list.BringToFront();   // same layer as the empty-state message: below the header, filling what is left
        return list;
    }

    /// <summary>Rebuilds a list; with no rows it hides the list and shows the card's empty-state message instead.</summary>
    private static void FillList(Panel list, EmptyState empty, IReadOnlyList<DashboardRowView> rows)
    {
        list.SuspendLayout();
        foreach (var old in list.Controls.OfType<DashboardRowView>().ToList())
        {
            list.Controls.Remove(old);
            old.Dispose();
        }

        // Rows are docked to the top, so add them last-to-first to keep the first row on top.
        for (int i = rows.Count - 1; i >= 0; i--)
        {
            rows[i].Dock = DockStyle.Top;
            list.Controls.Add(rows[i]);
        }
        list.AutoScrollPosition = Point.Empty;
        list.ResumeLayout(true);

        list.Visible = rows.Count > 0;
        empty.Visible = rows.Count == 0;
    }

    /// <summary>Shows who is signed in (name + role) and greets them by first name.</summary>
    public void ApplyUser(UserAccount user)
    {
        CurrentUser = user;
        userNameLabel.Text = user.Name;
        userRoleLabel.Text = user.Role.ToString();
        AvatarImages.Show(userAvatar, GetAvatarOrNull(user.Email));

        string first = user.Name.Split(' ', StringSplitOptions.RemoveEmptyEntries).FirstOrDefault() ?? user.Name;
        _dashboardSubtitle = $"Welcome back, {first}! Here's what's happening today.";
        if (bodyPanel.Visible) pageSubtitle.Text = _dashboardSubtitle;
    }

    private static byte[]? GetAvatarOrNull(string email)
    {
        try { return UserRepository.GetAvatar(email); }
        catch (Microsoft.Data.SqlClient.SqlException) { return null; } // no picture is better than no dashboard
    }

    private void ShowProfile()
    {
        if (CurrentUser is null) return;

        using var dlg = new ProfileDialog(CurrentUser, GetAvatarOrNull(CurrentUser.Email));
        if (dlg.ShowDialog(this) != DialogResult.OK || dlg.UpdatedUser is not { } updated) return;

        ApplyUser(updated);   // refreshes the chip and the welcome line
        if (employeesPage.Visible) RefreshUsers(); // the User Management table may list this account
    }

    private void ShowPage(AdminPage page) => PageFade.Swap(contentPanel, () => ShowPageCore(page));

    private void ShowPageCore(AdminPage page)
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
        if (page == AdminPage.Services) RefreshServices();
        if (page == AdminPage.Audit) RefreshAudit();
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

    private void RefreshServices()
    {
        try
        {
            servicesPage.Load(ServiceRepository.GetAll(), ServiceRepository.GetBookings());
        }
        catch (Microsoft.Data.SqlClient.SqlException ex)
        {
            Msg.Show("Could not load services: " + ex.Message);
        }
    }

    private void RefreshDashboard()
    {
        try
        {
            _snapshot = AdminStats.GetSnapshot();
            ApplySnapshot(_snapshot);
            ApplyAlerts(AdminStats.GetAlerts());
            ApplyActivity(AuditLog.GetRecent(8));
        }
        catch (Microsoft.Data.SqlClient.SqlException ex)
        {
            Msg.Show("Could not load dashboard: " + ex.Message);
        }
    }

    private static string Change(int now, int atMonthStart)
    {
        int diff = now - atMonthStart;
        return diff == 0 ? "No change vs. last month" : $"{(diff > 0 ? "+" : "−")}{Math.Abs(diff)} vs. last month";
    }

    private void ApplySnapshot(AdminSnapshot s)
    {
        var culture = System.Globalization.CultureInfo.CurrentCulture;

        // The three big cards and the Quick Metrics copies of them.
        customersValue.Text = metricCustomersValue.Text = s.Customers.ToString(culture);
        staffValue.Text = metricStaffValue.Text = s.Staff.ToString(culture);
        petsValue.Text = metricPetsValue.Text = s.Pets.ToString(culture);
        customersDelta.Text = metricCustomersDelta.Text = Change(s.Customers, s.CustomersAtMonthStart);
        staffDelta.Text = metricStaffDelta.Text = Change(s.Staff, s.StaffAtMonthStart);
        petsDelta.Text = metricPetsDelta.Text = Change(s.Pets, s.PetsAtMonthStart);

        ApplyGross();

        // Revenue Overview bars.
        if (s.ServicesByMonth.Concat(s.RetailByMonth).Any(v => v > 0))
            revenueChart.SetData(s.MonthLabels, new[]
            {
                new BarSeries("Services", dailyDot.DotColor, s.ServicesByMonth),
                new BarSeries("Retail", weeklyDot.DotColor, s.RetailByMonth),
            });
        else
            revenueChart.Clear();

        // Grooming appointments vs. walk-in retail sales (completed sales this month).
        int grooming = s.GroomingSalesThisMonth, retail = s.RetailSalesThisMonth, all = grooming + retail;
        groomingCount.Text = grooming == 1 ? "1 appointment" : $"{grooming.ToString(culture)} appointments";
        retailCount.Text = retail == 1 ? "1 sale" : $"{retail.ToString(culture)} sales";
        groomingPercent.Text = all == 0 ? "0%" : $"{Math.Round(grooming * 100.0 / all)}%";
        retailPercent.Text = all == 0 ? "0%" : $"{Math.Round(retail * 100.0 / all)}%";

        var segments = new List<(double, Color)>();
        if (grooming > 0) segments.Add((grooming, groomingDot.DotColor));
        if (retail > 0) segments.Add((retail, retailDot.DotColor));
        splitDonut.SetData(segments, groomingPercent.Text, "Grooming");
        _tips.SetToolTip(splitDonut, $"Completed sales this month: {grooming} from appointments, {retail} walk-in.");
    }

    /// <summary>Draws the Gross Revenue Summary line for the chosen Daily / Weekly / Monthly tab.</summary>
    private void ApplyGross()
    {
        if (_snapshot is not { } s) return;

        var points = _grossPeriod switch
        {
            GrossPeriod.Weekly => s.Weekly,
            GrossPeriod.Monthly => s.Monthly,
            _ => s.Daily,
        };

        if (points.All(p => p.Value <= 0)) grossChart.Clear();   // no sales in this range: show the empty message instead of a flat line
        else grossChart.SetData(points.Select(p => p.Label).ToList(), points.Select(p => p.Value).ToList());
    }

    private void ApplyAlerts(List<DashAlert> alerts)
    {
        if (_alertsList is null) return;

        var rows = alerts.Select(a => new DashboardRowView(a.Title, a.Detail, a.Meta, a.Kind switch
        {
            AlertKind.OutOfStock => Theme.Danger,
            AlertKind.LowStock => Theme.Orange,
            AlertKind.Waiting => Theme.Primary,
            _ => Theme.Purple,
        })).ToList();

        FillList(_alertsList, alertsEmpty, rows);
    }

    private void ApplyActivity(List<AuditLog.Entry> entries)
    {
        if (_activityList is null) return;

        var culture = System.Globalization.CultureInfo.CurrentCulture;
        var rows = entries.Select(e => new DashboardRowView(
            e.Action, e.Details.Length > 0 ? $"{e.Actor} · {e.Details}" : e.Actor,
            e.When.Date == DateTime.Today ? e.When.ToString("h:mm tt", culture) : e.When.ToString("MMM d", culture),
            e.Action.Contains("Deactivated", StringComparison.OrdinalIgnoreCase) ? Theme.Danger
            : e.Action.Contains("Created", StringComparison.OrdinalIgnoreCase) ? Theme.Green
            : Theme.Primary)).ToList();

        FillList(_activityList, activityEmpty, rows);
    }
    private void RefreshUsers()
    {
        try
        {
            employeesPage.LoadUsers(UserRepository.GetAll());
            employeesPage.LoadEmployees(EmployeeRepository.GetAll());
        }
        catch (Microsoft.Data.SqlClient.SqlException ex)
        {
            Msg.Show("Could not load users and employees: " + ex.Message);
        }
    }

    private void RefreshAudit()
    {
        try { auditPage.LoadEntries(AuditLog.GetRecent()); }
        catch (Microsoft.Data.SqlClient.SqlException ex)
        {
            Msg.Show("Could not load audit trail: " + ex.Message);
        }
    }
}