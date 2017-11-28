CREATE PROCEDURE [dbo].[Archive_Stops]
	@ArchiveDate DateTime
AS
	-- Account
	DELETE acs
	OUTPUT Deleted.[Id]
		,@@SERVERNAME + '.' + DB_NAME()
		,Deleted.[Code]
		,Deleted.[AccountTypeCode]
		,Deleted.[DepotId]
		,Deleted.[Name]
		,Deleted.[Address1]
		,Deleted.[Address2]
		,Deleted.[PostCode]
		,Deleted.[ContactName]
		,Deleted.[ContactNumber]
		,Deleted.[ContactNumber2]
		,Deleted.[ContactEmailAddress]
		,Deleted.[DateDeleted]
		,Deleted.[StopId]
		,Deleted.[CreatedBy]
		,Deleted.[DateCreated]
		,Deleted.[UpdatedBy]
		,Deleted.[DateUpdated]
		,Deleted.[LocationId]
		,@ArchiveDate
	INTO [$(WellArchive)].[dbo].Account
		([Id]
		,DataSource
		,[Code]
		,[AccountTypeCode]
		,[DepotId]
		,[Name]
		,[Address1]
		,[Address2]
		,[PostCode]
		,[ContactName]
		,[ContactNumber]
		,[ContactNumber2]
		,[ContactEmailAddress]
		,[DateDeleted]
		,[StopId]
		,[CreatedBy]
		,[DateCreated]
		,[UpdatedBy]
		,[DateUpdated]
		,[LocationId]
		,[ArchiveDate])
	FROM dbo.Account acs
	LEFT JOIN Stop s on acs.StopId = s.Id
	LEFT JOIN Job j on j.StopId = s.Id
	WHERE j.Id Is Null
	PRINT ('Deleted Accounts')
	PRINT ('----------------')

	-- Stop
	DELETE s
	OUTPUT Deleted.[Id]
		,@@SERVERNAME + '.' + DB_NAME()
		,Deleted.[TransportOrderReference]
		,Deleted.[PlannedStopNumber]
		,Deleted.[RouteHeaderCode]
		,Deleted.[RouteHeaderId]
		,Deleted.[DropId]
		,Deleted.[Previously]
		,Deleted.[LocationId]
		,Deleted.[DeliveryDate]
		,Deleted.[ShellActionIndicator]
		,Deleted.[AllowOvers]
		,Deleted.[CustUnatt]
		,Deleted.[PHUnatt]
		,Deleted.[StopStatusCode]
		,Deleted.[StopStatusDescription]
		,Deleted.[PerformanceStatusCode]
		,Deleted.[PerformanceStatusDescription]
		,Deleted.[Reason]
		,Deleted.[DateDeleted]
		,Deleted.[DeletedByImport]
		,Deleted.[ActualPaymentCash]
		,Deleted.[ActualPaymentCheque]
		,Deleted.[ActualPaymentCard]
		,Deleted.[AccountBalance]
		,Deleted.[CreatedBy]
		,Deleted.[DateCreated]
		,Deleted.[UpdatedBy]
		,Deleted.[DateUpdated]
		,Deleted.[Location_Id]
		,Deleted.[WellStatus]
		,@ArchiveDate
	INTO [$(WellArchive)].[dbo].[Stop]
		([Id]
		,DataSource
		,[TransportOrderReference]
		,[PlannedStopNumber]
		,[RouteHeaderCode]
		,[RouteHeaderId]
		,[DropId]
		,[Previously]
		,[LocationId]
		,[DeliveryDate]
		,[ShellActionIndicator]
		,[AllowOvers]
		,[CustUnatt]
		,[PHUnatt]
		,[StopStatusCode]
		,[StopStatusDescription]
		,[PerformanceStatusCode]
		,[PerformanceStatusDescription]
		,[Reason]
		,[DateDeleted]
		,[DeletedByImport]
		,[ActualPaymentCash]
		,[ActualPaymentCheque]
		,[ActualPaymentCard]
		,[AccountBalance]
		,[CreatedBy]
		,[DateCreated]
		,[UpdatedBy]
		,[DateUpdated]
		,[Location_Id]
		,[WellStatus]
		,[ArchiveDate])
	FROM Stop s
	LEFT JOIN Job j on j.StopId = s.Id
	WHERE j.Id Is Null
	PRINT ('Deleted Stops')
	PRINT ('----------------')

	