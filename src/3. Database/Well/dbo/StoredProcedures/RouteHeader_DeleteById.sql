CREATE PROCEDURE [dbo].[RouteHeader_DeleteById]
	@RouteheaderId int,
	@IsSoftDelete bit
AS

	IF @IsSoftDelete = 1
	BEGIN
		UPDATE RouteHeader 
		SET IsDeleted = 1
		WHERE Id = @RouteheaderId
	END
	ELSE
	BEGIN
		DELETE FROM RouteHeader WHERE Id = @RouteheaderId
	END

	
RETURN 0
