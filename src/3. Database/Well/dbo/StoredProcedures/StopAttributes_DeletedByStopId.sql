CREATE PROCEDURE [dbo].[StopAttributes_DeletedByStopId]
	@StopId int 
AS
	DELETE FROM StopAttribute WHERE StopId = @StopId
RETURN 0
