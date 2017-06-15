﻿CREATE PROCEDURE [dbo].[Routes_GetAllForBranch]
	@BranchId INT,
	@UserName VARCHAR(500)
AS
BEGIN
DECLARE  @JobStatus_Bypass INT = 8
		,@JobStatus_Exception INT = 4
		,@JobStatus_Clean INT = 3
	
	DECLARE @UserBranches Table(BranchId INT NOT NULL);

	INSERT INTO @UserBranches
	SELECT	BranchId 
	FROM	UserBranch ub
	INNER JOIN [User] u on u.Id = ub.UserId
	WHERE u.IdentityName = @UserName
		AND BranchId = @BranchId

	;WITH JobStatusCheck
	AS
	(
	SELECT	Stopid, JobStatusId
			,COUNT(*)  OVER (PARTITION BY StopId, JobStatusId) AS JobCountPerStatus
			,COUNT(*)  OVER (PARTITION BY Stopid) AS TotalJobs
	FROM Job j
	WHERE JobTypeCode NOT IN ('DEL-DOC', 'NOTDEF', 'UPL-SAN')
	), 
	RouteCleanCount AS
	(
		SELECT RouteHeaderId
			,COUNT(1) AS CleanCount
		FROM [Stop]
		WHERE Id IN	(	SELECT	StopId 
						FROM	JobStatusCheck
						WHERE	JobCountPerStatus = TotalJobs
						AND JobStatusId = @JobStatus_Clean)
		GROUP BY RouteHeaderId
	),
	RouteExceptionCount AS
	(
		SELECT RouteHeaderId
			,COUNT(1) AS ExceptionCount 
		FROM [Stop]
		WHERE Id IN (	SELECT	Stopid 
						FROM	Job
						WHERE	JobStatusId IN (@JobStatus_Exception, @JobStatus_Bypass)
					)
		GROUP BY RouteHeaderId
	)
	SELECT rh.Id
		   ,RouteOwnerId AS BranchId
		   ,b.Name AS BranchName	
		   ,RouteNumber
		   ,RouteDate
		   ,PlannedStops AS StopCount
		   ,ws.DisplayName AS RouteStatus
		   ,rsv.RouteStatus AS RouteStatusId
		   ,ISNULL(rec.ExceptionCount,0) AS ExceptionCount  -- count of stops with exceptions or bypassed
		   ,ISNULL(rcc.CleanCount,0) AS CleanCount  -- count of stops with all clean
		   ,DriverName
	FROM
		RouteHeader rh
	INNER JOIN Branch b on rh.RouteOwnerId = b.Id
	LEFT JOIN RouteExceptionCount rec ON rec.RouteHeaderId = rh.Id
	LEFT JOIN RouteCleanCount rcc ON rcc.RouteHeaderId = rh.Id
	LEFT JOIN RouteStatusView rsv ON rsv.RouteHeaderId = rh.Id
	--LEFT JOIN RouteStatus rs ON rs.Id = rsv.RouteStatus
	LEFT JOIN WellStatus ws ON ws.Id = rsv.RouteStatus
    WHERE 
		rh.DateDeleted IS NULL
		AND rh.RouteOwnerId in (Select BranchId FROM @UserBranches)
	ORDER BY rh.RouteDate DESC

	SELECT DISTINCT 
			rh.Id RouteId
			,j.Id as JobId
			,s.Id StopId
			,jobUser.Name
	FROM RouteHeader rh
	INNER JOIN 
		Branch b ON rh.RouteOwnerId = b.Id
	INNER JOIN
		[Stop] s ON s.RouteHeaderId = rh.Id
	INNER JOIN
		Job j ON j.StopId = s.Id
	INNER JOIN 
		UserJob uj ON uj.JobId = j.Id
	INNER JOIN 
		[User] jobUser ON uj.UserId = jobUser.Id
	WHERE 
		rh.DateDeleted IS NULL
		AND rh.RouteOwnerId in (Select BranchId FROM @UserBranches)

	SELECT 
		s.RouteHeaderId as RouteId,
		j.Id as JobId,
		j.JobTypeCode
	FROM Stop s 
	INNER JOIN
		Job j on j.StopId = s.Id
	INNER JOIN RouteHeader rh on s.RouteHeaderId = rh.Id
	WHERE 
		rh.DateDeleted IS NULL
		AND rh.RouteOwnerId in (Select BranchId FROM @UserBranches)

END