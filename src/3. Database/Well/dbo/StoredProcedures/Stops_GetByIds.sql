CREATE PROCEDURE [dbo].[Stops_GetByIds]
	@Ids dbo.IntTableType	READONLY
AS
BEGIN
	SET NOCOUNT ON;

	SELECT s.[Id]
		  ,s.[TransportOrderReference]
		  ,s.[PlannedStopNumber]
		  ,s.[RouteHeaderCode]
		  ,s.[RouteHeaderId]
		  ,s.[DropId]
		  ,s.[Previously]
		  ,s.[LocationId]
		  ,s.[DeliveryDate]
		  ,s.[ShellActionIndicator]
		  ,s.[AllowOvers]
		  ,s.[CustUnatt]
		  ,s.[PHUnatt]
		  ,s.[StopStatusCode]
		  ,s.[StopStatusDescription]
		  ,s.[PerformanceStatusCode]
		  ,s.[PerformanceStatusDescription]
		  ,s.[Reason] AS StopByPassReason
		  ,s.[DateDeleted]
		  ,s.[ActualPaymentCash]
		  ,s.[ActualPaymentCheque]
		  ,s.[ActualPaymentCard]
		  ,s.[AccountBalance]
		  ,s.[CreatedBy]
		  ,s.[DateCreated]
		  ,s.[UpdatedBy]
		  ,s.[DateUpdated]
		  ,s.[Location_Id] as LocationId
		  ,CAST(s.WellStatus AS int) as WellStatus
	FROM 
		[dbo].[Stop] s
	INNER JOIN @Ids ids ON ids.Value = s.Id	
	WHERE
			s.DateDeleted Is Null

	SELECT a.[Id]
		  ,a.[Code]
		  ,a.[AccountTypeCode]
		  ,a.[DepotId]
		  ,a.[Name]
		  ,a.[Address1]
		  ,a.[Address2]
		  ,a.[PostCode]
		  ,a.[ContactName]
		  ,a.[ContactNumber]
		  ,a.[ContactNumber2]
		  ,a.[ContactEmailAddress]
		  ,a.[DateDeleted]
		  ,a.[StopId]
		  ,a.[CreatedBy]
		  ,a.[DateCreated]
		  ,a.[UpdatedBy]
		  ,a.[DateUpdated]
		  ,a.[LocationId]
  FROM [dbo].[Account]  a
  INNER JOIN @Ids ids ON ids.Value = a.stopId	
  WHERE
			a.DateDeleted Is Null
END
