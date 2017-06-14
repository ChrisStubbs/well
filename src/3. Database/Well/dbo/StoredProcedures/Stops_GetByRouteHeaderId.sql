CREATE PROCEDURE [dbo].[Stops_GetByRouteHeaderId]
	@routeHeaderId int = 0

AS

SELECT 
	   s.[Id]
      ,s.[PlannedStopNumber]
      ,s.[RouteHeaderId]
      ,s.[DropId]
      ,s.[LocationId]
      ,s.[DeliveryDate]
	  ,s.[ShellActionIndicator] 
	  ,s.[AllowOvers] 
	  ,s.[CustUnatt] 
	  ,s.[PHUnatt] 
	  ,s.[DateCreated]
	  ,s.[DateDeleted]
	  ,ssv.WellStatusId
FROM 
	  [dbo].[Stop] s
INNER JOIN [dbo].[StopStatusView] ssv on ssv.StopId = s.Id
WHERE 
	  [RouteHeaderId] = @routeHeaderId

RETURN 0
