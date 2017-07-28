IF OBJECT_ID('dbo.CreditThresholdToBranch', 'U') IS NOT NULL 
BEGIN
	DROP TABLE dbo.CreditThresholdToBranch;
END

IF OBJECT_ID('dbo.ThresholdLevel', 'U') IS NOT NULL 
BEGIN	
	-- Drop related indexes first
	ALTER TABLE [dbo].[CreditThreshold] DROP CONSTRAINT [FK_CreditThreshold_ThresholdLevel]

	ALTER TABLE [dbo].[User] DROP CONSTRAINT [FK_ThresholdLevelId_ThesholdLevel]
	ALTER TABLE [dbo].[User] DROP COLUMN [ThresholdLevelId]
	
	DROP TABLE dbo.ThresholdLevel;

	DELETE FROM [dbo].[CreditThreshold]
END