CREATE PROCEDURE [dbo].[Job_GetByRefDetails]
	@PHAccount NVARCHAR(40),
	@PickListRef NVARCHAR(40),
	@StopId INT
AS
BEGIN
	SELECT [Id]
  FROM [dbo].[Job]
  WHERE [PHAccount] = @PHAccount
  AND [PickListRef] = @PickListRef
  AND [StopId] = @StopId

END
