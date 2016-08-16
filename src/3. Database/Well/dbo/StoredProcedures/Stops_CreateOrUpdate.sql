CREATE PROCEDURE [dbo].[Stops_CreateOrUpdate]
	@Id						INT = 0,
	@Username				NVARCHAR(50),
	@PlannedStopNumber		NVARCHAR(4),
	@PlannedArriveTime		NVARCHAR(12),
	@PlannedDepartTime		NVARCHAR(12),
	@RouteHeaderCode		NVARCHAR(10),
	@RouteHeaderId			INT,
	@DropId					NVARCHAR(2),
	@LocationId				NVARCHAR(20),
	@DeliveryDate			DATETIME ,
	@SpecialInstructions	NVARCHAR(100),
	@StartWindow			NVARCHAR(12),
	@EndWindow				NVARCHAR(12),
	@TextField1				NVARCHAR(100),
	@TextField2				NVARCHAR(100), 
	@TextField3				NVARCHAR(100),
	@TextField4				NVARCHAR(100),
	@StopStatusId			TINYINT= 4,
	@StopPerformanceStatusId TINYINT= 6,
	@ByPassReasonId			TINYINT= 13,
	@IsDeleted				BIT=0



AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @ChangeResult TABLE (ChangeType VARCHAR(10), Id INTEGER)

	MERGE INTO [Stop] AS Target
	USING (VALUES
		(@Id, @PlannedStopNumber, @PlannedArriveTime,@PlannedDepartTime,@RouteHeaderCode, @RouteHeaderId, @DropId, @LocationId, @DeliveryDate, @SpecialInstructions, @StartWindow, @EndWindow, @TextField1,
		 @TextField2, @TextField3,@TextField4, @StopStatusId,@StopPerformanceStatusId, @ByPassReasonId, @IsDeleted,   @Username, GETDATE(), @Username, GETDATE())
	)
	AS Source ([Id],[PlannedStopNumber],[PlannedArriveTime],[PlannedDepartTime],[RouteHeaderCode],[RouteHeaderId],[DropId],[LocationId],[DeliveryDate],[SpecialInstructions],
			   [StartWindow],[EndWindow],[TextField1],[TextField2], [TextField3], [TextField4],
			   [StopStatusId], [StopPerformanceStatusId], [ByPassReasonId],[IsDeleted], [CreatedBy],[DateCreated],[UpdatedBy],[DateUpdated])
	ON Target.[Id] = Source.[Id]
	WHEN MATCHED THEN
	UPDATE SET
		[PlannedStopNumber] = Source.[PlannedStopNumber],
		[PlannedArriveTime] = Source.[PlannedArriveTime],
		[PlannedDepartTime] = Source.[PlannedDepartTime],
		[RouteHeaderCode] = Source.[RouteHeaderCode],
		[RouteHeaderId] = Source.[RouteHeaderId],
		[DropId] = Source.[DropId],
		[LocationId] = Source.[LocationId],
		[DeliveryDate] = Source.[DeliveryDate],
		[SpecialInstructions] = Source.[SpecialInstructions],
		[StartWindow] = Source.[StartWindow],
		[EndWindow] = Source.[EndWindow],
		[TextField1] = Source.[TextField1],
		[TextField2] = Source.[TextField2],
		[TextField3] = Source.[TextField3],
		[TextField4] = Source.[TextField4],
		[StopStatusId] = Source.[StopStatusId],
		[StopPerformanceStatusId] = Source.[StopPerformanceStatusId],
		[ByPassReasonId] = Source.[ByPassReasonId],
		[IsDeleted] = Source.[IsDeleted],
		[CreatedBy] = Source.[CreatedBy],
		[DateCreated] = Source.[DateCreated],
		[UpdatedBy] = Source.[UpdatedBy],
		[DateUpdated] = Source.[DateUpdated]
	WHEN NOT MATCHED BY TARGET AND @Id = 0 THEN
	INSERT ([PlannedStopNumber],[PlannedArriveTime],[PlannedDepartTime],[RouteHeaderCode],[RouteHeaderId],[DropId],[LocationId],[DeliveryDate],[SpecialInstructions],
			   [StartWindow],[EndWindow],[TextField1],[TextField2], [TextField3], [TextField4],
			   [StopStatusId], [StopPerformanceStatusId], [ByPassReasonId],[IsDeleted], [CreatedBy],[DateCreated],[UpdatedBy],[DateUpdated])
	VALUES ([PlannedStopNumber],[PlannedArriveTime],[PlannedDepartTime],[RouteHeaderCode],[RouteHeaderId],[DropId],[LocationId],[DeliveryDate],[SpecialInstructions],
			   [StartWindow],[EndWindow],[TextField1],[TextField2], [TextField3], [TextField4],
			   [StopStatusId], [StopPerformanceStatusId], [ByPassReasonId],[IsDeleted], [CreatedBy],[DateCreated],[UpdatedBy],[DateUpdated])

	OUTPUT $action, inserted.Id INTO @ChangeResult;

	SELECT Id FROM @ChangeResult

END