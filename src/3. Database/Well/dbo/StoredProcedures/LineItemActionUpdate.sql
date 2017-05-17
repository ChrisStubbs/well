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
		,@UpdatedBy VARCHAR(50)
		,@UpdatedDate DATETIME
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
     -- ,[ActionedBy] = <ActionedBy, varchar(50),>
      ,[LastUpdatedBy] = @UpdatedBy
      ,[LastUpdatedDate] = @UpdatedDate
 WHERE @Id = Id

RETURN 0
