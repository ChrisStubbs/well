CREATE PROCEDURE [dbo].[CreditThresholdGetAll]
AS
BEGIN
SELECT
	[Id], [ThresholdLevelId], [Value], [CreatedBy], [CreatedDate], [LastUpdatedBy], [LastUpdatedDate]
FROM
	 [dbo].[CreditThreshold] 
		   
END