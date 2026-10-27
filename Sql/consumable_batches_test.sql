-- TEST DATA for expired-stock blocking and supply batches. Run Sql/consumable_batches.sql first.
-- Section A makes a retail product with an expired batch; section B does the same for a grooming supply; section C removes the test data.
-- Dates are relative to today, so this can be run on any day.

USE Pawfect;
GO

-- ============================================================ A. RETAIL PRODUCT "Dog Poop Bags" (AC-005): 10 units
DECLARE @p INT = (SELECT ProductId FROM dbo.Products WHERE Code = N'AC-005');
DECLARE @today DATE = CAST(SYSDATETIME() AS DATE);
DELETE FROM dbo.StockBatches WHERE ProductId = @p;
INSERT INTO dbo.StockBatches (ProductId, BatchNo, Supplier, ReceivedDate, ExpiryDate, QtyReceived, QtyRemaining) VALUES
    (@p, N'TEST-EXPIRED', N'Test supplier', DATEADD(DAY, -60, @today), DATEADD(DAY,  -7, @today), 5, 5),   -- already expired
    (@p, N'TEST-LATE',    N'Test supplier', DATEADD(DAY, -10, @today), DATEADD(DAY,  90, @today), 3, 3),   -- expires later
    (@p, N'TEST-SOON',    N'Test supplier', DATEADD(DAY,  -5, @today), DATEADD(DAY,  10, @today), 2, 2);   -- expires first (and was received last)
UPDATE dbo.Products SET QtyOnHand = 10, IsActive = 1 WHERE ProductId = @p;
GO

-- ============================================================ B. SUPPLY "General-Purpose Dog Shampoo" (CS001), used by Full Groom (0.05 bottle each)
DECLARE @c INT = (SELECT ConsumableId FROM dbo.Consumables WHERE ItemCode = N'CS001');
DECLARE @today DATE = CAST(SYSDATETIME() AS DATE);
DELETE FROM dbo.ConsumableBatches WHERE ConsumableId = @c;
INSERT INTO dbo.ConsumableBatches (ConsumableId, BatchNo, Supplier, ReceivedDate, ExpiryDate, QtyReceived, QtyRemaining) VALUES
    (@c, N'TEST-EXPIRED', N'Test supplier', DATEADD(DAY, -90, @today), DATEADD(DAY,  -3, @today), 1.000, 1.000),   -- expired
    (@c, N'TEST-LATE',    N'Test supplier', DATEADD(DAY, -10, @today), DATEADD(DAY,  60, @today), 0.080, 0.080),
    (@c, N'TEST-SOON',    N'Test supplier', DATEADD(DAY,  -2, @today), DATEADD(DAY,   5, @today), 0.040, 0.040);
UPDATE dbo.Consumables SET QtyOnHand = 2 WHERE ConsumableId = @c;
GO

-- ============================================================ CHECK (run after each sale)
SELECT N'Product' AS Kind, b.BatchNo, b.ExpiryDate, b.QtyRemaining
FROM dbo.StockBatches b JOIN dbo.Products p ON p.ProductId = b.ProductId WHERE p.Code = N'AC-005'
UNION ALL
SELECT N'Supply', b.BatchNo, b.ExpiryDate, b.QtyRemaining
FROM dbo.ConsumableBatches b JOIN dbo.Consumables c ON c.ConsumableId = b.ConsumableId WHERE c.ItemCode = N'CS001'
ORDER BY 1, 3;
SELECT ItemCode, Name, QtyOnHand FROM dbo.Consumables WHERE ItemCode = N'CS001';
GO

-- ============================================================ C. CLEAN UP (removes the test batches)
-- DELETE FROM dbo.StockBatches      WHERE BatchNo LIKE N'TEST-%';
-- DELETE FROM dbo.ConsumableBatches WHERE BatchNo LIKE N'TEST-%';
-- UPDATE dbo.Products    SET QtyOnHand = (SELECT ISNULL(SUM(QtyRemaining),0) FROM dbo.StockBatches b      WHERE b.ProductId    = dbo.Products.ProductId)    WHERE Code     = N'AC-005';
-- UPDATE dbo.Consumables SET QtyOnHand = (SELECT CAST(CEILING(ISNULL(SUM(QtyRemaining),0)) AS INT) FROM dbo.ConsumableBatches b WHERE b.ConsumableId = dbo.Consumables.ConsumableId) WHERE ItemCode = N'CS001';
