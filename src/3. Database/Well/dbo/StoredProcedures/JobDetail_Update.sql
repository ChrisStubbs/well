CREATE PROCEDURE [dbo].[JobDetail_Update]
	@Id						Int,
	@DeliveredQty			DECIMAL(8,3) ,
	@ShortQty				INT,
	@ShortsStatus		INT,
	@JobDetailReasonId		TINYINT,
	@JobDetailSourceId		TINYINT,
	@ShortsActionId INT,
	@LineDeliveryStatus		VARCHAR(20),
	@SubOuterDamageQty		INT,
	@ProductCode			VARCHAR(60),
	@ProductDescription		VARCHAR(100),
	@OrderedQty				DECIMAL(8,3),
	@UnitMeasure			VARCHAR(50),
	@ProductType			VARCHAR(50),
	@PackSize				VARCHAR(50),
	@SingleOrOuter			VARCHAR(10),
	@Barcode				VARCHAR(50),
	@SkuGoodsValue			FLOAT,
	@UpdatedBy				VARCHAR(50),
	@DateUpdated			DATETIME,
	@OriginalDespatchQty	INT

AS
BEGIN
	SET NOCOUNT ON;

UPDATE 
		[dbo].[JobDetail]
   SET 
	   [DeliveredQty] = @DeliveredQty
      ,[ShortQty] = @ShortQty    
      ,[ShortsStatus] = @ShortsStatus
	  ,[JobDetailReasonId] = @JobDetailReasonId
	  ,[JobDetailSourceId] = @JobDetailSourceId
	  ,[ShortsActionId] = @ShortsActionId
	  ,[LineDeliveryStatus] = @LineDeliveryStatus
	  ,SubOuterDamageTotal = @SubOuterDamageQty
	  ,PHProductCode = @ProductCode
	  ,ProdDesc = @ProductDescription
	  ,OrderedQty = @OrderedQty
	  ,UnitMeasure = @UnitMeasure
	  ,PHProductType = @ProductType
	  ,PackSize = @PackSize
	  ,SingleOrOuter = @SingleOrOuter
	  ,TobaccoBagBarcode = @Barcode
	  ,SkuGoodsValue = @SkuGoodsValue
      ,[UpdatedBy] = @UpdatedBy
      ,[DateUpdated] = @DateUpdated
	  ,OriginalDespatchQty = @OriginalDespatchQty
 WHERE 
	[Id] = @Id
END
