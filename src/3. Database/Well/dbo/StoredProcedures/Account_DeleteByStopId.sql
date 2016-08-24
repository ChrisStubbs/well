CREATE PROCEDURE [dbo].[Account_DeleteByStopId]
	@StopId int 
AS
	DELETE FROM Account WHERE StopId = @StopId
RETURN 0
