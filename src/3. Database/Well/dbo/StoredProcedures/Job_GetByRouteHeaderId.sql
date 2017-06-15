CREATE PROCEDURE [dbo].[Job_GetByRouteHeaderId]
	@RouteHeaderId INT
AS
BEGIN	
	SELECT		
		j.Id
	FROM
		Stop s
	INNER JOIN Job j on j.StopId = s.Id
	WHERE
		s.RouteHeaderId = @RouteHeaderId
	AND 
		J.JobTypeCode != 'UPL-SAN'
END