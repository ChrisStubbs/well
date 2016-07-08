CREATE PROCEDURE [dbo].[RouteHeader_CreateOrUpdate]
	@Id						INT = NULL,
	@Username				NVARCHAR(50),
	@CompanyId			    INT,
	@RouteNumber			NVARCHAR(50),
	@RouteDate				DATETIME,
	@DriverName				NVARCHAR(50),
	@VehicleReg				NVARCHAR(10),
	@StartDepotCode			NVARCHAR(10),
	@PlannedRouteStartTime  TIME,
	@PlannedRouteFinishTime TIME,
	@PlannedDistance		DECIMAL(3,2),
	@PlannedTravelTime		TIME,
	@PlannedStops			TINYINT,
	@RoutesId				INT


AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @ChangeResult TABLE (ChangeType VARCHAR(10), Id INTEGER)

	MERGE INTO [RouteHeader] AS Target
	USING (VALUES
		(@Id, @CompanyId, @RouteNumber, @RouteDate, @DriverName, @VehicleReg, @StartDepotCode, @PlannedRouteStartTime, @PlannedRouteFinishTime, @PlannedDistance, @PlannedTravelTime,
		 @PlannedStops, @RoutesId,  @Username, GETDATE(), @Username, GETDATE())
	)
	AS Source ([Id],[CompanyId],[RouteNumber],[RouteDate],[DriverName],[VehicleReg],[StartDepotCode],[PlannedRouteStartTime],[PlannedRouteFinishTime],
			   [PlannedDistance],[PlannedTravelTime],[PlannedStops],[RoutesId],[CreatedBy],[DateCreated],[UpdatedBy],[DateUpdated])
	ON Target.[Id] = Source.[Id]
	WHEN MATCHED THEN
	UPDATE SET
		[CompanyId] = Source.[CompanyId],
		[RouteNumber] = Source.[RouteNumber],
		[RouteDate] = Source.[RouteDate],
		[DriverName] = Source.[DriverName],
		[VehicleReg] = Source.[VehicleReg],
		[StartDepotCode] = Source.[StartDepotCode],
		[PlannedRouteStartTime] = Source.[PlannedRouteStartTime],
		[PlannedRouteFinishTime] = Source.[PlannedRouteFinishTime],
		[PlannedDistance] = Source.[PlannedDistance],
		[PlannedTravelTime] = Source.[PlannedTravelTime],
		[PlannedStops] = Source.[PlannedStops],
		[RoutesId] = Source.[RoutesId],
		[CreatedBy] = Source.[CreatedBy],
		[DateCreated] = Source.[DateCreated],
		[UpdatedBy] = Source.[UpdatedBy],
		[DateUpdated] = Source.[DateUpdated]
	WHEN NOT MATCHED BY TARGET AND @Id = 0 THEN
	INSERT ([CompanyId],[RouteNumber],[RouteDate],[DriverName],[VehicleReg],[StartDepotCode],[PlannedRouteStartTime],[PlannedRouteFinishTime],
			   [PlannedDistance],[PlannedTravelTime],[PlannedStops],[RoutesId],[CreatedBy],[DateCreated],[UpdatedBy],[DateUpdated])
	VALUES ([CompanyId],[RouteNumber],[RouteDate],[DriverName],[VehicleReg],[StartDepotCode],[PlannedRouteStartTime],[PlannedRouteFinishTime],
			   [PlannedDistance],[PlannedTravelTime],[PlannedStops],[RoutesId],[CreatedBy],[DateCreated],[UpdatedBy],[DateUpdated])

	OUTPUT $action, inserted.Id INTO @ChangeResult;

	SELECT Id FROM @ChangeResult

END
