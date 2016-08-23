CREATE PROCEDURE [dbo].[RouteHeaderAttributes_DeleteByRouteheaderId]
	@RouteheaderId int 
AS
	DELETE FROM RouteHeaderAttribute WHERE RouteHeaderId = @RouteheaderId
RETURN 0
