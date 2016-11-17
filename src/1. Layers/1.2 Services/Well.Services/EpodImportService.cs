﻿namespace PH.Well.Services
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Transactions;

    using Common.Contracts;
    using Common.Extensions;
    using Domain;
    using Domain.Enums;
    using Domain.ValueObjects;

    using PH.Well.Common;

    using Repositories.Contracts;
    using Well.Services.Contracts;

    public class EpodImportService : IEpodImportService
    {
        private readonly IRouteHeaderRepository routeHeaderRepository;
        private readonly IStopRepository stopRepository;
        private readonly IJobRepository jobRepository;
        private readonly IJobDetailRepository jobDetailRepository;
        private readonly IAccountRepository accountRepository;
        private readonly IJobDetailDamageRepository jobDetailDamageRepository;
        private readonly IEventLogger eventLogger;
        private readonly ILogger logger;
        
        public EpodImportService(IRouteHeaderRepository routeHeaderRepository, ILogger logger,
            IStopRepository stopRepository, IJobRepository jobRepository, IJobDetailRepository jobDetailRepository, 
            IAccountRepository accountRepository, IJobDetailDamageRepository jobDetailDamageRepository,
            IEventLogger eventLogger)
        {
            this.routeHeaderRepository = routeHeaderRepository;
            this.logger = logger;
            this.stopRepository = stopRepository;
            this.jobRepository = jobRepository;
            this.jobDetailRepository = jobDetailRepository;
            this.accountRepository = accountRepository;
            this.jobDetailDamageRepository = jobDetailDamageRepository;
            this.eventLogger = eventLogger;

            this.routeHeaderRepository.CurrentUser = "ePodImport";
            this.stopRepository.CurrentUser = this.routeHeaderRepository.CurrentUser;
            this.jobRepository.CurrentUser = this.routeHeaderRepository.CurrentUser;
            this.jobDetailRepository.CurrentUser = this.routeHeaderRepository.CurrentUser;
            this.accountRepository.CurrentUser = this.routeHeaderRepository.CurrentUser;
            this.jobDetailDamageRepository.CurrentUser = this.routeHeaderRepository.CurrentUser;
        }
        
        public void AddRoutesFile(RouteDelivery routeDelivery, int routeId)
        {
            foreach (var routeHeader in routeDelivery.RouteHeaders)
            {
                routeHeader.RoutesId = routeId;

                routeHeader.StartDepot = string.IsNullOrWhiteSpace(routeHeader.StartDepotCode)
                                        ? (int)Branches.Ndf
                                        : (int)Enum.Parse(typeof(Branches), routeHeader.StartDepotCode, true);

                routeHeader.EpodDepot = string.IsNullOrWhiteSpace(routeHeader.Depot)
                                        ? (int)Branches.Ndf
                                        : (int)Enum.Parse(typeof(Branches), routeHeader.Depot, true);


                this.routeHeaderRepository.RouteHeaderCreateOrUpdate(routeHeader);
          
                AddRouteHeaderStops(routeHeader);
            }

        }

        private void AddRouteHeaderStops(RouteHeader routeHeader)
        {
            foreach (var stop in routeHeader.Stops)
            {
                try
                {
                    using (var transactionScope = new TransactionScope())
                    {
                        stop.RouteHeaderId = routeHeader.Id;
                        stop.RouteHeaderCode = routeHeader.RouteNumber;
                        this.stopRepository.StopCreateOrUpdate(stop);

                        stop.Account.StopId = stop.Id;
                        stopRepository.StopAccountCreateOrUpdate(stop.Account);

                        AddStopJobs(stop);

                        transactionScope.Complete();
                    }
                }
                catch (Exception exception)
                {
                    this.logger.LogError($"Stop has an error on import! Stop Id ({stop.Id})", exception);
                    this.eventLogger.TryWriteToEventLog(
                        EventSource.WellAdamXmlImport,
                        $"Stop has an error on import! Stop Id ({stop.Id})",
                        9853);
                }
            }
        }

        public void AddStopJobs(Stop stop)
        {
            foreach (var job in stop.Jobs)
            {
                job.StopId = stop.Id;
                job.JobTypeCode = job.JobTypeCode.Replace("&", "");
                jobRepository.JobCreateOrUpdate(job);

                AddJobDetail(job);
            }
        }

        public void AddJobDetail(Job job)
        {
            foreach (var jobDetail in job.JobDetails)
            {
                jobDetail.JobId = job.Id;
                
                jobDetail.JobDetailStatusId = (int)JobDetailStatus.UnRes;
                
                jobDetailRepository.Save(jobDetail);

                foreach (var jobDetailDamage in jobDetail.JobDetailDamages)
                {
                    jobDetailDamage.JobDetailId = jobDetail.Id;
                    this.jobDetailDamageRepository.Save(jobDetailDamage);
                }
            }
        }

        public void AddRoutesEpodFile(RouteDelivery routeDelivery, int routesId)
        {
            foreach (var ePodRouteHeader in routeDelivery.RouteHeaders)
            {
                var currentRouteHeader = this.routeHeaderRepository.GetRouteHeaderByTransportOrderReference(ePodRouteHeader.RouteNumber, ePodRouteHeader.RouteDate);

                if (currentRouteHeader != null)
                {
                    ePodRouteHeader.Id = currentRouteHeader.Id;
                    ePodRouteHeader.Depot = ePodRouteHeader.Depot.Replace(">", string.Empty);
                    currentRouteHeader.RouteStatus = ePodRouteHeader.RouteStatus;
                    currentRouteHeader.RoutePerformanceStatusId = ePodRouteHeader.RoutePerformanceStatusId;
                    currentRouteHeader.AuthByPass = currentRouteHeader.AuthByPass + ePodRouteHeader.AuthByPass;
                    currentRouteHeader.NonAuthByPass = currentRouteHeader.NonAuthByPass + ePodRouteHeader.NonAuthByPass;
                    currentRouteHeader.ShortDeliveries = currentRouteHeader.ShortDeliveries + ePodRouteHeader.ShortDeliveries;
                    currentRouteHeader.DamagesRejected = currentRouteHeader.DamagesRejected + ePodRouteHeader.DamagesRejected;
                    currentRouteHeader.DamagesAccepted = currentRouteHeader.DamagesAccepted + ePodRouteHeader.DamagesAccepted;
                    currentRouteHeader.NotRequired = currentRouteHeader.NotRequired + ePodRouteHeader.NotRequired;
                    currentRouteHeader.EpodDepot = string.IsNullOrWhiteSpace(ePodRouteHeader.Depot) ? (int)Branches.Ndf : (int)(Branches)Enum.Parse(typeof(Branches), ePodRouteHeader.Depot, true);
                    currentRouteHeader.StartDepot = int.Parse(currentRouteHeader.StartDepotCode);
                    currentRouteHeader.ActualStopsCompleted = ePodRouteHeader.ActualStopsCompleted;

                    this.routeHeaderRepository.RouteHeaderCreateOrUpdate(currentRouteHeader);
                    
                    AddEpodRouteHeaderStops(ePodRouteHeader);
                }
                else
                {
                    logger.LogError($"No data found for Epod route: {ePodRouteHeader.RouteNumber} on date: {ePodRouteHeader.RouteDate}");
                    throw new Exception($"No data found for Epod route: {ePodRouteHeader.RouteNumber} on date: {ePodRouteHeader.RouteDate}");
                }
            }
        }

        public void AddAdamUpdateFile(RouteUpdates orderUpdates, int routesId)
        {
            foreach (var orderUpdate in orderUpdates.Order)
            {
                var selectedAction = GetOrderUpdateAction(orderUpdate.ActionIndicator);

                var orderTransportRefDetails = orderUpdate.TransportOrderRef.Split(' ');

                if (selectedAction == OrderActionIndicator.InsertOnly)
                {
                    var newOrderDetails = GetByOrderUpdateDetails(orderUpdate);

                    if (newOrderDetails != null)
                    {
                        this.logger.LogDebug(
                            $"Order details not found for transport order ref {orderUpdate.TransportOrderRef}!");
                        this.eventLogger.TryWriteToEventLog(
                            EventSource.WellAdamXmlImport,
                            $"Order details not found for transport order ref {orderUpdate.TransportOrderRef}!",
                            4365);
                    }

                   // AddStopByOrder(orderUpdate, orderTransportRefDetails, 0, true);
                }

                if (selectedAction != OrderActionIndicator.InsertOrUpdate)
                    continue;

                var exsitingOrderStopDetails = GetByOrderUpdateDetails(orderUpdate);

                if (exsitingOrderStopDetails == null)
                {
                    this.logger.LogDebug(
                            $"Order details not found for transport order ref {orderUpdate.TransportOrderRef}!");
                    this.eventLogger.TryWriteToEventLog(
                        EventSource.WellAdamXmlImport,
                        $"Order details not found for transport order ref {orderUpdate.TransportOrderRef}!",
                        4365);
                }
                
                //AddStopByOrder(orderUpdate, orderTransportRefDetails, exsitingOrderStopDetails.Id, false);
            }
        }

        private void AddStopByOrder(Order order, int currentStopId, bool insertOnly)
        {
            //var currentRoute = this.routeHeaderRepository.GetRouteHeaderByTransportOrderReference(transportOrderRef[0],
            //    DateTime.ParseExact(transportOrderRef[3], "dd/MM/yyyy", new DateTimeFormatInfo()));
           
            if (!insertOnly)
            {
                var currentStop = GetByOrderUpdateDetails(order);
                currentStopId = currentStop.Id;
            }

            var stop = new Stop
            {
                Id = currentStopId,
                PlannedStopNumber = order?.PlannedStopNumber,
               // RouteHeaderCode = transportOrderRef[0],
               // RouteHeaderId = currentRoute.Id,
                TransportOrderReference = order?.TransportOrderRef,
               // DropId = transportOrderRef[1],
               // LocationId = transportOrderRef[2],
               // DeliveryDate = DateTime.Parse(transportOrderRef[3]),
                ShellActionIndicator = order?.ShellActionIndicator,
                CustomerShopReference = order?.CustomerShopReference,
                StopStatusCodeId = (int)StopStatus.Notdef,
                StopPerformanceStatusCodeId = (int)PerformanceStatus.Notdef,
                ByPassReasonId = (int)ByPassReasons.Notdef
            };

            this.stopRepository.StopCreateOrUpdate(stop);

            var newStopAccountId = 0;

            AddStopAccountByOrderJob(stop.Id, order.Accounts, newStopAccountId, insertOnly);

            var currentJobId = 0;

            //AddJobByOrderJob(stop.Id, order.OrderJobs, DateTime.Parse(transportOrderRef[3]), currentJobId, insertOnly);
        }

        private void AddStopAccountByOrderJob(int stopId, Account stopAccount, int currentStopAccountId, bool insertOnly)
        {
            if (!insertOnly)
            {
                var currentStopAccount = this.accountRepository.GetAccountGetByAccountCode(stopAccount.Code, stopId);
                currentStopAccountId = currentStopAccount.Id;
            }

            var newStopAccount = new Account
            {
                Id = currentStopAccountId,
                Code = stopAccount.Code,
                AccountTypeCode = stopAccount.AccountTypeCode,
                DepotId = stopAccount.DepotId,
                Name = stopAccount.Name,
                Address1 = stopAccount.Address1,
                Address2 = stopAccount.Address2,
                PostCode = stopAccount.PostCode,
                ContactName = stopAccount.ContactName,
                ContactNumber = stopAccount.ContactNumber,
                ContactNumber2 = stopAccount.ContactNumber2,
                ContactEmailAddress = stopAccount.ContactEmailAddress,
                StopId = stopId
            };

            this.stopRepository.StopAccountCreateOrUpdate(newStopAccount);
        }

        private void AddJobByOrderJob(int stopId, ICollection<OrderJob> orderJobs, DateTime orderDate, int currentJobId, bool insertOnly)
        {
            foreach (var orderJob in orderJobs)
            {
                if (!insertOnly)
                {
                    var currentJob = this.jobRepository.JobGetByRefDetails(orderJob.PhAccount, orderJob.PickListRef, stopId);
                    currentJobId = currentJob.Id;
                }

                var newJob = new Job
                {
                    Id = currentJobId,
                    Sequence = orderJob.Sequence,
                    JobTypeCode = orderJob.JobTypeCode.Replace("&",""),
                    PhAccount = orderJob.PhAccount,
                    PickListRef = orderJob.PickListRef,
                    InvoiceNumber = orderJob.InvoiceNumber,
                    CustomerRef = orderJob.CustomerRef,
                    OrderDate = orderDate,
                    PerformanceStatus = PerformanceStatus.Notdef,
                    ByPassReason = ByPassReasons.Notdef,
                    StopId = stopId
                };

                jobRepository.JobCreateOrUpdate(newJob);

                var currentJobDetailId = 0;
                AddJobDetailByOrderJobDetail(newJob.Id, orderJob.OrderJobDetails, currentJobDetailId, insertOnly);
            }
        }

        private void AddJobDetailByOrderJobDetail(int jobId, ICollection<OrderJobDetail> orderJobDetails,
            int currentJobDetailId, bool insertOnly)
        {
            foreach (OrderJobDetail orderJobDetail in orderJobDetails)
            {
                if (!insertOnly)
                {
                    var currentJobDetail = this.jobDetailRepository.GetByJobLine(jobId, orderJobDetail.LineNumber);
                    currentJobDetailId = currentJobDetail.Id;
                }

                var newJobDetail = new JobDetail
                {
                    Id = currentJobDetailId,
                    LineNumber = orderJobDetail.LineNumber,
                    PhProductCode = orderJobDetail.PhProductCode,
                    ProdDesc = orderJobDetail.ProdDesc,
                    OrderedQty = orderJobDetail.OrderedQty,
                    UnitMeasure = orderJobDetail.UnitMeasure,
                    PhProductType = orderJobDetail.PhProductType,
                    PackSize = orderJobDetail.PackSize,
                    SingleOrOuter = orderJobDetail.SingleOrOuter,
                    SsccBarcode = orderJobDetail.SsccBarcode,
                    SkuGoodsValue = orderJobDetail.SkuGoodsValue,
                    JobId = jobId
                };

                if (!insertOnly)
                {
                    jobDetailRepository.Update(newJobDetail);
                }
                else
                {
                    jobDetailRepository.Save(newJobDetail);
                }
            }
        }

        private static OrderActionIndicator GetOrderUpdateAction(string actionIndicator)
        {
            return string.IsNullOrWhiteSpace(actionIndicator) ? OrderActionIndicator.InsertOrUpdate : StringExtensions.GetValueFromDescription<OrderActionIndicator>(actionIndicator);
        }

        private Stop GetByOrderUpdateDetails(Order order)
        {
            return this.stopRepository.GetByOrderUpdateDetails(order.TransportOrderRef);
        }

        private void AddEpodRouteHeaderStops(RouteHeader routeHeader)
        {
            foreach (var ePodStop in routeHeader.Stops)
            {

                var tranOrderRef = ePodStop.TransportOrderReference.Split(' ');

                ePodStop.RouteHeaderCode = tranOrderRef?[0];
                ePodStop.DropId = tranOrderRef?[1];
                ePodStop.LocationId = tranOrderRef?[2];
                ePodStop.DeliveryDate = DateTime.Parse(tranOrderRef?[3]);


                var currentStop = this.stopRepository.GetByRouteNumberAndDropNumber(ePodStop.RouteHeaderCode, routeHeader.Id, ePodStop.DropId);

                if (currentStop != null)
                {
                    currentStop.StopStatusCodeId = ePodStop.StopStatusCodeId;
                    currentStop.StopPerformanceStatusCodeId = ePodStop.StopPerformanceStatusCodeId;
                    currentStop.ByPassReasonId = ePodStop.ByPassReasonId;
                    currentStop.TransportOrderReference = ePodStop.TransportOrderReference;
                    this.stopRepository.StopCreateOrUpdate(currentStop);
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
            foreach (var ePodjob in stop.Jobs)
            {
                var currentJob = this.jobRepository.GetByAccountPicklistAndStopId(ePodjob.PhAccount, ePodjob.PickListRef, currentStopId);

                if (currentJob != null)
                {
                    currentJob.ByPassReason = ePodjob.ByPassReason;
                    currentJob.PerformanceStatus = ePodjob.PerformanceStatus;
                    currentJob.InvoiceNumber = ePodjob.InvoiceNumber;
                    AddEpodJobJobDetail(ePodjob, currentJob.Id);


                    var damages = new List<JobDetailDamage>();

                    var jobDetails = this.jobDetailRepository.GetByJobId(currentJob.Id).ToList();

                    foreach (var jobDetail in jobDetails)
                    {
                        damages.AddRange(this.jobDetailDamageRepository.GetJobDamagesByJobDetailId(jobDetail.Id));

                        foreach (var damage in damages)
                        {
                            jobDetail.JobDetailDamages.Add(damage);
                        }

                        damages.Clear();

                        currentJob.JobDetails.Add(jobDetail);
                        
                    }

                    jobRepository.JobCreateOrUpdate(currentJob);
                }
                else
                {
                    logger.LogError($"No data found for Epod job account: {ePodjob.PhAccount} on date: {stop.DeliveryDate}");
                    throw new Exception($"No data found for Epod job account: {ePodjob.PhAccount} on date: {stop.DeliveryDate}");
                }
            }
        }
        
        private void AddEpodJobJobDetail(Job job, int currentJobId)
        {
            foreach (var ePodJobDetail in job.JobDetails)
            {
                var currentJobDetail = this.jobDetailRepository.GetByJobLine(currentJobId, ePodJobDetail.LineNumber);

                if (currentJobDetail != null)
                {
                    if (currentJobDetail.JobDetailDamages.Any() == false)  
                    {
                        //Only save if no previous damages to prevent any Well changes being overwritten
                        foreach (var jobDetailDamage in ePodJobDetail.JobDetailDamages)
                        {
                            jobDetailDamage.JobDetailId = currentJobDetail.Id;
                            this.jobDetailDamageRepository.Save(jobDetailDamage);
                        }
                    }

                    if (currentJobDetail.ShortQty == 0)
                    {
                        currentJobDetail.ShortQty = ePodJobDetail.ShortQty;
                    }

                    currentJobDetail.JobDetailStatusId = currentJobDetail.IsClean()
                        ? (int) JobDetailStatus.Res
                        : (int) JobDetailStatus.UnRes;

                    currentJobDetail.JobDetailStatusId = !JobHasAnInvoiceNumber(currentJobId)
                        ? (int) JobDetailStatus.AwtInvNum
                        : currentJobDetail.JobDetailStatusId;

                    this.jobDetailRepository.Update(currentJobDetail);
                }
                else
                {
                    logger.LogError($"No job detail data found for Epod job account: {job.PhAccount} barcode: {ePodJobDetail.PhProductCode}");
                    throw new Exception($"No job detail data found for Epod job account: {job.PhAccount} barcode: {ePodJobDetail.PhProductCode}");
                }
            }
        }
        
        private bool JobHasAnInvoiceNumber(int currentJobId)
        {
            return !string.IsNullOrWhiteSpace(jobRepository.GetById(currentJobId).InvoiceNumber);
        }

        public IEnumerable<RouteAttributeException> GetRouteAttributeException()
        {
            return this.routeHeaderRepository.GetRouteAttributeException();
        }

        public void CopyFileToArchive(string filename, string fileNameWithoutPath, string archiveLocation)
        {

            if (string.IsNullOrWhiteSpace(archiveLocation))
                return;

            var archiveSubFolder = CreateArchiveSubFolder(fileNameWithoutPath);
            var finalArchivePath = Path.Combine(archiveLocation, archiveSubFolder);

            CreateArchiveDirectory(finalArchivePath);

            if (File.Exists(Path.Combine(finalArchivePath, fileNameWithoutPath)))
            {
                File.Delete(Path.Combine(finalArchivePath, fileNameWithoutPath));
            }

            File.Move(filename, Path.Combine(finalArchivePath, fileNameWithoutPath));
        }

        private string CreateArchiveSubFolder(string fileNameWithoutPath)
        {
            return fileNameWithoutPath + DateTime.Now.ToString("yyyyMMdd");
        }

        private void CreateArchiveDirectory(string directory)
        {
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }
    }
}