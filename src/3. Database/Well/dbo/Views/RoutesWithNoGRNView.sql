
CREATE VIEW RoutesWithNoGRNView
AS
    SELECT 
        rh.Id, 
        CONVERT(Bit, MAX(CASE 
                WHEN j.GrnProcessType = 1 AND j.GrnNumber IS NULL THEN 1 
                ELSE 0 
            END)) AS NoGRNButNeeds,
		MAX(rh.RouteOwnerId) AS BranchId
    FROM 
        Job j
		INNER JOIN [Stop] s ON j.StopId = s.id
        INNER JOIN RouteHeader rh on s.RouteHeaderId = rh.Id
	WHERE 
        j.DateDeleted IS NULL
    GROUP BY 
        rh.Id
