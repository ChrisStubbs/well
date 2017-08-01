CREATE PROCEDURE [dbo].[CreditThresholdGetByUser]
	@UserId int
AS
	SELECT ct.* FROM
		CreditThreshold ct
		LEFT JOIN CreditThresholdUser ctu on ct.Id = ctu.CreditThresholdId
		WHERE ctu.UserId = @UserId
