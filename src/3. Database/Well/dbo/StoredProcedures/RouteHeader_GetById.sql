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
		[RouteStatusId],
		[RoutePerformanceStatusId],
		[LastRouteUpdate],
		[AuthByPass],
		[NonAuthByPass],
		[ShortDeliveries],
		[DamagesRejected],
		[DamagesAccepted],
		[NotRequired],
		[Depot],
		[RouteOwnerId],
		[CreatedBy],
		[DateCreated],
		[UpdatedBy],
		[DateUpdated],
		[Version]
  FROM [dbo].[RouteHeader]
  WHERE [Id] = @Id
END
