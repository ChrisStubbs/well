CREATE VIEW NonSoftDeletedRoutesJobsView
AS 
    SELECT 
        rh.RouteDate,
        j.id AS JobId,
        s.Id AS StopId,
        rh.Id AS RouteId,
        j.ResolutionStatusId AS ResolutionStatus,
        rh.RouteOwnerId AS BranchId,
        j.RoyaltyCode
    FROM 
        RouteHeader rh
        inner join Stop s ON rh.id = s.RouteHeaderId
        inner join Job j ON s.id = j.StopId
    WHERE
        rh.DateDeleted IS NULL
        AND j.ResolutionStatusId IN 
        (
            SELECT Id
            FROM ResolutionStatus
            WHERE 
                Id = 1 --'Imported'
                OR Id = 2 --'Driver Completed'
                OR Id = 512 --'Manually Completed'
                OR 256 & Id != 0 --all closed status
        )