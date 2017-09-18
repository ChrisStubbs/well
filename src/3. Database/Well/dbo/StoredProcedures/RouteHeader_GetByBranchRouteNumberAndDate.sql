CREATE PROCEDURE [dbo].[RouteHeader_GetByBranchRouteNumberAndDate]
	@BranchId				INT,
	@RouteNumber			NVARCHAR(50),
	@RouteDate				DATETIME
AS
BEGIN
	 
	SELECT 
		rh.[Id],
		rh.[CompanyId],
		rh.[RouteNumber],
		rh.[RouteDate],
		rh.[DriverName],
		rh.[StartDepotCode],
		rh.[RouteOwnerId],
		rh.[PlannedStops],
		rh.[ActualStopsCompleted],
		rh.[RoutesId],
		rh.[RouteStatusCode],
		rh.[RouteStatusDescription],
		rh.[PerformanceStatusCode],
		rh.[PerformanceStatusDescription],
		rh.[LastRouteUpdate],
		rh.[AuthByPass],
		rh.[NonAuthByPass],
		rh.[ShortDeliveries],
		rh.[DamagesRejected],
		rh.[DamagesAccepted],
		rh.[WellStatus] RouteWellStatus,
		rh.[CreatedBy],
		rh.[DateCreated],
		rh.[UpdatedBy],
		rh.[DateUpdated],
		rh.[Version]
  FROM [dbo].[RouteHeader] rh
  WHERE [RouteNumber] = @RouteNumber
  AND [RouteDate] = @RouteDate
  AND rh.RouteOwnerId = @BranchId
END

