CREATE PROCEDURE [dbo].[Routes_DeleteById]
	@RoutesId int
AS
BEGIN
	UPDATE [Routes] 
	SET IsDeleted = 1
	WHERE Id = @RoutesId
END
