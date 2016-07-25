CREATE PROCEDURE [dbo].[Stop_GetByRouteNumberAndDropNumber]
	@RouteHeaderCode	NVARCHAR(10),
	@RouteHeaderId   	INT,
	@DropId				NVARCHAR(2)

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
  WHERE [RouteHeaderCode] = @RouteHeaderCode
  AND [RouteHeaderId] = @RouteHeaderId
  AND [DropId] = @DropId
END