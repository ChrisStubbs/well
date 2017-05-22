CREATE PROCEDURE  [dbo].GetBranchesForCreditThreshold
	@CreditThresholdId INT
AS
BEGIN

	SET NOCOUNT ON;

	SELECT b.Id, b.Name
	FROM [dbo].[Branch] b JOIN CreditThresholdToBranch sb on b.Id = sb.BranchId
	WHERE sb.CreditThresholdId = @CreditThresholdId

END