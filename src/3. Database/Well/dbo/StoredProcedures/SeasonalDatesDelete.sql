CREATE PROCEDURE [dbo].[SeasonalDatesDelete]
	@Id INT
AS
BEGIN
DELETE FROM
	 [dbo].[SeasonalDate]
WHERE
	Id = @Id
		   
END