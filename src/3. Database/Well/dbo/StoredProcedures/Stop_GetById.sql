CREATE PROCEDURE [dbo].[Stop_GetById]
	@Id INT

AS
BEGIN
SELECT [Id],
      [PlannedStopNumber],
      [PlannedArriveTime],
      [PlannedDepartTime],
	  [RouteHeaderCode],
      [RouteHeaderId],
      [DropId],
      [LocationId],
      [DeliveryDate],
      [SpecialInstructions],
      [StartWindow],
      [EndWindow],
      [TextField1],
      [TextField2],
      [TextField3],
      [TextField4],
	  [StopStatusId],
	  [StopPerformanceStatusId],
	  [ByPassReasonId]
  FROM [dbo].[Stop]
  WHERE [Id] = @Id
END
