CREATE PROCEDURE [dbo].[StopAccount_CreateOrUpdate]
	@Id						INT = 0,
	@Code					VARCHAR(50),
	@Username				VARCHAR(50),
	@AccountTypeCode		NVARCHAR(50),
	@DepotId				INT  NULL,
	@Name					NVARCHAR(50),
	@Address1				NVARCHAR(50),
	@Address2				NVARCHAR(50),
	@PostCode				NVARCHAR(10),
	@ContactName			NVARCHAR(50),
	@ContactNumber			NVARCHAR(15),
	@ContactNumber2			NVARCHAR(15),
	@ContactEmailAddress	NVARCHAR(50),
	@StopId					INT

AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @ChangeResult TABLE (ChangeType VARCHAR(10), Id INTEGER)

	MERGE INTO [Account] AS Target
	USING (VALUES
		(@Id, @Code, @AccountTypeCode,@DepotId,  @Name, @Address1, @Address2, @PostCode, @ContactName, @ContactNumber, @ContactNumber2, @ContactEmailAddress,
		 @StopId, @Username, GETDATE(), @Username, GETDATE())
	)
	AS Source ([Id],[Code],[AccountTypeCode],[DepotId],[Name], [Address1], [Address2], [PostCode], [ContactName], [ContactNumber],
				[ContactNumber2], [ContactEmailAddress], [StopId], [CreatedBy],[DateCreated],[UpdatedBy],[DateUpdated])
	ON Target.[Id] = Source.[Id]
	WHEN MATCHED THEN
	UPDATE SET
		[Code] = Source.[Code],
		[AccountTypeCode] = Source.[AccountTypeCode],
		[DepotId] = Source.[DepotId],
		[Name] = Source.[Name],
		[Address1] = Source.[Address1],
		[Address2] = Source.[Address2],
		[PostCode] = Source.[PostCode],
		[ContactName] = Source.[ContactName],
		[ContactNumber] = Source.[ContactNumber],
		[ContactNumber2] = Source.[ContactNumber2],
		[ContactEmailAddress] = Source.[ContactEmailAddress],
		[StopId] = Source.[StopId],
		[CreatedBy] = Source.[CreatedBy],
		[DateCreated] = Source.[DateCreated],
		[UpdatedBy] = Source.[UpdatedBy],
		[DateUpdated] = Source.[DateUpdated]
	WHEN NOT MATCHED BY TARGET AND @Id = 0 THEN
	INSERT ([Code],[AccountTypeCode],[DepotId],[Name], [Address1], [Address2], [PostCode], [ContactName], [ContactNumber],
				[ContactNumber2], [ContactEmailAddress], [StopId], [CreatedBy],[DateCreated],[UpdatedBy],[DateUpdated])
	VALUES ([Code],[AccountTypeCode],[DepotId],[Name], [Address1], [Address2], [PostCode], [ContactName], [ContactNumber],
				[ContactNumber2], [ContactEmailAddress], [StopId], [CreatedBy],[DateCreated],[UpdatedBy],[DateUpdated])

	OUTPUT $action, inserted.Id INTO @ChangeResult;

	SELECT Id FROM @ChangeResult

END
