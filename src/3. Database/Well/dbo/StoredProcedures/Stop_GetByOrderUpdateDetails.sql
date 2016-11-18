CREATE PROCEDURE [dbo].[Stop_GetByOrderUpdateDetails]
	@transportOrderReference	VARCHAR(50)
AS
BEGIN

  SELECT 
	  [Id],
	  [TransportOrderReference],
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
  FROM 
	  [dbo].[Stop]
  WHERE 
	  [TransportOrderReference] = @transportOrderReference
END