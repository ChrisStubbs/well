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
        Job j
        INNER JOIN [Stop] s 
            ON j.StopId = s.Id
        INNER JOIN RouteHeader rh 
            ON s.RouteHeaderId = rh.Id
        INNER JOIN ResolutionStatus r 
            ON j.ResolutionStatusId = r.Id 
    WHERE
		j.DateDeleted IS NULL
        AND s.DateDeleted IS NULL
        AND rh.DateDeleted IS NULL
    GROUP BY 
        rh.Id