CREATE PROCEDURE [dbo].[Account_DeleteByStopId]
	@StopId int
AS
BEGIN
	UPDATE Account 
	SET IsDeleted = 1
	WHERE StopId = @StopId
END