IF OBJECT_ID('CleanStops', 'P') IS NOT NULL 
BEGIN
	Drop Procedure CleanStops
END

IF OBJECT_ID('CleanRoutes', 'P') IS NOT NULL 
BEGIN
	Drop Procedure CleanRoutes
END

If exists(select 1 from sys.views where name='NonSoftDeletedRoutesJobsView' and type='v')
BEGIN
	drop view NonSoftDeletedRoutesJobsView;END
go
