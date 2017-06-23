CREATE PROCEDURE [dbo].[JobRoutes_GetByJobIds]
	@JobIds dbo.IntTableType	READONLY

AS
	SELECT 
		r.Id as RouteId 
		,r.RouteOwnerId as BranchId
		,r.RouteDate
		,j.Id as JobId
	FROM
		RouteHeader r
	INNER JOIN 
		Stop s on s.RouteHeaderId = r.Id
	INNER JOIN 
		Job j on j.StopId = s.Id
	INNER JOIN
		Branch b on b.Id = r.RouteOwnerId
	INNER JOIN 
		@JobIds ids ON ids.Value = j.Id
