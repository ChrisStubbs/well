CREATE PROCEDURE  [dbo].[UserStatsGet]
	@UserIdentity varchar(255)
AS
BEGIN 

  DECLARE @StatsTable TABLE
			( 
				ExceptionCount INT,
				AssignedCount INT,
				OutstandingCount INT,
				NotificationsCount INT
			)

INSERT INTO @StatsTable 
  Values (
	(Select count(j.Id) as ExceptionCount 
	  from Job j
	  INNER JOIN [Stop] s on s.Id = j.StopId
	  INNER JOIN RouteHeader rh on rh.Id = s.RouteHeaderId
	  INNER JOIN Branch b on rh.StartDepotCode = b.Id
	  INNER JOIN UserBranch ub on b.Id = ub.BranchId
	  INNER JOIN [User] u on u.Id = ub.UserId	
	  Where j.PerformanceStatusId in (3,4,5)
	  and u.IdentityName = @UserIdentity
	  ),
	  (
		select count(uj.Id) as AssignedCount 
		from UserJob uj
		inner join [User] u on uj.UserId = u.Id
		where u.IdentityName = @UserIdentity
		),
	(
	  Select count(j.Id) as OutstandingCount
		from Job j
		INNER JOIN [Stop] s on s.Id = j.StopId
		INNER JOIN RouteHeader rh on rh.Id = s.RouteHeaderId
		INNER JOIN Branch b on rh.StartDepotCode = b.Id
		INNER JOIN UserBranch ub on b.Id = ub.BranchId
		INNER JOIN [User] u on u.Id = ub.UserId  
		Where j.PerformanceStatusId in (3,4,5)
		And s.DeliveryDate < DATEADD(DAY,-1,GETDATE())
		and u.IdentityName = @UserIdentity
	),
	(
		SELECT Count(n.Id) AS NotificationsCount
			FROM [Notification] n
			WHERE IsArchived = 0
	)
	)

	SELECT * FROM @StatsTable   

END