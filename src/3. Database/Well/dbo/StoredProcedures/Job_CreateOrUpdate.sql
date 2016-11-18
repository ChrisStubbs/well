CREATE PROCEDURE [dbo].[Job_CreateOrUpdate]
	@Id						INT = 0,
	@Sequence				INT,
	@Username				VARCHAR(50),
	@JobTypeCode			VARCHAR(50),
	@PHAccount				VARCHAR(50)=NULL,
	@PickListRef			VARCHAR(50)=NULL,
	@InvoiceNumber			VARCHAR(50)=NULL,
	@CustomerRef			VARCHAR(10)=NULL,
	@OrderDate				VARCHAR(50) = NULL,
	@RoyaltyCode			VARCHAR(8)=NULL,
	@RoyaltyCodeDesc		VARCHAR(50)=NULL,
	@OrdOuters				INT = 0,
	@InvOuters				INT = 0,
	@ColOuters				INT = 0,
	@ColBoxes				INT = 0,
	@ReCallPrd				BIT = 0,
	@AllowSgCrd				BIT = 0,
	@AllowSOCrd				BIT = 0,
	@COD					BIT = 0,
	@GrnNumber				VARCHAR(50) = NULL,
	@GrnRefusedReason		VARCHAR(50)=NULL,
	@GrnRefusedDesc			VARCHAR(50) = NULL,
	@AllowReOrd				BIT = 0,
	@SandwchOrd				BIT = 0,
	@ComdtyType				VARCHAR(1) = NULL,
	@TotalCreditValueForThreshold DECIMAL(8,3) NULL,
	@PerformanceStatusId	TINYINT = 6,
	@ByPassReasonId         TINYINT = 13,
	@StopId					INT,
	@ActionLogNumber	    VARCHAR(50) = NULL,
	@OuterCount				TINYINT = NULL,
	@OuterDiscrepancyFound 	BIT = 0,
	@TotalOutersOver		INT = NULL,
	@TotalOutersShort		INT = NULL


AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @ChangeResult TABLE (ChangeType VARCHAR(10), Id INTEGER)

	MERGE INTO [Job] AS Target
	USING (VALUES
		(@Id, @Sequence, @JobTypeCode, @PHAccount, @PickListRef, @InvoiceNumber, @CustomerRef, @OrderDate, @RoyaltyCode, @RoyaltyCodeDesc,
		@OrdOuters, @InvOuters, @ColOuters, @ColBoxes, @ReCallPrd, @AllowSgCrd, @AllowSOCrd, @COD, @GrnNumber, @GrnRefusedReason, @GrnRefusedDesc,
		@AllowReOrd, @SandwchOrd, @ComdtyType, @TotalCreditValueForThreshold,   @PerformanceStatusId,@ByPassReasonId, @StopId, 
		@ActionLogNumber, @OuterCount, @OuterDiscrepancyFound, @TotalOutersOver, @TotalOutersShort, @Username, GETDATE(), @Username, GETDATE())
	)
	AS Source ([Id],[Sequence],[JobTypeCode],[PHAccount], [PickListRef], [InvoiceNumber], [CustomerRef], [OrderDate],
				[RoyaltyCode], [RoyaltyCodeDesc], [OrdOuters], [InvOuters], [ColOuters], [ColBoxes], [ReCallPrd], [AllowSgCrd], [AllowSOCrd],
				[COD], [GrnNumber], [GrnRefusedReason], [GrnRefusedDesc], [AllowReOrd], [SandwchOrd], [ComdtyType],	[TotalCreditValueForThreshold],
				[PerformanceStatusId],[ByPassReasonId], [StopId], [ActionLogNumber], [OuterCount], [OuterDiscrepancyFound], [TotalOutersOver], [TotalOutersShort],
				[CreatedBy],[DateCreated],[UpdatedBy],[DateUpdated])
	ON Target.[Id] = Source.[Id]
	WHEN MATCHED THEN
	UPDATE SET
		[Sequence] = Source.[Sequence],
		[JobTypeCode] = Source.[JobTypeCode],
		[PHAccount] = Source.[PHAccount],
		[PickListRef] = Source.[PickListRef],
		[InvoiceNumber] = Source.[InvoiceNumber],
		[CustomerRef] = Source.[CustomerRef],
		[OrderDate] = Source.[OrderDate],
		[RoyaltyCode] = Source.[RoyaltyCode],
		[RoyaltyCodeDesc] = Source.[RoyaltyCodeDesc],
		[OrdOuters] = Source.[OrdOuters],
		[InvOuters] = Source.[InvOuters],
		[ColOuters] = Source.[ColOuters],
		[ColBoxes] = Source.[ColBoxes],
		[ReCallPrd] = Source.[ReCallPrd],
		[AllowSgCrd] = Source.[AllowSgCrd],
		[AllowSOCrd] = Source.[AllowSOCrd],
		[COD] = Source.[COD],
		[GrnNumber] = Source.[GrnNumber],
		[GrnRefusedReason] = Source.[GrnRefusedReason],
		[GrnRefusedDesc] = Source.[GrnRefusedDesc],
		[AllowReOrd] = Source.[AllowReOrd],
		[SandwchOrd] = Source.[SandwchOrd],
		[ComdtyType] = Source.[ComdtyType],
		[TotalCreditValueForThreshold] = Source.[TotalCreditValueForThreshold],
		[PerformanceStatusId] = Source.[PerformanceStatusId],
		[ByPassReasonId] = Source.[ByPassReasonId],
		[StopId] = Source.[StopId],
		[ActionLogNumber] = Source.[ActionLogNumber], 
		[OuterCount] = Source.[OuterCount], 
		[OuterDiscrepancyFound] = Source.[OuterDiscrepancyFound],
		[TotalOutersOver] = Source.[TotalOutersOver], 
		[TotalOutersShort] = Source.[TotalOutersShort],
		[UpdatedBy] = Source.[UpdatedBy],
		[DateUpdated] = Source.[DateUpdated]
	WHEN NOT MATCHED BY TARGET AND @Id = 0 THEN
	INSERT ([Sequence],[JobTypeCode],[PHAccount], [PickListRef], [InvoiceNumber], [CustomerRef], [OrderDate], 
				[RoyaltyCode], [RoyaltyCodeDesc], [OrdOuters], [InvOuters], [ColOuters], [ColBoxes], [ReCallPrd], [AllowSgCrd], [AllowSOCrd],
				[COD], [GrnNumber], [GrnRefusedReason], [GrnRefusedDesc], [AllowReOrd], [SandwchOrd], [ComdtyType],	[TotalCreditValueForThreshold],	
				[PerformanceStatusId],[ByPassReasonId], [StopId], [ActionLogNumber], [OuterCount], [OuterDiscrepancyFound], [TotalOutersOver], [TotalOutersShort], 
				[CreatedBy],[DateCreated],[UpdatedBy],[DateUpdated])
	VALUES ([Sequence],[JobTypeCode],[PHAccount], [PickListRef], [InvoiceNumber], [CustomerRef], [OrderDate], 
				[RoyaltyCode], [RoyaltyCodeDesc], [OrdOuters], [InvOuters], [ColOuters], [ColBoxes], [ReCallPrd], [AllowSgCrd], [AllowSOCrd],
				[COD], [GrnNumber], [GrnRefusedReason], [GrnRefusedDesc], [AllowReOrd], [SandwchOrd], [ComdtyType],	[TotalCreditValueForThreshold],
				[PerformanceStatusId],[ByPassReasonId], [StopId],  [ActionLogNumber],[OuterCount], [OuterDiscrepancyFound], [TotalOutersOver], [TotalOutersShort],
				[CreatedBy],[DateCreated],[UpdatedBy],[DateUpdated])

	OUTPUT $action, inserted.Id INTO @ChangeResult;

	SELECT Id FROM @ChangeResult

END

