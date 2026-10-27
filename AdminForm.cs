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

        inventoryPage.UpdateStockRequested += (_, _) => OpenRestock(null);
        inventoryPage.RestockRequested += id => OpenRestock(id);

        inventoryPage.AddProductRequested += (_, _) =>
        {
            using var dlg = new ProductDialog();
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                AuditLog.Record(CurrentUser, "Product Created", $"Added product {dlg.SavedName} (#{dlg.SavedId})");
                RefreshInventory();
                RefreshDashboard();
            }
        };

        inventoryPage.EditProductRequested += id =>
        {
            try
            {
                if (InventoryData.GetProduct(id) is not { } product)
                {
                    Msg.Show("That product no longer exists.");
                    RefreshInventory();
                    return;
                }
                using var dlg = new ProductDialog(product);
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    AuditLog.Record(CurrentUser, "Product Edited", $"Edited product {dlg.SavedName} (#{id})");
                    RefreshInventory();
                    RefreshDashboard();
                }
            }
            catch (Microsoft.Data.SqlClient.SqlException ex)
            {
                Msg.Show("Could not open the product: " + ex.Message);
            }
        };

        inventoryPage.ViewBatchesRequested += (_, _) => inventoryPage.ShowAllBatches();
        inventoryPage.ViewAllAlertsRequested += (_, _) => inventoryPage.FilterByStatus("Out of Stock");
        inventoryPage.ViewAllLowStockRequested += (_, _) => inventoryPage.FilterByStatus("Low Stock");
        inventoryPage.GenerateReportRequested += (_, _) => ExportInventoryReport();

        // Reports & Financial Auditing: the page asks for data, this form reads it and hands it back.
        reportsPage.RefreshRequested += (_, _) => RefreshReports();
        reportsPage.DownloadReportRequested += (_, _) => ExportAudit(pdf: true);
        reportsPage.ExportPdfRequested += (_, _) => ExportAudit(pdf: true);
        reportsPage.ExportExcelRequested += (_, _) => ExportAudit(pdf: false);
        reportsPage.ViewAllReportsRequested += (_, _) => reportsPage.ResetFilters();
        reportsPage.ViewAllActivityRequested += (_, _) => ShowPage(AdminPage.Audit);
        reportsPage.ActivityRequested += e =>
        {
            // Same as the dashboard: open the Audit Trail showing just this entry.
            ShowPage(AdminPage.Audit);
            auditPage.ShowOnly(e.Details.Length > 0 ? e.Details : e.Action);
        };

        auditPage.ExportPdfRequested += (_, _) => ExportActivityLog(pdf: true);
        auditPage.ExportExcelRequested += (_, _) => ExportActivityLog(pdf: false);

        employeesPage.AddUserRequested += (_, _) =>
        {
            using var dlg = new AddUserDialog();
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                AuditLog.Record(CurrentUser, "User Created", $"Created account {dlg.CreatedEmail} (#{dlg.CreatedId})");
                RefreshUsers();
                RefreshDashboard();
            }
        };

        employeesPage.ToggleRequested += user =>
        {
            bool activate = !user.Active;
            if (!activate)
            {
                if (string.Equals(user.Email, CurrentUser?.Email, StringComparison.OrdinalIgnoreCase))
                {
                    Msg.Show("You can't deactivate the account you are signed in with.");
                    return;
                }
                if (Msg.Show(this, $"Deactivate the account for {user.Name} ({user.Email})?\n\nThey won't be able to sign in until you activate it again. Their records are kept.",
                        "Pawfect", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                    return;
            }
            try
            {
                if (UserRepository.SetActive(user.Id, activate))
                    AuditLog.Record(CurrentUser, activate ? "User Activated" : "User Deactivated",
                        $"{(activate ? "Activated" : "Deactivated")} account {user.Email} (#{user.Id})");
                else
                    Msg.Show("You can't deactivate the last active admin.");
            }
            catch (Microsoft.Data.SqlClient.SqlException ex)
            {
                Msg.Show("Could not update the account: " + ex.Message);
            }
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
                AuditLog.Record(CurrentUser, "Employee Created", $"Added {dlg.SavedName} ({dlg.SavedRole}) (#{dlg.SavedId})");
                if (dlg.CreatedLoginEmail is { } login)
                    AuditLog.Record(CurrentUser, "User Created", $"Created account {login} (#{dlg.CreatedLoginId})");
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

        employeesPage.ToggleEmployeeRequested += employee =>
        {
            bool activate = !employee.Active;
            if (!activate &&
                Msg.Show(this, $"Deactivate {employee.Name}?\n\nTheir record is kept. If they have a login account they won't be able to sign in, and groomers are taken off the appointment lists, until you activate them again.",
                    "Pawfect", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                return;
            try
            {
                switch (EmployeeRepository.SetActive(employee.Id, activate, CurrentUser?.Email))
                {
                    case EmployeeRepository.ActiveResult.Done:
                        AuditLog.Record(CurrentUser, activate ? "Employee Activated" : "Employee Deactivated",
                            $"{(activate ? "Activated" : "Deactivated")} {employee.Name} ({employee.Role}) (#{employee.Id})");
                        break;
                    case EmployeeRepository.ActiveResult.LastAdmin:
                        Msg.Show("You can't deactivate the last active admin.");
                        break;
                    case EmployeeRepository.ActiveResult.Self:
                        Msg.Show("You can't deactivate the account you are signed in with.");
                        break;
                }
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
                AuditLog.Record(CurrentUser, "Service Created", $"Added service {dlg.SavedName} (#{dlg.SavedId})");
                RefreshServices();
            }
        };

        servicesPage.ManagePricingRequested += (_, _) =>
        {
            using var dlg = new ManagePricingDialog();
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                AuditLog.Record(CurrentUser, "Service Prices Updated", $"Changed the price of {dlg.ChangedCount} service(s)");
                RefreshServices();
            }
        };

        servicesPage.SetTimeSlotsRequested += (_, _) =>
        {
            using var dlg = new TimeSlotsDialog();
            if (dlg.ShowDialog(this) == DialogResult.OK)
                AuditLog.Record(CurrentUser, "Time Slots Updated", $"Service hours set to {dlg.SavedSummary}");
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
        InitNotifications();

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
        // The designer sized these for "Daily" / "Weekly", which cut "Services" down to "Servic": fit each label to its text.
        foreach (var legend in new[] { dailyLegend, weeklyLegend })
            legend.Width = TextRenderer.MeasureText(legend.Text, legend.Font, new Size(int.MaxValue, int.MaxValue),
                TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine).Width + 8;
        revenueLegend.Width = dailyDot.Width + dailyLegend.Width + dailyLegend.Margin.Horizontal +
                              weeklyDot.Width + weeklyLegend.Width + weeklyLegend.Margin.Horizontal + 6;   // as wide as its content, so the title keeps the rest
        monthlyDot.Visible = false;
        monthlyLegend.Visible = false;

        // Alerts and Recent Activity: a scrolling list that replaces the empty-state message once there is something to show.
        _alertsList = AddList(alertsCard);
        _activityList = AddList(activityCard);
        Ui.MakeClickable(activityLink, (_, _) => ShowPage(AdminPage.Audit));   // "View All →"

        // The mouse wheel scrolls whichever of the two lists the pointer is over (a plain panel only reacts when it has focus).
        var wheel = new WheelScroller(this, () => new[] { _alertsList, _activityList });
        Application.AddMessageFilter(wheel);
        FormClosed += (_, _) => Application.RemoveMessageFilter(wheel);
        SetupRoomMonitor();
        RefreshRoomMonitor();
    }

    /// <summary>Routes mouse-wheel turns to the dashboard list under the pointer.</summary>
    private sealed class WheelScroller : IMessageFilter
    {
        private const int WM_MOUSEWHEEL = 0x020A;
        private readonly Form _form;
        private readonly Func<Panel?[]> _lists;

        public WheelScroller(Form form, Func<Panel?[]> lists) { _form = form; _lists = lists; }

        public bool PreFilterMessage(ref Message m)
        {
            if (m.Msg != WM_MOUSEWHEEL || !ReferenceEquals(Form.ActiveForm, _form)) return false;

            var pointer = Cursor.Position;
            foreach (var list in _lists())
            {
                if (list is null || !list.IsHandleCreated || !list.Visible || !list.VerticalScroll.Visible) continue;
                if (!list.RectangleToScreen(list.ClientRectangle).Contains(pointer)) continue;

                int delta = (short)((m.WParam.ToInt64() >> 16) & 0xFFFF);   // 120 per notch, positive = wheel up
                int offset = -list.AutoScrollPosition.Y;
                list.AutoScrollPosition = new Point(0, Math.Max(0, offset - delta * DashboardRowView.RowHeight / 120));
                return true;
            }
            return false;
        }

    }

    private Panel? _roomList;

    /// <summary>Turns the Quick Metrics card into Room Monitoring. Call once, from the constructor.</summary>
    private void SetupRoomMonitor()
    {
        metricsTitle.Text = "Room Monitoring";
        metricsTitle.AutoEllipsis = true;
        metricsBody.Visible = false;   // the old three metric tiles

        _roomList = new Panel { Dock = DockStyle.Fill, AutoScroll = true, BackColor = Color.White };
        metricsCard.Controls.Add(_roomList);
        _roomList.BringToFront();   // same layering the Alerts and Activity lists use
    }

    private sealed record RoomLine(string Name, string Status, string Next);

    /// <summary>Active rooms with a live status worked out from the appointments (dbo.Rooms has no Status column).</summary>
    private static List<RoomLine> LoadRooms()
    {
        var now = DateTime.Now;
        return RoomScheduleRepository.GetRooms(now)
            .Where(r => r.Active)
            .Select(r => new RoomLine(
                r.Name,
                r.State switch { RoomState.InUse => "Occupied", RoomState.Sanitizing => "Sanitizing", _ => "Available" },
                r.State == RoomState.InUse && r.Until is { } until ? $"In use until {until:h:mm tt}"
                : r.State == RoomState.Sanitizing && r.Until is { } ready ? $"Ready at {ready:h:mm tt}"
                : r.Next is { } next ? $"Next: {next:h:mm tt}"
                : "Nothing else booked"))
            .ToList();
    }

    private void RefreshRoomMonitor()
    {
        if (_roomList is null) return;

        List<RoomLine> rooms;
        try { rooms = LoadRooms(); }
        catch (Microsoft.Data.SqlClient.SqlException ex)
        {
            Msg.Show("Could not load rooms: " + ex.Message);
            return;
        }

        metricsTitle.Text = rooms.Count == 0
            ? "Room Monitoring"
            : $"Room Monitoring ({rooms.Count(r => r.Status == "Available")}/{rooms.Count} free)";

        _roomList.SuspendLayout();
        foreach (var old in _roomList.Controls.Cast<Control>().ToList())
        {
            _roomList.Controls.Remove(old);
            old.Dispose();
        }

        if (rooms.Count == 0)
        {
            _roomList.Controls.Add(new Label
            {
                Text = "No rooms yet.",
                Dock = DockStyle.Top,
                Height = 60,
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Theme.Muted,
                BackColor = Color.White,
            });
        }
        else
        {
            // Rows dock to the top, so add last-to-first to keep the first room on top.
            for (int i = rooms.Count - 1; i >= 0; i--)
            {
                var r = rooms[i];
                var row = new DashboardRowView(
                    r.Name,
                    r.Next,
                    r.Status,
                    r.Status switch { "Available" => Theme.Green, "Occupied" => Theme.Danger, _ => Theme.Orange });
                row.Dock = DockStyle.Top;
                _roomList.Controls.Add(row);
            }
        }
        _roomList.ResumeLayout(true);
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
        int keepScroll = -list.AutoScrollPosition.Y;   // the dashboard refreshes now and then: stay where the admin scrolled to
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
        list.ResumeLayout(true);
        list.AutoScrollPosition = new Point(0, keepScroll);

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
        if (page == AdminPage.Inventory) RefreshInventory();
        if (page == AdminPage.Reports) RefreshReports(reloadLists: true);
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

    private void OpenRestock(int? productId)
    {
        try
        {
            using var dlg = new AddStockDialog(productId);
            if (dlg.ShowDialog(this) == DialogResult.OK && dlg.Summary is { } summary)
            {
                AuditLog.Record(CurrentUser, "Stock Added", $"{summary} (Product #{dlg.ProductId})");
                RefreshInventory();
                RefreshDashboard();
                Msg.Show(this, summary, "Pawfect", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        catch (Microsoft.Data.SqlClient.SqlException ex)
        {
            Msg.Show(this, "Couldn't update the stock.\n\n" + ex.Message, "Pawfect", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void RefreshInventory()
    {
        try
        {
            var rows = InventoryData.GetRows();
            var (added, received) = InventoryData.GetMonthStats();
            inventoryPage.Load(rows, added, received);
        }
        catch (Microsoft.Data.SqlClient.SqlException ex)
        {
            Msg.Show("Could not load inventory: " + ex.Message);
        }
    }

    private void ExportInventoryReport()
    {
        List<InventoryRow> rows;
        try
        {
            rows = InventoryData.GetRows();
        }
        catch (Microsoft.Data.SqlClient.SqlException ex)
        {
            Msg.Show("Could not load inventory: " + ex.Message);
            return;
        }

        if (rows.Count == 0)
        {
            Msg.Show(this, "There are no products to put in a report yet.", "Pawfect", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        using var dialog = new SaveFileDialog
        {
            Filter = "PDF document (*.pdf)|*.pdf",
            FileName = $"inventory-{DateTime.Today.ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture)}.pdf",
        };
        if (dialog.ShowDialog(this) != DialogResult.OK) return;

        try
        {
            PdfReports.Inventory(dialog.FileName, rows, CurrentUser?.Name ?? "Admin");
        }
        catch (Exception ex)
        {
            Msg.Show(this, "Couldn't create the PDF.\n\n" + ex.Message, "Pawfect", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        try
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(dialog.FileName) { UseShellExecute = true });
        }
        catch
        {
            // The PDF is saved; there is just no PDF viewer to open it with.
        }
    }

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
            ApplyActivity(AuditLog.GetRecent(30));
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

        var rows = alerts.Select(a =>
        {
            var row = new DashboardRowView(a.Title, a.Detail, a.Meta, a.Kind switch
            {
                AlertKind.OutOfStock => Theme.Danger,
                AlertKind.LowStock => Theme.Orange,
                AlertKind.Waiting => Theme.Primary,
                _ => Theme.Purple,
            });
            if (AlertAction(a) is { } open)
            {
                row.Clickable = true;
                row.Click += (_, _) => open();
            }
            return row;
        }).ToList();

        FillList(_alertsList, alertsEmpty, rows);
    }

    /// <summary>What clicking an alert does: stock alerts open that product in Inventory, the others open the page that shows them.</summary>
    private Action? AlertAction(DashAlert a) => a.Kind switch
    {
        AlertKind.OutOfStock or AlertKind.LowStock when a.Meta == "Product" => () => { ShowPage(AdminPage.Inventory); inventoryPage.ShowItem(a.Title); },
        AlertKind.Waiting => () => ShowPage(AdminPage.Audit),     // its "Pending" tile counts the requests waiting
        AlertKind.Reversed => () => ShowPage(AdminPage.Reports),  // sales audit log
        _ => null,
    };

    private void ApplyActivity(List<AuditLog.Entry> entries)
    {
        if (_activityList is null) return;

        var culture = System.Globalization.CultureInfo.CurrentCulture;
        var rows = entries.Select(e =>
        {
            var row = new DashboardRowView(
                e.Action, e.Details.Length > 0 ? $"{e.Actor} · {e.Details}" : e.Actor,
                e.When.Date == DateTime.Today ? e.When.ToString("h:mm tt", culture) : e.When.ToString("MMM d", culture),
                (e.Action.Contains("Deactivated", StringComparison.OrdinalIgnoreCase) || e.Action.Contains("Removed", StringComparison.OrdinalIgnoreCase)) ? Theme.Danger
                : e.Action.Contains("Created", StringComparison.OrdinalIgnoreCase) ? Theme.Green
                : Theme.Primary);
            row.Clickable = true;
            // Opens the Audit Trail showing just this entry (its full text is there, the row only has room for part of it).
            row.Click += (_, _) => { ShowPage(AdminPage.Audit); auditPage.ShowOnly(e.Details.Length > 0 ? e.Details : e.Action); };
            return row;
        }).ToList();

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

    private void RefreshReports(bool reloadLists = false)
    {
        try
        {
            if (reloadLists)
                reportsPage.SetFilterLists(ReportsData.GetCategories(), ReportsData.GetStaff());

            var query = reportsPage.BuildQuery();
            var report = ReportsData.Load(query.Current, query.CompareNow, query.ComparePrev, query.Tab);
            reportsPage.ShowData(report, query.CompareLabel);
            reportsPage.SetRecentActivity(AuditLog.GetRecent(30));
        }
        catch (Microsoft.Data.SqlClient.SqlException ex)
        {
            Msg.Show("Could not load reports: " + ex.Message);
        }
    }

    /// <summary>Saves what the Reports table shows (the ticked rows, or every row of the current filter) as a PDF or an Excel workbook (.xlsx).</summary>
    private void ExportAudit(bool pdf)
    {
        var rows = reportsPage.ExportRows;
        if (rows.Count == 0)
        {
            Msg.Show(this, "There is nothing to export yet.\n\nPick another date range, category or tab, or wait until sales and stock are recorded.",
                "Pawfect", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var tab = reportsPage.CurrentTab;
        string period = reportsPage.PeriodText;
        string who = CurrentUser?.Name ?? "Admin";
        string baseName = tab switch
        {
            AuditTab.Movement => "inventory-movement",
            AuditTab.Summary => "audit-summary",
            _ => "sales-audit-log",
        };
        string ext = pdf ? ".pdf" : ".xlsx";

        using var dialog = new SaveFileDialog
        {
            Filter = pdf ? "PDF document (*.pdf)|*.pdf" : "Excel workbook (*.xlsx)|*.xlsx",
            FileName = $"{baseName}-{DateTime.Today.ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture)}{ext}",
        };
        if (dialog.ShowDialog(this) != DialogResult.OK) return;

        try
        {
            if (pdf)
                PdfReports.Audit(dialog.FileName, tab, period, rows, reportsPage.Totals, reportsPage.TotalRows, reportsPage.HasSelection, who);
            else
                XlsxExport.WriteAudit(dialog.FileName, tab, rows, reportsPage.Totals, period, who);
        }
        catch (Exception ex)
        {
            Msg.Show(this, (pdf ? "Couldn't create the PDF." : "Couldn't save the Excel file. If it is already open in Excel, close it and try again.") + "\n\n" + ex.Message,
                "Pawfect", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        AuditLog.Record(CurrentUser, "Report Exported", $"{ReportsData.Title(tab)} ({period}), {rows.Count} row{(rows.Count == 1 ? "" : "s")}, {(pdf ? "PDF" : "Excel")}");
        try { reportsPage.SetRecentActivity(AuditLog.GetRecent(30)); }
        catch (Microsoft.Data.SqlClient.SqlException) { }

        try
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(dialog.FileName) { UseShellExecute = true });
        }
        catch
        {
            // The file is saved; there is just nothing installed to open it with.
        }
    }

    private void RefreshAudit()
    {
        try
        {
            auditPage.LoadEntries(AuditLog.GetRecent(5000));
            var (pending, active, inactive) = AuditLog.GetCounts();
            auditPage.SetCounts(pending, active, inactive);
        }
        catch (Microsoft.Data.SqlClient.SqlException ex)
        {
            Msg.Show("Could not load audit trail: " + ex.Message);
        }
    }

    /// <summary>Saves the Audit Trail entries the filters show (or the ticked ones) as a PDF or an Excel workbook (.xlsx).</summary>
    private void ExportActivityLog(bool pdf)
    {
        var rows = auditPage.ExportRows;
        if (rows.Count == 0)
        {
            Msg.Show(this, "There is no activity to export.\n\nPick another date range or clear the filters first.",
                "Pawfect", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        string period = auditPage.PeriodText + (auditPage.HasSelection ? $"  \u00B7  {rows.Count} selected entries" : "");
        string who = CurrentUser?.Name ?? "Admin";
        string ext = pdf ? ".pdf" : ".xlsx";

        using var dialog = new SaveFileDialog
        {
            Filter = pdf ? "PDF document (*.pdf)|*.pdf" : "Excel workbook (*.xlsx)|*.xlsx",
            FileName = $"activity-logs-{DateTime.Today.ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture)}{ext}",
        };
        if (dialog.ShowDialog(this) != DialogResult.OK) return;

        try
        {
            if (pdf) PdfReports.ActivityLog(dialog.FileName, period, rows, who);
            else XlsxExport.WriteActivityLog(dialog.FileName, rows, period, who);
        }
        catch (Exception ex)
        {
            Msg.Show(this, (pdf ? "Couldn't create the PDF." : "Couldn't save the Excel file. If it is already open in Excel, close it and try again.") + "\n\n" + ex.Message,
                "Pawfect", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        AuditLog.Record(CurrentUser, "Report Exported", $"Activity Logs ({period}), {rows.Count} row{(rows.Count == 1 ? "" : "s")}, {(pdf ? "PDF" : "Excel")}");
        RefreshAudit();

        try
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(dialog.FileName) { UseShellExecute = true });
        }
        catch
        {
            // The file is saved; there is just nothing installed to open it with.
        }
    }
}
