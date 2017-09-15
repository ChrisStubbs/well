CREATE PROCEDURE [dbo].[Deliveries_Get]
	@UserName VARCHAR(500),
	@JobStatuses dbo.IntTableType READONLY
AS
BEGIN

	SET NOCOUNT ON;

	Create TABLE #JobIdsTable	(Id INT)

	INSERT INTO #JobIdsTable
	SELECT j.Id	
	FROM	RouteHeader rh 
			INNER JOIN [Stop] s on rh.Id = s.RouteHeaderId
			INNER JOIN Job j on s.Id = j.StopId	
			INNER JOIN Branch b on rh.RouteOwnerId = b.Id
			INNER JOIN UserBranch ub on b.Id = ub.BranchId
			INNER JOIN [User] u on u.Id = ub.UserId
			Inner Join @JobStatuses js on js.Value = j.JobStatusId
	WHERE	u.IdentityName = @UserName
			AND j.InvoiceNumber IS NOT NULL
			AND	rh.DateDeleted IS NULL
			AND	s.DateDeleted IS NULL
			AND j.DateDeleted IS NULL

	SELECT	j.Id,
			rh.RouteNumber, 
			rh.RouteDate,
			s.PlannedStopNumber as DropId,
			IsNull(jbt.Description,'Unknown') as DeliveryType,
			j.InvoiceNumber, 
			j.PHAccount as AccountCode, --this is the P&H account code that is on the invoice
			a.Name as AccountName ,
			jb.[Description] as JobStatus,
			s.DeliveryDate,
			ISNULL(u2.Name, 'Unallocated') as Assigned,
			u.IdentityName as CurrentUserIdentity,
			a.Id as AccountId,  -- this is the main P&H account that is attached to the stop, needed for contact info 
			b.Id as BranchId,
			u2.IdentityName,
			j.COD as CashOnDelivery,
			j.TotalOutersShort,
			j.DetailOutersShort,
			Case When pc.JobId is null Then 0 else 1 End IsPendingCredit,
			pc.CreatedBy as PendingCreditCreatedBy,
			j.ProofOfDelivery,
			ct.Threshold as ThresholdAmount,
			j.OuterDiscrepancyFound
	FROM	RouteHeader rh 
			INNER JOIN [Stop] s on rh.Id = s.RouteHeaderId
			INNER JOIN Job j on s.Id = j.StopId	
			INNER JOIN Account a on s.Id = a.StopId
			INNER JOIN Branch b on rh.RouteOwnerId = b.Id
			INNER JOIN UserBranch ub on b.Id = ub.BranchId
			INNER JOIN [User] u on u.Id = ub.UserId
			INNER JOIN JobStatus jb on jb.Id = j.JobStatusId
			LEFT JOIN CreditThresholdUser ctu on ctu.UserId = u.Id
			LEFT JOIN CreditThreshold ct on ct.Id = ctu.CreditThresholdId
			LEFT JOIN UserJob uj on uj.JobId = j.Id 
			LEFT JOIN [User] u2 on u2.Id = uj.UserId
			LEFT JOIN PendingCredit pc on pc.JobId = j.Id And pc.DateDeleted IS NULL
			LEFT JOIN JobType jbt on jbt.Id = j.JobTypeId
			INNER JOIN #JobIdsTable jt on jt.Id = j.Id		
	WHERE	u.IdentityName = @UserName
	Order By s.DeliveryDate DESC

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
			INNER JOIN #JobIdsTable jt on jt.Id = jd.JobId
	WHERE	jd.DateDeleted IS NULL

	SELECT	jdd.[JobDetailId]
			,jdd.[Qty] as Quantity
			,jdd.JobDetailSourceId
			,jdd.JobDetailReasonId
			,jdd.DamageActionId
	From 	[dbo].[JobDetailDamage] jdd
			inner join [dbo].[JobDetail] jd on jdd.JobDetailId = jd.Id	
			INNER JOIN #JobIdsTable jt on jt.Id = jd.JobId
	WHERE 	jd.DateDeleted IS NULL

	Drop Table #JobIdsTable
END