CREATE PROCEDURE [dbo].[LineItem_GetByIds]
	@Ids dbo.IntTableType	READONLY
AS
 BEGIN
	SELECT 
		li.Id
		,li.LineNumber 
		,li.ProductCode
		,li.ProductDescription 
		,li.AmendedDeliveryQuantity
		,li.AmendedShortQuantity 
		,li.OriginalShortQuantity 
		,li.ActivityId
		,jd.OriginalDespatchQty AS OriginalDespatchQuantity
		,jd.DeliveredQty as DeliveredQuantity
		,li.CreatedBy	as CreatedBy
		,li.CreatedDate as  DateCreated
		,li.LastUpdatedBy as UpdatedBy
		,li.LastUpdatedDate as DateUpdated
	FROM LineItem li
	INNER JOIN JobDetail jd ON jd.LineItemId = li.Id 
	INNER JOIN @Ids ids ON ids.Value = li.Id

	SELECT 
		lia.Id
		,lia.LineItemId
		,lia.ExceptionTypeId AS ExceptionType
		,lia.Quantity 
		,lia.SourceId as Source
		,lia.ReasonId as Reason
		,lia.ReplanDate
		,lia.SubmittedDate
		,lia.ApprovalDate
		,lia.ApprovedBy 
		,lia.ActionedBy 
		,lia.Originator 
		,lia.DeliveryActionId as DeliveryAction
		,lia.CreatedBy	as CreatedBy
		,lia.CreatedDate as  DateCreated
		,lia.LastUpdatedBy as UpdatedBy
		,lia.LastUpdatedDate as DateUpdated
	FROM LineItemAction lia
	INNER JOIN @Ids ids ON ids.Value = lia.LineItemId

 RETURN 0
END