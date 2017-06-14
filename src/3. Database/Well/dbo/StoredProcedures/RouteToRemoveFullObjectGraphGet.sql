CREATE PROCEDURE [dbo].[RouteToRemoveFullObjectGraphGet]	
	@routeId INT
AS
BEGIN


DECLARE @routeHeaders TABLE (	[RouteHeaderId] INT NOT NULL,
								[RouteId] INT NOT NULL,
								[BranchId] INT NOT NULL	)

INSERT @routeHeaders
--select Id, @routeId, StartDepotCode from RouteHeader where RoutesId = @routeId AND IsDeleted = 0
select Id, @routeId, RouteOwnerId from RouteHeader where RoutesId = @routeId 
	AND DateDeleted IS NULL

DECLARE @stops TABLE ( [StopId] INT NOT NULL,
					   [RouteHeaderId] INT NOT NULL )

INSERT @stops
select s.Id, r.RouteHeaderId from [stop] s join @routeHeaders r on r.RouteHeaderId = s.RouteHeaderId 
	where s.DateDeleted IS NULL

DECLARE @jobs TABLE ( [JobId] INT NOT NULL,
					  [StopId] INT NOT NULL,
					  [RoyaltyCode] VARCHAR(60) NULL )

INSERT @jobs
select j.Id, s.StopId, j.RoyaltyCode from job j join @stops s on s.StopId = j.StopId 
	where j.DateDeleted IS NULL

DECLARE @jobDetails TABLE ( [JobDetailId] INT NOT NULL,
							[JobId] INT NOT NULL,
							[ShortsStatus] INT NOT NULL,
							[DateUpdated] DATETIME NOT NULL)

INSERT @jobDetails 
select jd.Id, j.JobId, jd.ShortsStatus, jd.DateUpdated from JobDetail jd join @jobs j on j.JobId = jd.JobId 
	where jd.DateDeleted IS NULL

DECLARE @jobDamages TABLE ( [JobDamageId] INT NOT NULL,
							[JobDetailId] INT NOT NULL )

INSERT @jobDamages
select jd.Id, j.JobDetailId from JobDetailDamage jd join @jobDetails j on j.JobDetailId = jd.JobDetailId 
	where jd.DateDeleted IS NULL

select * from @routeHeaders
select * from @stops
select * from @jobs
select * from @jobDetails
select * from @jobDamages

END
