CREATE PROCEDURE [dbo].[CustomerRoyaltyException_Update]
	@Id int,
	@RoyaltyCode int,
	@Customer varchar(100),
	@ExceptionDays tinyint,
	@Username varchar(50)
AS

	UPDATE [CustomerRoyaltyException]
	SET
		[RoyaltyCode]	= @RoyaltyCode,
		[Customer]		= @Customer,
		[ExceptionDays] = @ExceptionDays,
		[CreatedBy]		= @Username,
		[DateCreated]	= GETDATE(),
		[UpdatedBy]		= @Username,
		[DateUpdated]	= GETDATE()		
	WHERE Id = @Id


