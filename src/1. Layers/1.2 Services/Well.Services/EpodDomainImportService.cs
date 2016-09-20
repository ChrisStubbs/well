namespace PH.Well.Services
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
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
        private readonly IJobDetailDamageRepo jobDetailDamageRepo;
        private readonly ILogger logger;
        public string CurrentUser { get; set; }
        public DateTime WellClearDate { get; set; }
        public int WellClearMonths { get; set; }

        private readonly string assemblyName = "PH.Well.Services";
        private readonly string correctExtension = ".xml";

        public EpodFileType EpodType { get; set; }
        private WellDeleteType JobDetailDeleteType { get; set; }
        private WellDeleteType JobDeleteType { get; set; }
        private WellDeleteType StopDeleteType { get; set; }
        private WellDeleteType RouteHeaderDeleteType { get; set; }

        

        public EpodDomainImportService(IRouteHeaderRepository routeHeaderRepository, ILogger logger,
            IStopRepository stopRepository, IJobRepository jobRepository, IJobDetailRepository jobDetailRepository, 
            IAccountRepository accountRepository, IJobDetailDamageRepo jobDetailDamageRepo)
        {
            this.routeHeaderRepository = routeHeaderRepository;
            this.logger = logger;
            this.stopRepository = stopRepository;
            this.jobRepository = jobRepository;
            this.jobDetailRepository = jobDetailRepository;
            this.accountRepository = accountRepository;
            this.jobDetailDamageRepo = jobDetailDamageRepo;
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
                
                AddStopJobs(stop, newStop.Id);
            }
        }

        public void AddStopJobs(Stop stop, int newStopId)
        {
            this.jobRepository.CurrentUser = this.CurrentUser;
            foreach (var job in stop.Jobs)
            {
                job.StopId = newStopId;
                jobRepository.JobCreateOrUpdate(job);

                AddJobJobDetail(job, job.Id);
            }
        }

        public void AddJobJobDetail(Job job, int newJobId)
        {
            this.jobDetailRepository.CurrentUser = this.CurrentUser;
            jobDetailDamageRepo.CurrentUser = CurrentUser;
            foreach (var jobDetail in job.JobDetails)
            {
                jobDetail.JobId = newJobId;

                var currentStop = this.stopRepository.GetByJobId(newJobId);

                jobDetail.JobDetailStatusId = (int) JobDetailStatus.UnRes;
                
                jobDetailRepository.Save(jobDetail);

                foreach (var jobDetailDamage in jobDetail.JobDetailDamages)
                {
                    jobDetailDamage.JobDetailId = jobDetail.Id;
                    jobDetailDamageRepo.Save(jobDetailDamage);
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
                    currentRouteHeader.ActualStopsCompleted = ePodRouteHeader.ActualStopsCompleted;

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
            var currentRoute = this.routeHeaderRepository.GetRouteHeaderByRouteNumberAndDate(transportOrderRef[0],
                DateTime.ParseExact(transportOrderRef[3], "dd/MM/yyyy", new DateTimeFormatInfo()));
           
            if (!insertOnly)
            {
                var currentStop = GetByOrderUpdateDetails(order);
                currentStopId = currentStop.Id;
            }

            var newStop = new Stop
            {
                Id = currentStopId,
                PlannedStopNumber = order?.PlannedStopNumber,
                RouteHeaderCode = transportOrderRef[0],
                RouteHeaderId = currentRoute.Id,
                TransportOrderRef = order?.TransportOrderRef,
                DropId = transportOrderRef[1],
                LocationId = transportOrderRef[2],
                DeliveryDate = DateTime.Parse(transportOrderRef[3]),
                ShellActionIndicator = order?.ShellActionIndicator,
                CustomerShopReference = order?.CustomerShopReference,
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
                    JobTypeCode = orderJob.JobTypeCode,
                    PhAccount = orderJob.PhAccount,
                    PickListRef = orderJob.PickListRef,
                    InvoiceNumber = orderJob.InvoiceNumber,
                    CustomerRef = orderJob.CustomerRef,
                    OrderDate = orderDate,
                    PerformanceStatus = PerformanceStatus.Notdef,
                    ByPassReason = ByPassReasons.Notdef,
                    StopId = stopId
                };

                this.jobRepository.CurrentUser = this.CurrentUser;
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

                this.jobDetailRepository.CurrentUser = this.CurrentUser;

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
                this.RouteHeaderDeleteType = DeleteType(route.ImportDate);

                if (!routeheaders.Any() || routeheaders.All(x=>x.IsDeleted))
                    this.routeHeaderRepository.RoutesDeleteById(route.Id, RouteHeaderDeleteType);

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
                    var jobDetailsForJob = this.jobDetailRepository.GetJobDetailByJobId(job.Id);

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
            var jobDetailList = this.jobDetailRepository.GetJobDetailByJobId(job.Id);

            this.JobDeleteType = DeleteType(job.DateCreated);

            if (jobDetailList.All(x=>x.IsDeleted) || !jobDetailList.Any())
                this.jobRepository.DeleteJobById(job.Id, JobDeleteType);
        }

        private void CheckStopsForDelete(Stop stop)
        {
            var jobs = this.jobRepository.GetByStopId(stop.Id);

            this.StopDeleteType = DeleteType(stop.DateCreated);

            if (!jobs.Any() || jobs.All(x=>x.IsDeleted))
                this.stopRepository.DeleteStopById(stop.Id, StopDeleteType);
        }

        private void CheckRouteheaderForDelete(RouteHeader routeHeader)
        {
            var stops = this.stopRepository.GetStopByRouteHeaderId(routeHeader.Id);

            this.RouteHeaderDeleteType = DeleteType(routeHeader.DateCreated);

            if (!stops.Any() || stops.All(x=>x.IsDeleted))
                this.routeHeaderRepository.DeleteRouteHeaderById(routeHeader.Id, RouteHeaderDeleteType);
        }


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
                var currentJob = this.jobRepository.GetByAccountPicklistAndStopId(ePodjob.PhAccount, ePodjob.PickListRef, currentStopId);

                if (currentJob != null)
                {
                    currentJob.ByPassReason = ePodjob.ByPassReason;
                    currentJob.PerformanceStatus = ePodjob.PerformanceStatus;
                    currentJob.InvoiceNumber = ePodjob.InvoiceNumber;
                    jobRepository.JobCreateOrUpdate(currentJob);
                    AddEpodJobJobDetail(ePodjob, currentJob.Id);
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
            this.jobDetailRepository.CurrentUser = this.CurrentUser;

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
                            jobDetailDamageRepo.CurrentUser = this.CurrentUser;
                            jobDetailDamageRepo.Save(jobDetailDamage);
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
            var fileTypeIdent = GetFileTypeIdentifier(fileNameWithoutPath);
            var fileType = GetEpodFileType(fileTypeIdent);

            return StringExtensions.GetEnumDescription(fileType) + DateTime.Now.ToString("yyyyMMdd");

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
