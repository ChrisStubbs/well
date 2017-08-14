CREATE PROCEDURE [dbo].[LineItem_InsertUpdate]
	@JobIds dbo.IntTableType READONLY
AS
BEGIN
	DECLARE @process VARCHAR(20) = 'WellUpdate'

	DECLARE @JobDetails TABLE(
		JobDetailId INT
		, LineNumber INT
		, ProductCode VARCHAR(60)
		, ProductDescription VARCHAR(100)
		, ActivityId INT)

	-- find all the job details with no LINEITEM id that are no tobacco bags
	INSERT INTO @JobDetails(JobDetailId, LineNumber, ProductCode, ProductDescription, ActivityId)
	SELECT	jd.Id, jd.LineNumber, jd.PHProductCode, jd.ProdDesc, j.ActivityId 
	FROM	JobDetail jd
	INNER JOIN Job j ON j.Id = jd.JobId
	INNER JOIN @JobIds jobIds ON jobIds.Value = j.Id
	WHERE LineItemId IS NULL
	AND ProdDesc NOT LIKE '%Tobacco bag%' 
	AND j.JobTypeCode NOT IN ('DEL-DOC', 'UPL-SAN', 'UPL-ASS')  -- no need to create line item or actions for these job types

	-- Update the Line Item table 
	MERGE INTO LineItem AS Target
	USING (	SELECT LineNumber,ProductCode, ProductDescription, ActivityId 
			FROM @JobDetails) AS Source
	(LineNumber, ProductCode, ProductDescription, ActivityId)
	ON Target.ActivityId = Source.ActivityId AND Target.ProductCode = Source.ProductCode AND Target.LineNumber = Source.LineNumber
	WHEN MATCHED THEN
	UPDATE SET ProductDescription = Source.ProductDescription, LastUpdatedBy = @process, LastUpdatedDate =  GETDATE()
	WHEN NOT MATCHED BY TARGET THEN
	INSERT (LineNumber, ProductCode, ProductDescription, ActivityId, CreatedBy, CreatedDate, LastUpdatedBy, LastUpdatedDate) 
	VALUES (LineNumber, ProductCode, ProductDescription, ActivityId,  @process, GETDATE(), @process, GETDATE())
	;
	
	-- find the lineitem ids for each jobdetail
	DECLARE @JobDetailsToUpdate TABLE
	(  JobDetailId INT,
		LineItemId INT	
	)
	INSERT INTO @JobDetailsToUpdate
	SELECT jd.JobDetailId, li.Id
	FROM  LineItem li
	INNER JOIN @JobDetails jd ON li.ActivityId = jd.ActivityId AND li.LineNumber = jd.LineNumber

	-- update JOB with the Activity Id
	UPDATE jd
	SET LineItemId = jtu.LineItemId
	FROM JobDetail jd
	INNER JOIN @JobDetailsToUpdate jtu ON jd.Id = jtu.JobDetailId

END


