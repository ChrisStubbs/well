CREATE PROCEDURE [dbo].[Stop_Update]
	@Id				INT,
	@StopStatusCodeId	TINYINT,
	@StopPerformanceStatusCodeId INT,
	@ByPassReasonId         TINYINT,
	@PlannedStopNumber	VARCHAR(4),
	@ShellActionIndicator VARCHAR(100),
	@ActualPaymentCash		DECIMAL(7,2),
	@ActualPaymentCheque	DECIMAL(7,2),
	@ActualPaymentCard		DECIMAL(7,2),
	@UpdatedBy VARCHAR(50),
	@UpdatedDate DATETIME
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE 
		[Stop] 
	SET 
		[StopStatusId] = @StopStatusCodeId,
		StopPerformanceStatusId = @StopPerformanceStatusCodeId, 
		[ByPassReasonId] = @ByPassReasonId,
		PlannedStopNumber = @PlannedStopNumber,
		ShellActionIndicator = @ShellActionIndicator,
		ActualPaymentCash = @ActualPaymentCash,
		ActualPaymentCheque = @ActualPaymentCheque,
		ActualPaymentCard = @ActualPaymentCard,
		UpdatedBy = @UpdatedBy, 
		DateUpdated = @UpdatedDate
	WHERE
		Id = @Id
END

