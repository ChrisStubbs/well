﻿CREATE PROCEDURE [dbo].[Delivery_GetById]
	@Id INT,
	@UserName VARCHAR(500)
AS
SELECT j.[Id] AS [Id]
	,j.PHAccount AS AccountCode
	,j.PerformanceStatusId as [Status]
	,a.Name AS AccountName
	,a.Address1 + ',' + a.Address2  + ',' + a.PostCode AS AccountAddress
	,j.InvoiceNumber
	,j.GrnNumber
	,a.ContactName
	,a.ContactNumber AS PhoneNumber
	,a.ContactNumber2 AS MobileNumber
	,js.[Description] AS JobStatus
	,u2.IdentityName
	,@UserName as CurrentUserIdentity
	,j.OuterCount
	,j.OuterDiscrepancyFound
	,j.TotalOutersShort
	,j.DetailOutersShort
	,rh.StartDepotCode AS BranchId
	,j.GrnProcessType
	,j.COD as CashOnDelivery
	,j.ProofOfDelivery
	,jb.[Description] as JobStatus
	,Case When pc.JobId is null Then 0 else 1 End IsPendingCredit
FROM	[dbo].RouteHeader rh
		JOIN [dbo].[Stop] s on rh.Id = s.RouteHeaderId
		JOIN [dbo].[Job] j on s.Id = j.StopId
		JOIN [dbo].JobStatus js on js.Id = j.JobStatusId
		JOIN [dbo].[Account] a on a.StopId = j.StopId
		inner join JobStatus jb on jb.Id = j.JobStatusId
		LEFT JOIN [dbo].[UserJob] uj on uj.JobId = j.Id
		LEFT JOIN dbo.[User] u2 on u2.Id = uj.UserId
		LEFT JOIN PendingCredit pc on pc.JobId = j.Id And pc.DateDeleted IS NULL
WHERE	j.Id = @Id
		AND j.DateDeleted IS NULL
		AND	s.DateDeleted IS NULL
		AND	rh.DateDeleted IS NULL