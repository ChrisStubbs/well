CREATE PROCEDURE  [dbo].[RemovePendingCredit]
	@invoiceNumber VARCHAR(50)
AS
BEGIN

	SET NOCOUNT ON;
	UPDATE PendingCreditToUSer SET IsDeleted = 1 WHERE InvoiceNumber = @invoiceNumber
END