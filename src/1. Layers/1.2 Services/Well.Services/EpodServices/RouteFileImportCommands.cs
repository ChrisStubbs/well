namespace PH.Well.Services.EpodServices
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Contracts;
    using Domain;
    using Domain.Extensions;
    using Repositories;
    using Repositories.Contracts;

    public class RouteFileImportCommands : IRouteFileImportCommands
    {
        private readonly IJobRepository jobRepository;
        private readonly IPostImportRepository postImportRepository;
        private readonly IStopRepository stopRepository;

        public RouteFileImportCommands(
            IJobRepository jobRepository,
            IPostImportRepository postImportRepository,
            IStopRepository stopRepository)
        {
            this.jobRepository = jobRepository;
            this.postImportRepository = postImportRepository;
            this.stopRepository = stopRepository;
        }
      
        public void UpdateExistingJob(Job fileJob, Job existingJob, RouteHeader routeHeader)
        {
            jobRepository.Update(existingJob);
        }

        public void PostJobImport(IList<int> jobIds)
        {
            this.postImportRepository.PostImportUpdate(jobIds);
        }

        public void DeleteStopsNotInFile(IEnumerable<Stop> existingRouteStopsFromDb, List<StopDTO> stops)
        {
            IEnumerable<Stop> stopsToBeDeleted = GetStopsToBeDeleted(existingRouteStopsFromDb, stops);

            foreach (var stopToBeDeleted in stopsToBeDeleted)
            {
                if (!stopToBeDeleted.HasStopBeenCompleted())
                {
                    var stopJobs = jobRepository.GetByStopId(stopToBeDeleted.Id);
                    if (stopJobs.All(x=> x.CanWeUpdateJobOnImport()))
                    {
                        stopToBeDeleted.DateDeleted = DateTime.Now;
                        stopToBeDeleted.DeletedByImport = true;
                        stopRepository.Update(stopToBeDeleted);
                    }
                }
            }
        }

        private IEnumerable<Stop> GetStopsToBeDeleted(IEnumerable<Stop> existRouteStops, List<StopDTO> fileStops)
        {
            var fileTransportOrderRef = fileStops
                .Select(s => s.TransportOrderReference)
                .Distinct()
                .ToDictionary(k => k, v => v, StringComparer.OrdinalIgnoreCase);

            return existRouteStops
                .Where(x => !fileTransportOrderRef.ContainsKey(x.TransportOrderReference));
        }
    }
}