CREATE PROCEDURE Locations_GET
	@BranchId Int
AS 
	WITH Invoices AS
	(
		SELECT 
			COUNT(DISTINCT j.InvoiceNumber) TotalInvoices,
			COUNT(j.id) AS TotalJobs,
			j.PHAccount AS Account
		FROM
			Job j 
			INNER JOIN JobType jt
				ON j.JobTypeCode = jt.Code
				AND jt.code != 'DEL-DOC'
		WHERE
			j.DateDeleted IS NULL
		GROUP BY 
			j.PHAccount
	),
	Execptions AS
	(
		SELECT 
			COUNT(DISTINCT j.id) AS TotalJobs,
			j.PHAccount AS Account
		FROM 
			LineItemExceptionsView v
			INNER JOIN JobDetail jd ON v.Id = jd.LineItemId AND jd.DateDeleted IS NULL
			INNER JOIN Job j ON jd.JobId = j.Id AND j.DateDeleted IS NULL
		GROUP BY 
			j.PHAccount
	)
	SELECT DISTINCT
		b.id AS BranchId,
		b.Name AS Branch,
		a.Code AS PrimaryAccountNumber,
		j.PHAccount AS AccountNumber,
		a.Name AS AccountName,
		a.Address1 + ' ' +  a.Address2 + ' ' + a.Postcode AS [Address],
		i.TotalInvoices,
		ISNULL(ex.TotalJobs, 0) AS exceptions
	FROM 
		Location l
		INNER JOIN Branch b
			on l.BranchId = b.Id
		INNER JOIN Account a
			ON a.LocationId = l.Id
		INNER JOIN Activity ac
			ON ac.LocationId = l.id
		INNER JOIN Job j
			ON j.ActivityId = ac.Id
		INNER JOIN Invoices	i
			ON j.PHAccount = i.Account
		LEFT JOIN Execptions ex
			ON j.PHAccount = ex.Account
	WHERE
		b.id = @BranchId