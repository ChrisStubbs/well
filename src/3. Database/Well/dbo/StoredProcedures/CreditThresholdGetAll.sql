CREATE PROCEDURE [dbo].[CreditThresholdGetAll]
AS
BEGIN
SELECT
	[Id], [UserRoleId], [Threshold], [CreatedBy], [CreatedDate], [LastUpdatedBy], [LastUpdatedDate]
FROM
	 [dbo].[CreditThreshold] 
		   
END