CREATE PROCEDURE [dbo].[Stop_GetById]
	@Id INT

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
	  [CustUnatt] ,
	  [PHUnatt] ,
	  [StopStatusCode],
	  [StopStatusDescription],
	  [PerformanceStatusCode],
	  [PerformanceStatusDescription],
	  [Reason],
	  [IsDeleted],
      [CreatedBy],
      [DateCreated],
      [UpdatedBy],
      [DateUpdated]
  FROM [dbo].[Stop]
  WHERE [Id] = @Id
END
