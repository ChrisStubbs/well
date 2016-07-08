CREATE PROCEDURE [dbo].[Routes_CreateOrUpdate]
	@Id						INT = NULL,
	@FileName				VARCHAR(50),
	@Username				VARCHAR(50)

AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @ChangeResult TABLE (ChangeType VARCHAR(10), Id INTEGER)

	MERGE INTO [Routes] AS Target
	USING (VALUES
		(@Id, @FileName, GETDATE(), @Username, GETDATE(), @Username, GETDATE())
	)
	AS Source ([Id],[FileName],[ImportDate],[CreatedBy],[DateCreated],[UpdatedBy],[DateUpdated])
	ON Target.[Id] = Source.[Id]
	WHEN MATCHED THEN
	UPDATE SET
		[FileName] = Source.[FileName],
		[ImportDate] = Source.[ImportDate],
		[CreatedBy] = Source.[CreatedBy],
		[DateCreated] = Source.[DateCreated],
		[UpdatedBy] = Source.[UpdatedBy],
		[DateUpdated] = Source.[DateUpdated]
	WHEN NOT MATCHED BY TARGET AND @Id = 0 THEN
	INSERT ([FileName],[ImportDate],[CreatedBy],[DateCreated],[UpdatedBy],[DateUpdated])
	VALUES ([FileName],[ImportDate],[CreatedBy],[DateCreated],[UpdatedBy],[DateUpdated])

	OUTPUT $action, inserted.Id INTO @ChangeResult;

	SELECT Id FROM @ChangeResult

END
