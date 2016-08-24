CREATE PROCEDURE [dbo].[JobArttributes_DeleteBylId]
	@JobId int 
AS
	DELETE FROM JobAttribute WHERE [JobId] = @JobId
RETURN 0
