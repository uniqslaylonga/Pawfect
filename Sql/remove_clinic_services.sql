-- Hides clinic-type services (3-in-1 vaccine, etc.) from Pawfect.
-- STEP 1: run only this SELECT and check the list. Add or remove names in the WHERE if needed.
SELECT ServiceId, Name, Category, IsActive
FROM dbo.Services
WHERE Name LIKE N'%vaccin%' OR Name LIKE N'%in-1%' OR Name LIKE N'%in 1%' OR Name LIKE N'%in1%'
   OR Name LIKE N'%rabies%' OR Name LIKE N'%deworm%';

-- STEP 2: when the list is right, run this. It deactivates them (same as "Deactivate" in Service Catalog),
-- so they disappear from booking and billing but old appointments and receipts that used them stay intact.
UPDATE dbo.Services SET IsActive = 0
WHERE Name LIKE N'%vaccin%' OR Name LIKE N'%in-1%' OR Name LIKE N'%in 1%' OR Name LIKE N'%in1%'
   OR Name LIKE N'%rabies%' OR Name LIKE N'%deworm%';
