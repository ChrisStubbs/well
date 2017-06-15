CREATE PROCEDURE [dbo].[DeliveryLines_GetByJobId]
	@JobId INT

AS
BEGIN
	SELECT	jd.[Id] as JobDetailId
			,jd.JobId
			,jd.[LineNumber] AS [LineNo]
			,jd.[PHProductCode] AS ProductCode
			,jd.[ProdDesc] AS ProductDescription
			,jd.[SkuGoodsValue] AS [Value]
			,jd.[OriginalDespatchQty] AS InvoicedQuantity
			,jd.[ShortQty] AS ShortQuantity
			,jd.[DeliveredQty] As DeliveredQuantity
			,jd.LineDeliveryStatus
			,jd.JobDetailReasonId
			,jd.JobDetailSourceId
			,jd.ShortsActionId
			,jd.IsHighValue
	FROM	[dbo].[JobDetail] jd
	WHERE	jd.JobId = @JobId and jd.DateDeleted IS NULL

	SELECT	jdd.[JobDetailId]
			,jdd.[Qty] as Quantity
			,jdd.JobDetailSourceId
			,jdd.JobDetailReasonId
			,jdd.DamageActionId
	From 	[dbo].[JobDetailDamage] jdd
			inner join [dbo].[JobDetail] jd on jdd.JobDetailId = jd.Id	
	WHERE 	jd.JobId = @JobId 
			AND jd.DateDeleted IS NULL

END
