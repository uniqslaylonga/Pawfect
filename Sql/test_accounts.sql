-- TEST ACCOUNTS ONLY. Run setup.sql first, then this once in SSMS.
-- Delete these accounts before real use:
--     DELETE FROM dbo.Users WHERE Email LIKE N'%@pawfect.test';
--
--   Admin     admin@pawfect.test      Admin123!
--   Employee  employee@pawfect.test   Employee123!

USE Pawfect;
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Users WHERE Email = N'admin@pawfect.test')
    INSERT INTO dbo.Users (FullName, Email, PasswordHash, Role)
    VALUES (N'Test Admin', N'admin@pawfect.test', N'iA/Dzv4un6kMfp+4Ddgy5w==.ORmI/SLx8TqJJ+FRays5TVMpD3a6Yl4sCknp0UnW/MY=', N'Admin');

IF NOT EXISTS (SELECT 1 FROM dbo.Users WHERE Email = N'employee@pawfect.test')
    INSERT INTO dbo.Users (FullName, Email, PasswordHash, Role)
    VALUES (N'Test Employee', N'employee@pawfect.test', N'kac0ImeEBkWy0KzpPUQ5Kw==.pT77ml8c0xy78CjunaX2BVo301tj7/BVlqxGxP5ckVs=', N'Employee');

GO

SELECT FullName, Email, Role, IsActive FROM dbo.Users;
GO
