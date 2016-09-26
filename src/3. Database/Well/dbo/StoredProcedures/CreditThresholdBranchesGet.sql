CREATE PROCEDURE [dbo].[CreditThresholdBranchesGet]
	@creditThresholdId INT
AS
BEGIN
SELECT
	s.[BranchId] as Id, b.TranscendMapping as Name
FROM
	 [dbo].[CreditThresholdToBranch] s
JOIN
	[dbo].[Branch] b on b.Id = s.BranchId
WHERE
	s.CreditThresholdId = @creditThresholdId
		   
END