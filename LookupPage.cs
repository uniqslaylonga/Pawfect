using MyApp.Controls;

namespace MyApp;

/// <summary>
/// Customer &amp; Pet Lookup screen (layout only for now). Open this file's designer in Visual Studio to move or restyle anything.
/// Nothing is loaded into it yet, so every list shows its empty state and every count reads 0.
/// </summary>
internal partial class LookupPage : UserControl
{
    public event EventHandler? SearchRequested;
    public event EventHandler? ExportCustomersRequested;
    public event EventHandler? ExportPetsRequested;

    public LookupPage()
    {
        InitializeComponent();
        DoubleBuffered = true;
    }

    private void customerTab_Click(object? sender, EventArgs e) => SelectTab(customerTab);

    private void petTab_Click(object? sender, EventArgs e) => SelectTab(petTab);

    private void SelectTab(TabPill selected)
    {
        customerTab.Active = ReferenceEquals(selected, customerTab);
        petTab.Active = ReferenceEquals(selected, petTab);
    }

    private void searchButton_Click(object? sender, EventArgs e) => SearchRequested?.Invoke(this, EventArgs.Empty);

    private void exportCustomersTile_Click(object? sender, EventArgs e) => ExportCustomersRequested?.Invoke(this, EventArgs.Empty);

    private void exportPetsTile_Click(object? sender, EventArgs e) => ExportPetsRequested?.Invoke(this, EventArgs.Empty);
}
