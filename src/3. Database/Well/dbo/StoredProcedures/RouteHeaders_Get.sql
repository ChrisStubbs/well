CREATE PROCEDURE [dbo].[RouteHeaders_Get]
	--@UserName VARCHAR(500)

AS
	SELECT 
	  rh.[Id]
      ,rh.[CompanyId]
      ,rh.[RouteNumber]
      ,rh.[RouteDate]
      ,rh.[DriverName]
      ,rh.[VehicleReg]
      ,rh.[StartDepotCode]
      ,rh.[PlannedRouteStartTime]
      ,rh.[PlannedRouteFinishTime]
      ,rh.[PlannedDistance]
      ,rh.[PlannedTravelTime]
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
 -- INNER JOIN
	--dbo.Branch b on rh.StartDepotCode = UPPER(b.TranscendMapping)
 -- INNER JOIN
	--dbo.UserBranch ub on b.Id = ub.BranchId
 -- INNER JOIN
	--dbo.[User] u on u.Id = ub.UserId
 -- WHERE
	--u.Name = @UserName

RETURN 0
