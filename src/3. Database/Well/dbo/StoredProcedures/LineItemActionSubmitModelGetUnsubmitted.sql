CREATE PROCEDURE [dbo].[LineItemActionSubmitModelGetUnsubmitted]
	@DeliveryActionId AS INT
AS
Select 
	lia.Id
	,j.Id as JobId
	,s.dropId as Stop
	,j.InvoiceNumber
	,li.ProductCode
	,jd.NetPrice
	,rh.RouteOwnerId as BranchId
	,lia.LineItemId
	,lia.ExceptionTypeId as ExceptionType
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
	,lia.CreatedDate
	,lia.CreatedBy
	,lia.LastUpdatedDate
	,lia.LastUpdatedDate
From  
	LineItemAction lia
Inner Join 
	LineItem li on li.Id = lia.LineItemId
Inner Join
	JobDetail jd on jd.LineItemId = li.Id
Inner Join 
	Job j on j.Id = jd.JobId
Inner Join 
	Stop s on s.Id = j.StopId
Inner Join 
	RouteHeader rh on rh.Id = s.RouteHeaderId
WHERE 
	lia.DeliveryActionId = @DeliveryActionId
	AND
	lia.SubmittedDate IS NULL
	AND
	lia.DateDeleted IS NULL
