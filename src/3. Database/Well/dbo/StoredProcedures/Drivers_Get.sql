CREATE PROCEDURE [dbo].[Drivers_Get]
AS
	SELECT DISTINCT 
		DriverName AS [Key],
		[DriverName] AS Value
	FROM 
		RouteHeader
	WHERE 
		DriverName IS NOT NULL
		AND LTRIM(DriverName) != ''