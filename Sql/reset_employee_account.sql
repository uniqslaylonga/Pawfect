-- Resets the test employee login. Run in SSMS on the same database the app uses.
--   Employee  employee@pawfect.test   Employee123!

USE Pawfect;
GO

UPDATE dbo.Users
SET PasswordHash = N'kac0ImeEBkWy0KzpPUQ5Kw==.pT77ml8c0xy78CjunaX2BVo301tj7BVlqxGxP5ckVs=',
    IsActive = 1,
    Role = N'Employee'
WHERE Email = N'employee@pawfect.test';
GO

SELECT Email, Role, IsActive, LEN(PasswordHash) AS HashLen
FROM dbo.Users
WHERE Email = N'employee@pawfect.test';
GO