using System.Data;
using Microsoft.Data.SqlClient;

namespace MyApp;

/// <summary>Everything the "View Pets" window shows about one pet. Photo and VaccinationCard are image bytes (JPEG), or null.</summary>
internal sealed record PetProfile(
    int Id, int CustomerId, string Name, string Species, string Breed,
    string Gender, DateTime? BirthDate, decimal? WeightKg, string CoatColor, string Notes,
    string OwnerName, string OwnerPhone, byte[]? Photo, byte[]? VaccinationCard);

/// <summary>Database calls for pet pictures, vaccination cards and extra details (see Sql/pet_profile.sql).</summary>
internal static class PetProfiles
{
    private static bool _columnsChecked;

    private static SqlConnection Open()
    {
        var conn = new SqlConnection(Database.ConnectionString);
        conn.Open();
        EnsureColumns(conn);
        return conn;
    }

    /// <summary>Adds the new Pets columns the first time they are needed, so no manual SQL step is required.</summary>
    internal static void EnsureColumns(SqlConnection conn)
    {
        if (_columnsChecked) return;
        using var cmd = new SqlCommand(@"
IF COL_LENGTH('dbo.Pets', 'PhotoImage')      IS NULL ALTER TABLE dbo.Pets ADD PhotoImage      VARBINARY(MAX) NULL;
IF COL_LENGTH('dbo.Pets', 'VaccinationCard') IS NULL ALTER TABLE dbo.Pets ADD VaccinationCard VARBINARY(MAX) NULL;
IF COL_LENGTH('dbo.Pets', 'Gender')          IS NULL ALTER TABLE dbo.Pets ADD Gender          NVARCHAR(10)   NULL;
IF COL_LENGTH('dbo.Pets', 'BirthDate')       IS NULL ALTER TABLE dbo.Pets ADD BirthDate       DATE           NULL;
IF COL_LENGTH('dbo.Pets', 'WeightKg')        IS NULL ALTER TABLE dbo.Pets ADD WeightKg        DECIMAL(5,2)   NULL;
IF COL_LENGTH('dbo.Pets', 'CoatColor')       IS NULL ALTER TABLE dbo.Pets ADD CoatColor       NVARCHAR(40)   NULL;
IF COL_LENGTH('dbo.Pets', 'Notes')           IS NULL ALTER TABLE dbo.Pets ADD Notes           NVARCHAR(500)  NULL;
IF COL_LENGTH('dbo.Pets', 'VaccineRevalidate') IS NULL ALTER TABLE dbo.Pets ADD VaccineRevalidate BIT NOT NULL CONSTRAINT DF_Pets_VaccineRevalidate DEFAULT 0;", conn);
        cmd.ExecuteNonQuery();
        _columnsChecked = true;
    }

    /// <summary>All active pets of one customer, with their pictures and details.</summary>
    public static List<PetProfile> GetForCustomer(int customerId)
    {
        const string sql = @"
SELECT p.PetId, p.CustomerId, p.Name, p.Species, p.Breed,
       ISNULL(p.Gender, N''), p.BirthDate, p.WeightKg, ISNULL(p.CoatColor, N''), ISNULL(p.Notes, N''),
       c.FullName, c.Phone, p.PhotoImage, p.VaccinationCard
FROM dbo.Pets p JOIN dbo.Customers c ON c.CustomerId = p.CustomerId
WHERE p.CustomerId = @c AND p.IsActive = 1
ORDER BY p.Name, p.PetId";

        using var conn = Open();
        using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.Add("@c", SqlDbType.Int).Value = customerId;
        using var r = cmd.ExecuteReader();

        var list = new List<PetProfile>();
        while (r.Read())
        {
            list.Add(new PetProfile(
                r.GetInt32(0), r.GetInt32(1), r.GetString(2), r.GetString(3), r.GetString(4),
                r.GetString(5),
                r.IsDBNull(6) ? null : r.GetDateTime(6),
                r.IsDBNull(7) ? null : r.GetDecimal(7),
                r.GetString(8), r.GetString(9),
                r.GetString(10), r.GetString(11),
                r.IsDBNull(12) ? null : (byte[])r.GetValue(12),
                r.IsDBNull(13) ? null : (byte[])r.GetValue(13)));
        }
        return list;
    }

    /// <summary>Just the profile picture and vaccination card of one pet (either can be null).</summary>
    public static (byte[]? Photo, byte[]? Card) GetPictures(int petId)
    {
        using var conn = Open();
        using var cmd = new SqlCommand("SELECT PhotoImage, VaccinationCard FROM dbo.Pets WHERE PetId = @id", conn);
        cmd.Parameters.Add("@id", SqlDbType.Int).Value = petId;
        using var r = cmd.ExecuteReader();
        if (!r.Read()) return (null, null);
        return (r.IsDBNull(0) ? null : (byte[])r.GetValue(0), r.IsDBNull(1) ? null : (byte[])r.GetValue(1));
    }

    /// <summary>
    /// Two cheap flags about a pet's vaccine record, without loading the card picture: whether a card is on file, and whether the pet's
    /// details were edited after its vaccine was accepted so that the card has to be validated again (see <see cref="SetNeedsRevalidation"/>).
    /// </summary>
    public static (bool HasCard, bool Revalidate) GetVaccineFlags(int petId)
    {
        using var conn = Open();
        using var cmd = new SqlCommand(
            "SELECT CASE WHEN DATALENGTH(VaccinationCard) > 0 THEN 1 ELSE 0 END, VaccineRevalidate FROM dbo.Pets WHERE PetId = @id", conn);
        cmd.Parameters.Add("@id", SqlDbType.Int).Value = petId;
        using var r = cmd.ExecuteReader();
        if (!r.Read()) return (false, false);
        return (r.GetInt32(0) == 1, r.GetBoolean(1));
    }

    /// <summary>
    /// Marks the pet as needing its vaccine card validated again (true), or clears the mark (false). It is set when a customer edits a pet
    /// whose vaccine was already accepted, and cleared when a card is scanned and accepted (PetVaccinations.SaveScan).
    /// </summary>
    public static void SetNeedsRevalidation(int petId, bool value)
    {
        using var conn = Open();
        using var cmd = new SqlCommand("UPDATE dbo.Pets SET VaccineRevalidate = @v WHERE PetId = @id AND IsActive = 1", conn);
        cmd.Parameters.Add("@v", SqlDbType.Bit).Value = value;
        cmd.Parameters.Add("@id", SqlDbType.Int).Value = petId;
        if (cmd.ExecuteNonQuery() == 0)
            throw new InvalidOperationException("That pet no longer exists.");
    }

    public static void SavePhoto(int petId, byte[]? image) => SaveImage(petId, "PhotoImage", image);

    public static void SaveVaccinationCard(int petId, byte[]? image) => SaveImage(petId, "VaccinationCard", image);

    // The column name only ever comes from the two callers above, never from user input.
    private static void SaveImage(int petId, string column, byte[]? image)
    {
        using var conn = Open();
        using var cmd = new SqlCommand($"UPDATE dbo.Pets SET {column} = @img WHERE PetId = @id AND IsActive = 1", conn);
        cmd.Parameters.Add("@img", SqlDbType.VarBinary, -1).Value = (object?)image ?? DBNull.Value;
        cmd.Parameters.Add("@id", SqlDbType.Int).Value = petId;
        if (cmd.ExecuteNonQuery() == 0)
            throw new InvalidOperationException("That pet no longer exists.");
    }

    public static void SaveDetails(int petId, string gender, DateTime? birthDate, decimal? weightKg, string coatColor, string notes)
    {
        using var conn = Open();
        using var cmd = new SqlCommand(@"
UPDATE dbo.Pets
SET Gender = @g, BirthDate = @b, WeightKg = @w, CoatColor = @c, Notes = @n
WHERE PetId = @id AND IsActive = 1", conn);
        cmd.Parameters.Add("@g", SqlDbType.NVarChar, 10).Value = gender.Trim();
        cmd.Parameters.Add("@b", SqlDbType.Date).Value = (object?)birthDate ?? DBNull.Value;
        var w = cmd.Parameters.Add("@w", SqlDbType.Decimal);
        w.Precision = 5;
        w.Scale = 2;
        w.Value = (object?)weightKg ?? DBNull.Value;
        cmd.Parameters.Add("@c", SqlDbType.NVarChar, 40).Value = coatColor.Trim();
        cmd.Parameters.Add("@n", SqlDbType.NVarChar, 500).Value = notes.Trim();
        cmd.Parameters.Add("@id", SqlDbType.Int).Value = petId;
        if (cmd.ExecuteNonQuery() == 0)
            throw new InvalidOperationException("That pet no longer exists.");
    }
}
