CREATE VIEW ExceptionTotalsPerSingleRoute
AS
        SELECT 
            COUNT(distinct li.id) AS TotalLInes,
            SUM(CASE 
			    WHEN lia.id IS NULL THEN 1
				ELSE 0
            END) AS NumberOfClean,
            r.id RouteId, 
			s.Id AS StopId,
            j.id AS JobId
        FROM 
            Job j
            INNER JOIN [Stop] s
                ON j.StopId = s.Id
				AND s.DateDeleted IS NULL
            inner JOIN RouteHeader r
                ON s.RouteHeaderId = r.Id
				AND r.DateDeleted IS NULL
            LEFT JOIN LineItem li
                ON j.Id = li.JobId
				AND li.DateDeleted is null
            LEFT join LineItemAction lia
                on li.Id = lia.LineItemId
				AND lia.DateDeleted is null
                AND lia.ExceptionTypeId != 4 --uplift
        WHERE
            j.DateDeleted IS NULL
            AND j.ResolutionStatusId > 1 --imported
        GROUP BY 
            j.id, 
            r.id, s.id

