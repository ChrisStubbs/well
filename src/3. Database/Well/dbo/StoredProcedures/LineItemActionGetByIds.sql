CREATE PROCEDURE [dbo].[LineItemActionGetByIds]
	@Ids dbo.IntTableType	READONLY
AS
SELECT
	[Id]
	,[ExceptionTypeId] as EntityType
	,[Quantity]
	,[SourceId] as Source
	,[ReasonId] as Reason
	,[ReplanDate]
	,[SubmittedDate]
	,[ApprovalDate]
	,[ApprovedBy]
	,[LineItemId]
	,[Originator]
	,[ActionedBy]
	,[CreatedBy]
	,[CreatedDate] as DateCreated
	,[LastUpdatedBy] as UpdatedBy
	,[LastUpdatedDate] as DateUpdated
	,[Version]
FROM 
	[dbo].[LineItemAction]
INNER JOIN @Ids ids ON ids.Value = Id
	

RETURN 0
