CREATE PROCEDURE [dbo].[Stop_Update]
	@Id				INT,
	@StopStatusCode VARCHAR(50),
	@StopStatusDescription VARCHAR(255),
	@PerformanceStatusCode VARCHAR(50),
	@PerformanceStatusDescription VARCHAR(255),
	@Reason         VARCHAR(255),
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
		[StopStatusCode] = @StopStatusCode,
		StopStatusDescription = @StopStatusDescription,
		[PerformanceStatusCode] = @PerformanceStatusCode,
		PerformanceStatusDescription = @PerformanceStatusDescription, 
		[Reason] = @Reason,
		ShellActionIndicator = @ShellActionIndicator,
		ActualPaymentCash = @ActualPaymentCash,
		ActualPaymentCheque = @ActualPaymentCheque,
		ActualPaymentCard = @ActualPaymentCard,
		UpdatedBy = @UpdatedBy, 
		DateUpdated = @UpdatedDate
	WHERE
		Id = @Id
END

