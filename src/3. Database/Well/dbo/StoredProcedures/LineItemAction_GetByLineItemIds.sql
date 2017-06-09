CREATE PROCEDURE [dbo].[LineItemAction_GetByLineItemIds]
	@Ids dbo.IntTableType READONLY
AS
BEGIN
	SELECT lia.Id
		, li.ProductCode
		, li.ProductDescription
		, lia.Originator
		, et.DisplayName AS Exception
		, jd.OriginalDespatchQty AS Invoiced
		, jd.DeliveredQty AS Delivered
		, lia.Quantity
		, jds.[Description] AS [Source]
		, jdr.[Description] AS Reason
		, lia.ReplanDate AS Erdd
		, lia.ActionedBy
		, lia.ApprovedBy  
	FROM LineItem li
	INNER JOIN JobDetail jd on jd.LineItemId = li.Id
	LEFT JOIN LineItemAction lia on lia.LineItemId = li.id
	LEFT JOIN ExceptionType et on et.Id = lia.ExceptionTypeId
	LEFT JOIN JobDetailSource jds on jds.Id = lia.SourceId
	LEFT JOIN JobDetailReason jdr on jdr.Id = lia.ReasonId
	INNER JOIN @Ids i on i.Value = li.Id
	WHERE
		lia.IsDeleted = 0

	RETURN 0
END