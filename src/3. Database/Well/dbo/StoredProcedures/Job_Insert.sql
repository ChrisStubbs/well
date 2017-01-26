﻿CREATE PROCEDURE [dbo].[Job_Insert]
	@Sequence				INT,
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
	@COD					VARCHAR(50) = NULL,
	@GrnProcessingType      INT = 0,
	@AllowReOrd				BIT = 0,
	@SandwchOrd				BIT = 0,
	@PerformanceStatusId	TINYINT = 6,
	@Reason					VARCHAR(255),
	@StopId					INT,
	@ActionLogNumber	    VARCHAR(50) = NULL,
	@OuterCount				TINYINT = NULL,
	@OuterDiscrepancyFound 	BIT = 0,
	@TotalOutersOver		INT = NULL,
	@TotalOutersShort		INT = NULL,
	@Picked					BIT = 0,
	@InvoiceValue			DECIMAL(8,3) = NULL,
	@ProofOfDelivery		INT = 0,
	@CreatedBy VARCHAR(50),
	@UpdatedBy VARCHAR(50),
	@CreatedDate DATETIME,
	@UpdatedDate DATETIME
AS
BEGIN
	SET NOCOUNT ON;

	INSERT [Job] (
		[Sequence],
		[JobTypeCode],
		[PHAccount], 
		[PickListRef], 
		[InvoiceNumber], 
		[CustomerRef], 
		[OrderDate], 
		[RoyaltyCode], 
		[RoyaltyCodeDesc], 
		[OrdOuters], 
		[InvOuters], 
		[ColOuters], 
		[ColBoxes], 
		[ReCallPrd], 
		[AllowSOCrd],
		[COD], 
		[GrnProcessType],
		[AllowReOrd], 
		[SandwchOrd], 
		[PerformanceStatusId],
		[Reason], 
		[StopId], 
		[ActionLogNumber], 
		[OuterCount], 
		[OuterDiscrepancyFound], 
		[TotalOutersOver], 
		[TotalOutersShort],
		[Picked],
		[InvoiceValue],
		[ProofOfDelivery],
		[CreatedBy],
		[DateCreated],
		[UpdatedBy],
		[DateUpdated])
	VALUES (
		@Sequence, 
		@JobTypeCode, 
		@PHAccount, 
		@PickListRef, 
		@InvoiceNumber, 
		@CustomerRef, 
		@OrderDate, 
		@RoyaltyCode, 
		@RoyaltyCodeDesc,
		@OrdOuters, 
		@InvOuters, 
		@ColOuters, 
		@ColBoxes, 
		@ReCallPrd, 
		@AllowSOCrd, 
		@COD, 
		@GrnProcessingType,
		@AllowReOrd, 
		@SandwchOrd, 
		@PerformanceStatusId,
		@Reason, 
		@StopId, 
		@ActionLogNumber, 
		@OuterCount, 
		@OuterDiscrepancyFound, 
		@TotalOutersOver, 
		@TotalOutersShort,
		@Picked,
		@InvoiceValue,
		@ProofOfDelivery,
		@CreatedBy,
		@CreatedDate,
		@UpdatedBy,
		@UpdatedDate)

	SELECT CAST(SCOPE_IDENTITY() as int);
END

