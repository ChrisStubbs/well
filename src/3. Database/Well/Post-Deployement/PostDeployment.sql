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
:r .\EnableAuditing.sql
:r .\Populate-AccountType.sql
:r .\Populate-ActionStatus.sql
:r .\Populate-ByPassReasons.sql
:r .\Populate-JobDetailReason.sql
:r .\Populate-JobDetailSource.sql
:r .\Populate-PerformanceStatus.sql
:r .\Populate-ExceptionType.sql
:r .\Populate-ActivityType.sql
:r .\Populate-JobType.sql
:r .\Populate-ReasonCategory.sql
:r .\Populate-RoutePerformanceStatus.sql
:r .\Populate-VisitTypes.sql
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
:r .\Populate-GrnRefused.sql
:r .\Populate-CreditThresholds.sql
:r .\Populate-CommodityType.sql
:r .\Populate-WidgetType.sql
:r .\Populate-DeliveryAction.sql
:r .\Populate-JobStatus.sql
:r .\Populate-WellStatus.sql
:r .\Populate-CommentReason.sql
:r .\Populate-ResolutionStatus.sql
:r .\Populate-BranchDateThreshold.sql