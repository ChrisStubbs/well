CREATE PROCEDURE [dbo].[RouteHeader_DeleteById]
	@RouteheaderId int
AS

BEGIN
	UPDATE RouteHeader 
	SET IsDeleted = 1
	WHERE Id = @RouteheaderId
END
