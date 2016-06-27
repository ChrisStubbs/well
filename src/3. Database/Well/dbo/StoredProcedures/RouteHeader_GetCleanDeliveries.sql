CREATE PROCEDURE [dbo].[RouteHeader_GetCleanDeliveries]

AS
BEGIN

SET NOCOUNT ON;

	SELECT COUNT(rh.id) 
	FROM  RouteHeader rh
	INNER JOIN [Stop] stp on rh.Id = stp.RouteHeaderId
	INNER JOIN Job jb ON jb.StopId = stp.Id
	INNER JOIN JobDetail Jd ON jd.JobId = jb.Id
	WHERE jd.Id NOT IN (SELECT JobDetailId FROM JobDetailDamageReasons)


END