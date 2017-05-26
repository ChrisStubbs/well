--SET NUMERIC_ROUNDABORT OFF;  
--GO
--SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT,  
--    QUOTED_IDENTIFIER, ANSI_NULLS ON; 
--GO

CREATE VIEW [dbo].[StopStatusView]
AS


	WITH StatusForStops (StopId, AwaitingInvoice, Incomplete, Clean, Exception, Resolved, Documents, CompletedOnPaper, Bypassed, TotalNoOfJobs)
		AS(
			SELECT StopId, [1] AS AwaitingInvoice, [2] AS Incomplete, [3] AS Clean, [4] AS Exception, [5] AS Resolved, [6] AS Documents, [7] CompletedOnPaper, [8] AS Bypassed, ([1] + [2] + [3] + [4] + [5] + [6] + [7]) As TotalNoOfJobs 
			FROM
				(SELECT s.Id AS StopId, j.JobStatusId AS JobStatusId , COUNT(j.JobStatusId) AS JobCount
				FROM [Stop] s
				INNER JOIN Job j ON j.StopId = s.id
				GROUP BY s.Id, j.JobStatusId)
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



