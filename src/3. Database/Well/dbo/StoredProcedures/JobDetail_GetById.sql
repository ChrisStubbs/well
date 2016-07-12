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
      ,[SkuWeight]
      ,[SkuCube]
      ,[UnitMeasure]
      ,[TextField1]
      ,[TextField2]
      ,[TextField3]
      ,[TextField4]
      ,[SkuGoodsValue]
      ,[JobId]
  FROM [Well].[dbo].[JobDetail]
  WHERE [Id] = @Id
END
