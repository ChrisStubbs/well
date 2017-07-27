CREATE PROCEDURE [dbo].[StopGetByJob]
	@Picklist VARCHAR(50),
	@Account VARCHAR(50)
AS
BEGIN
SELECT s.[Id]
  FROM [dbo].[Stop] s
  JOIN Job j on j.StopId = s.Id
  WHERE j.PickListRef = @Picklist 
		AND j.PHAccount = @Account
		AND s.DateDeleted IS NULL
END