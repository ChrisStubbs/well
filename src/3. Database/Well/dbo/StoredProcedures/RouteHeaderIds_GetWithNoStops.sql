CREATE PROCEDURE [dbo].[RouteHeaderIds_GetWithNoStops]

AS
	SELECT
		rh.Id 
	FROM 
		RouteHeader rh
	LEFT OUTER JOIN 
		Stop s on s.RouteHeaderId = rh.Id
	WHERE
		rh.DateDeleted IS NULL
		AND s.DateDeleted IS NULL
		AND s.Id IS NULL


