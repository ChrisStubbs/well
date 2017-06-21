
CREATE PROCEDURE [dbo].[Assignee_GetByStopId]
	@StopId int
AS
	SELECT DISTINCT 
			rh.Id RouteId
			,j.Id as JobId
			,s.Id StopId
			,jobUser.Name
			,jobUser.IdentityName
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
		AND
		s.Id = @StopId

RETURN 0
