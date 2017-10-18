IF (@@DATEFIRST = 7)
BEGIN
    -- this script should only be run when the first day of the week is Sunday
    -- the data was generated under that condition
    UPDATE SeasonalDate
        SET [From] = DATEADD(Day, 1, [From]),
        [To] = DATEADD(Day, 1, [To])
    WHERE
        DATEPART(dw, [To]) = 7  AND [Description] Like 'Week - %'
END