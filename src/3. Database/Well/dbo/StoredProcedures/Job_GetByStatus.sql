CREATE PROCEDURE [dbo].[Job_GetByStatus]
	@PerformanceStatusCode VARCHAR(5)

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
      ,[PerformanceStatusCode]
      ,[StopId]
  FROM [dbo].[Job]
  WHERE [PerformanceStatusCode] = @PerformanceStatusCode
END
