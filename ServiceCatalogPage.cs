using System.Data;
using Microsoft.Data.SqlClient;
using MyApp.Controls;

namespace MyApp;

/// <summary>
/// Service Catalog Management screen. Layout lives in ServiceCatalogPage.Designer.cs, so open this file's designer
/// in Visual Studio to move or restyle anything. Nothing is pre-filled: counts show 0 and the table, popular list
/// and category counts stay empty until services are loaded.
/// </summary>
internal partial class ServiceCatalogPage : UserControl
{
    public event Action<RoomRepository.RoomRow>? RoomEditRequested;
    public event Action<int, string>? RoomStatusRequested;

    private readonly Panel roomFeed = new() { Dock = DockStyle.Fill, BackColor = Color.White, AutoScroll = true, Visible = false };
    public event Action<ServiceRepository.ServiceRow>? EditRequested;
    public event Action<int, bool>? ToggleActiveRequested;

    private List<ServiceRepository.ServiceRow> _allServices = new();

    /// <summary>"+ Add Service" or the "Add New Service" quick action was clicked.</summary>
    public event EventHandler? AddServiceRequested;

    /// <summary>The "Manage Pricing" quick action was clicked.</summary>
    public event EventHandler? ManagePricingRequested;

    /// <summary>The "Set Time Slots" quick action was clicked.</summary>
    public event EventHandler? SetTimeSlotsRequested;

    public ServiceCatalogPage()
    {
        InitializeComponent();
        DoubleBuffered = true;

        categoryFilter.SetItems(new[] { "All Categories", "Grooming", "Health & Wellness", "Add-ons", "Others" });
        sizeFilter.SetItems(new[] { "All Sizes", "Small", "Medium", "Large", "Giant" });
        statusFilter.SetItems(new[] { "All Status", "Active", "Inactive" });

        addServiceButton.Click += (_, _) => AddServiceRequested?.Invoke(this, EventArgs.Empty);
        OnClick(addServiceAction, (_, _) => AddServiceRequested?.Invoke(this, EventArgs.Empty));
        OnClick(pricingAction, (_, _) => ManagePricingRequested?.Invoke(this, EventArgs.Empty));
        OnClick(timeSlotsAction, (_, _) => SetTimeSlotsRequested?.Invoke(this, EventArgs.Empty));

        listViewToggle.Click += (_, _) => SetView(list: true);
        gridViewToggle.Click += (_, _) => SetView(list: false);
        serviceSearch.Box.TextChanged += (_, _) => ApplyServiceFilter();
    }

    public void LoadServices(List<ServiceRepository.ServiceRow> services)
    {
        _allServices = services;
        ApplyServiceFilter();
    }

    private void ApplyServiceFilter()
    {
        string q = serviceSearch.Value.Trim();
        var shown = _allServices.Where(s =>
            q.Length == 0 ||
            s.Name.Contains(q, StringComparison.OrdinalIgnoreCase) ||
            s.Category.Contains(q, StringComparison.OrdinalIgnoreCase)).ToList();

        serviceRows.SuspendLayout();

        foreach (var old in serviceRows.Controls.Cast<Control>().Where(c => c != serviceEmpty).ToList())
        {
            serviceRows.Controls.Remove(old);
            old.Dispose();
        }

        serviceEmpty.Visible = shown.Count == 0;

        for (int i = shown.Count - 1; i >= 0; i--)
            serviceRows.Controls.Add(BuildServiceRow(shown[i]));

        // Tiles and category counts always cover every service, not just the search result.
        var active = _allServices.Where(s => s.Active).ToList();
        svcTotalValue.Text = _allServices.Count.ToString();
        svcPriceValue.Text = active.Count == 0 ? "₱0" : "₱" + active.Average(s => s.Price).ToString("N2");
        svcDurationValue.Text = active.Count == 0 ? "0 min" : $"{active.Average(s => s.DurationMinutes):0} min";

        catGroomingCount.Text = _allServices.Count(s => s.Category == "Grooming").ToString();
        catHealthCount.Text = _allServices.Count(s => s.Category == "Health & Wellness").ToString();
        catAddOnsCount.Text = _allServices.Count(s => s.Category == "Add-ons").ToString();
        catOthersCount.Text = _allServices.Count(s => s.Category == "Others").ToString();

        serviceShowing.Text = shown.Count == 0
            ? "No services"
            : $"Showing 1 to {shown.Count} of {_allServices.Count} services";

        serviceRows.ResumeLayout(true);
        popularCard.Controls.Add(roomFeed);
        roomFeed.BringToFront();
        popularTitle.Text = "Grooming Rooms";
        popularEmpty.Title = "No rooms yet";
        popularEmpty.Hint = "Use Manage Rooms to add your grooming rooms.";
        timeSlotsTitle.Text = "Manage Rooms";
        timeSlotsDesc.Text = "Add or edit grooming rooms";
    }

    public void LoadRooms(List<RoomRepository.RoomRow> rooms)
    {
        var active = rooms.Where(r => r.Active).ToList();
        popularTitle.Text = active.Count == 0
            ? "Grooming Rooms"
            : $"Grooming Rooms ({active.Count(r => r.Status == "Available")}/{active.Count} free)";

        roomFeed.SuspendLayout();
        foreach (var old in roomFeed.Controls.Cast<Control>().ToList())
        {
            roomFeed.Controls.Remove(old);
            old.Dispose();
        }

        for (int i = active.Count - 1; i >= 0; i--)
            roomFeed.Controls.Add(BuildRoomRow(active[i]));

        popularEmpty.Visible = active.Count == 0;
        roomFeed.Visible = active.Count > 0;
        roomFeed.ResumeLayout(true);
    }

    private Control BuildRoomRow(RoomRepository.RoomRow room)
    {
        Color statusColor = room.Status switch
        {
            "Available" => Color.FromArgb(46, 158, 91),
            "Occupied" => Color.FromArgb(220, 53, 69),
            _ => Color.FromArgb(217, 119, 6),   // Cleaning
        };

        var host = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(2, 8, 0, 0) };
        host.Controls.Add(new Label
        {
            Text = room.Type,
            Dock = DockStyle.Top,
            Height = 16,
            AutoEllipsis = true,
            UseMnemonic = false,
            Font = new Font("Segoe UI", 8F),
            ForeColor = Color.FromArgb(122, 136, 156),
            BackColor = Color.White,
        });
        host.Controls.Add(new Label
        {
            Text = room.Name,
            Dock = DockStyle.Top,
            Height = 20,
            AutoEllipsis = true,
            UseMnemonic = false,
            Font = new Font("Segoe UI", 9F, FontStyle.Bold),
            ForeColor = Color.FromArgb(31, 42, 60),
            BackColor = Color.White,
        });

        // Click the status to move it: Available -> Occupied -> Cleaning -> Available.
        var status = new Label
        {
            Text = room.Status,
            Dock = DockStyle.Right,
            Width = 74,
            TextAlign = ContentAlignment.MiddleCenter,
            Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
            ForeColor = statusColor,
            BackColor = Color.White,
            Cursor = Cursors.Hand,
            UseMnemonic = false,
        };
        status.Click += (_, _) =>
        {
            int next = (Array.IndexOf(RoomRepository.Statuses, room.Status) + 1) % RoomRepository.Statuses.Length;
            RoomStatusRequested?.Invoke(room.Id, RoomRepository.Statuses[next]);
        };

        var edit = new Label
        {
            Text = "Edit",
            Dock = DockStyle.Right,
            Width = 38,
            TextAlign = ContentAlignment.MiddleCenter,
            Font = new Font("Segoe UI", 9F, FontStyle.Bold),
            ForeColor = Color.FromArgb(58, 123, 213),
            BackColor = Color.White,
            Cursor = Cursors.Hand,
            UseMnemonic = false,
        };
        edit.Click += (_, _) => RoomEditRequested?.Invoke(room);

        var row = new Panel { Dock = DockStyle.Top, Height = 50, BackColor = Color.White };
        row.Controls.Add(host);    // Fill first, then the right-hand pieces
        row.Controls.Add(status);
        row.Controls.Add(edit);
        return row;
    }
    private Control BuildServiceRow(ServiceRepository.ServiceRow s)
    {
        var grid = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.White,
            Margin = Padding.Empty,
            ColumnCount = serviceColumnsGrid.ColumnCount,
            RowCount = 1,
        };
        foreach (ColumnStyle cs in serviceColumnsGrid.ColumnStyles)
            grid.ColumnStyles.Add(new ColumnStyle(cs.SizeType, cs.Width));
        grid.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

        void Put(Control header, string text, Color? color = null)
        {
            grid.Controls.Add(new Label
            {
                Text = text,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                AutoEllipsis = true,
                UseMnemonic = false,
                BackColor = Color.White,
                ForeColor = color ?? Color.FromArgb(31, 42, 60),
                Font = new Font("Segoe UI", 9F),
            }, serviceColumnsGrid.GetColumn(header), 0);
        }

        Put(serviceColService, s.Name);
        Put(serviceColCategory, s.Category);
        Put(serviceColSize, s.Size);
        Put(serviceColDuration, s.DurationMinutes + " min");
        Put(serviceColPrice, "₱" + s.Price.ToString("N2"));
        Put(serviceColStatus, s.Active ? "Active" : "Inactive",
            s.Active ? Color.FromArgb(46, 158, 91) : Color.FromArgb(122, 136, 156));

        var actions = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.White,
            WrapContents = false,
            Margin = Padding.Empty,
            Padding = new Padding(0, 14, 0, 0),
        };
        Label Link(string text, Action onClick)
        {
            var l = new Label
            {
                Text = text,
                AutoSize = true,
                Margin = new Padding(0, 0, 14, 0),
                BackColor = Color.White,
                ForeColor = Color.FromArgb(58, 123, 213),
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Cursor = Cursors.Hand,
                UseMnemonic = false,
            };
            l.Click += (_, _) => onClick();
            return l;
        }
        actions.Controls.Add(Link("Edit", () => EditRequested?.Invoke(s)));
        actions.Controls.Add(Link(s.Active ? "Deactivate" : "Activate",
            () => ToggleActiveRequested?.Invoke(s.Id, !s.Active)));
        grid.Controls.Add(actions, serviceColumnsGrid.GetColumn(serviceColActions), 0);

        var row = new Panel { Dock = DockStyle.Top, Height = 44, BackColor = Color.White };
        row.Controls.Add(grid);
        return row;
    }
    /// <summary>True while the table is in list view, false in grid view.</summary>
    public bool IsListView { get; private set; } = true;

    private void SetView(bool list)
    {
        IsListView = list;
        Style(listViewToggle, active: list);
        Style(gridViewToggle, active: !list);

        static void Style(IconBadge toggle, bool active)
        {
            toggle.FillColor = active ? Theme.Primary : Theme.Subtle;
            toggle.ForeColor = active ? Color.White : Theme.Muted;
        }
    }

    // Child controls swallow clicks, so hook the row and everything inside it.
    private static void OnClick(Control root, EventHandler handler)
    {
        root.Click += handler;
        foreach (Control child in root.Controls)
            OnClick(child, handler);
    }
}


