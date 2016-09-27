CREATE PROCEDURE  [dbo].GetBranchesForCleanPreference
	@CleanPreferenceId INT
AS
BEGIN

	SET NOCOUNT ON;

	SELECT b.Id, b.Name
	FROM [dbo].[Branch] b JOIN CleanPreferenceToBranch sb on b.Id = sb.BranchId
	WHERE sb.CleanPreferenceId = @CleanPreferenceId

END