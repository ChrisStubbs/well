CREATE PROCEDURE [dbo].[DeliveryLines_GetById]
	@Id INT

AS
BEGIN
	SELECT jd.[Id]
		,jd.[LineNumber] AS [LineNo]
	    ,jd.[Barcode] AS ProductCode
		,jd.[ProdDesc] AS ProductDescription
		,jd.[SkuGoodsValue] AS [Value]
		,jd.[OriginalDespatchQty] AS InvoicedQuantity
		,jd.OriginalDespatchQty - jd.ShortQty - jdd.Qty AS DeliveredQuantity
		,jdd.Qty AS DamagedQuantity
		,jd.[ShortQty] AS ShortQuantity
		,dr.[Description] AS Reason
	FROM [dbo].[JobDetail] jd
	LEFT JOIN [dbo].[JobDetailDamage] jdd on jdd.JobDetailId = jd.Id
	LEFT JOIN [dbo].[DamageReasons] dr on dr.Id = jdd.DamageReasonsId
	WHERE jd.JobId = @Id

END
