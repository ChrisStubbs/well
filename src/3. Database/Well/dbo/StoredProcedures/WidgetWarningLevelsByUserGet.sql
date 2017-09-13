CREATE PROCEDURE [dbo].[WidgetWarningLevelsByUserGet]
	@UserIdentity varchar(255)
AS
BEGIN 

  DECLARE @LevelsTable TABLE
			( 
				ExceptionWarningLevel INT,
				AssignedWarningLevel INT,
				OutstandingWarningLevel INT,
				NotificationsWarningLevel INT
			)

INSERT INTO @LevelsTable 
  Values (
	(
		SELECT TOP 1 w.WarningLevel  AS ExceptionWarningLevel
			FROM Widget w
			INNER JOIN WidgetToBranch wb on w.Id = wb.Widget_Id
			INNER JOIN [User] u on u.IdentityName =  @UserIdentity
			INNER JOIN UserBranch ub on u.Id = ub.UserId 
			INNER JOIN Branch b on b.Id = wb.Branch_Id
			WHERE w.Type = dbo.WidgetType_Exceptions()
			AND  wb.Branch_Id = ub.BranchId
			ORDER BY w.WarningLevel ASC
		),
	(
		SELECT TOP 1 w.WarningLevel  AS AssignedWarningLevel
			FROM Widget w
			INNER JOIN WidgetToBranch wb on w.Id = wb.Widget_Id
			INNER JOIN [User] u on u.IdentityName =  @UserIdentity
			INNER JOIN UserBranch ub on u.Id = ub.UserId 
			INNER JOIN Branch b on b.Id = wb.Branch_Id
			WHERE w.Type = dbo.WidgetType_Assigned()
			AND  wb.Branch_Id = ub.BranchId
			ORDER BY w.WarningLevel ASC
		),
	(
		SELECT TOP 1 w.WarningLevel  AS OutstandingWarningLevel
			FROM Widget w
			INNER JOIN WidgetToBranch wb on w.Id = wb.Widget_Id
			INNER JOIN [User] u on u.IdentityName =  @UserIdentity
			INNER JOIN UserBranch ub on u.Id = ub.UserId 
			INNER JOIN Branch b on b.Id = wb.Branch_Id
			WHERE w.Type = dbo.WidgetType_Outstanding()
			AND  wb.Branch_Id = ub.BranchId
			ORDER BY w.WarningLevel ASC
		),
	(
		SELECT TOP 1 w.WarningLevel  AS NotificationsWarningLevel
			FROM Widget w
			INNER JOIN WidgetToBranch wb on w.Id = wb.Widget_Id
			INNER JOIN [User] u on u.IdentityName =  @UserIdentity
			INNER JOIN UserBranch ub on u.Id = ub.UserId 
			INNER JOIN Branch b on b.Id = wb.Branch_Id
			WHERE w.Type = dbo.WidgetType_Notifications()
			AND  wb.Branch_Id = ub.BranchId
			ORDER BY w.WarningLevel ASC
		)
	)

	SELECT * FROM @LevelsTable   

END
