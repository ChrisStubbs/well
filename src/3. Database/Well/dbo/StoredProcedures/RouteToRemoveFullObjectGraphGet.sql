CREATE PROCEDURE [dbo].[RouteToRemoveFullObjectGraphGet]	
	@routeId INT
AS
BEGIN


DECLARE @routeHeaders TABLE (	[RouteHeaderId] INT NOT NULL,
								[RouteId] INT NOT NULL,
								[BranchId] INT NOT NULL	)

INSERT @routeHeaders
select Id, @routeId, StartDepotCode from RouteHeader where RoutesId = @routeId AND IsDeleted = 0

DECLARE @stops TABLE ( [StopId] INT NOT NULL,
					   [RouteHeaderId] INT NOT NULL )

INSERT @stops
select s.Id, r.RouteHeaderId from [stop] s join @routeHeaders r on r.RouteHeaderId = s.RouteHeaderId where s.IsDeleted = 0

DECLARE @jobs TABLE ( [JobId] INT NOT NULL,
					  [StopId] INT NOT NULL )

INSERT @jobs
select j.Id, s.StopId from job j join @stops s on s.StopId = j.StopId where j.IsDeleted = 0

DECLARE @jobDetails TABLE ( [JobDetailId] INT NOT NULL,
							[JobId] INT NOT NULL,
							[JobDetailStatusId] INT NOT NULL )

INSERT @jobDetails 
select jd.Id, j.JobId, jd.JobDetailStatusId from JobDetail jd join @jobs j on j.JobId = jd.JobId where jd.IsDeleted = 0

DECLARE @jobDamages TABLE ( [JobDamageId] INT NOT NULL,
							[JobDetailId] INT NOT NULL )

INSERT @jobDamages
select jd.Id, j.JobDetailId from JobDetailDamage jd join @jobDetails j on j.JobDetailId = jd.JobDetailId where jd.IsDeleted = 0

select * from @routeHeaders
select * from @stops
select * from @jobs
select * from @jobDetails
select * from @jobDamages

END
