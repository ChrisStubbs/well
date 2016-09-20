CREATE PROCEDURE [dbo].[Notifications_Get]
AS
BEGIN

	SET NOCOUNT ON;

	SELECT n.[Id]
		  ,n.[JobId]
		  ,n.[Type]
		  ,n.[Reason]
		  ,j.JobRef1
		  ,j.JobRef2
		  ,j.JobRef3
		  ,a.ContactName
		  ,a.DepotId
		--  ,jt.[Description]
	  FROM [dbo].[Notification] n
	  JOIN [dbo].[Job] j ON n.JobId = j.Id
	  JOIN [dbo].[Stop] s on j.StopId = s.Id
	  JOIN [dbo].[Account] a ON j.StopId = a.StopId
	  --JOIN [dbo].JobType jt on j.JobTypeCode = jt.Code
	  WHERE IsArchived = 0

END
