CREATE PROCEDURE UpdateReinstateJob
	@Id	INT,
	@JobStatusId TINYINT,
	@WellStatusId TINYINT,
	@ResolutionStatusId INT,
	@Sequence INT,
	@JobTypeCode VARCHAR(10),
	@PhAccount VARCHAR(40),
    @PickListRef VarChar(40),
	@InvoiceNumber VARCHAR(50),
	@CustomerRef VARCHAR(40),
	@PerformanceStatus	TINYINT,
	@Picked BIT,
	@OrdOuters INT,
	@InvOuters INT,
	@AllowSoCrd BIT,
	@Cod VARCHAR(50),
	@AllowReOrd BIT,
	@TotalOutersShort INT,
	@DetailOutersOver Int,
	@TotalOutersOver Int,
	@OuterCount Int
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE 
		[Job] 
	SET 
		JobStatusId = @JobStatusId,
		WellStatusId = @WellStatusId,
		ResolutionStatusId = @ResolutionStatusId,
		[Sequence] = @Sequence,
		JobTypeCode = @JobTypeCode,
		PHAccount = @PhAccount,
        PickListRef = @PickListRef,
		InvoiceNumber = @InvoiceNumber,
		CustomerRef = @CustomerRef,
		[PerformanceStatusId] = @PerformanceStatus, 
		Picked = @Picked,
		OrdOuters = @OrdOuters,
		InvOuters = @InvOuters,
		AllowSOCRD = @AllowSoCrd,
		Cod = @Cod,
		AllowReOrd = @AllowReOrd,
		TotalOutersShort = @TotalOutersShort,
		DetailOutersOver = @DetailOutersOver,
		TotalOutersOver = @TotalOutersOver,
		OuterCount = @OuterCount
	WHERE
		Id = @Id
END