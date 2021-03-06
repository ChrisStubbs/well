﻿CREATE VIEW LineItemExceptionsView 
AS
	WITH Exception (LineItemId, ExceptionTypeId, ExceptionCount) AS
	( 
		SELECT 
			li.id, ExceptionTypeId, SUM(lia.Quantity)
		FROM 
			LineItem li
			INNER JOIN LineItemAction lia ON li.Id = lia.LineItemId
		WHERE	
			lia.DateDeleted IS NULL
		GROUP BY 
			li.Id, ExceptionTypeId
	)
	SELECT 
		li.Id
		,COUNT(lia.Id) AS TotalExceptions
		,ISNULL((SELECT ExceptionCount FROM Exception WHERE LineItemId = li.Id AND ExceptionTypeId = 1) ,0) AS ShortTotal
		,ISNULL((SELECT ExceptionCount FROM Exception WHERE LineItemId = li.Id AND ExceptionTypeId = 2) ,0) AS BypassTotal
		,ISNULL((SELECT ExceptionCount FROM Exception WHERE LineItemId = li.Id AND ExceptionTypeId = 3) ,0) AS DamageTotal
	FROM 
		LineItem li
		INNER JOIN LineItemAction lia ON lia.LineItemId = li.Id
	WHERE	
		lia.DateDeleted IS NULL
		AND li.DateDeleted IS NULL
	GROUP BY 
		li.Id