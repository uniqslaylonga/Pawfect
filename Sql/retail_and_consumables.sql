-- Retail products and salon consumables (from Pawfect_norbs.xlsx > Retail Products and Salon Consumables).
-- Run once after employee_setup.sql. Safe to run again: it adds missing rows and refreshes name, category and price,
-- but NEVER touches stock quantities you have already counted.
--   Retail   -> dbo.Products     (shows in Counter POS & Billing; price = Suggested Selling Price, or Actual Selling Price if filled in)
--   Consumables -> dbo.Consumables (salon supplies used up while grooming; not sold, so not in billing)

USE Pawfect;
GO

-- ------------------------------------------------------------ 1. Products: extra inventory columns
IF COL_LENGTH('dbo.Products', 'Species')      IS NULL ALTER TABLE dbo.Products ADD Species      NVARCHAR(10)  NOT NULL CONSTRAINT DF_Products_Species DEFAULT N'Both';
IF COL_LENGTH('dbo.Products', 'PackSize')     IS NULL ALTER TABLE dbo.Products ADD PackSize     NVARCHAR(40)  NULL;
IF COL_LENGTH('dbo.Products', 'UnitCost')     IS NULL ALTER TABLE dbo.Products ADD UnitCost     DECIMAL(10,2) NULL;
IF COL_LENGTH('dbo.Products', 'QtyOnHand')    IS NULL ALTER TABLE dbo.Products ADD QtyOnHand    INT           NOT NULL CONSTRAINT DF_Products_Qty DEFAULT 0;
IF COL_LENGTH('dbo.Products', 'ReorderLevel') IS NULL ALTER TABLE dbo.Products ADD ReorderLevel INT           NOT NULL CONSTRAINT DF_Products_Reorder DEFAULT 0;
GO

-- ------------------------------------------------------------ 2. Consumables table
IF OBJECT_ID(N'dbo.Consumables', N'U') IS NULL
CREATE TABLE dbo.Consumables
(
    ConsumableId INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Consumables PRIMARY KEY,
    ItemCode     NVARCHAR(20)  NOT NULL,
    Name         NVARCHAR(100) NOT NULL,
    Category     NVARCHAR(40)  NOT NULL,
    Species      NVARCHAR(10)  NOT NULL CONSTRAINT DF_Consumables_Species DEFAULT N'Both',
    Unit         NVARCHAR(20)  NOT NULL,
    UnitCost     DECIMAL(10,2) NOT NULL,
    QtyOnHand    INT           NOT NULL CONSTRAINT DF_Consumables_Qty DEFAULT 0,
    MinStock     INT           NOT NULL CONSTRAINT DF_Consumables_Min DEFAULT 0,
    Notes        NVARCHAR(200) NULL,
    IsActive     BIT           NOT NULL CONSTRAINT DF_Consumables_Active DEFAULT 1,
    CONSTRAINT UQ_Consumables_Code UNIQUE (ItemCode)
);
GO

-- ------------------------------------------------------------ 3. Retail products
DECLARE @p TABLE (Code NVARCHAR(40), Name NVARCHAR(100), Category NVARCHAR(40), Species NVARCHAR(10), PackSize NVARCHAR(40),
                  UnitCost DECIMAL(10,2), Price DECIMAL(10,2), Qty INT, Reorder INT);
INSERT INTO @p VALUES
    (N'FO-CAT-001', N'Toei Cat Dry Food - Salmon (1 kg repacked)', N'Dry Cat Food', N'Cat', N'1 kg repacked', 101, 126, 0, 3),
    (N'FO-CAT-002', N'Vitality Catcare Dry Cat Food (1 kg)', N'Dry Cat Food', N'Cat', N'1 kg', 250, 300, 0, 3),
    (N'FO-CAT-003', N'Maxime Dry Cat Food (1 kg repacked)', N'Dry Cat Food', N'Cat', N'1 kg repacked', 157, 188, 0, 3),
    (N'FO-CAT-004', N'Aozi Adult Cat Dry Food (1 kg)', N'Dry Cat Food', N'Cat', N'1 kg', 175, 210, 0, 3),
    (N'FO-CAT-005', N'Whiskas Adult Cat Dry Food (1 kg)', N'Dry Cat Food', N'Cat', N'1 kg', 167, 200, 0, 3),
    (N'FO-CAT-006', N'Whiskas Kitten Dry Food (1 kg)', N'Dry Cat Food', N'Cat', N'1 kg', 180, 216, 0, 3),
    (N'FO-CAT-007', N'Cat Wet Food Pouch - Whiskas Adult (70 g pouch)', N'Wet Cat Food', N'Cat', N'70 g pouch', 33, 41, 0, 3),
    (N'FO-CAT-008', N'Cat Wet Food Pouch - Whiskas Kitten (70 g pouch)', N'Wet Cat Food', N'Cat', N'70 g pouch', 35, 44, 0, 3),
    (N'FO-CAT-009', N'Cat Wet Food Pouch - Goodest', N'Wet Cat Food', N'Cat', N'pouch', 28, 35, 0, 3),
    (N'FO-CAT-010', N'Cat Wet Food Pouch - Kitekat (70 g pouch)', N'Wet Cat Food', N'Cat', N'70 g pouch', 25, 31, 0, 3),
    (N'FO-CAT-011', N'Besger Wet Cat Food (400 g can)', N'Wet Cat Food', N'Cat', N'400 g can', 55, 69, 0, 3),
    (N'FO-CAT-012', N'SmartHeart Signature Wet Cat Food (80 g pouch)', N'Wet Cat Food', N'Cat', N'80 g pouch', 83, 100, 0, 3),
    (N'FO-CAT-013', N'Cat Treats - Tuna/Chicken (16 g sachet)', N'Cat Treats', N'Cat', N'16 g sachet', 5, 7, 0, 3),
    (N'FO-CAT-014', N'Cat Treats - Ciao Churu style puree (35 g pouch)', N'Cat Treats', N'Cat', N'35 g pouch', 30, 38, 0, 3),
    (N'FO-CAT-015', N'Cat Litter - Basic Clumping (2 kg bag)', N'Cat Litter', N'Cat', N'2 kg bag', 120, 150, 0, 3),
    (N'FO-CAT-016', N'Cat Litter - Feline Fresh (10 L bag)', N'Cat Litter', N'Cat', N'10 L bag', 260, 312, 0, 3),
    (N'FO-DOG-001', N'Top Breed Adult Dry Dog Food (1 kg)', N'Dry Dog Food', N'Dog', N'1 kg', 90, 113, 0, 3),
    (N'FO-DOG-002', N'Top Breed Puppy Dry Dog Food (1 kg)', N'Dry Dog Food', N'Dog', N'1 kg', 100, 125, 0, 3),
    (N'FO-DOG-003', N'Beef Pro Adult Dry Dog Food (1 kg)', N'Dry Dog Food', N'Dog', N'1 kg', 138, 166, 0, 3),
    (N'FO-DOG-004', N'Beef Pro Puppy Dry Dog Food (1 kg)', N'Dry Dog Food', N'Dog', N'1 kg', 152, 182, 0, 3),
    (N'FO-DOG-005', N'Aozi Adult Dry Dog Food (1 kg)', N'Dry Dog Food', N'Dog', N'1 kg', 167, 200, 0, 3),
    (N'FO-DOG-006', N'Pedigree Puppy Dry Dog Food (1 kg)', N'Dry Dog Food', N'Dog', N'1 kg', 175, 210, 0, 3),
    (N'FO-DOG-007', N'SmartHeart Dry Dog Food (1.5 kg bag)', N'Dry Dog Food', N'Dog', N'1.5 kg bag', 296, 355, 0, 3),
    (N'FO-DOG-008', N'Vitality High Energy Lamb & Beef (1 kg)', N'Dry Dog Food', N'Dog', N'1 kg', 270, 324, 0, 3),
    (N'FO-DOG-009', N'Wet Dog Food Pouch - Royal Canin (85-140 g variant)', N'Wet Dog Food', N'Dog', N'85-140 g variant', 77, 92, 0, 3),
    (N'FO-DOG-010', N'Wet Dog Food Can - Special Dog', N'Wet Dog Food', N'Dog', N'can', 80, 100, 0, 3),
    (N'FO-DOG-011', N'Dog Treats - Jerhigh', N'Dog Treats', N'Dog', N'pack', 86, 108, 0, 3),
    (N'FO-DOG-012', N'Dog Treats - Howbone (70 g pack)', N'Dog Treats', N'Dog', N'70 g pack', 60, 75, 0, 3),
    (N'GR-001', N'Saint Roche Premium Dog Shampoo (250 ml)', N'Grooming Retail', N'Dog', N'250 ml', 146, 183, 0, 3),
    (N'GR-002', N'Bearing Anti-Tick & Flea Shampoo (150 ml)', N'Grooming Retail', N'Dog', N'150 ml', 130, 163, 0, 3),
    (N'GR-003', N'Play Pet Shampoo & Conditioner (1 L)', N'Grooming Retail', N'Both', N'1 L', 250, 313, 0, 3),
    (N'GR-004', N'Saint Roche Dog Coat Conditioner (500 ml)', N'Grooming Retail', N'Dog', N'500 ml', 340, 408, 0, 3),
    (N'GR-005', N'Pet Ear-Care Lotion (100 ml)', N'Grooming Retail', N'Dog', N'100 ml', 199, 249, 0, 3),
    (N'GR-006', N'Pet Toothpaste', N'Grooming Retail', N'Both', N'tube', 40, 52, 0, 3),
    (N'GR-007', N'Pet Grooming Wipes', N'Grooming Retail', N'Both', N'pack', 100, 125, 0, 3),
    (N'GR-008', N'Pet Cologne (250 ml)', N'Grooming Retail', N'Both', N'250 ml', 180, 225, 0, 3),
    (N'AC-001', N'Stainless Steel Pet Nail Clipper', N'Accessories', N'Both', N'piece', 70, 91, 0, 3),
    (N'AC-002', N'Pet Slicker Brush', N'Accessories', N'Both', N'piece', 180, 234, 0, 3),
    (N'AC-003', N'Stainless Steel Pet Comb', N'Accessories', N'Both', N'piece', 120, 156, 0, 3),
    (N'AC-004', N'Pet Grooming Glove', N'Accessories', N'Both', N'piece', 82, 107, 0, 3),
    (N'AC-005', N'Dog Poop Bags', N'Accessories', N'Dog', N'roll', 20, 26, 0, 3),
    (N'AC-006', N'Cat Litter Scoop', N'Accessories', N'Cat', N'piece', 50, 65, 0, 3),
    (N'FT-001', N'Muddy Paws Citronella Tick & Flea Spray (100 ml)', N'Flea & Tick Retail', N'Dog', N'100 ml', 309, 355, 0, 3),
    (N'FT-002', N'Bearing Tick & Flea Shampoo for Cats and Dogs (3000 ml)', N'Flea & Tick Retail', N'Both', N'3000 ml', 1515, 1742, 0, 3),
    (N'FT-003', N'Vet Core+ Tick & Flea Spray', N'Flea & Tick Retail', N'Both', N'product pack', 395, 454, 0, 3),
    (N'FT-004', N'Detick Plus (1 cc / 2 cc variant)', N'Flea & Tick Retail', N'Both', N'1 cc / 2 cc variant', 125, 144, 0, 3),
    (N'FT-005', N'Bayopet Tick & Flea Dog Soap (90 g bar)', N'Flea & Tick Retail', N'Dog', N'90 g bar', 110, 132, 0, 3),
    (N'FT-006', N'Pet Flea Comb', N'Flea & Tick Retail', N'Both', N'piece', 80, 104, 0, 3),
    (N'CS-001', N'Pet Disinfectant for Grooming Tools', N'Salon / Cleaning Retail', N'Both', N'bottle', 250, 313, 0, 3),
    (N'CS-002', N'Pet-Safe Surface Cleaner', N'Salon / Cleaning Retail', N'Both', N'bottle', 220, 275, 0, 3);

UPDATE t SET Name = s.Name, Category = s.Category, Species = s.Species, PackSize = s.PackSize,
             UnitCost = s.UnitCost, Price = s.Price, ReorderLevel = s.Reorder, IsActive = 1
FROM dbo.Products t JOIN @p s ON s.Code = t.Code;

INSERT INTO dbo.Products (Code, Name, Category, Species, PackSize, UnitCost, Price, QtyOnHand, ReorderLevel)
SELECT s.Code, s.Name, s.Category, s.Species, s.PackSize, s.UnitCost, s.Price, s.Qty, s.Reorder
FROM @p s WHERE NOT EXISTS (SELECT 1 FROM dbo.Products t WHERE t.Code = s.Code);

-- ------------------------------------------------------------ 4. Salon consumables
DECLARE @c TABLE (ItemCode NVARCHAR(20), Name NVARCHAR(100), Category NVARCHAR(40), Species NVARCHAR(10), Unit NVARCHAR(20),
                  UnitCost DECIMAL(10,2), Qty INT, MinStock INT, Notes NVARCHAR(200));
INSERT INTO @c VALUES
    (N'CS001', N'General-Purpose Dog Shampoo', N'Shampoo', N'Dog', N'bottle', 150, 0, 2, N'Use suitable dog product'),
    (N'CS002', N'Cat-Safe Shampoo', N'Shampoo', N'Cat', N'bottle', 180, 0, 2, N'Must be labelled suitable for cats'),
    (N'CS003', N'Puppy Shampoo', N'Shampoo', N'Dog', N'bottle', 160, 0, 1, NULL),
    (N'CS004', N'Hypoallergenic Pet Shampoo', N'Shampoo', N'Both', N'bottle', 220, 0, 1, N'Check species label'),
    (N'CS005', N'De-Shedding Dog Shampoo', N'Shampoo', N'Dog', N'bottle', 220, 0, 1, NULL),
    (N'CS006', N'Pet Conditioner', N'Coat Care', N'Both', N'bottle', 250, 0, 1, N'Check species label'),
    (N'CS007', N'Pet Detangling Spray', N'Coat Care', N'Both', N'bottle', 200, 0, 1, N'Check species label'),
    (N'CS008', N'Cat Waterless Shampoo', N'Coat Care', N'Cat', N'bottle', 220, 0, 1, NULL),
    (N'CS009', N'Pet Ear-Cleaning Solution', N'Ear Care', N'Both', N'bottle', 180, 0, 1, N'Use as directed; not a treatment for ear disease'),
    (N'CS010', N'Pet Grooming Wipes', N'Wipes', N'Both', N'pack', 100, 0, 2, NULL),
    (N'CS011', N'Pet Cologne', N'Finishing Product', N'Both', N'bottle', 180, 0, 1, N'Only species-safe products'),
    (N'CS012', N'Clipper Blade Oil', N'Tool Maintenance', N'Both', N'bottle', 150, 0, 1, NULL),
    (N'CS013', N'Clipper Blade Coolant Spray', N'Tool Maintenance', N'Both', N'can', 250, 0, 1, NULL),
    (N'CS014', N'Clipper Blade Wash Solution', N'Tool Maintenance', N'Both', N'bottle', 250, 0, 1, NULL),
    (N'CS015', N'Grooming Tool Disinfectant', N'Sanitation', N'Both', N'bottle', 250, 0, 2, N'Follow label contact time'),
    (N'CS016', N'Pet-Safe Surface Disinfectant', N'Sanitation', N'Both', N'bottle', 220, 0, 2, N'Use for labelled surfaces'),
    (N'CS017', N'Pet Bath Tub Cleaner', N'Cleaning', N'Both', N'bottle', 180, 0, 1, NULL),
    (N'CS018', N'Cleaning Sponge', N'Cleaning', N'Both', N'piece', 25, 0, 5, NULL),
    (N'CS019', N'Microfiber Cleaning Cloth', N'Cleaning', N'Both', N'piece', 50, 0, 5, NULL),
    (N'CS020', N'Cotton Balls', N'Consumable', N'Both', N'pack', 45, 0, 2, NULL),
    (N'CS021', N'Cotton Pads', N'Consumable', N'Both', N'pack', 50, 0, 2, NULL),
    (N'CS022', N'Disposable Gloves', N'PPE', N'Both', N'box', 180, 0, 2, NULL),
    (N'CS023', N'Grooming Apron', N'PPE', N'Both', N'piece', 250, 0, 2, N'Reusable'),
    (N'CS024', N'Garbage Bags', N'Cleaning', N'Both', N'roll', 60, 0, 2, NULL),
    (N'CS025', N'Laundry Detergent', N'Laundry', N'Both', N'pack', 100, 0, 2, NULL),
    (N'CS026', N'First-Aid Kit', N'Safety', N'Both', N'set', 500, 0, 1, NULL),
    (N'CS027', N'Gauze Pads', N'First Aid', N'Both', N'pack', 80, 0, 1, NULL),
    (N'CS028', N'Bandage Roll', N'First Aid', N'Both', N'roll', 60, 0, 1, NULL),
    (N'CS029', N'Styptic Powder', N'Nail Care', N'Both', N'container', 180, 0, 1, N'For minor nail bleeding; follow directions'),
    (N'CS030', N'Dog Poop Bags', N'Consumable', N'Dog', N'roll', 50, 0, 2, NULL),
    (N'CS031', N'Cat Litter (clumping)', N'Cat Care', N'Cat', N'bag', 180, 0, 2, N'Only if salon uses litter trays');

UPDATE t SET Name = s.Name, Category = s.Category, Species = s.Species, Unit = s.Unit,
             UnitCost = s.UnitCost, MinStock = s.MinStock, Notes = s.Notes, IsActive = 1
FROM dbo.Consumables t JOIN @c s ON s.ItemCode = t.ItemCode;

INSERT INTO dbo.Consumables (ItemCode, Name, Category, Species, Unit, UnitCost, QtyOnHand, MinStock, Notes)
SELECT s.ItemCode, s.Name, s.Category, s.Species, s.Unit, s.UnitCost, s.Qty, s.MinStock, s.Notes
FROM @c s WHERE NOT EXISTS (SELECT 1 FROM dbo.Consumables t WHERE t.ItemCode = s.ItemCode);
GO

SELECT N'Retail products' AS What, COUNT(*) AS Rows FROM dbo.Products WHERE Code LIKE N'FO-%' OR Code LIKE N'GR-%' OR Code LIKE N'AC-%' OR Code LIKE N'FT-%' OR Code LIKE N'CS-%'
UNION ALL SELECT N'Consumables', COUNT(*) FROM dbo.Consumables;
GO
