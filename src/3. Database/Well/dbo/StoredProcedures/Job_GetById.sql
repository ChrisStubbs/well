CREATE PROCEDURE [dbo].[Job_GetById]
	@Id INT

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
  WHERE [Id] = @Id
END
