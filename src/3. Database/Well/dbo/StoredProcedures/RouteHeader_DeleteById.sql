CREATE PROCEDURE [dbo].[RouteHeader_DeleteById]
	@RouteheaderId int
AS

BEGIN
	UPDATE RouteHeader 
	SET DateDeleted = GETDATE()
	WHERE Id = @RouteheaderId
END
