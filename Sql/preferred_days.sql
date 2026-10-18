-- Preferred days: customers pick the days of the week that suit them (e.g. "Tue,Wed") when they request a grooming
-- appointment. Staff then set the exact date and time, but only on one of those days.
-- The app also adds this column by itself the first time it connects. Safe to run again.

IF COL_LENGTH(N'dbo.Appointments', N'PreferredDays') IS NULL
    ALTER TABLE dbo.Appointments ADD PreferredDays NVARCHAR(40) NULL;
GO
