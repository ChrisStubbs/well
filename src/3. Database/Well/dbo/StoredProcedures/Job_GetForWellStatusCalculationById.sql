CREATE PROCEDURE [dbo].[Job_GetForWellStatusCalculationById]
	@Id int
AS
	SELECT * FROM JobForWellStatusCalculation
	WHERE Id = @Id