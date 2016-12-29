namespace PH.Well.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Transactions;

    using PH.Well.Common.Contracts;
    using PH.Well.Domain;
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
        private readonly ICleanPreferenceRepository cleanPreferenceRepository;
        private readonly ISeasonalDateRepository seasonalDateRepository;

        public CleanDeliveryService(
            ILogger logger,
            IRouteHeaderRepository routeHeaderRepository,
            IStopRepository stopRepository,
            IJobRepository jobRepository,
            IJobDetailRepository jobDetailRepository,
            IRouteToRemoveRepository routeToRemoveRepository,
            ICleanPreferenceRepository cleanPreferenceRepository,
            ISeasonalDateRepository seasonalDateRepository)
        {
            this.logger = logger;
            this.routeHeaderRepository = routeHeaderRepository;
            this.stopRepository = stopRepository;
            this.jobRepository = jobRepository;
            this.jobDetailRepository = jobDetailRepository;
            this.routeToRemoveRepository = routeToRemoveRepository;
            this.cleanPreferenceRepository = cleanPreferenceRepository;
            this.seasonalDateRepository = seasonalDateRepository;
        }

        public void DeleteCleans()
        {
            if (DateTime.Now.DayOfWeek == DayOfWeek.Sunday) return;

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
                            var royaltyException = this.GetCustomerRoyaltyException(job.RoyaltyCode);
                            var cleanPreference =
                                this.cleanPreferenceRepository.GetByBranchId(routeHeader.BranchId);
                            var seasonalDates = this.seasonalDateRepository.GetByBranchId(routeHeader.BranchId);

                            foreach (var detail in job.JobDetails)
                            {
                                if (this.CanDelete(royaltyException, cleanPreference, seasonalDates, detail.DateUpdated)) detail.SetToDelete();
                            }

                            job.SetToDelete();
                        }

                        stop.SetToDelete();
                    }

                    routeHeader.SetToDelete();
                }

                route.SetToDelete();

                try
                {
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
                catch (Exception exception)
                {
                    this.logger.LogError("Error when trying to delete clean route!", exception);
                }
            }
        }

        public bool CanDelete(CustomerRoyaltyException royaltyException, CleanPreference cleanPreference, IEnumerable<SeasonalDate> seasonalDates, DateTime dateUpdated)
        {
            var now = DateTime.Now.Date;

            foreach (var seasonal in seasonalDates)
            {
                if (now >= seasonal.From.Date && now <= seasonal.To.Date) return false;
            }

            // TODO This royalty exception needs BDD
            if (royaltyException != null && royaltyException.ExceptionDays > 0)
            {
                var dateCanBeRemoved = dateUpdated.AddDays(royaltyException.ExceptionDays);

                if (dateCanBeRemoved.Date <= now) return true;
            }

            if (cleanPreference != null && cleanPreference.Days > 0)
            {
                var dateCanBeRemoved = dateUpdated.AddDays(cleanPreference.Days);

                if (dateCanBeRemoved.Date <= now) return true;
            }
            else
            {
                var dateCanBeRemoved = dateUpdated.AddDays(1);

                if (dateCanBeRemoved.Date <= now) return true;
            }

            return false;
        }

        public CustomerRoyaltyException GetCustomerRoyaltyException(string royaltyCode)
        {
            var royaltyParts = royaltyCode.Split(' ');

            int tryParseCode = 0;

            if (int.TryParse(royaltyParts[0], out tryParseCode))
            {
                var royaltyException = this.jobRepository.GetCustomerRoyaltyExceptions().FirstOrDefault(x => x.RoyaltyId == tryParseCode);

                return royaltyException;
            }

            return null;
        }
    }
}