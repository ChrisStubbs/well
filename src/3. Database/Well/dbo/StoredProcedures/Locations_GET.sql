﻿CREATE PROCEDURE Locations_GET
	@BranchId Int
AS 
	IF OBJECT_ID('tempdb..#LineDetails') IS NOT NULL DROP TABLE #LineDetails

	SELECT 
        li.ActivityId, 
        a.LocationId, 
        li.Id AS LineItemId,
        lia.DeliveryActionId
    INTO #LineDetails
	FROM
		LineItem li
		INNER JOIN LineItemAction lia ON lia.LineItemId = li.Id
	    INNER JOIN Activity a ON a.id = li.ActivityId
	WHERE 
		li.DateDeleted IS NULL
		AND lia.DateDeleted IS NULL
        AND a.DateDeleted IS NULL

    SELECT 
	    l.id
	    ,l.BranchId
	    ,b.Name BranchName
	    ,l.AccountCode AccountNumber
	    ,l.Name AccountName
	    ,l.AddressLine1
	    ,l.AddressLine2
	    ,l.Postcode
	    ,COUNT(a.Id) TotalInvoices
	    ,COUNT(invoicedJobs.ActivityId) Invoiced
	    ,COUNT(liwa.ActivityId) Exceptions
        ,CONVERT(Bit, MAX(ISNULL(WithUnresolvedAction.HasNotDefinedDeliveryAction, 0))) AS HasNotDefinedDeliveryAction
        ,CONVERT(Bit, MAX(ISNULL(WithNoGRNV.NoGRNButNeeds, 0))) AS NoGRNButNeeds
        ,CONVERT(Bit, MAX(ISNULL(PendingSubmitions.PendingSubmission, 0))) AS PendingSubmission
    FROM 
		Location l
	    JOIN Activity a 
            on a.LocationId = l.Id
	    LEFT JOIN 
	    (
            SELECT DISTINCT ld.ActivityId
	        FROM
		        #LineDetails ld
		        INNER JOIN JobDetail jd on ld.LineItemId = jd.LineItemId
		        INNER JOIN Job j on j.Id = jd.JobId
	        WHERE 
		        j.ResolutionStatusId > 1
				AND jd.DateDeleted IS NULL
				AND j.DateDeleted IS NULL
	        GROUP BY ld.ActivityId
        ) liwa 
            on liwa.ActivityId = a.Id
	    LEFT JOIN 
	    (
            SELECT DISTINCT j.ActivityId
	        FROM Job j
	        WHERE j.ResolutionStatusId > 1 
				AND j.DateDeleted IS NULL
	        GROUP BY j.ActivityId
        ) invoicedJobs 
            on invoicedJobs.ActivityId = a.Id
	    LEFT JOIN Branch b 
            ON b.Id = l.BranchId
        LEFT JOIN 
        (
            SELECT LocationId, HasNotDefinedDeliveryAction ^ 1 AS HasNotDefinedDeliveryAction
            FROM 
            (
                SELECT ld.LocationId, CONVERT(Bit, MIN(ISNULL(ld.DeliveryActionId, 0))) AS HasNotDefinedDeliveryAction
                FROM #LineDetails ld
                GROUP BY ld.LocationId
            ) Data
        ) WithUnresolvedAction  
            ON WithUnresolvedAction.LocationId = l.Id
        LEFT JOIN 
        (
            SELECT 
                a.LocationId, 
                MAX(CASE 
                        WHEN j.GrnProcessType = 1 AND j.GrnNumber IS NULL THEN 1 
                        ELSE 0 
                    END) AS NoGRNButNeeds
            FROM 
                Job j
                INNER JOIN JobDetail jd ON j.Id = jd.JobId
                INNER JOIN LineItem li on li.id = jd.LineItemId
		        INNER JOIN Activity a on a.id = li.ActivityId
	        WHERE 
                j.DateDeleted IS NULL
                AND li.DateDeleted IS NULL
                AND a.DateDeleted IS NULL
            GROUP BY a.LocationId
        ) WithNoGRNV
            ON l.id = WithNoGRNV.LocationId
        LEFT JOIN 
        (
            SELECT 
                ld.LocationId, 
                MAX(CASE 
                        WHEN r.Description IN ('Pending Submission', 'Pending Approval') THEN 1
                        ELSE 0
                    END) PendingSubmission
            FROM    
                Job j
                INNER JOIN JobDetail jd ON j.Id = jd.JobId
                INNER JOIN 
                (
                    SELECT ld.LineItemId, ld.LocationId
                    FROM #LineDetails ld
                    GROUP BY ld.LineItemId, ld.LocationId
                ) ld
                    ON jd.LineItemId = ld.LineItemId
                INNER JOIN ResolutionStatus r ON j.ResolutionStatusId = r.Id 
            WHERE
                j.DateDeleted IS NULL
            GROUP BY 
                ld.LocationId
        ) PendingSubmitions
            ON PendingSubmitions.LocationId = l.Id
    WHERE
	    (a.ActivityTypeId < 3)
		AND a.DateDeleted IS NULL
		AND l.BranchId = @BranchId
    GROUP BY 
	    l.Id
	    ,l.BranchId
	    ,l.AccountCode
	    ,l.AddressLine1
	    ,l.AddressLine2
	    ,l.Postcode
	    ,l.Name
	    ,b.Name

	IF OBJECT_ID('tempdb..#LineDetails') IS NOT NULL DROP TABLE #LineDetails