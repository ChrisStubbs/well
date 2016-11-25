CREATE PROCEDURE [dbo].[Stops_GetByRouteHeaderId]
	@routeHeaderId int = 0

AS

SELECT 
	   [Id]
      ,[PlannedStopNumber]
      ,[RouteHeaderId]
      ,[DropId]
      ,[LocationId]
      ,[DeliveryDate]
	  ,[ShellActionIndicator] 
	  ,[AllowOvers] 
	  ,[CustUnatt] 
	  ,[PHUnatt] 
	  ,[DateCreated]
	  ,[IsDeleted]
FROM 
	  [dbo].[Stop]
WHERE 
	  [RouteHeaderId] = @routeHeaderId

RETURN 0
