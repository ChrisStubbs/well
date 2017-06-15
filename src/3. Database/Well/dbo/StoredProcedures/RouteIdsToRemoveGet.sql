CREATE PROCEDURE [dbo].[RouteIdsToRemoveGet]
AS
BEGIN
	select Id 
	from Routes 
	where DateDeleted IS NULL
END
