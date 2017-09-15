CREATE PROCEDURE [dbo].[Activity_GetSourceById]
	 @activityId INT
AS
BEGIN

	SELECT  
		av.Id AS ActivityId,
		a.Name AS AccountName, 
		j.PHAccount AS AccountNumber, 
		a.Code AS PrimaryAccount, 
		rh.RouteOwnerId AS BranchId, 
		b.Name AS Branch,
		a.Address1 + ' ' +  a.Address2 + ' ' + Postcode AS AccountAddress,
		av.DocumentNumber AS ItemNumber ,  
		CASE WHEN j.COD = 'Cash' OR j.COD = 'Cheque' OR j.COD = 'Card' THEN 1 ELSE 0 END AS Cod,
		CASE WHEN av.ActivityTypeId = dbo.ActivityType_Invoice() THEN 1 ELSE 0 END AS IsInvoice,
		CASE WHEN j.ProofOfDelivery = 1 OR j.ProofOfDelivery = 8 THEN 1 ELSE 0 END AS Pod, -- lucozade = 1 cocacola = 8
		rh.DriverName AS Driver,
		rh.RouteDate AS [Date],
		CASE WHEN j.OuterDiscrepancyFound = 1 THEN (TotalOutersShort - DetailOutersShort) ELSE 0 END AS Tba,
		j.ResolutionStatusId ResolutionStatus,
		s.Location_Id as LocationId
	FROM Activity av 
		INNER JOIN Job j ON j.ActivityId = av.Id
		INNER JOIN Account a on a.LocationId = av.LocationId and a.StopId = j.StopId
		INNER JOIN [Stop] s ON s.Id = j.StopId
		INNER JOIN RouteHeader rh ON rh.Id = s.RouteHeaderId
		INNER JOIN Branch b on b.Id = rh.RouteOwnerId
	WHERE av.Id = @activityId

	 ;WITH LineItemWithActionRequired
	 AS
	 (
		SELECT DISTINCT LineItemId 
		FROM LineItemAction 
		WHERE	
			DeliveryActionId is Null OR DeliveryActionId = 0
		GROUP BY LineItemId
	 )
	SELECT 
		a.Id AS ActivityId,
		li.ProductCode AS Product,
		jt.Description  AS [Type],
		jd.SSCCBarcode AS Barcode,
		li.ProductDescription AS [Description],
		jd.NetPrice AS Value, -- ??
		jd.OriginalDespatchQty AS Expected,
		lie.DamageTotal AS Damaged,
		lie.ShortTotal + lie.BypassTotal AS Shorts,
		CASE WHEN jd.LineDeliveryStatus = 'Delivered' 
			OR LineDeliveryStatus = 'Exception' 
			OR (jd.LineDeliveryStatus = 'Unknown' AND jd.ShortQty > 0) 
			OR (jd.LineDeliveryStatus = 'Unknown' AND lie.DamageTotal = jd.OriginalDespatchQty)
			THEN 1 
			ELSE 0 
			END AS Checked,
		jd.IsHighValue AS HighValue,
		j.StopId,
		s.DropId as Stop,
		s.DeliveryDate AS StopDate,
		j.Id as JobId,
		jt.Code AS JobType,
		jt.Abbreviation AS JobTypeAbbreviation,
		li.Id AS LineItemId,
		j.ResolutionStatusId ResolutionStatus,
		IsNumeric(ar.LineItemId) HasUnresolvedActions
	FROM Activity a
		INNER JOIN Job j ON a.Id = j.ActivityId
		INNER JOIN JobDetail jd on jd.JobId = j.Id
		INNER JOIN LineItem li on li.Id = jd.LineItemId
		INNER JOIN [Stop] s ON s.Id = j.StopId
		INNER JOIN RouteHeader rh ON rh.Id = s.RouteHeaderId	
		INNER JOIN JobType jt on jt.Id = j.JobTypeId
		LEFT JOIN LineItemExceptionsView lie on li.Id = lie.Id
		LEFT JOIN LineItemWithActionRequired ar ON li.Id = ar.LineItemId

	WHERE a.Id = @activityId
		Order By li.LineNumber

	SELECT 
		jobUser.Name
	FROM Activity av 
		INNER JOIN Job j ON j.ActivityId = av.Id
		INNER JOIN UserJob uj ON uj.JobId = j.Id
		INNER JOIN [User] jobUser ON uj.UserId = jobUser.Id
		INNER JOIN [Stop] s ON s.Id = j.StopId
		INNER JOIN RouteHeader rh ON rh.Id = s.RouteHeaderId
	WHERE 
		av.Id = @activityId
END
GO


