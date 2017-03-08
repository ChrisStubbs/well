CREATE PROCEDURE  [dbo].[GetBranchIdForStop]
	@stopId INT
AS
BEGIN

	SET NOCOUNT ON;

	select
		rh.RouteOwnerId
	from
		RouteHeader rh
	join
		[Stop] s on s.RouteHeaderId = rh.Id
	where
		s.Id = @stopId

END
