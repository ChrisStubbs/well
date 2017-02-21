CREATE PROCEDURE [dbo].[Job_Update]
	@Id				INT,
	@PerformanceStatus	TINYINT,
	@Reason         VARCHAR(255),
	@InvoiceNumber VARCHAR(50),
	@CreditValue DECIMAL(8, 2),
	@JobTypeCode VARCHAR(10),
	@PhAccount VARCHAR(40),
	@GrnNumber VARCHAR(50),
	@Sequence INT,
	@CustomerRef VARCHAR(40),
	@UpdatedBy VARCHAR(50),
	@UpdatedDate DATETIME,
	@Picked BIT,
	@OrdOuters INT,
	@InvOuters INT,
	@AllowSoCrd BIT,
	@Cod VARCHAR(50),
	@AllowReOrd BIT,
	@HasException BIT
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE 
		[Job] 
	SET 
		[PerformanceStatusId] = @PerformanceStatus, 
		[Reason] = @Reason, 
		InvoiceNumber = @InvoiceNumber,
		TotalCreditValueForThreshold = @CreditValue,
		[Sequence] = @Sequence,
		JobTypeCode = @JobTypeCode,
		GrnNumber = @GrnNumber,
		PHAccount = @PhAccount,
		CustomerRef = @CustomerRef,
		UpdatedBy = @UpdatedBy, 
		DateUpdated = @UpdatedDate,
		Picked = @Picked,
		OrdOuters = @OrdOuters,
		InvOuters = @InvOuters,
		AllowSOCRD = @AllowSoCrd,
		Cod = @Cod,
		AllowReOrd = @AllowReOrd,
		HasException = @HasException
	WHERE
		Id = @Id
END

