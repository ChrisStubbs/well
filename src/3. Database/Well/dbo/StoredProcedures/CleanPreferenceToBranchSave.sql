Create PROCEDURE [dbo].[CleanPreferenceToBranchSave]
	@BranchId INT, 
	@CleanPreferenceId INT
AS
BEGIN
	SET NOCOUNT ON;

    INSERT INTO [dbo].[CleanPreferenceToBranch]
           ([BranchId]
		   ,[CleanPreferenceId])
     VALUES
           (@BranchId
		   ,@CleanPreferenceId);
END