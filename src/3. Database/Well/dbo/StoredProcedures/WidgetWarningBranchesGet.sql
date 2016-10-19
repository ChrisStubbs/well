CREATE PROCEDURE [dbo].[WidgetWarningBranchesGet]
	@widgetId INT 
AS
BEGIN
SELECT
	w.[Branch_Id] as Id, b.TranscendMapping as Name
FROM
	[dbo].[WidgetToBranch] w
JOIN 
	[dbo].[Branch] b on b.Id = w.Branch_Id
WHERE
	w.Widget_Id = @widgetId

END