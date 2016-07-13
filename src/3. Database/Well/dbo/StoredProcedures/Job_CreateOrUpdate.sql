CREATE PROCEDURE [dbo].[Job_CreateOrUpdate]
	@Id						INT = 0,
	@Sequence				INT,
	@Username				VARCHAR(50),
	@JobTypeCode			NVARCHAR(50),
	@JobRef1				NVARCHAR(50),
	@JobRef2				NVARCHAR(50),
	@JobRef3				NVARCHAR(50),
	@JobRef4				NVARCHAR(10),
	@OrderDate				DATETIME,
	@Originator				NVARCHAR(50),
	@TextField1				NVARCHAR(50),
	@TextField2				NVARCHAR(50),
	@PerformanceStatusId	TINYINT = NULL,
	@ByPassReasonId         TINYINT = NULL,
	@StopId					INT

AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @ChangeResult TABLE (ChangeType VARCHAR(10), Id INTEGER)

	MERGE INTO [Job] AS Target
	USING (VALUES
		(@Id, @Sequence, @JobTypeCode, @JobRef1, @JobRef2, @JobRef3, @JobRef4, @OrderDate, @Originator, @TextField1, @TextField2,
		 @PerformanceStatusId,@ByPassReasonId, @StopId, @Username, GETDATE(), @Username, GETDATE())
	)
	AS Source ([Id],[Sequence],[JobTypeCode],[JobRef1], [JobRef2], [JobRef3], [JobRef4], [OrderDate], [Originator],
				[TextField1], [TextField2], [PerformanceStatusId],[ByPassReasonId], [StopId], [CreatedBy],[DateCreated],[UpdatedBy],[DateUpdated])
	ON Target.[Id] = Source.[Id]
	WHEN MATCHED THEN
	UPDATE SET
		[Sequence] = Source.[Sequence],
		[JobTypeCode] = Source.[JobTypeCode],
		[JobRef1] = Source.[JobRef1],
		[JobRef2] = Source.[JobRef2],
		[JobRef3] = Source.[JobRef3],
		[JobRef4] = Source.[JobRef4],
		[OrderDate] = Source.[OrderDate],
		[Originator] = Source.[Originator],
		[TextField1] = Source.[TextField1],
		[TextField2] = Source.[TextField2],
		[PerformanceStatusId] = Source.[PerformanceStatusId],
		[ByPassReasonId] = Source.[ByPassReasonId],
		[StopId] = Source.[StopId],
		[CreatedBy] = Source.[CreatedBy],
		[DateCreated] = Source.[DateCreated],
		[UpdatedBy] = Source.[UpdatedBy],
		[DateUpdated] = Source.[DateUpdated]
	WHEN NOT MATCHED BY TARGET AND @Id = 0 THEN
	INSERT ([Sequence],[JobTypeCode],[JobRef1], [JobRef2], [JobRef3], [JobRef4], [OrderDate], [Originator],
				[TextField1], [TextField2], [PerformanceStatusId],[ByPassReasonId], [StopId], [CreatedBy],[DateCreated],[UpdatedBy],[DateUpdated])
	VALUES ([Sequence],[JobTypeCode],[JobRef1], [JobRef2], [JobRef3], [JobRef4], [OrderDate], [Originator],
				[TextField1], [TextField2], [PerformanceStatusId],[ByPassReasonId], [StopId], [CreatedBy],[DateCreated],[UpdatedBy],[DateUpdated])

	OUTPUT $action, inserted.Id INTO @ChangeResult;

	SELECT Id FROM @ChangeResult

END

