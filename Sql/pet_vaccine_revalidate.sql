-- Marks a pet whose details were edited after its vaccine card was accepted: the card has to be validated (scanned) again.
-- The app adds this column by itself the first time it is needed; this script is only here so the schema is on file.
-- 1 = scan the vaccine card again before booking. Cleared automatically when a new card is scanned and accepted.
IF COL_LENGTH('dbo.Pets', 'VaccineRevalidate') IS NULL
    ALTER TABLE dbo.Pets ADD VaccineRevalidate BIT NOT NULL CONSTRAINT DF_Pets_VaccineRevalidate DEFAULT 0;
