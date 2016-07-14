﻿namespace PH.Well.TranSend.Services
{
    using System;
    using System.Runtime.CompilerServices;
    using Common.Contracts;
    using Contracts;
    using Domain;
    using Enums;
    using Repositories.Contracts;

    public class EpodDomainImportService : IEpodDomainImportService
    {
        private readonly IRouteHeaderRepository routeHeaderRepository;
        private readonly IStopRepository stopRepository;
        private readonly IJobRepository jobRepository;
        private readonly IJobDetailRepository jobDetailRepository;
        private readonly ILogger logger;
        public string CurrentUser { get; set; }

        public EpodFileType EpodType { get; set; }

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

                if (this.EpodType == EpodFileType.RouteHeader)
                {
                    foreach (var attribute in routeHeader.EntityAttributes)
                    {
                        attribute.AttributeId = newRouteHeader.Id;
                        this.routeHeaderRepository.AddRouteHeaderAttributes(attribute);
                    }
                }
               
                AddRouteHeaderStops(routeHeader, newRouteHeader.Id);
            }

        }

        private void AddRouteHeaderStops(RouteHeader routeHeader, int id)
        {
            foreach (var stop in routeHeader.Stops)
            {
                stop.RouteHeaderId = id;
                this.stopRepository.CurrentUser = this.CurrentUser;
                var newStop = this.stopRepository.StopCreateOrUpdate(stop);

                stop.Accounts.StopId = newStop.Id;
                stopRepository.StopAccountCreateOrUpdate(stop.Accounts);

                if (this.EpodType == EpodFileType.RouteHeader)
                {
                    foreach (var stopAttribute in stop.EntityAttributes)
                    {
                        stopAttribute.AttributeId = newStop.Id;
                        this.stopRepository.AddStopAttributes(stopAttribute);
                    }
                }
                
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

                if (this.EpodType == EpodFileType.RouteHeader)
                {
                    foreach (var jobAttribute in job.EntityAttributes)
                    {
                        jobAttribute.AttributeId = newJob.Id;
                        jobRepository.AddJobAttributes(jobAttribute);
                    }
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

                if (this.EpodType == EpodFileType.RouteHeader)
                {
                    foreach (var jobDetailAttribute in jobDetail.EntityAttributes)
                    {
                        jobDetailAttribute.AttributeId = newJobDetail.Id;
                        jobDetailRepository.AddJobDetailAttributes(jobDetailAttribute);
                    }
                }
            }
        }

        public void AddRoutesEpodFile(RouteDeliveries routeDeliveries, int routesId)
        {
            foreach (var ePodRouteHeader in routeDeliveries.RouteHeaders)
            {
                var currentRouteHeader = this.routeHeaderRepository.GetRouteHeaderByRouteNumberAndDate(ePodRouteHeader.RouteNumber, ePodRouteHeader.RouteDate);

                if (currentRouteHeader != null)
                {
                    ePodRouteHeader.Id = currentRouteHeader.Id;
                    currentRouteHeader.RouteStatusId = ePodRouteHeader.RouteStatusId;
                    currentRouteHeader.RoutePerformanceStatusId = ePodRouteHeader.RoutePerformanceStatusId;
                    currentRouteHeader.AuthByPass = ePodRouteHeader.AuthByPass;
                    currentRouteHeader.NonAuthByPass = ePodRouteHeader.NonAuthByPass;
                    currentRouteHeader.ShortDeliveries = ePodRouteHeader.ShortDeliveries;
                    currentRouteHeader.DamagesRejected = ePodRouteHeader.DamagesRejected;
                    currentRouteHeader.DamagesAccepted = ePodRouteHeader.DamagesAccepted;
                    currentRouteHeader.NotRequired = ePodRouteHeader.NotRequired;
                    currentRouteHeader.Depot = ePodRouteHeader.Depot;

                    currentRouteHeader = this.routeHeaderRepository.RouteHeaderCreateOrUpdate(currentRouteHeader);
                    AddEpodRouteHeaderStops(ePodRouteHeader);
                }
                else
                {
                    logger.LogError($"No data found for Epod route: {ePodRouteHeader.RouteNumber} on date: {ePodRouteHeader.RouteDate}");
                    throw new Exception($"No data found for Epod route: {ePodRouteHeader.RouteNumber} on date: {ePodRouteHeader.RouteDate}");
                }

            }

        }

        private void AddEpodRouteHeaderStops(RouteHeader routeHeader)
        {
            this.stopRepository.CurrentUser = this.CurrentUser;

            foreach (var ePodStop in routeHeader.Stops)
            {
                var currentStop = this.stopRepository.GetByRouteNumberAndDropNumber(ePodStop.RouteHeaderCode, routeHeader.Id, ePodStop.DropId);

                if (currentStop != null)
                {
                    currentStop.StopStatusCodeId = ePodStop.StopStatusCodeId;
                    currentStop.StopPerformanceStatusCodeId = ePodStop.StopPerformanceStatusCodeId;
                    currentStop.ByPassReasonId = ePodStop.ByPassReasonId;
                    currentStop = this.stopRepository.StopCreateOrUpdate(currentStop);
                    AddEpodStopJobs(ePodStop, currentStop.Id);
                }
                else
                {
                    logger.LogError($"No stop data found for Epod route: {routeHeader.RouteNumber} on date: {routeHeader.RouteDate}");
                    throw new Exception($"No stop data found for Epod route: {routeHeader.RouteNumber} on date: {routeHeader.RouteDate}");
                }
            }
        }

        private void AddEpodStopJobs(Stop stop, int currentStopId)
        {
            this.jobRepository.CurrentUser = this.CurrentUser;

            foreach (var ePodjob in stop.Jobs)
            {
                var currentJob = this.jobRepository.GetByAccountPicklistAndStopId(ePodjob.JobRef1, ePodjob.JobRef2, currentStopId);

                if (currentJob != null)
                {
                    currentJob.ByPassReasonId = ePodjob.ByPassReasonId;
                    currentJob.PerformanceStatusId = ePodjob.PerformanceStatusId;
                    currentJob = this.jobRepository.JobCreateOrUpdate(currentJob);
                    AddEpodJobJobDetail(ePodjob, currentJob.Id);
                }
                else
                {
                    logger.LogError($"No data found for Epod job account: {ePodjob.JobRef1} on date: {stop.DeliveryDate}");
                    throw new Exception($"No data found for Epod job account: {ePodjob.JobRef1} on date: {stop.DeliveryDate}");
                }
            }
        }

        private void AddEpodJobJobDetail(Job job, int currentJobId)
        {
            this.jobDetailRepository.CurrentUser = this.CurrentUser;

            foreach (var ePodJobDetail in job.JobDetails)
            {
                var currentJobDetail = this.jobDetailRepository.GetByBarcodeLineNumberAndJobId(ePodJobDetail.LineNumber, ePodJobDetail.BarCode, currentJobId);

                if (currentJobDetail != null)
                {
                    currentJobDetail = this.jobDetailRepository.JobDetailCreateOrUpdate(currentJobDetail);

                    foreach (var jobDetailDamages in ePodJobDetail.JobDetailDamages)
                    {
                        jobDetailDamages.JobDetailId = currentJobDetail.Id;
                        this.jobDetailRepository.JobDetailDamageCreateOrUpdate(jobDetailDamages);
                    }
                }
                else
                {
                    logger.LogError($"No job detail data found for Epod job account: {job.JobRef1} barcode: {ePodJobDetail.BarCode}");
                    throw new Exception($"No job detail data found for Epod job account: {job.JobRef1} barcode: {ePodJobDetail.BarCode}");
                }
            }
        }
    }
}
