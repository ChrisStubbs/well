CREATE PROCEDURE [dbo].[JobDetailDamage_Update]
	@Id						Int,
	@JobDetailId			INT,
	@Qty					decimal(7,3),
	@JobDetailSource		tinyint,
	@JobDetailReason		tinyint,
	@UpdatedBy				VARCHAR(50),
	@DateUpdated			Datetime

AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[JobDetailDamage]
		SET [JobDetailId] = @JobDetailId
			,[Qty] = @Qty
			,JobDetailSource = @JobDetailSource
			,JobDetailReason = @JobDetailReason
			,[UpdatedBy] = @UpdatedBy
			,[DateUpdated] = @DateUpdated
		WHERE Id = @Id		   

END
