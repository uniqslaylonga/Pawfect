-- Employee-side tables (customers, pets, services, products, appointments, sales).
-- Run once in SSMS AFTER setup.sql. Safe to run again: it only creates what is missing.
-- This script inserts NO data. Every table starts empty.

USE Pawfect;
GO

IF OBJECT_ID(N'dbo.Customers', N'U') IS NULL
CREATE TABLE dbo.Customers
(
    CustomerId INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Customers PRIMARY KEY,
    FullName   NVARCHAR(100) NOT NULL,
    Phone      NVARCHAR(30)  NOT NULL CONSTRAINT DF_Customers_Phone DEFAULT N'',
    Email      NVARCHAR(256) NOT NULL CONSTRAINT DF_Customers_Email DEFAULT N'',
    Address    NVARCHAR(200) NOT NULL CONSTRAINT DF_Customers_Address DEFAULT N'',
    IsActive   BIT           NOT NULL CONSTRAINT DF_Customers_IsActive DEFAULT 1,
    CreatedAt  DATETIME2     NOT NULL CONSTRAINT DF_Customers_CreatedAt DEFAULT SYSDATETIME()
);
GO

IF OBJECT_ID(N'dbo.Pets', N'U') IS NULL
CREATE TABLE dbo.Pets
(
    PetId      INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Pets PRIMARY KEY,
    CustomerId INT           NOT NULL CONSTRAINT FK_Pets_Customers REFERENCES dbo.Customers(CustomerId),
    Name       NVARCHAR(60)  NOT NULL,
    Species    NVARCHAR(30)  NOT NULL CONSTRAINT DF_Pets_Species DEFAULT N'',
    Breed      NVARCHAR(60)  NOT NULL CONSTRAINT DF_Pets_Breed DEFAULT N'',
    IsActive   BIT           NOT NULL CONSTRAINT DF_Pets_IsActive DEFAULT 1,
    CreatedAt  DATETIME2     NOT NULL CONSTRAINT DF_Pets_CreatedAt DEFAULT SYSDATETIME()
);
GO

-- Services and Products are filled by the admin side. The employee screens only read them.
IF OBJECT_ID(N'dbo.Services', N'U') IS NULL
CREATE TABLE dbo.Services
(
    ServiceId INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Services PRIMARY KEY,
    Name      NVARCHAR(100) NOT NULL,
    Category  NVARCHAR(40)  NOT NULL CONSTRAINT DF_Services_Category DEFAULT N'Grooming',  -- Grooming | Health & Wellness | Add-ons | Others
    Price     DECIMAL(10,2) NOT NULL,
    IsActive  BIT           NOT NULL CONSTRAINT DF_Services_IsActive DEFAULT 1,
    CONSTRAINT UQ_Services_Name UNIQUE (Name)
);
GO

IF OBJECT_ID(N'dbo.Products', N'U') IS NULL
CREATE TABLE dbo.Products
(
    ProductId INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Products PRIMARY KEY,
    Code      NVARCHAR(40)  NOT NULL,
    Name      NVARCHAR(100) NOT NULL,
    Category  NVARCHAR(40)  NOT NULL CONSTRAINT DF_Products_Category DEFAULT N'',
    Price     DECIMAL(10,2) NOT NULL,
    IsActive  BIT           NOT NULL CONSTRAINT DF_Products_IsActive DEFAULT 1,
    CONSTRAINT UQ_Products_Code UNIQUE (Code)
);
GO

IF OBJECT_ID(N'dbo.Appointments', N'U') IS NULL
CREATE TABLE dbo.Appointments
(
    AppointmentId INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Appointments PRIMARY KEY,
    CustomerId    INT          NOT NULL CONSTRAINT FK_Appointments_Customers REFERENCES dbo.Customers(CustomerId),
    PetId         INT          NOT NULL CONSTRAINT FK_Appointments_Pets REFERENCES dbo.Pets(PetId),
    ServiceId     INT          NOT NULL CONSTRAINT FK_Appointments_Services REFERENCES dbo.Services(ServiceId),
    StartTime     DATETIME2    NOT NULL,
    Status        NVARCHAR(20) NOT NULL CONSTRAINT DF_Appointments_Status DEFAULT N'Pending',
    CreatedByUserId INT        NULL CONSTRAINT FK_Appointments_Users REFERENCES dbo.Users(UserId),
    CreatedAt     DATETIME2    NOT NULL CONSTRAINT DF_Appointments_CreatedAt DEFAULT SYSDATETIME(),
    CONSTRAINT CK_Appointments_Status CHECK (Status IN (N'Pending', N'Confirmed', N'CheckedIn', N'Rescheduled', N'Cancelled', N'Completed'))
);
GO

-- Upgrade path: an older copy of the table without the 'Completed' status.
IF EXISTS (SELECT 1 FROM sys.check_constraints WHERE name = N'CK_Appointments_Status'
           AND definition NOT LIKE N'%Completed%')
BEGIN
    ALTER TABLE dbo.Appointments DROP CONSTRAINT CK_Appointments_Status;
    ALTER TABLE dbo.Appointments ADD CONSTRAINT CK_Appointments_Status
        CHECK (Status IN (N'Pending', N'Confirmed', N'CheckedIn', N'Rescheduled', N'Cancelled', N'Completed'));
END
GO

IF OBJECT_ID(N'dbo.Transactions', N'U') IS NULL
CREATE TABLE dbo.Transactions
(
    TransactionId  INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Transactions PRIMARY KEY,
    ReceiptNumber  NVARCHAR(30)  NOT NULL CONSTRAINT UQ_Transactions_Receipt UNIQUE,
    CreatedAt      DATETIME2     NOT NULL CONSTRAINT DF_Transactions_CreatedAt DEFAULT SYSDATETIME(),
    CustomerId     INT           NULL CONSTRAINT FK_Transactions_Customers REFERENCES dbo.Customers(CustomerId),   -- NULL = walk-in
    AppointmentId  INT           NULL CONSTRAINT FK_Transactions_Appointments REFERENCES dbo.Appointments(AppointmentId),
    CashierUserId  INT           NULL CONSTRAINT FK_Transactions_Users REFERENCES dbo.Users(UserId),
    Subtotal       DECIMAL(10,2) NOT NULL,
    Discount       DECIMAL(10,2) NOT NULL,
    Total          DECIMAL(10,2) NOT NULL,
    PaymentMethod  NVARCHAR(20)  NOT NULL,                 -- Cash | E-Wallet
    AmountReceived DECIMAL(10,2) NOT NULL,
    ChangeGiven    DECIMAL(10,2) NOT NULL,
    Notes          NVARCHAR(500) NOT NULL CONSTRAINT DF_Transactions_Notes DEFAULT N'',
    Status         NVARCHAR(20)  NOT NULL CONSTRAINT DF_Transactions_Status DEFAULT N'Completed',
    CONSTRAINT CK_Transactions_Status CHECK (Status IN (N'Completed', N'Pending', N'Refunded', N'Voided'))
);
GO

IF OBJECT_ID(N'dbo.TransactionLines', N'U') IS NULL
CREATE TABLE dbo.TransactionLines
(
    LineId        INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_TransactionLines PRIMARY KEY,
    TransactionId INT           NOT NULL CONSTRAINT FK_TransactionLines_Transactions REFERENCES dbo.Transactions(TransactionId),
    LineKind      NVARCHAR(10)  NOT NULL,                  -- Service | Product
    ServiceId     INT           NULL CONSTRAINT FK_TransactionLines_Services REFERENCES dbo.Services(ServiceId),
    ProductId     INT           NULL CONSTRAINT FK_TransactionLines_Products REFERENCES dbo.Products(ProductId),
    Name          NVARCHAR(100) NOT NULL,                  -- copied at sale time so old receipts never change
    Detail        NVARCHAR(100) NOT NULL CONSTRAINT DF_TransactionLines_Detail DEFAULT N'',
    UnitPrice     DECIMAL(10,2) NOT NULL,
    Quantity      INT           NOT NULL,
    CONSTRAINT CK_TransactionLines_Kind CHECK (LineKind IN (N'Service', N'Product'))
);
GO

-- Helpful lookups
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_Appointments_StartTime')
    CREATE INDEX IX_Appointments_StartTime ON dbo.Appointments (StartTime);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_Transactions_CreatedAt')
    CREATE INDEX IX_Transactions_CreatedAt ON dbo.Transactions (CreatedAt);
GO

SELECT t.name AS [Table], SUM(p.rows) AS [Rows]
FROM sys.tables t JOIN sys.partitions p ON p.object_id = t.object_id AND p.index_id IN (0,1)
WHERE t.name IN (N'Customers', N'Pets', N'Services', N'Products', N'Appointments', N'Transactions', N'TransactionLines')
GROUP BY t.name ORDER BY t.name;
GO
