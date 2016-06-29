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

DECLARE @ServiceBrokerEnabled BIT

SELECT @ServiceBrokerEnabled = is_broker_enabled FROM sys.databases WHERE name = 'Well';

IF @ServiceBrokerEnabled = 0
BEGIN
	ALTER DATABASE [Well] SET ENABLE_BROKER WITH ROLLBACK IMMEDIATE
END

