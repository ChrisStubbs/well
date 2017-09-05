CREATE PROCEDURE ActivityDetails
	@Id INT
AS
	SELECT 
		a.ActivityId,
		a.Barcode,
		a.BypassTotal,
		a.DeliveryDate,
		a.Description,
		a.DropId,
		a.Expected,
		a.IsHighValue,
		a.JobId,
		a.JobStatusId,
		a.JobType,
		a.LineDeliveryStatus,
		a.LineItemId,
		a.OriginalDespatchQty,
		a.Product,
		a.ResolutionStatus,
		a.ShortTotal,
		a.StopId,
		a.Value
	FROM 
		ActivityDetailsView a
	WHERE 
		a.ActivityId = @Id