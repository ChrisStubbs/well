CREATE PROCEDURE [dbo].[RouteHeader_DeleteById]
	@RouteheaderId int,
	@UpdatedBy varchar(50)
AS
BEGIN
	UPDATE RouteHeader 
	SET 
		DateDeleted = GETDATE(),
		UpdatedBy = @UpdatedBy
	WHERE 
		Id = @RouteheaderId
END
