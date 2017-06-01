CREATE PROCEDURE [dbo].[UserJobs_GetbyJobIds]
	@JobIdsIds dbo.IntTableType	READONLY
AS
	SELECT 
		UserId,
		JobId
	From
		UserJob uj
	INNER JOIN @JobIdsIds ids ON ids.Value = uj.JobId
	

