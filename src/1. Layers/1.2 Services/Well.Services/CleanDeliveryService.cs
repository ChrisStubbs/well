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

        //private readonly ICleanPreferenceRepository cleanPreferenceRepository;

        private readonly ISeasonalDateRepository seasonalDateRepository;

        private readonly IAmendmentService amendmentService;

        public CleanDeliveryService(
            ILogger logger,
            IRouteHeaderRepository routeHeaderRepository,
            IStopRepository stopRepository,
            IJobRepository jobRepository,
            IJobDetailRepository jobDetailRepository,
            IRouteToRemoveRepository routeToRemoveRepository,
            //ICleanPreferenceRepository cleanPreferenceRepository,
            ISeasonalDateRepository seasonalDateRepository,
            IAmendmentService amendmentService)
        {
            this.logger = logger;
            this.routeHeaderRepository = routeHeaderRepository;
            this.stopRepository = stopRepository;
            this.jobRepository = jobRepository;
            this.jobDetailRepository = jobDetailRepository;
            this.routeToRemoveRepository = routeToRemoveRepository;
            //this.cleanPreferenceRepository = cleanPreferenceRepository;
            this.seasonalDateRepository = seasonalDateRepository;
            this.amendmentService = amendmentService;
        }

        public void DeleteCleans()
        {
            this.logger.LogDebug("Started cleaning the Well...");

            // Do not run on sunday
            if (DateTime.Now.DayOfWeek == DayOfWeek.Sunday)
            {
                return;
            }


            //get all route ids where not deleted
            var routeIds = this.routeToRemoveRepository.GetRouteIds();

            foreach (var routeId in routeIds)
            {

                var jobIdsForDeletion = new List<int>();

                var route = this.routeToRemoveRepository.GetRouteToRemove(routeId);

                foreach (var routeHeader in route.RouteHeaders)
                {
                    foreach (var stop in routeHeader.Stops)
                    {
                        foreach (var job in stop.Jobs)
                        {
                            var royaltyException = this.GetCustomerRoyaltyException(job.RoyaltyCode);
                            //var cleanPreference = this.cleanPreferenceRepository.GetByBranchId(routeHeader.BranchId);
                            var seasonalDates = this.seasonalDateRepository.GetByBranchId(routeHeader.BranchId).ToList();

                            foreach (var detail in job.JobDetails)
                            {
                                //if (this.CanDelete(royaltyException, cleanPreference, seasonalDates, detail.DateUpdated))
                                //{
                                //    detail.SetToDelete();
                                //}
                            }

                            job.SetToDelete();
                            //todo save all the ids of the jobs that are to be deleted 
                            if (job.IsDeleted)
                            {
                                jobIdsForDeletion.Add(job.JobId);
                            }
                        }

                        stop.SetToDelete();
                    }

                    routeHeader.SetToDelete();
                }

                route.SetToDelete();

                this.logger.LogDebug($"Start generating amendments for route id {routeId}...");
                // produce amendments for jobs to be deleted for this route
                this.amendmentService.ProcessAmendments(jobIdsForDeletion);

                this.logger.LogDebug($"Finished  generating amendments for route id {routeId}...");

                /*  try
                  {
                      foreach (var routeHeader in route.RouteHeaders)
                      {
                          foreach (var stop in routeHeader.Stops)
                          {
                              using (var transaction = new TransactionScope())
                              {
                                  foreach (var job in stop.Jobs)
                                  {
                                      foreach (var detail in job.JobDetails)
                                      {
                                          if (detail.IsDeleted)
                                          {
                                              this.jobDetailRepository.DeleteJobDetailById(detail.JobDetailId);
                                          }
                                      }

                                      if (job.IsDeleted)
                                      {
                                          this.jobRepository.DeleteJobById(job.JobId);
                                      }
                                  }

                                  if (stop.IsDeleted)
                                  {
                                      this.stopRepository.DeleteStopById(stop.StopId);
                                  }

                                  transaction.Complete();
                              }
                          }

                          if (routeHeader.IsDeleted)
                          {
                              this.routeHeaderRepository.DeleteRouteHeaderById(routeHeader.RouteHeaderId);
                          }
                      }

                      if (route.IsDeleted)
                      {
                          this.routeHeaderRepository.RoutesDeleteById(route.RouteId);
                      }


                  }
                  catch (Exception exception)
                  {
                      this.logger.LogError("Error when trying to delete clean route!", exception);
                  }*/
            }

            this.logger.LogDebug
                ("Finished cleaning the Well...");
        }


        public bool CanDelete(
            CustomerRoyaltyException royaltyException,
            //CleanPreference cleanPreference,
            IEnumerable<SeasonalDate> seasonalDates,
            DateTime dateUpdated)
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

                if (dateCanBeRemoved.Date <= now)
                {
                    return true;
                }
            }

            //if (cleanPreference != null && cleanPreference.Days > 0)
            //{
            //    var dateCanBeRemoved = dateUpdated.AddDays(cleanPreference.Days);

            //    if (dateCanBeRemoved.Date <= now)
            //    {
            //        return true;
            //    }
            //}
            //else
            //{
            //    var dateCanBeRemoved = dateUpdated.AddDays(1);

            //    if (dateCanBeRemoved.Date <= now)
            //    {
            //        return true;
            //    }
            //}

            return false;
        }

        public CustomerRoyaltyException GetCustomerRoyaltyException(string royaltyCode)
        {
            if (!string.IsNullOrWhiteSpace(royaltyCode))
            {

                var royaltyParts = royaltyCode.Split(' ');
                int tryParseCode = 0;

                //if (int.TryParse(royaltyParts[0], out tryParseCode))
                //{
                //    var royaltyException =
                //        this.jobRepository.GetCustomerRoyaltyExceptions()
                //            .FirstOrDefault(x => x.RoyaltyId == tryParseCode);

                //    return royaltyException;
                //}
            }

            return null;
        }
    }
}