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
	EXEC sp_addrole 'sql_dependency_subscriber'

	GRANT CREATE PROCEDURE to [PALMERHARVEY\CSDevService]
	GRANT CREATE QUEUE to [PALMERHARVEY\CSDevService]

	GRANT CREATE QUEUE to [PALMERHARVEY\CSDevService]
	GRANT CREATE SERVICE to [PALMERHARVEY\CSDevService]
	GRANT REFERENCES on CONTRACT::[http://schemas.microsoft.com/SQL/Notifications/PostQueryNotification] to [PALMERHARVEY\CSDevService] 
	GRANT VIEW DEFINITION TO [PALMERHARVEY\CSDevService]

	GRANT SELECT to [PALMERHARVEY\CSDevService]
	GRANT SUBSCRIBE QUERY NOTIFICATIONS TO [PALMERHARVEY\CSDevService] 
	GRANT RECEIVE ON QueryNotificationErrorsQueue TO [PALMERHARVEY\CSDevService] 
	GRANT REFERENCES on CONTRACT::[http://schemas.microsoft.com/SQL/Notifications/PostQueryNotification] to [PALMERHARVEY\CSDevService] 
	EXEC sp_addrolemember 'sql_dependency_subscriber', [PALMERHARVEY\CSDevService]
END

