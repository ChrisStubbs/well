--SET NUMERIC_ROUNDABORT OFF;  
--GO
--SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT,  
--    QUOTED_IDENTIFIER, ANSI_NULLS ON; 
--GO

CREATE VIEW [dbo].[StopStatusView]
AS
	WITH StatusForStops (StopId, AwaitingInvoice, Incomplete, Clean, Exception, Resolved, Documents, CompletedOnPaper, Bypassed, TotalNoOfJobs)
		AS(
			SELECT StopId, ISNULL( [1], 0) AS AwaitingInvoice, ISNULL( [2], 0 ) AS Incomplete, ISNULL ([3], 0) AS Clean, ISNULL ([4], 0) AS Exception, ISNULL ([5], 0) AS Resolved, ISNULL ([6], 0) AS Documents, ISNULL ([7], 0) AS CompletedOnPaper, ISNULL ([8], 0) AS Bypassed, TotalJobs 
			FROM
				(SELECT s.Id AS StopId, j.JobStatusId AS JobStatusId , COUNT(j.JobStatusId) AS JobCount
				,COUNT(j.Id)  OVER (PARTITION BY s.Id) AS TotalJobs
				FROM [Stop] s
				INNER JOIN Job j ON j.StopId = s.id
				AND j.DateDeleted IS NULL
				WHERE j.JobTypeCode != 'DEL-DOC'
				GROUP BY s.Id, j.JobStatusId, j.id)
			AS SourceTable
			PIVOT
				(SUM(JobCount)
				For JobStatusId in ([0], [1], [2], [3], [4], [5], [6], [7], [8])
				)
			AS PivotTable
)

	SELECT StopId
		,CASE WHEN AwaitingInvoice > 0 THEN 1		-- Planned
			WHEN Incomplete > 0 THEN 2				-- Invoiced	
			WHEN Bypassed > 0 AND (TotalNoOfJobs = Bypassed) THEN 4  -- all jobs bypassed
			ELSE 3									--otherwise complete
		END AS WellStatusId
	FROM StatusForStops



