CREATE PROCEDURE [dbo].[WidgetWarningDelete]
	@Id INT
AS
BEGIN

DELETE FROM
	 [dbo].[WidgetToBranch]
WHERE
	Widget_Id = @Id

DELETE FROM
	 [dbo].Widget
WHERE
	Id = @Id

END