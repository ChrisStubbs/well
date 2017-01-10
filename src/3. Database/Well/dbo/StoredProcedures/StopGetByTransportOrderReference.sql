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
	  [AllowOvers],
	  [CustUnatt],
	  [PHUnatt],
	  [StopStatusId],
	  [StopPerformanceStatusId],
	  [Reason],
	  [TransportOrderReference]
  FROM [dbo].[Stop]
  WHERE [TransportOrderReference] = @TransportOrderReference
END