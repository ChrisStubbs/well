CREATE PROCEDURE [dbo].[JobDetail_DeleteDamageReasonsByJobDetailId]
	@JobDetailId int
AS
BEGIN
	UPDATE JobDetailDamage 
	SET IsDeleted = 1
	WHERE JobDetailId = @JobDetailId
END
