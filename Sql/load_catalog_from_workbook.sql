-- Pawfect: remove the TEST data and load the catalogue from Pawfect_norbs.xlsx
--   * Service Menu      -> dbo.Services
--   * Retail Products   -> dbo.Products      (what Counter POS & Billing sells)
--   * Salon Consumables -> dbo.SalonConsumables (new table; nothing in the app reads it yet)
-- Equipment & Tools, Price References and Start Here are NOT loaded (only the three sheets above, for now).
-- Run in SSMS with the Pawfect database selected, after setup.sql, employee_setup.sql and appointment_scheduling.sql.
-- Safe to run more than once: rows that already exist are skipped. User accounts (test_accounts.sql) are NOT touched.

USE Pawfect;
GO

-- ======================================================================== 1. REMOVE TEST DATA
-- "Test" = name starts with TEST, or the customer's e-mail ends in @pawfect.test (the clean_test_names.sql leftovers).
SET NOCOUNT ON;
BEGIN TRAN;

DECLARE @cust TABLE (Id INT PRIMARY KEY);
DECLARE @pet  TABLE (Id INT PRIMARY KEY);
DECLARE @svc  TABLE (Id INT PRIMARY KEY);
DECLARE @prod TABLE (Id INT PRIMARY KEY);
DECLARE @room TABLE (Id INT PRIMARY KEY);
DECLARE @groo TABLE (Id INT PRIMARY KEY);
DECLARE @appt TABLE (Id INT PRIMARY KEY);
DECLARE @txn  TABLE (Id INT PRIMARY KEY);

INSERT @cust SELECT CustomerId FROM dbo.Customers WHERE FullName LIKE N'TEST%' OR Email LIKE N'%@pawfect.test';
INSERT @pet  SELECT PetId FROM dbo.Pets WHERE Name LIKE N'TEST%' OR CustomerId IN (SELECT Id FROM @cust);
INSERT @svc  SELECT ServiceId FROM dbo.Services WHERE Name LIKE N'TEST%';
INSERT @prod SELECT ProductId FROM dbo.Products WHERE Name LIKE N'TEST%' OR Code LIKE N'TEST-%';
INSERT @room SELECT RoomId FROM dbo.Rooms WHERE Name LIKE N'TEST%';
INSERT @groo SELECT GroomerId FROM dbo.Groomers WHERE Name LIKE N'TEST%';
INSERT @appt SELECT AppointmentId FROM dbo.Appointments
 WHERE CustomerId IN (SELECT Id FROM @cust) OR PetId IN (SELECT Id FROM @pet) OR ServiceId IN (SELECT Id FROM @svc)
    OR RoomId IN (SELECT Id FROM @room) OR GroomerId IN (SELECT Id FROM @groo);

INSERT @txn SELECT TransactionId FROM dbo.Transactions
 WHERE CustomerId IN (SELECT Id FROM @cust) OR AppointmentId IN (SELECT Id FROM @appt)
UNION SELECT TransactionId FROM dbo.TransactionLines
 WHERE ServiceId IN (SELECT Id FROM @svc) OR ProductId IN (SELECT Id FROM @prod);

DELETE FROM dbo.TransactionLines WHERE TransactionId IN (SELECT Id FROM @txn);
DELETE FROM dbo.Transactions     WHERE TransactionId IN (SELECT Id FROM @txn);
DELETE FROM dbo.Appointments     WHERE AppointmentId IN (SELECT Id FROM @appt);
IF OBJECT_ID(N'dbo.PetVaccinations', N'U') IS NOT NULL
    DELETE FROM dbo.PetVaccinations WHERE PetId IN (SELECT Id FROM @pet);
DELETE FROM dbo.Pets      WHERE PetId IN (SELECT Id FROM @pet);
DELETE FROM dbo.Customers WHERE CustomerId IN (SELECT Id FROM @cust);
DELETE FROM dbo.Services  WHERE ServiceId IN (SELECT Id FROM @svc);
DELETE FROM dbo.Products  WHERE ProductId IN (SELECT Id FROM @prod);
DELETE FROM dbo.Rooms     WHERE RoomId IN (SELECT Id FROM @room);
DELETE FROM dbo.Groomers  WHERE GroomerId IN (SELECT Id FROM @groo);

COMMIT;
GO

-- ======================================================================== 2. SERVICE MENU (47 services)
-- Category: DOG/CAT rows -> Grooming, ADD rows -> Add-ons.   Size: XS and Small -> Small, XL -> Giant, "Any size" -> All.
-- The weight band is kept in the service name so every row stays unique (Name + Size must be unique).
-- DurationMinutes is not in the workbook, so the table default (60) applies. Price = Final Price (= Suggested Price).
;WITH s (Name, Category, Size, Description, Price) AS (
    SELECT N'Dog Basic Bath & Blow Dry (XS / up to 5 kg)', N'Grooming', N'Small', N'Bath, blow dry, brushing, ear exterior wipe | Long/dense coat or severe tangles may require surcharge', 350
    UNION ALL
    SELECT N'Dog Full Groom (XS / up to 5 kg)', N'Grooming', N'Small', N'Bath, blow dry, haircut/trim, brush, nail trim, ear exterior clean | Confirm style and coat condition', 550
    UNION ALL
    SELECT N'Dog De-shedding Bath Package (XS / up to 5 kg)', N'Grooming', N'Small', N'Bath, blow dry, brushing/de-shedding | Only suitable coat types', 850
    UNION ALL
    SELECT N'Dog Bath + Nail Trim (XS / up to 5 kg)', N'Grooming', N'Small', N'Bath, blow dry, nail trim', 450
    UNION ALL
    SELECT N'Dog Nail Trim Only (XS / up to 5 kg)', N'Grooming', N'Small', N'Nail trim | Temperament/safety may affect service acceptance', 150
    UNION ALL
    SELECT N'Dog Basic Bath & Blow Dry (Small / 5.1-10 kg)', N'Grooming', N'Small', N'Bath, blow dry, brushing, ear exterior wipe | Long/dense coat or severe tangles may require surcharge', 450
    UNION ALL
    SELECT N'Dog Full Groom (Small / 5.1-10 kg)', N'Grooming', N'Small', N'Bath, blow dry, haircut/trim, brush, nail trim, ear exterior clean | Confirm style and coat condition', 700
    UNION ALL
    SELECT N'Dog De-shedding Bath Package (Small / 5.1-10 kg)', N'Grooming', N'Small', N'Bath, blow dry, brushing/de-shedding | Only suitable coat types', 1050
    UNION ALL
    SELECT N'Dog Bath + Nail Trim (Small / 5.1-10 kg)', N'Grooming', N'Small', N'Bath, blow dry, nail trim', 550
    UNION ALL
    SELECT N'Dog Nail Trim Only (Small / 5.1-10 kg)', N'Grooming', N'Small', N'Nail trim | Temperament/safety may affect service acceptance', 150
    UNION ALL
    SELECT N'Dog Basic Bath & Blow Dry (Medium / 10.1-20 kg)', N'Grooming', N'Medium', N'Bath, blow dry, brushing, ear exterior wipe | Long/dense coat or severe tangles may require surcharge', 600
    UNION ALL
    SELECT N'Dog Full Groom (Medium / 10.1-20 kg)', N'Grooming', N'Medium', N'Bath, blow dry, haircut/trim, brush, nail trim, ear exterior clean | Confirm style and coat condition', 900
    UNION ALL
    SELECT N'Dog De-shedding Bath Package (Medium / 10.1-20 kg)', N'Grooming', N'Medium', N'Bath, blow dry, brushing/de-shedding | Only suitable coat types', 1350
    UNION ALL
    SELECT N'Dog Bath + Nail Trim (Medium / 10.1-20 kg)', N'Grooming', N'Medium', N'Bath, blow dry, nail trim', 700
    UNION ALL
    SELECT N'Dog Nail Trim Only (Medium / 10.1-20 kg)', N'Grooming', N'Medium', N'Nail trim | Temperament/safety may affect service acceptance', 200
    UNION ALL
    SELECT N'Dog Basic Bath & Blow Dry (Large / 20.1-30 kg)', N'Grooming', N'Large', N'Bath, blow dry, brushing, ear exterior wipe | Long/dense coat or severe tangles may require surcharge', 800
    UNION ALL
    SELECT N'Dog Full Groom (Large / 20.1-30 kg)', N'Grooming', N'Large', N'Bath, blow dry, haircut/trim, brush, nail trim, ear exterior clean | Confirm style and coat condition', 1200
    UNION ALL
    SELECT N'Dog De-shedding Bath Package (Large / 20.1-30 kg)', N'Grooming', N'Large', N'Bath, blow dry, brushing/de-shedding | Only suitable coat types', 1700
    UNION ALL
    SELECT N'Dog Bath + Nail Trim (Large / 20.1-30 kg)', N'Grooming', N'Large', N'Bath, blow dry, nail trim', 900
    UNION ALL
    SELECT N'Dog Nail Trim Only (Large / 20.1-30 kg)', N'Grooming', N'Large', N'Nail trim | Temperament/safety may affect service acceptance', 200
    UNION ALL
    SELECT N'Dog Basic Bath & Blow Dry (XL / over 30 kg)', N'Grooming', N'Giant', N'Bath, blow dry, brushing, ear exterior wipe | Long/dense coat or severe tangles may require surcharge', 1000
    UNION ALL
    SELECT N'Dog Full Groom (XL / over 30 kg)', N'Grooming', N'Giant', N'Bath, blow dry, haircut/trim, brush, nail trim, ear exterior clean | Confirm style and coat condition', 1500
    UNION ALL
    SELECT N'Dog De-shedding Bath Package (XL / over 30 kg)', N'Grooming', N'Giant', N'Bath, blow dry, brushing/de-shedding | Only suitable coat types', 2200
    UNION ALL
    SELECT N'Dog Bath + Nail Trim (XL / over 30 kg)', N'Grooming', N'Giant', N'Bath, blow dry, nail trim', 1100
    UNION ALL
    SELECT N'Dog Nail Trim Only (XL / over 30 kg)', N'Grooming', N'Giant', N'Nail trim | Temperament/safety may affect service acceptance', 200
    UNION ALL
    SELECT N'Cat Bath & Blow Dry (Small / up to 4 kg)', N'Grooming', N'Small', N'Cat-appropriate bath, controlled drying, brushing | Only if cat can be handled safely', 450
    UNION ALL
    SELECT N'Cat Full Groom / Sanitary Trim (Small / up to 4 kg)', N'Grooming', N'Small', N'Brushing, sanitary trim as tolerated, nail trim | Lion cut or extensive mat removal should be assessed first', 800
    UNION ALL
    SELECT N'Cat Brush-Out & Nail Trim (Small / up to 4 kg)', N'Grooming', N'Small', N'Coat brushing and nail trim', 650
    UNION ALL
    SELECT N'Cat Nail Trim Only (Small / up to 4 kg)', N'Grooming', N'Small', N'Nail trim', 150
    UNION ALL
    SELECT N'Cat Sanitary Trim Only (Small / up to 4 kg)', N'Grooming', N'Small', N'Sanitary area trim where safely tolerated', 250
    UNION ALL
    SELECT N'Cat Bath & Blow Dry (Medium / 4.1-6 kg)', N'Grooming', N'Medium', N'Cat-appropriate bath, controlled drying, brushing | Only if cat can be handled safely', 550
    UNION ALL
    SELECT N'Cat Full Groom / Sanitary Trim (Medium / 4.1-6 kg)', N'Grooming', N'Medium', N'Brushing, sanitary trim as tolerated, nail trim | Lion cut or extensive mat removal should be assessed first', 950
    UNION ALL
    SELECT N'Cat Brush-Out & Nail Trim (Medium / 4.1-6 kg)', N'Grooming', N'Medium', N'Coat brushing and nail trim', 750
    UNION ALL
    SELECT N'Cat Nail Trim Only (Medium / 4.1-6 kg)', N'Grooming', N'Medium', N'Nail trim', 150
    UNION ALL
    SELECT N'Cat Sanitary Trim Only (Medium / 4.1-6 kg)', N'Grooming', N'Medium', N'Sanitary area trim where safely tolerated', 250
    UNION ALL
    SELECT N'Cat Bath & Blow Dry (Large / over 6 kg)', N'Grooming', N'Large', N'Cat-appropriate bath, controlled drying, brushing | Only if cat can be handled safely', 650
    UNION ALL
    SELECT N'Cat Full Groom / Sanitary Trim (Large / over 6 kg)', N'Grooming', N'Large', N'Brushing, sanitary trim as tolerated, nail trim | Lion cut or extensive mat removal should be assessed first', 1100
    UNION ALL
    SELECT N'Cat Brush-Out & Nail Trim (Large / over 6 kg)', N'Grooming', N'Large', N'Coat brushing and nail trim', 850
    UNION ALL
    SELECT N'Cat Nail Trim Only (Large / over 6 kg)', N'Grooming', N'Large', N'Nail trim', 150
    UNION ALL
    SELECT N'Cat Sanitary Trim Only (Large / over 6 kg)', N'Grooming', N'Large', N'Sanitary area trim where safely tolerated', 250
    UNION ALL
    SELECT N'Ear Exterior Cleaning', N'Add-ons', N'All', N'External ear cleaning only | No diagnosis or treatment of ear disease', 100
    UNION ALL
    SELECT N'Paw Pad Trim', N'Add-ons', N'All', N'Trim excess hair around paw pads | If safe and appropriate', 150
    UNION ALL
    SELECT N'Face Trim', N'Add-ons', N'All', N'Face/eye-area coat trim | Only if safe; use suitable tools', 200
    UNION ALL
    SELECT N'Teeth Brushing', N'Add-ons', N'All', N'Pet toothpaste and brush | Cosmetic hygiene only; not dental treatment', 150
    UNION ALL
    SELECT N'De-matting Assessment', N'Add-ons', N'All', N'Assess coat condition and safe options | Do not promise full dematting; may require extra fee or referral', 0
    UNION ALL
    SELECT N'Flea/Tick Visual Check', N'Add-ons', N'All', N'Visual check and inform owner | No medical diagnosis or medication application included', 0
    UNION ALL
    SELECT N'Special Handling / Extra Time', N'Add-ons', N'All', N'Additional handling time if approved | Only when safe; may refuse unsafe grooming', 200
)
INSERT INTO dbo.Services (Name, Category, Size, Description, Price)
SELECT s.Name, s.Category, s.Size, s.Description, s.Price
FROM s
WHERE NOT EXISTS (SELECT 1 FROM dbo.Services x WHERE x.Name = s.Name AND x.Size = s.Size);
GO

-- ======================================================================== 3. RETAIL PRODUCTS (50 products)
-- Code = SKU. Price = Actual Selling Price if filled in, otherwise the Suggested Selling Price (cost + markup).
-- Category is mapped to the Inventory page filter: foods -> Pet Food, treats -> Treats, Grooming Retail -> Grooming,
-- Flea & Tick -> Health, Cat Litter and Salon / Cleaning -> Supplies, Accessories -> Accessories.
;WITH p (Code, Name, Category, Price) AS (
    SELECT N'FO-CAT-001', N'Toei Cat Dry Food - Salmon (1 kg repacked)', N'Pet Food', 126
    UNION ALL
    SELECT N'FO-CAT-002', N'Vitality Catcare Dry Cat Food (1 kg)', N'Pet Food', 300
    UNION ALL
    SELECT N'FO-CAT-003', N'Maxime Dry Cat Food (1 kg repacked)', N'Pet Food', 188
    UNION ALL
    SELECT N'FO-CAT-004', N'Aozi Adult Cat Dry Food (1 kg)', N'Pet Food', 210
    UNION ALL
    SELECT N'FO-CAT-005', N'Whiskas Adult Cat Dry Food (1 kg)', N'Pet Food', 200
    UNION ALL
    SELECT N'FO-CAT-006', N'Whiskas Kitten Dry Food (1 kg)', N'Pet Food', 216
    UNION ALL
    SELECT N'FO-CAT-007', N'Cat Wet Food Pouch - Whiskas Adult (70 g pouch)', N'Pet Food', 41
    UNION ALL
    SELECT N'FO-CAT-008', N'Cat Wet Food Pouch - Whiskas Kitten (70 g pouch)', N'Pet Food', 44
    UNION ALL
    SELECT N'FO-CAT-009', N'Cat Wet Food Pouch - Goodest (pouch)', N'Pet Food', 35
    UNION ALL
    SELECT N'FO-CAT-010', N'Cat Wet Food Pouch - Kitekat (70 g pouch)', N'Pet Food', 31
    UNION ALL
    SELECT N'FO-CAT-011', N'Besger Wet Cat Food (400 g can)', N'Pet Food', 69
    UNION ALL
    SELECT N'FO-CAT-012', N'SmartHeart Signature Wet Cat Food (80 g pouch)', N'Pet Food', 100
    UNION ALL
    SELECT N'FO-CAT-013', N'Cat Treats - Tuna/Chicken (16 g sachet)', N'Treats', 7
    UNION ALL
    SELECT N'FO-CAT-014', N'Cat Treats - Ciao Churu style puree (35 g pouch)', N'Treats', 38
    UNION ALL
    SELECT N'FO-CAT-015', N'Cat Litter - Basic Clumping (2 kg bag)', N'Supplies', 150
    UNION ALL
    SELECT N'FO-CAT-016', N'Cat Litter - Feline Fresh (10 L bag)', N'Supplies', 312
    UNION ALL
    SELECT N'FO-DOG-001', N'Top Breed Adult Dry Dog Food (1 kg)', N'Pet Food', 113
    UNION ALL
    SELECT N'FO-DOG-002', N'Top Breed Puppy Dry Dog Food (1 kg)', N'Pet Food', 125
    UNION ALL
    SELECT N'FO-DOG-003', N'Beef Pro Adult Dry Dog Food (1 kg)', N'Pet Food', 166
    UNION ALL
    SELECT N'FO-DOG-004', N'Beef Pro Puppy Dry Dog Food (1 kg)', N'Pet Food', 182
    UNION ALL
    SELECT N'FO-DOG-005', N'Aozi Adult Dry Dog Food (1 kg)', N'Pet Food', 200
    UNION ALL
    SELECT N'FO-DOG-006', N'Pedigree Puppy Dry Dog Food (1 kg)', N'Pet Food', 210
    UNION ALL
    SELECT N'FO-DOG-007', N'SmartHeart Dry Dog Food (1.5 kg bag)', N'Pet Food', 355
    UNION ALL
    SELECT N'FO-DOG-008', N'Vitality High Energy Lamb & Beef (1 kg)', N'Pet Food', 324
    UNION ALL
    SELECT N'FO-DOG-009', N'Wet Dog Food Pouch - Royal Canin (85-140 g variant)', N'Pet Food', 92
    UNION ALL
    SELECT N'FO-DOG-010', N'Wet Dog Food Can - Special Dog (can)', N'Pet Food', 100
    UNION ALL
    SELECT N'FO-DOG-011', N'Dog Treats - Jerhigh (pack)', N'Treats', 108
    UNION ALL
    SELECT N'FO-DOG-012', N'Dog Treats - Howbone (70 g pack)', N'Treats', 75
    UNION ALL
    SELECT N'GR-001', N'Saint Roche Premium Dog Shampoo (250 ml)', N'Grooming', 183
    UNION ALL
    SELECT N'GR-002', N'Bearing Anti-Tick & Flea Shampoo (150 ml)', N'Grooming', 163
    UNION ALL
    SELECT N'GR-003', N'Play Pet Shampoo & Conditioner (1 L)', N'Grooming', 313
    UNION ALL
    SELECT N'GR-004', N'Saint Roche Dog Coat Conditioner (500 ml)', N'Grooming', 408
    UNION ALL
    SELECT N'GR-005', N'Pet Ear-Care Lotion (100 ml)', N'Grooming', 249
    UNION ALL
    SELECT N'GR-006', N'Pet Toothpaste (tube)', N'Grooming', 52
    UNION ALL
    SELECT N'GR-007', N'Pet Grooming Wipes (pack)', N'Grooming', 125
    UNION ALL
    SELECT N'GR-008', N'Pet Cologne (250 ml)', N'Grooming', 225
    UNION ALL
    SELECT N'AC-001', N'Stainless Steel Pet Nail Clipper (piece)', N'Accessories', 91
    UNION ALL
    SELECT N'AC-002', N'Pet Slicker Brush (piece)', N'Accessories', 234
    UNION ALL
    SELECT N'AC-003', N'Stainless Steel Pet Comb (piece)', N'Accessories', 156
    UNION ALL
    SELECT N'AC-004', N'Pet Grooming Glove (piece)', N'Accessories', 107
    UNION ALL
    SELECT N'AC-005', N'Dog Poop Bags (roll)', N'Accessories', 26
    UNION ALL
    SELECT N'AC-006', N'Cat Litter Scoop (piece)', N'Accessories', 65
    UNION ALL
    SELECT N'FT-001', N'Muddy Paws Citronella Tick & Flea Spray (100 ml)', N'Health', 355
    UNION ALL
    SELECT N'FT-002', N'Bearing Tick & Flea Shampoo for Cats and Dogs (3000 ml)', N'Health', 1742
    UNION ALL
    SELECT N'FT-003', N'Vet Core+ Tick & Flea Spray (product pack)', N'Health', 454
    UNION ALL
    SELECT N'FT-004', N'Detick Plus (1 cc / 2 cc variant)', N'Health', 144
    UNION ALL
    SELECT N'FT-005', N'Bayopet Tick & Flea Dog Soap (90 g bar)', N'Health', 132
    UNION ALL
    SELECT N'FT-006', N'Pet Flea Comb (piece)', N'Health', 104
    UNION ALL
    SELECT N'CS-001', N'Pet Disinfectant for Grooming Tools (bottle)', N'Supplies', 313
    UNION ALL
    SELECT N'CS-002', N'Pet-Safe Surface Cleaner (bottle)', N'Supplies', 275
)
INSERT INTO dbo.Products (Code, Name, Category, Price)
SELECT p.Code, p.Name, p.Category, p.Price
FROM p
WHERE NOT EXISTS (SELECT 1 FROM dbo.Products x WHERE x.Code = p.Code);
GO

-- ======================================================================== 4. SALON CONSUMABLES (31 items)
-- Supplies the salon uses up (shampoo, disinfectant, gloves...). They are not sold, so they do not go in dbo.Products.
-- The app has no table for them yet, so this one is new. QtyOnHand starts at 0 (the workbook has it blank).
IF OBJECT_ID(N'dbo.SalonConsumables', N'U') IS NULL
CREATE TABLE dbo.SalonConsumables
(
    ConsumableId INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_SalonConsumables PRIMARY KEY,
    ItemCode     NVARCHAR(20)  NOT NULL CONSTRAINT UQ_SalonConsumables_Code UNIQUE,
    Name         NVARCHAR(100) NOT NULL,
    Category     NVARCHAR(40)  NOT NULL,
    Species      NVARCHAR(10)  NOT NULL,          -- Dog | Cat | Both
    Unit         NVARCHAR(20)  NOT NULL,
    UnitCost     DECIMAL(10,2) NOT NULL CONSTRAINT DF_SalonConsumables_Cost DEFAULT 0,   -- estimated, in PHP
    MinimumStock INT           NOT NULL CONSTRAINT DF_SalonConsumables_Min  DEFAULT 0,
    QtyOnHand    INT           NOT NULL CONSTRAINT DF_SalonConsumables_Qty  DEFAULT 0,
    Notes        NVARCHAR(200) NULL,
    IsActive     BIT           NOT NULL CONSTRAINT DF_SalonConsumables_Active DEFAULT 1
);
GO

;WITH c (ItemCode, Name, Category, Species, Unit, UnitCost, MinimumStock, Notes) AS (
    SELECT N'CS001', N'General-Purpose Dog Shampoo', N'Shampoo', N'Dog', N'bottle', 150, 2, N'Use suitable dog product'
    UNION ALL
    SELECT N'CS002', N'Cat-Safe Shampoo', N'Shampoo', N'Cat', N'bottle', 180, 2, N'Must be labelled suitable for cats'
    UNION ALL
    SELECT N'CS003', N'Puppy Shampoo', N'Shampoo', N'Dog', N'bottle', 160, 1, NULL
    UNION ALL
    SELECT N'CS004', N'Hypoallergenic Pet Shampoo', N'Shampoo', N'Both', N'bottle', 220, 1, N'Check species label'
    UNION ALL
    SELECT N'CS005', N'De-Shedding Dog Shampoo', N'Shampoo', N'Dog', N'bottle', 220, 1, NULL
    UNION ALL
    SELECT N'CS006', N'Pet Conditioner', N'Coat Care', N'Both', N'bottle', 250, 1, N'Check species label'
    UNION ALL
    SELECT N'CS007', N'Pet Detangling Spray', N'Coat Care', N'Both', N'bottle', 200, 1, N'Check species label'
    UNION ALL
    SELECT N'CS008', N'Cat Waterless Shampoo', N'Coat Care', N'Cat', N'bottle', 220, 1, NULL
    UNION ALL
    SELECT N'CS009', N'Pet Ear-Cleaning Solution', N'Ear Care', N'Both', N'bottle', 180, 1, N'Use as directed; not a treatment for ear disease'
    UNION ALL
    SELECT N'CS010', N'Pet Grooming Wipes', N'Wipes', N'Both', N'pack', 100, 2, NULL
    UNION ALL
    SELECT N'CS011', N'Pet Cologne', N'Finishing Product', N'Both', N'bottle', 180, 1, N'Only species-safe products'
    UNION ALL
    SELECT N'CS012', N'Clipper Blade Oil', N'Tool Maintenance', N'Both', N'bottle', 150, 1, NULL
    UNION ALL
    SELECT N'CS013', N'Clipper Blade Coolant Spray', N'Tool Maintenance', N'Both', N'can', 250, 1, NULL
    UNION ALL
    SELECT N'CS014', N'Clipper Blade Wash Solution', N'Tool Maintenance', N'Both', N'bottle', 250, 1, NULL
    UNION ALL
    SELECT N'CS015', N'Grooming Tool Disinfectant', N'Sanitation', N'Both', N'bottle', 250, 2, N'Follow label contact time'
    UNION ALL
    SELECT N'CS016', N'Pet-Safe Surface Disinfectant', N'Sanitation', N'Both', N'bottle', 220, 2, N'Use for labelled surfaces'
    UNION ALL
    SELECT N'CS017', N'Pet Bath Tub Cleaner', N'Cleaning', N'Both', N'bottle', 180, 1, NULL
    UNION ALL
    SELECT N'CS018', N'Cleaning Sponge', N'Cleaning', N'Both', N'piece', 25, 5, NULL
    UNION ALL
    SELECT N'CS019', N'Microfiber Cleaning Cloth', N'Cleaning', N'Both', N'piece', 50, 5, NULL
    UNION ALL
    SELECT N'CS020', N'Cotton Balls', N'Consumable', N'Both', N'pack', 45, 2, NULL
    UNION ALL
    SELECT N'CS021', N'Cotton Pads', N'Consumable', N'Both', N'pack', 50, 2, NULL
    UNION ALL
    SELECT N'CS022', N'Disposable Gloves', N'PPE', N'Both', N'box', 180, 2, NULL
    UNION ALL
    SELECT N'CS023', N'Grooming Apron', N'PPE', N'Both', N'piece', 250, 2, N'Reusable'
    UNION ALL
    SELECT N'CS024', N'Garbage Bags', N'Cleaning', N'Both', N'roll', 60, 2, NULL
    UNION ALL
    SELECT N'CS025', N'Laundry Detergent', N'Laundry', N'Both', N'pack', 100, 2, NULL
    UNION ALL
    SELECT N'CS026', N'First-Aid Kit', N'Safety', N'Both', N'set', 500, 1, NULL
    UNION ALL
    SELECT N'CS027', N'Gauze Pads', N'First Aid', N'Both', N'pack', 80, 1, NULL
    UNION ALL
    SELECT N'CS028', N'Bandage Roll', N'First Aid', N'Both', N'roll', 60, 1, NULL
    UNION ALL
    SELECT N'CS029', N'Styptic Powder', N'Nail Care', N'Both', N'container', 180, 1, N'For minor nail bleeding; follow directions'
    UNION ALL
    SELECT N'CS030', N'Dog Poop Bags', N'Consumable', N'Dog', N'roll', 50, 2, NULL
    UNION ALL
    SELECT N'CS031', N'Cat Litter (clumping)', N'Cat Care', N'Cat', N'bag', 180, 2, N'Only if salon uses litter trays'
)
INSERT INTO dbo.SalonConsumables (ItemCode, Name, Category, Species, Unit, UnitCost, MinimumStock, Notes)
SELECT c.ItemCode, c.Name, c.Category, c.Species, c.Unit, c.UnitCost, c.MinimumStock, c.Notes
FROM c
WHERE NOT EXISTS (SELECT 1 FROM dbo.SalonConsumables x WHERE x.ItemCode = c.ItemCode);
GO

-- ======================================================================== check
SELECT N'Services' AS [Table], COUNT(*) AS [Rows] FROM dbo.Services
UNION ALL SELECT N'Products', COUNT(*) FROM dbo.Products
UNION ALL SELECT N'SalonConsumables', COUNT(*) FROM dbo.SalonConsumables
UNION ALL SELECT N'Leftover TEST rows', (SELECT COUNT(*) FROM dbo.Services WHERE Name LIKE N'TEST%')
                                      + (SELECT COUNT(*) FROM dbo.Products WHERE Name LIKE N'TEST%')
                                      + (SELECT COUNT(*) FROM dbo.Customers WHERE FullName LIKE N'TEST%')
                                      + (SELECT COUNT(*) FROM dbo.Rooms WHERE Name LIKE N'TEST%');
GO
