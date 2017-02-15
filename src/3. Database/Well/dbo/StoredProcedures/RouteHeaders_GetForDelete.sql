﻿CREATE PROCEDURE [dbo].[RouteHeaders_GetForDelete]
AS
	SELECT 
	  [Id]
      ,[CompanyId]
      ,[RouteNumber]
      ,[RouteDate]
      ,[DriverName]
      --,[StartDepotCode]
	  ,[RouteOwnerId]
      ,[PlannedStops]
      ,[ActualStopsCompleted]
      ,[RoutesId]
      ,[RouteStatusCode]
      ,[RouteStatusDescription]
	  ,[PerformanceStatusCode]
	  ,[PerformanceStatusDescription]
      ,[CreatedBy]
      ,[DateCreated]
      ,[UpdatedBy]
      ,[DateUpdated]
      ,[Version]

  FROM [dbo].[RouteHeader] 

RETURN 0
