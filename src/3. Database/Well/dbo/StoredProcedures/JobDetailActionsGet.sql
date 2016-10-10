CREATE PROCEDURE [dbo].[JobDetailActionsGet]
	@JobId int
AS
BEGIN
	SELECT
		jd.LineNumber,
		jd.PHProductCode as ProductCode,
		jd.ProdDesc as ProductDescription,
		jd.SkuGoodsValue as [Value],
		j.InvOuters as InvoiceQuantity,
		jd.DeliveredQty as DeliveredQuantity,
		jd.SubOuterDamageTotal as DamagedQuantity,
		jd.ShortQty as ShortQuantity,
		a.Quantity as ActionQuantity,
		e.[Description] as [Action]
	FROM 
		JobDetail jd
	JOIN
		Job j on j.Id = @jobId
	JOIN
		JobDetailAction a on a.JobDetailId = jd.Id
	JOIN
		ExceptionAction e on e.Id = a.ActionId
	WHERE
		jd.JobId = @jobId
END