CREATE PROCEDURE [dbo].[Job_GetByAccountPicklistAndStopId]
	@AccountId		NVARCHAR(50),
	@PicklistId		NVARCHAR(50),
	@StopId         INT

AS
BEGIN
SELECT [Id]
      ,[Sequence]
      ,[JobTypeCode]
      ,[JobRef1]
      ,[JobRef2]
      ,[JobRef3]
      ,[JobRef4]
      ,[OrderDate]
      ,[Originator]
      ,[TextField1]
      ,[TextField2]
	  ,[PerformanceStatusId] as PerformanceStatus
	  ,[ByPassReasonId] as ByPassReason
      ,[StopId]
  FROM dbo.[Job]
  WHERE [JobRef1] = @AccountId
  AND [JobRef2] = @PicklistId
  AND [StopId] = @StopId
END