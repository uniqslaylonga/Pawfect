-- Queue statuses (Triage Pending, Admitted, In-Service, Sanitation Buffer, Ready for Pick-up, Completed).
-- The app applies this by itself the first time it connects; run it by hand only if that account can't alter tables.
-- Stored text -> label:  Pending = Triage Pending, Confirmed = Admitted, CheckedIn = In-Service,
--                        SanitationBuffer = Sanitation Buffer, ReadyForPickup = Ready for Pick-up, Completed = Completed.
-- Existing rows keep their text, so nothing needs converting.

IF COL_LENGTH(N'dbo.Appointments', N'HoldUntil') IS NULL
    ALTER TABLE dbo.Appointments ADD HoldUntil DATE NULL;           -- 14-day immunity: can't be admitted before this day
IF COL_LENGTH(N'dbo.Appointments', N'SanitationUntil') IS NULL
    ALTER TABLE dbo.Appointments ADD SanitationUntil DATETIME2 NULL; -- when the 30-minute sanitation buffer ends
GO

IF EXISTS (SELECT 1 FROM sys.check_constraints WHERE name = N'CK_Appointments_Status'
           AND parent_object_id = OBJECT_ID(N'dbo.Appointments'))
    ALTER TABLE dbo.Appointments DROP CONSTRAINT CK_Appointments_Status;
GO

ALTER TABLE dbo.Appointments ADD CONSTRAINT CK_Appointments_Status
    CHECK (Status IN (N'Pending', N'Confirmed', N'CheckedIn', N'SanitationBuffer', N'ReadyForPickup', N'Rescheduled', N'Cancelled', N'Completed'));
GO
