CREATE PROCEDURE [dbo].[PendingCreditDeliveriesGet]
	@UserName VARCHAR(500)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		j.Id,
		rh.RouteNumber, 
		s.DropId,
		j.InvoiceNumber, 
		j.PHAccount as AccountCode, --this is the P&H account code that is on the invoice
		a.Name as AccountName ,
		ps.Description as JobStatus,
		s.DeliveryDate,
		a.Id as AccountId,  -- this is the main P&H account that is attached to the stop, needed for contact info 
		b.Id as BranchId,
		j.COD as CashOnDelivery,
		'£foo' as TotalCredit,
		pc.CreatedBy as PendingCreditCreatedBy
	FROM
		RouteHeader rh 
	INNER JOIN 
		[Stop] s on rh.Id = s.RouteHeaderId
	INNER JOIN 
		Job j on s.Id = j.StopId	
	INNER JOIN 
		dbo.Account a on s.Id = a.StopId
	INNER JOIN
		dbo.PerformanceStatus ps on ps.Id = j.PerformanceStatusId
	INNER JOIN
		dbo.Branch b on rh.StartDepotCode = b.Id
	INNER JOIN
		dbo.UserBranch ub on b.Id = ub.BranchId
	INNER JOIN
		dbo.[User] u on u.Id = ub.UserId
	INNER JOIN
		dbo.[PendingCreditToUser] pc on pc.UserId = u.Id AND pc.InvoiceNumber = j.InvoiceNumber
	WHERE
		u.IdentityName = @UserName
	AND 
		j.InvoiceNumber IS NOT NULL
	AND 
		j.COD IS NOT NULL
	AND 
		rh.IsDeleted = 0
	AND
		s.IsDeleted = 0
	AND 
		j.IsDeleted = 0

END