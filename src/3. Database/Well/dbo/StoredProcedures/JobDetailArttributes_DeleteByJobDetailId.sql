CREATE PROCEDURE [dbo].[JobDetailArttributes_DeleteByJobDetailId]
	@JobDetailId int 
AS
	DELETE FROM JobDetailAttribute WHERE [JobDetailId] = @JobDetailId
RETURN 0
