CREATE PROCEDURE [dbo].[JobDetail_DeleteById]
	@JobDetailId int
AS
BEGIN
	UPDATE JobDetail 
	SET IsDeleted = 1
	WHERE Id = @JobDetailId
END
