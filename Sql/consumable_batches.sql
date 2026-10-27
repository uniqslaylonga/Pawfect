-- Grooming supplies in batches (with expiry dates) + how much of each supply a service uses.
-- Safe to run more than once. Run it once in SSMS on the Pawfect database (needs Sql/retail_and_consumables.sql to have been run first).
-- Until this file is run, the app simply skips supply tracking.

USE Pawfect;
GO

-- ------------------------------------------------------------ 1. supply batches
IF OBJECT_ID(N'dbo.ConsumableBatches', N'U') IS NULL
CREATE TABLE dbo.ConsumableBatches
(
    BatchId      INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_ConsumableBatches PRIMARY KEY,
    ConsumableId INT            NOT NULL CONSTRAINT FK_ConsumableBatches_Consumables REFERENCES dbo.Consumables(ConsumableId),
    BatchNo      NVARCHAR(40)   NOT NULL,
    Supplier     NVARCHAR(100)  NULL,
    ReceivedDate DATE           NOT NULL CONSTRAINT DF_ConsumableBatches_Received DEFAULT CAST(SYSDATETIME() AS DATE),
    ExpiryDate   DATE           NULL,
    QtyReceived  DECIMAL(10,3)  NOT NULL,
    QtyRemaining DECIMAL(10,3)  NOT NULL
);
GO

-- ------------------------------------------------------------ 2. how much of each supply one service uses (in the supply's own unit)
IF OBJECT_ID(N'dbo.ServiceConsumables', N'U') IS NULL
CREATE TABLE dbo.ServiceConsumables
(
    ServiceName    NVARCHAR(100) NOT NULL,   -- dbo.Services.Name (every weight band of that service uses the same amount)
    ConsumableCode NVARCHAR(20)  NOT NULL,   -- dbo.Consumables.ItemCode
    QtyPerService  DECIMAL(8,3)  NOT NULL,   -- e.g. 0.05 = one twentieth of a bottle
    CONSTRAINT PK_ServiceConsumables PRIMARY KEY (ServiceName, ConsumableCode)
);
GO

-- Starting amounts. These are estimates: change them to what your salon really uses (UPDATE dbo.ServiceConsumables ...).
DECLARE @m TABLE (ServiceName NVARCHAR(100), Code NVARCHAR(20), Qty DECIMAL(8,3));
INSERT INTO @m VALUES
    (N'Basic Bath & Blow Dry',          N'CS001', 0.05), (N'Basic Bath & Blow Dry',          N'CS016', 0.02), (N'Basic Bath & Blow Dry', N'CS015', 0.01),
    (N'Full Groom',                     N'CS001', 0.05), (N'Full Groom',                     N'CS006', 0.03), (N'Full Groom', N'CS012', 0.02),
    (N'Full Groom',                     N'CS016', 0.02), (N'Full Groom',                     N'CS015', 0.01),
    (N'De-shedding Bath Package',       N'CS005', 0.06), (N'De-shedding Bath Package',       N'CS006', 0.03), (N'De-shedding Bath Package', N'CS016', 0.02),
    (N'Bath + Nail Trim',               N'CS001', 0.05), (N'Bath + Nail Trim',               N'CS029', 0.01), (N'Bath + Nail Trim', N'CS016', 0.02),
    (N'Nail Trim Only',                 N'CS029', 0.01), (N'Nail Trim Only',                 N'CS015', 0.01),
    (N'Cat Bath & Blow Dry',            N'CS002', 0.05), (N'Cat Bath & Blow Dry',            N'CS016', 0.02), (N'Cat Bath & Blow Dry', N'CS015', 0.01),
    (N'Cat Full Groom / Sanitary Trim', N'CS002', 0.05), (N'Cat Full Groom / Sanitary Trim', N'CS012', 0.02), (N'Cat Full Groom / Sanitary Trim', N'CS016', 0.02),
    (N'Brush-Out & Nail Trim',          N'CS029', 0.01), (N'Brush-Out & Nail Trim',          N'CS015', 0.01),
    (N'Sanitary Trim Only',             N'CS012', 0.02), (N'Sanitary Trim Only',             N'CS010', 0.10),
    (N'Ear Exterior Cleaning',          N'CS009', 0.05), (N'Ear Exterior Cleaning',          N'CS020', 0.10);

INSERT INTO dbo.ServiceConsumables (ServiceName, ConsumableCode, QtyPerService)
SELECT m.ServiceName, m.Code, m.Qty FROM @m m
WHERE NOT EXISTS (SELECT 1 FROM dbo.ServiceConsumables x WHERE x.ServiceName = m.ServiceName AND x.ConsumableCode = m.Code);
GO

-- ------------------------------------------------------------ 3. stock counted before batches existed becomes an "OPENING" batch (no expiry date)
INSERT INTO dbo.ConsumableBatches (ConsumableId, BatchNo, Supplier, ReceivedDate, ExpiryDate, QtyReceived, QtyRemaining)
SELECT c.ConsumableId, N'OPENING', NULL, CAST(SYSDATETIME() AS DATE), NULL, c.QtyOnHand, c.QtyOnHand
FROM dbo.Consumables c
WHERE c.IsActive = 1 AND c.QtyOnHand > 0
  AND NOT EXISTS (SELECT 1 FROM dbo.ConsumableBatches b WHERE b.ConsumableId = c.ConsumableId);
GO

SELECT N'Supply batches' AS What, COUNT(*) AS Rows FROM dbo.ConsumableBatches
UNION ALL SELECT N'Service -> supply rules', COUNT(*) FROM dbo.ServiceConsumables;
GO
