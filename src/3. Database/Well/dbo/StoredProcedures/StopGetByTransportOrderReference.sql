CREATE PROCEDURE [dbo].[StopGetByTransportOrderReference]
	@TransportOrderReference	VARCHAR(50)
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
	  [ByPassReasonId],
	  [TransportOrderReference]
  FROM [dbo].[Stop]
  WHERE [TransportOrderReference] = @TransportOrderReference
END