CREATE PROCEDURE [dbo].[LineItem_GetByActivityId]
	@Id Int
AS
 BEGIN

	DECLARE @LineItems TABLE(
		LineNumber INT
		,  ProductCode VARCHAR(60)
		,  ProductDescription VARCHAR(100)
		,  AmendedDeliveryQuantity INT
		,  AmendedShortQuantity INT
		,  OriginalShortQuantity INT
		, Id INT
	
	)

	INSERT INTO @LineItems(LineNumber 
		,  ProductCode
		,  ProductDescription 
		,  AmendedDeliveryQuantity
		,  AmendedShortQuantity 
		,  OriginalShortQuantity 
		, Id)

	SELECT   LineNumber 
		,  ProductCode
		,  ProductDescription 
		,  AmendedDeliveryQuantity
		,  AmendedShortQuantity 
		,  OriginalShortQuantity 
		, Id
	FROM LineItem li
	WHERE ActivityId = @Id

	SELECT  LineNumber 
		,  ProductCode
		,  ProductDescription 
		,  AmendedDeliveryQuantity
		,  AmendedShortQuantity 
		,  OriginalShortQuantity 
		, Id
	FROM @LineItems

	SELECT  LineItemId
        , ExceptionTypeId 
        , Quantity 
        , SourceId 
        , ReasonId 
        , ReplanDate 
        , SubmittedDate 
        , ApprovalDate
        , ApprovedBy 
		, lia.Id
		
	FROM LineItemAction lia
	INNER JOIN @LineItems lis ON lis.Id = lia.LineItemId
	WHERE  lia.DateDeleted IS NULL

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
	INNER JOIN @LineItems lis ON lis.id = lia.LineItemId
	WHERE 
		lia.IsDeleted = 0

  RETURN 0
END