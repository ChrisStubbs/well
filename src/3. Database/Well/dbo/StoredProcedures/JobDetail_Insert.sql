CREATE PROCEDURE [dbo].[JobDetail_Insert]
	@LineNumber				INT,
	@OriginalDespatchQty	INT,
	@ProdDesc				VARCHAR(100),
	@OrderedQty				INT,
	@DeliveredQty				DECIMAL(8,3) ,
	@ShortQty				INT,
	@JobDetailReasonId		INT,
	@JobDetailSourceId		INT,
	@UnitMeasure			VARCHAR(20),
	@PHProductCode			VARCHAR(50),
	@PHProductType			VARCHAR(50) NULL,
	@PackSize				VARCHAR(50),
	@SingleOrOuter			VARCHAR(50)=NULL,
	@SSCCBarcode			VARCHAR(50)=NULL,
	@SubOuterDamageTotal	INT=NULL,
	@SkuGoodsValue			FLOAT,
	@NetPrice				FLOAT,
	@JobId					INT,
	@ShortsStatus			INT,
	@LineDeliveryStatus		VARCHAR(20),
	@IsHighValue			BIT = 0,
	--@DateLife				DATETIME,
	@CreatedBy				VARCHAR(50),
	@DateCreated			Datetime,
	@UpdatedBy				VARCHAR(50),
	@DateUpdated			Datetime,
	@UpliftActionId			TINYINT,
	@IsSubOuterQuantity		BIT

AS
BEGIN
	SET NOCOUNT ON;

INSERT INTO [dbo].[JobDetail]
           ([LineNumber]
		   ,[OriginalDespatchQty]
		   ,[ProdDesc]
		   ,[OrderedQty]
		   ,[DeliveredQty]
           ,[ShortQty]
           ,[UnitMeasure]
           ,[PHProductCode]
           ,[PHProductType]
           ,[PackSize]
           ,[SingleOrOuter]
           ,[SSCCBarcode]
		   ,[SubOuterDamageTotal]
           ,[SkuGoodsValue]
		   ,[NetPrice]
           ,[JobId]
           ,[ShortsStatus]
		   ,[LineDeliveryStatus]
		   ,[IsHighValue]
			,[JobDetailReasonId]
			,[JobDetailSourceId]
           ,[CreatedBy]
           ,[DateCreated]
           ,[UpdatedBy]
           ,[DateUpdated]
		   ,[UpliftAction_Id]
		   ,[IsSubOuterQuantity])
     VALUES
           (@LineNumber
		   ,@OriginalDespatchQty
		   ,@ProdDesc
           ,@OrderedQty
		   ,@DeliveredQty
           ,@ShortQty
           ,@UnitMeasure
           ,@PHProductCode     
		   ,@PHProductType
		   ,@PackSize			
		   ,@SingleOrOuter			
		   ,@SSCCBarcode			
		   ,@SubOuterDamageTotal	
           ,@SkuGoodsValue
		   ,@NetPrice
           ,@JobId
           ,@ShortsStatus
		   ,@LineDeliveryStatus
		   ,@IsHighValue
		  ,@JobDetailReasonId
		  ,@JobDetailSourceId
           ,@CreatedBy
           ,@DateCreated
           ,@UpdatedBy
           ,@DateUpdated
		   ,@UpliftActionId
		   ,@IsSubOuterQuantity)
		   
SELECT CAST(SCOPE_IDENTITY() as int);
END
