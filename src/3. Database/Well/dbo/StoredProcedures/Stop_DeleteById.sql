CREATE PROCEDURE [dbo].[Stop_DeleteById]
	@Id int,
	@IsSoftDelete bit
AS

	IF @IsSoftDelete = 1
	BEGIN
		UPDATE [Stop] 
		SET IsDeleted = 1
		WHERE Id = @Id
	END
	ELSE
	BEGIN
		DELETE FROM [Stop] WHERE Id = @Id
	END


	
RETURN 0
