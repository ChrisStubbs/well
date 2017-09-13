CREATE PROCEDURE [dbo].[Routeheader_GetByRouteId]
	@RouteId int 
AS
begin
	SELECT[Id]
      ,[CompanyId]
      ,[RouteNumber]
      ,[RouteDate]
      ,[DriverName]
      ,[StartDepotCode]
      ,[PlannedStops]
      ,[ActualStopsCompleted]
      ,[RoutesId]
      ,[RouteStatusCode]
	  ,[RouteStatusDescription]
      ,[PerformanceStatusCode]
	  ,[PerformanceStatusDescription]
      ,[LastRouteUpdate]
      ,[AuthByPass]
      ,[NonAuthByPass]
      ,[ShortDeliveries]
      ,[DamagesRejected]
      ,[DamagesAccepted]
	  ,[RouteOwnerId]
	  ,[DateDeleted]
      ,[CreatedBy]
      ,[DateCreated]
      ,[UpdatedBy]
      ,[DateUpdated]
      ,[Version]
	  ,[WellStatus] as RouteWellStatus
  FROM [dbo].[RouteHeader]
  WHERE [RoutesId] = @RouteId
  end