-- Weight-based service prices (from Pawfect_norbs.xlsx > Service Menu).
-- One service NAME per treatment (no weight in the name); the price depends on the pet's weight and species.
-- Safe to run more than once: it adds missing rows and refreshes price / duration of existing ones.
-- Edit a price here (or in the Service Catalog) and run it again; weights are in kg, a pet matches a band when
--   MinKg < weight <= MaxKg   (MinKg NULL / MaxKg NULL = no lower / upper limit).

USE Pawfect;
GO

-- ------------------------------------------------------------ 1. schema (the app also does this by itself on start-up)
IF COL_LENGTH('dbo.Services', 'Species') IS NULL
    ALTER TABLE dbo.Services ADD Species NVARCHAR(10) NOT NULL CONSTRAINT DF_Services_Species DEFAULT N'Both';
GO
IF COL_LENGTH('dbo.Services', 'MinKg') IS NULL ALTER TABLE dbo.Services ADD MinKg DECIMAL(6,2) NULL;
GO
IF COL_LENGTH('dbo.Services', 'MaxKg') IS NULL ALTER TABLE dbo.Services ADD MaxKg DECIMAL(6,2) NULL;
GO
IF EXISTS (SELECT 1 FROM sys.check_constraints WHERE name = N'CK_Services_Size' AND parent_object_id = OBJECT_ID(N'dbo.Services')
           AND definition NOT LIKE N'%XL%')
BEGIN
    ALTER TABLE dbo.Services DROP CONSTRAINT CK_Services_Size;
    ALTER TABLE dbo.Services ADD CONSTRAINT CK_Services_Size
        CHECK (Size IN (N'XS', N'Small', N'Medium', N'Large', N'XL', N'Giant', N'All'));
END
GO
IF EXISTS (SELECT 1 FROM sys.indexes i WHERE i.name = N'UQ_Services_Name_Size' AND i.object_id = OBJECT_ID(N'dbo.Services')
           AND NOT EXISTS (SELECT 1 FROM sys.index_columns ic JOIN sys.columns c ON c.object_id = ic.object_id AND c.column_id = ic.column_id
                           WHERE ic.object_id = i.object_id AND ic.index_id = i.index_id AND c.name = N'Species'))
BEGIN
    ALTER TABLE dbo.Services DROP CONSTRAINT UQ_Services_Name_Size;
    ALTER TABLE dbo.Services ADD CONSTRAINT UQ_Services_Name_Size UNIQUE (Name, Species, Size);
END
GO

-- ------------------------------------------------------------ 2. the services and their weight bands
DECLARE @src TABLE (Name NVARCHAR(100), Category NVARCHAR(50), Species NVARCHAR(10), Size NVARCHAR(10),
                    MinKg DECIMAL(6,2), MaxKg DECIMAL(6,2), Description NVARCHAR(300), Price DECIMAL(10,2), Minutes INT);
INSERT INTO @src VALUES
    (N'Basic Bath & Blow Dry', N'Grooming', N'Dog', N'XS', 0.0, 5.0, N'Bath, blow dry, brushing, ear exterior wipe', 350, 45),
    (N'Full Groom', N'Grooming', N'Dog', N'XS', 0.0, 5.0, N'Bath, blow dry, haircut/trim, brush, nail trim, ear exterior clean', 550, 90),
    (N'De-shedding Bath Package', N'Grooming', N'Dog', N'XS', 0.0, 5.0, N'Bath, blow dry, brushing/de-shedding', 850, 90),
    (N'Bath + Nail Trim', N'Grooming', N'Dog', N'XS', 0.0, 5.0, N'Bath, blow dry, nail trim', 450, 60),
    (N'Nail Trim Only', N'Grooming', N'Dog', N'XS', 0.0, 5.0, N'Nail trim', 150, 15),
    (N'Basic Bath & Blow Dry', N'Grooming', N'Dog', N'Small', 5.0, 10.0, N'Bath, blow dry, brushing, ear exterior wipe', 450, 45),
    (N'Full Groom', N'Grooming', N'Dog', N'Small', 5.0, 10.0, N'Bath, blow dry, haircut/trim, brush, nail trim, ear exterior clean', 700, 90),
    (N'De-shedding Bath Package', N'Grooming', N'Dog', N'Small', 5.0, 10.0, N'Bath, blow dry, brushing/de-shedding', 1050, 90),
    (N'Bath + Nail Trim', N'Grooming', N'Dog', N'Small', 5.0, 10.0, N'Bath, blow dry, nail trim', 550, 60),
    (N'Nail Trim Only', N'Grooming', N'Dog', N'Small', 5.0, 10.0, N'Nail trim', 150, 15),
    (N'Basic Bath & Blow Dry', N'Grooming', N'Dog', N'Medium', 10.0, 20.0, N'Bath, blow dry, brushing, ear exterior wipe', 600, 45),
    (N'Full Groom', N'Grooming', N'Dog', N'Medium', 10.0, 20.0, N'Bath, blow dry, haircut/trim, brush, nail trim, ear exterior clean', 900, 90),
    (N'De-shedding Bath Package', N'Grooming', N'Dog', N'Medium', 10.0, 20.0, N'Bath, blow dry, brushing/de-shedding', 1350, 90),
    (N'Bath + Nail Trim', N'Grooming', N'Dog', N'Medium', 10.0, 20.0, N'Bath, blow dry, nail trim', 700, 60),
    (N'Nail Trim Only', N'Grooming', N'Dog', N'Medium', 10.0, 20.0, N'Nail trim', 200, 15),
    (N'Basic Bath & Blow Dry', N'Grooming', N'Dog', N'Large', 20.0, 30.0, N'Bath, blow dry, brushing, ear exterior wipe', 800, 60),
    (N'Full Groom', N'Grooming', N'Dog', N'Large', 20.0, 30.0, N'Bath, blow dry, haircut/trim, brush, nail trim, ear exterior clean', 1200, 105),
    (N'De-shedding Bath Package', N'Grooming', N'Dog', N'Large', 20.0, 30.0, N'Bath, blow dry, brushing/de-shedding', 1700, 105),
    (N'Bath + Nail Trim', N'Grooming', N'Dog', N'Large', 20.0, 30.0, N'Bath, blow dry, nail trim', 900, 75),
    (N'Nail Trim Only', N'Grooming', N'Dog', N'Large', 20.0, 30.0, N'Nail trim', 200, 15),
    (N'Basic Bath & Blow Dry', N'Grooming', N'Dog', N'XL', 30.0, NULL, N'Bath, blow dry, brushing, ear exterior wipe', 1000, 75),
    (N'Full Groom', N'Grooming', N'Dog', N'XL', 30.0, NULL, N'Bath, blow dry, haircut/trim, brush, nail trim, ear exterior clean', 1500, 120),
    (N'De-shedding Bath Package', N'Grooming', N'Dog', N'XL', 30.0, NULL, N'Bath, blow dry, brushing/de-shedding', 2200, 120),
    (N'Bath + Nail Trim', N'Grooming', N'Dog', N'XL', 30.0, NULL, N'Bath, blow dry, nail trim', 1100, 90),
    (N'Nail Trim Only', N'Grooming', N'Dog', N'XL', 30.0, NULL, N'Nail trim', 200, 15),
    (N'Cat Bath & Blow Dry', N'Grooming', N'Cat', N'Small', 0.0, 4.0, N'Cat-appropriate bath, controlled drying, brushing', 450, 45),
    (N'Cat Full Groom / Sanitary Trim', N'Grooming', N'Cat', N'Small', 0.0, 4.0, N'Brushing, sanitary trim as tolerated, nail trim', 800, 90),
    (N'Brush-Out & Nail Trim', N'Grooming', N'Cat', N'Small', 0.0, 4.0, N'Coat brushing and nail trim', 650, 45),
    (N'Nail Trim Only', N'Grooming', N'Cat', N'Small', 0.0, 4.0, N'Nail trim', 150, 15),
    (N'Sanitary Trim Only', N'Grooming', N'Cat', N'Small', 0.0, 4.0, N'Sanitary area trim where safely tolerated', 250, 20),
    (N'Cat Bath & Blow Dry', N'Grooming', N'Cat', N'Medium', 4.0, 6.0, N'Cat-appropriate bath, controlled drying, brushing', 550, 45),
    (N'Cat Full Groom / Sanitary Trim', N'Grooming', N'Cat', N'Medium', 4.0, 6.0, N'Brushing, sanitary trim as tolerated, nail trim', 950, 90),
    (N'Brush-Out & Nail Trim', N'Grooming', N'Cat', N'Medium', 4.0, 6.0, N'Coat brushing and nail trim', 750, 45),
    (N'Nail Trim Only', N'Grooming', N'Cat', N'Medium', 4.0, 6.0, N'Nail trim', 150, 15),
    (N'Sanitary Trim Only', N'Grooming', N'Cat', N'Medium', 4.0, 6.0, N'Sanitary area trim where safely tolerated', 250, 20),
    (N'Cat Bath & Blow Dry', N'Grooming', N'Cat', N'Large', 6.0, NULL, N'Cat-appropriate bath, controlled drying, brushing', 650, 60),
    (N'Cat Full Groom / Sanitary Trim', N'Grooming', N'Cat', N'Large', 6.0, NULL, N'Brushing, sanitary trim as tolerated, nail trim', 1100, 105),
    (N'Brush-Out & Nail Trim', N'Grooming', N'Cat', N'Large', 6.0, NULL, N'Coat brushing and nail trim', 850, 60),
    (N'Nail Trim Only', N'Grooming', N'Cat', N'Large', 6.0, NULL, N'Nail trim', 150, 15),
    (N'Sanitary Trim Only', N'Grooming', N'Cat', N'Large', 6.0, NULL, N'Sanitary area trim where safely tolerated', 250, 20),
    (N'Ear Exterior Cleaning', N'Add-ons', N'Both', N'All', NULL, NULL, N'External ear cleaning only', 100, 15),
    (N'Paw Pad Trim', N'Add-ons', N'Both', N'All', NULL, NULL, N'Trim excess hair around paw pads', 150, 15),
    (N'Face Trim', N'Add-ons', N'Both', N'All', NULL, NULL, N'Face/eye-area coat trim', 200, 15),
    (N'Teeth Brushing', N'Add-ons', N'Both', N'All', NULL, NULL, N'Pet toothpaste and brush', 150, 15),
    (N'De-matting Assessment', N'Add-ons', N'Both', N'All', NULL, NULL, N'Assess coat condition and safe options', 0, 15),
    (N'Flea/Tick Visual Check', N'Add-ons', N'Both', N'All', NULL, NULL, N'Visual check and inform owner', 0, 15),
    (N'Special Handling / Extra Time', N'Add-ons', N'Both', N'All', NULL, NULL, N'Additional handling time if approved', 200, 15);

-- Older rows with the same name and no weight band would show up next to the new ones: hide them (kept for old appointments).
UPDATE s SET IsActive = 0
FROM dbo.Services s
WHERE s.MinKg IS NULL AND s.MaxKg IS NULL AND s.Species = N'Both'
  AND EXISTS (SELECT 1 FROM @src x WHERE x.Name = s.Name AND x.MinKg IS NOT NULL);

UPDATE s SET Category = x.Category, MinKg = x.MinKg, MaxKg = x.MaxKg, Description = x.Description,
             Price = x.Price, DurationMinutes = x.Minutes, IsActive = 1
FROM dbo.Services s
JOIN @src x ON x.Name = s.Name AND x.Species = s.Species AND x.Size = s.Size;

INSERT INTO dbo.Services (Name, Category, Species, Size, MinKg, MaxKg, Description, Price, DurationMinutes)
SELECT x.Name, x.Category, x.Species, x.Size, x.MinKg, x.MaxKg, x.Description, x.Price, x.Minutes
FROM @src x
WHERE NOT EXISTS (SELECT 1 FROM dbo.Services s WHERE s.Name = x.Name AND s.Species = x.Species AND s.Size = x.Size);
GO

SELECT Name, Species, Size, MinKg, MaxKg, Price, DurationMinutes
FROM dbo.Services WHERE Species <> N'Both' OR MinKg IS NOT NULL OR Category = N'Add-ons'
ORDER BY Species, Name, ISNULL(MinKg, 0);
GO
