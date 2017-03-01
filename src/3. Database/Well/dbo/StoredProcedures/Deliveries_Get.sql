CREATE PROCEDURE [dbo].[Deliveries_Get]
	@UserName VARCHAR(500),
	@JobStatus INT
AS
BEGIN

	SET NOCOUNT ON;

	SELECT	j.Id,
			rh.RouteNumber, 
			s.PlannedStopNumber as DropId,
			j.InvoiceNumber, 
			j.PHAccount as AccountCode, --this is the P&H account code that is on the invoice
			a.Name as AccountName ,
			jb.[Description] as JobStatus,
			s.DeliveryDate,
			ISNULL(u2.Name, 'Unallocated') as Assigned,
			a.Id as AccountId,  -- this is the main P&H account that is attached to the stop, needed for contact info 
			b.Id as BranchId,
			u2.IdentityName,
			j.COD as CashOnDelivery,
			j.TotalCreditValueForThreshold,
			j.TotalOutersShort,
			Case When pc.JobId is null Then 0 else 1 End IsPendingCredit,
			pc.CreatedBy as PendingCreditCreatedBy,
			j.ProofOfDelivery,
			ct.Threshold as ThresholdAmount
	FROM	RouteHeader rh 
			INNER JOIN [Stop] s on rh.Id = s.RouteHeaderId
			INNER JOIN Job j on s.Id = j.StopId	
			INNER JOIN Account a on s.Id = a.StopId
			INNER JOIN Branch b on rh.RouteOwnerId = b.Id
			INNER JOIN UserBranch ub on b.Id = ub.BranchId
			INNER JOIN [User] u on u.Id = ub.UserId
			INNER JOIN JobStatus jb on jb.Id = j.JobStatusId
			LEFT JOIN ThresholdLevel tl on tl.Id = u.ThresholdLevelId
			LEFT JOIN CreditThreshold ct on ct.ThresholdLevelId = tl.Id
			LEFT JOIN UserJob uj on uj.JobId = j.Id 
			LEFT JOIN [User] u2 on u2.Id = uj.UserId
			LEFT JOIN PendingCredit pc on pc.JobId = j.Id And pc.isDeleted = 0
	WHERE	u.IdentityName = @UserName
			AND j.InvoiceNumber IS NOT NULL
			AND	rh.IsDeleted = 0
			AND	s.IsDeleted = 0
			AND j.IsDeleted = 0
			AND	j.JobStatusId = @JobStatus
END
