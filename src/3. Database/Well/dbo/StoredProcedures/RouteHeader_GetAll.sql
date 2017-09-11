CREATE PROCEDURE [dbo].[RouteHeader_GetAll]
	@UserName VARCHAR(500)
AS
BEGIN
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
      ,rh.[RouteStatusCode]
	  ,rh.[RouteStatusDescription]
      ,rh.[PerformanceStatusCode]
	  ,rh.[PerformanceStatusDescription]
      ,rh.[CreatedBy]
      ,rh.[DateCreated]
      ,rh.[UpdatedBy]
      ,rh.[DateUpdated]
      ,rh.[Version]
	  ,[WellStatus] as RouteWellStatus
	  ,(SELECT COUNT(1) AS TotalDrops FROM Stop s WHERE s.RouteHeaderId = rh.Id) AS TotalDrops
  FROM 
	RouteHeader rh
	INNER JOIN Branch b 
		on rh.StartDepotCode = b.Id
	INNER JOIN
		UserBranch ub on b.Id = ub.BranchId
	INNER JOIN
		[User] u on u.Id = ub.UserId
   WHERE u.IdentityName = @UserName
   AND rh.DateDeleted IS NULL
   Order By rh.RouteDate DESC

SELECT 
	   [s].[Id]
      ,[PlannedStopNumber]
      ,[RouteHeaderId]
      ,[DropId]
      ,[LocationId]
      ,[DeliveryDate]
	  ,[ShellActionIndicator] 
	  ,[AllowOvers] 
	  ,[CustUnatt] 
	  ,[PHUnatt] 
	  ,[s].[DateCreated]
	  ,[s].[DateDeleted]
FROM 
	  [dbo].[Stop] s
	INNER JOIN RouteHeader rh
		on rh.Id = s.RouteHeaderId
	INNER JOIN Branch b 
		on rh.StartDepotCode = b.Id
	INNER JOIN
		UserBranch ub on b.Id = ub.BranchId
	INNER JOIN
		[User] u on u.Id = ub.UserId
WHERE u.IdentityName = @UserName
AND s.DateDeleted IS NULL

SELECT [j].[Id]
      ,[Sequence]
      ,[JobTypeCode]
      ,[PHAccount]
      ,[PickListRef]
      ,[InvoiceNumber]
      ,[CustomerRef]
      ,[OrderDate]
	  ,[RoyaltyCode]
	  ,[RoyaltyCodeDesc] 
	  ,[OrdOuters] 
	  ,[InvOuters] 
	  ,[ColOuters] 
	  ,[ColBoxes] 
	  ,[ReCallPrd] 
	  ,[AllowSOCrd] 
	  ,[COD] 
	  ,[GrnNumber] 
	  ,[GrnRefusedReason] 
	  ,[GrnRefusedDesc] 
	  ,[AllowReOrd] 
	  ,[SandwchOrd] 
	  ,[PerformanceStatusId] as PerformanceStatus
	  ,[j].[Reason]
	  ,[j].[DateDeleted]
      ,[StopId]
      ,[j].[CreatedBy]
      ,[j].[DateCreated]
      ,[j].[UpdatedBy]
      ,[j].[DateUpdated]
      ,[j].[Version]
	  ,[j].[JobStatusId] as JobStatus
  FROM [dbo].[Job] j
	INNER JOIN [dbo].[Stop] s
		on j.StopId = s.Id
	INNER JOIN RouteHeader rh
		on rh.Id = s.RouteHeaderId
	INNER JOIN Branch b 
		on rh.StartDepotCode = b.Id
	INNER JOIN
		UserBranch ub on b.Id = ub.BranchId
	INNER JOIN
		[User] u on u.Id = ub.UserId
	WHERE u.IdentityName = @UserName
	AND j.DateDeleted IS NULL
END
