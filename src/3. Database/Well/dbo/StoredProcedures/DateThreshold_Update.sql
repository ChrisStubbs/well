CREATE PROCEDURE DateThreshold_Update
	@NumberOfDays	TinyInt,
	@BranchId		Int
AS
	 UPDATE [BranchDateThreshold]		
	SET 
		NumberOfDays = @NumberOfDays
	WHERE 
		BranchId = @BranchId