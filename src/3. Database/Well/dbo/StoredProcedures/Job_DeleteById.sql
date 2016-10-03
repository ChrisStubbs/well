CREATE PROCEDURE [dbo].[Job_DeleteById]
	@JobId int
AS

BEGIN
	UPDATE Job 
	SET IsDeleted = 1
	WHERE Id = @JobId
END
