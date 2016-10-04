CREATE PROCEDURE [dbo].[CleanPreferenceByBranchGet]
	@branchId INT
AS
BEGIN
SELECT
	c.[Id], c.[Days], c.[CreatedBy], c.[CreatedDate], c.[LastUpdatedBy], c.[LastUpdatedDate]
FROM
	 [dbo].[CleanPreference] c
JOIN
	[dbo].[CleanPreferenceToBranch] b on c.Id = b.CleanPreferenceId
WHERE b.BranchId = @branchId
		   
END