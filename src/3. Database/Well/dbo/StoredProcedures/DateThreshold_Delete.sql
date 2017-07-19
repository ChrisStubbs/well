CREATE PROCEDURE DateThreshold_Delete
	@BranchId int
AS
	DELETE BranchDateThreshold
	WHERE BranchId = @BranchId