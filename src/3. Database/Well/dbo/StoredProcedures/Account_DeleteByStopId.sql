CREATE PROCEDURE [dbo].[Account_DeleteByStopId]
	@StopId int,
	@IsSoftDelete bit
AS
	IF @IsSoftDelete = 1
	BEGIN
		UPDATE Account 
		SET IsDeleted = 1
		WHERE StopId = @StopId
	END
	ELSE
	BEGIN
		DELETE FROM Account WHERE StopId = @StopId
	END
RETURN 0
