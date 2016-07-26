CREATE PROCEDURE [dbo].[JobDetail_GetById]
	@Id INT

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
  FROM [dbo].[JobDetail]
  WHERE [Id] = @Id
END
