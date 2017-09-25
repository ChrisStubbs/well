CREATE PROCEDURE [dbo].[Stop_UpdateWellStatus]
	@Data WellStatusUpdate ReadOnly
AS
	UPDATE Stop
	SET WellStatus = s.WellStatus
	FROM 
	(
		SELECT d.EntityKey AS StopId, d.WellStatus
		FROM @Data d
	) s
	WHERE Id = s.StopId
