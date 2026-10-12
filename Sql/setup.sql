-- Run this once in SQL Server Management Studio (SSMS).
-- Safe to run again: it only creates what is missing.

IF DB_ID(N'Pawfect') IS NULL
    CREATE DATABASE Pawfect;
GO

USE Pawfect;
GO

IF OBJECT_ID(N'dbo.Users', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Users
    (
        UserId       INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Users PRIMARY KEY,
        FullName     NVARCHAR(100) NOT NULL,
        Email        NVARCHAR(256) NOT NULL,
        PasswordHash NVARCHAR(200) NOT NULL,   -- salted PBKDF2 hash made by the app, never the password itself
        Role         NVARCHAR(10)  NOT NULL,
        IsActive     BIT           NOT NULL CONSTRAINT DF_Users_IsActive DEFAULT 1,
        CreatedAt    DATETIME2     NOT NULL CONSTRAINT DF_Users_CreatedAt DEFAULT SYSUTCDATETIME(),
        CONSTRAINT UQ_Users_Email UNIQUE (Email)
    );
END
GO

-- Allowed roles. This also upgrades an older copy of the table that used 'Staff'.
IF EXISTS (SELECT 1 FROM sys.check_constraints
           WHERE name = N'CK_Users_Role' AND parent_object_id = OBJECT_ID(N'dbo.Users'))
    ALTER TABLE dbo.Users DROP CONSTRAINT CK_Users_Role;
GO

UPDATE dbo.Users SET Role = N'Employee' WHERE Role = N'Staff';
GO

ALTER TABLE dbo.Users ADD CONSTRAINT CK_Users_Role CHECK (Role IN (N'Employee', N'Admin'));
GO
