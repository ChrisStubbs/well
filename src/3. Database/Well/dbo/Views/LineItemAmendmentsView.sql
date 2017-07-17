CREATE VIEW [dbo].[LineitemAmendmentsView]
	AS	
	WITH Rejected (LineItemId, Quantity)
	AS
	(	
		SELECT 
			li.id, SUM(lia.Quantity)
		FROM 
			LineItem li
			INNER JOIN LineItemAction lia ON li.Id = lia.LineItemId
		WHERE	
			lia.DateDeleted IS NULL AND UPPER(REPLACE(LIA.PdaReasonDescription, ' ', '')) LIKE '%NOTREQUIRED%' and ExceptionTypeId = dbo.ExceptionType_Damage()
		GROUP BY 
			li.Id, ExceptionTypeId
	),
	Damage (LineItemId, Quantity) AS
	(
		SELECT 
			li.Id, SUM(lia.Quantity)
		FROM 
			LineItem li
			INNER JOIN LineItemAction lia ON li.Id = lia.LineItemId
		WHERE 
			lia.DateDeleted IS NULL AND UPPER(REPLACE(lia.PdaReasonDescription, ' ', '')) NOT LIKE '%NOTREQUIRED%' and ExceptionTypeId = dbo.ExceptionType_Damage()
		GROUP BY li.Id
	),
	Short (LineItemId, Quantity) AS
	(
			SELECT 
			li.Id, SUM(lia.Quantity)
		FROM 
			LineItem li
			INNER JOIN LineItemAction lia ON li.Id = lia.LineItemId
		WHERE 
			lia.DateDeleted IS NULL AND (ExceptionTypeId = dbo.ExceptionType_Short() ) 
		GROUP BY li.Id
	)

	SELECT li.Id AS LineItemId
	,COUNT(lia.Id) AS TotalExceptions
	,ISNULL((SELECT Quantity FROM Rejected WHERE LineItemId = li.Id), 0) AS RejectedTotal
	,ISNULL((SELECT Quantity FROM Damage WHERE LineItemId = li.Id), 0) AS DamageTotal
	,ISNULL((SELECT Quantity FROM Short WHERE LineItemId = li.Id), 0) AS ShortTotal
	FROM LineItem li
	INNER JOIN LineItemAction lia ON lia.LineItemId = li.Id
	WHERE	
		lia.DateDeleted IS NULL
	GROUP BY 
		li.Id
