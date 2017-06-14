CREATE PROCEDURE [dbo].[Job_DeleteById]
	@JobId int
AS

BEGIN
	UPDATE Job 
	SET DateDeleted = GETDATE()
	WHERE Id = @JobId
END
