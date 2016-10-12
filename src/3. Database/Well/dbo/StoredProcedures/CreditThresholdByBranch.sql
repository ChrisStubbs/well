CREATE PROCEDURE [dbo].[CreditThresholdByBranch]
	@branchId INT
AS
BEGIN
SELECT
	c.[Id], c.[ThresholdLevelId], c.[Threshold], c.[CreatedBy], c.[CreatedDate], c.[LastUpdatedBy], c.[LastUpdatedDate]
FROM
	 [dbo].[CreditThreshold] c
JOIN
	[dbo].[CreditThresholdToBranch] b on b.CreditThresholdId = c.Id
WHERE
	b.BranchId = @branchId
		   
END