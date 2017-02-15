Create PROCEDURE [dbo].[PendingCreditInsert]
	@jobId INT,
	@originator VARCHAR(50)
AS
BEGIN
	SET NOCOUNT ON;

    INSERT INTO [dbo].[PendingCredit]
           ([JobId]
           ,[DateCreated]
		   ,[DateUpdated]
           ,[CreatedBy]
           ,[UpdatedBy])
     VALUES
           (@jobId
           ,getdate()
           ,getdate()
           ,@originator
		   ,@originator);
END