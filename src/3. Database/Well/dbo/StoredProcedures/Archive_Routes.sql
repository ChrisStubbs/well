CREATE PROCEDURE [dbo].[Archive_Routes]
	@ArchiveDate DateTime
AS
	-- Route
	DECLARE @CleanDate AS DateTime = @ArchiveDate - 3;

	DELETE r
	OUTPUT Deleted.[Id]
		,@@SERVERNAME + '.' + DB_NAME()
		,Deleted.[FileName]
		,Deleted.[DateDeleted]
		,Deleted.[CreatedBy]
		,Deleted.[DateCreated]
		,Deleted.[UpdatedBy]
		,Deleted.[DateUpdated]
		,@ArchiveDate
	INTO [$(WellArchive)].[dbo].[Routes]
		([Id]
		,DataSource
		,[FileName]
		,[DateDeleted]
		,[CreatedBy]
		,[DateCreated]
		,[UpdatedBy]
		,[DateUpdated]
		,[ArchiveDate])
	FROM dbo.[Routes] r
	LEFT JOIN RouteHeader rh on rh.RoutesId = r.Id
	WHERE r.DateCreated  <= @CleanDate
	AND rh.Id IS NULL

	PRINT ('Deleted Routes')
	PRINT ('----------------')


	