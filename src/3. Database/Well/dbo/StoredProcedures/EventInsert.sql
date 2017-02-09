CREATE PROCEDURE  [dbo].[EventInsert]
	@Event VARCHAR(MAX),
	@ExceptionActionId INT,
	@DateCanBeProcessed DATETIME,
	@CreatedBy VARCHAR(50),
	@DateCreated DATETIME,
	@UpdatedBy VARCHAR(50),
	@DateUpdated DATETIME
AS
BEGIN

	INSERT INTO [dbo].[ExceptionEvent]([Event], ExceptionActionId, Processed, DateCanBeProcessed, CreatedBy, DateCreated, UpdatedBy, DateUpdated)
	VALUES (@Event, @ExceptionActionId, 0, @DateCanBeProcessed, @CreatedBy, @DateCreated, @UpdatedBy, @DateUpdated)

END