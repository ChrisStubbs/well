CREATE PROCEDURE [dbo].[Job_GetByStopId]
	@StopId int = 0
AS
	SELECT TOP 1000 [Id]
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
      ,[PerformanceStatusId]
      ,[ByPassReasonId]
      ,[StopId]
      ,[CreatedBy]
      ,[DateCreated]
      ,[UpdatedBy]
      ,[DateUpdated]
      ,[Version]
  FROM [dbo].[Job]
  WHERE [StopId] = @StopId

RETURN 0
