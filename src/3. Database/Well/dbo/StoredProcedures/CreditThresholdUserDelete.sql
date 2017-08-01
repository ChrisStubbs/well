CREATE PROCEDURE [dbo].[CreditThresholdUserDelete]
	@UserId int
AS
	DELETE FROM CreditThresholdUser WHERE UserId = @UserId
RETURN 0
