CREATE PROCEDURE Locations_GET
	@BranchId Int
AS 
SELECT 
	l.id
	,l.BranchId
	,b.Name BranchName
	,l.AccountCode AccountNumber
	,l.Name AccountName
	,l.AddressLine1
	,l.AddressLine2
	,l.Postcode
	,COUNT(a.Id) TotalInvoices
	,COUNT(invoicedJobs.ActivityId) Invoiced
	,COUNT(liwa.ActivityId) Exceptions	
FROM Location l
	JOIN Activity a on a.LocationId = l.Id

	LEFT JOIN 
	(SELECT 
		DISTINCT li.ActivityId
	FROM
		LineItem li
		INNER JOIN LineItemAction lia on lia.LineItemId = li.Id
		INNER JOIN JobDetail jd on li.id = jd.LineItemId
		INNER JOIN Job j on j.Id = jd.JobId
	WHERE 
		(lia.DeliveryActionId = 0 OR lia.DeliveryActionId IS NULL)
		AND
		j.ResolutionStatusId > 1
		AND
		j.JobTypeCode != 'DEL-DOC'
	GROUP BY li.ActivityId) 
		liwa on liwa.ActivityId = a.Id

	LEFT JOIN 
	(SELECT 
		DISTINCT j.ActivityId
	FROM
		Job j
	WHERE 
		j.ResolutionStatusId > 1
		AND
		j.JobTypeCode != 'DEL-DOC'
	GROUP BY j.ActivityId) 
		invoicedJobs on invoicedJobs.ActivityId = a.Id

	LEFT JOIN 
		Branch b ON b.Id = l.BranchId
WHERE
	(a.ActivityTypeId = 1 OR a.ActivityTypeId = 2)
GROUP BY 
	l.Id
	,l.BranchId
	,l.AccountCode
	,l.AddressLine1
	,l.AddressLine2
	,l.Postcode
	,l.Name
	,b.Name