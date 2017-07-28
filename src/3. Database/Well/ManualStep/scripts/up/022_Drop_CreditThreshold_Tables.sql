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

	EXEC sp_RENAME 'CreditThreshold.ThresholdLevelId', 'Level', 'COLUMN' -- Rename column which previously was FK of Threshold level. Data should match actual level value
END