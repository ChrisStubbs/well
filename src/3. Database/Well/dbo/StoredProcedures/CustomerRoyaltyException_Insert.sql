CREATE PROCEDURE [dbo].[CustomerRoyaltyException_Insert]
	@RoyaltyId int,
	@Customer varchar(100),
	@ExceptionDays int,
	@Username varchar(50)
AS

SET IDENTITY_INSERT [CustomerRoyaltyException] ON

	INSERT INTO [CustomerRoyaltyException]
	(
		[RoyaltyId],
		[Customer],
		[ExceptionDays],
		[CreatedBy],
		[DateCreated],
		[UpdatedBy],
		[DateUpdated]
	)
	VALUES
	(
		@RoyaltyId,
		@Customer,
		@ExceptionDays,
		@Username,
		GETDATE(),
		@Username,
		GETDATE()
	)

SET IDENTITY_INSERT [CustomerRoyaltyException] OFF

SELECT CAST(SCOPE_IDENTITY() as int);

RETURN 0
