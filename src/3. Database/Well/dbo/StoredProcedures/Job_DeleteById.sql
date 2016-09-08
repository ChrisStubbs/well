CREATE PROCEDURE [dbo].[Job_DeleteById]
	@JobId int,
	@IsSoftDelete bit
AS

	IF @IsSoftDelete = 1
	BEGIN
		UPDATE Job 
		SET IsDeleted = 1
		WHERE Id = @JobId
	END
	ELSE
	BEGIN
		DELETE FROM Job WHERE Id = @JobId
	END
	
RETURN 0
