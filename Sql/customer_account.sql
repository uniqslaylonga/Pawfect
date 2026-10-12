-- Adds the Customer role and ONE test customer login. Run AFTER setup.sql and employee_setup.sql.
-- Safe to run again.
--
--   Customer login:  customer@pawfect.test   Customer123!
--
-- Remove with:  DELETE FROM dbo.Users WHERE Email = N'customer@pawfect.test';  (after unlinking, see bottom)

USE Pawfect;
GO

-- 1. Allow the Customer role
IF EXISTS (SELECT 1 FROM sys.check_constraints WHERE name = N'CK_Users_Role' AND parent_object_id = OBJECT_ID(N'dbo.Users'))
    ALTER TABLE dbo.Users DROP CONSTRAINT CK_Users_Role;
GO
ALTER TABLE dbo.Users ADD CONSTRAINT CK_Users_Role CHECK (Role IN (N'Employee', N'Admin', N'Customer'));
GO

-- 2. Link a customer record to a login
IF COL_LENGTH(N'dbo.Customers', N'UserId') IS NULL
    ALTER TABLE dbo.Customers ADD UserId INT NULL;
GO
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_Customers_Users')
    ALTER TABLE dbo.Customers ADD CONSTRAINT FK_Customers_Users FOREIGN KEY (UserId) REFERENCES dbo.Users(UserId);
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'UX_Customers_UserId')
    CREATE UNIQUE INDEX UX_Customers_UserId ON dbo.Customers (UserId) WHERE UserId IS NOT NULL;
GO

-- 3. The test login
IF NOT EXISTS (SELECT 1 FROM dbo.Users WHERE Email = N'customer@pawfect.test')
    INSERT INTO dbo.Users (FullName, Email, PasswordHash, Role)
    VALUES (N'TEST Customer', N'customer@pawfect.test',
            N'Mtl/kcLuVp19iLUijPv5CQ==.w4PEAVgnhFcDXHH9/z1YtyprGT9FqIPliqFfqcRKrvw=', N'Customer');
GO

-- 4. Link it to the TEST customer (created here if test_customer.sql wasn't run)
IF NOT EXISTS (SELECT 1 FROM dbo.Customers WHERE FullName = N'TEST Customer')
    INSERT INTO dbo.Customers (FullName, Phone, Email, Address)
    VALUES (N'TEST Customer', N'0000-000-0000', N'customer@pawfect.test', N'Test address');

UPDATE dbo.Customers
SET UserId = (SELECT UserId FROM dbo.Users WHERE Email = N'customer@pawfect.test')
WHERE FullName = N'TEST Customer' AND UserId IS NULL;
GO

SELECT u.Email, u.Role, c.FullName AS LinkedCustomer
FROM dbo.Users u LEFT JOIN dbo.Customers c ON c.UserId = u.UserId
WHERE u.Role = N'Customer';
GO

-- To remove: UPDATE dbo.Customers SET UserId = NULL WHERE UserId IS NOT NULL;
--            DELETE FROM dbo.Users WHERE Email = N'customer@pawfect.test';
