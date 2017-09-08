CREATE VIEW [dbo].[LocationExceptionsView]
	AS 
	
WITH Exception (LocationId, ExceptionTypeId, ExceptionCount) AS
	( SELECT l.id, lia.ExceptionTypeId, COUNT(lia.ExceptionTypeId)
		FROM Location l
		INNER JOIN Activity a on a.LocationId = l.Id
		INNER JOIN LineItem li ON li.ActivityId = a.Id
		INNER JOIN LineItemAction lia ON li.Id = lia.LineItemId AND lia.DateDeleted IS NULL
		GROUP BY l.Id, lia.ExceptionTypeId
	)

SELECT l.Id AS LocationId
, SUM(ExceptionCount)   AS TotalExceptions
, ISNULL((SELECT  ExceptionCount FROM Exception WHERE LocationId = l.Id and ExceptionTypeId = 1), 0) AS ShortExceptions 
, ISNULL((SELECT  ExceptionCount FROM Exception WHERE LocationId = l.Id and ExceptionTypeId = 2), 0) AS BypassExceptions
, ISNULL((SELECT  ExceptionCount FROM Exception WHERE LocationId = l.Id and ExceptionTypeId = 3), 0) AS DamageExceptions    
FROM Location l
INNER JOIN Exception e on e.LocationId = l.Id
GROUP BY l.Id
