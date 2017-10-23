DECLARE @ArchiveDate DateTime = GETDATE();
	DELETE ex
	OUTPUT Deleted.[Id]
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
	INTO archive.ExceptionEvent
		([Id]
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
	LEFT Join
		Job j on CAST(j.Id AS VARCHAR(50)) = ex.SourceId
	WHERE
		ex.ExceptionActionId = 4 --GRNS
		AND j.Id Is Null