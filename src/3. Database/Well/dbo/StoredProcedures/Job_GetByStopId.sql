CREATE PROCEDURE [dbo].[Job_GetByStopId]
	@StopId int = 0
AS
	SELECT [Id]
	FROM [dbo].[Job]
	WHERE [StopId] = @StopId
RETURN 0
