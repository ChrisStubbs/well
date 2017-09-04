CREATE VIEW ActivityDetailsView
AS 
    WITH Actions(LineItemId, ExceptionTypeId, Quantity)
    AS
    (
        SELECT LineItemId, ExceptionTypeId, SUM(Quantity) AS Quantity
        FROM LineItemAction l
        WHERE DateDeleted IS NULL
        GROUP BY LineItemId, l.ExceptionTypeId
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
        CASE 
            WHEN ac.ExceptionTypeId = 2 THEN ac.Quantity --ExceptionType.Bypass
            ELSE null
        END AS BypassTotal,
        CASE 
            WHEN ac.ExceptionTypeId = 3 THEN ac.Quantity --ExceptionType.Damage
            ELSE null
        END AS DamageTotal,
        CASE 
            WHEN ac.ExceptionTypeId = 1 THEN ac.Quantity --ExceptionType.Short
            ELSE null
        END AS ShortTotal
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
        AND a.id = 1