CREATE PROCEDURE [dbo].[JobDetail_GetByJobId]
	@JobId int

AS
	SELECT  [Id]
            ,[LineNumber]
		   ,[OriginalDespatchQty]
		   ,[ProdDesc]
		   ,[OrderedQty]
           ,[ShortQty]
           ,[UnitMeasure]
           ,[PHProductCode]
           ,[PHProductType]
           ,[PackSize]
           ,[SingleOrOuter]
           ,[SSCCBarcode]
		   ,[SubOuterDamageTotal]
           ,[SkuGoodsValue]
           ,[JobId]
           ,[JobDetailStatusId]
           ,[CreatedBy]
           ,[DateCreated]
           ,[UpdatedBy]
           ,[DateUpdated]
		   ,[IsDeleted]
           ,[Version]
  FROM [dbo].[JobDetail]
  WHERE [JobId] = @JobId
RETURN 0
