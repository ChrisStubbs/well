namespace PH.Well.TranSend.Services
{
    using Common.Contracts;
    using Contracts;
    using Domain;
    using Repositories.Contracts;

    public class EpodDomainImportService : IEpodDomainImportService
    {
        private readonly IRouteHeaderRepository routeHeaderRepository;
        private readonly IStopRepository stopRepository;
        private readonly IJobRepository jobRepository;
        private readonly IJobDetailRepository jobDetailRepository;
        private readonly ILogger logger;
        public string CurrentUser { get; set; }

        public EpodDomainImportService(IRouteHeaderRepository routeHeaderRepository, ILogger logger, IStopRepository stopRepository,
                                        IJobRepository jobRepository, IJobDetailRepository jobDetailRepository)
        {
            this.routeHeaderRepository = routeHeaderRepository;
            this.logger = logger;
            this.stopRepository = stopRepository;
            this.jobRepository = jobRepository;
            this.jobDetailRepository = jobDetailRepository;
        }


        public Routes GetByFileName(string filename)
        {
            return this.routeHeaderRepository.GetByFilename(filename);
        }

        public Routes CreateOrUpdate(Routes routes)
        {
            routeHeaderRepository.CurrentUser = CurrentUser;
            return this.routeHeaderRepository.CreateOrUpdate(routes);
        }

        public void AddRoutesFile(RouteDeliveries routeDeliveries, int routesId)
        {

            foreach (var routeHeader in routeDeliveries.RouteHeaders)
            {
                routeHeader.RoutesId = routesId;
                var newRouteHeader = this.routeHeaderRepository.RouteHeaderCreateOrUpdate(routeHeader);

                //route header attributes
                foreach (var attribute in routeHeader.EntityAttributes)
                {
                    attribute.AttributeId = newRouteHeader.Id;
                    this.routeHeaderRepository.AddRouteHeaderAttributes(attribute);
                }

                //stops
                AddRouteHeaderStops(routeHeader, newRouteHeader.Id);
            }

        }

        public void AddRouteHeaderStops(RouteHeader routeHeader, int id)
        {
            foreach (var stop in routeHeader.Stops)
            {
                stop.RouteHeaderId = id;
                this.stopRepository.CurrentUser = this.CurrentUser;
                var newStop = this.stopRepository.StopCreateOrUpdate(stop);

                stop.Accounts.StopId = newStop.Id;
                stopRepository.StopAccountCreateOrUpdate(stop.Accounts);

                foreach (var stopAttribute in stop.EntityAttributes)
                {
                    stopAttribute.AttributeId = newStop.Id;
                    this.stopRepository.AddStopAttributes(stopAttribute);
                }
                //jobs
                AddStopJobs(stop, newStop.Id);
            }
        }


        public void AddStopJobs(Stop stop, int newStopId)
        {
            this.jobRepository.CurrentUser = this.CurrentUser;
            foreach (var job in stop.Jobs)
            {
                job.StopId = newStopId;
                var newJob = this.jobRepository.JobCreateOrUpdate(job);

                foreach (var jobAttribute in job.EntityAttributes)
                {
                    jobAttribute.AttributeId = newJob.Id;
                    jobRepository.AddJobAttributes(jobAttribute);
                }

                AddJobJobDetail(job, newJob.Id);
            }
        }

        public void AddJobJobDetail(Job job, int newJobId)
        {
            this.jobDetailRepository.CurrentUser = this.CurrentUser;
            foreach (var jobDetail in job.JobDetails)
            {
                jobDetail.JobId = newJobId;
                var newJobDetail = this.jobDetailRepository.JobDetailCreateOrUpdate(jobDetail);

                foreach (var jobDetailAttribute in jobDetail.EntityAttributes)
                {
                    jobDetailAttribute.AttributeId = newJobDetail.Id;
                    jobDetailRepository.AddJobDetailAttributes(jobDetailAttribute);
                }
            }
        }




    }
}
