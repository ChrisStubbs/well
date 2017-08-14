CREATE PROCEDURE [dbo].[Stops_GetByRouteHeaderId]
	@routeHeaderId int = 0

AS

SELECT 
	   s.[Id]
FROM 
	  [dbo].[Stop] s
INNER JOIN [dbo].[StopStatusView] ssv on ssv.StopId = s.Id
WHERE 
	  [RouteHeaderId] = @routeHeaderId
	  AND S.DateDeleted IS NULL
	
