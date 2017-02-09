CREATE PROCEDURE  [dbo].[RemovePendingCredit]
	@jobId VARCHAR(50)
AS
BEGIN

	SET NOCOUNT ON;
	UPDATE PendingCreditToUSer SET IsDeleted = 1 WHERE JobId = @jobId
END