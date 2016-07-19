/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

DECLARE @WidgetStatRows INT 

SELECT @WidgetStatRows=count(*) FROM WidgetStats

IF @WidgetStatRows = 0
BEGIN
	INSERT INTO WidgetStats
	(
	  [NoOfExceptions],
      [Assigned],
      [Outstanding],
      [OnHold],
      [Notifications]
	)
	VALUES
	(0,0,0,0,0)
END
