CREATE PROCEDURE [dbo].[Job_Update]
	@Id				INT,
	@PerformanceStatus	TINYINT,
	@Reason         VARCHAR(255),
	@InvoiceNumber VARCHAR(50),
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
	@JobStatusId TINYINT,
	@OuterCount INT,
	@OuterDiscrepancyFound BIT,
	@TotalOutersOver INT,
	@TotalOutersShort INT,
	@InvoiceValue DECIMAL(8,2),
	@DetailOutersOver INT,
	@DetailOutersShort INT,
	@ResolutionStatusId INT
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE 
		[Job] 
	SET 
		[PerformanceStatusId] = @PerformanceStatus, 
		[Reason] = @Reason, 
		InvoiceNumber = @InvoiceNumber,
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
		JobStatusId = @JobStatusId,
		OuterCount = @OuterCount,
		OuterDiscrepancyFound = @OuterDiscrepancyFound,
		TotalOutersOver = @TotalOutersOver,
		TotalOutersShort = @TotalOutersShort,
		InvoiceValue = @InvoiceValue,
		DetailOutersOver = @DetailOutersOver,
		DetailOutersShort = @DetailOutersShort,
		ResolutionStatusId = @ResolutionStatusId
	WHERE
		Id = @Id
END