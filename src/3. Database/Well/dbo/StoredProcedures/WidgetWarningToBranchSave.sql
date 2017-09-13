CREATE PROCEDURE [dbo].[WidgetWarningToBranchSave]
	@BranchId INT, 
	@WidgetId INT
AS
BEGIN
	SET NOCOUNT ON;

    INSERT INTO [dbo].[WidgetToBranch]
           ([Branch_Id]
		   ,[Widget_Id])
     VALUES
           (@BranchId
		   ,@WidgetId);
END
