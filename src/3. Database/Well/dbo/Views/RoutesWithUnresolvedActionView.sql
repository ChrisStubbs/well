CREATE VIEW RoutesWithUnresolvedActionView
AS
    SELECT 
        Id, 
       CONVERT(Bit,  HasNotDefinedDeliveryAction ^ 1) AS HasNotDefinedDeliveryAction,
	   BranchId
    FROM 
    (
        SELECT 
            rh.Id, 
            CONVERT(Bit, MIN(ISNULL(lia.DeliveryActionId, 0))) AS HasNotDefinedDeliveryAction,
			MAX(rh.RouteOwnerId) AS BranchId
        FROM 
            LineItem li 
            INNER JOIN LineItemAction lia ON li.Id = lia.LineItemId
            INNER JOIN Job j ON li.JobId = j.id
            INNER JOIN [Stop] s ON j.StopId = s.id
			INNER JOIN RouteHeader rh on s.RouteHeaderId = rh.Id
        WHERE 
            li.DateDeleted IS NULL
            AND lia.DateDeleted IS NULL
            AND j.DateDeleted IS NULL
            AND s.DateDeleted IS NULL
            AND rh.DateDeleted IS NULL
        GROUP BY 
            rh.Id
    ) Data