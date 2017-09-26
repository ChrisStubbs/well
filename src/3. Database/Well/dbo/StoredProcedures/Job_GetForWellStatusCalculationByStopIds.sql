CREATE PROCEDURE Job_GetForWellStatusCalculationByStopIds
	@StopIds dbo.IntTableType	READONLY
AS
	SELECT 
		Id,
        StopId,      
        ResolutionStatus, 
        JobStatus, 
        WellStatus 
	FROM 
		JobForWellStatusCalculation j
        INNER JOIN @StopIds i
            ON j.StopId = i.Value