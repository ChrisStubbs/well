CREATE PROCEDURE [dbo].[PendingCredits_Insert]
	@CreditLines dbo.CreditTableType READONLY,
	@UserId INT
AS

BEGIN

DECLARE @UserIndent VARCHAR(50)

	SELECT @UserIndent=IdentityName FROM dbo.[User] WHERE Id=@UserId

	INSERT INTO PendingCreditToUser
	(
	  [UserId],
      [InvoiceNumber],
      [IsDeleted],
      [CreatedBy],
      [DateCreated],
      [UpdatedBy],
      [DateUpdated]
	)
	SELECT 
		@UserId,
		InvoiceNumber,
		0,
		@UserIndent,
		GETDATE(),
		@UserIndent,
		GETDATE()
	FROM
		Job 
	WHERE Id IN (SELECT CreditId FROM @CreditLines WHERE IsPending = 1)

END
