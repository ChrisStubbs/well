CREATE PROCEDURE [dbo].[RouteHeader_GetExceptions]

AS
BEGIN

SELECT [NoOfExceptions],[Assigned],[Outstanding],[OnHold],[Notifications] from dbo.SampleExceptions

END