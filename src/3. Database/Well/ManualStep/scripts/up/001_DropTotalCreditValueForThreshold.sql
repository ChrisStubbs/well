IF COL_LENGTH('dbo.job', 'TotalCreditValueForThreshold') IS NOT NULL
BEGIN
	ALTER TABLE [Job] DROP COLUMN [TotalCreditValueForThreshold];
END













