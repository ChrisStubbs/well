CREATE PROCEDURE [dbo].[Archive_RouteHeader]
	@ArchiveDate DateTime
AS
	-- Routes
	DELETE rh
	OUTPUT Deleted.[Id]
		,@@SERVERNAME + '.' + DB_NAME()
		,Deleted.[CompanyId]
		,Deleted.[RouteNumber]
		,Deleted.[RouteDate]
		,Deleted.[DriverName]
		,Deleted.[StartDepotCode]
		,Deleted.[PlannedStops]
		,Deleted.[ActualStopsCompleted]
		,Deleted.[RoutesId]
		,Deleted.[RouteStatusCode]
		,Deleted.[RouteStatusDescription]
		,Deleted.[PerformanceStatusCode]
		,Deleted.[PerformanceStatusDescription]
		,Deleted.[LastRouteUpdate]
		,Deleted.[AuthByPass]
		,Deleted.[NonAuthByPass]
		,Deleted.[ShortDeliveries]
		,Deleted.[DamagesRejected]
		,Deleted.[DamagesAccepted]
		,Deleted.[RouteOwnerId]
		,Deleted.[WellStatus]
		,Deleted.[DateDeleted]
		,Deleted.[CreatedBy]
		,Deleted.[DateCreated]
		,Deleted.[UpdatedBy]
		,Deleted.[DateUpdated]
		,@ArchiveDate
	INTO [$(WellArchive)].[dbo].RouteHeader
		(  [Id]
	      ,DataSource
		  ,[CompanyId]
		  ,[RouteNumber]
		  ,[RouteDate]
		  ,[DriverName]
		  ,[StartDepotCode]
		  ,[PlannedStops]
		  ,[ActualStopsCompleted]
		  ,[RoutesId]
		  ,[RouteStatusCode]
		  ,[RouteStatusDescription]
		  ,[PerformanceStatusCode]
		  ,[PerformanceStatusDescription]
		  ,[LastRouteUpdate]
		  ,[AuthByPass]
		  ,[NonAuthByPass]
		  ,[ShortDeliveries]
		  ,[DamagesRejected]
		  ,[DamagesAccepted]
		  ,[RouteOwnerId]
		  ,[WellStatus]
		  ,[DateDeleted]
		  ,[CreatedBy]
		  ,[DateCreated]
		  ,[UpdatedBy]
		  ,[DateUpdated]
		  ,[ArchiveDate]
		)
	FROM RouteHeader rh
	LEFT JOIN dbo.Stop s ON s.RouteHeaderId = rh.Id
	WHERE s.Id Is Null
	PRINT ('Deleted RouteHeader')
	PRINT ('----------------')


	