
CREATE PROCEDURE ActivityDetails
	@Id INT
AS
	;WITH LineWithProblems AS
	(
		SELECT Li.Id, la.Id LineItemAction, la.DeliveryActionId
		FROM LineItem Li INNER JOIN  LineItemAction la ON li.Id = la.LineItemId
		WHERE li.DateDeleted IS NULL AND la.DateDeleted IS NULL 
	)
	SELECT 
		a.ActivityId,
		a.Barcode,
		a.BypassTotal AS Bypass,
		a.DeliveryDate AS StopDate,
		a.Description,
		a.DropId AS [Stop],
		a.Expected,
		a.IsHighValue AS HighValue,
		a.JobId,
		a.JobStatusId,
		a.JobType,
		a.LineDeliveryStatus,
		a.LineItemId,
		a.OriginalDespatchQty,
		a.Product,
		a.ResolutionStatus,
		a.ShortTotal AS Shorts,
		a.StopId,
		a.Value,
		a.DamageTotal AS Damaged,
		CONVERT(Bit, (
			SELECT COUNT(LineItemAction)
			FROM LineWithProblems la
			WHERE la.Id = a.LineItemId AND (la.DeliveryActionId = 0 OR la.DeliveryActionId IS NULL)--NotDefined
		)) AS HasNoDefinedActions,
		CONVERT(Bit, (
			SELECT COUNT(LineItemAction)
			FROM LineWithProblems la
			WHERE la.Id = a.LineItemId 
		)) AS HasLineItemActions
	FROM 
		ActivityDetailsView a
	WHERE 
		a.ActivityId = @Id