Create PROCEDURE [dbo].[CreditThresholdToBranchSave]
	@BranchId INT, 
	@CreditThresholdId INT
AS
BEGIN
	SET NOCOUNT ON;

    INSERT INTO [dbo].[CreditThresholdToBranch]
           ([BranchId]
		   ,[CreditThresholdId])
     VALUES
           (@BranchId
		   ,@CreditThresholdId);
END