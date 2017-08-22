CREATE PROCEDURE [dbo].[Stops_GetByRouteHeaderId]
	@routeHeaderId int = 0

AS

SELECT 
	   s.Id
FROM 
	[Stop] s
	INNER JOIN StopStatusView ssv on ssv.StopId = s.Id
WHERE 
	  RouteHeaderId = @routeHeaderId
	  AND S.DateDeleted IS NULL
	
