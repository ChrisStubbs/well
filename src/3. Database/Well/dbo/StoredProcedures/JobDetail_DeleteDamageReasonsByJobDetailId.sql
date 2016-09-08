CREATE PROCEDURE [dbo].[JobDetail_DeleteDamageReasonsByJobDetailId]
	@JobDetailId int,
	@IsSoftDelete bit
AS

	IF @IsSoftDelete = 1
	BEGIN
		UPDATE JobDetailDamage 
		SET IsDeleted = 1
		WHERE JobDetailId = @JobDetailId
	END
	ELSE
	BEGIN
		DELETE FROM JobDetailDamage WHERE JobDetailId = @JobDetailId
	END


	
RETURN 0
