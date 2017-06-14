CREATE PROCEDURE [dbo].[LineItemActionCommentInsert]
		@LineItemActionId	INT
		,@CommentReasonId	INT
		,@CreatedBy			VARCHAR (50)
		,@DateCreated		DATETIME
AS
BEGIN

	INSERT INTO [dbo].[LineItemActionComment]
           ([LineItemActionId]
           ,[CommentReasonId]
           ,[CreatedBy]
           ,[DateCreated]
           ,[UpdatedBy]
           ,[DateUpdated])
    VALUES
			(@LineItemActionId	
			,@CommentReasonId	
			,@CreatedBy			
			,@DateCreated
			,@CreatedBy			
			,@DateCreated)
				   
	SELECT CAST(SCOPE_IDENTITY() as int);

END
