CREATE PROCEDURE [dbo].[LineItemActionUpdate]
		 @Id INT
		,@ExceptionTypeId INT
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
		,@DeliveryActionId INT
		,@UpdatedBy VARCHAR(50)
		,@UpdatedDate DATETIME
		,@DateDeleted DATETIME = NULL
AS
UPDATE [dbo].[LineItemAction]
   SET [ExceptionTypeId] = @ExceptionTypeId
      ,[Quantity] = @Quantity
      ,[SourceId] =@SourceId
      ,[ReasonId] =@ReasonId
      ,[ReplanDate] = @ReplanDate
      ,[SubmittedDate] =@SubmittedDate
      ,[ApprovalDate] = @ApprovalDate
      ,[ApprovedBy] = @ApprovedBy
      ,[LineItemId] = @LineItemId
      ,[Originator] = @Originator
      ,[ActionedBy] = @ActionedBy
	  ,[DeliveryActionId] = @DeliveryActionId
      ,[LastUpdatedBy] = @UpdatedBy
      ,[LastUpdatedDate] = @UpdatedDate
	  ,[DateDeleted] = @DateDeleted
 WHERE @Id = Id

RETURN 0
