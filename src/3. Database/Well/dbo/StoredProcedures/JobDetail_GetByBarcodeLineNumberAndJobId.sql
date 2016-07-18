CREATE PROCEDURE [dbo].[JobDetail_GetByBarcodeLineNumberAndJobId]
	@LineNumber				INT,
	@Barcode				VARCHAR(60),
	@JobId					INT

AS
BEGIN
SELECT [Id]
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
  FROM [Well].[dbo].[JobDetail]
  WHERE [LineNumber] = @LineNumber
  AND [Barcode] = @Barcode
  AND [JobId] = @JobId
END