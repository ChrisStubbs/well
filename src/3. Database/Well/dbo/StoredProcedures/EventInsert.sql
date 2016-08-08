CREATE PROCEDURE  [dbo].[EventInsert]
	@Event VARCHAR(2500),
	@ExceptionActionId INT,
	@CreatedBy VARCHAR(50),
	@DateCreated DATETIME,
	@UpdatedBy VARCHAR(50),
	@DateUpdated DATETIME
AS
BEGIN

	INSERT INTO [dbo].[ExceptionEvent]([Event], ExceptionActionId, Processed, CreatedBy, DateCreated, UpdatedBy, DateUpdated)
	VALUES (@Event, @ExceptionActionId, 0, @CreatedBy, @DateCreated, @UpdatedBy, @DateUpdated)

END