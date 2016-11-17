CREATE PROCEDURE [dbo].[Stops_CreateOrUpdate]
	@Id						INT = 0,
	@Username				VARCHAR(50),
	@TransportOrderReference VARCHAR(50),
	@PlannedStopNumber		VARCHAR(4),
	@RouteHeaderCode		VARCHAR(10),
	@RouteHeaderId			INT,
	@DropId					VARCHAR(2),
	@LocationId				VARCHAR(20),
	@DeliveryDate			DATETIME = null, 
	@ShellActionIndicator	VARCHAR(100),
	@CustomerShopReference	VARCHAR(100),
	@AllowOvers				BIT NULL=0,
	@CustUnatt				BIT NULL=0,
	@PHUnatt				BIT NULL=0,
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
		(@Id, @PlannedStopNumber, @TransportOrderReference, @RouteHeaderCode,  @RouteHeaderId, @DropId, @LocationId, @DeliveryDate,@ShellActionIndicator,@CustomerShopReference, @AllowOvers, @CustUnatt, @PHUnatt,
		 @StopStatusId,@StopPerformanceStatusId, @ByPassReasonId, @IsDeleted,   @Username, GETDATE(), @Username, GETDATE())
	)
	AS Source ([Id],[PlannedStopNumber],[TransportOrderReference],[RouteHeaderCode],[RouteHeaderId],[DropId],[LocationId],[DeliveryDate],[ShellActionIndicator], [CustomerShopReference], [AllowOvers], [CustUnatt], [PHUnatt],
			   [StopStatusId], [StopPerformanceStatusId], [ByPassReasonId],[IsDeleted], [CreatedBy],[DateCreated],[UpdatedBy],[DateUpdated])
	ON Target.[Id] = Source.[Id]
	WHEN MATCHED THEN
	UPDATE SET
		[PlannedStopNumber] = Source.[PlannedStopNumber],
		[TransportOrderReference] = Source.[TransportOrderReference],
		[RouteHeaderCode] = Source.[RouteHeaderCode],
		[RouteHeaderId] = Source.[RouteHeaderId],
		[DropId] = Source.[DropId],
		[LocationId] = Source.[LocationId],
		[DeliveryDate] = Source.[DeliveryDate],
		[ShellActionIndicator] = Source.[ShellActionIndicator],
		[CustomerShopReference] = Source.[CustomerShopReference],
		[AllowOvers] = Source.[AllowOvers], 
		[CustUnatt] = Source.[CustUnatt], 
		[PHUnatt] = Source.[PHUnatt],
		[StopStatusId] = Source.[StopStatusId],
		[StopPerformanceStatusId] = Source.[StopPerformanceStatusId],
		[ByPassReasonId] = Source.[ByPassReasonId],
		[IsDeleted] = Source.[IsDeleted],
		[CreatedBy] = Source.[CreatedBy],
		[DateCreated] = Source.[DateCreated],
		[UpdatedBy] = Source.[UpdatedBy],
		[DateUpdated] = Source.[DateUpdated]
	WHEN NOT MATCHED BY TARGET AND @Id = 0 THEN
	INSERT ([PlannedStopNumber],[TransportOrderReference],[RouteHeaderCode],[RouteHeaderId],[DropId],[LocationId],[DeliveryDate],[ShellActionIndicator], [CustomerShopReference], [AllowOvers], [CustUnatt], [PHUnatt],
			   [StopStatusId], [StopPerformanceStatusId], [ByPassReasonId],[IsDeleted], [CreatedBy],[DateCreated],[UpdatedBy],[DateUpdated])
	VALUES ([PlannedStopNumber],[TransportOrderReference], [RouteHeaderCode],[RouteHeaderId],[DropId],[LocationId],[DeliveryDate],[ShellActionIndicator], [CustomerShopReference], [AllowOvers], [CustUnatt], [PHUnatt],
			   [StopStatusId], [StopPerformanceStatusId], [ByPassReasonId],[IsDeleted], [CreatedBy],[DateCreated],[UpdatedBy],[DateUpdated])

	OUTPUT $action, inserted.Id INTO @ChangeResult;

	SELECT Id FROM @ChangeResult

END