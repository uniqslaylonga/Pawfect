using System.Data;
using Microsoft.Data.SqlClient;

namespace MyApp;

/// <summary>One saved vaccination of a pet.</summary>
internal sealed record PetVaccinationRecord(int Id, string Vaccine, DateTime? Given, decimal? WeightKg, string BatchNo, DateTime? Expires);

/// <summary>Saves what the vaccination-card scan found (see Sql/pet_vaccinations.sql). The table is created the first time it is needed.</summary>
internal static class PetVaccinations
{
    private static bool _tableChecked;

    private const string CreateTableSql = @"
IF OBJECT_ID('dbo.PetVaccinations', 'U') IS NULL
CREATE TABLE dbo.PetVaccinations (
    VaccinationId INT IDENTITY(1,1) PRIMARY KEY,
    PetId         INT           NOT NULL REFERENCES dbo.Pets(PetId),
    VaccineType   NVARCHAR(80)  NOT NULL,
    DateGiven     DATE          NULL,
    WeightKg      DECIMAL(5,2)  NULL,
    BatchNo       NVARCHAR(40)  NULL,
    ExpiryDate    DATE          NULL,
    CreatedAt     DATETIME2     NOT NULL DEFAULT SYSDATETIME()
);";

    private static SqlConnection Open()
    {
        var conn = new SqlConnection(Database.ConnectionString);
        conn.Open();
        PetProfiles.EnsureColumns(conn);
        if (!_tableChecked)
        {
            using var cmd = new SqlCommand(CreateTableSql, conn);
            cmd.ExecuteNonQuery();
            _tableChecked = true;
        }
        return conn;
    }

    /// <summary>
    /// Adds the pet (or updates <paramref name="existingPetId"/>, which must belong to the customer), stores the card picture, the pet's photo
    /// (cut out of the card) and the details, and adds the vaccination rows (rows already saved for that pet are skipped). All or nothing.
    /// Blank details never overwrite what is already saved. Returns the pet id.
    /// </summary>
    public static int SaveScan(int customerId, int? existingPetId, string name, string species, string breed,
        string gender, DateTime? birthDate, decimal? weightKg, byte[]? cardImage, byte[]? petPhoto, IEnumerable<ScannedVaccination> rows)
    {
        using var conn = Open();
        using var tx = conn.BeginTransaction();

        int petId;
        if (existingPetId is int id)
        {
            using var upd = new SqlCommand(@"
UPDATE dbo.Pets
SET Species = CASE WHEN @s = N'' THEN Species ELSE @s END,
    Breed   = CASE WHEN @b = N'' THEN Breed   ELSE @b END
WHERE PetId = @id AND CustomerId = @c AND IsActive = 1", conn, tx);
            upd.Parameters.Add("@s", SqlDbType.NVarChar, 30).Value = species.Trim();
            upd.Parameters.Add("@b", SqlDbType.NVarChar, 60).Value = breed.Trim();
            upd.Parameters.Add("@id", SqlDbType.Int).Value = id;
            upd.Parameters.Add("@c", SqlDbType.Int).Value = customerId;
            if (upd.ExecuteNonQuery() == 0)
                throw new InvalidOperationException("That pet no longer exists.");
            petId = id;
        }
        else
        {
            using var ins = new SqlCommand(
                "INSERT INTO dbo.Pets (CustomerId, Name, Species, Breed) OUTPUT INSERTED.PetId VALUES (@c, @n, @s, @b)", conn, tx);
            ins.Parameters.Add("@c", SqlDbType.Int).Value = customerId;
            ins.Parameters.Add("@n", SqlDbType.NVarChar, 60).Value = name.Trim();
            ins.Parameters.Add("@s", SqlDbType.NVarChar, 30).Value = species.Trim();
            ins.Parameters.Add("@b", SqlDbType.NVarChar, 60).Value = breed.Trim();
            petId = (int)ins.ExecuteScalar()!;
        }

        using (var details = new SqlCommand(@"
UPDATE dbo.Pets
SET Gender          = CASE WHEN @g = N'' THEN Gender ELSE @g END,
    BirthDate       = ISNULL(@b, BirthDate),
    WeightKg        = ISNULL(@w, WeightKg),
    VaccinationCard = ISNULL(@img, VaccinationCard),
    PhotoImage      = ISNULL(@photo, PhotoImage)
WHERE PetId = @id", conn, tx))
        {
            details.Parameters.Add("@g", SqlDbType.NVarChar, 10).Value = gender.Trim();
            details.Parameters.Add("@b", SqlDbType.Date).Value = (object?)birthDate ?? DBNull.Value;
            var w = details.Parameters.Add("@w", SqlDbType.Decimal);
            w.Precision = 5;
            w.Scale = 2;
            w.Value = (object?)weightKg ?? DBNull.Value;
            details.Parameters.Add("@img", SqlDbType.VarBinary, -1).Value = (object?)cardImage ?? DBNull.Value;
            details.Parameters.Add("@photo", SqlDbType.VarBinary, -1).Value = (object?)petPhoto ?? DBNull.Value;
            details.Parameters.Add("@id", SqlDbType.Int).Value = petId;
            details.ExecuteNonQuery();
        }

        foreach (var row in rows)
        {
            if (string.IsNullOrWhiteSpace(row.Vaccine)) continue;
            using var cmd = new SqlCommand(@"
IF NOT EXISTS (SELECT 1 FROM dbo.PetVaccinations
               WHERE PetId = @p AND VaccineType = @v AND ((DateGiven = @d) OR (DateGiven IS NULL AND @d IS NULL)))
    INSERT INTO dbo.PetVaccinations (PetId, VaccineType, DateGiven, WeightKg, BatchNo, ExpiryDate)
    VALUES (@p, @v, @d, @w, @bn, @e)", conn, tx);
            cmd.Parameters.Add("@p", SqlDbType.Int).Value = petId;
            cmd.Parameters.Add("@v", SqlDbType.NVarChar, 80).Value = row.Vaccine.Trim();
            cmd.Parameters.Add("@d", SqlDbType.Date).Value = (object?)row.Given ?? DBNull.Value;
            var rw = cmd.Parameters.Add("@w", SqlDbType.Decimal);
            rw.Precision = 5;
            rw.Scale = 2;
            rw.Value = (object?)row.WeightKg ?? DBNull.Value;
            cmd.Parameters.Add("@bn", SqlDbType.NVarChar, 40).Value = row.BatchNo.Trim();
            cmd.Parameters.Add("@e", SqlDbType.Date).Value = (object?)row.Expires ?? DBNull.Value;
            cmd.ExecuteNonQuery();
        }

        tx.Commit();
        return petId;
    }

    /// <summary>The saved vaccinations of one pet, newest first.</summary>
    /// <summary>
    /// True when any vaccine on the card has lapsed. Each vaccine is judged by its newest record, so an old shot that was
    /// already renewed doesn't count. A vaccine whose expiry date couldn't be read can't be shown to be current, so it counts as lapsed.
    /// A card with no readable rows at all says nothing about expiry and is not treated as lapsed.
    /// </summary>
    public static bool AnyLapsed(IReadOnlyList<PetVaccinationRecord> rows)
    {
        var today = DateTime.Today;
        return rows
            .GroupBy(r => (r.Vaccine ?? "").Trim(), StringComparer.OrdinalIgnoreCase)
            .Any(g => !g.Any(r => r.Expires is { } e && e.Date >= today));
    }

    public static List<PetVaccinationRecord> GetForPet(int petId)
    {
        using var conn = Open();
        using var cmd = new SqlCommand(@"
SELECT VaccinationId, VaccineType, DateGiven, WeightKg, ISNULL(BatchNo, N''), ExpiryDate
FROM dbo.PetVaccinations WHERE PetId = @p
ORDER BY DateGiven DESC, VaccinationId DESC", conn);
        cmd.Parameters.Add("@p", SqlDbType.Int).Value = petId;
        using var r = cmd.ExecuteReader();

        var list = new List<PetVaccinationRecord>();
        while (r.Read())
            list.Add(new PetVaccinationRecord(r.GetInt32(0), r.GetString(1),
                r.IsDBNull(2) ? null : r.GetDateTime(2), r.IsDBNull(3) ? null : r.GetDecimal(3),
                r.GetString(4), r.IsDBNull(5) ? null : r.GetDateTime(5)));
        return list;
    }
}
