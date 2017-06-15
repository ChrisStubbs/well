CREATE PROCEDURE  [dbo].[RemovePendingCredit]
	@jobId VARCHAR(50)
AS
BEGIN

	SET NOCOUNT ON;
	UPDATE PendingCredit 
	SET DateDeleted = GETDATE()
	WHERE JobId = @jobId
END