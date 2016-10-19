CREATE PROCEDURE [dbo].[ResolveJobAndJobDetails]
	@jobId INT
AS
BEGIN
	BEGIN TRANSACTION
	Update 
		job
	SET 
		PerformanceStatusId = 8
	WHERE 
		Id = @jobId

	UPDATE
		JobDetail
	SET
		JobDetailStatusId = 1
	WHERE
		JobId = @jobId
	COMMIT
END
