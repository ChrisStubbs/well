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
:r .\Populate-PerformanceStatus.sql
:r .\Populate-JobType.sql
:r .\Populate-ReasonCategory.sql
:r .\Populate-RoutePerformanceStatus.sql
:r .\Populate-RouteStatus.sql
:r .\Populate-VisitTypes.sql
:r .\Populate-StopStatus.sql
:r .\Populate-RouteHeaderExceptions.sql
:r .\Populate-Branch.sql
:r .\Populate-ExceptionAction.sql
:r .\Populate-JobDetailStatus.sql
:r .\Populate-HolidayExceptions.sql
:r .\Populate-CustomerRoyalty.sql
:r .\Populate-PDACreditReasons.sql
:r .\Populate-PODCreditReasons.sql
:r .\Populate-PODCreditActions.sql
:r .\Populate-CSFRejection.sql
:r .\Populate-UserRoles.sql
