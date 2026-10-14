-- Queue tickets: every appointment request gets a queue number (A01, A02, ... restarting each day).
-- Run this once (after employee_setup.sql and appointment_scheduling.sql). Safe to run again.

IF COL_LENGTH(N'dbo.Appointments', N'QueueNumber') IS NULL
    ALTER TABLE dbo.Appointments ADD QueueNumber NVARCHAR(10) NULL;
GO
