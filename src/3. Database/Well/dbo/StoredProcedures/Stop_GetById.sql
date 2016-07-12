CREATE PROCEDURE [dbo].[Stop_GetById]
	@Id INT

AS
BEGIN
SELECT [Id],
      [PlannedStopNumber],
      [PlannedArriveTime],
      [PlannedDepartTime],
      [RouteHeaderId],
      [DropId],
      [LocatiodId],
      [DeliveryDate],
      [SpecialInstructions],
      [StartWindow],
      [EndWindow],
      [TextField1],
      [TextField2],
      [TextField3],
      [TextField4]
  FROM [Well].[dbo].[Stop]
  WHERE [Id] = @Id
END
