CREATE PROCEDURE [dbo].[Stop_Update]
	@Id				INT,
	@StopStatusCodeId	TINYINT,
	@StopPerformanceStatusCodeId INT,
	@ByPassReasonId         TINYINT,
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
		UpdatedBy = @UpdatedBy, 
		DateUpdated = @UpdatedDate
	WHERE
		Id = @Id
END

