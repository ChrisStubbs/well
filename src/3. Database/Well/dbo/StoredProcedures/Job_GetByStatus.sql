CREATE PROCEDURE [dbo].[Job_GetByStatus]
	@PerformanceStatusCode VARCHAR(5)

AS
BEGIN
SELECT j.[Id]
      ,j.[Sequence]
      ,j.[JobTypeCode]
      ,j.[JobRef1]
      ,j.[JobRef2]
      ,j.[JobRef3]
      ,j.[JobRef4]
      ,j.[OrderDate]
      ,j.[Originator]
      ,j.[TextField1]
      ,j.[TextField2]
      ,j.[PerformanceStatusId]
      ,j.[StopId]
  FROM [dbo].[Job] AS j
  INNER JOIN PerformanceStatus AS ps 
  ON ps.Id = j.PerformanceStatusId	
  WHERE ps.Code = @PerformanceStatusCode
END
