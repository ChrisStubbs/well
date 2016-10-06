CREATE PROCEDURE [dbo].[CleanPreferencesGetAll]
AS
BEGIN
SELECT
	[Id], [Days], [CreatedBy], [CreatedDate], [LastUpdatedBy], [LastUpdatedDate]
FROM
	 [dbo].[CleanPreference] 
		   
END