CREATE PROCEDURE [dbo].[Archive_Activity]
	@ArchiveDate DateTime
AS
	-- Activity
	DELETE a
	OUTPUT Deleted.[Id]
		,Deleted.[DocumentNumber]
		,Deleted.[InitialDocument]
		,Deleted.[ActivityTypeId]
		,Deleted.[LocationId]
		,Deleted.[CreatedBy]
		,Deleted.[CreatedDate]
		,Deleted.[LastUpdatedBy]
		,Deleted.[LastUpdatedDate]
		,Deleted.[DateDeleted]
		,@ArchiveDate
	INTO archive.Activity
		([Id]
		,[DocumentNumber]
		,[InitialDocument]
		,[ActivityTypeId]
		,[LocationId]
		,[CreatedBy]
		,[CreatedDate]
		,[LastUpdatedBy]
		,[LastUpdatedDate]
		,[DateDeleted]
		,[ArchiveDate])
	FROM dbo.Activity a
	LEFT JOIN Job j on j.ActivityId = a.Id
	WHERE
		j.Id IS NULL

	PRINT ('Deleted Activity')
	PRINT ('----------------')


	