-- Vaccination records read from a pet's vaccination card (customer portal > Scan Vaccination Card).
-- The app creates this table by itself the first time it is needed; this script is only here so the schema is on file.
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
);
