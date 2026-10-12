-- Deletes everything created by test_customer.sql AND any appointments/sales made with it.
USE Pawfect;
GO
DECLARE @cust TABLE (Id INT);
INSERT @cust SELECT CustomerId FROM dbo.Customers WHERE FullName LIKE N'TEST%';

DELETE l FROM dbo.TransactionLines l
  JOIN dbo.Transactions t ON t.TransactionId = l.TransactionId
 WHERE t.CustomerId IN (SELECT Id FROM @cust)
    OR l.ServiceId IN (SELECT ServiceId FROM dbo.Services WHERE Name LIKE N'TEST%')
    OR l.ProductId IN (SELECT ProductId FROM dbo.Products WHERE Name LIKE N'TEST%');

DELETE FROM dbo.Transactions WHERE CustomerId IN (SELECT Id FROM @cust)
   OR TransactionId NOT IN (SELECT TransactionId FROM dbo.TransactionLines);

DELETE FROM dbo.Appointments WHERE CustomerId IN (SELECT Id FROM @cust);
DELETE FROM dbo.Pets WHERE CustomerId IN (SELECT Id FROM @cust);
DELETE FROM dbo.Customers WHERE CustomerId IN (SELECT Id FROM @cust);
DELETE FROM dbo.Services WHERE Name LIKE N'TEST%';
DELETE FROM dbo.Products WHERE Name LIKE N'TEST%';
GO
