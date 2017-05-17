CREATE PROCEDURE [dbo].[Job_GetByIds]
	@Ids dbo.IntTableType	READONLY
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
	  ,j.ProofOfDelivery
	  ,j.GrnProcessType
	  ,j.TotalOutersShort
	  ,j.DetailOutersShort
	  ,j.OuterDiscrepancyFound
	FROM [dbo].[Job] j
	INNER JOIN @Ids ids ON ids.Value = j.Id


	SELECT  d.Id, d.LineNumber, d.PHProductCode, d.OriginalDespatchQty, d.DeliveredQty, d.ProdDesc, d.OrderedQty, d.ShortQty, d.ShortsActionId, d.JobDetailReasonId, d.JobDetailSourceId, d.UnitMeasure, 
            d.PHProductType, d.PackSize, d.SingleOrOuter,d.SSCCBarcode, d.SubOuterDamageTotal, d.SkuGoodsValue, d.NetPrice, d.JobId, d.ShortsStatus, d.LineDeliveryStatus, d.IsHighValue, d.DateLife, d.IsDeleted, 
            d.CreatedBy, d.DateCreated, d.UpdatedBy, d.DateUpdated, d.Version, d.LineItemId
	FROM	Job AS j 
			INNER JOIN Stop AS s ON j.StopId = s.Id 
			INNER JOIN RouteHeader AS r ON s.RouteHeaderId = r.Id 
			INNER JOIN JobDetail AS d ON j.Id = d.JobId
			INNER JOIN @Ids ids ON ids.Value = j.Id


	SELECT	dd.Id, dd.JobDetailId, dd.Qty, dd.IsDeleted, dd.JobDetailSourceId, dd.JobDetailReasonId, dd.DamageActionId, dd.DamageStatus, dd.PdaReasonDescription, dd.CreatedBy, dd.DateCreated, dd.UpdatedBy, dd.DateUpdated, dd.Version
	FROM	Job AS j 
			INNER JOIN Stop AS s ON j.StopId = s.Id 
			INNER JOIN RouteHeader AS r ON s.RouteHeaderId = r.Id 
			INNER JOIN JobDetail AS d ON j.Id = d.JobId 
			INNER JOIN JobDetailDamage AS dd ON d.Id = dd.JobDetailId
			INNER JOIN @Ids ids ON ids.Value = j.Id
END

GO

