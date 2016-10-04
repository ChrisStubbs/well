CREATE PROCEDURE [dbo].[RouteIdsToRemoveGet]
AS
BEGIN
	select Id from Routes where IsDeleted = 0
END