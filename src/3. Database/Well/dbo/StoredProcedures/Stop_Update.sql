CREATE PROCEDURE [dbo].[Stop_Update]
	@Id				INT,
	@StopStatusCodeId	TINYINT,
	@StopPerformanceStatusCodeId INT,
	@ByPassReasonId         TINYINT,
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
		ActualPaymentCash = @ActualPaymentCash,
		ActualPaymentCheque = @ActualPaymentCheque,
		ActualPaymentCard = @ActualPaymentCard,
		UpdatedBy = @UpdatedBy, 
		DateUpdated = @UpdatedDate
	WHERE
		Id = @Id
END

