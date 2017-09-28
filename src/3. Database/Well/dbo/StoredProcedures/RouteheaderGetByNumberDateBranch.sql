CREATE PROCEDURE [dbo].[RouteheaderGetByNumberDateBranch]
	@filter GetByNumberDateBranchFilter READONLY
AS
	SELECT 
		Id,
		WellStatus,
		r.RouteNumber,
		r.RouteDate,
		r.RouteOwnerId AS BranchId,
		[WellStatus] RouteWellStatus
	FROM 
		RouteHeader r
		INNER JOIN @filter f
			ON r.RouteNumber = f.RouteNumber
			AND r.RouteDate = f.RouteDate
			AND r.RouteOwnerId = f.BranchId