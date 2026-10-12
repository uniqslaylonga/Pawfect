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

ALTER TABLE dbo.Users ADD CONSTRAINT CK_Users_Role CHECK (Role IN (N'Employee', N'Admin', N'Customer'));
GO

USE Pawfect;
GO

IF OBJECT_ID(N'dbo.Services', N'U') IS NOT NULL
   AND NOT EXISTS (SELECT 1 FROM dbo.Services)
    DROP TABLE dbo.Services;
GO

IF OBJECT_ID(N'dbo.Services', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Services
    (
        ServiceId       INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Services PRIMARY KEY,
        Name            NVARCHAR(100) NOT NULL,
        Category        NVARCHAR(50)  NOT NULL,
        Size            NVARCHAR(10)  NOT NULL,
        Description     NVARCHAR(300) NULL,
        Price           DECIMAL(10,2) NOT NULL CONSTRAINT CK_Services_Price CHECK (Price >= 0),
        DurationMinutes INT           NOT NULL CONSTRAINT DF_Services_Duration DEFAULT 60,
        IsActive        BIT           NOT NULL CONSTRAINT DF_Services_IsActive DEFAULT 1,
        CreatedAt       DATETIME2     NOT NULL CONSTRAINT DF_Services_CreatedAt DEFAULT SYSUTCDATETIME(),
        CONSTRAINT CK_Services_Size CHECK (Size IN (N'Small', N'Medium', N'Large', N'Giant', N'All')),
        CONSTRAINT UQ_Services_Name_Size UNIQUE (Name, Size)
    );
END
GO
