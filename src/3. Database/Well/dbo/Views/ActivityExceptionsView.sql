CREATE VIEW [dbo].[ActivityExceptionsView]
	AS 
	
WITH Exception (ActivityId, ExceptionTypeId, ExceptionCount) AS
	( SELECT a.id, lia.ExceptionTypeId, COUNT(lia.ExceptionTypeId)
		FROM Activity a
		INNER JOIN LineItem li ON li.ActivityId = a.Id
		INNER JOIN LineItemAction lia ON li.Id = lia.LineItemId
		GROUP BY a.Id, lia.ExceptionTypeId
	)

SELECT a.Id 
, SUM(ExceptionCount)   AS TotalExceptions
, ISNULL((SELECT  ExceptionCount FROM Exception WHERE ActivityId = a.Id and ExceptionTypeId = 1), 0) AS ShortExceptions 
, ISNULL((SELECT  ExceptionCount FROM Exception WHERE ActivityId = a.Id and ExceptionTypeId = 2), 0) AS BypassExceptions
, ISNULL((SELECT   ExceptionCount FROM Exception WHERE ActivityId = a.Id and ExceptionTypeId = 3), 0) AS DamageExceptions    
FROM Activity a
INNER JOIN Exception e on e.ActivityId = a.Id
GROUP BY a.Id
