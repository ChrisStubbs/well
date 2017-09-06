
CREATE PROCEDURE ActivityDetails
	@Id INT
AS
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
		CONVERT(Bit, LineWithProblems.TotalNotDefined) AS HasNoDefinedActions
	FROM 
		ActivityDetailsView a
				LEFT JOIN 
		(
			SELECT Li.Id, COUNT(la.Id) AS TotalNotDefined
			FROM LineItem Li INNER JOIN  LineItemAction la ON li.Id = la.LineItemId
			WHERE la.DeliveryActionId = 0 --NotDefined
			GROUP BY Li.Id
		) LineWithProblems 
			ON LineWithProblems.Id = a.LineItemId
	WHERE 
		a.ActivityId = @Id