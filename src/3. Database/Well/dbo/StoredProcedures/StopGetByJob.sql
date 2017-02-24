CREATE PROCEDURE [dbo].[StopGetByJob]
	@Picklist VARCHAR(50),
	@Account VARCHAR(50)
AS
BEGIN
SELECT s.[Id],
      s.[PlannedStopNumber],
	  s.[RouteHeaderCode],
      s.[RouteHeaderId],
      s.[DropId],
      s.[LocationId],
      s.[DeliveryDate],
	  s.[ShellActionIndicator],
	  s.[AllowOvers],
	  s.[CustUnatt],
	  s.[PHUnatt],
	  s.[StopStatusCode],
	  s.[StopStatusDescription],
	  s.[PerformanceStatusCode],
	  s.[PerformanceStatusDescription],
	  s.[Reason],
	  s.[TransportOrderReference]
  FROM [dbo].[Stop] s
  JOIN Job j on j.StopId = s.Id
  WHERE j.PickListRef = @Picklist AND j.PHAccount = @Account
END