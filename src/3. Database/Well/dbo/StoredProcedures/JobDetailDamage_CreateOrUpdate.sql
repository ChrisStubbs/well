CREATE PROCEDURE [dbo].[JobDetailDamage_CreateOrUpdate]
	@Id						INT = 0,
	@JobDetailId			INT,
	@Qty					INT,
	@DamageReasonsId		TINYINT,
	@DamageReasonCategoryId		TINYINT,
	@Username				NVARCHAR(50) 

AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @ChangeResult TABLE (ChangeType VARCHAR(10), Id INTEGER)

	MERGE INTO [JobDetailDamage] AS Target
	USING (VALUES
		(@Id, @JobDetailId, @Qty, @DamageReasonsId, @DamageReasonCategoryId	, @Username, GETDATE(), @Username, GETDATE())
	)
	AS Source ([Id],[JobDetailId],[Qty],[DamageReasonsId], [DamageReasonCategoryId], [CreatedBy],[DateCreated],[UpdatedBy],[DateUpdated])
	ON Target.[Id] = Source.[Id]
	WHEN MATCHED THEN
	UPDATE SET
		[JobDetailId] = Source.[JobDetailId],
		[Qty] = Source.[Qty],
		[DamageReasonsId] = Source.[DamageReasonsId],
		[DamageReasonCategoryId] = Source.[DamageReasonCategoryId],
		[CreatedBy] = Source.[CreatedBy],
		[DateCreated] = Source.[DateCreated],
		[UpdatedBy] = Source.[UpdatedBy],
		[DateUpdated] = Source.[DateUpdated]
	WHEN NOT MATCHED BY TARGET AND @Id = 0 THEN
	INSERT ([JobDetailId],[Qty],[DamageReasonsId], [DamageReasonCategoryId], [CreatedBy],[DateCreated],[UpdatedBy],[DateUpdated])
	VALUES ([JobDetailId],[Qty],[DamageReasonsId], [DamageReasonCategoryId], [CreatedBy],[DateCreated],[UpdatedBy],[DateUpdated])

	OUTPUT $action, inserted.Id INTO @ChangeResult;

	SELECT Id FROM @ChangeResult

END