CREATE PROCEDURE [dbo].[StopAttributes_DeletedByStopId]
	@StopId int,
	@IsSoftDelete bit
AS

	IF @IsSoftDelete = 1
	BEGIN
		UPDATE StopAttribute 
		SET IsDeleted = 1
		WHERE StopId = @StopId
	END
	ELSE
	BEGIN
		DELETE FROM StopAttribute WHERE StopId = @StopId
	END


	
RETURN 0
