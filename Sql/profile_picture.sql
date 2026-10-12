-- Optional: adds the profile-picture column to Users.
-- The app also does this automatically the first time someone opens their profile picture,
-- so you only need to run this if the app's database login can't alter tables.
USE Pawfect;
GO

IF COL_LENGTH(N'dbo.Users', N'AvatarImage') IS NULL
    ALTER TABLE dbo.Users ADD AvatarImage VARBINARY(MAX) NULL;
GO
