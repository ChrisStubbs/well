CREATE PROCEDURE [dbo].[Notifications_Get]
AS
BEGIN

	SET NOCOUNT ON;

SELECT n.[Id]
		  ,n.[JobId]
		  ,n.[Type]
		  ,n.[ErrorMessage]
		  ,j.PHAccount
		  ,j.PickListRef
		  ,j.InvoiceNumber
		  ,a.ContactName
		  ,a.DepotId
		  ,u.Name AS UserName
		--  ,jt.[Description]
	  FROM [dbo].[Notification] n
	  JOIN [dbo].[Job] j ON n.JobId = j.Id
	  JOIN [dbo].[Stop] s on j.StopId = s.Id
	  JOIN [dbo].[Account] a ON j.StopId = a.StopId
	  --JOIN [dbo].JobType jt on j.JobTypeCode = jt.Code
	  LEFT JOIN [dbo].[UserJob] uj ON j.Id = uj.JobId
	  LEFT JOIN [dbo].[User] u ON uj.UserId = u.Id 
	  WHERE IsArchived = 0

END
