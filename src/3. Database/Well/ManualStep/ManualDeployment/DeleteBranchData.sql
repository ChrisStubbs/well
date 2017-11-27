	/*
		Delete all data for 
		branches that are no longer needed in the database
	*/
BEGIN TRAN --NOTE ******When you are happy you need to commit the transaction at the bottom!!******
	
	DECLARE @JobIds IntTableType;

	DECLARE @DatabaseSplit TABLE(DatabaseName VARCHAR(50),BranchId INT)
	
	INSERT INTO  @DatabaseSplit
	SELECT		 'Well',59		-- Bristol
	UNION SELECT 'Well',20		-- Hemel
	UNION SELECT 'Well',55		-- Plymouth
	
	UNION SELECT 'Well2',82		-- Haydock
	UNION SELECT 'Well2',22		-- Birtley
	UNION SELECT 'Well2',14		-- Leeds

	UNION SELECT 'Well3',2		-- Medway
	UNION SELECT 'Well3',9		-- Dunfermlin
	UNION SELECT 'Well3',42		-- Brandon 
	
	UNION SELECT 'Well4',3		-- Coventry
	UNION SELECT 'Well4',5		-- Fareham
	UNION SELECT 'Well4',33		-- Belfast 

	-- Select all the JobIds 
	-- for branches that a
	-- not in the current database
	INSERT INTO @JobIds 
	SELECT j.Id 
	FROM	Job  j
	INNER JOIN Stop s on j.StopId = s.Id
	INNER JOIN RouteHeader rh on s.RouteHeaderId = rh.Id
	INNER JOIN @DatabaseSplit db on rh.RouteOwnerId = db.BranchId
	WHERE
		db.DatabaseName != DB_NAME()
	
	--LineItemActionComment
	DELETE c
	FROM dbo.LineItemActionComment c
	INNER JOIN LineItemAction lia ON lia.Id = c.LineItemActionId
	INNER JOIN LineItem li ON li.Id = lia.LineItemId
	INNER JOIN @JobIds jobIds ON li.JobId = jobIds.Value
	PRINT ('Deleted LineItemActionComment')
	PRINT ('----------------')

	--LineItemAction
	DELETE lia
	FROM dbo.LineItemAction lia
	INNER JOIN LineItem li ON li.Id = lia.LineItemId
	INNER JOIN @JobIds jobIds ON li.JobId = jobIds.Value
	PRINT ('Deleted LineItemAction')
	PRINT ('----------------')

	--JobDetailDamage
	DELETE jdd
	FROM dbo.JobDetailDamage jdd
	INNER JOIN JobDetail Jd ON Jd.Id = jdd.JobDetailId 
	INNER JOIN @JobIds jobIds ON jd.JobId = jobIds.Value
	PRINT ('Deleted JobDetailDamage')
	PRINT ('----------------')

	SELECT jd.BagId 
	INTO #Bag 
	FROM dbo.JobDetail jd
	INNER JOIN LineItem li ON li.id = jd.LineItemId
	INNER JOIN @JobIds jobIds ON li.JobId = jobIds.Value

	--JobDetail
	DELETE jd
	FROM dbo.JobDetail jd
	INNER JOIN @JobIds jobIds ON jd.JobId = jobIds.Value
	PRINT ('Deleted JobDetail')
	PRINT ('----------------')

	--JobDetail
	DELETE jd
	FROM dbo.JobDetail jd
	INNER JOIN LineItem li ON li.id = jd.LineItemId
	INNER JOIN @JobIds jobIds ON li.JobId = jobIds.Value
	PRINT ('Deleted JobDetail')
	PRINT ('----------------')
	
	--LineItem
	DELETE	li
	FROM dbo.LineItem li
	INNER JOIN @JobIds jobIds ON li.JobId = jobIds.Value
	PRINT ('Deleted LineItem')
	PRINT ('----------------')

	--Bag
	DELETE b
	FROM dbo.Bag b
	INNER JOIN #Bag bid on bid.BagId = b.Id
	PRINT ('Deleted Bag')
	PRINT ('----------------')

	DROP Table #Bag

	--JobResolutionStatus
	DELETE jrs
	FROM dbo.JobResolutionStatus jrs
	INNER JOIN @JobIds jobIds ON jrs.Job = jobIds.Value
	PRINT ('Deleted JobResolutionStatus')
	PRINT ('----------------')

	--UserJob
	DELETE uj
	FROM dbo.UserJob uj
	INNER JOIN @JobIds jobIds ON uj.JobId = jobIds.Value
	PRINT ('Deleted UserJob')
	PRINT ('----------------')

	--Job
	DELETE j
	FROM dbo.Job j
	INNER JOIN @JobIds jobIds ON j.Id = jobIds.Value
	PRINT ('Job')
	PRINT ('----------------')

	-- Account
	DELETE acs
	FROM dbo.Account acs
	LEFT JOIN Stop s on acs.StopId = s.Id
	LEFT JOIN Job j on j.StopId = s.Id
	WHERE j.Id Is Null
	PRINT ('Deleted Accounts')
	PRINT ('----------------')

	-- Stop
	DELETE s
	FROM Stop s
	LEFT JOIN Job j on j.StopId = s.Id
	WHERE j.Id Is Null
	PRINT ('Deleted Stops')
	PRINT ('----------------')

	-- Routes Header
	DELETE rh
	FROM RouteHeader rh
	LEFT JOIN dbo.Stop s ON s.RouteHeaderId = rh.Id
	WHERE s.Id Is Null
	PRINT ('Deleted RouteHeader')
	PRINT ('----------------')
	
	-- Route
	DELETE r
	FROM dbo.[Routes] r
	LEFT JOIN RouteHeader rh on rh.RoutesId = r.Id
	WHERE 
		rh.Id IS NULL
	
	-- Activity
	DELETE a
	FROM dbo.Activity a
	LEFT JOIN Job j on j.ActivityId = a.Id
	WHERE
		j.Id IS NULL

	PRINT ('Deleted Activity')
	PRINT ('----------------')
	
	-- Clear Down The Exception Event Table
	Delete From dbo.ExceptionEvent Where SourceId Is Null AND DB_NAME() != 'Well'
	

	-- delete all the exceptionEvents that do not belong to this database
	DELETE	ex 
	FROM	dbo.ExceptionEvent  ex
	INNER JOIN @JobIds jobIds ON ex.SourceId = jobIds.Value
		
	
	-- Maybe Rebuild The Statistics

--ROLLBACK
--COMMIT