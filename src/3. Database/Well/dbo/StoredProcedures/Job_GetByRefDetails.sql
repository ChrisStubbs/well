CREATE PROCEDURE [dbo].[Job_GetByRefDetails]
	@Ref1 NVARCHAR(40),
	@Ref2 NVARCHAR(40),
	@StopId INT
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
  WHERE [JobRef1] = @Ref1
  AND [JobRef2] = @Ref2
  AND [StopId] = @StopId
RETURN 0
