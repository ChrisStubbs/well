CREATE PROCEDURE [dbo].[LineItemAction_GetByLineItemId]
	@Id Int
AS
 BEGIN
	SELECT   LineItemId
        , ExceptionTypeId 
        , Quantity 
        , SourceId 
        , ReasonId 
        , ReplanDate 
        , SubmittedDate 
        , ApprovalDate
        , ApprovedBy 
	FROM LineItemAction lia
	WHERE LineItemId = @Id

 RETURN 0
END