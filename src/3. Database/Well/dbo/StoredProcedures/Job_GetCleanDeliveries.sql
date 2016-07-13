CREATE PROCEDURE [dbo].[Job_GetCleanDeliveries]
AS
BEGIN
	SET NOCOUNT ON;
	SELECT	
		rh.RouteNumber, 
		s.DropId, 
		j.JobRef3 AS InvoiceNumber, 
		a.Code AS AccountCode, 
		a.Name AS AccountName ,
		jps.[Description] AS JobStatus
	FROM dbo.Job j
		JOIN dbo.JobPerformanceStatus jps on j.PerformanceStatusCode = jps.Code
		JOIN dbo.[Stop] s on j.StopId = s.Id
		JOIN dbo.Account a on s.Id = a.StopId
		JOIN dbo.RouteHeader rh on s.RouteHeaderId = rh.Id
	WHERE jps.Id = 6
END