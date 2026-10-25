USE Pawfect;
GO

IF OBJECT_ID(N'dbo.Groomers', N'U') IS NULL
CREATE TABLE dbo.Groomers
(
    GroomerId INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Groomers PRIMARY KEY,
    Name      NVARCHAR(80) NOT NULL,
    IsActive  BIT          NOT NULL CONSTRAINT DF_Groomers_IsActive DEFAULT 1
);
GO

IF OBJECT_ID(N'dbo.Rooms', N'U') IS NULL
CREATE TABLE dbo.Rooms
(
    RoomId   INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Rooms PRIMARY KEY,
    Name     NVARCHAR(60) NOT NULL,
    IsActive BIT          NOT NULL CONSTRAINT DF_Rooms_IsActive DEFAULT 1
);
GO

IF COL_LENGTH(N'dbo.Appointments', N'GroomerId') IS NULL
    ALTER TABLE dbo.Appointments ADD GroomerId INT NULL CONSTRAINT FK_Appointments_Groomers REFERENCES dbo.Groomers(GroomerId);
GO
IF COL_LENGTH(N'dbo.Appointments', N'RoomId') IS NULL
    ALTER TABLE dbo.Appointments ADD RoomId INT NULL CONSTRAINT FK_Appointments_Rooms REFERENCES dbo.Rooms(RoomId);
GO
IF COL_LENGTH(N'dbo.Appointments', N'Notes') IS NULL
    ALTER TABLE dbo.Appointments ADD Notes NVARCHAR(300) NULL;
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Groomers)
    INSERT INTO dbo.Groomers (Name) VALUES (N'Groomer 1'), (N'Groomer 2'), (N'Groomer 3');
IF NOT EXISTS (SELECT 1 FROM dbo.Rooms)
    INSERT INTO dbo.Rooms (Name) VALUES (N'Grooming Room 1'), (N'Grooming Room 2'), (N'Bath Area');
GO