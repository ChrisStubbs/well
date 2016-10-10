﻿CREATE PROCEDURE [dbo].[JobDetail_GetByJobId]
	@JobId int

AS
	SELECT  [Id]
            ,[LineNumber]
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
