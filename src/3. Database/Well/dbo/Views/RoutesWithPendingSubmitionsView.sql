CREATE VIEW RoutesWithPendingSubmitionsView
AS
    SELECT 
        rh.Id, 
        CONVERT(Bit, MAX(CASE 
                WHEN r.Description IN ('Pending Submission', 'Pending Approval') THEN 1
                ELSE 0
            END)) PendingSubmission,
		MAX(rh.RouteOwnerId) AS BranchId
   FROM    
        Job j WITH (NOLOCK)
        INNER JOIN [Stop] s WITH (NOLOCK) 
            ON j.StopId = s.Id
        INNER JOIN RouteHeader rh WITH (NOLOCK) 
            ON s.RouteHeaderId = rh.Id
        INNER JOIN ResolutionStatus r WITH (NOLOCK) 
            ON j.ResolutionStatusId = r.Id 
    WHERE
		j.DateDeleted IS NULL
        AND s.DateDeleted IS NULL
        AND rh.DateDeleted IS NULL
    GROUP BY 
        rh.Id