USE Pawfect;
GO

-- 1. Allow the roles Employee and Admin (fixes the old Staff rule)
IF EXISTS (SELECT 1 FROM sys.check_constraints
           WHERE name = N'CK_Users_Role' AND parent_object_id = OBJECT_ID(N'dbo.Users'))
    ALTER TABLE dbo.Users DROP CONSTRAINT CK_Users_Role;
GO
UPDATE dbo.Users SET Role = N'Employee' WHERE Role = N'Staff';
GO
ALTER TABLE dbo.Users ADD CONSTRAINT CK_Users_Role CHECK (Role IN (N'Employee', N'Admin'));
GO

-- 2. Recreate the two test accounts
DELETE FROM dbo.Users WHERE Email IN (N'admin@pawfect.test', N'employee@pawfect.test');

INSERT INTO dbo.Users (FullName, Email, PasswordHash, Role)
VALUES (N'Test Admin', N'admin@pawfect.test', N'iA/Dzv4un6kMfp+4Ddgy5w==.ORmI/SLx8TqJJ+FRays5TVMpD3a6Yl4sCknp0UnW/MY=', N'Admin');

INSERT INTO dbo.Users (FullName, Email, PasswordHash, Role)
VALUES (N'Test Employee', N'employee@pawfect.test', N'kac0ImeEBkWy0KzpPUQ5Kw==.pT77ml8c0xy78CjunaX2BVo301tj7/BVlqxGxP5ckVs=', N'Employee');
GO

-- 3. Check: you should see exactly these two rows
SELECT FullName, Email, Role, IsActive FROM dbo.Users;
GO
