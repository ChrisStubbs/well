CREATE PROCEDURE [dbo].[Job_GetByStopId]
	@StopId int = 0
AS
	SELECT 
		[Id]
	FROM 
		[dbo].[Job]
	WHERE 
		[StopId] = @StopId
		AND JobTypeCode != 'UPL-SAN'
		AND job.DateDeleted IS NULL
RETURN 0
