CREATE PROCEDURE [dbo].[JobDetail_DeleteById]
	@JobDetailId int,
	@IsSoftDelete bit
AS

	IF @IsSoftDelete = 1
	BEGIN
		UPDATE JobDetail 
		SET IsDeleted = 1
		WHERE Id = @JobDetailId
	END
	ELSE
	BEGIN
		DELETE FROM JobDetail WHERE Id = @JobDetailId
	END

	
RETURN 0
