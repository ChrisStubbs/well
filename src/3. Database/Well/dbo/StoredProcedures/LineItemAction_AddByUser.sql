CREATE PROCEDURE [dbo].[LineItemAction_AddByUser]
		@ExceptionTypeId INT
		,@Quantity INT
		,@SourceId TINYINT
		,@ReasonId TINYINT
		,@ReplanDate datetime
		,@SubmittedDate DATETIME
		,@ApprovalDate DATETIME
		,@ApprovedBy VARCHAR(50)
		,@LineItemId INT 
		,@CreatedBy VARCHAR(50)
		,@CreatedDate DATETIME
		,@LastUpdatedBy VARCHAR(50)
		,@LastUpdatedDate DATETIME
AS
	BEGIN
		SET NOCOUNT ON;

		INSERT INTO [dbo].LineItemAction
			(ExceptionTypeId 
		,Quantity 
		,SourceId 
		,ReasonId 
		,ReplanDate 
		,SubmittedDate 
		,ApprovalDate 
		,ApprovedBy 
		,LineItemId 
		,CreatedBy
		,CreatedDate
		,LastUpdatedBy
		,LastUpdatedDate
		)
		VALUES
		(@ExceptionTypeId 
		,@Quantity 
		,@SourceId 
		,@ReasonId 
		,@ReplanDate 
		,@SubmittedDate 
		,@ApprovalDate 
		,@ApprovedBy 
		,@LineItemId
		,@CreatedBy
		,@CreatedDate
		,@LastUpdatedBy
		,@LastUpdatedDate
		)

		SELECT CAST(SCOPE_IDENTITY() as int);
	END