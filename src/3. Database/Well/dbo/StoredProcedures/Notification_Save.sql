CREATE PROCEDURE [dbo].[Notification_Save]
	@JobId INT,
	@Type TINYINT,
	@ErrorMessage VARCHAR(255),
	@Branch VARCHAR(2) ,
	@Account VARCHAR(10) ,
	@InvoiceNumber VARCHAR(20) ,
	@LineNumber VARCHAR(3) ,
	@AdamErrorNumber VARCHAR(3) ,
	@AdamCrossReference VARCHAR(20) ,
	@UserName VARCHAR(10), 
	@CreatedBy VARCHAR(50),
	@DateCreated DATETIME,
	@UpdatedBy VARCHAR(50),
	@DateUpdated DATETIME
AS
BEGIN

	INSERT INTO [dbo].[Notification](JobId, [Type], ErrorMessage, Branch, Account, InvoiceNumber, LineNumber, AdamErrorNumber, AdamCrossReference, UserName,CreatedBy, CreatedDate, LastUpdatedBy, LastUpdatedDate)
	VALUES (@JobId, @Type, @ErrorMessage, @Branch, @Account, @InvoiceNumber, @LineNumber, @AdamErrorNumber, @AdamCrossReference,@UserName , @CreatedBy, @DateCreated, @UpdatedBy, @DateUpdated)

END
