CREATE PROCEDURE [dbo].[Job_GetByAccountPicklistAndStopId]
	@AccountId		VARCHAR(50),
	@PicklistId		VARCHAR(50),
	@StopId         INT

AS
BEGIN
	SELECT [Id]
  FROM [dbo].[Job]
  WHERE [PHAccount] = @AccountId
  AND [PickListRef] = @PicklistId
  AND [StopId] = @StopId
END