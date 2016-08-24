CREATE PROCEDURE [dbo].[Routes_DeleteById]
	@RoutesId int 
AS
	DELETE FROM [Routes] WHERE Id = @RoutesId
RETURN 0
