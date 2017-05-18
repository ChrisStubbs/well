CREATE PROCEDURE [dbo].[LineItemAction_InsertByUser]
		@ExceptionTypeId INT
		,@Quantity INT
		,@SourceId TINYINT
		,@ReasonId INT
		,@ReplanDate datetime
		,@SubmittedDate DATETIME
		,@ApprovalDate DATETIME
		,@ApprovedBy VARCHAR(50)
		,@LineItemId INT 
		,@Originator VARCHAR(50)
		,@ActionedBy VARCHAR(50)
		,@CreatedBy VARCHAR(50)
		,@CreatedDate DATETIME
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
		,Originator
		,ActionedBy
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
		,@Originator
		,@ActionedBy
		,@CreatedBy
		,@CreatedDate
		,@CreatedBy
		,@CreatedDate
		)

		SELECT CAST(SCOPE_IDENTITY() as int);
	END