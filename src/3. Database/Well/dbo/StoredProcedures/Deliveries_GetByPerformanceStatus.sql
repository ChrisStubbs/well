﻿CREATE PROCEDURE [dbo].[Deliveries_GetByPerformanceStatus]
	@PerformanceStatusId INT,
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
		ISNULL(u2.Name, 'Unallocated') as Assigned,
		a.Id as AccountId,  -- this is the main P&H account that is attached to the stop, needed for contact info 
		b.Id as BranchId,
		u2.IdentityName,
		ja.Value as CashOnDelivery
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
	LEFT JOIN
		dbo.UserJob uj on uj.JobId = j.Id 
	LEFT JOIN
		dbo.[User] u2 on u2.Id = uj.UserId
	LEFT JOIN 
		dbo.JobAttribute ja on ja.JobId = j.Id AND ja.Code = 'COD' 

	WHERE
		ps.Id =  @PerformanceStatusId
	AND 
		u.IdentityName = @UserName
	AND 
		j.InvoiceNumber IS NOT NULL



END
