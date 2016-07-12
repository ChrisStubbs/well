CREATE PROCEDURE [dbo].[RouteHeaders_Get]

AS
	SELECT 
	   [Id]
      ,[CompanyId]
      ,[RouteNumber]
      ,[RouteDate]
      ,[DriverName]
      ,[VehicleReg]
      ,[StartDepotCode]
      ,[PlannedRouteStartTime]
      ,[PlannedRouteFinishTime]
      ,[PlannedDistance]
      ,[PlannedTravelTime]
      ,[PlannedStops]
      ,[ActualStopsCompleted]
      ,[RoutesId]
      ,[RouteStatusId]
      ,[RoutePerformanceStatusId]
      ,[CreatedBy]
      ,[DateCreated]
      ,[UpdatedBy]
      ,[DateUpdated]
      ,[Version]

  FROM [Well].[dbo].[RouteHeader]

RETURN 0
