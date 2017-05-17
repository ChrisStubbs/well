CREATE PROCEDURE [dbo].[LineItem_GetByIds]
	@Ids dbo.IntTableType	READONLY
AS
 BEGIN
	SELECT   li.LineNumber 
		,  li.ProductCode
		,  li.ProductDescription 
		,  li.AmendedDeliveryQuantity
		,  li.AmendedShortQuantity 
		,  li.OriginalShortQuantity 
		,  li.ActivityId
		,  jd.OriginalDespatchQty AS OriginalDespatchQuantity
		,  jd.DeliveredQty as DeliveredQuantity
		, li.Id
	FROM LineItem li
	INNER JOIN JobDetail jd ON jd.LineItemId = li.Id 
	INNER JOIN @Ids ids ON ids.Value = li.Id

	SELECT lia.Id
		,LineItemId
		,et.DisplayName AS ExceptionType 
		,[Quantity] 
		,[SourceId]
		,[ReasonId]
		,[ReplanDate]
		,[SubmittedDate]
		,[ApprovalDate]
		,[ApprovedBy] 
	FROM LineItemAction lia
	INNER JOIN ExceptionType et on et.Id = lia.ExceptionTypeId
	INNER JOIN @Ids ids ON ids.Value = lia.LineItemId

 RETURN 0
END