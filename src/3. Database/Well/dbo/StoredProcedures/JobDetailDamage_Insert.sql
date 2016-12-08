CREATE PROCEDURE [dbo].[JobDetailDamage_Insert]
	@JobDetailId			INT,
	@Qty					decimal(7,3),
	@CreatedBy				VARCHAR(50),
	@DateCreated			Datetime,
	@UpdatedBy				VARCHAR(50),
	@DateUpdated			Datetime

AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO [dbo].[JobDetailDamage]
           ([JobDetailId]
           ,[Qty]
           ,[CreatedBy]
           ,[DateCreated]
           ,[UpdatedBy]
           ,[DateUpdated])
     VALUES
           (@JobDetailId
           ,@Qty
           ,@CreatedBy
           ,@DateCreated
           ,@UpdatedBy
           ,@DateUpdated)
		   
SELECT CAST(SCOPE_IDENTITY() as int);
END
