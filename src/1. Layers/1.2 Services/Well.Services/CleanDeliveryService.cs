namespace PH.Well.Services
{
    using System;
    using System.Linq;
    using System.Transactions;

    using PH.Well.Common.Contracts;
    using PH.Well.Domain;
    using PH.Well.Domain.Enums;
    using PH.Well.Domain.ValueObjects;
    using PH.Well.Repositories.Contracts;
    using PH.Well.Services.Contracts;

    public class CleanDeliveryService : ICleanDeliveryService
    {
        private readonly ILogger logger;

        private readonly IRouteHeaderRepository routeHeaderRepository;

        private readonly IStopRepository stopRepository;

        private readonly IJobRepository jobRepository;

        private readonly IJobDetailRepository jobDetailRepository;

        private readonly IRouteToRemoveRepository routeToRemoveRepository;

        public CleanDeliveryService(
            ILogger logger, 
            IRouteHeaderRepository routeHeaderRepository,
            IStopRepository stopRepository,
            IJobRepository jobRepository,
            IJobDetailRepository jobDetailRepository,
            IRouteToRemoveRepository routeToRemoveRepository)
        {
            this.logger = logger;
            this.routeHeaderRepository = routeHeaderRepository;
            this.stopRepository = stopRepository;
            this.jobRepository = jobRepository;
            this.jobDetailRepository = jobDetailRepository;
            this.routeToRemoveRepository = routeToRemoveRepository;
        }

        public void DeleteCleans()
        {
            var routeIds = this.routeToRemoveRepository.GetRouteIds();

            foreach (var id in routeIds)
            {
                var route = this.routeToRemoveRepository.GetRouteToRemove(id);

                foreach (var routeHeader in route.RouteHeaders)
                {
                    foreach (var stop in routeHeader.Stops)
                    {
                        foreach (var job in stop.Jobs)
                        {
                            foreach (var detail in job.JobDetails)
                            {
                                detail.SetToDelete();
                            }

                            job.SetToDelete();
                        }

                        stop.SetToDelete();
                    }

                    routeHeader.SetToDelete();
                }

                route.SetToDelete();

                using (var transaction = new TransactionScope())
                {
                    foreach (var routeHeader in route.RouteHeaders)
                    {
                        foreach (var stop in routeHeader.Stops)
                        {
                            foreach (var job in stop.Jobs)
                            {
                                foreach (var detail in job.JobDetails)
                                {
                                    if (detail.IsDeleted) this.jobDetailRepository.DeleteJobDetailById(detail.JobDetailId);
                                }

                                if (job.IsDeleted) this.jobRepository.DeleteJobById(job.JobId);
                            }

                            if (stop.IsDeleted) this.stopRepository.DeleteStopById(stop.StopId);
                        }

                        if (routeHeader.IsDeleted) this.routeHeaderRepository.DeleteRouteHeaderById(routeHeader.RouteHeaderId);
                    }

                    if (route.IsDeleted) this.routeHeaderRepository.RoutesDeleteById(route.RouteId);

                    transaction.Complete();
                }
            }
        }

        private void DeleteJobDetail(RouteHeader routeHeader)
        {
            var stops = this.stopRepository.GetStopByRouteHeaderId(routeHeader.Id);

                foreach (var stop in stops)
                {
                    var stopJobs = this.jobRepository.GetByStopId(stop.Id);

                    foreach (var job in stopJobs)
                    {
                        var jobDetailsForJob = this.jobDetailRepository.GetByJobId(job.Id);

                        var jobRoyaltyCode = GetCustomerRoyaltyCode(job.RoyaltyCode);
                        var noOutstandingJobRoyalty = false; // TODO DoesJobHaveCustomerRoyalty(jobRoyaltyCode, job.DateCreated);

                        foreach (var jobDetail in jobDetailsForJob)
                        {
                            if (jobDetail.JobDetailStatusId == (int)JobDetailStatus.Res)
                            {
                                this.jobDetailRepository.DeleteJobDetailById(jobDetail.Id);
                            }
                        }

                        this.DeleteOrphanedJobs(job);
                    }

                    this.DeleteOrphanedStops(stop);
                }

                this.DeleteOrphanedRouteHeaders(routeHeader);
        }

        private string GetCustomerRoyaltyCode(string jobTextField)
        {
            if (string.IsNullOrWhiteSpace(jobTextField))
                return string.Empty;

            var royaltyArray = jobTextField.Split(' ');
            return royaltyArray[0];
        }

        // TODO
        /*private bool DoesJobHaveCustomerRoyalty(string royalyCode, DateTime jobCreatedDate)
        {
            if (string.IsNullOrWhiteSpace(royalyCode))
                return true;

            var royaltyExceptions = this.jobRepository.GetCustomerRoyaltyExceptions().FirstOrDefault(x => x.RoyaltyId == int.Parse(royalyCode));
            return CanJobBeDeletedToday(jobCreatedDate, royaltyExceptions);
        }*/

        private void DeleteOrphanedJobs(Job job)
        {
            var jobDetailList = this.jobDetailRepository.GetByJobId(job.Id).Where(x => !x.IsDeleted);

            if (!jobDetailList.Any())
                this.jobRepository.DeleteJobById(job.Id);
        }

        private void DeleteOrphanedStops(Stop stop)
        {
            var jobs = this.jobRepository.GetByStopId(stop.Id).Where(x => !x.IsDeleted);

            if (!jobs.Any())
                this.stopRepository.DeleteStopById(stop.Id);
        }

        private void DeleteOrphanedRouteHeaders(RouteHeader routeHeader)
        {
            var stops = this.stopRepository.GetStopByRouteHeaderId(routeHeader.Id).Where(x => !x.IsDeleted);

            if (!stops.Any())
                this.routeHeaderRepository.DeleteRouteHeaderById(routeHeader.Id);
        }

        private void DeleteOrphanedRoutes()
        {
            var routes = this.routeHeaderRepository.GetRoutes();

            foreach (var route in routes)
            {
                var routeheaders = routeHeaderRepository.GetRouteHeadersGetByRoutesId(route.Id).Where(x => !x.IsDeleted);

                if (!routeheaders.Any())
                    this.routeHeaderRepository.RoutesDeleteById(route.Id);

            }
        }
    }
}