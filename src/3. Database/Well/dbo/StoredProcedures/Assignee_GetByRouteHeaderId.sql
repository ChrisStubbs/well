CREATE PROCEDURE [dbo].[Assignee_GetByRouteHeaderId]
	@RouteHeaderId int
AS
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
		rh.IsDeleted = 0
		AND
		rh.Id = @RouteHeaderId
RETURN 0
