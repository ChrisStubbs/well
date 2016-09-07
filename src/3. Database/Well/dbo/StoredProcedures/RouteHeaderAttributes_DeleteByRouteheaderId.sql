CREATE PROCEDURE [dbo].[RouteHeaderAttributes_DeleteByRouteheaderId]
	@RouteheaderId int,
	@IsSoftDelete bit
AS

	IF @IsSoftDelete = 1
	BEGIN
		UPDATE RouteHeaderAttribute 
		SET IsDeleted = 1
		WHERE [RouteHeaderId] = @RouteheaderId
	END
	ELSE
	BEGIN
		DELETE FROM RouteHeaderAttribute WHERE RouteHeaderId = @RouteheaderId
	END

	
RETURN 0
