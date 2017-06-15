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
		,dmg.PdaReasonDescription as DriverReason
		,li.CreatedBy	as CreatedBy
		,li.CreatedDate as  DateCreated
		,li.LastUpdatedBy as UpdatedBy
		,li.LastUpdatedDate as DateUpdated
		,jd.JobId
	FROM LineItem li
	INNER JOIN JobDetail jd ON jd.LineItemId = li.Id
	left Join JobDetailDamage dmg on dmg.JobDetailId = jd.id 
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
	WHERE 
		lia.IsDeleted = 0

	SELECT 
		c.id
		,c.LineItemActionId
		,c.CommentReasonId
		,cr.Description CommentDescription 
		,c.CreatedBy
		,c.DateCreated 
		,c.UpdatedBy
		,c.DateUpdated
	FROM 
		LineItemActionComment c
	INNER JOIN 
		CommentReason cr on cr.Id = c.CommentReasonId
	INNER JOIN 
		LineItemAction lia on c.LineItemActionId = lia.id
	INNER JOIN @Ids ids ON ids.Value = lia.LineItemId
	WHERE 
		lia.IsDeleted = 0

 RETURN 0
END