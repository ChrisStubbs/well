﻿CREATE PROCEDURE Assignee_GetByJobIds
	@JobIds dbo.IntTableType	READONLY
AS
	SELECT 
		rh.Id RouteId
		,j.Id as JobId
		,s.Id StopId
		,jobUser.Name
		,jobUser.IdentityName
	FROM 
		RouteHeader rh
		INNER JOIN Branch b 
			ON rh.RouteOwnerId = b.Id
		INNER JOIN [Stop] s 
			ON s.RouteHeaderId = rh.Id
		INNER JOIN Job j 
			ON j.StopId = s.Id
		INNER JOIN UserJob uj 
			ON uj.JobId = j.Id
		INNER JOIN [User] jobUser 
			ON uj.UserId = jobUser.Id
		INNER JOIN @JobIds ids 
			ON ids.Value = j.Id
	WHERE 
		rh.DateDeleted IS NULL

