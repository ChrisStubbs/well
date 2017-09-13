CREATE PROCEDURE [dbo].[LineItemActionCommentInsert]
		@LineItemActionId	INT
		,@CommentReasonId	INT
		,@FromQty			INT = NULL
		,@ToQty				INT
		,@CreatedBy			VARCHAR (50)
		,@DateCreated		DATETIME
AS
BEGIN

	INSERT INTO [dbo].[LineItemActionComment]
           ([LineItemActionId]
           ,[CommentReasonId]
		   ,[FromQty]
		   ,[ToQty]
           ,[CreatedBy]
           ,[DateCreated]
           ,[UpdatedBy]
           ,[DateUpdated])
    VALUES
			(@LineItemActionId	
			,@CommentReasonId
			,@FromQty
			,@ToQty
			,@CreatedBy			
			,@DateCreated
			,@CreatedBy			
			,@DateCreated)
				   
	SELECT CAST(SCOPE_IDENTITY() as int);

END
