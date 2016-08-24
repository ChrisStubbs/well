CREATE PROCEDURE [dbo].[Stop_GetByJobId]
	@JobId INT

AS
BEGIN
SELECT stp.[Id],
      stp.[PlannedStopNumber],
      stp.[PlannedArriveTime],
      stp.[PlannedDepartTime],
	  stp.[RouteHeaderCode],
      stp.[RouteHeaderId],
      stp.[DropId],
      stp.[LocationId],
      stp.[DeliveryDate],
      stp.[SpecialInstructions],
      stp.[StartWindow],
      stp.[EndWindow],
      stp.[TextField1],
      stp.[TextField2],
      stp.[TextField3],
      stp.[TextField4],
	  stp.[StopStatusId],
	  stp.[StopPerformanceStatusId],
	  stp.[ByPassReasonId]
  FROM [dbo].[Stop] stp
  INNER JOIN [dbo].[Job] jb
  ON stp.Id = jb.StopId
  WHERE jb.[Id] = @JobId
END
RETURN 0
