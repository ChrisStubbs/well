CREATE PROCEDURE [dbo].[ReadRoute_GetAll]
	@UserName VARCHAR(500)
AS
BEGIN

	DECLARE @Bypass INT = 8
			,@Exception INT = 4

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
	INNER JOIN
		UserBranch ub on b.Id = ub.BranchId
	INNER JOIN
		[User] u on u.Id = ub.UserId
	LEFT JOIN RouteExceptionCount rec ON rec.RouteHeaderId = rh.Id
    WHERE u.IdentityName = @UserName
    AND rh.IsDeleted = 0
	ORDER BY rh.RouteDate DESC

	SELECT rh.Id, u.Name
	FROM RouteHeader rh
	INNER JOIN 
		Branch b ON rh.RouteOwnerId = b.Id
	INNER JOIN
		UserBranch ub ON b.Id = ub.BranchId
	INNER JOIN
		[User] u ON u.Id = ub.UserId
	INNER JOIN
		[Stop] s ON s.RouteHeaderId = rh.Id
	INNER JOIN
		Job j ON j.StopId = s.Id
	INNER JOIN 
		UserJob uj ON uj.JobId = j.Id

END