CREATE PROCEDURE [dbo].[DeliveryLines_GetByJobId]
	@JobId INT

AS
BEGIN
	SELECT jd.[Id]
		,jd.JobId
		,jd.[LineNumber] AS [LineNo]
	    ,jd.[PHProductCode] AS ProductCode
		,jd.[ProdDesc] AS ProductDescription
		,jd.[SkuGoodsValue] AS [Value]
		,jd.[OriginalDespatchQty] AS InvoicedQuantity
		,jd.[ShortQty] AS ShortQuantity
	FROM [dbo].[JobDetail] jd
	WHERE jd.JobId = @JobId and jd.IsDeleted = 0

	SELECT jdd.[JobDetailId]
		,jdd.[Qty] as Quantity
		,jdd.[DamageReasonsId] as Reason
	From [dbo].[JobDetailDamage] jdd
	inner join [dbo].[JobDetail] jd on jdd.JobDetailId = jd.Id	
	WHERE jd.JobId = @JobId

	SELECT a.Id
		,a.[JobDetailId]
		,a.[Quantity]
		,a.ActionId as [Action]
		,a.StatusId as [Status]
		,a.[CreatedBy]
		,a.[DateCreated]
		,a.[UpdatedBy]
		,a.[DateUpdated]
		,a.[Version]
	From [dbo].[JobDetailAction] a
	inner join [dbo].[JobDetail] jd on a.JobDetailId = jd.Id	
	WHERE jd.JobId = @JobId

END
