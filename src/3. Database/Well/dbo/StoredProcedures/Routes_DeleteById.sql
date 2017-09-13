CREATE PROCEDURE [dbo].[Routes_DeleteById]
	@RoutesId int
AS
BEGIN
	UPDATE [Routes] 
	SET DateDeleted = GETDATE()
	WHERE Id = @RoutesId
END
