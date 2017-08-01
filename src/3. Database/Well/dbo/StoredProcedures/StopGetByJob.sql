CREATE PROCEDURE [dbo].[StopGetByJob]
	@Picklist VARCHAR(50),
	@Account VARCHAR(50),
	@BranchId INT
AS
BEGIN
SELECT s.[Id]
  FROM [dbo].[Stop] s
  JOIN Job j on j.StopId = s.Id
  JOIN RouteHeader rh on rh.Id = s.RouteHeaderId
  WHERE j.PickListRef = @Picklist AND j.PHAccount = @Account AND rh.RouteOwnerId = @BranchId
		AND s.DateDeleted IS NULL
END