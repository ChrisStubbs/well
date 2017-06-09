CREATE VIEW [dbo].[RouteStatusView]
	AS 
	-- this is the count of bypassed jobs for a route
	WITH RouteJobBypass (RouteId, RouteStatusCode, BypassJobCount) AS 
		(SELECT rh.Id, 
				rh.RouteStatusCode, 
				COUNT(j.ID)
		FROM RouteHeader rh
		INNER JOIN [Stop] s ON s.RouteHeaderId = rh.Id
		INNER JOIN Job j ON j.StopId = s.Id
		WHERE J.JobStatusId = 8
		GROUP by rh.id, rh.RouteStatusCode)
	, -- this is the count of jobs for a route
		RouteJobCount (RouteId, RouteStatusCode, JobCount) AS 
		(SELECT rh.Id, 
				rh.RouteStatusCode, 
				COUNT(j.ID)
		FROM RouteHeader rh
		INNER JOIN [Stop] s on s.RouteHeaderId = rh.Id
		INNER JOIN Job j on j.StopId = s.Id
		GROUP by rh.id, rh.RouteStatusCode
		)

	SELECT rh.id AS RouteHeaderId,
		CASE
			WHEN rjb.BypassJobCount = rjc.JobCount THEN 8  -- route is bypassed if all jobs are bypassed otherwise use the TranSend status for the route
			WHEN rh.RouteStatusCode = 'NDEPA' THEN 1  -- not departed = planned
			WHEN rh.RouteStatusCode = 'INPRO' THEN 5 -- in progress
			WHEN rh.RouteStatusCode = 'COMPL' THEN 3 -- complete
			WHEN rh.RouteStatusCode IS NULL THEN 1 -- treat as not departed ie planned
		END AS RouteStatus
	FROM RouteHeader rh
	LEFT JOIN RouteJobBypass rjb on rh.Id = rjb.RouteId 
	INNER JOIN RouteJobCount rjc on rh.Id = rjc.RouteId 
