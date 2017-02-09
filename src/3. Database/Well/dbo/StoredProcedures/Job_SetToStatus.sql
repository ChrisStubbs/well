CREATE PROCEDURE [dbo].[Job_SetToStatus]
	@JobId				INT,
	@Status             INT
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE 
		[Job] 
	SET 
		[PerformanceStatusId] = @Status
	WHERE
		Id = @JobId
END
