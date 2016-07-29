CREATE PROCEDURE [dbo].[Deliveries_GetByPerformanceStatus]
	@PerformanceStatusId INT,
	@UserName VARCHAR(500)
AS
BEGIN

	SET NOCOUNT ON;

		SELECT
		j.Id,
		rh.RouteNumber, 
		s.DropId,
		j.JobRef3 as InvoiceNumber, 
		j.JobRef1 as AccountCode, --this is the P&H account code that is on the invoice
		a.Name as AccountName ,
		ps.Description as JobStatus,
		s.DeliveryDate  as [DateTime],
		null as Action,
		null as Reason, 
		null as Assigned,
		a.Id as AccountId  -- this is the main P&H account that is attached to the stop, needed for contact info 
	FROM
		RouteHeader rh 
	INNER JOIN 
		[Stop] s on rh.Id = s.RouteHeaderId
	INNER JOIN 
		Job j on s.id = j.StopId	
	INNER JOIN 
		dbo.Account a on s.Id = a.StopId
	INNER JOIN
		dbo.PerformanceStatus ps on ps.Id = j.PerformanceStatusId
	INNER JOIN
		dbo.Branch b on rh.StartDepotCode = UPPER(b.TranscendMapping)
	INNER JOIN
		dbo.UserBranch ub on b.Id = ub.BranchId
	INNER JOIN
		dbo.[User] u on u.Id = ub.UserId

	WHERE
		ps.Id =  @PerformanceStatusId
	AND 
		u.IdentityName = @UserName


END
