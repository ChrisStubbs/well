CREATE PROCEDURE  [dbo].[GetBranchIdForJob]
	@jobId INT
AS
BEGIN

	SET NOCOUNT ON;

	select
		rh.RouteOwnerId
	from
		RouteHeader rh
	join
		[Stop] s on s.RouteHeaderId = rh.Id
	join
		Job j on j.StopId = s.Id
	where
		j.Id = @jobId

END