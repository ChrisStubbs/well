CREATE PROCEDURE [dbo].[JobDetailAction_Update]
	@Id				INT,
	@JobDetailId	INT,
	@Quantity		INT,
	@ActionId		INT,
	@StatusId		INT,
	@UpdatedBy		varchar(50),
	@DateUpdated	DateTime
AS
BEGIN
	SET NOCOUNT ON;

UPDATE [dbo].[JobDetailAction]
   SET [JobDetailId] = @JobDetailId
      ,[Quantity] = @Quantity
      ,[ActionId] = @ActionId
      ,[StatusId] = @StatusId
      ,[UpdatedBy] = @UpdatedBy
      ,[DateUpdated] = @DateUpdated
 WHERE [Id] = @Id

END
