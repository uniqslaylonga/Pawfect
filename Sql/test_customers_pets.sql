-- OPTIONAL. Only for trying Customer & Pet Lookup. Run employee_setup.sql first.
-- Adds 10 test customers with 1-2 pets each, so searching, filters (With pets / Without pets,
-- species), sorting, the "New Customers" count and paging (8 per page) can all be tried.
-- Every customer name starts with "TEST" so Sql/remove_test_data.sql deletes them (and their pets) again.
-- Safe to run more than once.
-- Note: "Last Visit" only fills in after a sale is billed to a customer.

USE Pawfect;
GO

;WITH c (FullName, Phone, Email, Address) AS (
    SELECT N'TEST Ana Reyes',        N'0000-000-0001', N'test.ana@pawfect.test',     N'Test address 1'  UNION ALL
    SELECT N'TEST Mark Villanueva',  N'0000-000-0002', N'test.mark@pawfect.test',    N'Test address 2'  UNION ALL
    SELECT N'TEST Camille Torres',   N'0000-000-0003', N'test.camille@pawfect.test', N'Test address 3'  UNION ALL
    SELECT N'TEST James Santos',     N'0000-000-0004', N'test.james@pawfect.test',   N'Test address 4'  UNION ALL
    SELECT N'TEST Liza Garcia',      N'0000-000-0005', N'test.liza@pawfect.test',    N'Test address 5'  UNION ALL
    SELECT N'TEST Robert Dela Cruz', N'0000-000-0006', N'test.robert@pawfect.test',  N'Test address 6'  UNION ALL
    SELECT N'TEST Angelica Ramos',   N'0000-000-0007', N'test.angelica@pawfect.test',N'Test address 7'  UNION ALL
    SELECT N'TEST Kevin Reyes',      N'0000-000-0008', N'test.kevin@pawfect.test',   N'Test address 8'  UNION ALL
    SELECT N'TEST Paolo Mendoza',    N'0000-000-0009', N'test.paolo@pawfect.test',   N'Test address 9'  UNION ALL
    SELECT N'TEST Bianca Lim',       N'0000-000-0010', N'test.bianca@pawfect.test',  N'Test address 10'
)
INSERT INTO dbo.Customers (FullName, Phone, Email, Address)
SELECT c.FullName, c.Phone, c.Email, c.Address
FROM c
WHERE NOT EXISTS (SELECT 1 FROM dbo.Customers x WHERE x.FullName = c.FullName);
GO

-- Pets (Paolo Mendoza and Bianca Lim are left without pets on purpose, to try the "Without pets" filter)
;WITH p (Owner, Name, Species, Breed) AS (
    SELECT N'TEST Ana Reyes',        N'TEST Luna',     N'Dog',    N'Golden Retriever' UNION ALL
    SELECT N'TEST Ana Reyes',        N'TEST Biscuit',  N'Dog',    N'Poodle'           UNION ALL
    SELECT N'TEST Mark Villanueva',  N'TEST Mochi',    N'Cat',    N'Persian'          UNION ALL
    SELECT N'TEST Camille Torres',   N'TEST Max',      N'Dog',    N'Shih Tzu'         UNION ALL
    SELECT N'TEST James Santos',     N'TEST Whiskers', N'Cat',    N'Domestic Shorthair' UNION ALL
    SELECT N'TEST Liza Garcia',      N'TEST Bella',    N'Dog',    N'French Bulldog'   UNION ALL
    SELECT N'TEST Liza Garcia',      N'TEST Coco',     N'Cat',    N'Ragdoll'          UNION ALL
    SELECT N'TEST Robert Dela Cruz', N'TEST Snowball', N'Rabbit', N'Holland Lop'      UNION ALL
    SELECT N'TEST Angelica Ramos',   N'TEST Pepper',   N'Cat',    N'Ragdoll'          UNION ALL
    SELECT N'TEST Kevin Reyes',      N'TEST Rocky',    N'Dog',    N'Beagle'
)
INSERT INTO dbo.Pets (CustomerId, Name, Species, Breed)
SELECT cu.CustomerId, p.Name, p.Species, p.Breed
FROM p
JOIN dbo.Customers cu ON cu.FullName = p.Owner
WHERE NOT EXISTS (SELECT 1 FROM dbo.Pets x WHERE x.CustomerId = cu.CustomerId AND x.Name = p.Name);
GO

SELECT N'Customer' AS What, FullName AS Name FROM dbo.Customers WHERE FullName LIKE N'TEST%'
UNION ALL
SELECT N'Pet', Name FROM dbo.Pets WHERE Name LIKE N'TEST%';
GO
