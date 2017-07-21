CREATE PROCEDURE [dbo].[CustomerRoyaltyException_Insert]
	@RoyaltyCode int,
	@Customer varchar(100),
	@ExceptionDays tinyint,
	@Username varchar(50)
AS

	INSERT INTO [CustomerRoyaltyException]
	(
		[RoyaltyCode],
		[Customer],
		[ExceptionDays],
		[CreatedBy],
		[DateCreated],
		[UpdatedBy],
		[DateUpdated]
	)
	VALUES
	(
		@RoyaltyCode,
		@Customer,
		@ExceptionDays,
		@Username,
		GETDATE(),
		@Username,
		GETDATE()
	)

SELECT CAST(SCOPE_IDENTITY() as int);

