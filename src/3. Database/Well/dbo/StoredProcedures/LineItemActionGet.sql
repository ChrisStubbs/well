CREATE PROCEDURE [dbo].[LineItemActionGet]
	@id int 
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
WHERE
	[id] = @Id
	

RETURN 0
