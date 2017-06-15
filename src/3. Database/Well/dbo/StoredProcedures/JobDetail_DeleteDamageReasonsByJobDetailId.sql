CREATE PROCEDURE [dbo].[JobDetail_DeleteDamageReasonsByJobDetailId]
	@JobDetailId int
AS
BEGIN
	UPDATE JobDetailDamage 
	SET DateDeleted = GETDATE()
	WHERE JobDetailId = @JobDetailId
END
