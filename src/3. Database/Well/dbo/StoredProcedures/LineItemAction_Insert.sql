CREATE PROCEDURE [dbo].[LineItemAction_Insert]
	@JobIds dbo.IntTableType READONLY
AS
BEGIN
-- Post TRANSEND import update

	DECLARE @process VARCHAR(20) = 'WellUpdate'
	DECLARE @Originator TINYINT = 0

	DECLARE @NewLineItemAction TABLE(
				Quantity INT
				,ExceptionType INT
				,LineItemId INT
			)
	
	-- damages
	INSERT INTO @NewLineItemAction(Quantity, ExceptionType, LineItemId)				
	SELECT Qty, dbo.ExceptionType_Damage(), jd.LineItemId
	FROM JobDetailDamage jdd
	INNER JOIN JobDetail jd on jd.Id = jdd.JobDetailId
	INNER JOIN @JobIds jobIds ON jobIds.Value = jd.JobId
	INNER JOIN LineItem li on jd.LineItemId = li.Id
	LEFT JOIN LineItemAction lia on li.Id = lia.LineItemId
	WHERE lia.Id IS NULL

	-- shorts
	INSERT INTO @NewLineItemAction(Quantity, ExceptionType, LineItemId)	
	SELECT jd.ShortQty, dbo.ExceptionType_Short(), jd.LineItemId
	FROM JobDetail jd
	INNER JOIN Job j on j.Id = jd.JobId
	INNER JOIN @JobIds jobIds ON jobIds.Value = j.Id
	INNER JOIN LineItem li on jd.LineItemId = li.Id
	LEFT JOIN LineItemAction lia on li.Id = lia.LineItemId
	WHERE lia.Id IS NULL AND jd.ShortQty > 0 and j.JobStatusId != dbo.JobStatus_Bypass()

	-- bypass
	INSERT INTO @NewLineItemAction(Quantity, ExceptionType, LineItemId)	
	SELECT jd.OriginalDespatchQty, dbo.ExceptionType_Bypass(), jd.LineItemId
	FROM JobDetail jd
	INNER JOIN Job j on j.Id = jd.JobId
	INNER JOIN @JobIds jobIds ON jobIds.Value = j.Id
	INNER JOIN LineItem li on jd.LineItemId = li.Id
	LEFT JOIN LineItemAction lia on li.Id = lia.LineItemId
	WHERE lia.Id IS NULL and j.JobStatusId = dbo.JobStatus_Bypass() and jd.OriginalDespatchQty > 0

	-- successful uplift
	INSERT INTO @NewLineItemAction(Quantity, ExceptionType, LineItemId)	
	SELECT jd.DeliveredQty, dbo.ExceptionType_Uplifted(), jd.LineItemId
	FROM JobDetail jd
	INNER JOIN Job j on j.Id = jd.JobId
	INNER JOIN @JobIds jobIds ON jobIds.Value = j.Id
	LEFT JOIN JobDetailDamage jdd on jdd.JobDetailId = jd.Id
	INNER JOIN LineItem li on jd.LineItemId = li.Id
	LEFT JOIN LineItemAction lia on li.Id = lia.LineItemId
	WHERE lia.Id IS NULL and jdd.Id IS NULL
	AND j.JobTypeCode = 'UPL-STD'

	BEGIN TRAN
		
		INSERT INTO LineItemAction(Quantity, ExceptionTypeId, LineItemId, Originator, CreatedBy, CreatedDate, LastUpdatedBy, LastUpdatedDate)	
		SELECT Quantity, ExceptionType, LineItemId , @Originator,  @process, GETDATE(), @process, GETDATE()
		FROM @NewLineItemAction
		
	COMMIT

	RETURN 0
END