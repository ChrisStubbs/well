CREATE PROCEDURE [dbo].[JobDetail_Get]
	@Id INT = null,
	@JobId int = null,
	@LineNumber int = null
AS

	DECLARE @JobDetailIdsTable TABLE ( JobDetailId INT )

	Insert into @JobDetailIdsTable
		SELECT	[Id]
		FROM	[dbo].[JobDetail]
		WHERE	DateDeleted IS NULL and  
				(Id = @Id or @Id is null) and 
				([JobId] = @JobId or @JobId is null) and 
				([LineNumber] = @LineNumber or @LineNumber is null)

	SELECT  jd.[Id]
            ,jd.[LineNumber]
		   ,jd.[OriginalDespatchQty]
		   ,jd.[ProdDesc]
		   ,jd.[OrderedQty]
		   ,jd.DeliveredQty
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
           ,jd.[ShortsStatus]
           ,jd.[CreatedBy]
           ,jd.[DateCreated]
           ,jd.[UpdatedBy]
           ,jd.[DateUpdated]
		   ,jd.[DateDeleted]
           ,jd.[Version]
		   ,jd.[UpliftAction_Id] AS UpliftAction
  FROM [dbo].[JobDetail] jd
  INNER JOIN @JobDetailIdsTable Ids ON Ids.JobDetailId = jd.Id

  SELECT jdd.[Id]
      ,jdd.[JobDetailId]
      ,[Qty]
      ,[JobDetailSourceId]
	  ,[JobDetailReasonId]
	  ,[DamageActionId]
	  ,[DamageStatus]
	  ,[PdaReasonDescription]
      ,[CreatedBy]
      ,[DateCreated]
      ,[UpdatedBy]
      ,[DateUpdated]
      ,[Version]
  FROM [dbo].[JobDetailDamage] jdd
  INNER JOIN @JobDetailIdsTable Ids ON Ids.JobDetailId = jdd.JobDetailId
  WHERE	DateDeleted IS NULL

RETURN 0