CREATE PROCEDURE [dbo].[CleanPreferencesBranchesGet]
	@cleanPreferenceId INT
AS
BEGIN
SELECT
	s.[BranchId] as Id, b.TranscendMapping as Name
FROM
	 [dbo].[CleanPreferenceToBranch] s
JOIN
	[dbo].[Branch] b on b.Id = s.BranchId
WHERE
	s.CleanPreferenceId = @cleanPreferenceId
		   
END