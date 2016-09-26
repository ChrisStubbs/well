CREATE PROCEDURE [dbo].[CreditThresholdGetAll]
AS
BEGIN
SELECT
	[Id], [ThresholdLevelId], [Threshold], [CreatedBy], [CreatedDate], [LastUpdatedBy], [LastUpdatedDate]
FROM
	 [dbo].[CreditThreshold] 
		   
END