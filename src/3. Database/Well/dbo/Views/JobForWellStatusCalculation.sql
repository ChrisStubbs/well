CREATE VIEW [dbo].[JobForWellStatusCalculation]
	AS
	SELECT 
		j.Id,
		j.StopId,
		j.ResolutionStatusId AS ResolutionStatus,
		j.JobStatusId AS JobStatus,
		j.WellStatusId AS WellStatus	
	FROM Job j
