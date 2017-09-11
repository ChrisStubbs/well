CREATE PROCEDURE [dbo].[Stop_GetForWellStatusCalculationById]
	@Id int
AS
	SELECT 
		s.Id,
		s.WellStatus
	FROM Stop s
	WHERE Id = @Id
