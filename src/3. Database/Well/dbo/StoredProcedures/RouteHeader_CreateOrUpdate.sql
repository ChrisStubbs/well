CREATE PROCEDURE [dbo].[RouteHeader_CreateOrUpdate]
	@Id						INT = NULL,
	@Username				NVARCHAR(50),
	@CompanyId			    INT,
	@RouteNumber			NVARCHAR(50),
	@RouteDate				DATETIME,
	@DriverName				NVARCHAR(50) = NULL,
	@StartDepotCode			INT,
	@PlannedStops			TINYINT,
	@ActualStopsCompleted   TINYINT = 0, 
	@RoutesId				INT,
	@RouteStatusId			TINYINT= NULL,
	@RoutePerformanceStatusId TINYINT= NULL,
	@LastRouteUpdate		DATETIME = GETDATE,
	@AuthByPass			    INT = NULL,
	@NonAuthByPass		    INT = NULL,
	@ShortDeliveries        INT = NULL,
	@DamagesRejected        INT = NULL,
	@DamagesAccepted        INT = NULL,
	@NotRequired            INT = NULL,
	@Depot                  INT = NULL	 
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @ChangeResult TABLE (ChangeType VARCHAR(10), Id INTEGER)

	MERGE INTO [RouteHeader] AS Target
	USING (VALUES
		(@Id, @CompanyId, @RouteNumber, @RouteDate, @DriverName,  @StartDepotCode,
		 @PlannedStops,@ActualStopsCompleted, @RoutesId, @RouteStatusId, @RoutePerformanceStatusId, @LastRouteUpdate, @AuthByPass, @NonAuthByPass, @ShortDeliveries, @DamagesRejected,
		 @DamagesAccepted, @NotRequired,  @Depot, @Username, GETDATE(), @Username, GETDATE())
	)
	AS Source ([Id],[CompanyId],[RouteNumber],[RouteDate],[DriverName],[StartDepotCode],[PlannedStops], [ActualStopsCompleted],[RoutesId],[RouteStatusId], [RoutePerformanceStatusId], [LastRouteUpdate],
			   [AuthByPass], [NonAuthByPass], [ShortDeliveries], [DamagesRejected], [DamagesAccepted], [NotRequired], [Depot],
			   [CreatedBy],[DateCreated],[UpdatedBy],[DateUpdated])
	ON Target.[Id] = Source.[Id]
	WHEN MATCHED THEN
	UPDATE SET
		[CompanyId] = Source.[CompanyId],
		[RouteNumber] = Source.[RouteNumber],
		[RouteDate] = Source.[RouteDate],
		[DriverName] = Source.[DriverName],
		[StartDepotCode] = Source.[StartDepotCode],
		[PlannedStops] = Source.[PlannedStops],
		[RoutesId] = Source.[RoutesId],
	    [RouteStatusId] = Source.[RouteStatusId],
		[RoutePerformanceStatusId] = Source.[RoutePerformanceStatusId],
		[LastRouteUpdate] = Source.[LastRouteUpdate],
		[AuthByPass] = Source.[AuthByPass],
		[NonAuthByPass] = Source.[NonAuthByPass],
		[ShortDeliveries] = Source.[ShortDeliveries],
		[DamagesRejected] = Source.[DamagesRejected],
		[DamagesAccepted] = Source.[DamagesAccepted],
		[NotRequired] = Source.[NotRequired],
		[Depot] = Source.[Depot],
		[CreatedBy] = Source.[CreatedBy],
		[DateCreated] = Source.[DateCreated],
		[UpdatedBy] = Source.[UpdatedBy],
		[DateUpdated] = Source.[DateUpdated]
	WHEN NOT MATCHED BY TARGET AND @Id = 0 THEN
	INSERT ([CompanyId],[RouteNumber],[RouteDate],[DriverName],[StartDepotCode],[PlannedStops], [ActualStopsCompleted],  [RoutesId], [RouteStatusId], [RoutePerformanceStatusId], [LastRouteUpdate],
			   [AuthByPass], [NonAuthByPass], [ShortDeliveries], [DamagesRejected], [DamagesAccepted], [NotRequired], [Depot],
			   [CreatedBy],[DateCreated],[UpdatedBy],[DateUpdated])
	VALUES ([CompanyId],[RouteNumber],[RouteDate],[DriverName],[StartDepotCode],[PlannedStops], [ActualStopsCompleted],  [RoutesId], [RouteStatusId], [RoutePerformanceStatusId], [LastRouteUpdate],
			   [AuthByPass], [NonAuthByPass], [ShortDeliveries], [DamagesRejected], [DamagesAccepted], [NotRequired], [Depot],
			   [CreatedBy],[DateCreated],[UpdatedBy],[DateUpdated])

	OUTPUT $action, inserted.Id INTO @ChangeResult;

	SELECT Id FROM @ChangeResult

END
