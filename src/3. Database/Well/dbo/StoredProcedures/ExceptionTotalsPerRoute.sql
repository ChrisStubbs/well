CREATE PROCEDURE [dbo].[ExceptionTotalsPerRoute]
	@BranchId int
AS

	;WITH data 
    AS
    (
        SELECT DISTINCT
            MAX(CASE 
					WHEN lia.LineItemActionId IS NULL THEN 0
					ELSE 1
            END) AS HasNull,
            MAX(Data.routenumber) Routenumber,
            Data.RouteId, 
			Data.StopId,
			MAX(RouteOwnerId) AS BranchId
        FROM
            (
                SELECT j.id AS JobId, r.RouteNumber, r.id AS RouteId, r.RouteOwnerId, s.id AS StopId
                FROM 
                    Job j WITH (NOLOCK)
                    INNER JOIN [Stop] s WITH (NOLOCK)
                        ON j.StopId = s.Id
				        AND s.DateDeleted IS NULL
                    inner JOIN RouteHeader r WITH (NOLOCK)
                        ON s.RouteHeaderId = r.Id
				        AND r.DateDeleted IS NULL
                WHERE
                    j.DateDeleted IS NULL
                    AND j.ResolutionStatusId > 1 --imported
                    AND r.RouteOwnerId = @BranchId
            ) Data
            INNER JOIN 
            (
                SELECT lia.id AS LineItemActionId, li.JobId
                FROM 
                    LineItem li WITH (NOLOCK)
                    INNER join LineItemAction lia WITH (NOLOCK)
                        on li.Id = lia.LineItemId
				        AND lia.DateDeleted is null
				        AND lia.ExceptionTypeId != 4 --uplift
            WHERE
                li.DateDeleted is null
            ) Lia
                ON Lia.JobId = Data.JobId
        GROUP BY 
             Data.RouteId, Data.StopId
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