CREATE PROCEDURE [dbo].[SeasonalDatesByBranchGet]
	@branchId INT
AS
BEGIN
SELECT
	s.[Id], s.[Description], s.[From], s.[To], s.[CreatedBy], s.[CreatedDate], s.[LastUpdatedBy], s.[LastUpdatedDate]
FROM
	 [dbo].[SeasonalDate] s 
JOIN
	[dbo].[SeasonalDateToBranch] b on b.SeasonalDateId = s.Id
WHERE
	b.BranchId = @branchId
		   
END