using System.Data;
using System.Globalization;
using Microsoft.Data.SqlClient;

namespace MyApp;

/// <summary>
/// Weight-based service prices. A service has ONE name ("Full Groom") and one row per weight band in dbo.Services
/// (Species + MinKg/MaxKg + Price, see Sql/weight_based_pricing.sql). The band is chosen from the pet's species and
/// weight, so customers and staff only ever see the plain service name with the price that fits the pet.
/// A pet matches a band when MinKg &lt; weight &lt;= MaxKg (NULL = no limit); a service without bounds applies to every pet.
/// </summary>
internal static class ServicePricing
{
    private static bool _schemaChecked;

    // The WHERE part shared by every lookup below (table alias "s"; @sp = species, @w = weight in kg or NULL when unknown).
    private const string FitsPet = @"
s.Species IN (N'Both', @sp)
AND (@w IS NULL OR ((s.MinKg IS NULL OR @w > s.MinKg) AND (s.MaxKg IS NULL OR @w <= s.MaxKg)))";

    // When a service name matches several rows, prefer the one made for the pet's species, then the cheapest.
    private const string PreferSpecies = "CASE WHEN s.Species = N'Both' THEN 1 ELSE 0 END, CASE WHEN s.Size = N'All' THEN 0 ELSE 1 END, s.Price, s.ServiceId";

    // ------------------------------------------------------------------ schema

    /// <summary>
    /// Adds Species / MinKg / MaxKg to dbo.Services and widens its Size and unique rules the first time the app connects,
    /// so Sql/weight_based_pricing.sql only has to be run to load the prices. Safe to call any number of times.
    /// </summary>
    internal static void EnsureSchema(SqlConnection conn)
    {
        if (_schemaChecked) return;
        string[] steps =
        {
            @"
IF OBJECT_ID(N'dbo.Services', N'U') IS NOT NULL AND COL_LENGTH(N'dbo.Services', N'Species') IS NULL
    ALTER TABLE dbo.Services ADD Species NVARCHAR(10) NOT NULL CONSTRAINT DF_Services_Species DEFAULT N'Both';
IF OBJECT_ID(N'dbo.Services', N'U') IS NOT NULL AND COL_LENGTH(N'dbo.Services', N'MinKg') IS NULL
    ALTER TABLE dbo.Services ADD MinKg DECIMAL(6,2) NULL;
IF OBJECT_ID(N'dbo.Services', N'U') IS NOT NULL AND COL_LENGTH(N'dbo.Services', N'MaxKg') IS NULL
    ALTER TABLE dbo.Services ADD MaxKg DECIMAL(6,2) NULL;",

            @"
IF EXISTS (SELECT 1 FROM sys.check_constraints WHERE name = N'CK_Services_Size' AND parent_object_id = OBJECT_ID(N'dbo.Services')
           AND definition NOT LIKE N'%XL%')
BEGIN
    ALTER TABLE dbo.Services DROP CONSTRAINT CK_Services_Size;
    ALTER TABLE dbo.Services ADD CONSTRAINT CK_Services_Size
        CHECK (Size IN (N'XS', N'Small', N'Medium', N'Large', N'XL', N'Giant', N'All'));
END",

            @"
IF EXISTS (SELECT 1 FROM sys.indexes i WHERE i.name = N'UQ_Services_Name_Size' AND i.object_id = OBJECT_ID(N'dbo.Services')
           AND NOT EXISTS (SELECT 1 FROM sys.index_columns ic JOIN sys.columns c ON c.object_id = ic.object_id AND c.column_id = ic.column_id
                           WHERE ic.object_id = i.object_id AND ic.index_id = i.index_id AND c.name = N'Species'))
BEGIN
    ALTER TABLE dbo.Services DROP CONSTRAINT UQ_Services_Name_Size;
    ALTER TABLE dbo.Services ADD CONSTRAINT UQ_Services_Name_Size UNIQUE (Name, Species, Size);
END",
        };

        try
        {
            foreach (var sql in steps)
            {
                using var cmd = new SqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
            }
            _schemaChecked = true;
        }
        catch (SqlException)
        {
            // no permission to alter the table: the real error shows up in the query that needs the new columns
        }
    }

    // ------------------------------------------------------------------ text

    /// <summary>"Dog · up to 5 kg", "Cat · over 6 kg", "Dog · 5.1-10 kg"; empty for a service that is not priced by weight.</summary>
    internal static string BandText(string species, decimal? minKg, decimal? maxKg)
    {
        if (minKg is null && maxKg is null) return "";

        string range;
        if (maxKg is null) range = $"over {Kg(minKg ?? 0)} kg";
        else if ((minKg ?? 0) == 0) range = $"up to {Kg(maxKg.Value)} kg";
        else range = $"{Kg(minKg.Value + 0.1m)}-{Kg(maxKg.Value)} kg";

        return string.Equals(species, "Both", StringComparison.OrdinalIgnoreCase) ? range : species + " · " + range;
    }

    private static string Kg(decimal value) => value.ToString("0.##", CultureInfo.InvariantCulture);

    // ------------------------------------------------------------------ lookups for one pet

    /// <summary>The pet's species and weight (null weight = not recorded), or null when the pet doesn't exist.</summary>
    public static (string Species, decimal? WeightKg)? GetPet(int petId)
    {
        using var conn = Open();
        return ReadPet(conn, petId);
    }

    /// <summary>
    /// The cards on step 4 (Choose a Grooming Service): every active Grooming / Add-ons service once, priced for this pet's
    /// weight. A service that has no band for the pet's weight or species is left out.
    /// </summary>
    public static List<ServiceChoice> GetChoices(int petId)
    {
        using var conn = Open();
        var pet = ReadPet(conn, petId);

        string sql = $@"
SELECT ServiceId, Name, Price, DurationMinutes
FROM (
    SELECT s.ServiceId, s.Name, s.Price, s.DurationMinutes,
           ROW_NUMBER() OVER (PARTITION BY s.Name ORDER BY {PreferSpecies}) AS rn
    FROM dbo.Services s
    WHERE s.IsActive = 1 AND s.Category IN (N'Grooming', N'Add-ons') AND {FitsPet}
) x
WHERE rn = 1
ORDER BY Price, Name";

        using var cmd = new SqlCommand(sql, conn);
        AddPetParameters(cmd, pet);
        using var r = cmd.ExecuteReader();
        var list = new List<ServiceChoice>();
        while (r.Read()) list.Add(new ServiceChoice(r.GetInt32(0), r.GetString(1), r.GetDecimal(2), r.GetInt32(3)));
        return list;
    }

    /// <summary>
    /// Every active service that fits this pet, with the price for its weight (staff booking a visit). When the pet's weight
    /// isn't recorded, every band is listed and <see cref="ServiceOffering.Detail"/> says which one it is.
    /// </summary>
    public static List<ServiceOffering> GetForPet(int petId)
    {
        using var conn = Open();
        var pet = ReadPet(conn, petId);

        string sql = $@"
SELECT s.ServiceId, s.Name, s.Price, s.Species, s.MinKg, s.MaxKg
FROM dbo.Services s
WHERE s.IsActive = 1 AND {FitsPet}
ORDER BY s.Name, s.Species, ISNULL(s.MinKg, 0)";

        using var cmd = new SqlCommand(sql, conn);
        AddPetParameters(cmd, pet);
        using var r = cmd.ExecuteReader();
        var list = new List<ServiceOffering>();
        while (r.Read())
        {
            string detail = pet?.WeightKg is null
                ? BandText(r.GetString(3), r.IsDBNull(4) ? null : r.GetDecimal(4), r.IsDBNull(5) ? null : r.GetDecimal(5))
                : "";
            list.Add(new ServiceOffering(r.GetInt32(0), r.GetString(1), r.GetDecimal(2), detail));
        }
        return list;
    }

    /// <summary>The service called <paramref name="name"/> priced for this pet's species and weight, or null when there is none.</summary>
    public static ServiceOffering? Find(string? name, int petId)
    {
        if (string.IsNullOrWhiteSpace(name)) return null;

        using var conn = Open();
        var pet = ReadPet(conn, petId);

        string sql = $@"
SELECT TOP (1) s.ServiceId, s.Name, s.Price
FROM dbo.Services s
WHERE s.IsActive = 1 AND s.Name = @n AND {FitsPet}
ORDER BY {PreferSpecies}";

        using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.Add("@n", SqlDbType.NVarChar, 100).Value = name.Trim();
        AddPetParameters(cmd, pet);
        using var r = cmd.ExecuteReader();
        return r.Read() ? new ServiceOffering(r.GetInt32(0), r.GetString(1), r.GetDecimal(2)) : null;
    }

    // ------------------------------------------------------------------ helpers

    private static SqlConnection Open()
    {
        var conn = new SqlConnection(Database.ConnectionString);
        conn.Open();
        EnsureSchema(conn);
        return conn;
    }

    private static (string Species, decimal? WeightKg)? ReadPet(SqlConnection conn, int petId)
    {
        PetProfiles.EnsureColumns(conn);   // makes sure Pets.WeightKg exists before it is selected
        using var cmd = new SqlCommand("SELECT ISNULL(Species, N''), WeightKg FROM dbo.Pets WHERE PetId = @p", conn);
        cmd.Parameters.Add("@p", SqlDbType.Int).Value = petId;
        using var r = cmd.ExecuteReader();
        if (!r.Read()) return null;
        return (r.GetString(0), r.IsDBNull(1) ? null : r.GetDecimal(1));
    }

    private static void AddPetParameters(SqlCommand cmd, (string Species, decimal? WeightKg)? pet)
    {
        cmd.Parameters.Add("@sp", SqlDbType.NVarChar, 10).Value = pet?.Species ?? "";
        var w = cmd.Parameters.Add("@w", SqlDbType.Decimal);
        w.Precision = 10;
        w.Scale = 2;
        w.Value = pet?.WeightKg is decimal kg && kg > 0 ? (object)kg : DBNull.Value;
    }
}
