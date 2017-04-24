CREATE PROCEDURE [dbo].[JobDetailDamage_Insert]
	@JobDetailId			INT,
    @JobDetailSourceId		TINYINT,
	@JobDetailReasonId 		TINYINT,
	@DamageActionId			INT,
	@DamageStatus			INT,
	@Qty					INT,
	@PdaReasonDescription   VARCHAR(50),
	@CreatedBy				VARCHAR(50),
	@DateCreated			Datetime,
	@UpdatedBy				VARCHAR(50),
	@DateUpdated			Datetime

AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO [dbo].[JobDetailDamage]
           ([JobDetailId]
		   ,JobDetailSourceId
		   ,JobDetailReasonId
		   ,DamageActionId
		   ,[DamageStatus]
           ,[Qty]
		   ,[PdaReasonDescription]
           ,[CreatedBy]
           ,[DateCreated]
           ,[UpdatedBy]
           ,[DateUpdated])
     VALUES
           (@JobDetailId
		   ,@JobDetailSourceId
		   ,@JobDetailReasonId
		   ,@DamageActionId
		   ,@DamageStatus
           ,@Qty
		   ,@PdaReasonDescription
           ,@CreatedBy
           ,@DateCreated
           ,@UpdatedBy
           ,@DateUpdated)
		   
SELECT CAST(SCOPE_IDENTITY() as int);
END
