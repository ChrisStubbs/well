CREATE PROCEDURE [dbo].[SeasonalDatesBranchesGet]
	@seasonalDateId INT
AS
BEGIN
SELECT
	s.[BranchId] as Id, b.TranscendMapping as Name
FROM
	 [dbo].[SeasonalDateToBranch] s
JOIN
	[dbo].[Branch] b on b.Id = s.BranchId
WHERE
	s.SeasonalDateId = @seasonalDateId
		   
END