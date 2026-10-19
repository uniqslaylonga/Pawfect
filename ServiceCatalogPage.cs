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


/// <summary>Everything the Service Catalog page needs from dbo.Services.</summary>
internal static class ServiceRepository
{

    public sealed record ServiceRow(
        int Id, string Name, string Category, string Size, string? Description,
        decimal Price, int DurationMinutes, bool Active,
        string Species = "Both", decimal? MinKg = null, decimal? MaxKg = null);   // weight band: price applies when MinKg < weight <= MaxKg

    public static List<ServiceRow> GetAll()
    {
        var list = new List<ServiceRow>();

        using var conn = new SqlConnection(Database.ConnectionString);
        conn.Open();
        ServicePricing.EnsureSchema(conn);

        using var cmd = new SqlCommand(
            "SELECT ServiceId, Name, Category, Size, Description, Price, DurationMinutes, IsActive, Species, MinKg, MaxKg " +
            "FROM dbo.Services ORDER BY Name, Species, ISNULL(MinKg, 0), Size", conn);
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
            list.Add(new ServiceRow(
                reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3),
                reader.IsDBNull(4) ? null : reader.GetString(4),
                reader.GetDecimal(5), reader.GetInt32(6), reader.GetBoolean(7),
                reader.GetString(8), reader.IsDBNull(9) ? null : reader.GetDecimal(9), reader.IsDBNull(10) ? null : reader.GetDecimal(10)));
        return list;
    }

    /// <summary>Throws SqlException 2627/2601 if this name and size already exist.</summary>
    public static void Add(string name, string category, string size, string? description, decimal price, int minutes,
        string species = "Both", decimal? minKg = null, decimal? maxKg = null)
    {
        using var conn = new SqlConnection(Database.ConnectionString);
        conn.Open();
        ServicePricing.EnsureSchema(conn);

        using var cmd = new SqlCommand(
            "INSERT INTO dbo.Services (Name, Category, Size, Description, Price, DurationMinutes, Species, MinKg, MaxKg) " +
            "VALUES (@n, @c, @s, @d, @p, @m, @sp, @min, @max)", conn);
        Fill(cmd, name, category, size, description, price, minutes, species, minKg, maxKg);
        cmd.ExecuteNonQuery();
    }

    /// <summary>Throws SqlException 2627/2601 if this name and size are taken.</summary>
    public static void Update(int id, string name, string category, string size, string? description, decimal price, int minutes,
        string species = "Both", decimal? minKg = null, decimal? maxKg = null)
    {
        using var conn = new SqlConnection(Database.ConnectionString);
        conn.Open();
        ServicePricing.EnsureSchema(conn);

        using var cmd = new SqlCommand(
            "UPDATE dbo.Services SET Name = @n, Category = @c, Size = @s, Description = @d, " +
            "Price = @p, DurationMinutes = @m, Species = @sp, MinKg = @min, MaxKg = @max WHERE ServiceId = @id", conn);
        Fill(cmd, name, category, size, description, price, minutes, species, minKg, maxKg);
        cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
        cmd.ExecuteNonQuery();
    }

    /// <summary>Soft delete: hides the service from new bookings but keeps old records valid.</summary>
    public static void SetActive(int id, bool active)
    {
        using var conn = new SqlConnection(Database.ConnectionString);
        conn.Open();

        using var cmd = new SqlCommand("UPDATE dbo.Services SET IsActive = @a WHERE ServiceId = @id", conn);
        cmd.Parameters.Add("@a", SqlDbType.Bit).Value = active;
        cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
        cmd.ExecuteNonQuery();
    }

    private static void Fill(SqlCommand cmd, string name, string category, string size, string? description, decimal price, int minutes,
        string species, decimal? minKg, decimal? maxKg)
    {
        cmd.Parameters.Add("@sp", SqlDbType.NVarChar, 10).Value = species;
        foreach (var (pname, kg) in new[] { ("@min", minKg), ("@max", maxKg) })
        {
            var w = cmd.Parameters.Add(pname, SqlDbType.Decimal);
            w.Precision = 6;
            w.Scale = 2;
            w.Value = kg is decimal v && v > 0 ? (object)v : DBNull.Value;   // 0 / empty = no limit
        }
        cmd.Parameters.Add("@n", SqlDbType.NVarChar, 100).Value = name.Trim();
        cmd.Parameters.Add("@c", SqlDbType.NVarChar, 50).Value = category;
        cmd.Parameters.Add("@s", SqlDbType.NVarChar, 10).Value = size;
        cmd.Parameters.Add("@d", SqlDbType.NVarChar, 300).Value =
            string.IsNullOrWhiteSpace(description) ? DBNull.Value : description.Trim();
        var p = cmd.Parameters.Add("@p", SqlDbType.Decimal);
        p.Precision = 10;
        p.Scale = 2;
        p.Value = price;
        cmd.Parameters.Add("@m", SqlDbType.Int).Value = minutes;
    }
}