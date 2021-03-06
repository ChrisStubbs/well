﻿CREATE PROCEDURE [dbo].[LineItemAction_Insert]
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
				,PDAReasonDescription VARCHAR(50)
			)
	
	-- damages
	INSERT INTO @NewLineItemAction(Quantity, ExceptionType, LineItemId, PDAReasonDescription)				
	SELECT Qty, dbo.ExceptionType_Damage(), jd.LineItemId, ISNULL(jdd.PDAReasonDescription, '')
	FROM JobDetailDamage jdd
	INNER JOIN JobDetail jd on jd.Id = jdd.JobDetailId
	INNER JOIN @JobIds jobIds ON jobIds.Value = jd.JobId
	INNER JOIN Job j on j.Id = jd.JobId
	INNER JOIN LineItem li on jd.LineItemId = li.Id
	LEFT JOIN LineItemAction lia on li.Id = lia.LineItemId AND lia.DateDeleted IS NULL
	WHERE lia.Id IS NULL AND j.JobTypeCode != 'UPL-GLO'

	

	-- shorts
	INSERT INTO @NewLineItemAction(Quantity, ExceptionType, LineItemId, PDAReasonDescription)	
	SELECT jd.ShortQty, dbo.ExceptionType_Short(), jd.LineItemId, ''
	FROM JobDetail jd
	INNER JOIN Job j on j.Id = jd.JobId
	INNER JOIN @JobIds jobIds ON jobIds.Value = j.Id
	INNER JOIN LineItem li on jd.LineItemId = li.Id
	LEFT JOIN LineItemAction lia on li.Id = lia.LineItemId AND lia.DateDeleted IS NULL
	WHERE lia.Id IS NULL AND jd.ShortQty > 0 and j.JobStatusId != dbo.JobStatus_Bypass()
		   AND j.JobTypeCode != 'UPL-GLO'

	-- bypass
	INSERT INTO @NewLineItemAction(Quantity, ExceptionType, LineItemId, PDAReasonDescription)	
	SELECT jd.OriginalDespatchQty, dbo.ExceptionType_Bypass(), jd.LineItemId, ''
	FROM JobDetail jd
	INNER JOIN Job j on j.Id = jd.JobId
	INNER JOIN @JobIds jobIds ON jobIds.Value = j.Id
	INNER JOIN LineItem li on jd.LineItemId = li.Id
	LEFT JOIN LineItemAction lia on li.Id = lia.LineItemId AND lia.DateDeleted IS NULL
	WHERE lia.Id IS NULL and j.JobStatusId = dbo.JobStatus_Bypass() and jd.OriginalDespatchQty > 0
		  AND j.JobTypeCode != 'UPL-GLO'

	-- successful uplift
	INSERT INTO @NewLineItemAction(Quantity, ExceptionType, LineItemId, PDAReasonDescription)	
	SELECT jd.DeliveredQty, dbo.ExceptionType_Uplifted(), jd.LineItemId, ''
	FROM JobDetail jd
	INNER JOIN Job j on j.Id = jd.JobId
	INNER JOIN @JobIds jobIds ON jobIds.Value = j.Id
	LEFT JOIN JobDetailDamage jdd on jdd.JobDetailId = jd.Id
	INNER JOIN LineItem li on jd.LineItemId = li.Id
	LEFT JOIN LineItemAction lia on li.Id = lia.LineItemId AND lia.DateDeleted IS NULL
	WHERE lia.Id IS NULL and jdd.Id IS NULL
	AND j.JobTypeCode = 'UPL-STD' AND jd.ShortQty = 0 AND j.JobStatusId != dbo.JobStatus_Bypass()  

	BEGIN TRAN
		
		INSERT INTO LineItemAction(Quantity, ExceptionTypeId, LineItemId, Originator, CreatedBy, CreatedDate, LastUpdatedBy, LastUpdatedDate)	
		SELECT Quantity, ExceptionType, LineItemId , @Originator,  @process, GETDATE(), @process, GETDATE()
		FROM @NewLineItemAction
		
	COMMIT

	RETURN 0
END