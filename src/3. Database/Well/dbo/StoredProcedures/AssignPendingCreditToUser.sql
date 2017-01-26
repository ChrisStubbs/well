Create PROCEDURE [dbo].[AssignPendingCreditToUser]
	@userId INT,
	@jobId INT,
	@originator VARCHAR(50)
AS
BEGIN
	SET NOCOUNT ON;

    INSERT INTO [dbo].[PendingCreditToUser]
           ([UserId]
		   ,[JobId]
           ,[DateCreated]
		   ,[DateUpdated]
           ,[CreatedBy]
           ,[UpdatedBy])
     VALUES
           (@userId
		   ,@jobId
           ,getdate()
           ,getdate()
           ,@originator
		   ,@originator);
END