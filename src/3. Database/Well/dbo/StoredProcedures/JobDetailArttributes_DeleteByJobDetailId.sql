CREATE PROCEDURE [dbo].[JobDetailArttributes_DeleteByJobDetailId]
	@JobDetailId int,
	@IsSoftDelete bit
AS

	IF @IsSoftDelete = 1
	BEGIN
		UPDATE JobDetailAttribute 
		SET IsDeleted = 1
		WHERE JobDetailId = @JobDetailId
	END
	ELSE
	BEGIN
		DELETE FROM JobDetailAttribute WHERE [JobDetailId] = @JobDetailId
	END

	
RETURN 0
