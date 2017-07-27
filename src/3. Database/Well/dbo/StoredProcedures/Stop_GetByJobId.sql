CREATE PROCEDURE [dbo].[Stop_GetByJobId]
	@JobId INT

AS
BEGIN
	SELECT stp.[Id]
	FROM 
		[dbo].[Stop] stp
	INNER JOIN [dbo].[Job] jb ON stp.Id = jb.StopId
	WHERE jb.[Id] = @JobId
	AND stp.DateDeleted is NULL
		
END

