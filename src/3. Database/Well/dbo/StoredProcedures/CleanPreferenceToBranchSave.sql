Create PROCEDURE [dbo].[CleanPreferenceToBranchSave]
	@BranchId INT, 
	@CleanPreferenceId INT
AS
begin

	SET NOCOUNT ON;

    INSERT INTO [dbo].[CleanPreferenceToBranch]
           ([BranchId]
		   ,[CleanPreferenceId])
     VALUES
           (@BranchId
		   ,@CleanPreferenceId);
end