CREATE PROCEDURE [dbo].[CreditThresholdDelete]
	@Id INT
AS
BEGIN
DELETE FROM
	 [dbo].[CreditThreshold]
WHERE
	Id = @Id		   		   
END