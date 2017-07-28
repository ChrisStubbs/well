IF OBJECT_ID('dbo.BranchDateThreshold', 'U') IS NOT NULL 
BEGIN
	DELETE FROM dbo.BranchDateThreshold WHERE BranchId = 99
END

IF OBJECT_ID('dbo.Branch', 'U') IS NOT NULL 
BEGIN
	DELETE FROM dbo.Branch WHERE Id = 99
END