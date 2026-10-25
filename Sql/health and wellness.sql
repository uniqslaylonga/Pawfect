UPDATE dbo.Services
SET Category = N'Health & Wellness'
WHERE Name LIKE N'%flea%' OR Name LIKE N'%tick%';