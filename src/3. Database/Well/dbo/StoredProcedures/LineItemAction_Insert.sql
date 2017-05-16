CREATE PROCEDURE [dbo].[LineItemAction_Insert]
AS
BEGIN
	DECLARE @process VARCHAR(20) = 'WellUpdate'
	DECLARE @Originator VARCHAR(20) = 'Driver'

	DECLARE @ExceptionShort INT = 1
			,@ExceptionBypass INT = 2
			,@ExceptionDamage INT = 3

	DECLARE @NewLineItemAction TABLE(
				Quantity INT
				,ExceptionType INT
				,LineItemId INT
			)
	
	
	INSERT INTO @NewLineItemAction(Quantity, ExceptionType, LineItemId)				
	SELECT Qty, @ExceptionDamage, jd.LineItemId
	FROM JobDetailDamage jdd
	INNER JOIN JobDetail jd on jd.Id = jdd.JobDetailId
	INNER JOIN LineItem li on jd.LineItemId = li.Id
	LEFT JOIN LineItemAction lia on li.Id = lia.LineItemId
	WHERE lia.Id IS NULL

	INSERT INTO @NewLineItemAction(Quantity, ExceptionType, LineItemId)	
	SELECT jd.ShortQty, @ExceptionShort, jd.LineItemId
	FROM JobDetail jd
	INNER JOIN LineItem li on jd.LineItemId = li.Id
	LEFT JOIN LineItemAction lia on li.Id = lia.LineItemId
	WHERE lia.Id IS NULL AND jd.ShortQty > 0

	BEGIN TRAN
		
		INSERT INTO LineItemAction(Quantity, ExceptionTypeId, LineItemId, Originator, CreatedBy, CreatedDate, LastUpdatedBy, LastUpdatedDate)	
		SELECT Quantity, ExceptionType, LineItemId , @Originator,  @process, GETDATE(), @process, GETDATE()
		FROM @NewLineItemAction
		
	COMMIT

	RETURN 0
END

	

