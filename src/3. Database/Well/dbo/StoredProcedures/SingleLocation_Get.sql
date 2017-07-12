CREATE PROCEDURE SingleLocation_Get
	@LocationId Int = NULL,
	@AccountNumber VarChar(64) = NULL,
	@BranchId Int = NULL
AS
	IF @LocationId IS NULL
		SELECT @LocationId = Id FROM Location l WHERE l.BranchId = @BranchId AND l.AccountCode = @accountNumber

	SELECT DISTINCT
		l.id,
		b.Id AS BranchId,
		b.Name AS BranchName,
		l.AccountCode AS AccountNumber,
		a.Name as AccountName,
		a.Address1 + ' ' +  a.Address2 + ' ' + a.Postcode AS AccountAddress
	FROM 
		Location l
		INNER JOIN Branch b
			ON l.BranchId = b.Id
		INNER JOIN Account a
			ON l.id = a.LocationId
	WHERE
		l.id = @locationId

	SELECT 
		rh.DriverName AS Driver,
		rh.RouteDate AS [Date],
		jt.Id AS JobTypeId,
		jt.Description AS JobType,
		js.Id AS JobStatusId,
		js.Description as JobStatus,
		CONVERT(Bit, CASE WHEN ISNULL(j.COD, '') = '' THEN 0 ELSE 1 END) AS Cod,
		CONVERT(Bit, CASE WHEN j.ProofOfDelivery IS NULL THEN 0 ELSE 0 END) AS Pod,
		ISNULL(ex.Exceptions, 0) AS Exceptions,
		ISNULL(cl.Total, 0) - ISNULL(ex.Exceptions, 0) AS Clean,
		CASE 
			WHEN j.OuterDiscrepancyFound = 1 THEN j.TotalOutersShort - j.DetailOutersShort
			ELSE 0 
		END AS TBA,
		credit.CreditValue AS Credit,
		j.ResolutionStatusId AS ResolutionStatus,
		j.InvoiceNumber as Invoice
	FROM 
		Job j
		INNER JOIN Activity a
			ON j.ActivityId = a.Id
			AND j.DateDeleted IS NULL
		INNER JOIN Location l
			ON a.LocationId = l.Id
		INNER JOIN [Stop] s
			ON j.StopId = s.Id
			AND s.DateDeleted IS NULL
		INNER JOIN RouteHeader rh
			ON s.RouteHeaderId = rh.Id
			AND rh.DateDeleted IS NULL
		INNER JOIN JobType jt
			ON j.JobTypeCode = jt.Code
			AND jt.code != 'DEL-DOC'
		INNER JOIN JobStatus js
			ON j.JobStatusId = js.Id
		LEFT JOIN
		(
			SELECT 
				j.JobId, CONVERT(Decimal(18, 4), SUM(lia.Quantity * j.NetPrice)) AS CreditValue
			FROM 
				JobDetail j
				INNER JOIN LineItem li
					ON j.LineItemId = li.id
					AND j.DateDeleted IS NULL
					AND li.DateDeleted IS NULL
				INNER JOIN LineItemAction lia
					ON li.Id = lia.LineItemId
					AND lia.DateDeleted IS NULL
				INNER JOIN DeliveryAction da
					ON lia.DeliveryActionId = da.id
					AND da.Description = 'Credit'
				WHERE 
						lia.DateDeleted IS NULL
			GROUP BY 
				j.JobId
		) credit
			ON j.Id = credit.JobId
		LEFT JOIN 
		(
			SELECT SUM(v.ShortTotal + v.DamageTotal + v.BypassTotal) AS Exceptions, jd.JobId
			FROM 
				LineItemExceptionsView v
				INNER JOIN JobDetail jd ON v.Id = jd.LineItemId AND jd.DateDeleted IS NULL
			GROUP BY jd.JobId
		) AS ex
			ON ex.JobId = j.Id
		LEFT JOIN 
		(
			SELECT SUM(jd.OriginalDespatchQty) AS Total, jd.JobId
			FROM 
				JobDetail jd
				LEFT JOIN LineItemAction la on jd.LineItemId = la.LineItemId AND ISNULL(la.Quantity, 0) = 0 AND la.DateDeleted IS NULL AND jd.DateDeleted IS NULL
			GROUP BY jd.JobId
		) cl
			ON j.Id = cl.JobId
	WHERE
		l.Id = @LocationId
