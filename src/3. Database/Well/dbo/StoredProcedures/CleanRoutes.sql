CREATE PROCEDURE CleanRoutes
    @JobIds IntTableType READONLY,
    @DateDeleted DateTime,
	@UpdatedBy VARCHAR(50)
AS

    DECLARE @MinDeletedDate DateTime = '19000101'

    ;WITH Data(Id, MinDateDeleted) AS 
    (
        SELECT 
            rh.Id,
            MIN (ISNULL(s.DateDeleted, @MinDeletedDate)) AS MinDateDeleted
        FROM 
            RouteHeader rh
            INNER JOIN [STOP] s
                ON s.RouteHeaderId = rh.Id
            INNER JOIN Job j
                ON j.StopId = s.id
        WHERE
            s.Id IN 
            (
                SELECT StopId 
                FROM Job j
            )
        GROUP BY 
            rh.Id
    )
    UPDATE RouteHeader
    SET DateDeleted = @DateDeleted
		,UpdatedBy	= @UpdatedBy
    WHERE 
        ID IN (SELECT Id FROM Data WHERE MinDateDeleted != @MinDeletedDate)