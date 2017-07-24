CREATE PROCEDURE  [dbo].[EventInsert]
	@Event VARCHAR(MAX),
	@ExceptionActionId INT,
	@DateCanBeProcessed DATETIME,
	@CreatedBy VARCHAR(50),
	@DateCreated DATETIME,
	@UpdatedBy VARCHAR(50),
	@DateUpdated DATETIME,
	@EntityId VARCHAR(50)
AS
BEGIN

	INSERT INTO [dbo].[ExceptionEvent]([Event], ExceptionActionId, Processed, DateCanBeProcessed, EntityId, CreatedBy, DateCreated, UpdatedBy, DateUpdated)
	VALUES (@Event, @ExceptionActionId, 0, @DateCanBeProcessed, @EntityId, @CreatedBy, @DateCreated, @UpdatedBy, @DateUpdated)

END