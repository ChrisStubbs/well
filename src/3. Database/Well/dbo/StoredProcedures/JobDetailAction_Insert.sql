CREATE PROCEDURE [dbo].[JobDetailAction_Insert]
	@JobDetailId	INT,
	@Quantity		INT,
	@ActionId		INT,
	@StatusId		INT,
	@CreatedBy		varchar(50),
	@DateCreated	DateTime
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO [dbo].[JobDetailAction]
           ([JobDetailId]
		   ,[Quantity]
           ,[ActionId]
           ,[StatusId]
           ,[CreatedBy]
           ,[DateCreated]
           ,[UpdatedBy]
           ,[DateUpdated])
     VALUES
           (@JobDetailId
		   ,@Quantity
           ,@ActionId
           ,@StatusId
           ,@CreatedBy
           ,@DateCreated
           ,@CreatedBy
           ,@DateCreated)

SELECT CAST(SCOPE_IDENTITY() as int);
END
