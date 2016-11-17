CREATE PROCEDURE [dbo].[JobDetail_Update]
	@Id						Int,
	@OriginalDespatchQty	INT,
	@OrderedQty				INT,
	@DeliveredQty			DECIMAL(8,3) ,
	@ShortQty				INT,
	@JobDetailStatusId		INT,
	@IsDeleted				BIT,
	@UpdatedBy				VARCHAR(50),
	@DateUpdated			DATETIME,
	@LineStatus			    VARCHAR(20),
	@SubOuterDamageQty		INT = 0

AS
BEGIN
	SET NOCOUNT ON;

UPDATE [dbo].[JobDetail]
   SET 
      [OriginalDespatchQty] = @OriginalDespatchQty  
      ,[OrderedQty] = @OrderedQty
	  ,[DeliveredQty] = @DeliveredQty
      ,[ShortQty] = @ShortQty    
      ,[JobDetailStatusId] = @JobDetailStatusId
      ,[IsDeleted] = @IsDeleted
      ,[UpdatedBy] = @UpdatedBy
      ,[DateUpdated] = @DateUpdated
	  ,[LineStatus] = @LineStatus
	  ,[SubOuterDamageTotal] = @SubOuterDamageQty
 WHERE [Id] = @Id
END
