CREATE PROCEDURE  [dbo].[EventSetProcessed]
	@EventId INT,
	@UpdatedBy VARCHAR(50),
	@DateUpdated DATETIME
AS
BEGIN

	UPDATE
		[dbo].[ExceptionEvent]
	SET
		Processed = 1, UpdatedBy = @UpdatedBy, DateUpdated = @DateUpdated
	WHERE
		Id = @EventId

END