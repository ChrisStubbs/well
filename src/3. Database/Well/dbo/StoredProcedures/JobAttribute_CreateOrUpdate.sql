CREATE PROCEDURE [dbo].[JobAttribute_CreateOrUpdate]
	@Id						INT = NULL,
	@Code					NVARCHAR(50),
	@Value					NVARCHAR(50),
	@JobId		INT,
	@Username				NVARCHAR(50)

AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @ChangeResult TABLE (ChangeType VARCHAR(10), Id INTEGER)

	MERGE INTO [JobAttribute] AS Target
	USING (VALUES
		(@Id, @Code, @Value,  @JobId,  @Username, GETDATE(), @Username, GETDATE())
	)
	AS Source ([Id],[Code],[Value],[JobId],[CreatedBy],[DateCreated],[UpdatedBy],[DateUpdated])
	ON Target.[Id] = Source.[Id]
	WHEN MATCHED THEN
	UPDATE SET
		[Code] = Source.[Code],
		[Value] = Source.[Value],
		[JobId] = Source.[JobId],
		[CreatedBy] = Source.[CreatedBy],
		[DateCreated] = Source.[DateCreated],
		[UpdatedBy] = Source.[UpdatedBy],
		[DateUpdated] = Source.[DateUpdated]
	WHEN NOT MATCHED BY TARGET AND @Id = 0 THEN
	INSERT ([Code],[Value],[JobId],[CreatedBy],[DateCreated],[UpdatedBy],[DateUpdated])
	VALUES ([Code],[Value],[JobId],[CreatedBy],[DateCreated],[UpdatedBy],[DateUpdated])

	OUTPUT $action, inserted.Id INTO @ChangeResult;

	SELECT Id FROM @ChangeResult

END