CREATE PROCEDURE [dbo].[RouteHeaders_GetForDelete]
AS
	SELECT 
	  [Id]
      ,[CompanyId]
      ,[RouteNumber]
      ,[RouteDate]
      ,[DriverName]
      ,[StartDepotCode]
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

  FROM [dbo].[RouteHeader] 

RETURN 0
