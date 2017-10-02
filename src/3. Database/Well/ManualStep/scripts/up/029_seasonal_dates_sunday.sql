DECLARE @tmp table
(
    Id Int
)

DECLARE @d datetime
SELECT @d = '20171002'  

INSERT INTO SeasonalDate
    ([Description], [From], [To], CreatedBy, CreatedDate, LastUpdatedBy, LastUpdatedDate)
    OUTPUT inserted.Id INTO @tmp
SELECT 
    'Sunday - ' + CONVERT(VarChar(6),  DATEADD(dd, number, @d)) AS [Description], 
    DATEADD(dd, number, @d) AS [From], 
    DATEADD(dd, number, @d) AS [To],
    'Deployment' AS CreatedBy,
    GetDate() AS CreatedDate,
    'Deployment' AS UpdatedBy,
    GetDate() AS DateUpdated
FROM 
(
    SELECT TOP 100 PERCENT
        ones.n + 10 * tens.n + 100 * hundreds.n + thousands.n * 1000 AS Number
    FROM (VALUES(0),(1),(2),(3),(4),(5),(6),(7),(8),(9)) ones(n),
         (VALUES(0),(1),(2),(3),(4),(5),(6),(7),(8),(9)) tens(n),
         (VALUES(0),(1),(2),(3),(4),(5),(6),(7),(8),(9)) hundreds(n),
         (VALUES(0),(1),(2),(3),(4),(5),(6),(7),(8),(9)) thousands(n)
    ORDER BY Number
) Days
WHERE
    DATEPART(dw, DATEADD(dd, number, @d)) = 7

INSERT INTO SeasonalDateToBranch
    (BranchId, SeasonalDateId)
SELECT 
    b.Id, ids.Id
FROM 
    Branch b
    CROSS JOIN @tmp ids
