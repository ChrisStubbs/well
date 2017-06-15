IF COL_LENGTH('dbo.Account', 'IsDeleted') IS NOT NULL
BEGIN
	ALTER TABLE dbo.Account
	DROP COLUMN [IsDeleted]; 
END
 
IF COL_LENGTH('dbo.CleanPreference', 'IsDeleted') IS NOT NULL
BEGIN
	ALTER TABLE dbo.CleanPreference
	DROP COLUMN [IsDeleted]; 
END
 
IF COL_LENGTH('dbo.CreditThreshold', 'IsDeleted') IS NOT NULL
BEGIN
	ALTER TABLE dbo.CreditThreshold
	DROP COLUMN [IsDeleted]; 
END
 
IF COL_LENGTH('dbo.Job', 'IsDeleted') IS NOT NULL
BEGIN
	ALTER TABLE dbo.Job
	DROP COLUMN [IsDeleted]; 
END
 
IF COL_LENGTH('dbo.JobDetail', 'IsDeleted') IS NOT NULL
BEGIN
	ALTER TABLE dbo.JobDetail
	DROP COLUMN [IsDeleted]; 
END
 
IF COL_LENGTH('dbo.JobDetailDamage', 'IsDeleted') IS NOT NULL
BEGIN
	ALTER TABLE dbo.JobDetailDamage
	DROP COLUMN [IsDeleted]; 
END
 
IF COL_LENGTH('dbo.LineItemAction', 'IsDeleted') IS NOT NULL
BEGIN
	ALTER TABLE dbo.LineItemAction
	DROP COLUMN [IsDeleted]; 
END
 
IF COL_LENGTH('dbo.PendingCredit', 'IsDeleted') IS NOT NULL
BEGIN
	ALTER TABLE dbo.PendingCredit
	DROP COLUMN [IsDeleted]; 
END
 
IF COL_LENGTH('dbo.RouteHeader', 'IsDeleted') IS NOT NULL
BEGIN
	ALTER TABLE dbo.RouteHeader
	DROP COLUMN [IsDeleted]; 
END
 
IF COL_LENGTH('dbo.Routes', 'IsDeleted') IS NOT NULL
BEGIN
	ALTER TABLE dbo.Routes
	DROP COLUMN [IsDeleted]; 
END
 
IF COL_LENGTH('dbo.SeasonalDate', 'IsDeleted') IS NOT NULL
BEGIN
	ALTER TABLE dbo.SeasonalDate
	DROP COLUMN [IsDeleted]; 
END
 
IF COL_LENGTH('dbo.Stop', 'IsDeleted') IS NOT NULL
BEGIN
	ALTER TABLE dbo.Stop
	DROP COLUMN [IsDeleted]; 
END
 
IF COL_LENGTH('dbo.ThresholdLevel', 'IsDeleted') IS NOT NULL
BEGIN
	ALTER TABLE dbo.ThresholdLevel
	DROP COLUMN [IsDeleted]; 
END
 
IF COL_LENGTH('dbo.Widget', 'IsDeleted') IS NOT NULL
BEGIN
	ALTER TABLE dbo.Widget
	DROP COLUMN [IsDeleted]; 
END
 
IF COL_LENGTH('dbo.WidgetType', 'IsDeleted') IS NOT NULL
BEGIN
	ALTER TABLE dbo.WidgetType
	DROP COLUMN [IsDeleted]; 
END
