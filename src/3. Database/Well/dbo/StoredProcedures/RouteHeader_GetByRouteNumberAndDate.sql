CREATE PROCEDURE [dbo].[RouteHeader_GetByRouteNumberAndDate]
	@RouteNumber			NVARCHAR(50),
	@RouteDate				DATETIME
AS
BEGIN
	 
	SELECT [Id],
		[CompanyId],
		[RouteNumber],
		[RouteDate],
		[DriverName],
		[StartDepotCode],
		[PlannedStops],
		[ActualStopsCompleted],
		[RoutesId],
		[RouteStatusId],
		[RoutePerformanceStatusId],
		[LastRouteUpdate],
		[AuthByPass],
		[NonAuthByPass],
		[ShortDeliveries],
		[DamagesRejected],
		[DamagesAccepted],
		[NotRequired],
		[CreatedBy],
		[DateCreated],
		[UpdatedBy],
		[DateUpdated],
		[Version]
  FROM [dbo].[RouteHeader]
  WHERE [RouteNumber] = @RouteNumber
  AND [RouteDate] = @RouteDate
END

