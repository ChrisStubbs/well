CREATE PROCEDURE [dbo].[ResolveJobAndJobDetails]
	@jobId INT
AS
BEGIN
	BEGIN TRANSACTION
	Update 
		job
	SET 
		JobStatusId = 5
	WHERE 
		Id = @jobId

	--TODO potential to refactor the status out to explicit flags of resolved, awaiting invoice etc
	UPDATE
		JobDetail
	SET
		JobDetailStatusId = 1
	WHERE
		JobId = @jobId
	COMMIT
END
