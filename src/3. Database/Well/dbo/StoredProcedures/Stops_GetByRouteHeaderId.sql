CREATE PROCEDURE [dbo].[Stops_GetByRouteHeaderId]
	@routeHeaderId int = 0

AS

SELECT 
	   [Id]
      ,[PlannedStopNumber]
      ,[PlannedArriveTime]
      ,[PlannedDepartTime]
      ,[RouteHeaderId]
      ,[DropId]
      ,[LocationId]
      ,[DeliveryDate]
      ,[SpecialInstructions]
      ,[StartWindow]
      ,[EndWindow]
      ,[TextField1]
      ,[TextField2]
      ,[TextField3]
      ,[TextField4]
FROM 
	  [dbo].[Stop]
WHERE 
	  [RouteHeaderId] = @routeHeaderId

RETURN 0
