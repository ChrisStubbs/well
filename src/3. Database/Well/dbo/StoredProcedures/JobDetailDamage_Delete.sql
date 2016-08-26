CREATE PROCEDURE [dbo].[JobDetailDamage_Delete]
	@JobDetailId			INT
AS
BEGIN
	SET NOCOUNT ON;

	Delete from [dbo].[JobDetailDamage] WHERE [JobDetailId] = @JobDetailId		   

END
