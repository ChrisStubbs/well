CREATE PROCEDURE CleanStops
    @JobIds IntTableType READONLY,
    @DateDeleted DateTime,
	@UpdatedBy VARCHAR(50)
AS
    DECLARE @MinDeletedDate DateTime = '19000101'

    ;WITH Data(Id, MinDateDeleted) AS 
    (
        SELECT 
            s.id,
            MIN (ISNULL(j.DateDeleted, @MinDeletedDate)) AS MinDateDeleted
        FROM 
            STOP s
            INNER JOIN Job j
                ON j.StopId = s.id
        WHERE
            s.Id IN 
            (
                SELECT StopId 
                FROM Job j
            )
        GROUP BY 
            s.id
    )
    UPDATE Stop
    SET DateDeleted = @DateDeleted
		,UpdatedBy	= @UpdatedBy
    WHERE 
        ID IN (SELECT Id FROM Data WHERE MinDateDeleted != @MinDeletedDate)