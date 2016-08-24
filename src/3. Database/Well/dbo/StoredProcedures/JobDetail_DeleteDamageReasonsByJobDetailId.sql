CREATE PROCEDURE [dbo].[JobDetail_DeleteDamageReasonsByJobDetailId]
	@JobDetailId int 
AS
	DELETE FROM JobDetailDamage WHERE JobDetailId = @JobDetailId
RETURN 0
