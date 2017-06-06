﻿CREATE PROCEDURE [dbo].[LineItemAction_Insert]
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
	
	INSERT INTO @NewLineItemAction(Quantity, ExceptionType, LineItemId)				
	SELECT Qty, dbo.ExceptionType_Damage(), jd.LineItemId
	FROM JobDetailDamage jdd
	INNER JOIN JobDetail jd on jd.Id = jdd.JobDetailId
	INNER JOIN LineItem li on jd.LineItemId = li.Id
	LEFT JOIN LineItemAction lia on li.Id = lia.LineItemId
	WHERE lia.Id IS NULL

	INSERT INTO @NewLineItemAction(Quantity, ExceptionType, LineItemId)	
	SELECT jd.ShortQty, dbo.ExceptionType_Short(), jd.LineItemId
	FROM JobDetail jd
	INNER JOIN LineItem li on jd.LineItemId = li.Id
	LEFT JOIN LineItemAction lia on li.Id = lia.LineItemId
	WHERE lia.Id IS NULL AND jd.ShortQty > 0

	INSERT INTO @NewLineItemAction(Quantity, ExceptionType, LineItemId)	
	SELECT jd.OriginalDespatchQty, dbo.ExceptionType_Bypass(), jd.LineItemId
	FROM JobDetail jd
	INNER JOIN Job j on j.Id = jd.JobId
	INNER JOIN LineItem li on jd.LineItemId = li.Id
	LEFT JOIN LineItemAction lia on li.Id = lia.LineItemId
	WHERE lia.Id IS NULL and j.JobStatusId = dbo.JobStatus_Bypass()

	BEGIN TRAN
		
		INSERT INTO LineItemAction(Quantity, ExceptionTypeId, LineItemId, Originator, CreatedBy, CreatedDate, LastUpdatedBy, LastUpdatedDate)	
		SELECT Quantity, ExceptionType, LineItemId , @Originator,  @process, GETDATE(), @process, GETDATE()
		FROM @NewLineItemAction
		
	COMMIT

	RETURN 0
END

	

