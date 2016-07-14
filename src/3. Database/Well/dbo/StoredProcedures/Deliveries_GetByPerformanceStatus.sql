CREATE PROCEDURE [dbo].[Deliveries_GetByPerformanceStatus]
	@PerformanceStatusId INT
AS
BEGIN

	SET NOCOUNT ON;

	SELECT 
		rh.RouteNumber, 
		s.DropId,
		j.JobRef3 as InvoiceNumber, 
		a.Code as AccountCode, 
		a.Name as AccountName ,
		ps.Description as JobStatus,
		s.DeliveryDate  as [DateTime],
		null as Action,
		null as Assigned
	FROM
		RouteHeader rh 
	INNER JOIN 
		[Stop] s on rh.Id = s.RouteHeaderId
	INNER JOIN 
		Job j on s.id = j.StopId	
	INNER JOIN 
		dbo.Account a on s.Id = a.StopId
	INNER JOIN
		dbo.PerformanceStatus ps on ps.Id = j.PerformanceStatusId
	WHERE
		ps.Id =  @PerformanceStatusId


END
