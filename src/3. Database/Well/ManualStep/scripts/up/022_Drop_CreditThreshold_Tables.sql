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

-- Drop stored procedures
IF OBJECT_ID('CreditThresholdByBranch', 'P') IS NOT NULL 
BEGIN
	DROP PROCEDURE CreditThresholdByBranch
END

IF OBJECT_ID('CreditThresholdToBranchSave', 'P') IS NOT NULL 
BEGIN
	DROP PROCEDURE CreditThresholdToBranchSave
END

IF OBJECT_ID('CreditThresholdToBranch', 'P') IS NOT NULL 
BEGIN
	DROP PROCEDURE CreditThresholdToBranch
END

IF OBJECT_ID('CreditThresholdBranchesGet', 'P') IS NOT NULL 
BEGIN
	DROP PROCEDURE CreditThresholdBranchesGet
END

IF OBJECT_ID('ThresholdLevelSave', 'P') IS NOT NULL 
BEGIN
	DROP PROCEDURE ThresholdLevelSave
END

IF OBJECT_ID('User_GetCreditThreshold', 'P') IS NOT NULL 
BEGIN
	DROP PROCEDURE User_GetCreditThreshold
END

IF OBJECT_ID('User_SetCreditThreshold', 'P') IS NOT NULL 
BEGIN
	DROP PROCEDURE User_SetCreditThreshold
END

IF OBJECT_ID('GetBranchesForCreditThreshold', 'P') IS NOT NULL 
BEGIN
	DROP PROCEDURE GetBranchesForCreditThreshold
END

IF OBJECT_ID('UserByCreditThresholdGet', 'P') IS NOT NULL 
BEGIN
	DROP PROCEDURE UserByCreditThresholdGet
END

