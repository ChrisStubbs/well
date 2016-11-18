CREATE PROCEDURE [dbo].[Stops_CreateOrUpdate]
	@Id						INT = 0,
	@Username				NVARCHAR(50),
	@PlannedStopNumber		NVARCHAR(4),
	@RouteHeaderCode		NVARCHAR(10),
	@RouteHeaderId			INT,
	@DropId					NVARCHAR(2),
	@LocationId				NVARCHAR(20),
	@DeliveryDate			DATETIME , 
	@ShellActionIndicator	NVARCHAR(100),
	@CustomerShopReference	NVARCHAR(100),
	@AllowOvers				BIT NULL=0,
	@CustUnatt				BIT NULL=0,
	@PHUnatt				BIT NULL=0,
	@StopStatusId			TINYINT= 4,
	@StopPerformanceStatusId TINYINT= 6,
	@ByPassReasonId			TINYINT= 13,
	@IsDeleted				BIT=0,
	@ActualPaymentCash		DECIMAL(7,2),
	@ActualPaymentCheque	DECIMAL(7,2),
	@ActualPaymentCard		DECIMAL(7,2)


AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @ChangeResult TABLE (ChangeType VARCHAR(10), Id INTEGER)

	MERGE INTO [Stop] AS Target
	USING (VALUES
		(@Id, @PlannedStopNumber, @RouteHeaderCode,  @RouteHeaderId, @DropId, @LocationId, @DeliveryDate,@ShellActionIndicator,@CustomerShopReference, @AllowOvers, @CustUnatt, @PHUnatt,
		 @StopStatusId,@StopPerformanceStatusId, @ByPassReasonId, @IsDeleted, @ActualPaymentCash, @ActualPaymentCheque, @ActualPaymentCard ,  @Username, GETDATE(), @Username, GETDATE())
	)
	AS Source ([Id],[PlannedStopNumber],[RouteHeaderCode],[RouteHeaderId],[DropId],[LocationId],[DeliveryDate],[ShellActionIndicator], [CustomerShopReference], [AllowOvers], [CustUnatt], [PHUnatt],
			   [StopStatusId], [StopPerformanceStatusId], [ByPassReasonId],[IsDeleted], [ActualPaymentCash], [ActualPaymentCheque], [ActualPaymentCard], [CreatedBy],[DateCreated],[UpdatedBy],[DateUpdated])
	ON Target.[Id] = Source.[Id]
	WHEN MATCHED THEN
	UPDATE SET
		[PlannedStopNumber] = Source.[PlannedStopNumber],
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
		[ActualPaymentCash] = Source.[ActualPaymentCash], 
		[ActualPaymentCheque] = Source.[ActualPaymentCheque],
		[ActualPaymentCard] = Source.[ActualPaymentCard],
		[CreatedBy] = Source.[CreatedBy],
		[DateCreated] = Source.[DateCreated],
		[UpdatedBy] = Source.[UpdatedBy],
		[DateUpdated] = Source.[DateUpdated]
	WHEN NOT MATCHED BY TARGET AND @Id = 0 THEN
	INSERT ([PlannedStopNumber],[RouteHeaderCode],[RouteHeaderId],[DropId],[LocationId],[DeliveryDate],[ShellActionIndicator], [CustomerShopReference], [AllowOvers], [CustUnatt], [PHUnatt],
			   [StopStatusId], [StopPerformanceStatusId], [ByPassReasonId],[IsDeleted], [ActualPaymentCash], [ActualPaymentCheque], [ActualPaymentCard],[CreatedBy],[DateCreated],[UpdatedBy],[DateUpdated])
	VALUES ([PlannedStopNumber],[RouteHeaderCode],[RouteHeaderId],[DropId],[LocationId],[DeliveryDate],[ShellActionIndicator], [CustomerShopReference], [AllowOvers], [CustUnatt], [PHUnatt],
			   [StopStatusId], [StopPerformanceStatusId], [ByPassReasonId],[IsDeleted],[ActualPaymentCash], [ActualPaymentCheque], [ActualPaymentCard], [CreatedBy],[DateCreated],[UpdatedBy],[DateUpdated])

	OUTPUT $action, inserted.Id INTO @ChangeResult;

	SELECT Id FROM @ChangeResult

END