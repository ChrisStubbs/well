CREATE PROCEDURE [dbo].[Job_Update]
	@Id				INT,
	@PerformanceStatus	TINYINT,
	@ByPassReason         TINYINT,
	@InvoiceNumber VARCHAR(50),
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
		UpdatedBy = @UpdatedBy, 
		DateUpdated = @UpdatedDate
	WHERE
		Id = @Id
END

