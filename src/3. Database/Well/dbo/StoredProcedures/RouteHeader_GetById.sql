CREATE PROCEDURE [dbo].[RouteHeader_GetById]
	@Id INT
AS
BEGIN
	SELECT 
	   [Id],
       [CompanyId],
       [RouteNumber],
	   [RouteDate],
	   [DriverName],
	   [VehicleReg],
	   [StartDepotCode],
	   [PlannedRouteStartTime],
	   [PlannedRouteFinishTime],
	   [PlannedDistance],
	   [PlannedTravelTime],
	   [PlannedStops],
	   [ActualStopsCompleted],
	   [RoutesId],
	   [RouteStatusId],
	   [RoutePerformanceStatusId],
       [CreatedBy],
       [DateCreated],
       [UpdatedBy],
       [DateUpdated],
       [Version]
  FROM [Well].[dbo].[RouteHeader]
  WHERE [Id] = @Id
END
