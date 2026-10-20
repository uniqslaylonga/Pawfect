-- Audit trail table. The app also creates this by itself the first time it runs, so running this file is optional.
IF OBJECT_ID(N'dbo.AuditLog', N'U') IS NULL
    CREATE TABLE dbo.AuditLog
    (
        AuditId    INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_AuditLog PRIMARY KEY,
        ActorName  NVARCHAR(100) NOT NULL,
        ActorEmail NVARCHAR(256) NULL,
        Action     NVARCHAR(50)  NOT NULL,
        Details    NVARCHAR(500) NOT NULL CONSTRAINT DF_AuditLog_Details DEFAULT N'',
        CreatedAt  DATETIME2     NOT NULL CONSTRAINT DF_AuditLog_CreatedAt DEFAULT SYSUTCDATETIME()
    );
GO
