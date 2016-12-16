﻿CREATE PROCEDURE [dbo].[JobDetail_Update]
	@Id						Int,
	@DeliveredQty			DECIMAL(8,3) ,
	@ShortQty				INT,
	@JobDetailStatusId		INT,
	@JobDetailReasonId		TINYINT,
	@JobDetailSourceId		TINYINT,
	@LineDeliveryStatus		INT,
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
	@DateUpdated			DATETIME

AS
BEGIN
	SET NOCOUNT ON;

UPDATE 
		[dbo].[JobDetail]
   SET 
	   [DeliveredQty] = @DeliveredQty
      ,[ShortQty] = @ShortQty    
      ,[JobDetailStatusId] = @JobDetailStatusId
	  ,[JobDetailReasonId] = @JobDetailReasonId
	  ,[JobDetailSourceId] = @JobDetailSourceId
	  ,[LineDeliveryStatus] = @LineDeliveryStatus
	  ,SubOuterDamageTotal = @SubOuterDamageQty
	  ,PHProductCode = @ProductCode
	  ,ProdDesc = @ProductDescription
	  ,OrderedQty = @OrderedQty
	  ,UnitMeasure = @UnitMeasure
	  ,PHProductType = @ProductType
	  ,PackSize = @PackSize
	  ,SingleOrOuter = @SingleOrOuter
	  ,SSCCBarcode = @Barcode
	  ,SkuGoodsValue = @SkuGoodsValue
      ,[UpdatedBy] = @UpdatedBy
      ,[DateUpdated] = @DateUpdated
 WHERE 
	[Id] = @Id
END
