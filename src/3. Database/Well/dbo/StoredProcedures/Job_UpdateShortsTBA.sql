CREATE PROCEDURE [dbo].[Job_UpdateShortsTBA]
	@Ids dbo.IntTableType	READONLY
AS
BEGIN
	DECLARE @temp TABLE (
		Stopid INT, 
		OuterCount INT, 
		TotalShort INT, 
		DetailShort INT, 
		TotalLines INT
		)

	DECLARE @temp2 TABLE (
		Stopid INT, 
		OuterCount INT, 
		JobId INT, 
		UnknownDeliveryLineCount INT
		)

	DECLARE @temp3 TABLE (
		JobId INT, 
		DetailOutersShort INT, 
		Unconfirmed BIT, 
		DiscrepancyFound BIT
		)

	-- find all the jobs updated in this import
	-- group by stop & outer count, totalouters short is the same for all jobs in the group
	-- calculate the number of confirmed shorts and the number of lines in each group
	INSERT INTO @temp (Stopid ,OuterCount , TotalShort , DetailShort , TotalLines )
	SELECT StopId,  OuterCount,  j.TotalOutersShort, SUM(jd.ShortQty) as DetailOutersShort, COUNT(jd.Id) as NumberOfLines
	FROM job j 
	INNER JOIN @Ids ids ON ids.Value = j.Id
	INNER JOIN JobDetail jd ON jd.JobId = j.Id
	GROUP BY j.StopId, j.OuterCount, j.TotalOutersShort

	-- out of the job groups above, find the ones with unconfirmed delivery lines  
	INSERT INTO @temp2 (Stopid , OuterCount ,JobId, UnknownDeliveryLineCount)
	SELECT j.StopId, j.OuterCount,jd.JobId, COUNT(jd.Id) AS UnknownDeliveryStatus
	FROM @temp t
	INNER JOIN Job j ON t.Stopid = j.StopId AND t.OuterCount = j.OuterCount
	INNER JOIN JobDetail jd ON jd.jobid = j.id
	WHERE jd.LineDeliveryStatus = 'Unknown'
	GROUP BY j.StopId, j.OuterCount, jd.JobId

	-- for all the groups with unconfirmed lines
	-- find the job ids, the number of outers confirmed short for the group, whether any lines are unconfirmed
	-- and if there is a discrepancy between the outers confirmed short and the total short from the initial count
	
	INSERT INTO @temp3 (JobId, DetailOutersShort, Unconfirmed,  DiscrepancyFound )
	SELECT t2.JobId, t1.DetailShort, 
	CASE WHEN t2.UnknownDeliveryLineCount > 0 THEN 1 ELSE 0 END ,
	CASE WHEN ISNULL(t1.TotalShort,0) != t1.DetailShort THEN 1 ELSE 0 END 
	FROM @temp t1
	INNER JOIN @temp2 t2 ON t1.Stopid = t2.stopId AND t1.OuterCount = t2.OuterCount 

	-- If there is a discrepancy, and there are unconfirmed lines on the group of jobs, there are shorts to be advised.
	-- Update the job with the detail short figure and flag that a discrepancy is found
	BEGIN TRAN

		UPDATE j
		SET DetailOutersShort = t3.DetailOutersShort, OuterDiscrepancyFound = DiscrepancyFound
		FROM Job j 
		INNER JOIN @temp3 t3 ON j.id = t3.jobid

	COMMIT

	RETURN 0
END
