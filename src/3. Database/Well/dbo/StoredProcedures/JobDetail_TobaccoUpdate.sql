CREATE PROCEDURE [dbo].[JobDetail_TobaccoUpdate]
AS
BEGIN
	DECLARE @TobaccoDeliveryTable TABLE (
		DeliveryLineStatus VARCHAR(20), 
		JobDetailId INT, 
		DeliveredQuantity INT)

	DECLARE @TobaccoShortTable TABLE (
		DeliveryLineStatus VARCHAR(20), 
		JobDetailId INT, 
		ShortQuantity INT)

	-- find all the tobacco lines where the tobacco bag has been updated by TranSend as 'Delivered'
	INSERT INTO @TobaccoDeliveryTable
	SELECT jd1.LineDeliveryStatus , 
			jd2.Id , 
			jd2.OriginalDespatchQty
	FROM JobDetail jd1
	INNER JOIN JobDetail jd2 ON jd1.PHProductCode = jd2.SSCCBarcode
	WHERE jd1.LineDeliveryStatus = 'Delivered'
	AND jd1.PHProductType = 'Tobacco'
	AND jd2.LineDeliveryStatus IS NULL

	-- find all the tobacco lines where the tobacco bag has been updated by TranSend as 'Exception'
	INSERT INTO @TobaccoShortTable 
	SELECT jd1.LineDeliveryStatus, 
			jd2.Id, 
			jd2.OriginalDespatchQty
	FROM JobDetail jd1
	INNER JOIN JobDetail jd2 ON jd1.PHProductCode = jd2.SSCCBarcode
	WHERE jd1.LineDeliveryStatus = 'Exception'
	AND jd1.PHProductType = 'Tobacco'
	AND jd2.LineDeliveryStatus IS NULL

	-- update delivered tobacco lines 
	UPDATE 
		 jdd 
	SET 
		 LineDeliveryStatus = tbd.DeliveryLineStatus,
		 DeliveredQty = tbd.DeliveredQuantity
	FROM 
		 JobDetail jdd 
		 INNER JOIN @TobaccoDeliveryTable tbd 
		 ON jdd.Id = tbd.JobDetailId;

	-- update short tobacco lines 
	UPDATE 
		jds 
	SET 
		LineDeliveryStatus = tst.DeliveryLineStatus,
		ShortQty = tst.ShortQuantity
	FROM 
		JobDetail jds 
		INNER JOIN @TobaccoShortTable tst 
		ON jds.Id = tst.JobDetailId;
END