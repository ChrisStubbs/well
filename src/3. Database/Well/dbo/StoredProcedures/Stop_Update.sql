CREATE PROCEDURE [dbo].[Stop_Update]
		@Id INT,
		@TransportOrderReference VARCHAR(50),
		@PlannedStopNumber VARCHAR(4),
		@RouteHeaderCode VARCHAR(10),
		@RouteHeaderId INT,
		@DropId VARCHAR(2),
		@Previously VARCHAR(50),
		@LocationId  VARCHAR(20),
		@DeliveryDate DATETIME,
		@ShellActionIndicator VARCHAR(100),
		@AllowOvers BIT,
		@CustUnatt BIT,
		@PHUnatt BIT,
		@StopStatusCode VARCHAR(50),
		@StopStatusDescription VARCHAR(255),
		@PerformanceStatusCode VARCHAR(50),
		@PerformanceStatusDescription VARCHAR(255),
		@Reason VARCHAR(255),
		@DateDeleted DATETIME = NULL,
		@ActualPaymentCash DECIMAL(7,2),
		@ActualPaymentCheque DECIMAL(7,2),
		@ActualPaymentCard DECIMAL(7,2),
		@AccountBalance DECIMAL(7,2),
		@UpdatedBy VARCHAR(50),
		@DateUpdated DATETIME
		--@Location_Id INT
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[Stop]
	   SET [TransportOrderReference]		= @TransportOrderReference
		  ,[PlannedStopNumber]				= @PlannedStopNumber
		  ,[RouteHeaderCode]				= @RouteHeaderCode
		  ,[RouteHeaderId]					= @RouteHeaderId
		  ,[DropId]							= @DropId
		  ,[Previously]						= @Previously
		  ,[LocationId]						= @LocationId
		  ,[DeliveryDate]					= @DeliveryDate
		  ,[ShellActionIndicator]			= @ShellActionIndicator
		  ,[AllowOvers]						= @AllowOvers
		  ,[CustUnatt]						= @CustUnatt
		  ,[PHUnatt]						= @PHUnatt
		  ,[StopStatusCode]					= @StopStatusCode
		  ,[StopStatusDescription]			= @StopStatusDescription
		  ,[PerformanceStatusCode]			= @PerformanceStatusCode
		  ,[PerformanceStatusDescription]	= @PerformanceStatusDescription
		  ,[Reason]							= @Reason
		  ,[DateDeleted]					= @DateDeleted
		  ,[ActualPaymentCash]				= @ActualPaymentCash
		  ,[ActualPaymentCheque]			= @ActualPaymentCheque
		  ,[ActualPaymentCard]				= @ActualPaymentCard
		  ,[AccountBalance]					= @AccountBalance
		  ,[UpdatedBy]						= @UpdatedBy
		  ,[DateUpdated]					= @DateUpdated
		 -- ,[Location_Id]					= @Location_Id
	 WHERE
		Id = @Id
END

