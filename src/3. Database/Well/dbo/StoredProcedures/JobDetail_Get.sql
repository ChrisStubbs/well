CREATE PROCEDURE [dbo].[JobDetail_Get]
	@Id INT = null,
	@JobId int = null,
	@LineNumber int = null
AS

	DECLARE @JobDetailIdsTable TABLE ( JobDetailId INT )

	Insert into @JobDetailIdsTable
		SELECT	[Id]
		FROM	[dbo].[JobDetail]
		WHERE	[IsDeleted] = 0 and  
				(Id = @Id or @Id is null) and 
				([JobId] = @JobId or @JobId is null) and 
				([LineNumber] = @LineNumber or @LineNumber is null)

	SELECT  jd.[Id]
            ,jd.[LineNumber]
		   ,jd.[OriginalDespatchQty]
		   ,jd.[ProdDesc]
		   ,jd.[OrderedQty]
           ,jd.[ShortQty]
           ,jd.[UnitMeasure]
           ,jd.[PHProductCode]
           ,jd.[PHProductType]
           ,jd.[PackSize]
           ,jd.[SingleOrOuter]
           ,jd.[SSCCBarcode]
		   ,jd.[SubOuterDamageTotal]
           ,jd.[SkuGoodsValue]
		   ,jd.[NetPrice]
           ,jd.[JobId]
           ,jd.[JobDetailStatusId]
           ,jd.[CreatedBy]
           ,jd.[DateCreated]
           ,jd.[UpdatedBy]
           ,jd.[DateUpdated]
		   ,jd.[IsDeleted]
           ,jd.[Version]
  FROM [dbo].[JobDetail] jd
  INNER JOIN @JobDetailIdsTable Ids ON Ids.JobDetailId = jd.Id

  SELECT jdd.[Id]
      ,jdd.[JobDetailId]
      ,[Qty]
      ,[DamageReasonsId]
      ,[CreatedBy]
      ,[DateCreated]
      ,[UpdatedBy]
      ,[DateUpdated]
      ,[Version]
  FROM [dbo].[JobDetailDamage] jdd
  INNER JOIN @JobDetailIdsTable Ids ON Ids.JobDetailId = jdd.JobDetailId
	
   
RETURN 0