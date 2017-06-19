CREATE PROCEDURE JobDetailTotalsPerRouteHeader
	@RouteHeaderId Int
AS
	SELECT 
		lie.BypassTotal,
		lie.DamageTotal,
		lie.ShortTotal,
		lie.TotalExceptions,
		jd.Id AS JobDetailId
	FROM 
		LineItemExceptionsView lie
		INNER JOIN LineItem li
			ON lie.Id = li.Id
			AND li.DateDeleted IS NULL
		INNER JOIN JobDetail jd
			ON li.Id = jd.LineItemId
		INNER JOIN Job j
			ON jd.JobId = j.Id
			AND j.JobTypeCode != 'UPL-SAN'
		INNER JOIN Stop s
			ON j.StopId = s.Id
			AND s.RouteHeaderId = @RouteHeaderId