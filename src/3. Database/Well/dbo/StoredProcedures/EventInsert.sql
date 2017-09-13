CREATE PROCEDURE  [dbo].[EventInsert]
	@Event VARCHAR(MAX),
	@ExceptionActionId INT,
	@DateCanBeProcessed DATETIME,
	@SourceId VARCHAR(50) = NULL,
	@CreatedBy VARCHAR(50),
	@DateCreated DATETIME,
	@UpdatedBy VARCHAR(50),
	@DateUpdated DATETIME
AS
BEGIN

	INSERT INTO [dbo].[ExceptionEvent]
	(
		[Event], 
		ExceptionActionId, 
		Processed, 
		DateCanBeProcessed, 
		SourceId,
		CreatedBy, 
		DateCreated, 
		UpdatedBy, 
		DateUpdated
	)
	VALUES 
	(
		@Event, 
		@ExceptionActionId,
		0, 
		@DateCanBeProcessed,
		@SourceId,
		@CreatedBy, 
		@DateCreated, 
		@UpdatedBy, 
		@DateUpdated
	)

	SELECT CAST(SCOPE_IDENTITY() as int);

END