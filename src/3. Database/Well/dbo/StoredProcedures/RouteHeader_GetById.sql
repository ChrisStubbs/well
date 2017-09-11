CREATE PROCEDURE [dbo].[RouteHeader_GetById]
	@Id INT
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
		[RouteStatusCode],
		[RouteStatusDescription],
		[PerformanceStatusCode],
		[PerformanceStatusDescription],
		[LastRouteUpdate],
		[AuthByPass],
		[NonAuthByPass],
		[ShortDeliveries],
		[DamagesRejected],
		[DamagesAccepted],
		[RouteOwnerId],
		[CreatedBy],
		[DateCreated],
		[UpdatedBy],
		[DateUpdated],
		[Version],
		[WellStatus] as RouteWellStatus
  FROM [dbo].[RouteHeader]
  WHERE [Id] = @Id
END
