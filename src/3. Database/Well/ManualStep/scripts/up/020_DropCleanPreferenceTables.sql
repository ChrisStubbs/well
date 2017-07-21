IF OBJECT_ID('dbo.CleanPreference', 'U') IS NOT NULL 
BEGIN
DROP TABLE dbo.CleanPreference;
END

IF OBJECT_ID('dbo.CleanPreferenceToBranch', 'U') IS NOT NULL 
BEGIN
DROP TABLE dbo.CleanPreferenceToBranch;
END

IF OBJECT_ID('dbo.CleanPreference_Audit', 'U') IS NOT NULL 
BEGIN
DROP TABLE dbo.CleanPreference_Audit;
END