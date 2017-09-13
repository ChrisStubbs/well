CREATE PROCEDURE [dbo].[JobIds_GetByLineItemIds]
	@Ids dbo.IntTableType	READONLY
AS
BEGIN
	SELECT DISTINCT 
		jd.JobId
	FROM
		LineItem li
	INNER JOIN 
		@Ids ids ON ids.Value = li.Id
	INNER JOIN
		JobDetail jd ON jd.LineItemId = li.Id
	WHERE
		jd.DateDeleted IS NULL
		AND
		li.DateDeleted IS NULL
END