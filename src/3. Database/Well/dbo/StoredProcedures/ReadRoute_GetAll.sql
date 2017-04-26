CREATE PROCEDURE [dbo].[ReadRoute_GetAll]
	@UserName VARCHAR(500)
AS
BEGIN
	SELECT RouteOwnerId AS Branch
		   ,RouteNumber AS [Route]
		   ,RouteDate
		   ,PlannedStops AS StopCount
		   ,RouteStatusDescription AS RouteStatus
		   ,DriverName
	FROM
		   RouteHeader rh
	INNER JOIN 
		Branch b on rh.RouteOwnerId = b.Id
	INNER JOIN
		UserBranch ub on b.Id = ub.BranchId
	INNER JOIN
		[User] u on u.Id = ub.UserId
    WHERE u.IdentityName = @UserName
    AND rh.IsDeleted = 0
    ORDER BY rh.RouteDate DESC
END