CREATE PROCEDURE [dbo].[RouteHeader_GetExceptions]

AS
BEGIN

SET NOCOUNT ON;

	--SELECT COUNT(rh.id) 
	--FROM  RouteHeader rh
	--INNER JOIN [Stop] stp on rh.Id = stp.RouteHeaderId
	--INNER JOIN Job jb ON jb.StopId = stp.Id
	--INNER JOIN JobDetail Jd ON jd.JobId = jb.Id
	--INNER JOIN JobDetailDamageReasons jdm ON jdm.JobDetailId = jd.Id

	select [NoOfExceptions],[Assigned], [Outstanding], [OnHold], [Notifications] from SampleExceptions

END