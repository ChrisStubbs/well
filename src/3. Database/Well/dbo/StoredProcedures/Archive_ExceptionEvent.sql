CREATE PROCEDURE [dbo].[Archive_ExceptionEvent]
	@ArchiveDate DateTime
AS
	-- ExceptionEvent
	DELETE ex
	OUTPUT Deleted.[Id]
		,@@SERVERNAME + '.' + DB_NAME()
		,Deleted.[Event]
		,Deleted.[ExceptionActionId]
		,Deleted.[Processed]
		,Deleted.[DateCanBeProcessed]
		,Deleted.[SourceId]
		,Deleted.[CreatedBy]
		,Deleted.[DateCreated]
		,Deleted.[UpdatedBy]
		,Deleted.[DateUpdated]
		,@ArchiveDate
	INTO [$(WellArchive)].[dbo].ExceptionEvent
		([Id]
		,DataSource
		,[Event]
		,[ExceptionActionId]
		,[Processed]
		,[DateCanBeProcessed]
		,[SourceId]
		,[CreatedBy]
		,[DateCreated]
		,[UpdatedBy]
		,[DateUpdated]
		,[ArchiveDate])
	FROM dbo.ExceptionEvent ex
	WHERE ex.Processed = 1
	PRINT ('Deleted ExceptionEvents')
	PRINT ('----------------')
