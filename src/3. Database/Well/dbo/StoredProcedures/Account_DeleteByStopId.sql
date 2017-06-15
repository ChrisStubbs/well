CREATE PROCEDURE [dbo].[Account_DeleteByStopId]
	@StopId int
AS
BEGIN
	UPDATE Account 
	SET DateDeleted = GETDATE()
	WHERE StopId = @StopId
END
