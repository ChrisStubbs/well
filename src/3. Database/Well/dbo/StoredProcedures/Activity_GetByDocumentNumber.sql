CREATE PROCEDURE [dbo].[Activity_GetByDocumentNumber]
	 @documentNumber VARCHAR(40)
AS
BEGIN

	SELECT  
	av.Id AS ActivityId,
	a.Name, 
	j.PHAccount AS AccountNumber, 
	a.Code AS PrimaryAccount, 
	rh.RouteOwnerId AS BranchId, 
	b.Name AS Branch,
	 a.Address1 AS AccountAddress,
	av.DocumentNumber AS ItemNumber ,  
	CASE WHEN j.COD = 'Cash' OR j.COD = 'Cheque' OR j.COD = 'Card' THEN 1 ELSE 0 END AS Cod,
	CASE WHEN av.ActivityTypeId = dbo.ActivityType_Invoice() THEN 1 ELSE 0 END AS IsInvoice,
	CASE WHEN j.ProofOfDelivery = 4 OR j.ProofOfDelivery = 8 THEN 1 ELSE 0 END AS Pod,
	rh.DriverName AS Driver,
	rh.RouteDate AS DATE,
	CASE WHEN j.OuterDiscrepancyFound = 1 THEN (TotalOutersShort - DetailOutersShort) ELSE 0 END AS Tba
	
	FROM Activity av 
	INNER JOIN Account a on a.LocationId = av.LocationId
	INNER JOIN Job j ON j.ActivityId = av.Id
	INNER JOIN [Stop] s ON s.Id = j.StopId
	INNER JOIN RouteHeader rh ON rh.Id = s.RouteHeaderId
	INNER JOIN Branch b on b.Id = rh.RouteOwnerId
	WHERE av.DocumentNumber = @documentNumber

	DECLARE @ShortsAndDamages TABLE (LineItemId INT, ExceptionTypeId INT, Quantity INT)
	INSERT INTO @ShortsAndDamages (LineItemId, ExceptionTypeId, Quantity)
	SELECT li.Id, lia.ExceptionTypeId, SUM(Quantity)
	FROM LineItemAction lia
	JOIN LineItem li ON lia.LineItemId = li.Id
	GROUP BY li.Id, lia.ExceptionTypeId

	SELECT 
		a.Id AS ActivityId,
		li.ProductCode AS Product,
		jd.PHProductType AS [Type],
		jd.SSCCBarcode AS Barcode,
		li.ProductDescription AS [Description],
		jd.NetPrice AS Value, -- ??
		jd.OriginalDespatchQty AS Expected,
		sd2.Quantity AS Damaged,
		sd.Quantity AS Shorts,
		CASE WHEN jd.LineDeliveryStatus = 'Delivered' OR LineDeliveryStatus = 'Exception' THEN 1 ELSE 0 END AS Checked,
		jd.IsHighValue AS HighValue,
		jrs.[Status] AS Resolution,
		jrs.Id AS ResolutionId,
		j.StopId,
		s.DeliveryDate AS StopDate,
		j.Id as JobId,
		j.JobTypeCode AS JobType,
		jt.Abbreviation AS JobTypeAbbreviation,
		li.Id AS LineItemId
	FROM LineItem li
	INNER JOIN Activity a on li.ActivityId = a.Id
	INNER JOIN Job j ON a.Id = j.ActivityId
	INNER JOIN [Stop] s ON s.Id = j.StopId
	INNER JOIN JobDetail jd on jd.LineItemId = li.Id
	LEFT JOIN LineItemAction lia on lia.LineItemId = li.Id
	INNER JOIN JobType jt on jt.Code = j.JobTypeCode
	INNER JOIN JobResolutionStatus jrs on j.Id = jrs.Job
	LEFT JOIN @ShortsAndDamages sd ON sd.LineItemId = li.Id AND sd.ExceptionTypeId = dbo.ExceptionType_Short()
	LEFT JOIN @ShortsAndDamages sd2 ON sd2.LineItemId = li.Id AND sd2.ExceptionTypeId = dbo.ExceptionType_Damage()
	WHERE a.DocumentNumber = @documentNumber

	RETURN 0
END