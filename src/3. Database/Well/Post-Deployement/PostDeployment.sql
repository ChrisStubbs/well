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
:r .\Populate-AccountType.sql
:r .\Populate-ByPassReasons.sql
:r .\Populate-DamageReasons.sql
:r .\Populate-JobPerformanceStatus.sql
:r .\Populate-JobType.sql
:r .\Populate-ReasonCategory.sql
:r .\Populate-RoutePerformanceStatus.sql
:r .\Populate-RouteStatus.sql
:r .\Populate-VisitTypes.sql
:r .\EventServiceBroker.sql
