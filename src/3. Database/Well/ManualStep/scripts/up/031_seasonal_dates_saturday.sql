UPDATE SeasonalDate
    SET [From] = Data.[From],
    Description = Data.Description,
    LastUpdatedBy = 'Deployment',
    LastUpdatedDate =  GETDATE()
FROM 
(
    SELECT 
        id, 
        DateAdd(Day, -1, [FROM]) AS [From], 
        'Week - ' + CONVERT(VarChar, DATEPART(WK, [To])) AS Description
    FROM 
        SeasonalDate
    WHERE 
         DATEPART(dw, [To]) = 7 
) Data
WHERE
    SeasonalDate.id = data.Id