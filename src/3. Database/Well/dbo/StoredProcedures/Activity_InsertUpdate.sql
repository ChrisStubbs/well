CREATE PROCEDURE [dbo].[Activity_InsertUpdate]
AS
BEGIN
	DECLARE @process VARCHAR(20) = 'WellUpdate'

	DECLARE @jobs TABLE (
		DocumentNumber VARCHAR(40)
		,InitialDocument VARCHAR(40)
		,ActivityType INT
		,routeownerId INT
		,AccountCode VARCHAR(20)
		,PickListRef VARCHAR(40)
		,DeliveryDate DATETIME
		,LocationId INT
		,JobId INT
		,ActivityId INT)

	DECLARE @jobsToActivity TABLE (
		DocumentNumber VARCHAR(40)
		,InitialDocument VARCHAR(40)
		,ActivityType INT
		,LocationId INT
		,JobCount INT
		,ActivityId INt)

	-- Select the jobs where there is no ACTIVITY (new jobs), OR where the ACTIVITY has no invoice number, but the JOB does.
	-- this selects all the jobs that either need inserting or updating
	INSERT INTO @Jobs(DocumentNumber, InitialDocument, ActivityType, RouteOwnerId, AccountCode, DeliveryDate, LocationId, JobId, ActivityId)
	SELECT j.InvoiceNumber,j.PickListRef, jt.ActivityTypeId , rh.routeownerId,  j.PHAccount, s.DeliveryDate, s.Location_Id, j.id, a.Id FROM Job j
	INNER JOIN [Stop] s ON s.Id = j.Stopid
	INNER JOIN RouteHeader rh ON rh.Id = s.RouteHeaderId
	INNER JOIN JobType jt ON j.JobTypeCode = jt.Code
	LEFT JOIN Activity a on j.ActivityId = a.Id
	WHERE a.Id IS NULL 
	OR (a.DocumentNumber IS NULL AND j.InvoiceNumber IS NOT NULL) 

	--Group the jobs together by the INVOICENUMBER , and ACTIVITYTYPE and LOCATION. These are activities.
	--Note that UPLIFT may have the original INVOICE number, and the same LOCATION as the original invoice, so ACTIVITYTYPE is required
	--for grouping
	INSERT INTO @jobsToActivity(DocumentNumber, InitialDocument, ActivityType, LocationId, JobCount, ActivityId)
	SELECT DocumentNumber,InitialDocument, ActivityType, LocationId ,COUNT(1), ActivityId FROM @jobs
	GROUP BY  DocumentNumber,InitialDocument, ActivityType, LocationId, ActivityId
	
	BEGIN TRAN

	    -- Update the Activity table 
		MERGE INTO Activity AS Target
		USING (SELECT  DocumentNumber ,InitialDocument, ActivityType, LocationId , ActivityId FROM @JobsToActivity) AS Source
		( DocumentNumber ,InitialDocument, ActivityType, LocationId, ActivityId )
		ON Target.Id = Source.ActivityId
		WHEN MATCHED THEN
		UPDATE SET DocumentNumber = Source.DocumentNumber, InitialDocument = Source.Initialdocument, LastUpdatedBy = @process, LastUpdatedDate =  GETDATE()
		WHEN NOT MATCHED BY TARGET THEN
		INSERT (DocumentNumber ,InitialDocument, ActivityTypeId, LocationId, CreatedBy, CreatedDate, LastUpdatedBy, LastUpdatedDate) 
		VALUES (DocumentNumber,InitialDocument, ActivityType, LocationId,  @process, GETDATE(), @process, GETDATE())
		;
	
		-- find the job ids for each activity using the activity type, the document number and the location
		DECLARE @JobsToUpdate TABLE
		(  JobId INT,
		   ActivityId INT	
		)
		INSERT INTO @JobsToUpdate
		SELECT j.jobId, a.Id
		FROM  Activity a
		INNER JOIN @jobs j ON a.ActivityTypeId = j.ActivityType AND a.InitialDocument = j.InitialDocument AND a.LocationId = j.LocationId  -- original selection of jobs
		LEFT JOIN Job j2 on j2.Id = j.JobId                                                                                                -- join on Job to find the ones that do not have the activity id set
		WHERE j2.ActivityId IS NULL

		-- update JOB with the Activity Id
		UPDATE j
		SET ActivityId = jtu.ActivityId
		FROM Job j
		INNER JOIN @JobsToUpdate jtu ON j.Id = jtu.JobId
	
	
	COMMIT

	RETURN 0
END

