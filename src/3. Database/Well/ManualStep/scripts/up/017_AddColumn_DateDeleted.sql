IF COL_LENGTH('dbo.Account', 'DateDeleted') IS NULL
BEGIN
	ALTER TABLE dbo.Account
	ADD DateDeleted DateTime NULL 
END
 
IF COL_LENGTH('dbo.Activity', 'DateDeleted') IS NULL
BEGIN
	ALTER TABLE dbo.Activity
	ADD DateDeleted DateTime NULL 
END

IF COL_LENGTH('dbo.Bag', 'DateDeleted') IS NULL
BEGIN
	ALTER TABLE dbo.Bag
	ADD DateDeleted DateTime NULL 
END

IF COL_LENGTH('dbo.CleanPreference', 'DateDeleted') IS NULL
BEGIN
	ALTER TABLE dbo.CleanPreference
	ADD DateDeleted DateTime NULL 
END

IF COL_LENGTH('dbo.CreditThreshold', 'DateDeleted') IS NULL
BEGIN
	ALTER TABLE dbo.CreditThreshold
	ADD DateDeleted DateTime NULL 
END

IF COL_LENGTH('dbo.Job', 'DateDeleted') IS NULL
BEGIN
	ALTER TABLE dbo.Job
	ADD DateDeleted DateTime NULL 
END

IF COL_LENGTH('dbo.JobDetail', 'DateDeleted') IS NULL
BEGIN
	ALTER TABLE dbo.JobDetail
	ADD DateDeleted DateTime NULL 
END

IF COL_LENGTH('dbo.JobDetailDamage', 'DateDeleted') IS NULL
BEGIN
	ALTER TABLE dbo.JobDetailDamage
	ADD DateDeleted DateTime NULL 
END

IF COL_LENGTH('dbo.LineItem', 'DateDeleted') IS NULL
BEGIN
	ALTER TABLE dbo.LineItem
	ADD DateDeleted DateTime NULL 
END

IF COL_LENGTH('dbo.LineItemAction', 'DateDeleted') IS NULL
BEGIN
	ALTER TABLE dbo.LineItemAction
	ADD DateDeleted DateTime NULL 
END

IF COL_LENGTH('dbo.PendingCredit', 'DateDeleted') IS NULL
BEGIN
	ALTER TABLE dbo.PendingCredit
	ADD DateDeleted DateTime NULL 
END

IF COL_LENGTH('dbo.RouteHeader', 'DateDeleted') IS NULL
BEGIN
	ALTER TABLE dbo.RouteHeader
	ADD DateDeleted DateTime NULL 
END

IF COL_LENGTH('dbo.Routes', 'DateDeleted') IS NULL
BEGIN
	ALTER TABLE dbo.Routes
	ADD DateDeleted DateTime NULL 
END

IF COL_LENGTH('dbo.SeasonalDate', 'DateDeleted') IS NULL
BEGIN
	ALTER TABLE dbo.SeasonalDate
	ADD DateDeleted DateTime NULL 
END

IF COL_LENGTH('dbo.Stop', 'DateDeleted') IS NULL
BEGIN
	ALTER TABLE dbo.Stop
	ADD DateDeleted DateTime NULL 
END

IF COL_LENGTH('dbo.ThresholdLevel', 'DateDeleted') IS NULL
BEGIN
	ALTER TABLE dbo.ThresholdLevel
	ADD DateDeleted DateTime NULL 
END

IF COL_LENGTH('dbo.Widget', 'DateDeleted') IS NULL
BEGIN
	ALTER TABLE dbo.Widget
	ADD DateDeleted DateTime NULL 
END

IF COL_LENGTH('dbo.WidgetType', 'DateDeleted') IS NULL
BEGIN
	ALTER TABLE dbo.WidgetType
	ADD DateDeleted DateTime NULL 
END
