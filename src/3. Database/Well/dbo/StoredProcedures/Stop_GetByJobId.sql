CREATE PROCEDURE [dbo].[Stop_GetByJobId]
	@JobId INT

AS
BEGIN
SELECT stp.[Id],
      stp.[PlannedStopNumber],
	  stp.[RouteHeaderCode],
      stp.[RouteHeaderId],
      stp.[DropId],
      stp.[LocationId],
      stp.[DeliveryDate],
	  stp.[ShellActionIndicator],
	  stp.[AllowOvers],
	  stp.[CustUnatt],
	  stp.[PHUnatt],
	  stp.[StopStatusId],
	  stp.[StopPerformanceStatusId],
	  stp.[ByPassReasonId]
  FROM [dbo].[Stop] stp
  INNER JOIN [dbo].[Job] jb
  ON stp.Id = jb.StopId
  WHERE jb.[Id] = @JobId
END
RETURN 0
