CREATE PROCEDURE [dbo].[RouteHeaders_Get]
	@UserName VARCHAR(500)
AS
	SELECT 
	  rh.[Id]
      ,rh.[CompanyId]
      ,rh.[RouteNumber]
      ,rh.[RouteDate]
      ,rh.[DriverName]
	  ,rh.RouteOwnerId
      ,rh.[PlannedStops]
      ,rh.[ActualStopsCompleted]
      ,rh.[RoutesId]
      ,rh.[RouteStatusId]
      ,rh.[RoutePerformanceStatusId]
      ,rh.[CreatedBy]
      ,rh.[DateCreated]
      ,rh.[UpdatedBy]
      ,rh.[DateUpdated]
      ,rh.[Version]
	  ,(SELECT COUNT(1) AS TotalDrops FROM Stop s WHERE s.RouteHeaderId = rh.Id) AS TotalDrops
  FROM 
	RouteHeader rh
	INNER JOIN Branch b 
		on rh.StartDepotCode = b.Id
	INNER JOIN
		UserBranch ub on b.Id = ub.BranchId
	INNER JOIN
		[User] u on u.Id = ub.UserId
 WHERE
	u.IdentityName = @UserName