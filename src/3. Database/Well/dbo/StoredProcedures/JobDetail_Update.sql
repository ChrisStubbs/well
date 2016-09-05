CREATE PROCEDURE [dbo].[JobDetail_Update]
	@Id						Int,
	@OriginalDespatchQty	INT,
	@OrderedQty				INT,
	@ShortQty				INT,
	@JobDetailStatusId		INT,
	@IsDeleted				Bit,
	@UpdatedBy				VARCHAR(50),
	@DateUpdated			Datetime

AS
BEGIN
	SET NOCOUNT ON;

UPDATE [dbo].[JobDetail]
   SET 
      [OriginalDespatchQty] = @OriginalDespatchQty  
      ,[OrderedQty] = @OrderedQty
      ,[ShortQty] = @ShortQty    
      ,[JobDetailStatusId] = @JobDetailStatusId
      ,[IsDeleted] = @IsDeleted
      ,[UpdatedBy] = @UpdatedBy
      ,[DateUpdated] = @DateUpdated
 WHERE [Id] = @Id
END
