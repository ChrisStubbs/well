-- DELETE ALL DEFAULT VALUES oON IsDeleted columns first
declare constraints cursor for 
SELECT
	'alter table ' + t.name + ' drop constraint [' + d.name +']' 
FROM sys.columns AS C
       INNER JOIN sys.tables t
              ON C.object_id = t.object_id
       JOIN sys.default_constraints AS D
              ON C.column_id = D.parent_column_id
			  AND d.parent_object_id = t.object_id
WHERE
       c.name = 'IsDeleted';
	   
declare @cmd varchar(max)
open constraints
fetch next from constraints into @cmd
while @@FETCH_STATUS=0
begin
	exec(@cmd)
	fetch next from constraints into @cmd
end
close constraints
deallocate constraints
-------------------------------------------------------
-- NOW DELETE THE IsDeletedColumns
IF COL_LENGTH('dbo.Account', 'IsDeleted') IS NOT NULL
BEGIN
	UPDATE dbo.Account
	SET DateDeleted = GETDATE() WHERE IsDeleted = 1; 
END

IF COL_LENGTH('dbo.CleanPreference', 'IsDeleted') IS NOT NULL
BEGIN
	UPDATE dbo.CleanPreference
	SET DateDeleted = GETDATE() WHERE IsDeleted = 1; 
END

IF COL_LENGTH('dbo.CreditThreshold', 'IsDeleted') IS NOT NULL
BEGIN
	UPDATE dbo.CreditThreshold
	SET DateDeleted = GETDATE() WHERE IsDeleted = 1; 
END

IF COL_LENGTH('dbo.Job', 'IsDeleted') IS NOT NULL
BEGIN
	UPDATE dbo.Job
	SET DateDeleted = GETDATE() WHERE IsDeleted = 1; 
END

IF COL_LENGTH('dbo.JobDetail', 'IsDeleted') IS NOT NULL
BEGIN
	UPDATE dbo.JobDetail
	SET DateDeleted = GETDATE() WHERE IsDeleted = 1; 
END

IF COL_LENGTH('dbo.JobDetailDamage', 'IsDeleted') IS NOT NULL
BEGIN
	UPDATE dbo.JobDetailDamage
	SET DateDeleted = GETDATE() WHERE IsDeleted = 1; 
END

IF COL_LENGTH('dbo.LineItemAction', 'IsDeleted') IS NOT NULL
BEGIN
	UPDATE dbo.LineItemAction
	SET DateDeleted = GETDATE() WHERE IsDeleted = 1; 
END

IF COL_LENGTH('dbo.PendingCredit', 'IsDeleted') IS NOT NULL
BEGIN
	UPDATE dbo.PendingCredit
	SET DateDeleted = GETDATE() WHERE IsDeleted = 1; 
END

IF COL_LENGTH('dbo.RouteHeader', 'IsDeleted') IS NOT NULL
BEGIN
	UPDATE dbo.RouteHeader
	SET DateDeleted = GETDATE() WHERE IsDeleted = 1; 
END

IF COL_LENGTH('dbo.Routes', 'IsDeleted') IS NOT NULL
BEGIN
	UPDATE dbo.Routes
	SET DateDeleted = GETDATE() WHERE IsDeleted = 1; 
END

IF COL_LENGTH('dbo.SeasonalDate', 'IsDeleted') IS NOT NULL
BEGIN
	UPDATE dbo.SeasonalDate
	SET DateDeleted = GETDATE() WHERE IsDeleted = 1; 
END

IF COL_LENGTH('dbo.Stop', 'IsDeleted') IS NOT NULL
BEGIN
	UPDATE dbo.Stop
	SET DateDeleted = GETDATE() WHERE IsDeleted = 1; 
END

IF COL_LENGTH('dbo.ThresholdLevel', 'IsDeleted') IS NOT NULL
BEGIN
	UPDATE dbo.ThresholdLevel
	SET DateDeleted = GETDATE() WHERE IsDeleted = 1; 
END

IF COL_LENGTH('dbo.Widget', 'IsDeleted') IS NOT NULL
BEGIN
	UPDATE dbo.Widget
	SET DateDeleted = GETDATE() WHERE IsDeleted = 1; 
END

IF COL_LENGTH('dbo.WidgetType', 'IsDeleted') IS NOT NULL
BEGIN
	UPDATE dbo.WidgetType
	SET DateDeleted = GETDATE() WHERE IsDeleted = 1; 
END
