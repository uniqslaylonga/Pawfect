using System.Globalization;
using System.Text;
using Microsoft.Data.SqlClient;
using MyApp.Controls;

namespace MyApp;

/// <summary>Everything that connects the employee screens to the database.</summary>
internal partial class MainForm
{
    private Panel? _pendingList;
    private Panel? _alertsList;
    private Panel? _scheduleList;
    private bool _dbErrorShown;
    private List<AppointmentItem> _pendingToday = new();
    private List<AppointmentItem> _waiting = new();   // every appointment waiting to be confirmed, on any day (bell + Alerts card)
    private System.Windows.Forms.Timer? _alertTimer;
    private int _pendingFilter;   // 0 = All, 1 = Walk-in, 2 = Appointment, 3 = Add-on

    // ------------------------------------------------------------------ one-time wiring

    private void WireData()
    {
        _pendingList = AddListHost(pendingCard);
        _alertsList = AddListHost(alertsCard);

        // A customer can send a request from the kiosk (another PC) while staff are logged in,
        // so check for changes every 3 seconds and reload the screens when something is new.
        _alertTimer = new System.Windows.Forms.Timer { Interval = 3_000 };
        _alertTimer.Tick += (_, _) => PollForChanges();
        _alertTimer.Start();
        Disposed += (_, _) => _alertTimer.Dispose();
        _scheduleList = AddListHost(scheduleCard);

        // A negative id is a walk-in SALE listed under Walk-ins: View opens the sale, and it has nothing to edit or cancel.
        appointmentsPage.ViewRequested += (_, item) => { if (item.Id < 0) ShowWalkInSale(item); else ShowAppointmentDetails(item); };
        appointmentsPage.EditRequested += (_, item) => { if (item.Id < 0) ShowWalkInSale(item); else EditAppointment(item); };   // the row's Reschedule icon
        appointmentsPage.MoreRequested += (_, item) => { if (item.Id < 0) ShowWalkInSale(item); else ShowStatusMenu(item); };
        appointmentsPage.ManageCustomersRequested += (_, _) => ShowPage(Page.Lookup);
        appointmentsPage.ServicesCatalogRequested += (_, _) => ShowServiceList();
        appointmentsPage.NewAppointmentRequested += (_, _) => NewAppointment();

        billingPage.PetServiceLoader = EmployeeData.GetServicesForPet;
        billingPage.PrintReceiptRequested += (_, sale) => CompleteSale(sale);
        billingPage.ViewTransactionRequested += (_, item) => ShowTransactionDetails(item);
        billingPage.ViewCustomerRequested += (_, _) => ShowPage(Page.Lookup);
        billingPage.TransactionHistoryRequested += (_, _) => ShowTransactionHistory();
        appointmentsPage.ViewFullCalendarRequested += (_, _) => ShowFullCalendar();
        scheduleLink.Click += (_, _) => ShowFullCalendar(DateTime.Today);   // dashboard "Today's Schedule" -> View Calendar

        // The four tiles at the top of the dashboard open the list they count.
        Ui.MakeClickable(appointmentsCard, (_, _) => { ShowPage(Page.Appointments); appointmentsPage.ShowToday(); });
        Ui.MakeClickable(completedCard, (_, _) => { ShowPage(Page.Appointments); appointmentsPage.ShowToday(AppointmentStatus.Completed); });
        Ui.MakeClickable(queueCard, (_, _) => { ShowPage(Page.Appointments); appointmentsPage.ShowPending(); });
        Ui.MakeClickable(walkInsCard, (_, _) => ShowTransactionHistory());   // the "Sales" tile opens today's sales

        // Dashboard "View All" links
        pendingLink.Click += (_, _) =>
        {
            ShowPage(Page.Appointments);
            appointmentsPage.ShowPending();
        };
        alertsLink.Click += (_, _) =>
        {
            // Same items as the bell: appointments waiting to be confirmed.
            if (_waiting.Count > 0)
            {
                ShowPage(Page.Appointments);
                appointmentsPage.ShowPending();
                return;
            }
            Msg.Show(this,
                "No alerts right now.\n\nLow-stock, near-expiry and approval alerts will be listed here once they are available. " +
                "Stock levels and batches are managed by an admin (Inventory & FIFO).",
                "Alerts", MessageBoxButtons.OK, MessageBoxIcon.Information);
        };

        // Dashboard "pending requests" tabs. Only booked appointments exist, so Walk-in and Add-on stay empty.
        allTab.Click += (_, _) => { _pendingFilter = 0; ShowPendingList(); };
        walkInTab.Click += (_, _) => { _pendingFilter = 1; ShowPendingList(); };
        appointmentTab.Click += (_, _) => { _pendingFilter = 2; ShowPendingList(); };
        addOnTab.Click += (_, _) => { _pendingFilter = 3; ShowPendingList(); };

        lookupPage.AddCustomerRequested += (_, _) => AddCustomer();
        lookupPage.MoreRequested += (_, source) => ShowLookupMenu(source);
        lookupPage.ViewPetsRequested += (_, source) => ViewPets(source);
        lookupPage.ExportCustomersRequested += (_, _) => ExportCustomers();
        lookupPage.ExportPetsRequested += (_, _) => ExportPets();

        addCustomerTile.Click += (_, _) => AddCustomer();
        updateInventoryTile.Click += (_, _) => Msg.Show(this,
            "Stock levels and batches are managed by an admin (Inventory & FIFO).\n\nYou can sell products from Counter POS & Billing.",
            "Pawfect", MessageBoxButtons.OK, MessageBoxIcon.Information);
        generateReportTile.Click += (_, _) => ExportTodaySales();
    }

    /// <summary>A scrolling area that sits in the same spot as a card's empty-state message.</summary>
    private static Panel AddListHost(Control card)
    {
        var host = new Panel { Dock = DockStyle.Fill, AutoScroll = true, BackColor = Color.White, Visible = false };
        card.Controls.Add(host);
        card.Controls.SetChildIndex(host, 0);
        return host;
    }

    // ------------------------------------------------------------------ loading

    private void RefreshAll()
    {
        if (CurrentUser is null) return;

        try
        {
            var appointments = EmployeeData.GetAppointments();

            appointmentsPage.SetWalkInSales(EmployeeData.GetWalkInSales(DateTime.Today));
            appointmentsPage.SetAppointments(appointments);

            // Cancelled and already-billed appointments can't be billed again.
            billingPage.SetAppointments(appointments.Where(a =>
                a.Status is not (AppointmentStatus.Cancelled or AppointmentStatus.Completed)));
            billingPage.SetServices(EmployeeData.GetServices());
            billingPage.SetProducts(EmployeeData.GetProducts());
            billingPage.SetTransactions(EmployeeData.GetRecentTransactions());
            billingPage.ReceiptNumber = EmployeeData.PeekNextReceiptNumber();

            lookupPage.SetData(EmployeeData.GetCustomers(), EmployeeData.GetPets());

            ApplyDashboard(EmployeeData.GetDashboardStats(), appointments);
            _dbErrorShown = false;
        }
        catch (SqlException ex)
        {
            ShowDbError("load the latest data", ex);
        }
    }

    /// <summary>Refresh after the current click has finished, so a row isn't disposed while it is handling its own click.</summary>
    private void RefreshSoon()
    {
        if (IsHandleCreated) BeginInvoke(RefreshAll); else RefreshAll();
    }

    private void ShowDbError(string action, Exception ex)
    {
        if (_dbErrorShown && action.StartsWith("load")) return;   // don't nag on every page switch
        _dbErrorShown = true;
        Msg.Show(this,
            $"Couldn't {action}.\n\nCheck that SQL Server is running and that Sql/setup.sql, Sql/employee_setup.sql and Sql/appointment_scheduling.sql were run.\n\n{ex.Message}",
            "Pawfect", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }

    // ------------------------------------------------------------------ dashboard

    private static string Delta(int now, int before)
    {
        int diff = now - before;
        return diff == 0 ? "Same as yesterday" : $"{(diff > 0 ? "+" : "\u2212")}{Math.Abs(diff)} vs. yesterday";
    }

    /// <summary>"45 mins", "1 hr 15 mins", or an em dash when there are no completed services to average.</summary>
    private static string FormatMinutes(double? minutes)
    {
        if (minutes is not double m || m <= 0) return "\u2014";
        int total = (int)Math.Round(m);
        if (total < 60) return total == 1 ? "1 min" : $"{total} mins";
        int h = total / 60, rest = total % 60;
        string hours = h == 1 ? "1 hr" : $"{h} hrs";
        return rest == 0 ? hours : $"{hours} {rest} mins";
    }

    private void ApplyDashboard(DashboardSnapshot s, IReadOnlyList<AppointmentItem> appointments)
    {
        var culture = CultureInfo.CurrentCulture;

        appointmentsValue.Text = s.AppointmentsToday.ToString(culture);
        appointmentsDelta.Text = Delta(s.AppointmentsToday, s.AppointmentsYesterday);
        walkInsValue.Text = s.WalkInsToday.ToString(culture);
        walkInsDelta.Text = Delta(s.WalkInsToday, s.WalkInsYesterday);
        completedValue.Text = s.ServicesCompletedToday.ToString(culture);
        completedDelta.Text = Delta(s.ServicesCompletedToday, s.ServicesCompletedYesterday);
        queueValue.Text = appointments.Count(a => a.Status is AppointmentStatus.Pending or AppointmentStatus.Rescheduled).ToString(culture);
        queueDelta.Text = "Awaiting confirmation";

        servicesValue.Text = s.ServicesCompletedToday.ToString(culture);
        customersValue.Text = s.TotalCustomers.ToString(culture);
        revenueValue.Text = Money.Symbol + s.RevenueToday.ToString("N2", culture);
        avgTimeValue.Text = FormatMinutes(s.AvgServiceMinutes);

        // Service type breakdown (today's completed services)
        int Count(string category) => s.ServicesByCategory
            .Where(c => string.Equals(c.Category, category, StringComparison.OrdinalIgnoreCase)).Sum(c => c.Count);
        int grooming = Count("Grooming"), health = Count("Health & Wellness"), addOns = Count("Add-ons");
        int all = s.ServicesByCategory.Sum(c => c.Count);

        static string Pct(int part, int whole) => whole == 0 ? "0%" : $"{Math.Round(part * 100.0 / whole)}%";
        groomingCount.Text = grooming.ToString(culture);
        groomingPercent.Text = Pct(grooming, all);
        healthCount.Text = health.ToString(culture);
        healthPercent.Text = Pct(health, all);
        addOnsCount.Text = addOns.ToString(culture);
        addOnsPercent.Text = Pct(addOns, all);

        var segments = new List<(double, Color)>();
        if (grooming > 0) segments.Add((grooming, groomingDot.DotColor));
        if (health > 0) segments.Add((health, healthDot.DotColor));
        if (addOns > 0) segments.Add((addOns, addOnsDot.DotColor));
        serviceDonut.SetData(segments, all.ToString(culture), "Completed");

        // Today's lists
        var today = appointments.Where(a => a.Start.Date == DateTime.Today && a.Status != AppointmentStatus.Cancelled)
            .OrderBy(a => a.Start).ToList();
        // "Pending Requests" lists every request still waiting to be confirmed, on any day (same set as the bell and
        // the Alerts card), earliest first - not just the ones booked for today.
        var pending = appointments
            .Where(a => a.Status is AppointmentStatus.Pending or AppointmentStatus.Rescheduled)
            .OrderBy(a => a.Start).ToList();

        FillList(_scheduleList, scheduleEmpty, today);
        _pendingToday = pending;
        ShowPendingList();
        SetWaiting(appointments);

        // Pending requests come from booked appointments: services in the "Add-ons" category are the add-on requests,
        // the rest are plain appointments. Walk-ins are never booked, so that tab has nothing to count.
        allTab.Text = $"All ({pending.Count})";
        walkInTab.Text = $"Walk-in ({pending.Count(a => a.IsWalkIn)})";
        appointmentTab.Text = $"Appointment ({pending.Count(a => !a.IsWalkIn && !IsAddOn(a))})";
        addOnTab.Text = $"Add-on ({pending.Count(IsAddOn)})";
    }

    private static bool IsAddOn(AppointmentItem a)
        => string.Equals(a.Category, "Add-ons", StringComparison.OrdinalIgnoreCase);

    private void ShowPendingList()
    {
        var items = _pendingFilter switch
        {
            1 => _pendingToday.Where(a => a.IsWalkIn).ToList(),        // came in without a booking (kiosk, or staff ticked Walk-in)
            2 => _pendingToday.Where(a => !a.IsWalkIn && !IsAddOn(a)).ToList(),
            3 => _pendingToday.Where(IsAddOn).ToList(),
            _ => _pendingToday,
        };
        FillPendingList(items);
    }

    /// <summary>Fills the dashboard "Pending Requests" table (time, customer &amp; pet, request, status, actions).</summary>
    private void FillPendingList(List<AppointmentItem> items)
    {
        if (_pendingList is null) return;

        _pendingList.SuspendLayout();
        foreach (var old in _pendingList.Controls.OfType<PendingRequestRowView>().ToList())
        {
            _pendingList.Controls.Remove(old);
            old.Dispose();
        }

        // Rows are docked to the top, so add them last-to-first to keep the earliest on top.
        for (int i = items.Count - 1; i >= 0; i--)
        {
            var item = items[i];
            var row = new PendingRequestRowView(item) { Dock = DockStyle.Top };
            // Let the click finish first: these actions can reload the list and dispose this row.
            row.ViewClicked += (_, _) => BeginInvoke(() => ShowAppointmentDetails(item));
            row.MoreClicked += (_, _) => BeginInvoke(() => ShowStatusMenu(item));
            _pendingList.Controls.Add(row);
        }
        _pendingList.AutoScrollPosition = Point.Empty;
        _pendingList.ResumeLayout(true);

        _pendingList.Visible = items.Count > 0;
        pendingEmpty.Visible = items.Count == 0;
    }

    /// <summary>Updates the bell dot and the Alerts card with every appointment still waiting to be confirmed (any day).</summary>
    private void SetWaiting(IEnumerable<AppointmentItem> appointments)
    {
        _waiting = appointments
            .Where(a => a.Status is AppointmentStatus.Pending or AppointmentStatus.Rescheduled)
            .OrderBy(a => a.Start).ToList();
        notificationButton.ShowDot = _waiting.Count > 0;
        FillAlertsList(_waiting);
    }

    private string? _lastSignature;

    /// <summary>
    /// Runs every few seconds. Updates the bell, and when appointments were added or changed (by the kiosk or another PC)
    /// reloads the screens. It never shows errors, never interrupts the Billing page, and waits while a dialog is open.
    /// </summary>
    private void PollForChanges()
    {
        if (CurrentUser is null) return;
        if (Application.OpenForms.Cast<Form>().Any(f => f != this && f.Visible && f.Modal)) return;

        try
        {
            var appointments = EmployeeData.GetAppointments();
            SetWaiting(appointments);

            string signature = string.Join(";", appointments.Select(a =>
                $"{a.Id}:{a.Status}:{a.Start.Ticks}:{a.Service}:{a.QueueNumber}:{a.PetName}"));
            if (_lastSignature is null) { _lastSignature = signature; return; }   // first look: nothing to compare with yet
            if (signature == _lastSignature) return;

            _lastSignature = signature;
            if (!billingPage.Visible) RefreshAll();   // billing has a sale in progress; it reloads when the page is reopened
        }
        catch (SqlException) { }
    }

    /// <summary>Quiet background check for new requests; never shows an error (the next full refresh will).</summary>
    private void RefreshAlerts()
    {
        if (CurrentUser is null) return;
        try { SetWaiting(EmployeeData.GetAppointments()); }
        catch (SqlException) { }
    }

    /// <summary>"9:00 AM" for today, "Oct 14, 9:00 AM" for another day.</summary>
    private static string WhenText(DateTime start)
        => start.Date == DateTime.Today
            ? start.ToString("h:mm tt", CultureInfo.CurrentCulture)
            : start.ToString("MMM d, h:mm tt", CultureInfo.CurrentCulture);

    /// <summary>Fills the dashboard "Alerts" card with the same items the bell shows (appointments waiting to be confirmed).</summary>
    private void FillAlertsList(List<AppointmentItem> items)
    {
        if (_alertsList is null) return;

        _alertsList.SuspendLayout();
        foreach (var old in _alertsList.Controls.OfType<AlertRowView>().ToList())
        {
            _alertsList.Controls.Remove(old);
            old.Dispose();
        }

        // Rows are docked to the top, so add them last-to-first to keep the earliest on top.
        for (int i = items.Count - 1; i >= 0; i--)
        {
            var row = new AlertRowView(items[i]) { Dock = DockStyle.Top };
            // Let the click finish first: switching pages reloads the dashboard and disposes this row.
            row.Click += (_, _) => BeginInvoke(() =>
            {
                ShowPage(Page.Appointments);
                appointmentsPage.ShowPending();
            });
            _alertsList.Controls.Add(row);
        }
        _alertsList.AutoScrollPosition = Point.Empty;
        _alertsList.ResumeLayout(true);

        _alertsList.Visible = items.Count > 0;
        alertsEmpty.Visible = items.Count == 0;
    }

    private void ShowNotifications()
    {
        const int width = 340;
        var menu = PawMenu.Create(width);

        if (_waiting.Count == 0)
        {
            menu.Items.Add(new PawMenuItem(MenuKind.Header, "Notifications") { MenuWidth = width - 12, SubText = "Nothing waiting for you" });
            menu.AddDivider();
            menu.Items.Add(new PawMenuItem(MenuKind.Empty, "You're all caught up") { MenuWidth = width - 12, SubText = "No appointments need confirming." });
        }
        else
        {
            menu.Items.Add(new PawMenuItem(MenuKind.Header, "Notifications")
            {
                MenuWidth = width - 12,
                SubText = $"{_waiting.Count} appointment(s) need confirming",
                Badge = _waiting.Count.ToString(CultureInfo.CurrentCulture),
            });
            menu.AddDivider();

            foreach (var a in _waiting.Take(6))
            {
                var row = new PawMenuItem(MenuKind.Notification, $"{a.CustomerName} - {a.PetName}")
                {
                    MenuWidth = width - 12,
                    Glyph = a.PetName.Trim().Length > 0 ? a.PetName.Trim()[..1].ToUpperInvariant() : "?",
                    SubText = $"{a.Service}  \u2022  {WhenText(a.Start)}",
                    Status = a.Status,
                };
                row.Click += (_, _) => ShowPage(Page.Appointments);
                menu.Items.Add(row);
            }

            if (_waiting.Count > 6)
                menu.Items.Add(new PawMenuItem(MenuKind.Caption, $"+ {_waiting.Count - 6} more") { MenuWidth = width - 12 });

            menu.AddDivider();
            var open = new PawMenuItem(MenuKind.Button, "Open appointments") { MenuWidth = width - 12 };
            open.Click += (_, _) => ShowPage(Page.Appointments);
            menu.Items.Add(open);
        }

        // Right-align the dropdown with the bell so it doesn't run off the edge of the window.
        menu.Show(notificationButton, new Point(notificationButton.Width - width, notificationButton.Height + 6));
    }

    private void ShowFullCalendar(DateTime? startDay = null)
    {
        using var form = new FullCalendarForm(appointmentsPage.Appointments, startDay);
        if (form.ShowDialog(this) != DialogResult.OK || form.PickedDate is not { } day) return;

        ShowPage(Page.Appointments);
        appointmentsPage.ShowDate(day);
    }

    private void ShowTransactionHistory()
    {
        try
        {
            using var form = new TransactionHistoryForm();
            form.ShowDialog(this);
            if (form.Changed) RefreshSoon();
        }
        catch (SqlException ex)
        {
            ShowDbError("load the transaction history", ex);
        }
    }

    private static void FillList(Panel? host, EmptyState empty, List<AppointmentItem> items)
    {
        if (host is null) return;

        host.SuspendLayout();
        foreach (var old in host.Controls.OfType<UpcomingItemView>().ToList())
        {
            host.Controls.Remove(old);
            old.Dispose();
        }
        for (int i = items.Count - 1; i >= 0; i--)
            host.Controls.Add(new UpcomingItemView(items[i]) { Dock = DockStyle.Top });
        host.AutoScrollPosition = Point.Empty;
        host.ResumeLayout(true);

        host.Visible = items.Count > 0;
        empty.Visible = items.Count == 0;
    }

    // ------------------------------------------------------------------ customers

    private void AddCustomer()
    {
        using var form = new CustomerForm();
        if (form.ShowDialog(this) != DialogResult.OK) return;
        RefreshSoon();
    }

    /// <summary>The "..." menu on a Customer &amp; Pet Lookup row.</summary>
    private void ShowLookupMenu(object source)
    {
        var menu = PawMenu.Create(190);

        if (source is CustomerRecord c)
        {
            menu.AddAction("View pets", "\uE80F", MenuTone.Success, () => ViewPets(c));
            menu.AddAction("Add pet", "\uE710", MenuTone.Success, () => AddPetTo(c.Id, c.Name));
            menu.AddAction("New appointment", "\uE787", MenuTone.Primary, () => NewAppointment(c.Id));
            if (!c.HasLogin)
                menu.AddAction("Create login", "\uE77B", MenuTone.Primary, () => CreateLogin(c));
        }
        else if (source is PetRecord p)
        {
            menu.AddAction("View pet", "\uE80F", MenuTone.Success, () => ViewPets(p));
        }
        else
        {
            menu.Dispose();
            return;
        }

        menu.Show(Cursor.Position);
    }

    /// <summary>Opens the pet viewer (photo, details, vaccination card) for a customer, or straight to one pet.</summary>
    private void ViewPets(object source)
    {
        int customerId;
        string owner;
        int? petId = null;
        switch (source)
        {
            case CustomerRecord c: customerId = c.Id; owner = c.Name; break;
            case PetRecord p: customerId = p.CustomerId; owner = p.OwnerName; petId = p.Id; break;
            default: return;
        }

        try
        {
            using var form = new PetViewerForm(customerId, owner, petId);
            form.ShowDialog(this);
            if (form.PetsChanged) RefreshSoon();
        }
        catch (SqlException ex)
        {
            ShowDbError("open the pet profiles", ex);
        }
    }

    private void AddPetTo(int customerId, string ownerName)
    {
        using var form = new PetForm(customerId, ownerName);
        if (form.ShowDialog(this) != DialogResult.OK) return;
        RefreshSoon();
    }

    // ------------------------------------------------------------------ appointments

    /// <summary>Staff books an appointment for a customer (walk-in or phone call).</summary>
    private void NewAppointment(int? customerId = null)
    {
        try
        {
            using var form = new NewAppointmentForm(CurrentUser?.Email ?? "", customerId);
            if (form.ShowDialog(this) != DialogResult.OK) return;
        }
        catch (Exception ex) when (ex is SqlException or InvalidOperationException)
        {
            ShowDbError("open the new appointment form", ex);
            return;
        }
        RefreshSoon();
    }

    /// <summary>Staff creates the customer's sign-in so they can book their own grooming later.</summary>
    private void CreateLogin(CustomerRecord c)
    {
        using var form = new CreateCustomerLoginForm(c);
        if (form.ShowDialog(this) != DialogResult.OK) return;
        Msg.Show(this, $"{c.Name} can now sign in with the email and password you set.", "Pawfect", MessageBoxButtons.OK, MessageBoxIcon.Information);
        RefreshSoon();
    }

    private void EditAppointment(AppointmentItem item)
    {
        // Appointments are only changed through Reschedule, and only while pending, confirmed or rescheduled.
        if (item.Status is not (AppointmentStatus.Pending or AppointmentStatus.Confirmed or AppointmentStatus.Rescheduled))
        {
            Msg.Show(this,
                $"A {AppointmentStatusStyle.Label(item.Status).ToLowerInvariant()} appointment can't be rescheduled." +
                (item.Status == AppointmentStatus.Cancelled ? "\n\nUse the ... menu to restore it first." : ""),
                "Pawfect", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        try
        {
            using var form = new EditAppointmentForm(item);
            if (form.ShowDialog(this) != DialogResult.OK) return;
        }
        catch (Exception ex) when (ex is SqlException or InvalidOperationException)
        {
            ShowDbError("open the appointment", ex);
            return;
        }
        RefreshSoon();
    }

    private void ShowStatusMenu(AppointmentItem item)
    {
        var menu = PawMenu.Create(210);

        menu.Items.Add(new PawMenuItem(MenuKind.Caption, $"{item.CustomerName} - {item.PetName}") { MenuWidth = 198 });

        void Add(string text, string glyph, MenuTone tone, AppointmentStatus status)
            => menu.AddAction(text, glyph, tone, () => ChangeStatus(item, status));

        // Confirm and Check in live on the View card (see ShowAppointmentDetails).
        // Confirmed and checked-in appointments can't be cancelled; use Reschedule on the View card instead.
        if (item.Status is AppointmentStatus.Pending or AppointmentStatus.Rescheduled) Add("Cancel appointment", "\uE711", MenuTone.Warning, AppointmentStatus.Cancelled);
        if (item.Status == AppointmentStatus.Cancelled) Add("Restore as pending", "\uE777", MenuTone.Primary, AppointmentStatus.Pending);

        if (menu.Items.Count <= 1)
        {
            menu.Dispose();
            return;
        }

        menu.Show(Cursor.Position);
    }

    private void ChangeStatus(AppointmentItem item, AppointmentStatus status)
    {
        if (status == AppointmentStatus.Cancelled &&
            Msg.Show(this, $"Cancel {item.CustomerName}'s appointment for {item.PetName}?", "Pawfect",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            return;

        try
        {
            EmployeeData.SetAppointmentStatus(item.Id, status);
        }
        catch (SqlException ex)
        {
            ShowDbError("update the appointment", ex);
            return;
        }
        catch (InvalidOperationException ex)
        {
            // Not an allowed change (for example the appointment was completed or cancelled by someone else meanwhile).
            Msg.Show(this, ex.Message, "Pawfect", MessageBoxButtons.OK, MessageBoxIcon.Information);
            RefreshSoon();
            return;
        }
        RefreshSoon();
    }

    private void ShowServiceList()
    {
        try
        {
            using var form = new ServiceListForm();
            form.ShowDialog(this);
        }
        catch (SqlException ex)
        {
            ShowDbError("load the services", ex);
        }
    }

    private void ShowAppointmentDetails(AppointmentItem a)
    {
        using var dialog = new AppointmentDetailsDialog(a, offerActions: true);
        if (dialog.ShowDialog(this) != DialogResult.Yes) return;
        if (dialog.RescheduleRequested) { EditAppointment(a); return; }
        if (dialog.RequestedStatus is not { } status) return;
        ApplyReviewedStatus(a, status);
    }

    /// <summary>Confirm / Check in chosen from the review card: confirming still needs a groomer and room first.</summary>
    private void ApplyReviewedStatus(AppointmentItem item, AppointmentStatus status)
    {
        if (status == AppointmentStatus.Confirmed && (item.Groomer.Length == 0 || item.Room.Length == 0)) EditAppointment(item);
        else ChangeStatus(item, status);
    }


    // ------------------------------------------------------------------ billing

    private void CompleteSale(BillingCheckout sale)
    {
        if (CurrentUser is null) return;

        string receipt;
        try
        {
            receipt = EmployeeData.SaveSale(sale, CurrentUser.Email);
        }
        catch (InsufficientStockException ex)
        {
            Msg.Show(this,
                $"Not enough {ex.ProductName} in stock: {ex.Available} left, {ex.Requested} in the cart.\n\nLower the quantity or remove it, then try again. Nothing was saved.",
                "Pawfect", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            RefreshSoon();   // reload so the catalog shows the real counts; the cart is kept
            return;
        }
        catch (SqlException ex)
        {
            ShowDbError("save the sale", ex);
            return;   // keep the cart so nothing is lost
        }

        var data = ReceiptData.FromSale(sale, receipt, DateTime.Now, billingPage.CashierName,
            sale.Appointment?.CustomerName ?? "Walk-in", billingPage.StoreAddress, billingPage.StorePhone);

        billingPage.ClearTransaction();

        // The sale is already saved. The receipt opens on screen; Print (or Enter) sends it to the printer.
        using (var form = new ReceiptForm(data))
            form.ShowDialog(this);

        RefreshSoon();
    }

    /// <summary>Opens the sale behind a walk-in row (its id is the sale's id made negative).</summary>
    private void ShowWalkInSale(AppointmentItem item)
    {
        try
        {
            var sale = EmployeeData.GetRecentTransactions(1000).FirstOrDefault(t => t.Id == -item.Id);
            if (sale is null)
            {
                Msg.Show(this, "That sale can't be found any more.", "Pawfect", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            ShowTransactionDetails(sale);
        }
        catch (SqlException ex)
        {
            ShowDbError("open the sale", ex);
        }
    }

    private void ShowTransactionDetails(TransactionItem t)
    {
        using var form = new TransactionDetailsForm(t);
        if (form.ShowDialog(this) != DialogResult.OK) return;
        RefreshSoon();
    }

    // ------------------------------------------------------------------ export

    private string PreparedBy => CurrentUser?.Name ?? "Staff";

    private void ExportCustomers()
    {
        var rows = lookupPage.Customers;
        if (rows.Count == 0) { NothingToExport("customers"); return; }

        SavePdf("customers.pdf", path => PdfReports.Customers(path, rows, PreparedBy));
    }

    private void ExportPets()
    {
        var rows = lookupPage.Pets;
        if (rows.Count == 0) { NothingToExport("pets"); return; }

        SavePdf("pet-records.pdf", path => PdfReports.Pets(path, rows, PreparedBy));
    }

    private void ExportTodaySales()
    {
        List<TransactionItem> today;
        try
        {
            today = EmployeeData.GetRecentTransactions(1000).Where(x => x.When.Date == DateTime.Today).ToList();
        }
        catch (SqlException ex)
        {
            ShowDbError("load today's sales", ex);
            return;
        }

        if (today.Count == 0) { NothingToExport("sales for today"); return; }

        var name = $"sales-{DateTime.Today.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}.pdf";
        SavePdf(name, path => PdfReports.DailySales(path, today, PreparedBy));
    }

    /// <summary>Asks where to save, builds the PDF there, then opens it in the default PDF viewer.</summary>
    private void SavePdf(string suggestedName, Action<string> generate)
    {
        using var dialog = new SaveFileDialog { Filter = "PDF document (*.pdf)|*.pdf", FileName = suggestedName };
        if (dialog.ShowDialog(this) != DialogResult.OK) return;

        try
        {
            generate(dialog.FileName);
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

    private void NothingToExport(string what)
        => Msg.Show(this, $"There are no {what} to export yet.", "Pawfect", MessageBoxButtons.OK, MessageBoxIcon.Information);

    private void WriteCsv(string suggestedName, string[] header, IEnumerable<string[]> rows)
    {
        using var dialog = new SaveFileDialog { Filter = "CSV file (*.csv)|*.csv", FileName = suggestedName };
        if (dialog.ShowDialog(this) != DialogResult.OK) return;

        static string Q(string v)
        {
            // Stop spreadsheet formulas from running when the file is opened.
            if (v.Length > 0 && "=+-@".Contains(v[0])) v = "'" + v;
            return "\"" + v.Replace("\"", "\"\"") + "\"";
        }

        var sb = new StringBuilder();
        sb.AppendLine(string.Join(",", header.Select(Q)));
        foreach (var row in rows) sb.AppendLine(string.Join(",", row.Select(Q)));

        try
        {
            File.WriteAllText(dialog.FileName, sb.ToString(), new UTF8Encoding(true));
        }
        catch (IOException ex)
        {
            Msg.Show(this, "Couldn't save the file.\n\n" + ex.Message, "Pawfect", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}