-- OPTIONAL. Only for trying the employee screens. Run employee_setup.sql first.
-- Creates ONE clearly-labelled test customer with ONE test pet, plus ONE test service and ONE
-- test product (billing can't be tried without them; the admin side will normally add real ones).
-- Everything is named "TEST ..." so it is easy to spot. Safe to run again.
--
-- Remove it all again with Sql/remove_test_data.sql

USE Pawfect;
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Customers WHERE FullName = N'TEST Customer')
    INSERT INTO dbo.Customers (FullName, Phone, Email, Address)
    VALUES (N'TEST Customer', N'0000-000-0000', N'customer@pawfect.test', N'Test address');

IF NOT EXISTS (SELECT 1 FROM dbo.Pets WHERE Name = N'TEST Pet')
    INSERT INTO dbo.Pets (CustomerId, Name, Species, Breed)
    SELECT CustomerId, N'TEST Pet', N'Dog', N'Test breed' FROM dbo.Customers WHERE FullName = N'TEST Customer';

IF NOT EXISTS (SELECT 1 FROM dbo.Services WHERE Name = N'TEST Service')
    INSERT INTO dbo.Services (Name, Category, Price) VALUES (N'TEST Service', N'Grooming', 100);

IF NOT EXISTS (SELECT 1 FROM dbo.Products WHERE Code = N'TEST-001')
    INSERT INTO dbo.Products (Code, Name, Category, Price) VALUES (N'TEST-001', N'TEST Product', N'Supplies', 50);
GO

SELECT N'Customer' AS What, FullName AS Name FROM dbo.Customers WHERE FullName LIKE N'TEST%'
UNION ALL SELECT N'Pet', Name FROM dbo.Pets WHERE Name LIKE N'TEST%'
UNION ALL SELECT N'Service', Name FROM dbo.Services WHERE Name LIKE N'TEST%'
UNION ALL SELECT N'Product', Name FROM dbo.Products WHERE Name LIKE N'TEST%';
GO
