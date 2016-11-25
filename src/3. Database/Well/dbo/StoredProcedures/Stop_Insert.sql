﻿CREATE PROCEDURE [dbo].[Stop_Insert]
	@TransportOrderReference VARCHAR(50),
	@PlannedStopNumber		VARCHAR(4),
	@RouteHeaderCode		VARCHAR(10),
	@RouteHeaderId			INT,
	@DropId					VARCHAR(2),
	@LocationId				VARCHAR(20),
	@DeliveryDate			DATETIME = null, 
	@ShellActionIndicator	VARCHAR(100),
	@AllowOvers				BIT NULL,
	@CustUnatt				BIT NULL,
	@PHUnatt				BIT NULL,
	@StopStatusId			TINYINT,
	@StopPerformanceStatusId TINYINT,
	@ByPassReasonId			TINYINT,
	@ActualPaymentCash		DECIMAL(7,2),
	@ActualPaymentCheque	DECIMAL(7,2),
	@ActualPaymentCard		DECIMAL(7,2),
	@AccountBalance         DECIMAL(7,2),
	@CreatedBy				VARCHAR(50),
	@CreatedDate			DATETIME,
	@UpdatedBy				VARCHAR(50),
	@UpdatedDate			DATETIME
AS
BEGIN
	SET NOCOUNT ON;

	INSERT [Stop] (
		[PlannedStopNumber],
		[TransportOrderReference],
		[RouteHeaderCode],
		[RouteHeaderId],
		[DropId],
		[LocationId],
		[DeliveryDate],
		[ShellActionIndicator], 
		[AllowOvers], 
		[CustUnatt], 
		[PHUnatt],
		[StopStatusId], 
		[StopPerformanceStatusId], 
		[ByPassReasonId], 
		[ActualPaymentCash], 
		[ActualPaymentCheque], 
		[ActualPaymentCard],
		[AccountBalance],
		[CreatedBy],
		[DateCreated],
		[UpdatedBy],
		[DateUpdated])
	VALUES (
		@PlannedStopNumber, 
		@TransportOrderReference, 
		@RouteHeaderCode,  
		@RouteHeaderId, 
		@DropId, 
		@LocationId, 
		@DeliveryDate,
		@ShellActionIndicator,
		@AllowOvers, 
		@CustUnatt, 
		@PHUnatt,
		@StopStatusId,
		@StopPerformanceStatusId, 
		@ByPassReasonId,   
		@ActualPaymentCash, 
		@ActualPaymentCheque, 
		@ActualPaymentCard,
		@AccountBalance,
		@CreatedBy, 
		@CreatedDate, 
		@UpdatedBy, 
		@UpdatedDate)

	SELECT CAST(SCOPE_IDENTITY() as int);

END