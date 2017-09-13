CREATE PROCEDURE [dbo].[LineItemIds_GetByJobIds]
		@JobIds dbo.IntTableType	READONLY
AS
BEGIN
	SELECT 
		li.Id 
	FROM	
		LineItem li
	INNER JOIN 
		JobDetail jd ON jd.LineItemId = li.Id
	INNER JOIN 
		@JobIds ids ON ids.Value = jd.JobId
	WHERE
		jd.DateDeleted is Null;
END