﻿CREATE PROCEDURE JobsToBeApproved
AS
	DECLARE @PendingApproval varchar(50) = 'Pending Approval' 
	DECLARE @ResolutionStatus Int = (SELECT id FROM ResolutionStatus WHERE Description = @PendingApproval)
	DECLARE @DecliveryAction Int = (SELECT id FROM DeliveryAction WHERE Description = 'Credit')

	;WITH SubmittedInfo([By], [On], JobId) AS
	(
		SELECT [By], jrs.[On], jrs.Job
		FROM 
			JobResolutionStatus jrs
			INNER JOIN 
			(
				SELECT MAX(j.[On]) AS [On], j.Job
				FROM 
					JobResolutionStatus j
					INNER JOIN ResolutionStatus rs
						ON j.Status = rs.Description
						AND rs.Id = @ResolutionStatus
				GROUP BY j.job
			) MaxDates
				ON jrs.[On] = MaxDates.[On]
				AND jrs.Job = MaxDates.Job
				AND jrs.Status = @PendingApproval
	)
	SELECT DISTINCT
		j.id AS JobId,
		b.Id AS BranchId, 
		b.Name AS BranchName, 
		r.RouteDate AS DeliveryDate,
		a.Id AS AccountId,
		j.PhAccount AS Account,
		j.InvoiceNumber,
		si.[By] AS SubmittedBy,
		si.[On] AS DateSubmitted,
		Credit.CreditQuantity,
		Credit.CreditValue, 
		(SELECT TOP 1 ActivityId From LineItem l JOIN JobDetail jd ON l.Id = jd.LineItemId AND jd.JobId = j.id) AS InvoiceId
	FROM 
		Job j
		INNER JOIN Stop AS s 
			ON j.StopId = s.Id 
		INNER JOIN RouteHeader r
			ON s.RouteHeaderId = r.Id
		INNER JOIN Branch b 
			ON b.Id = r.RouteOwnerId
		INNER JOIN Account a 
			ON s.Id = a.StopId
		INNER JOIN SubmittedInfo si
			ON j.Id = si.JobId
		LEFT JOIN 
		(
			SELECT 
				jd.JobId, SUM(lia.Quantity) AS CreditQuantity, SUM(lia.Quantity * jd.NetPrice) AS CreditValue
			FROM	
				LineItem li
				INNER JOIN JobDetail jd 
					ON jd.LineItemId = li.Id
				INNER JOIN LineItemAction lia
					ON li.Id = lia.LineItemId
					AND lia.DeliveryActionId = @DecliveryAction
			WHERE
				jd.DateDeleted is Null
				AND lia.DateDeleted IS NULL
			GROUP BY 
				jd.JobId
				
		) Credit
			ON j.Id = Credit.JobId
	WHERE 
		j.ResolutionStatusId = @ResolutionStatus
		AND j.DateDeleted is Null
