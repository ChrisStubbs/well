CREATE PROCEDURE [dbo].[Stop_GetForWellStatusCalculationById]
	@Id int
AS
	SELECT 
		s.Id,
		s.WellStatus,
		s.RouteHeaderId
	FROM Stop s
	WHERE Id = @Id
