-- Links Employees.GroomerId to Groomers (one groomer, many employee rows).
-- Run this once on the Pawfect database (after employee_setup.sql). Safe to run again.

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_Employees_Groomers')
    ALTER TABLE dbo.Employees
    ADD CONSTRAINT FK_Employees_Groomers
    FOREIGN KEY (GroomerId) REFERENCES dbo.Groomers(GroomerId);
GO