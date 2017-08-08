CREATE PROCEDURE [dbo].[Bag_InsertUpdate]
	@JobIds dbo.IntTableType READONLY
AS
BEGIN
	DECLARE @process VARCHAR(20) = 'WellUpdate'

	DECLARE @BagDetails TABLE(
		JobDetailId INT
		, Barcode VARCHAR(60)
		, [Description] VARCHAR(100)
		, LineItemId INT)

		-- find all the tobacco bags in job details with no LINEITEM id
	INSERT INTO @BagDetails(JobDetailId, Barcode, [Description])
	SELECT jd.Id, jd.PHProductCode, jd.ProdDesc
	FROM JobDetail jd
	INNER JOIN Job j ON j.Id = jd.JobId
	INNER JOIN @JobIds jobIds ON jobIds.Value = j.Id
	WHERE jd.LineItemId IS NULL
	AND jd.ProdDesc LIKE 'Tobacco bag%' 

	-- Update the Bag table 
	MERGE INTO Bag AS Target
	USING (SELECT  Barcode, [Description] 
		   FROM @BagDetails) AS Source
	(  Barcode, [Description])
	ON Target.Barcode = Source.Barcode AND Target.[Description] = Source.[Description]
	WHEN NOT MATCHED BY TARGET THEN
	INSERT (Barcode, [Description], CreatedBy, CreatedDate, LastUpdatedBy, LastUpdatedDate) 
	VALUES (Barcode, [Description],  @process, GETDATE(), @process, GETDATE())
	;
	
	-- find the lineitem ids for each bag
	DECLARE @BagDetailsToUpdate TABLE
	(  BagId INT,
		LineItemId INT	
	)
	INSERT INTO @BagDetailsToUpdate
	SELECT b.Id, jd.LineItemId
	FROM Bag b
	INNER JOIN JobDetail jd ON jd.SSCCBarcode = b.Barcode 
	INNER JOIN @JobIds jobIds ON jobIds.Value = jd.JobId

	-- update LineItem with the Bag Id
	UPDATE li
	SET BagId = bu.BagId
	FROM LineItem li
	INNER JOIN @BagDetailsToUpdate bu ON li.Id = bu.LineItemId

	-- find the jobdetail id for each bag
	DECLARE @JobDetailForBag TABLE
	(  BagId INT,
		JobDetailId INT
	)
	INSERT INTO @JobDetailForBag
	SELECT b.Id, jd.Id
	FROM Bag b
	INNER JOIN JobDetail jd ON jd.PHProductCode = b.Barcode 
	INNER JOIN @JobIds jobIds ON jobIds.Value = jd.JobId

	-- update the JobDetail table with the bag ids
	UPDATE jd
	SET BagId = jb.BagId
	FROM JobDetail jd
	INNER JOIN @JobDetailForBag jb ON jd.Id = jb.JobDetailId -- id of the bag job detail

END
