CREATE PROCEDURE [dbo].[Stop_GetByRouteNumberAndDropNumber]
	@RouteHeaderCode	NVARCHAR(10),
	@RouteHeaderId   	INT,
	@DropId				NVARCHAR(2)

AS
BEGIN
SELECT [Id],
      [PlannedStopNumber],
	  [RouteHeaderCode],
      [RouteHeaderId],
      [DropId],
      [LocationId],
      [DeliveryDate],
	  [ShellActionIndicator],
	  [CustomerShopReference],
	  [AllowOvers],
	  [CustUnatt],
	  [PHUnatt],
	  [StopStatusId],
	  [StopPerformanceStatusId],
	  [ByPassReasonId]
  FROM [dbo].[Stop]
  WHERE [RouteHeaderCode] = @RouteHeaderCode
  AND [RouteHeaderId] = @RouteHeaderId
  AND [DropId] = @DropId
END