using MyApp.Controls;

namespace MyApp;

/// <summary>
/// The admin dashboard. Layout lives in AdminForm.Designer.cs, so open this form in the Visual Studio
/// designer to move or restyle anything. Every number, chart and list starts empty: nothing here is sample data.
/// </summary>
internal partial class AdminForm : Form
{
    private readonly ToolTip roomTip = new();

    private DateTime _grossFrom, _grossTo, _revFrom, _revTo;
    private readonly Panel roomMonitorFeed = new() { Dock = DockStyle.Fill, BackColor = Color.White, AutoScroll = true };

    private readonly Panel alertFeed = new() { Dock = DockStyle.Fill, BackColor = Color.White, AutoScroll = true, Visible = false };
    private readonly Panel activityFeed = new() { Dock = DockStyle.Fill, BackColor = Color.White, AutoScroll = true, Visible = false };

    private static readonly (AdminPage Page, string[] Words)[] SearchRoutes =
    {
    (AdminPage.Dashboard, new[] { "dashboard", "home" }),
    (AdminPage.Employees, new[] { "employee", "staff", "customer", "user", "groomer", "front desk" }),
    (AdminPage.Services, new[] { "service", "grooming", "price", "catalog" }),
    (AdminPage.Inventory, new[] { "inventory", "stock", "product", "batch", "fifo" }),
    (AdminPage.Reports, new[] { "report", "sales", "revenue", "finance" }),
    (AdminPage.Audit, new[] { "audit", "activity", "log", "history" }),
};

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
        alertsCard.Controls.Add(alertFeed);
        alertFeed.BringToFront();
        activityCard.Controls.Add(activityFeed);
        activityFeed.BringToFront();

        HookClick(userChip, (_, _) => ShowUserMenu());
        notificationButton.Click += (_, _) => ShowNotifications();
        activityLink.Click += (_, _) => ShowPage(AdminPage.Audit);
        searchTextBox.KeyDown += SearchTextBox_KeyDown;

        employeesPage.AddCustomerRequested += (_, _) =>
        {
            using var dlg = new CustomerDialog();
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                AuditLog.Record(CurrentUser, "Customer Created", $"Registered customer {dlg.SavedName}");
                RefreshCustomers();
                RefreshDashboard();
            }
        };

        employeesPage.CustomerEditRequested += cust =>
        {
            using var dlg = new CustomerDialog(cust);
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                AuditLog.Record(CurrentUser, "Customer Edited", $"Edited customer {dlg.SavedName} (#{cust.Id})");
                RefreshCustomers();
                RefreshDashboard();
            }
        };

        employeesPage.CustomerToggleActiveRequested += (id, active) =>
        {
            CustomerRepository.SetActive(id, active);
            AuditLog.Record(CurrentUser, active ? "Customer Activated" : "Customer Deactivated", $"Customer #{id}");
            RefreshCustomers();
            RefreshDashboard();
        };

        employeesPage.AddEmployeeRequested += (_, _) =>
        {
            using var dlg = new EmployeeDialog();
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                AuditLog.Record(CurrentUser, "Employee Created", $"Added employee {dlg.SavedName}");
                RefreshEmployees(); RefreshDashboard();
            }
        };

        employeesPage.EmployeeEditRequested += emp =>
        {
            using var dlg = new EmployeeDialog(emp);
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                AuditLog.Record(CurrentUser, "Employee Edited", $"Edited employee {dlg.SavedName} (#{emp.Id})");
                RefreshEmployees(); RefreshDashboard();
            }
        };

        employeesPage.EmployeeToggleActiveRequested += (id, active) =>
        {
            if (EmployeeRepository.SetActive(id, active))
                AuditLog.Record(CurrentUser, active ? "Employee Activated" : "Employee Deactivated", $"Employee #{id}");
            else
                MessageBox.Show("You can't deactivate the last active admin.");
            RefreshEmployees();
            RefreshDashboard();
        };

        servicesPage.AddServiceRequested += (_, _) =>
        {
            using var dlg = new ServiceDialog();
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                AuditLog.Record(CurrentUser, "Service Created", $"Created service {dlg.SavedName}");
                RefreshServices();
            }
        };

        servicesPage.EditRequested += svc =>
        {
            using var dlg = new ServiceDialog(svc);
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                AuditLog.Record(CurrentUser, "Service Edited", $"Edited service {dlg.SavedName} (#{svc.Id})");
                RefreshServices();
            }
        };

        servicesPage.ToggleActiveRequested += (id, active) =>
        {
            ServiceRepository.SetActive(id, active);
            AuditLog.Record(CurrentUser, active ? "Service Activated" : "Service Deactivated", $"Service #{id}");
            RefreshServices();
        };

        servicesPage.SetTimeSlotsRequested += (_, _) =>
        {
            using var dlg = new RoomDialog();
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                AuditLog.Record(CurrentUser, "Room Created", $"Added room {dlg.SavedName}");
                RefreshRooms();
            }
        };

        servicesPage.RoomEditRequested += room =>
        {
            using var dlg = new RoomDialog(room);
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                AuditLog.Record(CurrentUser, "Room Edited", $"Edited room {dlg.SavedName} (#{room.Id})");
                RefreshRooms();
            }
        };

        servicesPage.RoomStatusRequested += (id, status) =>
        {
            RoomRepository.SetStatus(id, status);
            AuditLog.Record(CurrentUser, "Room Status", $"Room #{id} set to {status}");
            RefreshRooms();
        };

        inventoryPage.AddProductRequested += (_, _) =>
        {
            using var dlg = new ProductDialog();
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                AuditLog.Record(CurrentUser, "Product Created", $"Added product {dlg.SavedName}");
                RefreshInventory();
            }
        };

        inventoryPage.UpdateStockRequested += (_, _) => OpenStockDialog(null, null);
        inventoryPage.RowStockRequested += (productId, stockIn) => OpenStockDialog(productId, stockIn);

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

        
  
        // "View" link on the dashboard Alerts card, like the one on Recent Activity
        var alertsLink = new Label
        {
            Text = "View",
            Dock = DockStyle.Right,
            Width = 46,
            TextAlign = ContentAlignment.MiddleRight,
            Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
            ForeColor = Color.FromArgb(58, 123, 213),
            BackColor = Color.White,
            Cursor = Cursors.Hand,
            UseMnemonic = false,
        };
        alertsLink.Click += (_, _) => ShowAlerts(false);
        alertsHeader.Controls.Add(alertsLink);

        employeesPage.EmployeeScheduleRequested += emp =>
        {
            using var dlg = new ScheduleDialog(emp, CurrentUser);
            dlg.ShowDialog(this);
            RefreshEmployees();   // absences save immediately, so always refresh
        };

        servicesPage.ManagePricingRequested += (_, _) =>
        {
            using var dlg = new PriceAdjustDialog();
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                AuditLog.Record(CurrentUser, "Prices Updated", dlg.Summary);
                RefreshServices();
            }
        };

        inventoryPage.ViewBatchesRequested += (_, _) => ShowBatches();
        inventoryPage.GenerateReportRequested += (_, _) => { ShowPage(AdminPage.Reports); reportsPage.SelectTab(1); };
        inventoryPage.ViewAllAlertsRequested += (_, _) => ShowAlerts(false);
        inventoryPage.ViewAllLowStockRequested += (_, _) => ShowAlerts(true);

        reportsPage.DownloadReportRequested += (_, _) => PrintReport();
        reportsPage.ExportPdfRequested += (_, _) => PrintReport();
        reportsPage.ExportExcelRequested += (_, _) => ExportReportCsv();
        reportsPage.ViewAllReportsRequested += (_, _) => ShowPage(AdminPage.Reports);
        reportsPage.ViewAllActivityRequested += (_, _) => ShowPage(AdminPage.Audit);
        SetupRangePickers();
        SetupRoomMonitor(); 
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
        if (employeesPage.Visible) RefreshCustomers(); // the User Management table may list this account
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
        if (page == AdminPage.Employees) { RefreshCustomers(); RefreshEmployees(); }
        if (page == AdminPage.Audit) RefreshAudit();
        if (page == AdminPage.Services) { RefreshServices(); RefreshRooms(); }
        if (page == AdminPage.Inventory) RefreshInventory();
        if (page == AdminPage.Reports) RefreshReports();

        Control target = page switch
        {
            AdminPage.Dashboard => bodyPanel,
            AdminPage.Employees => employeesPage,
            AdminPage.Services => servicesPage,
            AdminPage.Inventory => inventoryPage,
            AdminPage.Reports => reportsPage,
            _ => auditPage,
        };
        BeginInvoke(new Action(() => ScrollToTop(target)));
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

    private static void SetDelta(Label label, int addedThisMonth)
    {
        label.Text = addedThisMonth > 0 ? $"+{addedThisMonth} this month" : "— this month";
        label.ForeColor = addedThisMonth > 0 ? Color.FromArgb(46, 158, 91) : Color.FromArgb(122, 136, 156);
    }

    private void ShowSplit((int Services, int Items) s)
    {
        int total = s.Services + s.Items;
        int groomPct = total == 0 ? 0 : (int)Math.Round(s.Services * 100.0 / total);

        splitDonut.SetData(
            total == 0
                ? Array.Empty<(double, Color)>()
                : new[] { ((double)s.Services, Color.FromArgb(58, 123, 213)), ((double)s.Items, Color.FromArgb(158, 190, 235)) },
            groomPct + "%", "Grooming");

        groomingPercent.Text = groomPct + "%";
        retailPercent.Text = (total == 0 ? 0 : 100 - groomPct) + "%";
        groomingCount.Text = $"{s.Services} services";
        retailCount.Text = $"{s.Items} items";
    }

    private void FillAlerts(List<DashboardFeed.Alert> alerts)
    {
        alertsTitle.Text = alerts.Count == 0 ? "Alerts" : $"Alerts ({alerts.Count})";
        FillFeed(alertFeed, alertsEmpty, alerts.Take(30).Select(a =>
            FeedRow(a.Title, a.Detail, a.Critical ? Color.FromArgb(220, 53, 69) : Color.FromArgb(217, 119, 6))).ToList());
    }

    private void FillActivity(List<AuditLog.Entry> entries)
    {
        FillFeed(activityFeed, activityEmpty, entries.Select(e =>
            FeedRow(e.Action, $"{e.Actor}  ·  {Ago(e.When)}", Color.FromArgb(58, 123, 213))).ToList());
    }

    private static void FillFeed(Panel feed, EmptyState empty, List<Control> rows)
    {
        feed.SuspendLayout();
        foreach (var old in feed.Controls.Cast<Control>().ToList())
        {
            feed.Controls.Remove(old);
            old.Dispose();
        }

        // Dock = Top stacks the last-added control on top, so add in reverse.
        for (int i = rows.Count - 1; i >= 0; i--)
            feed.Controls.Add(rows[i]);

        empty.Visible = rows.Count == 0;
        feed.Visible = rows.Count > 0;
        feed.ResumeLayout(true);
    }

    private static Control FeedRow(string title, string detail, Color accent)
    {
        var bar = new Panel { Dock = DockStyle.Left, Width = 4, BackColor = accent };
        var host = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(10, 6, 0, 0) };

        var detailLabel = new Label
        {
            Text = detail,
            Dock = DockStyle.Top,
            Height = 18,
            AutoEllipsis = true,
            UseMnemonic = false,
            Font = new Font("Segoe UI", 8.5F),
            ForeColor = Color.FromArgb(122, 136, 156),
            BackColor = Color.White,
        };
        var titleLabel = new Label
        {
            Text = title,
            Dock = DockStyle.Top,
            Height = 20,
            AutoEllipsis = true,
            UseMnemonic = false,
            Font = new Font("Segoe UI", 9F, FontStyle.Bold),
            ForeColor = Color.FromArgb(31, 42, 60),
            BackColor = Color.White,
        };
        host.Controls.Add(detailLabel);
        host.Controls.Add(titleLabel);

        var row = new Panel { Dock = DockStyle.Top, Height = 52, BackColor = Color.White };
        row.Controls.Add(host);
        row.Controls.Add(bar);
        return row;
    }

    private static string Ago(DateTime when)
    {
        var span = DateTime.Now - when;
        if (span.TotalMinutes < 1) return "just now";
        if (span.TotalMinutes < 60) return $"{(int)span.TotalMinutes} min ago";
        if (span.TotalHours < 24) return $"{(int)span.TotalHours} h ago";
        return when.ToString("MMM d, h:mm tt");
    }

    private static void HookClick(Control root, EventHandler handler)
    {
        root.Click += handler;
        foreach (Control child in root.Controls)
            HookClick(child, handler);
    }

    private void ShowUserMenu()
    {
        var menu = new ContextMenuStrip();
        menu.Items.Add($"{CurrentUser?.Name ?? "Admin"} ({CurrentUser?.Role})").Enabled = false;
        menu.Items.Add(new ToolStripSeparator());
        menu.Items.Add("Log Out", null, (_, _) => logoutNav_Click(null, EventArgs.Empty));
        menu.Closed += (_, _) => BeginInvoke(menu.Dispose);
        menu.Show(userChip, new Point(0, userChip.Height));
    }

    private void ShowNotifications()
    {
        List<DashboardFeed.Alert> alerts;
        try { alerts = DashboardFeed.GetAlerts(); }
        catch (Microsoft.Data.SqlClient.SqlException ex)
        {
            MessageBox.Show("Could not load alerts: " + ex.Message);
            return;
        }

        var menu = new ContextMenuStrip();
        if (alerts.Count == 0)
            menu.Items.Add("No alerts right now").Enabled = false;
        foreach (var a in alerts.Take(8))
            menu.Items.Add($"{a.Title}: {a.Detail}", null, (_, _) => ShowPage(AdminPage.Inventory));
        if (alerts.Count > 8)
            menu.Items.Add($"+ {alerts.Count - 8} more in Inventory", null, (_, _) => ShowPage(AdminPage.Inventory));
        menu.Closed += (_, _) => BeginInvoke(menu.Dispose);
        menu.Show(notificationButton, new Point(0, notificationButton.Height));
    }

    private void SearchTextBox_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.Enter) return;
        e.SuppressKeyPress = true;

        string q = searchTextBox.Text.Trim().ToLowerInvariant();
        if (q.Length == 0) return;

        foreach (var (page, words) in SearchRoutes)
        {
            if (words.Any(w => w.StartsWith(q) || q.StartsWith(w)))
            {
                ShowPage(page);
                searchTextBox.Clear();
                return;
            }
        }
        MessageBox.Show("No page matches \"" + searchTextBox.Text.Trim() + "\". Try: employees, services, inventory, reports or audit.");
    }

    private static Button NewPickButton() => new()
    {
        Text = "Pick dates",
        Size = new Size(120, 28),
        Margin = new Padding(0, 4, 0, 0),
        FlatStyle = FlatStyle.Flat,
        BackColor = Color.FromArgb(58, 123, 213),
        ForeColor = Color.White,
        Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
        Cursor = Cursors.Hand,
        UseVisualStyleBackColor = false,
        FlatAppearance = { BorderSize = 0, MouseOverBackColor = Color.FromArgb(45, 105, 190) },
    };

    private void SetupRangePickers()
    {
        var to = DateTime.Today;
        var from = to.AddDays(-29);   // default: the last 30 days
        _grossFrom = _revFrom = from;
        _grossTo = _revTo = to;

        void Setup(FlowLayoutPanel host, string title,
                   Func<(DateTime, DateTime)> get, Action<DateTime, DateTime> set)
        {
            host.Controls.Clear();
            host.WrapContents = false;
            host.Width = 130;

            var button = NewPickButton();
            button.Click += (_, _) =>
            {
                var (a, b) = get();
                using var dlg = new DateRangeDialog(title, a, b);
                if (dlg.ShowDialog(this) != DialogResult.OK) return;
                set(dlg.From, dlg.To);
                RefreshRevenue();
            };
            host.Controls.Add(button);
        }

        Setup(grossTabs, "Gross Revenue Summary",
              () => (_grossFrom, _grossTo), (a, b) => { _grossFrom = a; _grossTo = b; });
        Setup(revenueLegend, "Revenue Overview",
              () => (_revFrom, _revTo), (a, b) => { _revFrom = a; _revTo = b; });

        grossSubtitle.Text = "Pick a date range";
        revenueSubtitle.Text = "Pick a date range";
    }

    private void RefreshRevenue()
    {
        try
        {
            var g = DashboardFeed.GetRevenue(_grossFrom, _grossTo);
            grossSubtitle.Text =
                $"₱{g.Sum(x => x.Total):N2} gross revenue, {_grossFrom:MMM d} to {_grossTo:MMM d, yyyy}";

            var r = DashboardFeed.GetRevenue(_revFrom, _revTo);
            revenueSubtitle.Text =
                $"₱{r.Sum(x => x.Total):N2} over {r.Count} day(s) with sales, {_revFrom:MMM d} to {_revTo:MMM d, yyyy}";

            // TODO: feed g to grossChart and r to revenueChart once I've seen LineChart and BarChart.
        }
        catch (Microsoft.Data.SqlClient.SqlException ex)
        {
            MessageBox.Show("Could not load revenue: " + ex.Message);
        }
    }

    // ---------------------------------------------------------------- room monitoring (replaces Quick Metrics)

    private void SetupRoomMonitor()
    {
        metricsTitle.Text = "Room Monitoring";
        metricsBody.Visible = false;
        metricsCard.Controls.Add(roomMonitorFeed);
        roomMonitorFeed.BringToFront();
    }

    private void RefreshRoomMonitor()
    {
        List<RoomRepository.RoomRow> rooms;
        try { rooms = RoomRepository.GetAll().Where(r => r.Active).ToList(); }
        catch (Microsoft.Data.SqlClient.SqlException ex) when (ex.Number is 207 or 208)
        {
            rooms = new();   // the Rooms table doesn't exist yet
        }
        catch (Microsoft.Data.SqlClient.SqlException ex)
        {
            MessageBox.Show("Could not load rooms: " + ex.Message);
            return;
        }

        metricsTitle.Text = rooms.Count == 0
            ? "Room Monitoring"
            : $"Room Monitoring ({rooms.Count(r => r.Status == "Available")}/{rooms.Count} free)";

        roomMonitorFeed.SuspendLayout();
        foreach (var old in roomMonitorFeed.Controls.Cast<Control>().ToList())
        {
            roomMonitorFeed.Controls.Remove(old);
            old.Dispose();
        }

        if (rooms.Count == 0)
        {
            roomMonitorFeed.Controls.Add(new Label
            {
                Text = "No rooms yet.\nAdd them in Service Catalog > Manage Rooms.",
                Dock = DockStyle.Top,
                Height = 60,
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Color.FromArgb(122, 136, 156),
                BackColor = Color.White,
            });
        }
            else
                {
                    Dictionary<int, List<RoomBoard.Booking>> bookings;
                    try
                    {
                        bookings = RoomBoard.GetToday().GroupBy(b => b.RoomId).ToDictionary(g => g.Key, g => g.ToList());
                    }
                    catch (Microsoft.Data.SqlClient.SqlException ex) when (ex.Number is 207 or 208)
                    {
                        bookings = new();   // appointments aren't linked to rooms in this database yet
                    }
                    catch (Microsoft.Data.SqlClient.SqlException ex)
                    {
                        MessageBox.Show("Could not load room schedules: " + ex.Message);
                        bookings = new();
                    }

                    for (int i = rooms.Count - 1; i >= 0; i--)
                        roomMonitorFeed.Controls.Add(BuildMonitorRow(
                            rooms[i], bookings.TryGetValue(rooms[i].Id, out var list) ? list : new()));
                }
        roomMonitorFeed.ResumeLayout(true);
    }

    private static Label DetailLabel(string text, Color color, bool bold = false) => new()
    {
        Text = text,
        Dock = DockStyle.Top,
        Height = 17,
        AutoEllipsis = true,
        UseMnemonic = false,
        Font = new Font("Segoe UI", 8.5F, bold ? FontStyle.Bold : FontStyle.Regular),
        ForeColor = color,
        BackColor = Color.White,
    };

    private Control BuildMonitorRow(RoomRepository.RoomRow room, List<RoomBoard.Booking> bookings)
    {
        Color color = room.Status switch
        {
            "Available" => Color.FromArgb(46, 158, 91),
            "Occupied" => Color.FromArgb(220, 53, 69),
            _ => Color.FromArgb(217, 119, 6),   // Cleaning
        };
        Color text = Color.FromArgb(31, 42, 60);
        Color muted = Color.FromArgb(122, 136, 156);
        Color blue = Color.FromArgb(58, 123, 213);

        static string Time(DateTime t) => t.ToString("h:mm tt");
        static string Who(RoomBoard.Booking b) => $"{b.Pet} ({b.Customer})";

        var current = bookings.FirstOrDefault(b => b.Status == "CheckedIn");   // being groomed right now
        var next = bookings.FirstOrDefault(b => b != current);
        var focus = current ?? next;

        string nowText = current is null
            ? "No appointment in progress"
            : $"Now: {Who(current)} \u00B7 {current.Service}";
        string groomerText = focus is null ? "Groomer: \u2014"
            : focus.Groomer.Length == 0 ? "Groomer: not assigned"
            : $"Groomer: {focus.Groomer}" + (current is null ? " (next appointment)" : "");
        string nextText = next is null
            ? "Next: nothing else booked today"
            : $"Next: {Time(next.Start)} \u00B7 {Who(next)} \u00B7 {next.Service}";
        string scheduleText = bookings.Count == 0
            ? "Today: free all day"
            : "Today: " + string.Join("  \u00B7  ", bookings.Take(3).Select(b => $"{Time(b.Start)} {b.Pet}"))
              + (bookings.Count > 3 ? $"  +{bookings.Count - 3} more" : "");

        var scheduleLabel = DetailLabel(scheduleText, muted);
        if (bookings.Count > 0)
            roomTip.SetToolTip(scheduleLabel, string.Join("\n", bookings.Select(b =>
                $"{Time(b.Start)}  {Who(b)} \u00B7 {b.Service} \u00B7 {(b.Groomer.Length == 0 ? "no groomer" : b.Groomer)}")));

        // Header line: room name, type, and the clickable status.
        var header = new Panel { Dock = DockStyle.Top, Height = 24, BackColor = Color.White };
        header.Controls.Add(new Label
        {
            Text = room.Type,
            Dock = DockStyle.Fill,
            AutoEllipsis = true,
            UseMnemonic = false,
            Padding = new Padding(8, 0, 0, 0),
            TextAlign = ContentAlignment.MiddleLeft,
            Font = new Font("Segoe UI", 8F),
            ForeColor = muted,
            BackColor = Color.White,
        });

        // Click the status to move it: Available -> Occupied -> Cleaning -> Available.
        var status = new Label
        {
            Text = room.Status,
            Dock = DockStyle.Right,
            Width = 80,
            TextAlign = ContentAlignment.MiddleCenter,
            Font = new Font("Segoe UI", 9F, FontStyle.Bold),
            ForeColor = color,
            BackColor = Color.White,
            Cursor = Cursors.Hand,
            UseMnemonic = false,
        };
        status.Click += (_, _) =>
        {
            int nextStatus = (Array.IndexOf(RoomRepository.Statuses, room.Status) + 1) % RoomRepository.Statuses.Length;
            try
            {
                RoomRepository.SetStatus(room.Id, RoomRepository.Statuses[nextStatus]);
                AuditLog.Record(CurrentUser, "Room Status", $"{room.Name} set to {RoomRepository.Statuses[nextStatus]}");
            }
            catch (Microsoft.Data.SqlClient.SqlException ex)
            {
                MessageBox.Show("Could not update the room: " + ex.Message);
            }
            RefreshRoomMonitor();
            FillActivity(AuditLog.GetRecent(6));
        };
        header.Controls.Add(status);
        header.Controls.Add(new Label
        {
            Text = room.Name,
            Dock = DockStyle.Left,
            AutoSize = true,
            TextAlign = ContentAlignment.MiddleLeft,
            UseMnemonic = false,
            Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
            ForeColor = text,
            BackColor = Color.White,
        });

        var row = new Panel { Dock = DockStyle.Top, Height = 108, BackColor = Color.White, Padding = new Padding(2, 6, 0, 4) };
        row.Controls.Add(new Panel { Dock = DockStyle.Bottom, Height = 1, BackColor = Color.FromArgb(227, 233, 242) });
        // Dock = Top stacks the last-added control on top, so add bottom-to-top.
        row.Controls.Add(scheduleLabel);
        row.Controls.Add(DetailLabel(nextText, text));
        row.Controls.Add(DetailLabel(groomerText, current is null ? muted : blue, bold: current is not null));
        row.Controls.Add(DetailLabel(nowText, current is null ? muted : blue, bold: current is not null));
        row.Controls.Add(header);
        return row;
    }


    private void RefreshDashboard()
    {
        try
        {
            string staff = AdminStats.CountActiveStaff().ToString();
            string customers = CustomerRepository.CountActive().ToString();
            string pets = DashboardFeed.CountActivePets().ToString();

            staffValue.Text = metricStaffValue.Text = staff;
            customersValue.Text = metricCustomersValue.Text = customers;
            petsValue.Text = metricPetsValue.Text = pets;

            SetDelta(customersDelta, DashboardFeed.NewCustomers());
            SetDelta(staffDelta, DashboardFeed.NewStaff());
            SetDelta(petsDelta, DashboardFeed.NewPets());

            ShowSplit(DashboardFeed.GetSplit());
            FillAlerts(DashboardFeed.GetAlerts());
            FillActivity(AuditLog.GetRecent(6));
            RefreshRevenue();
            RefreshRoomMonitor();
        }
        catch (Microsoft.Data.SqlClient.SqlException ex)
        {
            MessageBox.Show("Could not load dashboard: " + ex.Message);
        }
    }


    private void RefreshCustomers()
    {
        try { employeesPage.LoadCustomers(CustomerRepository.GetAll()); }
        catch (Microsoft.Data.SqlClient.SqlException ex)
        {
            MessageBox.Show("Could not load customers: " + ex.Message);
        }
    }

    private void RefreshServices()
    {
        try { servicesPage.LoadServices(ServiceRepository.GetAll()); }
        catch (Microsoft.Data.SqlClient.SqlException ex)
        {
            MessageBox.Show("Could not load services: " + ex.Message);
        }
    }

    private void RefreshRooms()
    {
        try { servicesPage.LoadRooms(RoomRepository.GetAll()); }
        catch (Microsoft.Data.SqlClient.SqlException ex)
        {
            MessageBox.Show("Could not load rooms: " + ex.Message);
        }
    }

    private void RefreshInventory()
    {
        try { inventoryPage.LoadInventory(InventoryRepository.GetProducts(), InventoryRepository.GetBatches()); }
        catch (Microsoft.Data.SqlClient.SqlException ex)
        {
            MessageBox.Show("Could not load inventory: " + ex.Message);
        }
    }

    private void RefreshReports()
    {
        try { reportsPage.LoadReports(ReportRepository.GetMovements(), AuditLog.GetRecent()); }
        catch (Microsoft.Data.SqlClient.SqlException ex)
        {
            MessageBox.Show("Could not load reports: " + ex.Message);
        }
    }

    private void OpenStockDialog(int? productId, bool? stockIn)
    {
        List<InventoryRepository.ProductRow> products;
        try { products = InventoryRepository.GetProducts().Where(p => p.Active).ToList(); }
        catch (Microsoft.Data.SqlClient.SqlException ex)
        {
            MessageBox.Show("Could not load products: " + ex.Message);
            return;
        }

        if (products.Count == 0)
        {
            MessageBox.Show("Add a product first.");
            return;
        }

        using var dlg = new StockDialog(products, CurrentUser?.Name ?? "System", productId, stockIn);
        if (dlg.ShowDialog(this) == DialogResult.OK)
        {
            AuditLog.Record(CurrentUser, dlg.AuditAction, dlg.AuditDetails);
            RefreshInventory();
        }
    }

    private void RefreshEmployees()
    {
        try { employeesPage.LoadEmployees(EmployeeRepository.GetAll(), ScheduleRepository.GetSummaries()); }
        catch (Microsoft.Data.SqlClient.SqlException ex)
        {
            MessageBox.Show("Could not load employees: " + ex.Message);
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
    private static void ScrollToTop(Control root)
    {
        if (root is ScrollableControl { AutoScroll: true } sc) sc.AutoScrollPosition = Point.Empty;
        foreach (Control c in root.Controls) ScrollToTop(c);
    }

    private void ShowAlerts(bool lowStockOnly)
    {
        List<DashboardFeed.Alert> alerts;
        try { alerts = DashboardFeed.GetAlerts(); }
        catch (Microsoft.Data.SqlClient.SqlException ex)
        {
            MessageBox.Show("Could not load alerts: " + ex.Message);
            return;
        }
        if (lowStockOnly)
            alerts = alerts.Where(a => a.Detail.StartsWith("Low stock") || a.Detail == "Out of stock").ToList();

        using var dlg = new ListDialog(
            lowStockOnly ? "Low Stock" : "All Alerts",
            lowStockOnly ? "Products at or below their low-stock level." : "Low stock, out of stock and expiring batches.",
            new[] { "Item", "Alert", "Level" }, new[] { 200, 290, 90 },
            alerts.Select(a => new[] { a.Title, a.Detail, a.Critical ? "Critical" : "Warning" }));
        dlg.ShowDialog(this);
    }

    private void ShowBatches()
    {
        try
        {
            var products = InventoryRepository.GetProducts().ToDictionary(p => p.Id);
            var rows = InventoryRepository.GetBatches().Select((b, i) => new[]
            {
            (i + 1).ToString(),
            products.TryGetValue(b.ProductId, out var prod) ? prod.Name : "?",
            b.BatchNo,
            b.Received.ToString("MMM d, yyyy"),
            b.Expiry?.ToString("MMM d, yyyy") ?? "—",
            b.Remaining.ToString(),
        }).ToList();

            using var dlg = new ListDialog("Stock Batches", "Batches with stock left, in the order they will be used (FIFO).",
                new[] { "Use order", "Product", "Batch", "Received", "Expiry", "Left" },
                new[] { 70, 170, 90, 100, 100, 50 }, rows);
            dlg.ShowDialog(this);
        }
        catch (Microsoft.Data.SqlClient.SqlException ex)
        {
            MessageBox.Show("Could not load batches: " + ex.Message);
        }
    }

    private (string Title, string[] Headers, List<string[]> Rows) CurrentReport()
    {
        string[] titles = { "Sales Audit Logs", "Inventory Movement Report", "Audit Summary" };
        var (headers, rows) = reportsPage.CurrentTable;
        return (titles[reportsPage.SelectedTab], headers, rows);
    }

    private void PrintReport()
    {
        var (title, headers, rows) = CurrentReport();
        if (rows.Count == 0) { MessageBox.Show("There is nothing to export on this tab yet."); return; }
        ReportExport.OpenPrintable(title, headers, rows);
    }

    private void ExportReportCsv()
    {
        var (title, headers, rows) = CurrentReport();
        if (rows.Count == 0) { MessageBox.Show("There is nothing to export on this tab yet."); return; }
        ReportExport.SaveCsv(this, title.Replace(' ', '-'), headers, rows);
    }
}