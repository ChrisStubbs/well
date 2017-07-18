CREATE PROCEDURE DateThreshold_Get
AS
	SELECT 
		th.BranchId,
		b.Name,
		th.NumberOfDays
	FROM 
		BranchDateThreshold th
		INNER JOIN Branch b
			ON th.BranchId = b.Id
	