CREATE PROCEDURE JobDetailTotalsPerStop
	@StopId Int
AS
	SELECT 
		lie.BypassTotal,
		lie.DamageTotal,
		COALESCE(NULLIF(lie.ShortTotal, 0), lie.BypassTotal, 0) AS ShortTotal,
		lie.TotalExceptions,
		jb.Id AS JobDetailId
	FROM 
		LineItemExceptionsView lie
		INNER JOIN LineItem li
			ON lie.Id = li.Id
			AND li.DateDeleted IS NULL
		INNER JOIN JobDetail jb
			ON li.Id = jb.LineItemId
		INNER JOIN Job j
			ON jb.JobId = j.Id
			AND j.StopId = @StopId
			--AND j.JobTypeCode != 'UPL-SAN'