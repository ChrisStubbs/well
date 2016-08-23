CREATE PROCEDURE [dbo].[JobDetail_DeleteById]
	@JobDetailId int 
AS
	DELETE FROM JobDetail WHERE Id = @JobDetailId
RETURN 0
