CREATE PROCEDURE [dbo].[Job_GetWithLineItemActions]
	@Ids dbo.IntTableType READONLY
AS
BEGIN

	SELECT DISTINCT j.Id 
	FROM LineItemAction lia
	INNER JOIN LineItem li ON li.Id = lia.LineItemId
	INNER JOIN Activity a ON a.Id = li.ActivityId
	INNER JOIN Job j ON j.ActivityId = a.Id
	INNER JOIN @Ids  i on i.Value = j.Id
	WHERE lia.DateDeleted is null
		
END