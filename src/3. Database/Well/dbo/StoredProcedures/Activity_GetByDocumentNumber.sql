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
	CASE WHEN j.COD = 'SCOD' OR j.COD = 'CCOD' THEN 1 ELSE 0 END AS Cod,
	CASE WHEN av.ActivityTypeId = dbo.ActivityType_Invoice() THEN 1 ELSE 0 END AS IsInvoice,
	CASE WHEN j.ProofOfDelivery = 4 OR j.ProofOfDelivery = 8 THEN 1 ELSE 0 END AS Pod,
	rh.DriverName AS Driver,
	rh.RouteDate AS DATE
	--tba
	
	FROM Activity av 
	INNER JOIN Account a on a.LocationId = av.LocationId
	INNER JOIN Job j ON j.ActivityId = av.Id
	INNER JOIN [Stop] s ON s.Id = j.StopId
	INNER JOIN RouteHeader rh ON rh.Id = s.RouteHeaderId
	INNER JOIN Branch b on b.Id = rh.RouteOwnerId
	WHERE av.DocumentNumber = @documentNumber

	SELECT 
	a.Id AS ActivityId,
	li.ProductCode AS Product,
		jd.PHProductType AS [Type],
		jd.SSCCBarcode AS Barcode,
		li.ProductDescription AS [Description],
		jd.NetPrice AS Value, -- ??
		jd.OriginalDespatchQty AS Expected,
	-- damage 
	-- short
		CASE WHEN jd.LineDeliveryStatus = 'Delivered' OR LineDeliveryStatus = 'Exception' THEN 1 ELSE 0 END AS Checked,
		jd.IsHighValue AS HighValue,
	-- resolution
	-- resolutionIfd
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
	WHERE a.DocumentNumber = @documentNumber


	RETURN 0
END