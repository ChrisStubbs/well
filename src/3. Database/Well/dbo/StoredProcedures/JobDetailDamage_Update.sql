CREATE PROCEDURE [dbo].[JobDetailDamage_Update]
	@Id						Int,
	@JobDetailId			INT,
	@Qty					INT,
	@JobDetailSourceId		tinyint,
	@JobDetailReasonId		tinyint,
	@DamageStatus			INT,
	@DamageActionId			int,
	@UpdatedBy				VARCHAR(50),
	@DateUpdated			Datetime

AS
BEGIN
	SET NOCOUNT ON;

	UPDATE 
		[dbo].[JobDetailDamage]
	SET 
		[JobDetailId] = @JobDetailId
		,[Qty] = @Qty
		,JobDetailSourceId = @JobDetailSourceId
		,JobDetailReasonId = @JobDetailReasonId
		,[DamageStatus] = @DamageStatus
		,DamageActionId = @DamageActionId
		,[UpdatedBy] = @UpdatedBy
		,[DateUpdated] = @DateUpdated
	WHERE 
		Id = @Id		   

END
