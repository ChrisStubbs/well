CREATE PROCEDURE [dbo].[Drivers_Get]
AS
	SELECT DISTINCT 
		DriverName AS [Key],
		[DriverName] AS Value
	FROM 
		RouteHeader
	WHERE
		DriverName is not null
