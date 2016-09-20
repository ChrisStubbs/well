CREATE PROCEDURE [dbo].[JobDetailDamage_Update]
	@Id						Int,
	@JobDetailId			INT,
	@Qty					decimal(7,3),
	@DamageReasonsId		tinyint,
	@DamageSourceId			tinyint,
	@UpdatedBy				VARCHAR(50),
	@DateUpdated			Datetime

AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[JobDetailDamage]
		SET [JobDetailId] = @JobDetailId
			,[Qty] = @Qty
			,[DamageReasonsId] = @DamageReasonsId
			,[DamageSourceId] = @DamageSourceId
			,[UpdatedBy] = @UpdatedBy
			,[DateUpdated] = @DateUpdated
		WHERE Id = @Id		   

END
