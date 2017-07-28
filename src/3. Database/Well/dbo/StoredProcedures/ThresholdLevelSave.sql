CREATE PROCEDURE  [dbo].[ThresholdLevelSave]
	@CreditThresholdId INT,
	@UserId INT
AS
BEGIN
	UPDATE [dbo].[User] SET [CreditThresholdId] = @CreditThresholdId WHERE Id = @UserId
END