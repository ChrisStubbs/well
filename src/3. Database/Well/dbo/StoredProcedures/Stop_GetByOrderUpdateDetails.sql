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
	  [AllowOvers],
	  [CustUnatt],
	  [PHUnatt],
	  [StopStatusId],
	  [StopPerformanceStatusId],
	  [Reason]
  FROM 
	  [dbo].[Stop]
  WHERE 
	  [TransportOrderReference] = @transportOrderReference
END