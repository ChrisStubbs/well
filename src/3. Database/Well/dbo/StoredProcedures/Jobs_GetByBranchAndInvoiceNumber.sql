CREATE PROCEDURE [dbo].[Jobs_GetByBranchAndInvoiceNumber]
	@BranchId INT,
	@InvoiceNumber VARCHAR(40)

AS
BEGIN
	SELECT j.[Id]
      ,j.[Sequence]
      ,j.[JobTypeCode]
      ,j.[PHAccount]
      ,j.[PickListRef]
      ,j.[InvoiceNumber]
      ,j.[CustomerRef]
      ,j.[OrderDate]
	  ,j.[RoyaltyCode]
	  ,j.[RoyaltyCodeDesc] 
	  ,j.[OrdOuters] 
	  ,j.[InvOuters] 
	  ,j.[ColOuters] 
	  ,j.[ColBoxes] 
	  ,j.[ReCallPrd] 
	  ,j.[AllowSOCrd] 
	  ,j.[COD] 
	  ,j.[GrnNumber] 
	  ,j.[GrnRefusedReason] 
	  ,j.[GrnRefusedDesc] 
	  ,j.[AllowReOrd] 
	  ,j.[SandwchOrd] 
	  ,j.[TotalCreditValueForThreshold]
	  ,j.[PerformanceStatusId] as PerformanceStatus
	  ,j.[Reason]
	  ,j.[IsDeleted]
      ,j.[StopId]
      ,j.[CreatedBy]
      ,j.[DateCreated]
      ,j.[UpdatedBy]
      ,j.[DateUpdated]
      ,j.[Version]
	  ,j.[JobStatusId] as JobStatus
	FROM [dbo].[Job] j
	INNER JOIN Stop s ON j.StopId = s.Id 
	INNER JOIN RouteHeader r ON s.RouteHeaderId = r.Id
	where r.RouteOwnerId = @BranchId and
	j.InvoiceNumber = @InvoiceNumber;

	SELECT        d.Id, d.LineNumber, d.PHProductCode, d.OriginalDespatchQty, d.DeliveredQty, d.ProdDesc, d.OrderedQty, d.ShortQty, d.ShortsActionId, d.JobDetailReasonId, d.JobDetailSourceId, d.UnitMeasure, 
                         d.PHProductType, d.PackSize, d.SingleOrOuter, d.SSCCBarcode, d.SubOuterDamageTotal, d.SkuGoodsValue, d.NetPrice, d.JobId, d.JobDetailStatusId, d.LineDeliveryStatus, d.IsHighValue, d.DateLife, d.IsDeleted, 
                         d.CreatedBy, d.DateCreated, d.UpdatedBy, d.DateUpdated, d.Version
FROM            Job AS j INNER JOIN
                         Stop AS s ON j.StopId = s.Id INNER JOIN
                         RouteHeader AS r ON s.RouteHeaderId = r.Id INNER JOIN
                         JobDetail AS d ON j.Id = d.JobId
WHERE        (r.RouteOwnerId = @BranchId) AND (j.InvoiceNumber = @InvoiceNumber)

SELECT        dd.Id, dd.JobDetailId, dd.Qty, dd.IsDeleted, dd.JobDetailSourceId, dd.JobDetailReasonId, dd.DamageActionId, dd.CreatedBy, dd.DateCreated, dd.UpdatedBy, dd.DateUpdated, dd.Version
FROM            Job AS j INNER JOIN
                         Stop AS s ON j.StopId = s.Id INNER JOIN
                         RouteHeader AS r ON s.RouteHeaderId = r.Id INNER JOIN
                         JobDetail AS d ON j.Id = d.JobId INNER JOIN
                         JobDetailDamage AS dd ON d.Id = dd.JobDetailId
WHERE        (r.RouteOwnerId = @BranchId) AND (j.InvoiceNumber = @InvoiceNumber)
END

GO

