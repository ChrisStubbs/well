CREATE PROCEDURE [dbo].[Job_GetByIds]
	@Ids dbo.IntTableType	READONLY
AS
	SELECT 
		j.Id
		,j.Sequence
		,j.JobTypeCode
		,jb.Description AS JobType
		,j.PHAccount
		,j.PickListRef
		,j.InvoiceNumber
		,j.CustomerRef
		,j.OrderDate
		,j.RoyaltyCode
		,j.RoyaltyCodeDesc 
		,j.OrdOuters 
		,j.InvOuters 
		,j.ColOuters 
		,j.ColBoxes 
		,j.ReCallPrd 
		,j.AllowSOCrd 
		,j.COD 
		,j.GrnNumber 
		,j.GrnRefusedReason 
		,j.GrnRefusedDesc 
		,j.AllowReOrd 
		,j.SandwchOrd 
		,j.PerformanceStatusId as PerformanceStatus
		,j.Reason
		,j.DateDeleted
		,j.StopId
		,j.CreatedBy
		,j.DateCreated
		,j.UpdatedBy
		,j.DateUpdated
		,j.Version
		,j.JobStatusId as JobStatus
		,jsv.WellStatusId as WellStatus
		,j.ProofOfDelivery
		,j.GrnProcessType
		,j.TotalOutersShort
		,j.DetailOutersShort
		,j.OuterDiscrepancyFound
		,a.Id AS PhAccountId
		,credit.CreditValue
		,jb.Abbreviation AS JobTypeAbbreviation
		,CAST(j.ResolutionStatusId as INTEGER) AS ResolutionStatus
		,OuterCount
	FROM 
		dbo.Job j
		INNER JOIN @Ids ids ON ids.Value = j.Id
		INNER JOIN Stop AS s 
			ON j.StopId = s.Id 
		INNER JOIN Account a 
			ON s.Id = a.StopId
		LEFT JOIN JobType jb
			ON j.JobTypeCode = jb.Code
		INNER JOIN JobStatusView jsv on jsv.JobId = j.Id
		LEFT JOIN
		(
			SELECT 
				j.JobId, CONVERT(Decimal(18, 4), SUM(lia.Quantity * j.NetPrice)) AS CreditValue
			FROM 
				JobDetail j
				INNER JOIN LineItem li
					ON j.LineItemId = li.id
				INNER JOIN LineItemAction lia
					ON li.Id = lia.LineItemId
				INNER JOIN DeliveryAction da
					ON lia.DeliveryActionId = da.id
					AND da.Description = 'Credit'
				WHERE 
					 lia.DateDeleted IS NULL
			GROUP BY 
				j.JobId
		) credit
			ON credit.JobId = j.Id

	SELECT  d.Id, d.LineNumber, d.PHProductCode, d.OriginalDespatchQty, d.DeliveredQty, d.ProdDesc, d.OrderedQty, d.ShortQty, d.ShortsActionId, d.JobDetailReasonId, d.JobDetailSourceId, d.UnitMeasure, 
            d.PHProductType, 
			d.PackSize, 
			d.SingleOrOuter,
			d.SSCCBarcode, 
			d.SubOuterDamageTotal, 
			d.SkuGoodsValue, 
			d.NetPrice, 
			d.JobId, 
			d.ShortsStatus, 
			d.LineDeliveryStatus, 
			d.IsHighValue, 
			d.DateLife, 
			d.DateDeleted, 
            d.CreatedBy, 
			d.DateCreated, 
			d.UpdatedBy, 
			d.DateUpdated, 
			d.Version, 
			d.LineItemId
	FROM	Job AS j 
			INNER JOIN Stop AS s ON j.StopId = s.Id 
			INNER JOIN RouteHeader AS r ON s.RouteHeaderId = r.Id 
			INNER JOIN JobDetail AS d ON j.Id = d.JobId
			INNER JOIN @Ids ids ON ids.Value = j.Id
			
	SELECT	dd.Id, 
		dd.JobDetailId, 
		dd.Qty, 
		dd.DateDeleted, 
		dd.JobDetailSourceId, 
		dd.JobDetailReasonId, 
		dd.DamageActionId, 
		dd.DamageStatus, 
		dd.PdaReasonDescription, 
		dd.CreatedBy, 
		dd.DateCreated, 
		dd.UpdatedBy, 
		dd.DateUpdated, 
		dd.Version
	FROM	Job AS j 
			INNER JOIN Stop AS s ON j.StopId = s.Id 
			INNER JOIN RouteHeader AS r ON s.RouteHeaderId = r.Id 
			INNER JOIN JobDetail AS d ON j.Id = d.JobId 
			INNER JOIN JobDetailDamage AS dd ON d.Id = dd.JobDetailId
			INNER JOIN @Ids ids ON ids.Value = j.Id
