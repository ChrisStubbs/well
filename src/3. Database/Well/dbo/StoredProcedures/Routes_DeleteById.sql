CREATE PROCEDURE [dbo].[Routes_DeleteById]
	@RoutesId int,
	@IsSoftDelete bit
AS

	IF @IsSoftDelete = 1
	BEGIN
		UPDATE [Routes] 
		SET IsDeleted = 1
		WHERE Id = @RoutesId
	END
	ELSE
	BEGIN
		DELETE FROM [Routes] WHERE Id = @RoutesId
	END
		
RETURN 0
