CREATE VIEW ExceptionTotalsPerRoute
AS 
    WITH data 
    AS
    (
        SELECT DISTINCT
            MAX(CASE 
					WHEN lia.id IS NULL THEN 0
					ELSE 1
            END) AS HasNull,
            MAX(r.routenumber) Routenumber,
            r.id routeid, 
			MAX(RouteOwnerId) AS BranchId
        FROM 
            Job j
            INNER JOIN [Stop] s
                ON j.StopId = s.Id
				AND s.DateDeleted IS NULL
            inner JOIN RouteHeader r
                ON s.RouteHeaderId = r.Id
				AND r.DateDeleted IS NULL
            INNER JOIN JobDetail jd
                ON j.id = jd.JobId
				AND jd.DateDeleted IS NULL
            LEFT JOIN LineItem li
                ON jd.LineItemId = li.id
				AND li.DateDeleted is null
            LEFT join LineItemAction lia
                on li.Id = lia.LineItemId
				AND lia.DateDeleted is null
        WHERE
            j.DateDeleted IS NULL
            AND j.ResolutionStatusId > 1 --imported
        GROUP BY 
             r.id, s.id
    )
    SELECT 
        x.Routeid,
		MAX(BranchId) AS BranchId,
        SUM(CASE WHEN x.HasNull = 1 THEN 1 ELSE 0 END) WithExceptions,
        SUM(CASE WHEN x.HasNull = 0 THEN 1 ELSE 0 END) WithOutExceptions
    FROM 
        data x
    GROUP BY 
        x.routeid