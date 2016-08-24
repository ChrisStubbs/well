CREATE PROCEDURE [dbo].[Job_DeleteById]
	@JobId int 
AS
	DELETE FROM Job WHERE Id = @JobId
RETURN 0
