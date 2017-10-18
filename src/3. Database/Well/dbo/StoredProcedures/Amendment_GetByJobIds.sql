CREATE PROCEDURE [dbo].[Amendment_GetByJobIds]
	@Ids dbo.IntTableType	READONLY
AS

SELECT 
	j.Id AS JobId
	,j.InvoiceNumber AS InvoiceNumber
	,j.PHAccount AS AccountNumber
	,rh.RouteOwnerId AS BranchId
	,u.IdentityName AS AmenderName
	,j.CustomerRef AS CustomerReference
	FROM 
		dbo.Job j with(NoLock)
		INNER JOIN @Ids ids ON ids.Value = j.Id	
		INNER JOIN dbo.[Stop] s with(NoLock) ON s.Id = j.StopId
		INNER JOIN dbo.RouteHeader rh with(NoLock) ON rh.Id = s.RouteHeaderId
		INNER JOIN dbo.UserJob uj with(NoLock) ON uj.JobId = j.Id 
		INNER JOIN dbo.[User] u with(NoLock) ON u.Id = uj.UserId
		--WHERE j.JobTypeCode != 'DEL-DOC' 

SELECT 
	j.Id AS JobId
	,j.InvoiceNumber AS InvoiceNumber
	,jd.PHProductCode AS ProductCode
	,jdtv.Delivered AS DeliveredQuantity
	,jdtv.ShortTotal AS ShortTotal
	,jdtv.DamageTotal AS DamageTotal
	,jdtv.RejectedTotal AS RejectedTotal
	,(jd.OriginalDespatchQty - (ISNULL(liav.ShortTotal, 0) + ISNULL(liav.DamageTotal, 0) + ISNULL(liav.RejectedTotal, 0))) As AmendedDeliveredQuantity
	,ISNULL(liav.ShortTotal, 0) AS AmendedShortTotal
	,ISNULL(liav.DamageTotal, 0) AS AmendedDamageTotal
	,ISNULL(liav.RejectedTotal,0) AS AmendedRejectedTotal
FROM dbo.Job j
INNER JOIN @Ids ids ON ids.Value = j.Id	
INNER JOIN dbo.UserJob uj with(NoLock) ON uj.JobId = j.Id 
INNER JOIN dbo.[User] u with(NoLock) ON u.Id = uj.UserId
LEFT JOIN dbo.JobDetail jd with(NoLock) ON jd.JobId = j.Id
LEFT JOIN dbo.JobDetailTotalsView jdtv with(NoLock) on jdtv.JobDetailId = jd.Id
LEFT JOIN dbo.LineItemAmendmentsView liav with(NoLock) on liav.LineItemId = jd.LineItemId
WHERE (jdtv.ShortTotal != liav.ShortTotal OR jdtv.DamageTotal != liav.DamageTotal OR jdtv.RejectedTotal != liav.RejectedTotal)
--AND j.JobTypeCode != 'DEL-DOC' 
