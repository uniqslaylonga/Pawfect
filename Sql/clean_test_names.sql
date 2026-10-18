-- Removes the "TEST" label from the sample data so it looks like real data (e.g. "TEST Ear Cleaning" -> "Ear Cleaning").
-- It RENAMES the rows instead of deleting them, so your appointments, sales and receipts stay intact.
-- Run this once in SSMS (select the Pawfect database first). Safe to run again.
-- If you would rather delete the test rows completely, use Sql/remove_test_data.sql instead.

USE Pawfect;
GO

-- Services: "TEST Ear Cleaning" -> "Ear Cleaning" (skipped if a service with that name and size already exists)
UPDATE s SET Name = LTRIM(SUBSTRING(s.Name, 5, 200))
FROM dbo.Services s
WHERE s.Name LIKE N'TEST %'
  AND NOT EXISTS (SELECT 1 FROM dbo.Services x WHERE x.Name = LTRIM(SUBSTRING(s.Name, 5, 200)) AND ISNULL(x.Size, N'') = ISNULL(s.Size, N''));

-- Products: names and codes ("TEST-P0001" -> "P0001", "TEST-001" -> "001")
UPDATE p SET Name = LTRIM(SUBSTRING(p.Name, 5, 200))
FROM dbo.Products p
WHERE p.Name LIKE N'TEST %'
  AND NOT EXISTS (SELECT 1 FROM dbo.Products x WHERE x.Name = LTRIM(SUBSTRING(p.Name, 5, 200)));

UPDATE p SET Code = SUBSTRING(p.Code, 6, 50)
FROM dbo.Products p
WHERE p.Code LIKE N'TEST-%'
  AND NOT EXISTS (SELECT 1 FROM dbo.Products x WHERE x.Code = SUBSTRING(p.Code, 6, 50));

-- Customers and pets: "TEST Ana Reyes" -> "Ana Reyes", "TEST Pet" -> "Pet"
UPDATE dbo.Customers SET FullName = LTRIM(SUBSTRING(FullName, 5, 200)) WHERE FullName LIKE N'TEST %';
UPDATE dbo.Pets      SET Name     = LTRIM(SUBSTRING(Name, 5, 200))     WHERE Name LIKE N'TEST %';
GO

-- Anything still starting with TEST (rename these by hand, or delete them):
SELECT N'Service' AS What, Name FROM dbo.Services  WHERE Name LIKE N'TEST%'
UNION ALL SELECT N'Product',  Name FROM dbo.Products  WHERE Name LIKE N'TEST%'
UNION ALL SELECT N'Customer', FullName FROM dbo.Customers WHERE FullName LIKE N'TEST%'
UNION ALL SELECT N'Pet',      Name FROM dbo.Pets      WHERE Name LIKE N'TEST%';
GO
