CREATE PROCEDURE [dbo].[ReadRoute_GetAll]
	@UserName VARCHAR(500)
AS
BEGIN

	DECLARE @Bypass INT = 8
			,@Exception INT = 4;

	DECLARE @UserBranches Table(BranchId INT NOT NULL);

	INSERT INTO @UserBranches
	SELECT	
			BranchId
		FROM	
			UserBranch ub
		INNER JOIN 
			[User] u on u.Id = ub.UserId
		WHERE 
			 u.IdentityName = @UserName

	;WITH RouteExceptionCount AS
	(
		SELECT RouteHeaderId
			,COUNT(1) AS ExceptionCount FROM [Stop]
		WHERE 
			Id IN 
			(SELECT Stopid 
				FROM Job
				WHERE JobStatusId IN (@Exception, @Bypass))
				GROUP BY RouteHeaderId
	)
	SELECT rh.Id
		   ,RouteOwnerId AS BranchId
		   ,b.Name AS BranchName	
		   ,RouteNumber AS [Route]
		   ,RouteDate
		   ,PlannedStops AS StopCount
		   ,RouteStatusDescription AS RouteStatus
		   ,ISNULL(rec.ExceptionCount,0) AS ExceptionCount  -- count of stops with exceptions or bypassed
		   ,DriverName
	FROM
		RouteHeader rh
	INNER JOIN 
		Branch b on rh.RouteOwnerId = b.Id
	LEFT JOIN RouteExceptionCount rec ON rec.RouteHeaderId = rh.Id
    WHERE 
		rh.IsDeleted = 0
		AND rh.RouteOwnerId in (Select BranchId FROM @UserBranches)
	ORDER BY rh.RouteDate DESC

	SELECT DISTINCT 
			rh.Id RouteId
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
		rh.IsDeleted = 0
		AND rh.RouteOwnerId in (Select BranchId FROM @UserBranches)

	SELECT 
		s.RouteHeaderId as RouteId,
		j.Id as JobId   		
	FROM Stop s 
	INNER JOIN
		Job j on j.StopId = s.Id
	INNER JOIN RouteHeader rh on s.RouteHeaderId = rh.Id
	WHERE 
		rh.IsDeleted = 0
		AND rh.RouteOwnerId in (Select BranchId FROM @UserBranches)
END