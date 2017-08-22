CREATE PROCEDURE Job_GetByRouteHeaderId
	@RouteHeaderId INT
AS
	SELECT		
		j.Id
	FROM
		Stop s
		INNER JOIN Job j on j.StopId = s.Id
	WHERE
		s.RouteHeaderId = @RouteHeaderId
		AND J.JobTypeCode != 'UPL-SAN'
		AND j.DateDeleted IS NULL
		AND s.DateDeleted IS NULL