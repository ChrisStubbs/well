﻿CREATE PROCEDURE [dbo].[Job_Update]
	@Id				INT,
	@PerformanceStatus	TINYINT,
	@ByPassReason         TINYINT,
	@InvoiceNumber VARCHAR(50),
	@JobTypeCode VARCHAR(10),
	@PhAccount VARCHAR(40),
	@Sequence INT,
	@CustomerRef VARCHAR(40),
	@UpdatedBy VARCHAR(50),
	@UpdatedDate DATETIME
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE 
		[Job] 
	SET 
		[PerformanceStatusId] = @PerformanceStatus, 
		[ByPassReasonId] = @ByPassReason, 
		InvoiceNumber = @InvoiceNumber,
		[Sequence] = @Sequence,
		JobTypeCode = @JobTypeCode,
		PHAccount = @PhAccount,
		CustomerRef = @CustomerRef,
		UpdatedBy = @UpdatedBy, 
		DateUpdated = @UpdatedDate
	WHERE
		Id = @Id
END
