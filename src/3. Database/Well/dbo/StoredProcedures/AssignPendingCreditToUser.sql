Create PROCEDURE [dbo].[AssignPendingCreditToUser]
	@userId INT,
	@invoiceNumber VARCHAR(50),
	@originator VARCHAR(50)
AS
BEGIN
	SET NOCOUNT ON;

    INSERT INTO [dbo].[PendingCreditToUser]
           ([UserId]
		   ,[InvoiceNumber]
           ,[DateCreated]
		   ,[DateUpdated]
           ,[CreatedBy]
           ,[UpdatedBy])
     VALUES
           (@userId
		   ,@invoiceNumber
           ,getdate()
           ,getdate()
           ,@originator
		   ,@originator);
END