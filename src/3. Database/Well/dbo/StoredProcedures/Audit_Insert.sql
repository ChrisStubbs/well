CREATE PROCEDURE [dbo].[Audit_Insert]	
	@Entry				VARCHAR(8000),
	@Type				INT,
	@InvoiceNumber		VARCHAR(255),
	@AccountCode		VARCHAR(255),
	@DeliveryDate		Datetime = null,
	@CreatedBy			VARCHAR(50),
	@DateCreated		Datetime
AS
BEGIN
	SET NOCOUNT ON;

INSERT INTO [dbo].[Audit]
           ([Entry]
           ,[Type]
           ,[InvoiceNumber]
           ,[AccountCode]           
           ,[DeliveryDate]
           ,[CreatedBy]
           ,[DateCreated]
           ,[UpdatedBy]
           ,[DateUpdated])
     VALUES
           (@Entry
           ,@Type
           ,@InvoiceNumber
           ,@AccountCode           
           ,@DeliveryDate
           ,@CreatedBy
           ,@DateCreated
           ,@CreatedBy
           ,@DateCreated)
		   
SELECT CAST(SCOPE_IDENTITY() as int);
END
