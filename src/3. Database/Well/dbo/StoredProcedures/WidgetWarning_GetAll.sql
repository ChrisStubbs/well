CREATE PROCEDURE [dbo].[WidgetWarning_GetAll]
AS
BEGIN
SELECT
	[Id], 
	[Description] AS WidgetName,
	[WarningLevel],
	[Type],
	[CreatedBy], 
	[CreatedDate], 
	[LastUpdatedBy], 
	[LastUpdatedDate]
FROM 
	[dbo].Widget
END