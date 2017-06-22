CREATE PROCEDURE [dbo].[LineItemActionCommentUpdate]
		@Id					INT
		,@LineItemActionId	INT
		,@CommentReasonId	INT
		,@FromQty			INT = NULL
		,@ToQty				INT
		,@UpdatedBy			VARCHAR (50)
		,@DateUpdated		DATETIME
		,@DateDeleted		DATETIME = NULL
AS
BEGIN

	UPDATE [dbo].[LineItemActionComment]
	SET 
		[LineItemActionId]	= @LineItemActionId
		,[CommentReasonId]	= @CommentReasonId
		,[FromQty]			= @FromQty
		,[ToQty]			= @ToQty
		,[UpdatedBy]		= @UpdatedBy
		,[DateUpdated]		= @DateUpdated
		,[DateDeleted]		= @DateDeleted
	WHERE
		[Id] = @Id
END
