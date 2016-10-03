namespace PH.Well.Services
{
    using System;
    using System.Linq;

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

        private WellDeleteType JobDetailDeleteType { get; set; }
        private WellDeleteType JobDeleteType { get; set; }
        private WellDeleteType StopDeleteType { get; set; }
        private WellDeleteType RouteHeaderDeleteType { get; set; }
        public DateTime WellClearDate { get; set; }
        public int WellClearMonths { get; set; }

        public CleanDeliveryService(
            ILogger logger, 
            IRouteHeaderRepository routeHeaderRepository,
            IStopRepository stopRepository,
            IJobRepository jobRepository,
            IJobDetailRepository jobDetailRepository)
        {
            this.logger = logger;
            this.routeHeaderRepository = routeHeaderRepository;
            this.stopRepository = stopRepository;
            this.jobRepository = jobRepository;
            this.jobDetailRepository = jobDetailRepository;
        }

        public void GetRouteHeadersForDelete()
        {
            try
            {
                var currentRouteHeaders = this.routeHeaderRepository.GetRouteHeadersForDelete();
                foreach (var routeheader in currentRouteHeaders)
                {
                    SoftDeleteJobDetail(routeheader);
                }

                CheckRouteFilesForDelete();
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }
        }

        private void SoftDeleteJobDetail(RouteHeader routeHeader)
        {
            var stops = this.stopRepository.GetStopByRouteHeaderId(routeHeader.Id);

            foreach (var stop in stops)
            {
                var stopJobs = this.jobRepository.GetByStopId(stop.Id);

                foreach (var job in stopJobs)
                {
                    var jobDetailsForJob = this.jobDetailRepository.GetByJobId(job.Id);

                    var jobRoyaltyCode = GetCustomerRoyaltyCode(job.RoyaltyCode);
                    var noOutstandingJobRoyalty = DoesJobHaveCustomerRoyalty(jobRoyaltyCode, job.DateCreated);
                    JobDetailDeleteType = DeleteType(job.DateCreated);

                    foreach (var jobDetail in jobDetailsForJob)
                    {
                        if (jobDetail.JobDetailStatusId == (int)JobDetailStatus.Res && noOutstandingJobRoyalty)
                            DeleteJobDetail(jobDetail.Id, JobDetailDeleteType);
                    }

                    CheckJobDetailsForJob(job);
                }

                CheckStopsForDelete(stop);
            }

            CheckRouteheaderForDelete(routeHeader);

        }

        private string GetCustomerRoyaltyCode(string jobTextField)
        {
            if (string.IsNullOrWhiteSpace(jobTextField))
                return string.Empty;

            var royaltyArray = jobTextField.Split(' ');
            return royaltyArray[0];
        }

        private bool DoesJobHaveCustomerRoyalty(string royalyCode, DateTime jobCreatedDate)
        {
            if (string.IsNullOrWhiteSpace(royalyCode))
                return true;

            var royaltyExceptions = this.jobRepository.GetCustomerRoyaltyExceptions().FirstOrDefault(x => x.RoyaltyId == int.Parse(royalyCode));
            return CanJobBeDeletedToday(jobCreatedDate, royaltyExceptions);
        }

        private bool CanJobBeDeletedToday(DateTime jobCreatedDate, CustomerRoyaltyException royalException)
        {
            if (royalException == null)
                return true;

            var exceptionDays = royalException.ExceptionDays;
            var currentDays = (WellClearDate - jobCreatedDate).TotalDays;

            var exceptionDaysPassed = currentDays > exceptionDays;
            return exceptionDaysPassed && !IsWeekendOrPublicHoliday();

        }

        private bool IsWeekendOrPublicHoliday()
        {
            if (DateTime.Now.DayOfWeek == DayOfWeek.Friday || DateTime.Now.DayOfWeek == DayOfWeek.Saturday || DateTime.Now.DayOfWeek == DayOfWeek.Sunday)
                return true;

            return routeHeaderRepository.HolidayExceptionGet().Any(x => x.ExceptionDate == WellClearDate);
        }

        private WellDeleteType DeleteType(DateTime compareDate)
        {
            var dateSpan = DateTimeSpan.CompareDates(compareDate, WellClearDate);
            return dateSpan.Months < WellClearMonths ? WellDeleteType.SoftDelete : WellDeleteType.HardDelete;
        }


        private void DeleteJobDetail(int id, WellDeleteType deleteType)
        {
            this.jobDetailRepository.DeleteJobDetailById(id, deleteType);
        }

        private void CheckJobDetailsForJob(Job job)
        {
            var jobDetailList = this.jobDetailRepository.GetByJobId(job.Id);

            this.JobDeleteType = DeleteType(job.DateCreated);

            if (jobDetailList.All(x => x.IsDeleted) || !jobDetailList.Any())
                this.jobRepository.DeleteJobById(job.Id, JobDeleteType);
        }

        private void CheckStopsForDelete(Stop stop)
        {
            var jobs = this.jobRepository.GetByStopId(stop.Id);

            this.StopDeleteType = DeleteType(stop.DateCreated);

            if (!jobs.Any() || jobs.All(x => x.IsDeleted))
                this.stopRepository.DeleteStopById(stop.Id, StopDeleteType);
        }

        private void CheckRouteheaderForDelete(RouteHeader routeHeader)
        {
            var stops = this.stopRepository.GetStopByRouteHeaderId(routeHeader.Id);

            this.RouteHeaderDeleteType = DeleteType(routeHeader.DateCreated);

            if (!stops.Any() || stops.All(x => x.IsDeleted))
                this.routeHeaderRepository.DeleteRouteHeaderById(routeHeader.Id, RouteHeaderDeleteType);
        }

        private void CheckRouteFilesForDelete()
        {
            var routes = this.routeHeaderRepository.GetRoutes();

            foreach (var route in routes)
            {
                var routeheaders = routeHeaderRepository.GetRouteHeadersGetByRoutesId(route.Id);
                this.RouteHeaderDeleteType = DeleteType(route.ImportDate);

                if (!routeheaders.Any() || routeheaders.All(x => x.IsDeleted))
                    this.routeHeaderRepository.RoutesDeleteById(route.Id, RouteHeaderDeleteType);

            }
        }
    }
}