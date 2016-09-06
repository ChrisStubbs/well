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

  SELECT jda.[Id]
      ,[Code]
      ,[Value]
      ,jda.[JobDetailId] as AttributeId
      ,[CreatedBy]
      ,[DateCreated]
      ,[UpdatedBy]
      ,[DateUpdated]
      ,[Version]
  FROM [dbo].[JobDetailAttribute] jda
  INNER JOIN @JobDetailIdsTable Ids ON Ids.JobDetailId = jda.JobDetailId	
   
RETURN 0