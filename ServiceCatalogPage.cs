using MyApp.Controls;

namespace MyApp;

/// <summary>
/// Service Catalog Management screen. Layout lives in ServiceCatalogPage.Designer.cs, so open this file's designer
/// in Visual Studio to move or restyle anything. Nothing is pre-filled: counts show 0 and the table, popular list
/// and category counts stay empty until services are loaded.
/// </summary>
internal partial class ServiceCatalogPage : UserControl
{
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
