CREATE VIEW ActivityDetailsView
AS 
    WITH Actions(LineItemId, BypassTotal, DamageTotal, ShortTotal)
    AS
    (
        SELECT 
            LineItemId, 
            SUM(CASE 
                WHEN ExceptionTypeId = 2 THEN Quantity --ExceptionType.Bypass
                ELSE 0
            END) AS BypassTotal,
            SUM(CASE 
                WHEN ExceptionTypeId = 3 THEN Quantity --ExceptionType.Damage
                ELSE 0
            END) AS DamageTotal,
            SUM(CASE 
                WHEN ExceptionTypeId = 1 THEN Quantity --ExceptionType.Short
                ELSE 0
            END) AS ShortTotal
        FROM 
            LineItemAction l
        WHERE 
            DateDeleted IS NULL
        GROUP BY 
            LineItemId
    )
    SELECT 
        a.Id AS ActivityId,
        li.ProductCode AS Product,
        j.JobTypeCode AS JobType,
        jd.SSCCBarcode AS Barcode,
        li.ProductDescription AS Description,
        jd.NetPrice AS Value,
        jd.OriginalDespatchQty AS Expected,
        jd.LineDeliveryStatus AS LineDeliveryStatus,
        jd.IsHighValue,
        s.id AS StopId,
        s.DropId,
        s.DeliveryDate,
        j.Id AS JobId,
        li.id as LineItemId,
        j.ResolutionStatusId AS ResolutionStatus,
        jd.OriginalDespatchQty,
        j.JobStatusId,
        ac.BypassTotal,
        ac.DamageTotal,
        ac.ShortTotal,
		jd.UpliftAction_Id
    FROM 
        Activity a
        INNER JOIN Job j
            ON a.Id = j.ActivityId
        INNER JOIN JobDetail jd
            ON jd.JobId = j.Id
        LEFT JOIN LineItem li
            ON jd.LineItemId = li.Id
        INNER JOIN [Stop] s
            ON j.StopId = s.id
        LEFT JOIN Actions ac
            ON ac.LineItemId = li.Id
    WHERE
        a.DateDeleted IS NULL
        AND j.DateDeleted IS NULL
        AND jd.DateDeleted IS NULL
        AND li.DateDeleted IS NULL
        AND s.DateDeleted IS NULL
        AND jd.PHProductCode NOT IN 
        (
            SELECT barCode FROM Bag
        )