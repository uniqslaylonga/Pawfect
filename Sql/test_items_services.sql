-- OPTIONAL. Only for trying Counter POS & Billing. Run employee_setup.sql first.
-- Adds several test retail products and services so every part of the screen can be tried
-- (categories, search, qty, discount, cash / e-wallet).
-- Every name starts with "TEST" so Sql/remove_test_data.sql deletes them all again.
-- Safe to run more than once.

USE Pawfect;
GO

-- ---------------------------------------------------------------- services
-- Category must be one of: Grooming | Health & Wellness | Add-ons | Others
;WITH s (Name, Category, Price) AS (
    SELECT N'TEST Full Groom',          N'Grooming',          800  UNION ALL
    SELECT N'TEST Bath & Blow Dry',     N'Grooming',          500  UNION ALL
    SELECT N'TEST Haircut',             N'Grooming',          450  UNION ALL
    SELECT N'TEST Nail Trim',           N'Add-ons',           150  UNION ALL
    SELECT N'TEST Ear Cleaning',        N'Add-ons',           120  UNION ALL
    SELECT N'TEST Teeth Cleaning',      N'Add-ons',           250  UNION ALL
    SELECT N'TEST Wellness Check',      N'Health & Wellness', 600  UNION ALL
    SELECT N'TEST Flea & Tick Treatment', N'Health & Wellness', 350
)
INSERT INTO dbo.Services (Name, Category, Price)
SELECT s.Name, s.Category, s.Price
FROM s
WHERE NOT EXISTS (SELECT 1 FROM dbo.Services x WHERE x.Name = s.Name);
GO

-- ---------------------------------------------------------------- retail products
;WITH p (Code, Name, Category, Price) AS (
    SELECT N'TEST-P0001', N'TEST Pet Shampoo 250ml', N'Grooming Supplies', 250  UNION ALL
    SELECT N'TEST-P0002', N'TEST Dog Treats 200g',   N'Treats',            120  UNION ALL
    SELECT N'TEST-P0003', N'TEST Nail Clipper',      N'Grooming Supplies', 150  UNION ALL
    SELECT N'TEST-P0004', N'TEST Cat Food 1kg',      N'Pet Food',          380  UNION ALL
    SELECT N'TEST-P0005', N'TEST Dog Food 5kg',      N'Pet Food',          1250 UNION ALL
    SELECT N'TEST-P0006', N'TEST Cat Treats 100g',   N'Treats',            95   UNION ALL
    SELECT N'TEST-P0007', N'TEST Pet Brush',         N'Grooming Supplies', 220  UNION ALL
    SELECT N'TEST-P0008', N'TEST Dog Collar',        N'Accessories',       180  UNION ALL
    SELECT N'TEST-P0009', N'TEST Leash 1.5m',        N'Accessories',       260  UNION ALL
    SELECT N'TEST-P0010', N'TEST Chew Toy',          N'Accessories',       140
)
INSERT INTO dbo.Products (Code, Name, Category, Price)
SELECT p.Code, p.Name, p.Category, p.Price
FROM p
WHERE NOT EXISTS (SELECT 1 FROM dbo.Products x WHERE x.Code = p.Code);
GO

SELECT N'Service' AS What, Name, Category, Price FROM dbo.Services WHERE Name LIKE N'TEST%'
UNION ALL
SELECT N'Product', Name, Category, Price FROM dbo.Products WHERE Name LIKE N'TEST%';
GO
