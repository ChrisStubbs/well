CREATE PROCEDURE [dbo].[JobDetail_DeleteById]
	@JobDetailId int
AS

BEGIN
	UPDATE JobDetail 
	SET DateDeleted = GETDATE()
	WHERE Id = @JobDetailId
END
