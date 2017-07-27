MERGE INTO BranchDateThreshold AS Target
USING	
(
	SELECT 
		b.Id AS BranchId,
		2 AS NumberOfDays,
		'deployment' AS CreatedBy,
		GETDATE() AS DateCreated,
		'deployment' AS UpdatedBy,
		GETDATE() AS DateUpdated
	FROM  
		Branch b
)
AS Source 
	ON Target.BranchId = Source.BranchId

WHEN NOT MATCHED BY TARGET THEN
	INSERT (BranchId, NumberOfDays, CreatedBy, DateCreated, UpdatedBy, DateUpdated)
	VALUES (BranchId, NumberOfDays, CreatedBy, DateCreated, UpdatedBy, DateUpdated)
WHEN NOT MATCHED BY SOURCE THEN DELETE;