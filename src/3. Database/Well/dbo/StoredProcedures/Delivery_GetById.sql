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
	,ps.[Description] AS DeliveryType
	,u2.IdentityName
	,j.OuterCount
	,j.OuterDiscrepancyFound
	,j.TotalOutersShort
	,rh.StartDepotCode AS BranchId
FROM 
	[dbo].RouteHeader rh
JOIN
	[dbo].[Stop] s on rh.Id = s.RouteHeaderId
JOIN
	[dbo].[Job] j on s.Id = j.StopId
JOIN 
	[dbo].PerformanceStatus ps on ps.Id = j.PerformanceStatusId
JOIN 
	[dbo].[Account] a on a.StopId = j.StopId
LEFT JOIN
	[dbo].[UserJob] uj on uj.JobId = j.Id 
LEFT JOIN
	dbo.[User] u2 on u2.Id = uj.UserId
WHERE 
	j.Id = @Id
AND
	j.IsDeleted = 0
AND
	s.IsDeleted = 0
AND
	rh.IsDeleted = 0
