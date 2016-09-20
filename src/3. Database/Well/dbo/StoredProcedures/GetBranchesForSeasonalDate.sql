CREATE PROCEDURE  [dbo].GetBranchesForSeasonalDate
	@SeasonalDateId INT
AS
BEGIN

	SET NOCOUNT ON;

	SELECT b.Id, b.Name
	FROM [dbo].[Branch] b JOIN SeasonalDateToBranch sb on b.Id = sb.BranchId
	WHERE sb.SeasonalDateId = @SeasonalDateId

END