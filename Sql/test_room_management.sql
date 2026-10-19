-- OPTIONAL. Only for trying the Room Management page.
-- Run these first (once): setup.sql, employee_setup.sql, appointment_scheduling.sql
--
-- Creates 6 "TEST Room ..." rooms, 2 "TEST Groomer ..." groomers and a few appointments whose times are
-- relative to RIGHT NOW, so every status shows up on the page:
--
--   TEST Room In Use       -> In use        (Groomer: TEST Groomer Anna, ends in 40 min; another booking in 3 hours)
--   TEST Room Sanitizing   -> Sanitizing    (last groomer: TEST Groomer Ben, ready in 15 min)
--   TEST Room Upcoming     -> Vacant        (next booking in 2 hours)  -> Delete is BLOCKED (has an upcoming booking)
--   TEST Room Vacant       -> Vacant        (only a Completed booking yesterday) -> Delete works, history loses its room
--   TEST Room Empty        -> Vacant        (no bookings at all)       -> Delete works
--   TEST Room Inactive     -> Inactive      (switched off)
--
-- The times are fixed when the script runs, so run it again to reset them to "now".
-- Open Room Management, wait/refresh, and click Schedule on "TEST Room In Use" to see the sanitizing blocks.
-- Try again after ~40 minutes: In use turns into Sanitizing, then Vacant.
--
-- Remove everything again: run the block at the bottom of this file (after "CLEAN UP").

USE Pawfect;
GO

-- ---- a test customer, pet and 60-minute service (the same ones test_customer.sql makes)
IF NOT EXISTS (SELECT 1 FROM dbo.Customers WHERE FullName = N'TEST Customer')
    INSERT INTO dbo.Customers (FullName, Phone, Email, Address)
    VALUES (N'TEST Customer', N'0000-000-0000', N'customer@pawfect.test', N'Test address');

IF NOT EXISTS (SELECT 1 FROM dbo.Pets WHERE Name = N'TEST Pet')
    INSERT INTO dbo.Pets (CustomerId, Name, Species, Breed)
    SELECT CustomerId, N'TEST Pet', N'Dog', N'Test breed' FROM dbo.Customers WHERE FullName = N'TEST Customer';

IF NOT EXISTS (SELECT 1 FROM dbo.Services WHERE Name = N'TEST Service')
    INSERT INTO dbo.Services (Name, Category, Price) VALUES (N'TEST Service', N'Grooming', 100);

-- The page works out when a booking ends from the service length, so fix it at 60 minutes.
UPDATE dbo.Services SET DurationMinutes = 60 WHERE Name = N'TEST Service';
GO

-- ---- rooms and groomers
INSERT INTO dbo.Rooms (Name, IsActive)
SELECT v.Name, v.IsActive
FROM (VALUES (N'TEST Room In Use', 1), (N'TEST Room Sanitizing', 1), (N'TEST Room Upcoming', 1),
             (N'TEST Room Vacant', 1), (N'TEST Room Empty', 1), (N'TEST Room Inactive', 0)) AS v(Name, IsActive)
WHERE NOT EXISTS (SELECT 1 FROM dbo.Rooms r WHERE r.Name = v.Name);

INSERT INTO dbo.Groomers (Name)
SELECT v.Name
FROM (VALUES (N'TEST Groomer Anna'), (N'TEST Groomer Ben')) AS v(Name)
WHERE NOT EXISTS (SELECT 1 FROM dbo.Groomers g WHERE g.Name = v.Name);
GO

-- ---- appointments (deleted and recreated every run so the times stay relative to now)
DECLARE @now  DATETIME2 = SYSDATETIME();
DECLARE @cust INT = (SELECT CustomerId FROM dbo.Customers WHERE FullName = N'TEST Customer');
DECLARE @pet  INT = (SELECT TOP 1 PetId FROM dbo.Pets WHERE CustomerId = @cust ORDER BY PetId);
DECLARE @svc  INT = (SELECT TOP 1 ServiceId FROM dbo.Services WHERE Name = N'TEST Service');

DECLARE @anna INT = (SELECT GroomerId FROM dbo.Groomers WHERE Name = N'TEST Groomer Anna');
DECLARE @ben  INT = (SELECT GroomerId FROM dbo.Groomers WHERE Name = N'TEST Groomer Ben');

DECLARE @rInUse      INT = (SELECT RoomId FROM dbo.Rooms WHERE Name = N'TEST Room In Use');
DECLARE @rSanitizing INT = (SELECT RoomId FROM dbo.Rooms WHERE Name = N'TEST Room Sanitizing');
DECLARE @rUpcoming   INT = (SELECT RoomId FROM dbo.Rooms WHERE Name = N'TEST Room Upcoming');
DECLARE @rVacant     INT = (SELECT RoomId FROM dbo.Rooms WHERE Name = N'TEST Room Vacant');

DELETE FROM dbo.Appointments
WHERE RoomId IN (SELECT RoomId FROM dbo.Rooms WHERE Name LIKE N'TEST Room%');

INSERT INTO dbo.Appointments (CustomerId, PetId, ServiceId, StartTime, Status, GroomerId, RoomId)
VALUES
    -- In use: started 20 min ago, 60-min service -> ends in 40 min
    (@cust, @pet, @svc, DATEADD(MINUTE,  -20, @now), N'CheckedIn', @anna, @rInUse),
    -- ... and another booking later today, so the schedule shows more than one block
    (@cust, @pet, @svc, DATEADD(HOUR,      3, @now), N'Confirmed', @anna, @rInUse),
    -- Sanitizing: ended 15 min ago -> ready in 15 min
    (@cust, @pet, @svc, DATEADD(MINUTE,  -75, @now), N'CheckedIn', @ben,  @rSanitizing),
    -- Vacant, but with an upcoming booking (Delete is blocked)
    (@cust, @pet, @svc, DATEADD(HOUR,      2, @now), N'Confirmed', @ben,  @rUpcoming),
    -- Vacant, only history (Delete works; this appointment keeps existing but loses its room)
    (@cust, @pet, @svc, DATEADD(DAY,      -1, @now), N'Completed', @anna, @rVacant);
GO

-- ---- what the Room Management page should show right now
SELECT r.Name AS Room,
       CASE WHEN r.IsActive = 0 THEN N'Inactive'
            WHEN r.Name = N'TEST Room In Use' THEN N'In use'
            WHEN r.Name = N'TEST Room Sanitizing' THEN N'Sanitizing'
            ELSE N'Vacant' END AS [Expected status],
       (SELECT COUNT(*) FROM dbo.Appointments a WHERE a.RoomId = r.RoomId) AS Appointments
FROM dbo.Rooms r
WHERE r.Name LIKE N'TEST Room%'
ORDER BY r.Name;
GO


-- ======================================================================================== CLEAN UP
-- Select this block and run it to remove the test rooms, groomers and their appointments.
-- (Sql/remove_test_data.sql removes the TEST customer, pet and service.)
--
-- DELETE FROM dbo.Appointments WHERE RoomId IN (SELECT RoomId FROM dbo.Rooms WHERE Name LIKE N'TEST Room%')
--                                 OR GroomerId IN (SELECT GroomerId FROM dbo.Groomers WHERE Name LIKE N'TEST Groomer%');
-- DELETE FROM dbo.Rooms    WHERE Name LIKE N'TEST Room%';
-- DELETE FROM dbo.Groomers WHERE Name LIKE N'TEST Groomer%';
-- GO
