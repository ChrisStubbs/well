CREATE PROCEDURE [dbo].[UserJobs_GetbyJobIds]
	@JobIds dbo.IntTableType	READONLY
AS
	SELECT 
		UserId,
		JobId
	From
		UserJob uj
	INNER JOIN @JobIds ids ON ids.Value = uj.JobId
	

