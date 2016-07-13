﻿CREATE PROCEDURE [dbo].[Job_GetCleanDeliveries]
AS
BEGIN

	SET NOCOUNT ON;

	SELECT rh.RouteNumber, s.DropId, j.JobRef3, a.Code, a.Name ,jps.[Description] 
	FROM Well.dbo.Job j
	JOIN Well.dbo.JobPerformanceStatus jps on j.PerformanceStatusCode = jps.Code
	JOIN Well.dbo.[Stop] s on j.StopId = s.Id
	JOIN Well.dbo.Account a on s.Id = a.StopId
	JOIN Well.dbo.RouteHeader rh on s.RouteHeaderId = rh.Id
	WHERE jps.Id = 6


END