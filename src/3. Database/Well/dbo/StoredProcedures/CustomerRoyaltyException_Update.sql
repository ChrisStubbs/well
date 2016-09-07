CREATE PROCEDURE [dbo].[CustomerRoyaltyException_Update]
	@Id int,
	@RoyaltyId int,
	@Customer varchar(100),
	@ExceptionDays int,
	@Username varchar(50)
AS

SET IDENTITY_INSERT [CustomerRoyaltyException] ON

	UPDATE [CustomerRoyaltyException]
	SET
		[RoyaltyId] = @RoyaltyId,
		[Customer] = @Customer,
		[ExceptionDays] = @ExceptionDays,
		[CreatedBy] = @Username,
		[DateCreated] = GETDATE(),
		[UpdatedBy] = @Username,
		[DateUpdated] = GETDATE()		
	WHERE Id = @Id


SET IDENTITY_INSERT [CustomerRoyaltyException] OFF
RETURN 0
