CREATE PROCEDURE [dbo].[Notifications_Get]
AS
BEGIN

	SET NOCOUNT ON;

SELECT n.[Id]
		  ,n.[JobId]
		  ,n.[Type]
		  ,n.[ErrorMessage]
		  ,n.Account
		  ,n.InvoiceNumber
		  ,n.Branch
		  ,n.AdamErrorNumber
		  ,n.AdamCrossReference
		  ,n.UserName
		  ,n.CreatedDate AS NotificationDate
	  FROM [dbo].[Notification] n
	  WHERE IsArchived = 0

END
