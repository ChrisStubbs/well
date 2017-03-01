Create PROCEDURE [dbo].[PendingCreditInsert]
	@jobId INT,
	@originator VARCHAR(50)
AS
BEGIN
	SET NOCOUNT ON;

	if not exists(select Id from dbo.PendingCredit where JobId = @jobId)
	begin

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
	end
END