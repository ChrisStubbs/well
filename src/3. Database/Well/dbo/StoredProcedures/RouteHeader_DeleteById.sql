CREATE PROCEDURE [dbo].[RouteHeader_DeleteById]
	@RouteheaderId int 
AS
	DELETE FROM RouteHeader WHERE Id = @RouteheaderId
RETURN 0
