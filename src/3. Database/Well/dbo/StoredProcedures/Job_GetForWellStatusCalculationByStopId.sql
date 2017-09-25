CREATE PROCEDURE [dbo].[Job_GetForWellStatusCalculationByStopId]
	@StopId int
AS
	SELECT 
		* 
	FROM 
		JobForWellStatusCalculation
	WHERE 
		StopId = @StopId
