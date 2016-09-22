CREATE PROCEDURE [dbo].[SeasonalDatesDelete]
	@Id INT
AS
BEGIN

DELETE FROM
	 [dbo].[SeasonalDateToBranch]
WHERE
	SeasonalDateId = @Id

DELETE FROM
	 [dbo].[SeasonalDate]
WHERE
	Id = @Id
		   		   
END