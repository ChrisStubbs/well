CREATE PROCEDURE [dbo].[LineItemByBranchRouteDate]
	@BranchId int = 0,
	@RouteDate Date
AS
	SELECT 
        li.Id, 
        li.LineNumber, 
        li.ProductCode, 
        li.ProductDescription, 
        li.AmendedDeliveryQuantity, 
        li.AmendedShortQuantity, 
        li.OriginalShortQuantity, 
        li.BagId, 
        li.ActivityId, 
        li.CreatedBy, 
        li.CreatedDate, 
        li.LastUpdatedBy, 
        li.LastUpdatedDate, 
        li.DateDeleted, 
        li.DeletedByImport, 
        li.JobId
    FROM 
        LineItem li
        INNER JOIN LineItemAction lia
            on lia.LineItemId = li.id
		INNER JOIN JobDetail jd
            ON li.Id = jd.LineItemId
        INNER JOIN Job j
            ON jd.JobId = j.id
			AND (j.ResolutionStatusId & 256) != 256
        INNER JOIN [Stop] s
            ON j.StopId = s.Id
        INNER JOIN RouteHeader rh
            ON s.RouteHeaderId = rh.Id
            AND rh.RouteOwnerId = @BranchId
            AND CONVERT(Date, rh.RouteDate) = @RouteDate

	SELECT  
		lia.Id, 
		lia.ExceptionTypeId, 
		lia.Quantity, 
		lia.SourceId, 
		lia.ReasonId, 
		lia.ReplanDate, 
		lia.SubmittedDate, 
		lia.ApprovalDate, 
		lia.ApprovedBy, 
		lia.LineItemId, 
		lia.Originator, 
		lia.ActionedBy, 
		lia.DeliveryActionId, 
		lia.PDAReasonDescription, 
		lia.CreatedBy, 
		lia.CreatedDate, 
		lia.LastUpdatedBy, 
		lia.LastUpdatedDate, 
		lia.DateDeleted, 
		lia.DeletedByImport
	FROM 
		LineItemAction lia
		INNER JOIN JobDetail jd
            ON lia.LineItemId = jd.LineItemId
        INNER JOIN Job j
            ON jd.JobId = j.id
			AND (j.ResolutionStatusId & 256) != 256
        INNER JOIN [Stop] s
            ON j.StopId = s.Id
        INNER JOIN RouteHeader rh
            ON s.RouteHeaderId = rh.Id
            AND rh.RouteOwnerId = @BranchId
            AND CONVERT(Date, rh.RouteDate) = @RouteDate
