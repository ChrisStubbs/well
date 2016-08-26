CREATE PROCEDURE [dbo].[JobDetail_GetByBarcodeAndProdDesc]
	@Barcode	NVARCHAR(60),
	@JobId		INT
AS
	SELECT TOP 1000 [Id]
      ,[LineNumber]
      ,[Barcode]
      ,[OriginalDespatchQty]
      ,[ProdDesc]
      ,[OrderedQty]
      ,[ShortQty]
      ,[SkuWeight]
      ,[SkuCube]
      ,[UnitMeasure]
      ,[TextField1]
      ,[TextField2]
      ,[TextField3]
      ,[TextField4]
      ,[TextField5]
      ,[SkuGoodsValue]
      ,[JobId]
	  ,[JobDetailStatusId]
	  ,[IsDeleted]
      ,[CreatedBy]
      ,[DateCreated]
      ,[UpdatedBy]
      ,[DateUpdated]
      ,[Version]
  FROM [dbo].[JobDetail]
  WHERE [Barcode] = @Barcode 
  AND JobId = @JobId
  and IsDeleted = 0
RETURN 0
