CREATE PROCEDURE [dbo].[RouteheaderGetByNumberDateBranch]
	@RouteNumber			VARCHAR(50),
	@RouteDate				DATETIME,
	@BranchId				INT
AS
BEGIN
	 
	SELECT [Id],
		[CompanyId],
		[RouteNumber],
		[RouteDate],
		[DriverName],	
		[StartDepotCode],
		[RouteOwnerId],
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
		[WellStatus] RouteWellStatus,
		[CreatedBy],
		[DateCreated],
		[UpdatedBy],
		[DateUpdated],
		[Version]
  FROM 
	[dbo].[RouteHeader]
  WHERE 
	[RouteNumber] = @RouteNumber
  AND 
	[RouteDate] = @RouteDate
  AND
	[RouteOwnerId] = @BranchId
END

