﻿CREATE PROCEDURE [dbo].[RouteHeaders_Get]

AS
	SELECT 
	   [Id]
      ,[CompanyId]
      ,[RouteNumber]
      ,[RouteDate]
      ,[DriverName]
      ,[VehicleReg]
      ,[StartDepotCode]
      ,[StartDepotId]
      ,[FinishDepotCode]
      ,[FinishDepotId]
      ,[SubDepotCode]
      ,[SubDepotId]
      ,[FinishSubDepotCode]
      ,[FinishSubDepotId]
      ,[PlannedRouteStartTime]
      ,[PlannedRouteFinishTime]
      ,[InitialSealNumber]
      ,[PlannedDistance]
      ,[PlannedTravelTime]
      ,[PlannedStops]
      ,[ActualStopsCompleted]
      ,[AuthByPass]
      ,[NonAuthByPass]
      ,[ShortDeliveries]
      ,[DamagesRejected]
      ,[DamagesAccepted]
      ,[NotRequired]
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
