Create PROCEDURE [dbo].[SeasonalDatesToBranchSave]
	@BranchId INT, 
	@SeasonalDateId INT
AS
BEGIN
	SET NOCOUNT ON;

    INSERT INTO [dbo].[SeasonalDateToBranch]
           ([BranchId]
		   ,[SeasonalDateId])
     VALUES
           (@BranchId
		   ,@SeasonalDateId);
END