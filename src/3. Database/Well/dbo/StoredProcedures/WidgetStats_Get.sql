CREATE PROCEDURE [dbo].[WidgetStats_Get]

AS
BEGIN
	select [NoOfExceptions],[Assigned], [Outstanding], [OnHold], [Notifications] from dbo.WidgetStats
END