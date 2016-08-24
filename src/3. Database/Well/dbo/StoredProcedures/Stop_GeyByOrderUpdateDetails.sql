CREATE PROCEDURE [dbo].[Stop_GeyByOrderUpdateDetails]
	@RouteHeaderCode NVARCHAR(10),
	@DropId          NVARCHAR(2),
	@LocationId		 NVARCHAR(20),
	@DeliveryDate    DATETIME
AS
	SELECT [Id]
      ,[PlannedStopNumber]
      ,[PlannedArriveTime]
      ,[PlannedDepartTime]
      ,[RouteHeaderCode]
      ,[RouteHeaderId]
      ,[DropId]
      ,[LocationId]
      ,[DeliveryDate]
      ,[SpecialInstructions]
      ,[StartWindow]
      ,[EndWindow]
      ,[TextField1]
      ,[TextField2]
      ,[TextField3]
      ,[TextField4]
      ,[StopStatusId]
      ,[StopPerformanceStatusId]
      ,[ByPassReasonId]
      ,[CreatedBy]
      ,[DateCreated]
      ,[UpdatedBy]
      ,[DateUpdated]
      ,[Version]
  FROM [dbo].[Stop]
  WHERE [RouteHeaderCode] = @RouteHeaderCode
  AND [DropId] = @DropId
  AND [LocationId] = @LocationId
  AND [DeliveryDate] = @DeliveryDate
		
RETURN 0
