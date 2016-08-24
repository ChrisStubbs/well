namespace PH.Well.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.InteropServices.WindowsRuntime;
    using Common.Contracts;
    using Common.Extensions;
    using Domain;
    using Domain.Enums;
    using Domain.ValueObjects;
    using Repositories.Contracts;
    using Well.Services.Contracts;

    public class EpodDomainImportService : IEpodDomainImportService
    {
        private readonly IRouteHeaderRepository routeHeaderRepository;
        private readonly IStopRepository stopRepository;
        private readonly IJobRepository jobRepository;
        private readonly IJobDetailRepository jobDetailRepository;
        private readonly IAccountRepository accountRepository;
        private readonly ILogger logger;
        public string CurrentUser { get; set; }

        private readonly string assemblyName = "PH.Well.Services";
        private readonly string correctExtension = ".xml";

        public EpodFileType EpodType { get; set; }

        public EpodDomainImportService(IRouteHeaderRepository routeHeaderRepository, ILogger logger, IStopRepository stopRepository,
                                        IJobRepository jobRepository, IJobDetailRepository jobDetailRepository, IAccountRepository accountRepository)
        {
            this.routeHeaderRepository = routeHeaderRepository;
            this.logger = logger;
            this.stopRepository = stopRepository;
            this.jobRepository = jobRepository;
            this.jobDetailRepository = jobDetailRepository;
            this.accountRepository = accountRepository;
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

                routeHeader.StartDepot = string.IsNullOrWhiteSpace(routeHeader.StartDepotCode)
                                        ? (int)Branches.Ndf
                                        : (int)Enum.Parse(typeof(Branches), routeHeader.StartDepotCode, true);

                routeHeader.EpodDepot = string.IsNullOrWhiteSpace(routeHeader.Depot)
                                        ? (int)Branches.Ndf
                                        : (int)Enum.Parse(typeof(Branches), routeHeader.Depot, true);


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

                var currentStop = this.stopRepository.GetByJobId(newJobId);

                jobDetail.JobDetailStatusId = (int)GetStatusForJobDetail(jobDetail, currentStop.DeliveryDate);

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
                        throw new Exception($"Transport details already exsist insert operation cannot be completed");

                    AddStopByOrder(orderUpdate, orderTransportRefDetails, 0, true);
                }

                if (selectedAction != OrderActionIndicator.InsertOrUpdate) continue;
                var exsitingOrderStopDetails = GetByOrderUpdateDetails(orderUpdate);
                if (exsitingOrderStopDetails == null)
                    throw new Exception($"No transport details found for update operation");

                AddStopByOrder(orderUpdate, orderTransportRefDetails, exsitingOrderStopDetails.Id, false);
            }

        }

        private void AddStopByOrder(Order order, IReadOnlyList<string> transportOrderRef, int currentStopId, bool insertOnly)
        {
            var currentRoute = this.routeHeaderRepository.GetRouteHeaderByRouteNumberAndDate(transportOrderRef[0], DateTime.Parse(transportOrderRef[3]));
           
            if (!insertOnly)
            {
                var currentStop = GetByOrderUpdateDetails(order);
                currentStopId = currentStop.Id;
            }

            var newStop = new Stop
            {
                Id = currentStopId,
                PlannedStopNumber = order?.PlannedStopNumber,
                PlannedArriveTime = order?.PlannedArriveTime,
                PlannedDepartTime = order?.PlannedDepartTime,
                RouteHeaderCode = transportOrderRef[0],
                RouteHeaderId = currentRoute.Id,
                TransportOrderRef = order?.TransportOrderRef,
                DropId = transportOrderRef[1],
                LocationId = transportOrderRef[2],
                DeliveryDate = DateTime.Parse(transportOrderRef[3]),
                SpecialInstructions = order?.SpecialInstructions,
                StartWindow = order?.StartWindow,
                EndWindow = order?.EndWindow,
                TextField1 = order?.TextField1,
                TextField2 = order?.TextField2,
                TextField3 = order?.TextField3,
                TextField4 = order?.TextField4,
                StopStatusCodeId = (int)StopStatus.Notdef,
                StopPerformanceStatusCodeId = (int)PerformanceStatus.Notdef,
                ByPassReasonId = (int)ByPassReasons.Notdef
            };

            

            this.stopRepository.CurrentUser = this.CurrentUser;
            newStop = this.stopRepository.StopCreateOrUpdate(newStop);

            var newStopAccountId = 0;

            AddStopAccountByOrderJob(newStop.Id, order.Accounts, newStopAccountId, insertOnly);

            var currentJobId = 0;

            AddJobByOrderJob(newStop.Id, order.OrderJobs, DateTime.Parse(transportOrderRef[3]), currentJobId, insertOnly);


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
                StartWindow = stopAccount.StartWindow,
                EndWindow = stopAccount.EndWindow,
                Latitude = stopAccount.Latitude,
                Longitude = stopAccount.Longitude,
                IsDropAndDrive = stopAccount.IsDropAndDrive,
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
                    var currentJob = this.jobRepository.JobGetByRefDetails(orderJob.JobRef1, orderJob.JobRef2, stopId);
                    currentJobId = currentJob.Id;
                }

                var newJob = new Job
                {
                    Id = currentJobId,
                    Sequence = orderJob.Sequence,
                    JobTypeCode = orderJob.JobTypeCode,
                    JobRef1 = orderJob.JobRef1,
                    JobRef2 = orderJob.JobRef2,
                    JobRef3 = orderJob.JobRef3,
                    JobRef4 = orderJob.JobRef4,
                    OrderDate = orderDate,
                    Originator = string.Empty,
                    TextField1 = string.Empty,
                    TextField2 = string.Empty,
                    PerformanceStatusId = (int)PerformanceStatus.Notdef,
                    ByPassReasonId = (int)ByPassReasons.Notdef,
                    StopId = stopId
                };

                this.jobRepository.CurrentUser = this.CurrentUser;
                newJob = this.jobRepository.JobCreateOrUpdate(newJob);

                var currentJobDetailId = 0;
                AddJobDetailByOrderJobDetail(newJob.Id, orderJob.OrderJobDetails, currentJobDetailId, insertOnly);

            }
        }

        private void AddJobDetailByOrderJobDetail(int jobId, ICollection<OrderJobDetail> orderJobDetails,
            int currentJobDetailId, bool insertOnly)
        {
            foreach (var orderJobDetail in orderJobDetails)
            {
                if (!insertOnly)
                {
                    var currentJobDetail =
                        this.jobDetailRepository.JobDetailGetByBarcodeAndProdDesc(orderJobDetail.BarCode, jobId) ;

                    currentJobDetailId = currentJobDetail.Id;
                }

                var newJobDetail = new JobDetail
                {
                    Id = currentJobDetailId,
                    LineNumber = orderJobDetail.LineNumber,
                    BarCode = orderJobDetail.BarCode,
                    OriginalDispatchQty = orderJobDetail.OrderedQty,
                    ProdDesc = orderJobDetail.ProdDesc,
                    OrderedQty = orderJobDetail.OrderedQty,
                    ShortQty = 0,
                    SkuWeight = orderJobDetail.SkuWeight,
                    SkuCube = orderJobDetail.SkuCube,
                    UnitMeasure = orderJobDetail.UnitMeasure,
                    TextField1 = orderJobDetail.TextField1,
                    TextField2 = orderJobDetail.TextField2,
                    TextField3 = orderJobDetail.TextField3,
                    TextField4 = orderJobDetail.TextField4,
                    TextField5 = orderJobDetail.TextField5,
                    SkuGoodsValue = orderJobDetail.SkuGoodsValue,
                    JobId = jobId
                
                };

                this.jobDetailRepository.CurrentUser = this.CurrentUser;
                this.jobDetailRepository.JobDetailCreateOrUpdate(newJobDetail);
            }
        }

        private JobDetailStatus GetStatusForJobDetail(JobDetail jobDetail, DateTime currentStopDate)
        {
            var currentDayDifference = ((TimeSpan) (DateTime.Now - currentStopDate)).Days;

            if (currentDayDifference < 1 && jobDetail.JobDetailDamages.Count > 0)
                return JobDetailStatus.UnRes;

            return JobDetailStatus.Res;
            
        }

        public void GetRouteHeadersForDelete(ref string statusmessage)
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

        private void CheckRouteFilesForDelete()
        {
            var routes = this.routeHeaderRepository.GetRoutes();

            foreach (var route in routes)
            {
                var routeheaders = routeHeaderRepository.GetRouteHeadersGetByRoutesId(route.Id);

                if(!routeheaders.Any())
                    this.routeHeaderRepository.RoutesDeleteById(route.Id);

            }
        }

        private void SoftDeleteJobDetail(RouteHeader routeHeader)
        {
            var stops = this.stopRepository.GetStopByRouteHeaderId(routeHeader.Id);

            var royaltyExceptions = this.jobRepository.GetCustomerRoyaltyExceptions();

            var customerRoyaltyExceptions = royaltyExceptions as CustomerRoyaltyException[] ?? royaltyExceptions.ToArray();

            foreach (var stop in stops)
            {
                var stopJobs = this.jobRepository.GetByStopId(stop.Id);

                foreach (var job in stopJobs)
                {
                    var jobDetailsForJob = this.jobDetailRepository.GetByJobId(job.Id);

                    //var jobRoyaltyCode = job.TextField1.Substring(0, 4).TrimEnd();
                  
                    foreach (var jobDetail in jobDetailsForJob)
                    {
                        if (jobDetail.IsDeleted)
                            DeleteJobDetail(jobDetail.Id);
                    }

                    CheckJobDetailsForJob(jobDetailsForJob, job.Id);
                }

                CheckStopsForDelete(stop.Id);
            }

            CheckRouteheaderForDelete(routeHeader.Id);

        }

        private void DeleteJobDetail(int id)
        {
            this.jobDetailRepository.DeleteJobDetailById(id);
        }

        private void CheckJobDetailsForJob(IEnumerable<JobDetail> jobDetails, int jobId)
        {
            var jobDetailList = jobDetails as IList<JobDetail> ?? jobDetails.ToList();

            if (jobDetailList.All(x=>x.IsDeleted) || !jobDetailList.Any())
                this.jobRepository.DeleteJobById(jobId);
        }

        private void CheckStopsForDelete(int stopId)
        {
            var jobs = this.jobRepository.GetByStopId(stopId);
            
            if(!jobs.Any())
                this.stopRepository.DeleteStopById(stopId);
        }

        private void CheckRouteheaderForDelete(int routeHeaderId)
        {
            var stops = this.stopRepository.GetStopByRouteHeaderId(routeHeaderId);

            if (!stops.Any())
                this.routeHeaderRepository.DeleteRouteHeaderById(routeHeaderId);
        }

        /// <summary>
        /// Place holder ToDo when we figure out the rules concerning the royal exceptions
        /// </summary>
        /// <param name="royalException"></param>
        /// <returns></returns>
        //private bool CanJobBeDeletedToday(CustomerRoyaltyException royalException)
        //{

        //}

        private static OrderActionIndicator GetOrderUpdateAction(string actionIndicator)
        {
            return string.IsNullOrWhiteSpace(actionIndicator) ? OrderActionIndicator.InsertOrUpdate : StringExtensions.GetValueFromDescription<OrderActionIndicator>(actionIndicator);
        }


        private Stop GetByOrderUpdateDetails(Order order)
        {

            var orderTransportRefDetails = order?.TransportOrderRef.Split(' ');

            var routeHeaderCode = orderTransportRefDetails?[0];
            var dropId = orderTransportRefDetails?[1];
            var locationId = orderTransportRefDetails?[2];
            var deliveryDate = DateTime.Parse(orderTransportRefDetails?[3]);

            return this.stopRepository.GetByOrderUpdateDetails(routeHeaderCode, dropId, locationId, deliveryDate);
        }

        private void AddEpodRouteHeaderStops(RouteHeader routeHeader)
        {
            this.stopRepository.CurrentUser = this.CurrentUser;

            foreach (var ePodStop in routeHeader.Stops)
            {

                var tranOrderRef = ePodStop.TransportOrderRef.Split(' ');

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
                    currentStop.TransportOrderRef = ePodStop.TransportOrderRef;
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
                    currentJobDetail.JobDetailStatusId = currentJobDetail.JobDetailDamages.Any()
                        ? (int) JobDetailStatus.UnRes
                        : (int) JobDetailStatus.Res;

                    currentJobDetail = this.jobDetailRepository.JobDetailCreateOrUpdate(currentJobDetail);

                    foreach (var jobDetailDamage in ePodJobDetail.JobDetailDamages)
                    {
                        jobDetailDamage.JobDetailId = currentJobDetail.Id;
                        jobDetailDamage.Reason = jobDetailDamage.Reason ?? DamageReasons.Notdef;
                        this.jobDetailRepository.CreateOrUpdateJobDetailDamage(jobDetailDamage);
                    }
                }
                else
                {
                    logger.LogError($"No job detail data found for Epod job account: {job.JobRef1} barcode: {ePodJobDetail.BarCode}");
                    throw new Exception($"No job detail data found for Epod job account: {job.JobRef1} barcode: {ePodJobDetail.BarCode}");
                }
            }
        }

        public bool IsFileXmlType(string fileName)
        {
            return Path.GetExtension(fileName) == correctExtension;
        }

        public string MatchFileNameToSchema(string fileTypeIndentifier)
        {
            var fileType = GetEpodFileType(fileTypeIndentifier);

            var schemaType = TransendSchemaType.RouteHeaderSchema;

            switch (fileType)
            {
                case EpodFileType.RouteHeader:
                    schemaType = TransendSchemaType.RouteHeaderSchema;
                    break;
                case EpodFileType.RouteEpod:
                    schemaType = TransendSchemaType.RouteEpodSchema;
                    break;
                case EpodFileType.OrderUpdate:
                    schemaType = TransendSchemaType.RouteUpdateSchema;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(fileType));
            }

            return StringExtensions.GetEnumDescription(schemaType);
        }

        public string GetFileTypeIdentifier(string filename)
        {
            var position = filename.IndexOf("_", StringComparison.Ordinal);
            return filename.Substring(0, position + 1);
        }

        public EpodFileType GetEpodFileType(string fileTypeIndentifier)
        {
            return StringExtensions.GetValueFromDescription<EpodFileType>(fileTypeIndentifier);
        }

        public string GetSchemaFilePath(string schemaName)
        {
            var bundleAssembly = AppDomain.CurrentDomain.GetAssemblies().First(x => x.FullName.Contains(assemblyName));
            var asmPath = new Uri(bundleAssembly.CodeBase).LocalPath;
            return Path.Combine(Path.GetDirectoryName(asmPath), "Schemas", schemaName);
        }

        public IEnumerable<RouteAttributeException> GetRouteAttributeException()
        {
            return this.routeHeaderRepository.GetRouteAttributeException();
        }

        public void CopyFileToArchive(string filename, string fileNameWithoutPath, string archiveLocation)
        {
            if (File.Exists(Path.Combine(archiveLocation, fileNameWithoutPath)))
            {
                File.Delete(Path.Combine(archiveLocation, fileNameWithoutPath));
            }

            File.Move(filename, Path.Combine(archiveLocation, fileNameWithoutPath));
        }
    }
}
