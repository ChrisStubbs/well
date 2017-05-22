CREATE PROCEDURE [dbo].[Account_Insert]
	@Code					VARCHAR(50),
	@AccountTypeCode		VARCHAR(50),
	@DepotId				INT  NULL,
	@Name					VARCHAR(50),
	@Address1				VARCHAR(50),
	@Address2				VARCHAR(50),
	@PostCode				VARCHAR(10),
	@ContactName			VARCHAR(50),
	@ContactNumber			VARCHAR(15),
	@ContactNumber2			VARCHAR(15),
	@ContactEmailAddress	VARCHAR(50),
	@StopId					INT,
	@CreatedBy VARCHAR(50),
	@UpdatedBy VARCHAR(50),
	@CreatedDate DATETIME,
	@UpdatedDate DATETIME
AS
BEGIN
	SET NOCOUNT ON;

	INSERT [Account] (
		[Code],
		[AccountTypeCode],
		[DepotId],
		[Name], 
		[Address1], 
		[Address2], 
		[PostCode], 
		[ContactName], 
		[ContactNumber],
		[ContactNumber2], 
		[ContactEmailAddress], 
		[StopId], 
		[CreatedBy],
		[DateCreated],
		[UpdatedBy],
		[DateUpdated])
	VALUES (
		@Code, 
		@AccountTypeCode,
		@DepotId,  
		@Name, 
		@Address1, 
		@Address2, 
		@PostCode, 
		@ContactName, 
		@ContactNumber, 
		@ContactNumber2, 
		@ContactEmailAddress,
		@StopId, 
		@CreatedBy, 
		@CreatedDate, 
		@UpdatedBy, 
		@UpdatedDate)

	SELECT CAST(SCOPE_IDENTITY() as int);

END
