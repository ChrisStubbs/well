CREATE PROCEDURE [dbo].[RouteHeaders_Get]
	@UserName VARCHAR(500)

AS
begin
	SELECT 
	  rh.[Id]
      ,rh.[CompanyId]
      ,rh.[RouteNumber]
      ,rh.[RouteDate]
      ,rh.[DriverName]
      --,rh.[StartDepotCode]
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

  FROM [dbo].[RouteHeader] rh
  INNER JOIN
	dbo.Branch b on rh.StartDepotCode = b.Id
INNER JOIN
	dbo.UserBranch ub on b.Id = ub.BranchId
 INNER JOIN
	dbo.[User] u on u.Id = ub.UserId
 WHERE
	u.IdentityName = @UserName
end
