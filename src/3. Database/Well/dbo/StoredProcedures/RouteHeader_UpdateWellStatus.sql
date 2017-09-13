CREATE PROCEDURE [dbo].[RouteHeader_UpdateWellStatus]
	@Id				int,
	@WellStatusId	TinyInt
AS
	UPDATE RouteHeader
	SET WellStatus = @WellStatusId
	WHERE Id = @Id
