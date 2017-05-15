CREATE PROCEDURE [dbo].[LineItem_GetByIds]
	@Ids dbo.IntTableType	READONLY
AS
 BEGIN
	SELECT   LineNumber 
		,  ProductCode
		,  ProductDescription 
		,  AmendedDeliveryQuantity
		,  AmendedShortQuantity 
		,  OriginalShortQuantity 
		, Id
	FROM LineItem li
	INNER JOIN @Ids ids ON ids.Value = li.Id

	SELECT Id
		,LineItemId
		,[ExceptionTypeId] 
		,[Quantity] 
		,[SourceId]
		,[ReasonId]
		,[ReplanDate]
		,[SubmittedDate]
		,[ApprovalDate]
		,[ApprovedBy] 
	FROM LineItemAction lia
	INNER JOIN @Ids ids ON ids.Value = lia.LineItemId

 RETURN 0
END