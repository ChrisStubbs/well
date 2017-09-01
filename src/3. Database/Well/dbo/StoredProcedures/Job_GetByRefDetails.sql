CREATE PROCEDURE [dbo].[Job_GetByRefDetails]
	@JobTypeCode VARCHAR(10),
	@PHAccount NVARCHAR(40),
	@PickListRef NVARCHAR(40),
	@StopId INT
AS
BEGIN
	SELECT [Id]
  FROM [dbo].[Job]
  WHERE 
  [JobTypeCode] = @JobTypeCode
  AND [PHAccount] = @PHAccount
  AND [PickListRef] = @PickListRef
  AND [StopId] = @StopId
  AND DateDeleted IS NULL

END
