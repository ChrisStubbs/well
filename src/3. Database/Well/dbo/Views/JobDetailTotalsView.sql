CREATE VIEW [dbo].[JobDetailTotalsView]
	AS 
	WITH Rejected (JobDetailId, Quantity) AS
	(
		SELECT 
			jd.Id, SUM(jdd.Qty)
		FROM 
			JobDetail jd
			INNER JOIN JobDetailDamage jdd on jdd.JobDetailId = jd.Id
		WHERE UPPER(REPLACE(jdd.PdaReasonDescription, ' ', '')) LIKE '%NOTREQUIRED%'
		GROUP BY jd.Id
	),
	Damage (JobDetailId, Quantity) AS
	(
		SELECT 
			jd.Id, SUM(jdd.Qty)
		FROM 
			JobDetail jd
		INNER JOIN JobDetailDamage jdd on jdd.JobDetailId = jd.Id
		WHERE UPPER(REPLACE(jdd.PdaReasonDescription, ' ', '')) NOT LIKE '%NOTREQUIRED%'
		GROUP BY jd.Id
	),
	Short (JobDetailId, Quantity) AS
	(
		SELECT 
			jd.Id, jd.ShortQty
		FROM 
			JobDetail jd
		WHERE 
			ShortQty IS NOT NULL
	)
	SELECT 
		jd.Id AS JobDetailId
		,jd.DeliveredQty AS Delivered
		,ISNULL((SELECT Quantity FROM Short WHERE JobDetailId = jd.Id) , 0) AS ShortTotal
		,ISNULL((SELECT Quantity FROM Damage WHERE JobDetailId = jd.Id) , 0) AS DamageTotal
		,ISNULL((SELECT Quantity FROM Rejected WHERE JobDetailId = jd.Id) , 0) AS RejectedTotal 
	FROM 
		JobDetail jd
	WHERE 
		jd.DateDeleted IS NULL
	


