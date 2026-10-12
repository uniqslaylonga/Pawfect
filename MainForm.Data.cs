using System.Globalization;
using System.Text;
using Microsoft.Data.SqlClient;
using MyApp.Controls;

namespace MyApp;

/// <summary>Everything that connects the employee screens to the database.</summary>
internal partial class MainForm
{
    private Panel? _pendingList;
    private Panel? _scheduleList;
    private bool _dbErrorShown;
    private List<AppointmentItem> _pendingToday = new();
    private int _pendingFilter;   // 0 = All, 1 = Walk-in, 2 = Appointment, 3 = Add-on

    // ------------------------------------------------------------------ one-time wiring

    private void WireData()
    {
        _pendingList = AddListHost(pendingCard);
        _scheduleList = AddListHost(scheduleCard);

        appointmentsPage.NewAppointmentRequested += (_, _) => NewAppointment();
        appointmentsPage.ViewRequested += (_, item) => ShowAppointmentDetails(item);
        appointmentsPage.EditRequested += (_, item) => EditAppointment(item);
        appointmentsPage.MoreRequested += (_, item) => ShowStatusMenu(item);
        appointmentsPage.ManageCustomersRequested += (_, _) => ShowPage(Page.Lookup);
        appointmentsPage.ServicesCatalogRequested += (_, _) => ShowServiceList();

        billingPage.PrintReceiptRequested += (_, sale) => CompleteSale(sale);
        billingPage.ViewTransactionRequested += (_, item) => ShowTransactionDetails(item);
        billingPage.ViewCustomerRequested += (_, _) => ShowPage(Page.Lookup);
        billingPage.TransactionHistoryRequested += (_, _) => ShowTransactionHistory();
        appointmentsPage.ViewFullCalendarRequested += (_, _) => ShowFullCalendar();

        // Dashboard "pending requests" tabs. Only booked appointments exist, so Walk-in and Add-on stay empty.
        allTab.Click += (_, _) => { _pendingFilter = 0; ShowPendingList(); };
        walkInTab.Click += (_, _) => { _pendingFilter = 1; ShowPendingList(); };
        appointmentTab.Click += (_, _) => { _pendingFilter = 2; ShowPendingList(); };
        addOnTab.Click += (_, _) => { _pendingFilter = 3; ShowPendingList(); };

        lookupPage.BookRequested += (_, customerId) => NewAppointment(customerId);
        lookupPage.AddCustomerRequested += (_, _) => AddCustomer();
        lookupPage.MoreRequested += (_, source) => ShowLookupMenu(source);
        lookupPage.ExportCustomersRequested += (_, _) => ExportCustomers();
        lookupPage.ExportPetsRequested += (_, _) => ExportPets();

        addCustomerTile.Click += (_, _) => AddCustomer();
        updateInventoryTile.Click += (_, _) => MessageBox.Show(this,
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
        MessageBox.Show(this,
            $"Couldn't {action}.\n\nCheck that SQL Server is running and that Sql/setup.sql and Sql/employee_setup.sql were run.\n\n{ex.Message}",
            "Pawfect", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }

    // ------------------------------------------------------------------ dashboard

    private static string Delta(int now, int before)
    {
        int diff = now - before;
        return diff == 0 ? "Same as yesterday" : $"{(diff > 0 ? "+" : "\u2212")}{Math.Abs(diff)} vs. yesterday";
    }

    private void ApplyDashboard(DashboardStats s, IReadOnlyList<AppointmentItem> appointments)
    {
        var culture = CultureInfo.CurrentCulture;

        appointmentsValue.Text = s.AppointmentsToday.ToString(culture);
        appointmentsDelta.Text = Delta(s.AppointmentsToday, s.AppointmentsYesterday);
        walkInsValue.Text = s.WalkInsToday.ToString(culture);
        walkInsDelta.Text = Delta(s.WalkInsToday, s.WalkInsYesterday);
        completedValue.Text = s.ServicesCompletedToday.ToString(culture);
        completedDelta.Text = Delta(s.ServicesCompletedToday, s.ServicesCompletedYesterday);
        queueValue.Text = s.PendingQueue.ToString(culture);
        queueDelta.Text = "Awaiting confirmation";

        servicesValue.Text = s.ServicesCompletedToday.ToString(culture);
        customersValue.Text = s.TotalCustomers.ToString(culture);
        revenueValue.Text = Money.Symbol + s.RevenueToday.ToString("N2", culture);
        avgTimeValue.Text = "\u2014";   // service duration isn't recorded

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
        var pending = today.Where(a => a.Status is AppointmentStatus.Pending or AppointmentStatus.Rescheduled).ToList();

        FillList(_scheduleList, scheduleEmpty, today);
        _pendingToday = pending;
        ShowPendingList();
        notificationButton.ShowDot = pending.Count > 0;

        // Pending requests only come from booked appointments, so the walk-in and add-on tabs have nothing to count.
        allTab.Text = $"All ({pending.Count})";
        walkInTab.Text = "Walk-in (0)";
        appointmentTab.Text = $"Appointment ({pending.Count})";
        addOnTab.Text = "Add-on (0)";
    }

    private void ShowPendingList()
        => FillList(_pendingList, pendingEmpty, _pendingFilter is 1 or 3 ? new List<AppointmentItem>() : _pendingToday);

    private void ShowNotifications()
    {
        var menu = new ContextMenuStrip { ShowImageMargin = false, Font = Theme.Face(9f) };

        if (_pendingToday.Count == 0)
        {
            menu.Items.Add(new ToolStripMenuItem("You're all caught up") { Enabled = false });
        }
        else
        {
            menu.Items.Add(new ToolStripMenuItem($"{_pendingToday.Count} appointment(s) today need confirming")
                { Enabled = false, Font = Theme.Face(9f, FontStyle.Bold) });
            foreach (var a in _pendingToday.Take(6))
                menu.Items.Add($"{a.Start.ToString("h:mm tt", CultureInfo.CurrentCulture)}  {a.CustomerName} - {a.PetName}", null,
                    (_, _) => ShowPage(Page.Appointments));
            menu.Items.Add(new ToolStripSeparator());
            menu.Items.Add("Open appointments", null, (_, _) => ShowPage(Page.Appointments));
        }

        menu.Closed += (_, _) => BeginInvoke(menu.Dispose);
        menu.Show(notificationButton, new Point(0, notificationButton.Height));
    }

    private void ShowFullCalendar()
    {
        using var form = new FullCalendarForm(appointmentsPage.Appointments);
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
        var menu = new ContextMenuStrip { ShowImageMargin = false, Font = Theme.Face(9f) };

        if (source is CustomerRecord c)
        {
            menu.Items.Add("Edit customer", null, (_, _) => EditCustomer(c));
            menu.Items.Add("Add pet", null, (_, _) => AddPetTo(c.Id, c.Name));
            menu.Items.Add("Book appointment", null, (_, _) => NewAppointment(c.Id));
            menu.Items.Add(new ToolStripSeparator());
            menu.Items.Add("Delete customer", null, (_, _) => DeleteCustomer(c));
        }
        else if (source is PetRecord p)
        {
            menu.Items.Add("Edit pet", null, (_, _) => EditPet(p));
            menu.Items.Add("Book appointment", null, (_, _) => NewAppointment(p.CustomerId));
            menu.Items.Add(new ToolStripSeparator());
            menu.Items.Add("Delete pet", null, (_, _) => DeletePet(p));
        }
        else
        {
            menu.Dispose();
            return;
        }

        menu.Closed += (_, _) => BeginInvoke(menu.Dispose);
        menu.Show(Cursor.Position);
    }

    private void EditCustomer(CustomerRecord c)
    {
        using var form = new CustomerForm(c);
        if (form.ShowDialog(this) != DialogResult.OK) return;
        RefreshSoon();
    }

    private void AddPetTo(int customerId, string ownerName)
    {
        using var form = new PetForm(customerId, ownerName);
        if (form.ShowDialog(this) != DialogResult.OK) return;
        RefreshSoon();
    }

    private void EditPet(PetRecord p)
    {
        using var form = new PetForm(p.CustomerId, p.OwnerName, p);
        if (form.ShowDialog(this) != DialogResult.OK) return;
        RefreshSoon();
    }

    private void DeleteCustomer(CustomerRecord c)
    {
        if (MessageBox.Show(this,
                $"Delete {c.Name}?\n\nTheir pets are removed from the lists too. Past appointments and receipts are kept.",
                "Pawfect", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
            return;

        string? problem;
        try { problem = EmployeeData.DeleteCustomer(c.Id); }
        catch (SqlException ex) { ShowDbError("delete the customer", ex); return; }

        if (problem is not null)
        {
            MessageBox.Show(this, problem, "Pawfect", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }
        RefreshSoon();
    }

    private void DeletePet(PetRecord p)
    {
        if (MessageBox.Show(this, $"Delete {p.Name} ({p.OwnerName}'s pet)?\n\nPast appointments and receipts are kept.",
                "Pawfect", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
            return;

        string? problem;
        try { problem = EmployeeData.DeletePet(p.Id); }
        catch (SqlException ex) { ShowDbError("delete the pet", ex); return; }

        if (problem is not null)
        {
            MessageBox.Show(this, problem, "Pawfect", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }
        RefreshSoon();
    }

    // ------------------------------------------------------------------ appointments

    private void NewAppointment(int? customerId = null)
    {
        if (CurrentUser is null) return;

        try
        {
            using var form = new NewAppointmentForm(CurrentUser.Email, customerId);
            if (form.ShowDialog(this) != DialogResult.OK) return;
        }
        catch (SqlException ex)
        {
            ShowDbError("open the appointment form", ex);
            return;
        }
        RefreshSoon();
    }

    private void EditAppointment(AppointmentItem item)
    {
        if (item.Status is AppointmentStatus.Cancelled or AppointmentStatus.Completed)
        {
            MessageBox.Show(this,
                $"A {AppointmentStatusStyle.Label(item.Status).ToLowerInvariant()} appointment can't be edited." +
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
        var menu = new ContextMenuStrip { ShowImageMargin = false, Font = Theme.Face(9f) };

        void Add(string text, AppointmentStatus status)
            => menu.Items.Add(text, null, (_, _) => ChangeStatus(item, status));

        if (item.Status is AppointmentStatus.Pending or AppointmentStatus.Rescheduled) Add("Confirm", AppointmentStatus.Confirmed);
        if (item.Status is AppointmentStatus.Pending or AppointmentStatus.Rescheduled or AppointmentStatus.Confirmed) Add("Check in", AppointmentStatus.CheckedIn);
        if (item.Status is not (AppointmentStatus.Cancelled or AppointmentStatus.Completed)) Add("Cancel appointment", AppointmentStatus.Cancelled);
        if (item.Status == AppointmentStatus.Cancelled) Add("Restore as pending", AppointmentStatus.Pending);

        // Billed appointments keep their receipt, so they can't be deleted.
        if (item.Status != AppointmentStatus.Completed)
        {
            if (menu.Items.Count > 0) menu.Items.Add(new ToolStripSeparator());
            menu.Items.Add("Delete appointment", null, (_, _) => DeleteAppointment(item));
        }

        if (menu.Items.Count == 0)
        {
            menu.Dispose();
            return;
        }

        menu.Closed += (_, _) => BeginInvoke(menu.Dispose);
        menu.Show(Cursor.Position);
    }

    private void ChangeStatus(AppointmentItem item, AppointmentStatus status)
    {
        if (status == AppointmentStatus.Cancelled &&
            MessageBox.Show(this, $"Cancel {item.CustomerName}'s appointment for {item.PetName}?", "Pawfect",
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
        RefreshSoon();
    }

    private void DeleteAppointment(AppointmentItem item)
    {
        if (MessageBox.Show(this,
                $"Permanently delete {item.CustomerName}'s appointment for {item.PetName}?\n\nThis can't be undone. (Use Cancel instead to keep a record.)",
                "Pawfect", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
            return;

        string? problem;
        try { problem = EmployeeData.DeleteAppointment(item.Id); }
        catch (SqlException ex) { ShowDbError("delete the appointment", ex); return; }

        if (problem is not null)
        {
            MessageBox.Show(this, problem, "Pawfect", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        MessageBox.Show(this,
            $"Customer:  {a.CustomerName}\nPhone:  {(a.CustomerPhone.Length == 0 ? "\u2014" : a.CustomerPhone)}\n\n" +
            $"Pet:  {a.PetName}{(a.PetBreed.Length == 0 ? "" : $" ({a.PetBreed})")}\nService:  {a.Service}\n\n" +
            $"When:  {a.Start.ToString("dddd, MMM d, yyyy  h:mm tt", CultureInfo.CurrentCulture)}\n" +
            $"Status:  {AppointmentStatusStyle.Label(a.Status)}",
            "Appointment", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        catch (SqlException ex)
        {
            ShowDbError("save the sale", ex);
            return;   // keep the cart so nothing is lost
        }

        billingPage.ClearTransaction();
        MessageBox.Show(this,
            $"Sale saved.\n\nReceipt:  {receipt}\nTotal:  {Money.Format(sale.Total)}" +
            (sale.Method == PaymentKind.Cash ? $"\nChange:  {Money.Format(sale.Change)}" : ""),
            "Pawfect", MessageBoxButtons.OK, MessageBoxIcon.Information);
        RefreshSoon();
    }

    private void ShowTransactionDetails(TransactionItem t)
    {
        using var form = new TransactionDetailsForm(t);
        if (form.ShowDialog(this) != DialogResult.OK) return;
        RefreshSoon();
    }

    // ------------------------------------------------------------------ export

    private void ExportCustomers()
    {
        var rows = lookupPage.Customers;
        if (rows.Count == 0) { NothingToExport("customers"); return; }

        WriteCsv("customers.csv",
            new[] { "Name", "Phone", "Email", "Address", "Pets", "Last Visit", "Added" },
            rows.Select(c => new[]
            {
                c.Name, c.Phone, c.Email, c.Address, c.PetNames,
                c.LastVisit?.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) ?? "",
                c.CreatedAt.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
            }));
    }

    private void ExportPets()
    {
        var rows = lookupPage.Pets;
        if (rows.Count == 0) { NothingToExport("pets"); return; }

        WriteCsv("pets.csv",
            new[] { "Pet", "Species", "Breed", "Owner", "Owner Phone" },
            rows.Select(p => new[] { p.Name, p.Species, p.Breed, p.OwnerName, p.OwnerPhone }));
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

        var inv = CultureInfo.InvariantCulture;
        WriteCsv($"sales-{DateTime.Today.ToString("yyyy-MM-dd", inv)}.csv",
            new[] { "Receipt", "Time", "Customer", "Services", "Items", "Total", "Paid By", "Status" },
            today.OrderBy(x => x.When).Select(x => new[]
            {
                x.ReceiptNumber, x.When.ToString("HH:mm", inv), x.CustomerName,
                x.ServiceCount.ToString(inv), x.ItemCount.ToString(inv), x.Total.ToString("0.00", inv),
                x.PaymentMethod, TransactionStatusStyle.Label(x.Status),
            }));
    }

    private void NothingToExport(string what)
        => MessageBox.Show(this, $"There are no {what} to export yet.", "Pawfect", MessageBoxButtons.OK, MessageBoxIcon.Information);

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
            MessageBox.Show(this, "Couldn't save the file.\n\n" + ex.Message, "Pawfect", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
