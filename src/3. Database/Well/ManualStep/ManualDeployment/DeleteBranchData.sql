	DECLARE @JobIds IntTableType;

	-- Here Put in the Branches you Wish To Keep Depending on The Daatbase
	IF(DB_NAME() != 'Well')
	BEGIN
		INSERT INTO @JobIds 
		SELECT j.Id 
		FROM	Job  j
		INNER JOIN Stop s on j.StopId = s.Id
		INNER JOIN RouteHeader rh on s.RouteHeaderId = s.Id
		WHERE rh.RouteOwnerId NOT IN ( 59,20,55 ) -- Add additional Branches here
	END

	IF(DB_NAME() != 'Well2')
	BEGIN
		INSERT INTO @JobIds 
		SELECT j.Id 
		FROM	Job  j
		INNER JOIN Stop s on j.StopId = s.Id
		INNER JOIN RouteHeader rh on s.RouteHeaderId = s.Id
		WHERE rh.RouteOwnerId NOT IN ( 82,280,507 ) -- Add additional Branches here
	END

	IF(DB_NAME() != 'Well3')
	BEGIN
		INSERT INTO @JobIds 
		SELECT j.Id 
		FROM	Job  j
		INNER JOIN Stop s on j.StopId = s.Id
		INNER JOIN RouteHeader rh on s.RouteHeaderId = s.Id
		WHERE rh.RouteOwnerId NOT IN ( 2,9,42 ) -- Add additional Branches here
	END

	IF(DB_NAME() != 'Well4')
	BEGIN
		INSERT INTO @JobIds 
		SELECT j.Id 
		FROM	Job  j
		INNER JOIN Stop s on j.StopId = s.Id
		INNER JOIN RouteHeader rh on s.RouteHeaderId = s.Id
		WHERE rh.RouteOwnerId NOT IN ( 3,5,33 ) -- Add additional Branches here
	END
	

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
	IF(DB_NAME() != 'Well')
	BEGIN
		TRUNCATE TABLE ExceptionEvent
	END


	-- Maybe Rebuild The Statistics