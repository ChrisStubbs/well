CREATE PROCEDURE [dbo].[JobArttributes_DeleteBylId]
	@JobId int,
	@IsSoftDelete bit
AS

	IF @IsSoftDelete = 1
	BEGIN
		UPDATE JobAttribute 
		SET IsDeleted = 1
		WHERE [JobId] = @JobId
	END
	ELSE
	BEGIN
		DELETE FROM JobAttribute WHERE [JobId] = @JobId
	END	
RETURN 0
